# Repository Pattern Example Project

This project demonstrates how to implement the Repository Pattern in a .NET application. It contains three branches:

1. **main**: This branch contains the initial implementation without using the Repository Pattern.
2. **RepositoryPattern**: This branch implements the Repository Pattern using SQL Server as the database.
3. **RepositoryPatternRedis**: This branch implements the Repository Pattern using Redis as the database.

## Branches Overview

### main

The `main` branch contains the baseline implementation where the Repository Pattern is not applied. The data access logic is tightly coupled with the business logic, making it less maintainable and harder to test.

### RepositoryPattern

The `RepositoryPattern` branch demonstrates how to refactor the project to use the Repository Pattern with SQL Server as the data source. In this branch, data access logic is abstracted into repository classes, promoting separation of concerns and improving testability.

### RepositoryPatternRedis

The `RepositoryPatternRedis` branch shows how to use the Repository Pattern with Redis as the data source. Similar to the `RepositoryPattern` branch, this implementation abstracts data access into repository classes, but uses Redis to manage data storage and retrieval.

