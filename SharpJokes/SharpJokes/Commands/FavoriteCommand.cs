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
    public class FavoriteCommand : ICommand {
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private ViewModels.PostViewModel pvm;

        public FavoriteCommand(ViewModels.PostViewModel pvm) {
            this.pvm = pvm;
        }

        public bool CanExecute(object parameter) {
            return pvm.SelectedPost != null && !pvm.PostFavorited;
        }

        public async void Execute(object parameter) {
            // Write the favorited post to the favorites DB
            DBAccess.DB.AddFavorite(pvm.SelectedPost.PostId, pvm.SelectedPost.Title, pvm.SelectedPost.Body, pvm.SelectedPost.Link, pvm.SelectedPost.UserName);
            // Set PostFavorited on the viewmodel to true
            pvm.PostFavorited = true;
            // Update can execute
            FireCanExecuteChanged();
        }

        public void FireCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
