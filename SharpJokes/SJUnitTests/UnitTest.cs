
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpJokes.Models;

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
                    PostId = -1,
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
    }
}
