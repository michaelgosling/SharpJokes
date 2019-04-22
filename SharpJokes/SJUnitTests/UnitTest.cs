
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpJokes.Models;
using SharpJokes;
using System.Diagnostics;


namespace SJUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreatePostModel_WithNullLink_NoException()
        {
            try
            {
                PostModel post = new PostModel()
                {
                    PostId = "",
                    Title = "Test",
                    Body = "Test body",
                    Link = null,
                    UserName = "Test User"
                };
            } catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RedditController_Initialize_NoException()
        {
            try
            {
                RedditController.Init();
            } catch (Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RedditController_RefreshPosts_PostsPropertyGreaterThanZero()
        {
            try
            {
                RedditController.Init();
                RedditController.RefreshPosts();
                Assert.IsTrue(RedditController.Posts.Count > 0);
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
