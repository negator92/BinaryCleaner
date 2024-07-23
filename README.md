# BinaryCleaner
BinaryCleaner is cross-platform [open-source](https://github.com/negator92/BinaryCleaner/blob/master/LICENSE) software built with [Avalonia](https://github.com/AvaloniaUI/Avalonia).

App can clean \bin, \obj recursive in folder you choose (where C# projects stores).

# Views

## First screen

![](docs/FirstView.jpg)

## Cleaning example

![](docs/AfterClean.jpg)

# To-do

* asyncronus
* loader
* ...
* style
* add local nuget clean

# How to build

 * dotnet clean
 * dotnet restore
 * dotnet build -c release

## Release package build for your runtime [RID](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog#known-rids)

 * dotnet publish -c release -r win-x64 | win-x86 | win-arm64 | linux-x64 | linux-x86 | linux-arm | linux-arm64 | osx-arm64 | osx-x64

# Run

 * dotnet run

# On debug

 * dotnet watch run
