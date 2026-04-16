using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Orchestrates the full parse flow for an arbitrary payload:
/// handles StandardBusinessDocument envelopes, e-Fatura Package bundles,
/// ZIP archives, and plain UBL documents.
/// </summary>
public interface IEnvelopeProcessor
{
    /// <summary>Returns only header-level summaries.</summary>
    UblProcessingResult Process(byte[] payload, string? fileName = null);

    /// <summary>Returns summaries + the fully-typed UBL objects
    /// (InvoiceType, DespatchAdviceType, ...).</summary>
    UblFullProcessingResult ProcessFull(byte[] payload, string? fileName = null);
}
