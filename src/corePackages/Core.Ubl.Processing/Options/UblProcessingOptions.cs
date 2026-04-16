namespace Core.Ubl.Processing.Options;

/// <summary>
/// Runtime options for UBL processing (size limits, zip-bomb guards).
/// </summary>
public sealed class UblProcessingOptions
{
    public const string SectionName = "UblProcessing";

    /// <summary>Maximum accepted input size in bytes (XML or ZIP). Default 32 MB.</summary>
    public long MaxInputSizeBytes { get; set; } = 32L * 1024 * 1024;

    /// <summary>Maximum accepted uncompressed total size inside a ZIP. Default 128 MB.</summary>
    public long MaxUncompressedZipSizeBytes { get; set; } = 128L * 1024 * 1024;

    /// <summary>Maximum compression ratio before a ZIP is rejected as a zip bomb.</summary>
    public double MaxZipCompressionRatio { get; set; } = 100d;

    /// <summary>Maximum number of entries accepted inside a ZIP.</summary>
    public int MaxZipEntries { get; set; } = 1000;

    /// <summary>If true, the XML reader resolves DTDs / external entities. Default false (safe).</summary>
    public bool AllowDtdProcessing { get; set; }
}
