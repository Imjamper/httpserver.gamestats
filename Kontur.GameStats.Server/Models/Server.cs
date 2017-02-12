using GL.HttpServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Kontur.GameStats.Server.Models
{
    /// <summary>
    /// Информация о сервере
    /// </summary>
    public class Server : Entity
    {
        public Server()
        {

        }

        /// <summary>
        /// Адрес сервера
        /// </summary>
        [BsonIndex]
        public string Endpoint { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Игровые моды
        /// </summary>
        public List<string> GameModes { get; set; }
    }
}
