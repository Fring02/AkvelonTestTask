# AkvelonTestTask
This project is a Web API for managing projects and tasks
------------------------------------------------------------------------------------------------------------------------
>This repository contains several layers: (*Domain*), (*Data*), (*Application*), (*Web*)

## ***About***
The project is a web service which allows to manage projects and tasks in a general CRUD manner. The service uses the relational DBMS named PostGreSQL

## ***Technologies stack***

The following technologies were used for the development:
***    
   - **C#, .NET 6.0**
   - **ASP.NET Core 6.0**
   - **PostGreSQL**
   - **Entity Framework Core 6.0 (ORM)**
   - **Npgsql (PostGreSQL provider for C#)**
   - **Automapper (for mapping entities and DTOs)**
   - **Swashbuckle, SwaggerGen**
   - **Docker**
***

## ***Notes***

    - The used database is PostGreSQL.
    - The both database and project are run in Docker. Use the docker-compose.yml for running both.

## ***Configuration***

Configuration is stored in appsettings.json. Sample:  
```
  "ConnectionStrings": {
    "DefaultConnection": "Database connection..."
  }
```
## ***Maintainers***

For any questions or issues of project contact [Fring02](https://github.com/Fring02).
