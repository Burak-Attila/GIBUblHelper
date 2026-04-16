using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Peeks at a raw XML payload's root element (without full deserialization)
/// to identify its <see cref="UblDocumentKind"/>.
/// </summary>
public interface IUblRootInspector
{
    UblDocumentKind Identify(byte[] xml);
}
