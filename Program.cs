using System.Globalization;

public class Program
{

    public static void Main()
    {
        Payroll payroll = new Payroll();

        while (true)
        {
            ClearScreenIfPossible();
            Console.WriteLine("╔══════════════════════════════════════════╗");
            Console.WriteLine("║                                          ║");
            Console.WriteLine("║          PAYROLL MANAGEMENT SYSTEM       ║");
            Console.WriteLine("║                                          ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║              MAIN MENU                   ║");
            Console.WriteLine("╠══════════════════════════════════════════╣");
            Console.WriteLine("║                                          ║");
            Console.WriteLine("║   [1]    Employee Management             ║");
            Console.WriteLine("║   [2]    Payroll Management              ║");
            Console.WriteLine("║   [3]    Exit                            ║");
            Console.WriteLine("║                                          ║");
            Console.WriteLine("╚══════════════════════════════════════════╝");



            string choice = ReadRequiredInput("➜ Select an option: ");

            switch (choice)
            {
                case "1":
                    ManageEmployees(payroll);
                    break;
                case "2":
                    ManagePayroll(payroll);
                    break;
                case "3":
                    Console.WriteLine("\nThank you for using the Payroll Management System.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Pause();
                    break;
            }
        }
    }

    private static void ManageEmployees(Payroll payroll)
    {
        while (true)
        {
            ClearScreenIfPossible();
            Console.WriteLine("=====================================");
            Console.WriteLine("EMPLOYEE MANAGEMENT");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Search Employee");
            Console.WriteLine("4. Back");

            string choice = ReadRequiredInput("Select an option: ");

            switch (choice)
            {
                case "1":
                    AddEmployee(payroll);
                    break;
                case "2":
                    payroll.ViewEmployees();
                    Pause();
                    break;
                case "3":
                    SearchEmployee(payroll);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Pause();
                    break;
            }
        }
    }

    private static void AddEmployee(Payroll payroll)
    {
        ClearScreenIfPossible();
        Console.WriteLine("ADD EMPLOYEE");
        Console.WriteLine("-------------------------------");

        try
        {
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Duplicate employee ID. Please use a unique ID.");
                Pause();
                return;
            }

            string name = ReadRequiredInput("Name: ");
            string department = ReadRequiredInput("Department: ");
            decimal basicSalary = ReadNonNegativeDecimal("Basic Salary: ");

            Employee employee = new Employee(employeeId, name, department, basicSalary);
            payroll.AddEmployee(employee);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    private static void SearchEmployee(Payroll payroll)
    {
        ClearScreenIfPossible();
        Console.WriteLine("SEARCH EMPLOYEE");
        Console.WriteLine("-------------------------------");

        string employeeId = ReadRequiredInput("Enter Employee ID: ");

        Employee? employee = payroll.SearchEmployee(employeeId);

        if (employee == null)
        {
            Console.WriteLine("Employee not found.");
        }
        else
        {
            Console.WriteLine("\nEmployee Details");
            Console.WriteLine("-------------------------------");
            Console.WriteLine(employee.ToString());
        }

        Pause();
    }

    private static void ManagePayroll(Payroll payroll)
    {
        while (true)
        {
            ClearScreenIfPossible();
            Console.WriteLine("=====================================");
            Console.WriteLine("PAYROLL MANAGEMENT");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Calculate Salary");
            Console.WriteLine("2. View Salary");
            Console.WriteLine("3. Mark Salary as Paid");
            Console.WriteLine("4. Back");

            string choice = ReadRequiredInput("Select an option: ");

            switch (choice)
            {
                case "1":
                    CalculateSalary(payroll);
                    break;
                case "2":
                    ViewSalary(payroll);
                    break;
                case "3":
                    MarkSalaryAsPaid(payroll);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Pause();
                    break;
            }
        }
    }

    private static void CalculateSalary(Payroll payroll)
    {
        ClearScreenIfPossible();
        Console.WriteLine("CALCULATE SALARY");
        Console.WriteLine("-------------------------------");

        try
        {
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (!payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Employee not found.");
                Pause();
                return;
            }

            decimal allowance = ReadNonNegativeDecimal("Allowance: ");
            decimal deduction = ReadNonNegativeDecimal("Deduction: ");

            payroll.CalculateSalary(employeeId, allowance, deduction);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    private static void ViewSalary(Payroll payroll)
    {
        ClearScreenIfPossible();
        Console.WriteLine("VIEW SALARY");
        Console.WriteLine("-------------------------------");

        try
        {
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (!payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Employee not found.");
                Pause();
                return;
            }

            payroll.ViewSalary(employeeId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    private static void MarkSalaryAsPaid(Payroll payroll)
    {
        ClearScreenIfPossible();
        Console.WriteLine("MARK SALARY AS PAID");
        Console.WriteLine("-------------------------------");

        try
        {
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (!payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Employee not found.");
                Pause();
                return;
            }

            payroll.MarkSalaryAsPaid(employeeId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Pause();
    }

    private static string ReadRequiredInput(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input.Trim();
            }

            Console.WriteLine("This field cannot be empty. Please enter a value.");
        }
    }

    private static decimal ReadNonNegativeDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();

            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value))
            {
                if (value < 0)
                {
                    Console.WriteLine("Negative value is not allowed.");
                    continue;
                }

                return value;
            }

            Console.WriteLine("Invalid numeric input. Please enter a valid number.");
        }
    }

    private static void ClearScreenIfPossible()
    {
        try
        {
            if (Environment.UserInteractive && !Console.IsOutputRedirected)
            {
                Console.Clear();
            }
        }
        catch
        {
            // ignore
        }
    }

    private static void Pause()
    {
        if (Environment.UserInteractive && !Console.IsInputRedirected)
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine();
        }
    }
}
