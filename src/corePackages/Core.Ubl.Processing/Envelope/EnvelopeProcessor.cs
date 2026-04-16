using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Core.Ubl.EnvelopeSerialization;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Processing.Parsing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Envelope;

/// <inheritdoc cref="IEnvelopeProcessor"/>
public sealed class EnvelopeProcessor : IEnvelopeProcessor
{
    private readonly IUblRootInspector _inspector;
    private readonly IUblDocumentDispatcher _dispatcher;
    private readonly IUblZipExtractor _zipExtractor;
    private readonly IUblSerializerCache _serializerCache;
    private readonly UblProcessingOptions _options;
    private readonly ILogger<EnvelopeProcessor> _logger;

    public EnvelopeProcessor(
        IUblRootInspector inspector,
        IUblDocumentDispatcher dispatcher,
        IUblZipExtractor zipExtractor,
        IUblSerializerCache serializerCache,
        IOptions<UblProcessingOptions> options,
        ILogger<EnvelopeProcessor> logger)
    {
        _inspector = inspector ?? throw new ArgumentNullException(nameof(inspector));
        _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        _zipExtractor = zipExtractor ?? throw new ArgumentNullException(nameof(zipExtractor));
        _serializerCache = serializerCache ?? throw new ArgumentNullException(nameof(serializerCache));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public UblProcessingResult Process(byte[] payload, string? fileName = null)
    {
        ArgumentNullException.ThrowIfNull(payload);

        if (payload.LongLength > _options.MaxInputSizeBytes)
            throw new InvalidOperationException(
                $"Payload size {payload.LongLength} exceeds MaxInputSizeBytes ({_options.MaxInputSizeBytes}).");

        if (IsZip(payload, fileName))
            return ProcessZip(payload);

        return ProcessXml(payload);
    }

    private UblProcessingResult ProcessZip(byte[] zipBytes)
    {
        var entries = _zipExtractor.ExtractXmlEntries(zipBytes);
        _logger.LogDebug("ZIP extracted with {Count} XML entries.", entries.Count);

        var documents = new List<UblDocumentSummary>(entries.Count);
        EnvelopeSummary? envelopeSummary = null;
        UblDocumentKind rootKind = UblDocumentKind.Unknown;

        foreach (var (entryName, xml) in entries)
        {
            var sub = ProcessXml(xml);
            documents.AddRange(sub.Documents);
            envelopeSummary ??= sub.EnvelopeSummary;
            if (rootKind == UblDocumentKind.Unknown)
                rootKind = sub.RootKind;
        }

        return new UblProcessingResult(rootKind, envelopeSummary, documents);
    }

    private UblProcessingResult ProcessXml(byte[] xml)
    {
        var kind = _inspector.Identify(xml);
        return kind switch
        {
            UblDocumentKind.StandardBusinessDocument => ProcessSbd(xml),
            UblDocumentKind.Package => ProcessPackage(xml),
            UblDocumentKind.Invoice
                or UblDocumentKind.CreditNote
                or UblDocumentKind.DespatchAdvice
                or UblDocumentKind.ReceiptAdvice
                or UblDocumentKind.ApplicationResponse => ProcessSingle(kind, xml),
            _ => new UblProcessingResult(UblDocumentKind.Unknown, null, Array.Empty<UblDocumentSummary>())
        };
    }

    private UblProcessingResult ProcessSbd(byte[] xml)
    {
        var sbdSerializer = _serializerCache.Get<StandardBusinessDocument>();
        StandardBusinessDocument sbd;
        using (var ms = new MemoryStream(xml, writable: false))
        using (var reader = XmlReader.Create(ms, UblParserBase.BuildReaderSettings(_options)))
        {
            sbd = (StandardBusinessDocument?)sbdSerializer.Deserialize(reader)
                ?? throw new InvalidOperationException("StandardBusinessDocument deserialization returned null.");
        }

        var envelopeSummary = sbd.StandardBusinessDocumentHeader is null
            ? null
            : EnvelopeUblHelper.GetSummary(xml);

        var documents = new List<UblDocumentSummary>();
        if (sbd.Any is not null)
        {
            var inner = ProcessXml(Encoding.UTF8.GetBytes(sbd.Any.OuterXml));
            documents.AddRange(inner.Documents);
        }

        return new UblProcessingResult(
            UblDocumentKind.StandardBusinessDocument,
            envelopeSummary,
            documents);
    }

    private UblProcessingResult ProcessPackage(byte[] xml)
    {
        var serializer = _serializerCache.Get<Package>();
        using var ms = new MemoryStream(xml, writable: false);
        using var reader = XmlReader.Create(ms, UblParserBase.BuildReaderSettings(_options));
        var package = (Package?)serializer.Deserialize(reader)
            ?? throw new InvalidOperationException("Package deserialization returned null.");

        var summaries = new List<UblDocumentSummary>();
        var elements = package.Elements;
        if (elements is not null)
        {
            foreach (var element in elements)
            {
                var innerItems = element?.ElementList?.Any;
                if (innerItems is null)
                    continue;

                foreach (var innerXml in innerItems)
                {
                    if (innerXml is null)
                        continue;

                    var innerBytes = Encoding.UTF8.GetBytes(innerXml.OuterXml);
                    summaries.Add(_dispatcher.Dispatch(innerBytes));
                }
            }
        }

        return new UblProcessingResult(UblDocumentKind.Package, null, summaries);
    }

    private UblProcessingResult ProcessSingle(UblDocumentKind kind, byte[] xml)
    {
        var summary = _dispatcher.Dispatch(kind, xml);
        return new UblProcessingResult(kind, null, new[] { summary });
    }

    private static bool IsZip(byte[] payload, string? fileName)
    {
        if (payload.Length >= 4 &&
            payload[0] == 0x50 && payload[1] == 0x4B &&
            (payload[2] == 0x03 || payload[2] == 0x05 || payload[2] == 0x07))
        {
            return true;
        }
        return fileName is not null &&
               fileName.EndsWith(".zip", StringComparison.OrdinalIgnoreCase);
    }
}
