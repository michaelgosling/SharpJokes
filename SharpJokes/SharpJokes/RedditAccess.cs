using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reddit;

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

        // will replace this with our own post model and code that fills it
        public static Reddit.Controllers.SubredditPosts Posts { get; } = sr.Posts;

        public static void RefreshPosts()
        {
            switch (PostSortType)
            {
                case SortType.Top:
                    Posts.GetTop();
                    break;
                case SortType.Best:
                    Posts.GetBest();
                    break;
                case SortType.New:
                    Posts.GetNew();
                    break;
            }
        }
    }
}
