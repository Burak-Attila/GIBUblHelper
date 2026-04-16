namespace Core.Ubl.Processing.Abstractions;

/// <summary>
/// Safely extracts XML entries from a ZIP archive while enforcing
/// size, entry count, and compression-ratio limits.
/// </summary>
public interface IUblZipExtractor
{
    /// <summary>
    /// Returns every XML entry in the archive as (fileName, bytes).
    /// </summary>
    IReadOnlyList<(string FileName, byte[] Xml)> ExtractXmlEntries(byte[] zipBytes);
}
