using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Orchestrates the full parse flow for an arbitrary payload:
/// handles StandardBusinessDocument envelopes, e-Fatura Package bundles,
/// ZIP archives, and plain UBL documents.
/// </summary>
public interface IEnvelopeProcessor
{
    UblProcessingResult Process(byte[] payload, string? fileName = null);
}
