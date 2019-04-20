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
    public class ShowSelectedFavoriteCommand : ICommand {
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private ViewModels.PostViewModel pvm;

        public ShowSelectedFavoriteCommand(ViewModels.PostViewModel pvm) {
            this.pvm = pvm;
        }

        public bool CanExecute(object parameter) {
            return pvm.SelectedPost != null;
        }

        public async void Execute(object parameter) {
            SelectedFavoriteDialog sfd = new SelectedFavoriteDialog(pvm.PostTitle, pvm.PostBody, pvm.PostBody, pvm.PostLink);
            ContentDialogResult result = await sfd.ShowAsync();
        }

        public void FireCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
