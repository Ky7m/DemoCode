using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace CSharp7
{
    public sealed class InferredTupleNames
    {
        private readonly List<Employee> _employees = new List<Employee>
        {
            new Employee("William M. Sneed", 58),
            new Employee("Frank M. Kennedy", 85),
            new Employee("Felix Salte", 43)
        };
        
        public InferredTupleNames()
        {
            (var name, var age) = _employees
                .Select(p => (p.Name, p.Age))
                .FirstOrDefault(tuple => tuple.Age > 50);
            WriteLine($"{name} - {age}");
        }
    }

    internal struct Employee
    {
        public string Name;
        public int Age;

        public Employee(string name, int age) => (Name, Age) = (name, age);
    }
}
