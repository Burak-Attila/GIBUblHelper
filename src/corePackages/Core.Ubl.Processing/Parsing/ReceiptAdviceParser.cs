using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Tr.MainDoc;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Parsing;

public sealed class ReceiptAdviceParser : UblParserBase
{
    public ReceiptAdviceParser(IUblSerializerCache cache, IOptions<UblProcessingOptions> options)
        : base(cache, options) { }

    public override UblDocumentKind Kind => UblDocumentKind.ReceiptAdvice;

    protected override object DeserializeTyped(XmlReader reader) =>
        (ReceiptAdviceType)Deserialize(typeof(ReceiptAdviceType), reader);

    protected override UblDocumentSummary BuildSummary(object typed)
    {
        var advice = (ReceiptAdviceType)typed;
        return new UblDocumentSummary(
            Kind,
            advice.ID?.Value,
            advice.UUID?.Value,
            advice.IssueDate?.Value,
            advice.DespatchSupplierParty?.Party?.PartyName?.Name?.Value,
            advice.DeliveryCustomerParty?.Party?.PartyName?.Name?.Value,
            advice.ProfileID?.Value,
            advice.CustomizationID?.Value);
    }
}
