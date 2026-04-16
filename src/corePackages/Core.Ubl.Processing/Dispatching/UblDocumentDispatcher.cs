using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;

namespace Core.Ubl.Processing.Dispatching;

/// <inheritdoc cref="IUblDocumentDispatcher"/>
public sealed class UblDocumentDispatcher : IUblDocumentDispatcher
{
    private readonly IUblRootInspector _inspector;
    private readonly IReadOnlyDictionary<UblDocumentKind, IUblDocumentParser> _parsers;

    public UblDocumentDispatcher(
        IUblRootInspector inspector,
        IEnumerable<IUblDocumentParser> parsers)
    {
        _inspector = inspector ?? throw new ArgumentNullException(nameof(inspector));
        ArgumentNullException.ThrowIfNull(parsers);

        _parsers = parsers
            .GroupBy(p => p.Kind)
            .ToDictionary(g => g.Key, g => g.Last());
    }

    public UblDocumentSummary Dispatch(byte[] xml) =>
        Dispatch(_inspector.Identify(xml), xml);

    public UblDocumentSummary Dispatch(UblDocumentKind kind, byte[] xml) =>
        ResolveParser(kind).Parse(xml);

    public UblDocument DispatchFull(byte[] xml) =>
        DispatchFull(_inspector.Identify(xml), xml);

    public UblDocument DispatchFull(UblDocumentKind kind, byte[] xml) =>
        ResolveParser(kind).ParseFull(xml);

    private IUblDocumentParser ResolveParser(UblDocumentKind kind)
    {
        if (!_parsers.TryGetValue(kind, out var parser))
            throw new NotSupportedException($"No parser registered for UBL document kind '{kind}'.");
        return parser;
    }
}
