using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using GL.HttpServer.Types;
using LiteDB;

namespace GL.HttpServer.Database
{
    public class LiteDb : LiteDatabase
    {
        private static LiteDb _readWrite;
        private static readonly string ConnectionString = Path.Combine($"{ServerEnviroment.ConnectionString}", "LiteDb.db");

        public LiteDb(string connectionString, BsonMapper mapper = null) : base(connectionString, mapper)
        {
            //Mapper.RegisterType(value => new BsonValue(value.Value.UtcDateTime.ToString("o")), bson => new DateOffset(DateTimeOffset.ParseExact(bson.AsString, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)));
        }

        public LiteDb(Stream stream, BsonMapper mapper = null, string password = null) : base(stream, mapper, password)
        {
            //Mapper.RegisterType(value => new BsonValue(value.Value.UtcDateTime.ToString("o")), bson => new DateOffset(DateTimeOffset.ParseExact(bson.AsString, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind)));
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

        public static LiteDb ReadWrite => _readWrite ?? (_readWrite = ServerEnviroment.InMemoryDatabase ? new LiteDb(new MemoryStream()) : new LiteDb(ConnectionString));
    }
}
