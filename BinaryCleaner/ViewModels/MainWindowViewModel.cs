using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BinaryCleaner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            CleanFolderCommand = ReactiveCommand.Create(CleanFolder, this.WhenAnyValue(x => x.PathToClean).Select(y => Directory.Exists(y)));
            OpenFolderCommand = ReactiveCommand.Create(OpenFolder);
        }

        private string pathToClean = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string log = $"{nameof(BinaryCleaner)}{nameof(Log)}\n";

        public ICommand CleanFolderCommand { get; set; }
        public ICommand OpenFolderCommand { get; set; }

        public string Log
        {
            get { return log; }
            set { this.RaiseAndSetIfChanged(ref log, value); }
        }

        public string PathToClean
        {
            get { return pathToClean; }
            set { this.RaiseAndSetIfChanged(ref pathToClean, value); }
        }

        private async Task CleanFolder()
        {
            Debug.WriteLine(pathToClean);
        }

        private async Task OpenFolder()
        {
            var dlg = new OpenFolderDialog
            {
                Directory = PathToClean
            };

            var result = await dlg.ShowAsync(new Window()).ConfigureAwait(false);
            if (!string.IsNullOrEmpty(result))
            {
                PathToClean = result;
            }
        }
    }
}