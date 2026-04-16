using System.Xml;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Tr.MainDoc;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Parsing;

public sealed class CreditNoteParser : UblParserBase
{
    public CreditNoteParser(IUblSerializerCache cache, IOptions<UblProcessingOptions> options)
        : base(cache, options) { }

    public override UblDocumentKind Kind => UblDocumentKind.CreditNote;

    protected override UblDocumentSummary ParseCore(XmlReader reader)
    {
        var note = (CreditNoteType)Deserialize(typeof(CreditNoteType), reader);
        return new UblDocumentSummary(
            Kind,
            note.ID?.Value,
            note.UUID?.Value,
            note.IssueDate?.Value,
            note.AccountingSupplierParty?.Party?.PartyName?.Name?.Value,
            note.AccountingCustomerParty?.Party?.PartyName?.Name?.Value,
            note.ProfileID?.Value,
            note.CustomizationID?.Value);
    }
}
