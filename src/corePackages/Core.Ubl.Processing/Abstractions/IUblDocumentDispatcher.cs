using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Routes a raw XML payload to the correct <see cref="IUblDocumentParser"/>
/// based on its detected <see cref="UblDocumentKind"/>.
/// </summary>
public interface IUblDocumentDispatcher
{
    UblDocumentSummary Dispatch(byte[] xml);

    UblDocumentSummary Dispatch(UblDocumentKind kind, byte[] xml);
}
