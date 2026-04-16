using Core.Ubl.Processing.Caching;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Core.Ubl.Processing.Parsing;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Tests;

public class InvoiceParserTests
{
    [Fact]
    public void Parse_ExtractsHeaderFields_FromSampleFixture()
    {
        var xml = File.ReadAllBytes(Path.Combine("Fixtures", "invoice-sample.xml"));
        var parser = new InvoiceParser(
            new UblSerializerCache(),
            Options.Create(new UblProcessingOptions()));

        var summary = parser.Parse(xml);

        Assert.Equal(UblDocumentKind.Invoice, summary.Kind);
        Assert.Equal("ABC2024000000001", summary.Id);
        Assert.Equal("11111111-2222-3333-4444-555555555555", summary.Uuid);
        Assert.Equal(new DateTime(2024, 5, 1), summary.IssueDate);
        Assert.Equal("ACME Supplier Ltd", summary.SupplierTitle);
        Assert.Equal("ACME Customer Co", summary.CustomerTitle);
        Assert.Equal("TICARIFATURA", summary.ProfileId);
        Assert.Equal("TR1.2", summary.CustomizationId);
    }
}
