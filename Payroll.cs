using System;
using System.Collections.Generic;
using System.Linq;
using MySqlConnector;

public class Payroll
{
    // list stores employees in memory while application is running.
    private readonly List<Employee> employees;

    // This dictionary stores the salary information for each employee.
    private readonly Dictionary<string, (decimal Allowance, decimal Deduction, decimal NetSalary, bool IsPaid)> salaryRecords;

    // Connection string to connect this C# app to the XAMPP MySQL database.
    private readonly string connectionString = "server=127.0.0.1;port=3306;database=payroll_db;uid=root;pwd=;";

    public Payroll()
    {
        employees = new List<Employee>();
        salaryRecords = new Dictionary<string, (decimal Allowance, decimal Deduction, decimal NetSalary, bool IsPaid)>(StringComparer.OrdinalIgnoreCase);
        LoadEmployeesFromDatabase();
    }

    // This property gives access to the employee list from other classes.
    public List<Employee> Employees
    {
        get { return employees; }
    }

    // Creates a new MySqlConnection using the connection string.
    private MySqlConnection GetConnection()
    {
        return new MySqlConnection(connectionString);
    }

    // Reads all employee records from the Employees table in MySQL.
    private void LoadEmployeesFromDatabase()
    {
        try
        {
            using var connection = GetConnection();
            connection.Open();

            string query = "SELECT EmployeeID, Name, Department, BasicSalary FROM Employees";
            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string id = reader.GetString("EmployeeID");
                string name = reader.GetString("Name");
                string dept = reader.GetString("Department");
                decimal salary = reader.GetDecimal("BasicSalary");

                employees.Add(new Employee(id, name, dept, salary));
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Database connection error while loading employees: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unexpected error while loading employees: " + ex.Message);
        }
    }

