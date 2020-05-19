using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryCleaner.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => nameof(BinaryCleaner);
        public string PathToClean { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}