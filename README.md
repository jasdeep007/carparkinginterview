# This application is build on .net 8.0 in .net core.





Assumptions

1. appsettings.json has DefaultConnection property which has connection to sql server.
2. On building the project first time it will automatically create database.
3. On building first time it will create 2 tables ParkingSpaces and Vehicles.
4. For ParkingSpaces, it will create 20 small, 50 medium and 30 large parking spots to have the test data.
5. only parking type of small, medium and large is allowed.
6. Classes has validations to allow parking and vehicle data.



Setup and run

1. To set and run, have .net 8.0 installed and visual studio and sql server.
2. Sometimes building it in visual studio download required dependencies, allow it to download and build.
3. This project is created in Microsoft Visual Studio Community 2022 (64-bit) and Version 17.14.12.
4. to Run press cntrl+f5 or just f5.
5. On running swagger ui will open with url ends with /swagger/index.html
6. All APIs can be tested on swagger itself.
7. To run unit test, either use visual studio option to click Test and run all tests or run terminal with dotnet run in the project directory.



Questions

1. Does it include test cases? - Yes it includes basic test cases.
2. Is it .net core built project or not ? - Its .net core project.
3. Will it create database and tables automatically ? - Yes it will on first time build.
4. Can we delete all data and reset database on first time built ? - Yes there is a line Database.EnsureDeleted() in seeddata in carparkcontext.cs file, which can reset the database.
