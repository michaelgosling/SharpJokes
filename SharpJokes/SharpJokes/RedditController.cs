using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedditSharp;
using SharpJokes.Models;
using Windows.UI.Xaml.Controls;

namespace SharpJokes
{
    public class RedditController
    {
        private const string CLIENT_ID = "dcIemkZe3jHe6g";
        private const string APP_SECRET = "WCz5sbo4A9XlZ4Am4p_z4rBKMKw";
        private static RedditSharp.Things.Subreddit subreddit;
        private static Reddit reddit;
        private static BotWebAgent webAgent;

        /// <summary>
        /// Enum for sort type
        /// </summary>
        public enum SortType { Top, Hot, New }

        /// <summary>
        /// Static property for setting sort type of posts
        /// </summary>
        public static SortType PostSortType { get; set; } = SortType.Top;

        /// <summary>
        /// List of posts based on postmodel
        /// </summary>
        public static List<PostModel> Posts { get; } = new List<PostModel>();

        /// <summary>
        /// Initialize Controller with necessary attributes
        /// </summary>
        public static void Init()
        {
            webAgent = new BotWebAgent("SharpJokesBot", "SharpJokesBotPass", CLIENT_ID, APP_SECRET, "http://localhost:8080/");
            reddit = new Reddit(webAgent, false);
            subreddit = reddit.GetSubreddit("/r/ProgrammerHumor");
            RefreshPosts();
        }

        /// <summary>
        /// Refresh posts based on the current sort type
        /// </summary>
        public static void RefreshPosts()
        {
            // create new  object for posts
            IEnumerable<RedditSharp.Things.Post> posts = null;

            // fill listing dependent on the sort type
            if (PostSortType == SortType.Top)
                posts = subreddit.GetTop(RedditSharp.Things.FromTime.Week).Take(50);
            else if (PostSortType == SortType.New)
                posts = subreddit.New.Take(50);
            else if (PostSortType == SortType.Hot)
                posts = subreddit.Hot.Take(50);

            // clear the posts list
            Posts.Clear();

            
            // if posts is null, return without doing anything
            if (posts == null) return;

            // for each post, create a PostModel and add it to the Posts property
            foreach (var post in posts)
            {

                var body = "";
                var link = "";
                if (post.IsSelfPost)
                {
                    link = null;
                    body = post.SelfText.ToString();
                }
                else
                {
                    link = post.Url.OriginalString.ToString();
                    body = null;
                }
                var cPost = new PostModel()
                { 
                    PostId = post.Id,
                    Title = post.Title,
                    UserName = post.AuthorName,
                    Body = body,
                    Link = link
                };

                Posts.Add(cPost);
            }
        }
    }
}
