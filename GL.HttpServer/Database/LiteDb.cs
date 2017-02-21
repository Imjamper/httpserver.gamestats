using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using FileMode = LiteDB.FileMode;
using FileOptions = LiteDB.FileOptions;

namespace GL.HttpServer.Database
{
    public class LiteDb : LiteDatabase
    {
        private static LiteDb _readWrite;
        private static LiteDb _read;
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

        public LiteDb(bool readOnly) : base(new FileDiskService(ConnectionString, new FileOptions {FileMode = FileMode.ReadOnly}), null, null, TimeSpan.FromMilliseconds(600), 10000)
        {
        }

        public static void EnsureDbCreate()
        {
            if (!File.Exists(ConnectionString))
            {
                if (!ReadWrite.CollectionExists("InitCollection"))
                {
                    var initCollection = ReadWrite.GetCollection("InitCollection");
                    var initDocument = new BsonDocument();
                    initDocument["someData"] = "someValue";
                    initCollection.Insert(initDocument);
                    ReadWrite.DropCollection("InitCollection");
                }
            }
        }

        public static LiteDb ReadWrite => _readWrite ?? (_readWrite = new LiteDb(new FileDiskService(ConnectionString, new FileOptions { FileMode = FileMode.Shared }), null, null, TimeSpan.FromMilliseconds(600), 10000));

        public static LiteDb Read => _read ?? (_read = new LiteDb(new FileDiskService(ConnectionString, new FileOptions { FileMode = FileMode.ReadOnly}), null, null, TimeSpan.FromMilliseconds(600), 10000));
    }
}
