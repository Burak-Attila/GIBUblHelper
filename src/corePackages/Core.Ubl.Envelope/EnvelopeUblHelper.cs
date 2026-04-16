
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Core.Ubl.EnvelopeSerialization;

public static class EnvelopeUblHelper
{
    private static readonly XmlSerializer _sbdSerializer =
            new XmlSerializer(typeof(StandardBusinessDocument));

    private static readonly XmlSerializerNamespaces _namespaces = BuildNamespaces();

    private static readonly Encoding _utf8NoBom = new UTF8Encoding(false);

    private static XmlSerializerNamespaces BuildNamespaces()
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("", "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader");
        ns.Add("xsd", "http://www.w3.org/2001/XMLSchema");
        ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        return ns;
    }

    #region Deserialize

    /// <summary>
    /// byte[] XML'den StandardBusinessDocument okur.
    /// </summary>
    public static StandardBusinessDocument Deserialize(byte[] xmlBytes)
    {
        using (var ms = new MemoryStream(xmlBytes))
        using (var reader = XmlReader.Create(ms, ReaderSettings()))
        {
            return (StandardBusinessDocument)_sbdSerializer.Deserialize(reader);
        }
    }

    /// <summary>
    /// string XML'den StandardBusinessDocument okur.
    /// </summary>
    public static StandardBusinessDocument Deserialize(string xml)
    {
        using (var sr = new StringReader(xml))
        using (var reader = XmlReader.Create(sr, ReaderSettings()))
        {
            return (StandardBusinessDocument)_sbdSerializer.Deserialize(reader);
        }
    }

    /// <summary>
    /// Stream'den StandardBusinessDocument okur.
    /// </summary>
    public static StandardBusinessDocument Deserialize(Stream stream)
    {
        using (var reader = XmlReader.Create(stream, ReaderSettings()))
        {
            return (StandardBusinessDocument)_sbdSerializer.Deserialize(reader);
        }
    }

    /// <summary>
    /// Dosyadan StandardBusinessDocument okur.
    /// </summary>
    public static StandardBusinessDocument DeserializeFromFile(string filePath)
    {
        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            return Deserialize(fs);
        }
    }

    #endregion

    #region Serialize

    /// <summary>
    /// StandardBusinessDocument'ı byte[] olarak döner (UTF-8, BOM'suz).
    /// </summary>
    public static byte[] SerializeToBytes(StandardBusinessDocument document)
    {
        using (var ms = new MemoryStream())
        {
            using (var writer = XmlWriter.Create(ms, WriterSettings()))
            {
                _sbdSerializer.Serialize(writer, document, _namespaces);
            }
            return ms.ToArray();
        }
    }

    /// <summary>
    /// StandardBusinessDocument'ı string olarak döner.
    /// </summary>
    public static string SerializeToString(StandardBusinessDocument document)
    {
        var sb = new StringBuilder(4096);
        using (var sw = new StringWriter(sb))
        using (var writer = XmlWriter.Create(sw, WriterSettings()))
        {
            _sbdSerializer.Serialize(writer, document, _namespaces);
        }
        return sb.ToString();
    }

    /// <summary>
    /// StandardBusinessDocument'ı dosyaya yazar.
    /// </summary>
    public static void SerializeToFile(StandardBusinessDocument document, string filePath)
    {
        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (var writer = XmlWriter.Create(fs, WriterSettings()))
        {
            _sbdSerializer.Serialize(writer, document, _namespaces);
        }
    }

    #endregion

    #region Header Helpers

    /// <summary>
    /// Zarf XML'inden sadece header bilgilerini çıkarır (hızlı).
    /// Tüm belgeyi parse etmeden sadece header okur.
    /// </summary>
    public static StandardBusinessDocumentHeader ExtractHeader(byte[] xmlBytes)
    {
        var doc = Deserialize(xmlBytes);
        return doc?.StandardBusinessDocumentHeader;
    }

    /// <summary>
    /// Zarf XML'inden sadece header bilgilerini çıkarır (hızlı).
    /// </summary>
    public static StandardBusinessDocumentHeader ExtractHeader(string xml)
    {
        var doc = Deserialize(xml);
        return doc?.StandardBusinessDocumentHeader;
    }

    /// <summary>
    /// Header'dan gönderici VKN/TCKN bilgisini alır.
    /// </summary>
    public static string GetSenderIdentifier(StandardBusinessDocumentHeader header)
    {
        if (header?.Sender == null || header.Sender.Length == 0)
            return null;
        return header.Sender[0]?.Identifier?.Value;
    }

    /// <summary>
    /// Header'dan alıcı VKN/TCKN bilgisini alır.
    /// </summary>
    public static string GetReceiverIdentifier(StandardBusinessDocumentHeader header)
    {
        if (header?.Receiver == null || header.Receiver.Length == 0)
            return null;
        return header.Receiver[0]?.Identifier?.Value;
    }

    /// <summary>
    /// Header'dan belge tipini alır (SENDERENVELOPE, POSTBOXENVELOPE vb.).
    /// </summary>
    public static string GetDocumentType(StandardBusinessDocumentHeader header)
    {
        return header?.DocumentIdentification?.Type;
    }

    /// <summary>
    /// Header'dan UUID (InstanceIdentifier) alır.
    /// </summary>
    public static string GetInstanceIdentifier(StandardBusinessDocumentHeader header)
    {
        return header?.DocumentIdentification?.InstanceIdentifier;
    }

    /// <summary>
    /// Header'dan oluşturulma tarihini alır.
    /// </summary>
    public static DateTime? GetCreationDate(StandardBusinessDocumentHeader header)
    {
        return header?.DocumentIdentification?.CreationDateAndTime;
    }

    /// <summary>
    /// Gönderici ContactInformation'dan belirtilen tipteki değeri alır.
    /// Örn: GetSenderContactInfo(header, "VKN_TCKN") veya GetSenderContactInfo(header, "UNVAN")
    /// </summary>
    public static string GetSenderContactInfo(StandardBusinessDocumentHeader header, string contactType)
    {
        return FindContactInfo(header?.Sender, contactType);
    }

    /// <summary>
    /// Alıcı ContactInformation'dan belirtilen tipteki değeri alır.
    /// </summary>
    public static string GetReceiverContactInfo(StandardBusinessDocumentHeader header, string contactType)
    {
        return FindContactInfo(header?.Receiver, contactType);
    }

    #endregion

    #region Quick Info (tek satırda bilgi alma)

    /// <summary>
    /// byte[] XML'den tek seferde özet bilgi çıkarır.
    /// </summary>
    public static EnvelopeSummary GetSummary(byte[] xmlBytes)
    {
        var header = ExtractHeader(xmlBytes);
        return BuildSummary(header);
    }

    /// <summary>
    /// string XML'den tek seferde özet bilgi çıkarır.
    /// </summary>
    public static EnvelopeSummary GetSummary(string xml)
    {
        var header = ExtractHeader(xml);
        return BuildSummary(header);
    }

    #endregion

    #region Private Methods

    private static XmlReaderSettings ReaderSettings()
    {
        return new XmlReaderSettings
        {
            IgnoreWhitespace = true,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true
        };
    }

    private static XmlWriterSettings WriterSettings()
    {
        return new XmlWriterSettings
        {
            Encoding = _utf8NoBom,
            Indent = true,
            IndentChars = "  ",
            OmitXmlDeclaration = false
        };
    }

    private static string FindContactInfo(Partner[] partners, string contactType)
    {
        if (partners == null || partners.Length == 0)
            return null;

        var partner = partners[0];
        if (partner?.ContactInformation == null)
            return null;

        foreach (var ci in partner.ContactInformation)
        {
            if (string.Equals(ci.ContactTypeIdentifier, contactType, StringComparison.OrdinalIgnoreCase))
                return ci.Contact;
        }
        return null;
    }

    private static EnvelopeSummary BuildSummary(StandardBusinessDocumentHeader header)
    {
        if (header == null)
            return null;

        return new EnvelopeSummary
        {
            InstanceIdentifier = GetInstanceIdentifier(header),
            DocumentType = GetDocumentType(header),
            CreationDate = header.DocumentIdentification?.CreationDateAndTime,
            HeaderVersion = header.HeaderVersion,
            SenderIdentifier = GetSenderIdentifier(header),
            SenderVknTckn = GetSenderContactInfo(header, "VKN_TCKN"),
            SenderTitle = GetSenderContactInfo(header, "UNVAN"),
            ReceiverIdentifier = GetReceiverIdentifier(header),
            ReceiverVknTckn = GetReceiverContactInfo(header, "VKN_TCKN"),
            ReceiverTitle = GetReceiverContactInfo(header, "UNVAN")
        };
    }

    #endregion
}

/// <summary>
/// Zarf özet bilgisi.
/// </summary>
public class EnvelopeSummary
{
    public string InstanceIdentifier { get; set; }
    public string DocumentType { get; set; }
    public DateTime? CreationDate { get; set; }
    public string HeaderVersion { get; set; }
    public string SenderIdentifier { get; set; }
    public string SenderVknTckn { get; set; }
    public string SenderTitle { get; set; }
    public string ReceiverIdentifier { get; set; }
    public string ReceiverVknTckn { get; set; }
    public string ReceiverTitle { get; set; }

    public override string ToString()
    {
        return $"[{DocumentType}] {InstanceIdentifier} | " +
               $"Gönderen: {SenderTitle} ({SenderVknTckn}) | " +
               $"Alıcı: {ReceiverTitle} ({ReceiverVknTckn}) | " +
               $"Tarih: {CreationDate:yyyy-MM-dd HH:mm:ss}";
    }
}

