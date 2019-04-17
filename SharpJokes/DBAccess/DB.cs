using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DBAccess
{
    public class DB
    {
        public static void InitializeDatabase()
        {
            using (var db = new SqliteConnection("Filename=SharpJokes.db"))
            {
                db.Open();
                const string createTable =
                    "CREATE TABLE IF NOT EXISTS Favorites ( " +
                        "favorite_id integer PRIMARY KEY, " +
                        "favorite_title nvarchar(255) NOT NULL DEFAULT('untitled'), " +
                        "favorite_text nvarchar(255) " +
                        "favorite_link nvarchar(255) " +
                    ");";
                new SqliteCommand(createTable, db).ExecuteReader();
            }
        }

        public static void AddFavorite(Object favorite)
        {
            using (var db = new SqliteConnection("Filename=SharpJokes.db"))
            {
                db.Open();
                var insertCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = ""
                };
                insertCommand.ExecuteReaderAsync();
            }
        }

        public static void DeleteFavorite(object favorite)
        {
            using (var db = new SqliteConnection("Filename=SharpJokes.db"))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = ""
                };
                deleteCommand.ExecuteReaderAsync();
            }
        }

    }
}
