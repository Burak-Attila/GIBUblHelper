using System.IO.Compression;
using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Options;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Zip;

/// <inheritdoc cref="IUblZipExtractor"/>
public sealed class UblZipExtractor : IUblZipExtractor
{
    private readonly UblProcessingOptions _options;

    public UblZipExtractor(IOptions<UblProcessingOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public IReadOnlyList<(string FileName, byte[] Xml)> ExtractXmlEntries(byte[] zipBytes)
    {
        ArgumentNullException.ThrowIfNull(zipBytes);

        if (zipBytes.LongLength > _options.MaxInputSizeBytes)
            throw new InvalidOperationException(
                $"ZIP payload exceeds MaxInputSizeBytes ({_options.MaxInputSizeBytes}).");

        using var ms = new MemoryStream(zipBytes, writable: false);
        using var archive = new ZipArchive(ms, ZipArchiveMode.Read);

        if (archive.Entries.Count > _options.MaxZipEntries)
            throw new InvalidOperationException(
                $"ZIP has {archive.Entries.Count} entries, exceeding MaxZipEntries ({_options.MaxZipEntries}).");

        long totalUncompressed = 0;
        var results = new List<(string FileName, byte[] Xml)>(archive.Entries.Count);

        foreach (var entry in archive.Entries)
        {
            if (string.IsNullOrEmpty(entry.Name))
                continue;
            if (!entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                continue;

            if (entry.Length > _options.MaxUncompressedZipSizeBytes)
                throw new InvalidOperationException(
                    $"Entry '{entry.FullName}' exceeds MaxUncompressedZipSizeBytes.");

            if (entry.CompressedLength > 0)
            {
                var ratio = (double)entry.Length / entry.CompressedLength;
                if (ratio > _options.MaxZipCompressionRatio)
                    throw new InvalidOperationException(
                        $"Entry '{entry.FullName}' compression ratio {ratio:F1} exceeds MaxZipCompressionRatio ({_options.MaxZipCompressionRatio}). Possible zip bomb.");
            }

            totalUncompressed += entry.Length;
            if (totalUncompressed > _options.MaxUncompressedZipSizeBytes)
                throw new InvalidOperationException(
                    $"Aggregate uncompressed size exceeds MaxUncompressedZipSizeBytes ({_options.MaxUncompressedZipSizeBytes}).");

            var bytes = ReadEntry(entry, _options.MaxUncompressedZipSizeBytes);
            results.Add((entry.FullName, bytes));
        }

        return results;
    }

    private static byte[] ReadEntry(ZipArchiveEntry entry, long hardLimit)
    {
        using var entryStream = entry.Open();
        using var target = new MemoryStream(capacity: (int)Math.Min(entry.Length, int.MaxValue));
        var buffer = new byte[81920];
        long total = 0;
        int read;
        while ((read = entryStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            total += read;
            if (total > hardLimit)
                throw new InvalidOperationException(
                    $"Entry '{entry.FullName}' exceeded uncompressed hard limit during read.");
            target.Write(buffer, 0, read);
        }
        return target.ToArray();
    }
}
