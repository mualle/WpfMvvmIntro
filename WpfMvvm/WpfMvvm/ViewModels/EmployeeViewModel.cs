using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMvvm.Models;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Data;

namespace WpfMvvm.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public EmployeeViewModel()
        {

            Employees = new ObservableCollection<Employee>();
            InitDepartments();

           
            CloseView = new RelayCommand(() => this.Close(), () => true);
            CancelCommand = new RelayCommand(() => this.Cancel(), () =>  true);
            NewCommand = new RelayCommand(() => this.New(), () => true);
            EditCommand = new RelayCommand(() => this.Edit(), () => CanEdit);
            DeleteCommand = new RelayCommand(() => this.Delete(), () => CanEdit);
            HelCommand = new RelayCommand(() => this.OpenHelp(), () => true);
            RefreshCommand = new RelayCommand(() => this.Refresh(), () => IsInEditMode == false);
            SaveCommand = new RelayCommand(() => this.Save(), () => CanSave);

            Stats = new ObservableCollection<KeyValuePair<string, int>>();
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
                OnpropertyChanged("IsFemale");
            }
        }
        public bool IsFemale
        {
            get { return !IsMale; }
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

        #region Commands

        public RelayCommand SaveCommand
        {
            get;
            set;
        }

        public RelayCommand CloseView
        {
            get;
            set;
        }

        public RelayCommand CancelCommand
        {
            get;
            set;
        }

        public RelayCommand EditCommand
        {
            get;
            set;
        }

        public RelayCommand NewCommand
        {
            get;
            set;
        }

        public RelayCommand DeleteCommand
        {
            get;
            set;
        }

        public RelayCommand HelCommand
        {
            get;
            set;
        }

        public RelayCommand RefreshCommand
        {
            get;
            set;
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

        public ObservableCollection<DepartmentModel> Departments
        {
            get;
            protected set;
        }

        public ObservableCollection<Employee> Employees
        {
            get;
            protected set;
        }

        public ObservableCollection<KeyValuePair<string, int>> Stats
        {
            get; set;
        }



        public Action CloseAction { get; set; }

        #region Helpers

        private void InitDepartments()
        {
            Departments = new ObservableCollection<DepartmentModel>();
            Departments.Add(new DepartmentModel { Name = "Human Resource" });
            Departments.Add(new DepartmentModel { Name = "Finance And Admin" });
            Departments.Add(new DepartmentModel { Name = "IT Department" });
            Departments.Add(new DepartmentModel { Name = "Sales And Marketing" });
        }

        private void SetSelectedEmployeeDetails()
        {
            if (SelectedEmployee == null)
            {
                ClearInputs();
            }
            else
            {
                Id = SelectedEmployee.Id;
                Name = SelectedEmployee.Name;
                Surname = SelectedEmployee.Surname;
                IsMale = SelectedEmployee.IsMale;
                Department = SelectedEmployee.Department;
                Gross = SelectedEmployee.Gross;
                HasPensionFund = SelectedEmployee.HasPensionFund;
            }
        }

        private void ClearInputs()
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

        private bool CanSave
        {
            get { return Valid(); }
        }

        private bool Valid()
        {
            if(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) || string.IsNullOrWhiteSpace(Department) || Gross <= 0)
            {
                return false;
            }

            return true;

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
            if (Valid())
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

                    ClearInputs();

                   
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

                    Refresh();

                     SelectedEmployee = selectedEmployee;
                    IsInEditMode = false;
                }

                GetStats();
            }
        }

        private void GetStats()
        {
            var departments = from emp in Employees
                              group emp by emp.Department into grp
                              select new { Key = grp.Key, Value = grp.Count() };

            Stats.Clear();
            foreach (var dep in departments.ToList())
            {
                Stats.Add(new KeyValuePair<string, int>(dep.Key, dep.Value));
            }

        }

        private void Delete()
        {
            Employees.Remove(SelectedEmployee);
        }

        private void Refresh()
        {
            List<Employee> _tempEmployess = new List<Employee>();
            foreach (var emp in Employees)
            {
                _tempEmployess.Add(emp);
            }

            Employees.Clear();
            foreach (var emp in _tempEmployess)
            {
                Employees.Add(emp);
            }


            GetStats();
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
