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
        public string PostId { get; set; }

        /// <summary>
        /// Constructor with Properterties
        /// </summary>
        /// <param name="title">Title of the post</param>
        /// <param name="body">Body of a text post</param>
        /// <param name="link">Link of a link post</param>
        /// <param name="username">Username of poster</param>
        /// <param name="postID">Post ID</param>
        public PostModel(string title, string body, string link, string username, string postID)
        {
            Title = title;
            Body = body;
            Link = link;
            UserName = username;
            PostId = postID;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PostModel() { }
    }

}
