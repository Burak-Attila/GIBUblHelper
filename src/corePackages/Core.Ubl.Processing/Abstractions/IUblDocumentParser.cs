using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Parses a raw UBL XML payload of a specific <see cref="UblDocumentKind"/>
/// into a <see cref="UblDocumentSummary"/>.
/// </summary>
public interface IUblDocumentParser
{
    UblDocumentKind Kind { get; }

    UblDocumentSummary Parse(byte[] xml);
}
