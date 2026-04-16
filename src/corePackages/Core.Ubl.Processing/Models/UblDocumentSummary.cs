namespace Core.Ubl.Processing.Models;

/// <summary>
/// Lightweight header-level summary extracted from a single UBL document.
/// </summary>
public sealed record UblDocumentSummary(
    UblDocumentKind Kind,
    string? Id,
    string? Uuid,
    DateTime? IssueDate,
    string? SupplierTitle,
    string? CustomerTitle,
    string? ProfileId,
    string? CustomizationId);
