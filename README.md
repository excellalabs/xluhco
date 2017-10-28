# xluh.co

A URL shortener for Excella that lives at <http://xluh.co> (Excella co. Get it?)

## How to Build This Project

### Prerequisites

* This project requires [.NET Core](https://www.microsoft.com/net/download/core) v2.0. You'll want to download and install it. It's cross-platform; you should be able to find an installer that works for you.
* For an IDE, you'll want to consider [VS Code](https://code.visualstudio.com) or [Visual Studio 2017](https://www.visualstudio.com/). VS Code is free / cross-platform and would be easy to use for this app.

### Building the Project

* Move to the `src` directory
* Run `dotnet restore` to restore all of the packages
* Run `dotnet build` to build the solution

You can also build using the commands in the IDE.

### Running the Project

* Move to the `xluhco.Web` directory
* Run `dotnet run` or `dotnet watch run` to run the project on a local web server.

You can also run using the commands in the IDE.

## How This Works

* ASP.NET route binding to a `RedirectController`. (a basic `HomeController` matches empty routes.)
* A `RedirectController` that makes use of an `IShortLinkRepository` to obtain links and permanently redirect if a matching short code is found
* An implementation of `IShortLinkRepository` that reads a CSV file in `wwwroot` using [CsvHelper](https://joshclose.github.io/CsvHelper/) -- more coming soon.
* Google Analytics, written into the page from the server side response, that includes tracking links.
* An automatic redirect using a callback from the Google Analytics event posting.

## Lessons We've Learned So Far

* Originally, we used IIS URL rewriting to redirect everything to the `RedirectController` and returned server-side 302 redirects. This worked fine and was really fast, but didn't afford us the ability to capture all of the analytics information we needed (server-side analytics experience just simply isn't as rich). So, we went back to loading a page with Google Analytics injected.