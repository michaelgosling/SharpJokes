using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpJokes.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SharpJokes.ViewModels
{
    public class PostViewModel : INotifyPropertyChanged
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
        public BitmapImage PostImg = null;


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

                // try to set the image of the post if there is one
                try {
                    PostImg = new BitmapImage(new Uri(value.Link, UriKind.Absolute));
                }
                catch (Exception e) { PostImg = null; };

                // Fire off PropertyChange events
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostTitle"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostBody"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostUserName"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostId"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostLink"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("_selectedPost"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PostImg"));
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
                _allPosts.Add(new PostModel("Test" + i, "Test Body" + i, "https://i.redd.it/zo0c94gz6et21.png", "TestUser " + i, i));
            }

            // clone dummy data into observable collection
            PerformFiltering();

        }

        /// <summary>
        /// Filter search list
        /// </summary>
        private void PerformFiltering()
        {
            if (_filter == null) {
                _filter = "";
            }

            //If _filter has a value (ie. user entered something in Filter textbox)
            //Lower-case and trim string
            var lowerCaseFilter = Filter.ToLowerInvariant().Trim();

            //Use LINQ query to get all note names that match filter text, as a list
            var result =
                _allPosts.Where(d => d.Title.ToLowerInvariant()
                .Contains(lowerCaseFilter))
                .ToList();

            //Get list of values in current filtered list that we want to remove
            //(ie. don't meet new filter criteria)
            var toRemove = Posts.Except(result).ToList();

            //Loop to remove items that fail filter
            foreach (var x in toRemove) {
                Posts.Remove(x);
            }

            var resultCount = result.Count;
            // Add back in correct order.
            for (int i = 0; i < resultCount; i++) {
                var resultItem = result[i];
                if (i + 1 > Posts.Count || !Posts[i].Equals(resultItem)) {
                    Posts.Insert(i, resultItem);
                }
            }
        }


    }
}
