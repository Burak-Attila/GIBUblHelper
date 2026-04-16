using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Processing.Parsing;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Dispatching;

/// <inheritdoc cref="IUblRootInspector"/>
public sealed class UblRootInspector : IUblRootInspector
{
    private readonly UblProcessingOptions _options;

    public UblRootInspector(IOptions<UblProcessingOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public UblDocumentKind Identify(byte[] xml)
    {
        ArgumentNullException.ThrowIfNull(xml);
        using var ms = new MemoryStream(xml, writable: false);
        using var reader = XmlReader.Create(ms, UblParserBase.BuildReaderSettings(_options));

        while (reader.Read())
        {
            if (reader.NodeType != XmlNodeType.Element)
                continue;

            return reader.LocalName switch
            {
                "StandardBusinessDocument" => UblDocumentKind.StandardBusinessDocument,
                "Package" => UblDocumentKind.Package,
                "Invoice" => UblDocumentKind.Invoice,
                "CreditNote" => UblDocumentKind.CreditNote,
                "DespatchAdvice" => UblDocumentKind.DespatchAdvice,
                "ReceiptAdvice" => UblDocumentKind.ReceiptAdvice,
                "ApplicationResponse" => UblDocumentKind.ApplicationResponse,
                _ => UblDocumentKind.Unknown
            };
        }

        return UblDocumentKind.Unknown;
    }
}
