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
    private readonly string[] _binaries = { "bin", "obj", ".vs", ".idea", "packages", "node_modules" };
    private bool _inProgress;
    private string _log = string.Empty;

    private int _memory;
    private string _pathToClean = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

    public MainWindowViewModel()
    {
        CleanFolderCommand = ReactiveCommand.Create(async () => await CleanFolder(),
            this.WhenAnyValue(x => x.PathToClean).Select(y => Directory.Exists(y)));
        OpenFolderCommand = ReactiveCommand.Create(OpenFolder);
    }

    public ICommand CleanFolderCommand { get; set; }
    public ICommand OpenFolderCommand { get; set; }

    public bool InProgress
    {
        get => _inProgress;
        set => this.RaiseAndSetIfChanged(ref _inProgress, value);
    }

    public int Memory
    {
        get => _memory;
        set => this.RaiseAndSetIfChanged(ref _memory, value);
    }

    public string Log
    {
        get => _log;
        set => this.RaiseAndSetIfChanged(ref _log, value);
    }

    public string PathToClean
    {
        get => _pathToClean;
        set => this.RaiseAndSetIfChanged(ref _pathToClean, value);
    }

    private async Task CleanFolder()
    {
        Log = string.Empty;
        Memory = default;
        Log = PathToClean;
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
                    if (Array.Exists(_binaries, b => b.Equals(di.Name)))
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
        var dlg = new OpenFolderDialog { Directory = PathToClean };

        var result = await dlg.ShowAsync(new Window()).ConfigureAwait(false);
        if (!string.IsNullOrEmpty(result))
        {
            PathToClean = result;
        }
    }
}
