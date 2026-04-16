using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Caching;
using Core.Ubl.Processing.Dispatching;
using Core.Ubl.Processing.Envelope;
using Core.Ubl.Processing.Options;
using Core.Ubl.Processing.Parsing;
using Core.Ubl.Processing.Zip;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.Ubl.Processing.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the UBL processing pipeline, options, and all built-in parsers.
    /// </summary>
    public static IServiceCollection AddUblProcessing(
        this IServiceCollection services,
        Action<UblProcessingOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        var builder = services.AddOptions<UblProcessingOptions>();
        if (configure is not null)
            builder.Configure(configure);

        services.TryAddSingleton<IUblSerializerCache, UblSerializerCache>();
        services.TryAddSingleton<IUblRootInspector, UblRootInspector>();
        services.TryAddSingleton<IUblZipExtractor, UblZipExtractor>();
        services.TryAddSingleton<IUblDocumentDispatcher, UblDocumentDispatcher>();
        services.TryAddSingleton<IEnvelopeProcessor, EnvelopeProcessor>();

        services.AddSingleton<IUblDocumentParser, InvoiceParser>();
        services.AddSingleton<IUblDocumentParser, CreditNoteParser>();
        services.AddSingleton<IUblDocumentParser, DespatchAdviceParser>();
        services.AddSingleton<IUblDocumentParser, ReceiptAdviceParser>();
        services.AddSingleton<IUblDocumentParser, ApplicationResponseParser>();

        return services;
    }

    /// <summary>
    /// Registers the UBL processing pipeline using a pre-built options instance.
    /// If <paramref name="options"/> is null, default values are used.
    /// </summary>
    public static IServiceCollection AddUblProcessing(
        this IServiceCollection services,
        UblProcessingOptions? options)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (options is not null)
            return services.AddUblProcessing(opts =>
            {
                opts.MaxInputSizeBytes = options.MaxInputSizeBytes;
                opts.MaxUncompressedZipSizeBytes = options.MaxUncompressedZipSizeBytes;
                opts.MaxZipCompressionRatio = options.MaxZipCompressionRatio;
                opts.MaxZipEntries = options.MaxZipEntries;
                opts.AllowDtdProcessing = options.AllowDtdProcessing;
            });

        return services.AddUblProcessing();
    }

    /// <summary>
    /// Registers the UBL processing pipeline and binds options from the given configuration section.
    /// </summary>
    public static IServiceCollection AddUblProcessing(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = UblProcessingOptions.SectionName)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddOptions<UblProcessingOptions>().Bind(configuration.GetSection(sectionName));
        return services.AddUblProcessing();
    }
}
