using System.Collections.Generic;
using System.Threading.Tasks;
using GL.HttpServer.Cache;
using NUnit.Framework;

namespace GL.HttpServer.UnitTests.Cache
{
    [TestFixture]
    public class MemoryCacheTests
    {
        [Test, Order(1)]
        public void TryGetValue_AddNewItem_ReturnSameData()
        {
            var inputCacheItem = new List<string> { "SomeData" };
            var addTask = MemoryCache.Cache<List<string>>().PutAsync("TestKey", inputCacheItem);
            addTask.Wait();
            List<string> returnCacheItem;
            MemoryCache.Cache<List<string>>().TryGetValue("TestKey", out returnCacheItem);
            Assert.AreEqual(inputCacheItem, returnCacheItem);
        }

        [Test, Order(2)]
        public void Remove_AddNewItem_ReturnNull()
        {
            var inputCacheItem = new List<string> {"SomeData"};
            var addTask = MemoryCache.Cache<List<string>>().PutAsync("TestKey", inputCacheItem);
            addTask.Wait();
            MemoryCache.Cache<List<string>>().Remove("TestKey");
            List<string> returnCacheItem;
            MemoryCache.Cache<List<string>>().TryGetValue("TestKey", out returnCacheItem);
            Assert.AreNotEqual(inputCacheItem, returnCacheItem);
        }

        [Test, Order(3)]
        public void Contains_AddNewItem_ReturnTrue()
        {
            var inputCacheItem = new List<string> { "SomeData" };
            var addTask = MemoryCache.Cache<List<string>>().PutAsync("TestKey", inputCacheItem);
            addTask.Wait();
            Assert.IsTrue(MemoryCache.Cache<List<string>>().Contains("TestKey"));
        }

        [Test, Order(4)]
        public void Clear_AddTwoItems_ReturnZeroCount()
        {
            var inputCacheFirstItem = new List<string> { "SomeData1" };
            var inputCacheSecondItem = new List<string> { "SomeData2" };
            var addTask1 = MemoryCache.Cache<List<string>>().PutAsync("TestKey", inputCacheFirstItem);
            addTask1.Wait();
            var addTask2 = MemoryCache.Cache<List<string>>().PutAsync("TestKey", inputCacheSecondItem);
            addTask2.Wait();
            MemoryCache.Cache<List<string>>().Clear();
            Assert.AreEqual(MemoryCache.Cache<List<string>>().GetAll().Count, 0);
        }

        [Test, Order(5)]
        public void PutAsync_AddItemsInMultiThreads_GetValidPutData()
        {
            var testModel = new Configuration();
            testModel.Prefix = "TestPrefix";
            var tasks = new List<Task>();

            for (int i = 0; i < 100; i++)
            {
                var key = $"TestKey{i}";
                tasks.Add(MemoryCache.Cache<Configuration>().PutAsync(key, testModel));
                tasks.Add(Task.Factory.StartNew(() => MemoryCache.Cache<Configuration>().Get(key)));   
            }

            Task.WaitAll(tasks.ToArray());
            Assert.AreEqual(MemoryCache.Cache<Configuration>().GetAll().Count, 100);
        }
    }
}
