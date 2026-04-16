using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Tr.MainDoc;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Parsing;

public sealed class ApplicationResponseParser : UblParserBase
{
    public ApplicationResponseParser(IUblSerializerCache cache, IOptions<UblProcessingOptions> options)
        : base(cache, options) { }

    public override UblDocumentKind Kind => UblDocumentKind.ApplicationResponse;

    protected override UblDocumentSummary ParseCore(XmlReader reader)
    {
        var appResp = (ApplicationResponseType)Deserialize(typeof(ApplicationResponseType), reader);
        return new UblDocumentSummary(
            Kind,
            appResp.ID?.Value,
            appResp.UUID?.Value,
            appResp.IssueDate?.Value,
            appResp.SenderParty?.PartyName?.Name?.Value,
            appResp.ReceiverParty?.PartyName?.Name?.Value,
            appResp.ProfileID?.Value,
            appResp.CustomizationID?.Value);
    }
}
