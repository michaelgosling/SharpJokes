﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DBAccess
{
    public class DB
    {
        /// <summary>
        /// Initialize the database.
        /// </summary>
        public static void InitializeDatabase()
        {
            using (var db = new SqliteConnection("Filename=SharpJokesNew.db"))
            {
                db.Open();
                const string createTable =
                    "CREATE TABLE IF NOT EXISTS Favorites ( " +
                        "favorite_id nvarchar(255) PRIMARY KEY, " +
                        "favorite_title nvarchar(255), " +
                        "favorite_text nvarchar(255), " +
                        "favorite_link nvarchar(255), " +
                        "favorite_username nvarchar(255) " +
                    ");";
                new SqliteCommand(createTable, db).ExecuteReader();
            }
        }

        /// <summary>
        /// Add a favorite to the database
        /// </summary>
        /// <param name="id">Post ID of the favorite</param>
        /// <param name="title">Title of the favorite</param>
        /// <param name="body">Body of the favorite</param>
        /// <param name="link">Link of the favorite</param>
        /// <param name="username">Username that posted the favorite</param>
        public static void AddFavorite(string id, string title, string body, string link, string username)
        {
            string insertCommandText;
            if (string.IsNullOrEmpty(link))
            {
                insertCommandText = "INSERT INTO Favorites (favorite_id, favorite_title, " +
                    "favorite_text, favorite_username) " +
                    "VALUES (\"" + id + "\", \"" + title + "\", \"" + body + "\", \"" + username + "\");";
            } else
            {
                insertCommandText = "INSERT INTO Favorites (favorite_id, favorite_title, " +
                    "favorite_link, favorite_username) " +
                    "VALUES (\"" + id + "\", \"" + title + "\", \"" + link + "\", \"" + username + "\");";
            }

            using (var db = new SqliteConnection("Filename=SharpJokesNew.db"))
            {
                db.Open();

                // check if this is already favorited, so we don't insert a duplicate row
                var selectCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "SELECT * FROM Favorites WHERE favorite_id = \"" + id + "\";"
                };
                var selectResult = selectCommand.ExecuteReader();
                if (selectResult.HasRows) return;

                // insert favorite
                var insertCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = insertCommandText
                };
                insertCommand.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Delete a favorite from the database
        /// </summary>
        /// <param name="postId">Post ID of the favorite to delete</param>
        public static void DeleteFavorite(string postId)
        {
            using (var db = new SqliteConnection("Filename=SharpJokesNew.db"))
            {
                db.Open();
                var deleteCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "DELETE FROM Favorites WHERE favorite_id = \"" + postId + "\";"
                };
                deleteCommand.ExecuteReaderAsync();
            }
        }

        /// <summary>
        /// Checks if a post has been favorited
        /// </summary>
        /// <param name="postId">Post ID</param>
        /// <returns>Boolean indicating if post has been faved</returns>
        public static bool CheckFavorite(string postId)
        {
            bool favorited = false;
            using (var db = new SqliteConnection("Filename=SharpJokesNew.db"))
            {
                db.Open();
                var selectCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "SELECT favorite_id FROM Favorites WHERE favorite_id = \"" + postId + "\";"
                };
                var results = selectCommand.ExecuteReader();
                if (results.HasRows) favorited = true;
            }
            return favorited;
        }

        /// <summary>
        /// Get the favorites from the database
        /// </summary>
        /// <returns>List of favorites (represented by string arrays)</returns>
        public static List<string[]> GetFavorites()
        {
            using (var db = new SqliteConnection("Filename=SharpJokesNew.db"))
            {
                db.Open();
                var selectAllCommand = new SqliteCommand
                {
                    Connection = db,
                    CommandText = "SELECT * FROM Favorites;"
                };
                var result = selectAllCommand.ExecuteReader();
                List<string[]> favorites = new List<string[]>();
                if (result.HasRows)
                {
                    while(result.Read())
                    {
                        var favorite = new string[result.FieldCount];
                        for (var i = 0; i < result.FieldCount; i++)
                            if (!result.IsDBNull(i)) favorite[i] = result.GetString(i);

                        favorites.Add(favorite);
                    }
                }

                return favorites;
            }
        }

    }
}
