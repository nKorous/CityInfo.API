# CityInfo.API
Created by following along with Pluralsight **ASP.NET Core 6 Web API Fundamentals** by Kevin Dockx. I have not followed the tutorial 100% and used my own flair and direction on some things so that I can better learn but the concepts are there.

## Notes
1. There is a lot of commented out code. I've done this so that I have reference to old code used in the tutorial and so that I know what and why I did things.

1. There are a mix of Dto classes and Entities because we first created this API using an in-memory database and then changed to a SQLite persistent database. I wanted to keep both files so that I can reference this project in the future and know how to do things.

## Commands

### Install dependencies
`dotnet restore`

### Create/update Database through Migrations
`dotnet ef database update`

### Run
`dotnet run`

