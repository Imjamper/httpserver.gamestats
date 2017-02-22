using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GL.HttpServer.Entities;

namespace GL.HttpServer.UnitTests.Entities
{
    public class TestEntity : Entity
    {
        public TestEntity()
        {
            
        }
        public string TestString { get; set; }
    }
}
