using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SharpJokes.Commands
{
    public class DeleteFavoriteCommand : ICommand {
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private ViewModels.PostViewModel pvm;

        public DeleteFavoriteCommand(ViewModels.PostViewModel pvm) {
            this.pvm = pvm;
        }

        public bool CanExecute(object parameter) {
            return pvm.SelectedPost != null;
        }

        public async void Execute(object parameter) {
            // Delete the Favorite
            DBAccess.DB.DeleteFavorite(pvm.SelectedPost.PostId);
            // Clean up left overs
            pvm.SelectedPost = null;
            pvm.PostId = null;
            pvm.PostImg = null;
            pvm.PostLink = null;
            pvm.PostTitle = null;
            pvm.PostBody = null;
            pvm.PostUserName = null;
            // Refresh posts
            pvm.Posts = new System.Collections.ObjectModel.ObservableCollection<Models.PostModel>();
            pvm.GetFavorites();
        }

        public void FireCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
