using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Tr.MainDoc;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Parsing;

public sealed class DespatchAdviceParser : UblParserBase
{
    public DespatchAdviceParser(IUblSerializerCache cache, IOptions<UblProcessingOptions> options)
        : base(cache, options) { }

    public override UblDocumentKind Kind => UblDocumentKind.DespatchAdvice;

    protected override UblDocumentSummary ParseCore(XmlReader reader)
    {
        var advice = (DespatchAdviceType)Deserialize(typeof(DespatchAdviceType), reader);
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
