# [egram.tel](https://egram.tel) - Telegram client

Egram is an unofficial crossplatform Telegram client written in C#, .NET Core, [ReactiveUI](https://github.com/reactiveui/ReactiveUI) and [Avalonia](https://github.com/AvaloniaUI/Avalonia).

| Platform | Status | Download |
| -------- | ------ | -------- |
| MacOS    | [![Build Status](https://dev.azure.com/egramtel/egramtel/_apis/build/status/egram?branchName=master)](https://dev.azure.com/egramtel/egramtel/_build/latest?definitionId=5?branchName=master) | **[.dmg](https://github.com/egramtel/egram.tel/releases)** |
| Windows  | [![Build Status](https://dev.azure.com/egramtel/egramtel/_apis/build/status/egram?branchName=master)](https://dev.azure.com/egramtel/egramtel/_build/latest?definitionId=5?branchName=master) | **[.exe](https://github.com/egramtel/egram.tel/releases)** **[.zip](https://github.com/egramtel/egram.tel/releases)** |
| Linux    | [![Build Status](https://dev.azure.com/egramtel/egramtel/_apis/build/status/egram?branchName=master)](https://dev.azure.com/egramtel/egramtel/_build/latest?definitionId=5?branchName=master) | **[.tar.gz](https://github.com/egramtel/egram.tel/releases)** |

## Project

This project aims to be a full featured Telegram client with different approach to UI. Also some new features might be introduced to explore what might be implemented on Telegram platform. 100% compatibility with official clients is top priority for this project - features won't be added if they break this rule.

![screenshot](https://raw.githubusercontent.com/egramtel/egram.tel/master/screenshot.png)

## License

Egram is MIT licensed.

## Compiling

To compile and run the application, you need to [download and install latest .NET Core SDK](https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial). Clone the repository using [Git](https://git-scm.com/). Then, go into `egram.tel/src/Tel.Egram` directory and run `dotnet run` command.

```sh
# Remember to install .NET Core SDK and git before executing this.
git clone https://github.com/egramtel/egram.tel
cd egram.tel/src/Tel.Egram
dotnet restore
dotnet run
```

## Contributing

Contributors are welcome. Please submit an issue before introducing new features, then you might create a "work in progress" (WIP) pull request to prevent other people from working on the same feature. Dev group is here: [egram_dev](https://t.me/egram_dev), please feel free to ask questions. If you are new to this project there are some entry-level issues marked with "good first issue" tag.

## Technology stack

* [.NET Core](https://github.com/dotnet)
* [Avalonia](https://github.com/AvaloniaUI/Avalonia)
* [Rx.NET](https://github.com/dotnet/reactive)
* [ReactiveUI](https://github.com/reactiveui/ReactiveUI)
* [Dynamic Data](https://github.com/RolandPheasant/DynamicData)
* [TDLib](https://github.com/tdlib/td)