    // Prevents duplicate employee IDs before saving.
    public void AddEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee));

        if (EmployeeExists(employee.EmployeeId))
            throw new InvalidOperationException("Employee ID already exists. Please use a unique ID.");

        try
        {
            using var connection = GetConnection();
            connection.Open();

            string query = @"INSERT INTO Employees (EmployeeID, Name, Department, BasicSalary)
                             VALUES (@EmployeeID, @Name, @Department, @BasicSalary)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeId);
            cmd.Parameters.AddWithValue("@Name", employee.Name);
            cmd.Parameters.AddWithValue("@Department", employee.Department);
            cmd.Parameters.AddWithValue("@BasicSalary", employee.BasicSalary);

            int rows = cmd.ExecuteNonQuery();

            if (rows > 0)
            {
                employees.Add(employee);
                Console.WriteLine("Employee added successfully and saved to database.");
            }
            else
            {
                Console.WriteLine("Employee was not saved to database (no rows affected).");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Database error while adding employee: " + ex.Message);
        }
    }

    // Shows all employees currently stored in the List<Employee>.
    public void ViewEmployees()
    {
        if (employees.Count == 0)
        {
            Console.WriteLine("No employees available yet.");
            return;
        }

        Console.WriteLine("\nEmployee List");
        Console.WriteLine("-------------------------------");

        foreach (var e in employees)
        {
            Console.WriteLine(e.ToString());
            Console.WriteLine("-------------------------------");
        }
    }

    // Returns null if the employee is not found.
    public Employee? SearchEmployee(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            return null;

        return employees.FirstOrDefault(e => e.EmployeeId.Equals(employeeId.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    // Checks whether an employee ID already exists.
    public bool EmployeeExists(string employeeId)
    {
        return SearchEmployee(employeeId) != null;
    }

    // Calculates salary using:
    // Net Salary = Basic Salary + Allowance - Deduction
    public void CalculateSalary(string employeeId, decimal allowance, decimal deduction)
    {
        var employee = SearchEmployee(employeeId);
        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        if (allowance < 0 || deduction < 0)
        {
            Console.WriteLine("Allowance and deduction cannot be negative.");
            return;
        }

        decimal netSalary = employee.BasicSalary + allowance - deduction;
        if (netSalary < 0)
        {
            Console.WriteLine("Net salary cannot be negative.");
            return;
        }

        // Save calculation in memory before storing in database for quick access.
        salaryRecords[employee.EmployeeId] = (allowance, deduction, netSalary, false);

        try
        {
            using var connection = GetConnection();
            connection.Open();

            string query = @"INSERT INTO SalaryRecords (EmployeeID, Allowance, Deduction, NetSalary, IsPaid)
                             VALUES (@EmployeeID, @Allowance, @Deduction, @NetSalary, @IsPaid)";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeId);
            cmd.Parameters.AddWithValue("@Allowance", allowance);
            cmd.Parameters.AddWithValue("@Deduction", deduction);
            cmd.Parameters.AddWithValue("@NetSalary", netSalary);
            cmd.Parameters.AddWithValue("@IsPaid", false);

            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                Console.WriteLine($"Salary calculated and saved for {employee.Name}.");
            }
            else
            
            {
                Console.WriteLine("Salary calculation was not saved to database (no rows affected).");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Database error while saving salary: " + ex.Message);
        }
    }

    // Fetches the latest salary record for a specific employee and displays it.
    public void ViewSalary(string employeeId)
    {
        var employee = SearchEmployee(employeeId);
        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        try
        {
            using var connection = GetConnection();
            connection.Open();

            string query = @"SELECT Allowance, Deduction, NetSalary, IsPaid
                             FROM SalaryRecords
                             WHERE EmployeeID = @EmployeeID
                             ORDER BY SalaryID DESC
                             LIMIT 1";

            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeId);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                Console.WriteLine($"No salary has been calculated for {employee.Name} yet.");
                return;
            }

            decimal allowance = reader.GetDecimal("Allowance");
            decimal deduction = reader.GetDecimal("Deduction");
            decimal netSalary = reader.GetDecimal("NetSalary");
            bool isPaid = reader.GetBoolean("IsPaid");

            salaryRecords[employee.EmployeeId] = (allowance, deduction, netSalary, isPaid);

            Console.WriteLine($"Employee: {employee.Name}");
            Console.WriteLine($"Department: {employee.Department}");
            Console.WriteLine($"Basic Salary: {employee.BasicSalary:C}");
            Console.WriteLine($"Allowance: {allowance:C}");
            Console.WriteLine($"Deduction: {deduction:C}");
            Console.WriteLine($"Net Salary: {netSalary:C}");
            Console.WriteLine($"Status: {(isPaid ? "Paid" : "Unpaid")}");
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Database error while viewing salary: " + ex.Message);
        }
    }

    // Marks the latest salary record for an employee as paid.
    public void MarkSalaryAsPaid(string employeeId)
    {
        var employee = SearchEmployee(employeeId);
        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
            return;
        }

        try
        {
            using var connection = GetConnection();
            connection.Open();

            // Find the latest salary row for this employee.
            string findQuery = "SELECT SalaryID FROM SalaryRecords WHERE EmployeeID = @EmployeeID ORDER BY SalaryID DESC LIMIT 1";
            using var findCmd = new MySqlCommand(findQuery, connection);
            findCmd.Parameters.AddWithValue("@EmployeeID", employee.EmployeeId);

            object? result = findCmd.ExecuteScalar();
            if (result == null)
            {
                Console.WriteLine("Please calculate the salary before marking it as paid.");
                return;
            }

            long salaryId = Convert.ToInt64(result);

            // Update the status of that row to Paid.
            string updateQuery = "UPDATE SalaryRecords SET IsPaid = @IsPaid WHERE SalaryID = @SalaryID";
            using var updateCmd = new MySqlCommand(updateQuery, connection);
            updateCmd.Parameters.AddWithValue("@IsPaid", true);
            updateCmd.Parameters.AddWithValue("@SalaryID", salaryId);

            int rows = updateCmd.ExecuteNonQuery();
            if (rows > 0)
            {
                if (salaryRecords.ContainsKey(employee.EmployeeId))
                {
                    var s = salaryRecords[employee.EmployeeId];
                    salaryRecords[employee.EmployeeId] = (s.Allowance, s.Deduction, s.NetSalary, true);
                }

                Console.WriteLine($"Salary for {employee.Name} has been marked as paid.");
            }
            else
            {
                Console.WriteLine("No salary record was updated.");
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine("Database error while updating salary status: " + ex.Message);
        }
    }
}
