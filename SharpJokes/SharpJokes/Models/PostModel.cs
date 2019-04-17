using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpJokes.Models
{
    public class PostModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public string UserName { get; set; }
        public int PostId { get; set; }

        public PostModel(string title, string body, string link, string username, int postID)
        {
            Title = title;
            Body = body;
            Link = link;
            UserName = username;
            PostId = postID;
        }
    }

}
