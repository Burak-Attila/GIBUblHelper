using Core.Ubl.EnvelopeSerialization;

namespace Core.Ubl.Processing.Models;

/// <summary>
/// Aggregate result for a processed input: either a single UBL document,
/// a Package bundle, or a wrapping StandardBusinessDocument envelope.
/// </summary>
public sealed record UblProcessingResult(
    UblDocumentKind RootKind,
    EnvelopeSummary? EnvelopeSummary,
    IReadOnlyList<UblDocumentSummary> Documents);
