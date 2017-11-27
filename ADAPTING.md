# How to Adapt this Project

## Step 1: Fork or Clone this Repository

You'll want a copy of this repository so that you can edit it and adapt it for your needs. This can be done by either:

* Forking the repository to your own organization / account
* Downloading a copy of this source code and then uploading it to your account.

Forking will allow you to receive upstream updates, but over time those updates may conflict with the changes you plan to make on your own.

## Step 2: Set up a Google Analytics Account

This site is designed to work with [Google Analytics](https://www.google.com/analytics), so if you set up a GA account, you'll be able to track all of the hits and origins of those who are using your short links. It's also possible to check which links people are looking for that aren't found, etc. in order to gain more insight.

Once you sign up for Google Analytics, you'll want to [setup a tracking property](https://support.google.com/analytics/answer/1042508?hl=en).

## Step 3: Modify the Settings

### `appsettings.json` files

In the `src/xluhco.web` directory, you'll find a file called `appsettings.json`.

In this file, you'll want to update:

* `TrackingPropertyId`: Update this with the tracking property that you got when you created the Google Analytics for your site. If you leave it set to the default, we'll receive all of your hits and it will confuse us. :)
* `ShortLinkUrl`: This is the URL where you've chosen to host your link shortener -- your equivalent of `xluh.co`, `aka.ms`, `bit.ly`, etc. It will prefix all of your short links throughout the app, show up in the title bar and on the home page, etc.
* `CompanyName`: The name of your company. Shows up on the home page, etc.
* `CompanyHomePageUrl`: Your company's web site. This link shows up on the home page.
* `InstrumentationKey`: If you choose to use AppInsights, you'll want to replace this with your own key so it isn't confused with ours.

## Step 4: Set up Continuous Integration

This application uses Appveyor out of the box. We supply an `appveyor.yml` file that should have all of the settings & steps necessary to create a CI build from your source code for every one of your own changes. This ensures you'll know the app works if you make any other changes to it.

If you sign up for Appveyor and point it to your cloned repository, the `appveyor.yml` file should take care of most of your work.

## Step 5: Set up a Deployment Pipeline

In the case of the live `xluh.co` domain, we use an Azure Web Site set up to automatically deploy from our master branch. Step by step instructions on that are below.

TODO