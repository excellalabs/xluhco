# How to Contribute to this Project

## How to Submit an Issue

Please submit an issue on our Github issues page at <http://github.com/excellalabs/xluhco/issues/new>.

We'll do our best to work with you and help you, whatever the issue is. For the best possible results:

* Try to make your issue as specific as possible
* Provide steps you can take to reproduce the issue
* Tell us what your expectation was of what *should* happen, as well as what *actually* happened. Lots of issues can be resolved by understanding a user's expectations.
* If you're a coder and you can provide a failing unit test, this would be perfect. :) But we know not everyone's a coder, and we'll help you regardless of what you're able to provide. 

## Have an Idea / Request for Improvement?

That's great! We love to hear feedback on how we can improve things.

Please know, however, that we may not be able to immediately implement your ideas or suggestions, however great they may be. OSS work is sometimes limited by time and other constraints.

We suggest [submitting an issue](http://github.com/excellalabs/xluhco/issues/new) with the idea or improvement so that we can discuss it first. At that point, assuming we agree it's a good fit, we will attempt to implement it, or guide you to help you do so within your own pull request(s).

## Contributing Code via a Pull Request

So you've got some code you'd like to contribute to the project. First off -- Thank you! It means a lot to us that you'd take your time to help improve this project.

We'll try to avoid being super strict about accepting PRs because we value contributions from others, but some general guidelines are below:

* You should [submit an issue](http://github.com/excellalabs/xluhco/issues/new) before beginning a pull request. This makes sure that we have a good heads up that you want to contribute, and also makes sure that if we don't think the idea is a good fit, you don't spend time writing code only to have the PR rejected later.
* You should fork the project first and create a branch for your changes off of the `master` branch
* You should do your best to add automated tests that cover your changes. We're not striving for 100% coverage or anything, but the more well-defined tests there are, the higher our confidence will be. Don't worry about asking for help on this if you need it; that's what we're here for!
* We suggest creating a PR early in the progress and placing `WIP` or `In Progress` in the title of the PR (you can edit it later). This way, as you add changes, we can see the progress, and might be able to jump in to help if we see things going off the rails. This one's your call, though; do whatever suits you.
* Try to make many small commits, with notes, at each step of the way. This will help us understand your thought process when we review the PR. We'll squash these changes at the end of the process, so no worries about being verbose throughout.
* Similarly, don't worry about pre-squashing your changes for us. We'll use Github's functionality at the end of the PR to do that when accepting it.
* Before asking for a review or declaring the PR to be ready, make sure the CI build passes. You'll receive updates on this as you go, so the status at any given time should hopefully be clear.

## How to Build This Project

### Prerequisites

* This project requires [.NET Core](https://www.microsoft.com/net/download/core) v2.0. You'll want to download and install it. It's cross-platform; you should be able to find an installer that works for you.
* For an IDE, you'll want to consider [VS Code](https://code.visualstudio.com) or [Visual Studio 2017](https://www.visualstudio.com/). VS Code is free / cross-platform and would be sufficient to use for this app.

### Building the Project

* Move to the `src` directory
* Run `dotnet restore` to restore all of the packages
* Run `dotnet build` to build the solution
* Run `dotnet test` to run the unit tests. This will also build the project.

You can also build using the commands in the IDE.

### Running the Project

* Move to the `xluhco.Web` directory
* Run `dotnet run` or `dotnet watch run` to run the project on a local web server.
* You can also use `dotnet watch test` if you want to see the tests run whenever there are changes.

You can also run using the commands in the IDE.
