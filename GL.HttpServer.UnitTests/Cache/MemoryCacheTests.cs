using System.Collections.Generic;
using System.Threading;
using GL.HttpServer.Cache;
using NUnit.Framework;

namespace GL.HttpServer.UnitTests.Cache
{
    [TestFixture]
    public class MemoryCacheTests
    {
        [Test]
        public void TryGetValue_AddNewItem_ReturnSameData()
        {
            var inputCacheItem = new List<string>();
            inputCacheItem.Add("SomeData");
            MemoryCache.Cache<List<string>>().AddOrUpdate("TestKey", inputCacheItem);
            Thread.Sleep(5000);
            List<string> returnCacheItem;
            MemoryCache.Cache<List<string>>().TryGetValue("TestKey", out returnCacheItem);
            Assert.AreEqual(inputCacheItem, returnCacheItem);
        }
    }
}
