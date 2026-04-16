using System.Text;
using Core.Ubl.Processing.Dispatching;
using Core.Ubl.Processing.Models;
using Core.Ubl.Processing.Options;
using Microsoft.Extensions.Options;

namespace Core.Ubl.Processing.Tests;

public class UblRootInspectorTests
{
    private static readonly UblRootInspector _inspector =
        new(Options.Create(new UblProcessingOptions()));

    [Theory]
    [InlineData("<Invoice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\"/>", UblDocumentKind.Invoice)]
    [InlineData("<CreditNote xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2\"/>", UblDocumentKind.CreditNote)]
    [InlineData("<DespatchAdvice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:DespatchAdvice-2\"/>", UblDocumentKind.DespatchAdvice)]
    [InlineData("<ReceiptAdvice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:ReceiptAdvice-2\"/>", UblDocumentKind.ReceiptAdvice)]
    [InlineData("<ApplicationResponse xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2\"/>", UblDocumentKind.ApplicationResponse)]
    [InlineData("<Package xmlns=\"http://www.efatura.gov.tr/package-namespace\"/>", UblDocumentKind.Package)]
    [InlineData("<StandardBusinessDocument xmlns=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\"/>", UblDocumentKind.StandardBusinessDocument)]
    [InlineData("<Garbage/>", UblDocumentKind.Unknown)]
    public void Identify_DetectsRootElement(string xml, UblDocumentKind expected)
    {
        var bytes = Encoding.UTF8.GetBytes(xml);
        Assert.Equal(expected, _inspector.Identify(bytes));
    }
}
