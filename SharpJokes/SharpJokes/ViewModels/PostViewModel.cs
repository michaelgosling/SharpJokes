﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpJokes.Models;
namespace SharpJokes.ViewModels
{
    public class PostViewModel
    {
        public ObservableCollection<PostModel> Posts { get; set; }
        public PostViewModel()
        {
            Posts = new ObservableCollection<PostModel>();
            PostModel testPost1 = new PostModel("Title 1", "Body 1", null, "UserName1", 1);
            Posts.Add(testPost1);
            PostModel testPost2 = new PostModel("Title 2", "Body 2", null, "UserName2", 2);
            Posts.Add(testPost2);
        }
    }
}
