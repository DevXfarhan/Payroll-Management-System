public class Employee
{
    // Private fields protect employee details
    private string employeeId = string.Empty;
    private string name = string.Empty;
    private string department = string.Empty;
    private decimal basicSalary;

    // Default constructor initializes empty values
    public Employee()
    {
        employeeId = string.Empty;
        name = string.Empty;
        department = string.Empty;
        basicSalary = 0;
    }

    // Parameterized constructor sets all properties
    public Employee(string employeeId, string name, string department, decimal basicSalary)
    {
        EmployeeId = employeeId;
        Name = name;
        Department = department;
        BasicSalary = basicSalary;
    }

    // Validates employee ID before assignment
    public string EmployeeId
    {
        get => employeeId;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
               throw new ArgumentException("Employee ID cannot be empty.");
            }

            employeeId = value.Trim();
        }
    }

    // Validates employee name before assignment
    public string Name
    {
        get => name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
               throw new ArgumentException("Employee name cannot be empty.");
            }

            name = value.Trim();
        }
    }

    // Validates department name before assignment
    public string Department
    {
        get => department;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
               throw new ArgumentException("Department cannot be empty.");
            }

            department = value.Trim();
        }
    }

    // Prevents negative basic salary values
    public decimal BasicSalary
    {
        get => basicSalary;
        set
        {
            if (value < 0)
            {
               throw new ArgumentOutOfRangeException(nameof(value), "❌ Basic salary cannot be negative.");
            }

            basicSalary = value;
        }
    }

    // Formats employee details for display
    public override string ToString()
    {
        return $"Employee ID: {EmployeeId}\n" +
               $"Name: {Name}\n" +
               $"Department: {Department}\n" +
               $"Basic Salary: {BasicSalary:C}";
    }
}
