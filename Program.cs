using System.Globalization;

public class Program
{

    public static void Main()
    {
        // Starts the payroll system and creates one payroll object for the whole app.
        Payroll payroll = new Payroll();

        while (true)
        {
            // Clears the screen before showing the main menu again.
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

            // Reads the user's menu choice and sends it to the switch.
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
        // Shows employee-related choices and keeps the menu open until the user goes back.
        while (true)
        {
            // Refreshes the screen before showing employee menu options.
            ClearScreenIfPossible();
            Console.WriteLine("=====================================");
            Console.WriteLine("EMPLOYEE MANAGEMENT");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Add Employee");
            Console.WriteLine("2. View Employees");
            Console.WriteLine("3. Search Employee");
            Console.WriteLine("4. Back");

            // Reads the employee menu choice.
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
        // Collects employee details and saves them after validation.
        ClearScreenIfPossible();
        Console.WriteLine("ADD EMPLOYEE");
        Console.WriteLine("-------------------------------");

        try
        {
            // Reads employee ID and rejects duplicates before saving.
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Duplicate employee ID. Please use a unique ID.");
                Pause();
                return;
            }

            // Reads other employee details and validates the salary.
            string name = ReadRequiredInput("Name: ");
            string department = ReadRequiredInput("Department: ");
            decimal basicSalary = ReadNonNegativeDecimal("Basic Salary: ");

            // Creates an Employee object and sends it to the Payroll layer.
            Employee employee = new Employee(employeeId, name, department, basicSalary);
            payroll.AddEmployee(employee);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Pauses so the user can read the result before returning to the menu.
        Pause();
    }

    private static void SearchEmployee(Payroll payroll)
    {
        // Looks up an employee by ID and prints their information if found.
        ClearScreenIfPossible();
        Console.WriteLine("SEARCH EMPLOYEE");
        Console.WriteLine("-------------------------------");

        // Reads the employee ID to search.
        string employeeId = ReadRequiredInput("Enter Employee ID: ");

        // Finds the employee from the payroll list.
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
        // Shows salary-related actions and keeps the menu open until the user exits this section.
        while (true)
        {
            // Refreshes the screen before the payroll menu appears.
            ClearScreenIfPossible();
            Console.WriteLine("=====================================");
            Console.WriteLine("PAYROLL MANAGEMENT");
            Console.WriteLine("=====================================");
            Console.WriteLine("1. Calculate Salary");
            Console.WriteLine("2. View Salary");
            Console.WriteLine("3. Mark Salary as Paid");
            Console.WriteLine("4. Back");

            // Reads the payroll menu option selected by the user.
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
        // Calculates the salary for an employee using net salary formula.
        ClearScreenIfPossible();
        Console.WriteLine("CALCULATE SALARY");
        Console.WriteLine("-------------------------------");

        try
        {
            // Confirms the employee exists before doing any calculation.
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (!payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Employee not found.");
                Pause();
                return;
            }

            // Reads allowance and deduction values and validates them.
            decimal allowance = ReadNonNegativeDecimal("Allowance: ");
            decimal deduction = ReadNonNegativeDecimal("Deduction: ");

            // Sends salary values to the payroll logic for calculation.
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
        // Displays the latest salary information for an employee.
        ClearScreenIfPossible();
        Console.WriteLine("VIEW SALARY");
        Console.WriteLine("-------------------------------");

        try
        {
            // Reads the employee ID and checks whether the employee exists.
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (!payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Employee not found.");
                Pause();
                return;
            }

            // Calls the payroll logic to display salary details.
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
        // Updates the employee's salary record to show it has been paid.
        ClearScreenIfPossible();
        Console.WriteLine("MARK SALARY AS PAID");
        Console.WriteLine("-------------------------------");

        try
        {
            // Reads employee ID and verifies that the employee exists.
            string employeeId = ReadRequiredInput("Employee ID: ");

            if (!payroll.EmployeeExists(employeeId))
            {
                Console.WriteLine("Employee not found.");
                Pause();
                return;
            }

            // Passes the ID to payroll logic to mark the latest salary as paid.
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
        // Repeats until the user enters a valid non-empty value.
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
        // Ensures the user enters a valid number and not a negative value.
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
        // Clears the console only when the app is running interactively.
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
        // Pauses the screen only in interactive mode; skips when running in automated mode.
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
