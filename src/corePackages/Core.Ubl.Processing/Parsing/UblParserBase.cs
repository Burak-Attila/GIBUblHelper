using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Parsing;

/// <summary>
/// Shared deserialization pipeline for all UBL document parsers.
/// Applies safe <see cref="XmlReaderSettings"/> (no DTD, no external resolver).
/// </summary>
public abstract class UblParserBase : IUblDocumentParser
{
    private readonly IUblSerializerCache _cache;
    private readonly UblProcessingOptions _options;

    protected UblParserBase(IUblSerializerCache cache, IOptions<UblProcessingOptions> options)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public abstract UblDocumentKind Kind { get; }

    public UblDocumentSummary Parse(byte[] xml)
    {
        ArgumentNullException.ThrowIfNull(xml);
        using var ms = new MemoryStream(xml, writable: false);
        using var reader = XmlReader.Create(ms, BuildReaderSettings(_options));
        return ParseCore(reader);
    }

    protected abstract UblDocumentSummary ParseCore(XmlReader reader);

    protected object Deserialize(Type type, XmlReader reader)
    {
        var serializer = _cache.Get(type);
        return serializer.Deserialize(reader)
            ?? throw new InvalidOperationException($"Deserialization returned null for {type.FullName}.");
    }

    internal static XmlReaderSettings BuildReaderSettings(UblProcessingOptions options) => new()
    {
        IgnoreWhitespace = true,
        IgnoreComments = true,
        IgnoreProcessingInstructions = true,
        DtdProcessing = options.AllowDtdProcessing ? DtdProcessing.Parse : DtdProcessing.Prohibit,
        XmlResolver = options.AllowDtdProcessing ? new XmlUrlResolver() : null,
        MaxCharactersFromEntities = 1024,
        CloseInput = false
    };
}
