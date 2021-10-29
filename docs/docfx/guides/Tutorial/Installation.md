---
uid: Guides.GettingStarted.Installation
title: Twitch .NET Installation
---

## Twitch .NET Installation

You can download Twitch .NET via the NuGet package manager which we <strong>strongly</strong> recommend. Alternatively, you may also compile the library yourself if you see any advantage in that.

## Supported Platform

Twitch .NET currently only targets [.NET Standard]("https://docs.microsoft.com/en-us/dotnet/standard/net-standard") 2.1. This means you can create applications with [.NET Core]("https://docs.microsoft.com/en-us/dotnet/fundamentals/") 3.0 or higher.

## Installing with NuGet


### [Using Visual Studio](#tab/visualstudio-install)

1. Right click on 'References', and 'Manage Nuget packages'

2. In the "Browse" tab, search for `Naendo.Twitch.NET`

3. Click install.


### [Using JetBrains Rider](#tab/rider-install)

1. Open the NuGet windows (Tools > NuGet > Manage NuGet packages for Solution)

2. In the "Packages" tab, search `Naendo.Twitch.NET`


### [Using the Nuget Package Manager](#tab/npm-install)

1. Click on 'Tools', 'Nuget Package Manager' and 'Package Manager Console'
2. Enter `Install-Package Naendo.Twitch.NET`

