# Application preview

This is a .NET Core application with a .NET Web API and Angular application that retrieves information about SpaceX launch missions.
The data is retrieved from the https://github.com/r-spacex/SpaceX-API open-source API.

## Starting the application

- First you need to execute the database_script.sql (found in project root folder/Services) on your local MSSQL server in order to create the database.
- To start the .NET Core API, open a Command Prompt terminal, change the location to the project's API folder and run the following command:

```bash
dotnet run --launch-profile https
```
For more information about the API, once it is started, visit the [API's Swagger UI page](https://localhost:7203/swagger/index.html) page.
- After starting the API, you need to start the Angular application. Open another Command Prompt terminal, change the location to the project's UI folder and run the following command:

```bash
ng serve
```

To open the application, open your browser and navigate to `http://localhost:4200/`.



