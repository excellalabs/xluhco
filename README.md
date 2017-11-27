# xluh.co

A URL shortener for Excella that lives at <http://xluh.co> (Excella co. Get it?)

[![Build status](https://ci.appveyor.com/api/projects/status/n2268tskumo12j57?svg=true)](https://ci.appveyor.com/project/SeanKilleen/xluhco)

## Adapting xluh.co for Your Own Domain Name

See the [Adapting.md](Adapting.md) guide for advice on how to use this project to host your own URL shortener at your own domain.

## Issue? Idea? Code to Contribute?

Check out the [Contributing.md](Contributing.md) document for guidelines on how to raise issues, build the project, submit PRs, etc.

## How This Works

* ASP.NET route binding to a `RedirectController`. (a basic `HomeController` matches empty routes.)
* A `RedirectController` that makes use of an `IShortLinkRepository` to obtain links and permanently redirect if a matching short code is found
* An implementation of `IShortLinkRepository` that reads a CSV file in `wwwroot` using [CsvHelper](https://joshclose.github.io/CsvHelper/) -- more coming soon.
* Google Analytics, written into the page from the server side response, that includes tracking links.
* An automatic redirect using a callback from the Google Analytics event posting.

Would you like us to expound more on any of these explanations? Please [submit an issue](http://github.com/excellalabs/xluhco/issues/new) with your question and we'll be happy to answer it!

## Lessons We've Learned So Far

* Originally, we used IIS URL rewriting to redirect everything to the `RedirectController` and returned server-side 302 redirects. This worked fine and was really fast, but didn't afford us the ability to capture all of the analytics information we needed (server-side analytics experience just simply isn't as rich). So, we went back to loading a page with Google Analytics injected.
