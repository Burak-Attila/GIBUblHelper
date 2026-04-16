using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Parses a raw UBL XML payload of a specific <see cref="UblDocumentKind"/>
/// into either a lightweight summary or the full typed object.
/// </summary>
public interface IUblDocumentParser
{
    UblDocumentKind Kind { get; }

    UblDocumentSummary Parse(byte[] xml);

    UblDocument ParseFull(byte[] xml);
}
