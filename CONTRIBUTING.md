# How to Contribute to this Project

## How to Submit an Issue

Please submit an issue on our Github issues page at <http://github.com/excellalabs/xluhco/issues/new>.

We'll do our best to work with you and help you, whatever the issue is. For the best possible results:

* Try to make your issue as specific as possible
* Provide steps you can take to reproduce the issue
* Tell us what your expectation was of what *should* happen, as well as what *actually* happened. Lots of issues can be resolved by understanding a user's expectations.

## Have an Idea / Request for improvement?

That's great! We love to hear feedback on how we can improve things.

Please know, however, that we may not be able to immediately implement your ideas or suggestions, however great they may be. OSS work is sometimes limited by time anf other constraints.

We suggest [submitting an issue](http://github.com/excellalabs/xluhco/issues/new) with the idea or improvement so that we can discuss it first. At that point, assuming we agree it's a good fit, we will attempt to implement it, or guide you to help you do so within your own pull request(s).

## How to Submit a Pull Request

TODO

## How to Build This Project

### Prerequisites

* This project requires [.NET Core](https://www.microsoft.com/net/download/core) v2.0. You'll want to download and install it. It's cross-platform; you should be able to find an installer that works for you.
* For an IDE, you'll want to consider [VS Code](https://code.visualstudio.com) or [Visual Studio 2017](https://www.visualstudio.com/). VS Code is free / cross-platform and would be sufficient to use for this app.

### Building the Project

* Move to the `src` directory
* Run `dotnet restore` to restore all of the packages
* Run `dotnet build` to build the solution

You can also build using the commands in the IDE.

### Running the Project

* Move to the `xluhco.Web` directory
* Run `dotnet run` or `dotnet watch run` to run the project on a local web server.

You can also run using the commands in the IDE.
