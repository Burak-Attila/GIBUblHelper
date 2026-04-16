using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.DependencyInjection;
using Core.Ubl.Processing.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Ubl.Processing.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddUblProcessing_RegistersFullPipeline()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddUblProcessing();

        using var provider = services.BuildServiceProvider();

        Assert.NotNull(provider.GetRequiredService<IUblSerializerCache>());
        Assert.NotNull(provider.GetRequiredService<IUblRootInspector>());
        Assert.NotNull(provider.GetRequiredService<IUblZipExtractor>());
        Assert.NotNull(provider.GetRequiredService<IUblDocumentDispatcher>());
        Assert.NotNull(provider.GetRequiredService<IEnvelopeProcessor>());

        var parsers = provider.GetServices<IUblDocumentParser>().ToList();
        var kinds = parsers.Select(p => p.Kind).ToHashSet();
        Assert.Contains(UblDocumentKind.Invoice, kinds);
        Assert.Contains(UblDocumentKind.CreditNote, kinds);
        Assert.Contains(UblDocumentKind.DespatchAdvice, kinds);
        Assert.Contains(UblDocumentKind.ReceiptAdvice, kinds);
        Assert.Contains(UblDocumentKind.ApplicationResponse, kinds);
    }

    [Fact]
    public void EnvelopeProcessor_ParsesInvoiceFixture_EndToEnd()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddUblProcessing();
        using var provider = services.BuildServiceProvider();

        var processor = provider.GetRequiredService<IEnvelopeProcessor>();
        var xml = File.ReadAllBytes(Path.Combine("Fixtures", "invoice-sample.xml"));

        var result = processor.Process(xml);

        Assert.Equal(UblDocumentKind.Invoice, result.RootKind);
        Assert.Single(result.Documents);
        Assert.Equal("ABC2024000000001", result.Documents[0].Id);
    }
}
