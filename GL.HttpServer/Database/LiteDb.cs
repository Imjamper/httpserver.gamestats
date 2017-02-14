using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace GL.HttpServer.Database
{
    public class LiteDb : LiteDatabase
    {
        private static readonly string ConnectionString = $"{ServerEnviroment.ConnectionString}\\LiteDb.db";

        public LiteDb(string connectionString, BsonMapper mapper = null) : base(connectionString, mapper)
        {
            
        }

        public LiteDb(Stream stream, BsonMapper mapper = null, string password = null) : base(stream, mapper, password)
        {
        }

        public LiteDb(IDiskService diskService, BsonMapper mapper = null, string password = null, TimeSpan? timeout = null, int cacheSize = 5000, Logger log = null) : base(diskService, mapper, password, timeout, cacheSize, log)
        {
        }

        public LiteDb() : base(ConnectionString)
        {

        }
    }
}
