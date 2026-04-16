namespace Core.Ubl.Processing.Models;

/// <summary>
/// A single UBL business document: its detected kind, the lightweight
/// summary and the concrete typed object (e.g. InvoiceType, DespatchAdviceType).
/// </summary>
public sealed record UblDocument(
    UblDocumentKind Kind,
    UblDocumentSummary Summary,
    object Document);
