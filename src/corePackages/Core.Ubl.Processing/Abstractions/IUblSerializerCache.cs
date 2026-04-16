using System.Xml.Serialization;

namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Thread-safe, per-type <see cref="XmlSerializer"/> cache.
/// <see cref="XmlSerializer"/> instances are expensive to construct and should
/// be shared for the lifetime of the process.
/// </summary>
public interface IUblSerializerCache
{
    XmlSerializer Get(Type type);

    XmlSerializer Get<T>();
}
