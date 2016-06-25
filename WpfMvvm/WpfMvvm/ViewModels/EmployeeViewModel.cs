using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvm.Models;
using System.Linq;

namespace WpfMvvm.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public EmployeeViewModel()
        {

            Employees = new ObservableCollection<Employee>();
            InitDepartments();
        }

        #region Primitive Binding Properties

        private Guid _id;
        public Guid Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnpropertyChanged("Id");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnpropertyChanged("Name");
            }
        }

        private string _surname;
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnpropertyChanged("Surname");
            }
        }

        private bool _isMale = true;
        public bool IsMale
        {
            get { return _isMale; }
            set
            {
                _isMale = value;
                OnpropertyChanged("IsMale");
            }
        }

        private string _department;
        public string Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnpropertyChanged("Department");
            }
        }

        private decimal _gross;
        public decimal Gross
        {
            get { return _gross; }
            set
            {
                _gross = value;
                OnpropertyChanged("Gross");

                Payee = CalculatePAYE(value);
            }
        }

        private decimal _payee;
        public decimal Payee
        {
            get { return _payee; }
            set
            {
                _payee = value;
                OnpropertyChanged("Payee");
                OnpropertyChanged("Net");
            }
        }

        public decimal Net { get { return Gross - Payee; } }

        private bool _hasPensionFund;
        public bool HasPensionFund
        {
            get { return _hasPensionFund; }
            set
            {
                _hasPensionFund = value;

                OnpropertyChanged("HasPensionFund");

            }
        }

        #endregion

        private bool _canEdit = false;
        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                OnpropertyChanged("CanEdit");

            }
        }

        private bool _isInEditMode = false;
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                OnpropertyChanged("IsInEditMode");
            }
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;

                OnpropertyChanged("SelectedEmployee");

                CanEdit = value == null ? false : true;

                SetSelectedEmployeeDetails();
            }
        }

        public ObservableCollection<Department> Departments
        {
            get;
            protected set;
        }

        public ObservableCollection<Employee> Employees
        {
            get;
            protected set;
        }

        public Action CloseAction { get; set; }

        #region Helpers

        private void InitDepartments()
        {
            Departments = new ObservableCollection<Department>();
            Departments.Add(new Department { Name = "Human Resource" });
            Departments.Add(new Department { Name = "Finance And Admin" });
            Departments.Add(new Department { Name = "IT Department" });
            Departments.Add(new Department { Name = "Sales And Marketing" });
        }

        private void SetSelectedEmployeeDetails()
        {
            if (SelectedEmployee == null)
            {
                Id = Guid.Empty;
                Name = null;
                Surname = null;
                IsMale = true;
                Department = null;
                Gross = 0;
                HasPensionFund = false;

                IsInEditMode = false;
            }
            else
            {
                Id = SelectedEmployee.Id;
                Name = SelectedEmployee.Name;
                Surname = SelectedEmployee.Surname;
                IsMale = SelectedEmployee.IsMale;
                Department = SelectedEmployee.Department;
                Gross = SelectedEmployee.Gross;
            }
        }

        private bool Valid()
        {
            return (!string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Surname)
                && !string.IsNullOrWhiteSpace(Department) 
                && Gross > 0);
        }

        private void OpenHelp()
        {
            var currentLocation = Environment.CurrentDirectory;

            System.Diagnostics.Process.Start(currentLocation + "/Demo_Help.pdf");
        }

        private void Close()
        {
            CloseAction();
        }

        private void Cancel()
        {
            IsInEditMode = false;
            SelectedEmployee = null;
        }

        private void New()
        {
            SelectedEmployee = null;
            IsInEditMode = true;

            Id = Guid.NewGuid();
            Name = string.Empty;
            Surname = string.Empty;
            Department = null;
            Gross = 0;
            IsMale = true;

        }

        private void Edit()
        {
            IsInEditMode = true;
        }

        private void Save()
        {
            var selectedEmployee = Employees.FirstOrDefault(x => x.Id == Id);
            if (selectedEmployee == null)
            {
                Employees.Add(new Employee
                {
                    Id = Id,
                    Name = Name,
                    Surname = Surname,
                    IsMale = IsMale,
                    Department = Department,
                    Gross = Gross,
                    HasPensionFund = HasPensionFund,
                    Payee = Payee

                });

                selectedEmployee = null;
                IsInEditMode = false;
            }
            else
            {
                selectedEmployee.Name = Name;
                selectedEmployee.Surname = Surname;
                selectedEmployee.IsMale = IsMale;
                selectedEmployee.Department = Department;
                selectedEmployee.Gross = Gross;
                selectedEmployee.HasPensionFund = HasPensionFund;
                selectedEmployee.Payee = Payee;

                OnpropertyChanged("Employees");

                SelectedEmployee = selectedEmployee;
                IsInEditMode = false;
            }
        }

        private void Delete()
        {
            Employees.Remove(SelectedEmployee);
        }

        private void Refresh()
        {
            OnpropertyChanged("Employees");
        }

        private decimal CalculatePAYE(decimal taxableAmount)
        {
            try
            {
                double minPercentage = 0.2; // 20%
                double maxPercentage = 0.3; // 30%
                decimal minAmount = decimal.Parse("2805");
                decimal maxAmount = decimal.Parse("4747");

                decimal GovRen = decimal.Parse(minPercentage.ToString()) * decimal.Parse(minAmount.ToString());
                GovRen = decimal.Round(GovRen, 2);

                decimal PAYE = 0;

                if (taxableAmount <= minAmount)
                {
                    PAYE = 0;
                }
                else if (minAmount < taxableAmount && taxableAmount <= maxAmount)
                {
                    PAYE = taxableAmount * decimal.Parse(minPercentage.ToString());
                    PAYE = decimal.Round(PAYE, 2) - GovRen;
                }
                else
                {
                    decimal Rem = taxableAmount - decimal.Parse(maxAmount.ToString());
                    Rem = decimal.Round(Rem, 2);

                    decimal LowerRate = decimal.Parse(maxAmount.ToString()) * decimal.Parse(minPercentage.ToString());
                    decimal UpperRate = decimal.Parse(maxPercentage.ToString()) * Rem;

                    LowerRate = decimal.Round(LowerRate, 2);
                    UpperRate = decimal.Round(UpperRate, 2);

                    PAYE = (LowerRate + UpperRate) - GovRen;
                }
                return PAYE;
            }
            catch
            {

                return 0;
            }



        }

        #endregion


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnpropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

    }
}
