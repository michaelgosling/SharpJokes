using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit;
using SharpJokes.Models;

namespace SharpJokes
{
    public class RedditAccess
    {
        private const string REFRESH_TOKEN = "dcIemkZe3jHe6g";
        private const string APP_SECRET = "WCz5sbo4A9XlZ4Am4p_z4rBKMKw";
        private static readonly RedditAPI api = new RedditAPI(appId: Constants.Global.APP_NAME, appSecret: APP_SECRET, refreshToken: REFRESH_TOKEN);
        private static readonly Reddit.Controllers.Subreddit sr = api.Subreddit("ProgrammingHumor").About();

        public enum SortType { Top, Best, New }

        public static SortType PostSortType { get; set; } = SortType.Top;

        public static List<PostModel> Posts { get; }

        public static void RefreshPosts()
        {
            List<Reddit.Controllers.Post> posts = new List<Reddit.Controllers.Post>();
            switch (PostSortType)
            {
                case SortType.Top:
                    sr.Posts.GetTop();
                    posts = sr.Posts.Top;
                    break;
                case SortType.Best:
                    sr.Posts.GetBest();
                    posts = sr.Posts.Best;
                    break;
                case SortType.New:
                    sr.Posts.GetNew();
                    posts = sr.Posts.New;
                    break;
            }

            // clear the posts list
            Posts.Clear();

            // for each post, create a PostModel and add it to the Posts property
            foreach (var post in posts)
            {
                var body = "";
                var link = "";
                if (post.Listing.IsSelf)
                {
                    link = null;
                    body = post.Listing.SelfText;
                } else
                {
                    link = post.Listing.URL;
                    body = null;
                }
                var cPost = new PostModel()
                {
                    PostId = Int32.Parse(post.Id),
                    Title = post.Title,
                    UserName = post.Author,
                    Body = body,
                    Link = link
                };

                Posts.Add(cPost);
            }
        }
    }
}
