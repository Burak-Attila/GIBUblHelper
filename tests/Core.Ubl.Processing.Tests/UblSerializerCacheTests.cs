using Core.Ubl.Processing.Caching;
using Core.Ubl.Tr.MainDoc;

namespace Core.Ubl.Processing.Tests;

public class UblSerializerCacheTests
{
    [Fact]
    public void Get_ReturnsSameInstance_ForSameType()
    {
        var cache = new UblSerializerCache();

        var first = cache.Get<InvoiceType>();
        var second = cache.Get<InvoiceType>();

        Assert.Same(first, second);
    }

    [Fact]
    public void Get_ReturnsDifferentInstance_ForDifferentTypes()
    {
        var cache = new UblSerializerCache();

        var invoice = cache.Get<InvoiceType>();
        var credit = cache.Get<CreditNoteType>();

        Assert.NotSame(invoice, credit);
    }
}
