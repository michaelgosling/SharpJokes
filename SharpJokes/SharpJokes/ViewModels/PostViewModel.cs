using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpJokes.Models;

namespace SharpJokes.ViewModels
{
    public class PostViewModel
    {
        // Event Handlers
        public event PropertyChangedEventHandler PropertyChanged;

        // Observable posts collection
        private ObservableCollection<PostModel> _observablePosts;
        public ObservableCollection<PostModel> Posts {
            get => _observablePosts;
            set
            {
                _observablePosts = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Posts"));
            }
        }

        // list of allposts
        private List<PostModel> _allPosts;

        // Selected Post Info
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public string PostUserName { get; set; }
        public int PostId { get; set; }
        public string PostLink { get; set; }


        // Selected Post field/property
        private PostModel _selectedPost;
        public PostModel SelectedPost
        {
            get => _selectedPost;
            set
            {
                // set selected post field to the value
                _selectedPost = value;

                // Set values, set to null if selected post is null
                PostTitle = value == null ? "" : value.Title;
                PostBody = value == null ? "" : value.Body ?? "";
                PostUserName = value == null ? "" : value.UserName;
                PostId = value == null ? -1 : value.PostId;
                PostLink = value == null ? "" : value.Link ?? "";

                // Fire off PropertyChange events
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostTitle"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostBody"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostUserName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostId"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostLink"));
            }
        }

        // Filter
        private string _filter;
        public string Filter
        {
            get => _filter;
            set
            {
                if (value == _filter) return;

                _filter = value;
                SelectedPost = null;
                PerformFiltering();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filter)));
            }
        }

        public PostViewModel()
        {
            // create list and observable collection
            _observablePosts = new ObservableCollection<PostModel>();
            _allPosts = new List<PostModel>();

            // some dummy data
            for (var i = 0; i < 10; i++)
            {
                _allPosts.Add(new PostModel("Test" + i, "Test Body" + i, null, "TestUser " + i, i));
            }

            // clone dummy data into observable collection
            PerformFiltering();

        }

        /// <summary>
        /// Filter search list
        /// </summary>
        private void PerformFiltering()
        {
            if (_filter == null) _filter = ""; // null should be empty
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim(); // lowercase it and trim it
            var result = _allPosts.Where(n => n.Title.ToLowerInvariant()
                .Contains(lowerCaseFilter)).ToList(); // compare lowercase filter with all Posts
            var toRemove = Posts.Except(result).ToList(); // get all the Posts that don't match the result
            foreach (var x in toRemove) _allPosts.Remove(x); // remove Posts that don't match from observable collection
            for (var i = 0; i < result.Count; i++)
                if (i + 1 > Posts.Count || !Posts[i].Equals(result[i]))
                    Posts.Insert(i, result[i]); // add items back in their correct order
        }


    }
}
