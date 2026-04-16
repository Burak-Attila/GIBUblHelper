using System.Collections.Concurrent;
using System.Xml.Serialization;
using Core.Ubl.Processing.Abstractions;

namespace Core.Ubl.Processing.Caching;

/// <inheritdoc cref="IUblSerializerCache"/>
public sealed class UblSerializerCache : IUblSerializerCache
{
    private readonly ConcurrentDictionary<Type, XmlSerializer> _cache = new();

    public XmlSerializer Get(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return _cache.GetOrAdd(type, static t => new XmlSerializer(t));
    }

    public XmlSerializer Get<T>() => Get(typeof(T));
}
