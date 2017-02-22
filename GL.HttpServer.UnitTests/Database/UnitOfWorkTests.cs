using System;
using System.IO;
using GL.HttpServer.Database;
using GL.HttpServer.UnitTests.Entities;
using NUnit.Framework;

namespace GL.HttpServer.UnitTests.Database
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        [Test]
        public void DeleteAll_AddEntity_ReturnZeroCount()
        {
            ServerEnviroment.ConnectionString = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}", "TestDatabase");
            using (var unit = new UnitOfWork())
            {
                var inputEntity = new TestEntity();
                unit.Collection<TestEntity>().Insert(inputEntity);

            }
        }
    }
}
