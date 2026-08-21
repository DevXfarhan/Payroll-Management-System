public class Employee
{
    private string employeeId = string.Empty;
    private string name = string.Empty;
    private string department = string.Empty;
    private decimal basicSalary;

    public Employee()
    {
        employeeId = string.Empty;
        name = string.Empty;
        department = string.Empty;
        basicSalary = 0;
    }

    public Employee(string employeeId, string name, string department, decimal basicSalary)
    {
        EmployeeId = employeeId;
        Name = name;
        Department = department;
        BasicSalary = basicSalary;
    }

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

    public override string ToString()
    {
        return $"Employee ID: {EmployeeId}\n" +
               $"Name: {Name}\n" +
               $"Department: {Department}\n" +
               $"Basic Salary: {BasicSalary:C}";
    }
}
