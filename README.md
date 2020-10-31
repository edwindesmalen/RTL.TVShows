# RTL TVShows App
This application scrapes the [TVMaze Api](https://www.tvmaze.com/api) and aggregates movies and cast information in a ([Mongo](https://www.mongodb.com/)) database. The scraped data is accessible by a REST API.

# Important
Due to the guidelines, this solution is written in .NET Core 2.2, which isn't [supported anymore](https://dotnet.microsoft.com/platform/support/policy/dotnet-core). I higly recommend upgrading this solution to the current LTS version (.NET Core 3.1).

# Notes
- Whenever you cancel scraping, you can continue where you last left off (based on the setting "ProceedWhereLastLeftOff: true")
- You can use a task scheduler to automatically run the ScraperApp (for example each day)
- You can configure the number of retries and the amount of seconds to wait to counter the TooManyRequests (429)-response from the TVMaze Api.

## Get started
- Clone the repository
- Install [MongoDB](https://www.mongodb.com/try/download/community)
- Create Event Source (see below)
- To start scraping: Run RTL.TVShows.ScraperApp
- To fetch data using the Api: Run RTL.TVShows.Api (and browse to: https://localhost:44379/swagger)

## Create Event Source
Run a PowerShell window in Admin mode and execute the following commands:
```powershell
New-EventLog -LogName Application -Source "RTL.TVShows.Api"
New-EventLog -LogName Application -Source "RTL.TVShows.ScraperApp"
```

## Nice to haves
- All output of the TVMaze Api [is cached](https://www.tvmaze.com/api#caching) for 60 minutes. To improve the user experience we could check if the requested data from the database is out of date and return the latest version (and update the database).

## New skills aquired during this test
- The existence and use of Polly
- How to use MongoDB