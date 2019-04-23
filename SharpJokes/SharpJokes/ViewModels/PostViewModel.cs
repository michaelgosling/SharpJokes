using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpJokes.Commands;
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
        
        // Sort Type Property
        public string Sorting
        {
            get
            {
                string sorting = "";
                switch (RedditController.PostSortType)
                {
                    case RedditController.SortType.Top:
                        sorting = "Top";
                        break;
                    case RedditController.SortType.New:
                        sorting = "New";
                        break;
                    case RedditController.SortType.Hot:
                        sorting = "Popular";
                        break;
                }
                return sorting;
            }
            set
            {
                if (value.Equals("New")) RedditController.PostSortType = RedditController.SortType.New;
                else if (value.Equals("Popular")) RedditController.PostSortType = RedditController.SortType.Hot;
                else if (value.Equals("Top")) RedditController.PostSortType = RedditController.SortType.Top;
            }
        }

        // list of allposts
        private List<PostModel> _allPosts;

        // Selected Post Info
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public string PostUserName { get; set; }
        public string PostId { get; set; }
        public string PostLink { get; set; }
        public BitmapImage PostImg = null;

        public bool PostFavorited { get; set; }

        // Commands
        public FilterTopCommand FilterTopCommand { get; }
        public FilterNewCommand FilterNewCommand { get; }
        public FilterPopularCommand FilterPopularCommand { get; }
        public FavoriteCommand FavoriteCommand { get; }
        public DeleteFavoriteCommand DeleteFavoriteCommand { get; }
        public ShowSelectedFavoriteCommand ShowSelectedFavoriteCommand { get; }


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
                PostId = value == null ? "" : value.PostId;
                PostLink = value == null ? "" : value.Link ?? "";

                // determine if the post is favorited
                if(value != null)
                    PostFavorited = DBAccess.DB.CheckFavorite(value.PostId);

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

                // Can executes
                FilterTopCommand.FireCanExecuteChanged();
                FilterNewCommand.FireCanExecuteChanged();
                FilterPopularCommand.FireCanExecuteChanged();
                FavoriteCommand.FireCanExecuteChanged();
                DeleteFavoriteCommand.FireCanExecuteChanged();
                ShowSelectedFavoriteCommand.FireCanExecuteChanged();

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

        /// <summary>
        /// Default constructor
        /// </summary>
        public PostViewModel() { }

        public PostViewModel(bool isFavorites)
        {
            // create list and observable collection
            _observablePosts = new ObservableCollection<PostModel>();
            _allPosts = new List<PostModel>();

            if (!isFavorites)
            {
                // Initialize the API
                RedditController.Init();

                // Get posts from the API
                GetPostsFromAPI();
            } else
            {
                GetFavorites();
            }

            // clone data into observable collection
            PerformFiltering();

            // Create commands
            FilterTopCommand = new FilterTopCommand(this);
            FilterNewCommand = new FilterNewCommand(this);
            FilterPopularCommand = new FilterPopularCommand(this);
            FavoriteCommand = new FavoriteCommand(this);
            DeleteFavoriteCommand = new DeleteFavoriteCommand(this);
            ShowSelectedFavoriteCommand = new ShowSelectedFavoriteCommand(this);

        }

        /// <summary>
        /// Get favorites from the database and fill them into allposts
        /// </summary>
        public void GetFavorites()
        {
            // get list of string arrays
            var favStrList = DBAccess.DB.GetFavorites();
            // for each string array, create a post and add it to the list
            foreach (var fav in favStrList)
            {
                var post = new PostModel()
                {
                    PostId = fav[0],
                    Title = fav[1],
                    Body = fav[2],
                    Link = fav[3],
                    UserName = fav[4]
                };
                _allPosts.Add(post);
            }
        }


        /// <summary>
        /// Refresh API posts and fill the viewmodels list with them.
        /// </summary>
        public void GetPostsFromAPI()
        {
            // refresh api
            RedditController.RefreshPosts();
            // clear this list
            _allPosts.Clear();
            // fill this list
            foreach (var post in RedditController.Posts)
            {
                _allPosts.Add(post);
            }
        }

        /// <summary>
        /// Filter search list
        /// </summary>
        public void PerformFiltering()
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
