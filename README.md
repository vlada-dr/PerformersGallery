# PerformersGallery
WEB API (ASP.NET Core 2.2 + EntityFramework Core) for int20h hackathon test task. 
Flicker API and Face++ API (for filtering by emotions) was used.

## How to start the project
You need to have dotnet environment and [.NET Core 2.2 SDK (click here to download 2.2 version)](https://dotnet.microsoft.com/download/dotnet-core/2.2).
- ### To run from Visual Studio
Click on the IIS Express button (button locates in the same place where you run your other projects in VS, in the top toolbar).
- ### To run from command line
Navigate to the project folder and run 
```sh
dotnet run
```
or navigate to the root solution folder and run
```sh
dotnet run --project PerformersGallery
```
Then navigate to https://localhost:5001 (it is by default, you can see different port in console if 5001 is unavailable, but as you can see
you have http (5000) and https (5001), NAVIGATE TO HTTPS PORT)


## Database
You haven't got access to the Azure db because of Azure firewall, so you will need to create your own DB using migrations. 
(if you don't want to create db - write to  me and I will add your IP-adress in Azure).
### To create your own DB
You need to have SQL Server. Go to the appsettings.json and follow instructions what line you need to comment and uncomment.
Please, look at the connection to your server and change if you need to.
Then go to the `Package Manager Console` and run `update-database`. Thats it! You have your own DB.
### To fill DB
First request to `api/Photos` will fill your DB. It will be a very long request, but then all requests will have average response time 1s
or less, and your data will be always up-to-date.


## Additional Info
> Hackathon int20h (Kyiv, UA) team: Performers.
> Front-end part of task located [here](https://github.com/vlada-dr/performers-gallery) and developed by Doroshenko Vlada.
