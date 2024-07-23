using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

namespace BinaryCleaner.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly string[] binaries = {"bin", "obj", ".vs", ".idea", "packages", "node_modules"};
    private bool inProgress;
    private string log = string.Empty;

    private int memory;
    private string pathToClean = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public MainWindowViewModel()
    {
        CleanFolderCommand = ReactiveCommand.Create(async () => CleanFolder(),
            this.WhenAnyValue(x => x.PathToClean).Select(y => Directory.Exists(y)));
        OpenFolderCommand = ReactiveCommand.Create(OpenFolder);
    }

    public ICommand CleanFolderCommand { get; set; }
    public ICommand OpenFolderCommand { get; set; }

    public bool InProgress
    {
        get => inProgress;
        set => this.RaiseAndSetIfChanged(ref inProgress, value);
    }

    public int Memory
    {
        get => memory;
        set => this.RaiseAndSetIfChanged(ref memory, value);
    }

    public string Log
    {
        get => log;
        set => this.RaiseAndSetIfChanged(ref log, value);
    }

    public string PathToClean
    {
        get => pathToClean;
        set => this.RaiseAndSetIfChanged(ref pathToClean, value);
        // Log += $"\n{value}";
    }

    private async Task CleanFolder()
    {
        // Log = PathToClean;
        await RecursiveCleaning(Directory.GetDirectories(PathToClean)).ConfigureAwait(false);
        Log += $"\n\nTotally cleaned: {Memory} bytes.";
    }

    private async Task RecursiveCleaning(string[] dirs)
    {
        InProgress = true;
        try
        {
            if (dirs.Length > 0)
            {
                foreach (var dir in dirs)
                {
                    var di = new DirectoryInfo(dir);
                    if (binaries.Any(b => b.Equals(di.Name)))
                    {
                        di.Delete(true);
                        Log += $"\n{dir}";
                        Memory += dir.Length;
                        Log += 0.Equals(dir.Length) ? string.Empty : $" {dir.Length} bytes";
                    }
                    else
                    {
                        await RecursiveCleaning(Directory.GetDirectories(dir)).ConfigureAwait(false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log += $"{ex.Message}\n";
            throw;
        }
        finally
        {
            InProgress = false;
        }
    }

    private async Task OpenFolder()
    {
        var dlg = new OpenFolderDialog {Directory = PathToClean};

        var result = await dlg.ShowAsync(new Window()).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
        {
            PathToClean = result;
        }
    }
}