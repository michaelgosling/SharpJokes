
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpJokes.Models;
using SharpJokes;
using System.Diagnostics;
using System.Collections.Generic;

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
        public void CreatePostModel_WithNullBody_NoException()
        {
            try
            {
                var post = new PostModel()
                {
                    PostId = "",
                    Title = "",
                    Body = null,
                    Link = "",
                    UserName = ""
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
            RedditController.Init();
            RedditController.RefreshPosts();
            Assert.IsTrue(RedditController.Posts.Count > 0);
        }

        [TestMethod]
        public void DBAccess_GetFavorites_ReturnsListOfStringArrays()
        {
            var faves = DBAccess.DB.GetFavorites();
            Assert.IsTrue(faves.GetType() == typeof(List<string[]>));
        }

        [TestMethod]
        public void RedditController_GivenTopSortTypeAndRefreshPosts_PostsLengthEqualsFifty()
        {
            RedditController.Init();
            RedditController.PostSortType = RedditController.SortType.Top;
            RedditController.RefreshPosts();
            Assert.AreEqual(50, RedditController.Posts.Count);
        }

        [TestMethod]
        public void RedditController_GivenHotSortTypeAndRefreshPosts_PostsLengthEqualsFifty()
        {
            RedditController.Init();
            RedditController.PostSortType = RedditController.SortType.Hot;
            RedditController.RefreshPosts();
            Assert.AreEqual(50, RedditController.Posts.Count);
        }


        [TestMethod]
        public void RedditController_GivenNewSortTypeAndRefreshPosts_PostsLengthEqualsFifty()
        {
            RedditController.Init();
            RedditController.PostSortType = RedditController.SortType.New;
            RedditController.RefreshPosts();
            Assert.AreEqual(50, RedditController.Posts.Count);
        }

        [TestMethod]
        public void DBAccess_AddFavorite_Successful()
        {
            var newFave = new PostModel()
            {
                PostId = "fake_post_id",
                Title = "Test Post title",
                Body = "Test Post Body",
                UserName = "TestUser123",
                Link = null
            };
            DBAccess.DB.AddFavorite(newFave.PostId, newFave.Title, newFave.Body, newFave.Link, newFave.UserName);
            Assert.IsTrue(DBAccess.DB.CheckFavorite(newFave.PostId));
        }

        [TestMethod]
        public void DBAccess_DeleteFavorite_Successful()
        {
            // create fave
            var newFave = new PostModel()
            {
                PostId = "fake_post_id",
                Title = "Test Post title",
                Body = "Test Post Body",
                UserName = "TestUser123",
                Link = null
            };
            // add before delete
            DBAccess.DB.AddFavorite(newFave.PostId, newFave.Title, newFave.Body, newFave.Link, newFave.UserName);
            // delete
            DBAccess.DB.DeleteFavorite(newFave.PostId);
            Assert.IsFalse(DBAccess.DB.CheckFavorite(newFave.PostId));
        }
    }
}
