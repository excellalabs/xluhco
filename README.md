# xluhco

A URL shortener for Excella that lives at http://xluh.co (Excellaco. Get it?)

## How Does This Work?

* URL rewriting which is set up in the startup configuration, to redirect `xluh.co/[shortLinkCode]` to `/api/Redirect/[shortLinkCode]`
* A `RedirectController` that makes use of an `IShortLinkRepository` to obtain links and permanently redirect if a matching short code is found
* An implementation of `IShortLinkRepository` that reads a CSV file in `wwwroot` using [CsvHelper](https://joshclose.github.io/CsvHelper/)

## The Road map

* We probably should add some Unit Tests
* We probably should add some logging
* We probably should add AppInsights for some easier analytics
* We probably should make the app more configuration friendly (file paths, AppInsights info, etc.)