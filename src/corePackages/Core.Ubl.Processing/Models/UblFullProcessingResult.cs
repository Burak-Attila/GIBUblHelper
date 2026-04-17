using Core.Ubl.EnvelopeSerialization;

namespace Core.Ubl.Processing.Models;

/// <summary>
/// Full processing result: summaries plus the concrete typed UBL objects
/// (InvoiceType, DespatchAdviceType, ...). Use this when the caller needs
/// the whole document, not just the header summary.
/// </summary>
public sealed record UblFullProcessingResult(
    UblDocumentKind RootKind,
    GibEnvelopeType? EnvelopeType,
    EnvelopeSummary? EnvelopeSummary,
    IReadOnlyList<UblDocument> Documents);
