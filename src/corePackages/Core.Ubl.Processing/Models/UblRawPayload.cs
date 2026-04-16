namespace Core.Ubl.Processing.Models;

/// <summary>
/// Raw XML payload extracted from a ZIP or from an envelope entry,
/// paired with its canonical UBL document kind.
/// </summary>
public sealed record UblRawPayload(UblDocumentKind Kind, string FileName, byte[] Xml);
