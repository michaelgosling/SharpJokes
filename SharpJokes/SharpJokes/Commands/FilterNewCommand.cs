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
    public class FilterNewCommand : ICommand {
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        private ViewModels.PostViewModel pvm;

        public FilterNewCommand(ViewModels.PostViewModel pvm) {
            this.pvm = pvm;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public async void Execute(object parameter) {
            pvm.Sorting = "New";
            pvm.GetPostsFromAPI();
            pvm.Filter = "                                                                      ";
            pvm.Filter = "";
        }

        public void FireCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
