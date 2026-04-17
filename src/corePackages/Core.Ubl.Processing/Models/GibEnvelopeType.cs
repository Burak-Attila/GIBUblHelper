namespace Core.Ubl.Processing.Models;

/// <summary>
/// GİB UBL-TR zarf tipleri (SBD DocumentIdentification.Type alanından).
/// </summary>
public enum GibEnvelopeType
{
    Unknown = 0,

    /// <summary>Gönderici zarfı — belgeleri GİB'e gönderir.</summary>
    SenderEnvelope,

    /// <summary>Posta kutusu zarfı — GİB'den alıcıya iletilen belgeler.</summary>
    PostboxEnvelope,

    /// <summary>Sistem zarfı — GİB sistem yanıtı (kabul/red/durum).</summary>
    SystemEnvelope,

    /// <summary>Kullanıcı zarfı — kullanıcı hesap işlemleri.</summary>
    UserEnvelope,

    /// <summary>Kullanıcı hesap işleme zarfı.</summary>
    ProcessUserAccount
}

public static class GibEnvelopeTypeParser
{
    public static GibEnvelopeType Parse(string? documentType) =>
        documentType?.Trim().ToUpperInvariant() switch
        {
            "SENDERENVELOPE" => GibEnvelopeType.SenderEnvelope,
            "POSTBOXENVELOPE" => GibEnvelopeType.PostboxEnvelope,
            "SYSTEMENVELOPE" => GibEnvelopeType.SystemEnvelope,
            "USERENVELOPE" => GibEnvelopeType.UserEnvelope,
            "PROCESSUSERACCOUNT" => GibEnvelopeType.ProcessUserAccount,
            _ => GibEnvelopeType.Unknown
        };
}
