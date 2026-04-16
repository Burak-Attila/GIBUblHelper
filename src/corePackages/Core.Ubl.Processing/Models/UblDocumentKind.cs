namespace Core.Ubl.Processing.Models;

/// <summary>
/// Canonical list of payloads this library can parse.
/// </summary>
public enum UblDocumentKind
{
    Unknown = 0,
    StandardBusinessDocument,
    Package,
    Invoice,
    CreditNote,
    DespatchAdvice,
    ReceiptAdvice,
    ApplicationResponse
}
