using System;
using System.IO;
using LiteDB;

namespace GL.HttpServer.Database
{
    public class LiteDb : LiteDatabase
    {
        private static LiteDb _readWrite;
        private static readonly string ConnectionString = Path.Combine($"{ServerEnviroment.ConnectionString}", "LiteDb.db");

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

        public static void EnsureDbCreate()
        {
            if (!File.Exists(ConnectionString)) using (new LiteEngine(ConnectionString)) { }
        }

        public static void RefreshDb()
        {
            if (ServerEnviroment.InMemoryDatabase)
            {
                lock (_readWrite)
                {
                    _readWrite.Dispose();
                    _readWrite = new LiteDb(new MemoryStream());
                }
            }
        }

        public static LiteDb ReadWrite => _readWrite ?? (_readWrite = ServerEnviroment.InMemoryDatabase ? new LiteDb(new MemoryStream()) : new LiteDb(ConnectionString));
    }
}
