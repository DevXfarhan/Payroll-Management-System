# Payroll Management System

A beginner-friendly C# Console Application for managing employees and payroll records. This project is designed for a software-company internship and is kept simple, easy to understand, and modular for future extension into ASP.NET Core and SQL Server projects.

## Project Overview

This application allows a user to:
- Manage employees
- View employee information
- Search for an employee
- Calculate salary using the formula:
  Basic Salary + Allowance - Deduction = Net Salary
- View salary details
- Mark salary as paid

The project uses a console-based menu system and stores data in a MySQL database using XAMPP for local development.

## Technologies Used

- C#
- .NET Console Application
- MySQL Database
- XAMPP (for local database server)
- MySqlConnector library for C# to MySQL connectivity

## Features

### Employee Management
- Add Employee
- View Employees
- Search Employee
- Back to Main Menu

### Payroll Management
- Calculate Salary
- View Salary
- Mark Salary as Paid
- Back to Main Menu

## Business Logic

The salary calculation is based on this formula:

Net Salary = Basic Salary + Allowance - Deduction

## Project Structure

- Program.cs
  - Contains the console menu and user interaction logic
- Employee.cs
  - Contains employee properties and validation logic
- Payroll.cs
  - Handles payroll operations, database connection, employee storage, and salary processing

## Database Setup

This project connects to a MySQL database named `payroll_db` running in XAMPP.

### Create Database

```sql
CREATE DATABASE payroll_db;
USE payroll_db;
```

### Create Employee Table

```sql
CREATE TABLE Employees (
    EmployeeID VARCHAR(50) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Department VARCHAR(100) NOT NULL,
    BasicSalary DECIMAL(10,2) NOT NULL
);
```

### Create Salary Table

```sql
CREATE TABLE SalaryRecords (
    SalaryID INT AUTO_INCREMENT PRIMARY KEY,
    EmployeeID VARCHAR(50),
    Allowance DECIMAL(10,2) NOT NULL,
    Deduction DECIMAL(10,2) NOT NULL,
    NetSalary DECIMAL(10,2) NOT NULL,
    IsPaid BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (EmployeeID) REFERENCES Employees(EmployeeID)
);
```

## Connection String

The project uses this connection string to connect to MySQL:

```csharp
server=127.0.0.1;port=3306;database=payroll_db;uid=root;pwd=;
```

## How to Run the Project

### 1. Start XAMPP
- Open XAMPP Control Panel
- Start MySQL

### 2. Create the database and tables
- Open phpMyAdmin
- Create the database and tables shown above

### 3. Open the project in Visual Studio or VS Code
- Open the .sln or .csproj file

### 4. Restore NuGet packages
- Install MySqlConnector package

### 5. Run the project

```bash
dotnet run
```

## Validation Rules Included

- Prevent duplicate employee IDs
- Prevent negative salary values
- Prevent invalid numeric input
- Prevent searching for non-existing employees
- Show clear and user-friendly messages

## Learning Goals

This project helps beginners understand:
- C# classes and objects
- Properties and methods
- Collections such as List and Dictionary
- Loops and conditions
- Encapsulation
- Input validation
- Exception handling
- Database connectivity with MySQL

## Future Scope

This project is intentionally simple so it can later be extended into:
- ASP.NET Core Web API
- SQL Server database
- MVC architecture
- Authentication and authorization
- Role-based access control
- Report generation

## Project Purpose

This project is a practical beginner-level internship assignment that demonstrates the foundation of employee management and payroll processing in a real-world software system.

## Author

Payroll Management System

## License

This project is created for educational and internship learning purposes.
