using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Tr.MainDoc;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Parsing;

public sealed class InvoiceParser : UblParserBase
{
    public InvoiceParser(IUblSerializerCache cache, IOptions<UblProcessingOptions> options)
        : base(cache, options) { }

    public override UblDocumentKind Kind => UblDocumentKind.Invoice;

    protected override object DeserializeTyped(XmlReader reader) =>
        (InvoiceType)Deserialize(typeof(InvoiceType), reader);

    protected override UblDocumentSummary BuildSummary(object typed)
    {
        var invoice = (InvoiceType)typed;
        return new UblDocumentSummary(
            Kind,
            invoice.ID?.Value,
            invoice.UUID?.Value,
            invoice.IssueDate?.Value,
            invoice.AccountingSupplierParty?.Party?.PartyName?.Name?.Value,
            invoice.AccountingCustomerParty?.Party?.PartyName?.Name?.Value,
            invoice.ProfileID?.Value,
            invoice.CustomizationID?.Value);
    }
}
