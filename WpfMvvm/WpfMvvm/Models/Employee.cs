using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMvvm.Models
{
    public class Employee
    {
        public Employee()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsMale { get; set; }
        public string Gender { get { return IsMale ? "Male" : "Female"; } }
        public string Department { get; set; }
        public decimal Gross { get; set; }
        public decimal Payee { get; set; }
        public decimal Net { get { return Gross - Payee; } }
        public bool HasPensionFund { get; set; }

    }
}
