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
        private const string REFRESH_TOKEN = "";
        private const string APP_SECRET = "";
        private static RedditAPI api = new RedditAPI(appId: "SharpJokes", appSecret: APP_SECRET, refreshToken: REFRESH_TOKEN);
    }
}
