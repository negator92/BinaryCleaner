using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCleaner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; }

        public bool IsOpenFolderCommandEnabled { get; set; } = false;
        public string Greeting => nameof(BinaryCleaner);
        public string PathToClean { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        //public MainWindowViewModel()
        //{
        //    OpenFolderCommand = ReactiveCommand.Create(OpenFolder);
        //}

        private async Task OpenFolderCommand()
        {

            var dlg = new OpenFolderDialog();

                dlg.Directory = PathToClean;

            var result = await dlg.ShowAsync();
            if (!string.IsNullOrEmpty(result))
            {
                PathToClean = result;
            }
        }
    }
}