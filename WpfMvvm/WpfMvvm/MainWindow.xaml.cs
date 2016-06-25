using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/// <summary>
/// muallematsela@gmail.com
/// </summary>
namespace WpfMvvm
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Department> _departments;
        private ObservableCollection<Employee> _employees;
        private List<Employee> _tempEmployees;


        bool isNew = false;

        public MainWindow()
        {
            InitializeComponent();

            InitDepartmentsLookup();

            InitEmployeeList();



            this.Closing += MainWindow_Closing;
            btnCancel.Click += BtnCancel_Click;
        }



        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Do you want to close the application?", "Confirm:"
                , MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
            DisableControls();
        }


        /// <summary>
        /// Executed when  window loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Disable emmployee group box
            DisableControls();

        }


        /// <summary>
        /// Executed when Button New Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            isNew = true;
            ClearControls();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgvEmployees.SelectedIndex > -1)
            {
                isNew = false;
                grpBoxEmployeeDetails.IsEnabled = true;
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvEmployees.SelectedIndex > -1)
            {
                if (MessageBox.Show("Do you really want to delete selected employe?", "Confirm:", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _employees.RemoveAt(dgvEmployees.SelectedIndex);
                    ClearControls();
                    DisableControls();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Validate())
            {
                if (isNew)
                {
                    AddNew();

                }
                else
                {
                    Update();

                }

            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grpBoxEmployeeDetails.IsEnabled = false;

            if (dgvEmployees.SelectedIndex > -1)
            {

                var employee = _employees[dgvEmployees.SelectedIndex];

                txtEmployeeNo.Text = employee.Id.ToString();
                txtFirstName.Text = employee.FirstName;
                txtLastName.Text = employee.LastName;
                rbtnMale.IsChecked = employee.Gender == "Male";
                rntFemale.IsChecked = employee.Gender == "Female";
                cmbDepartments.SelectedValue = employee.Department;
                txtGrossSalary.Text = decimal.Round(employee.Gross, 2).ToString();

            }
            else
            {
                ClearControls();
            }
        }

        private void rbtnMale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rntFemale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtGrossSalary_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                decimal gross = 0;
                decimal.TryParse(txtGrossSalary.Text, out gross);
                decimal paye = CalculatePAYE(gross);

                txtPayee.Text = decimal.Round(paye, 2).ToString();

                decimal net = gross - paye;
                txtNetSalary.Text = (decimal.Round(net, 2)).ToString();

            }
            catch
            {

            }
        }

        private void rbnCloseApp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void rbtnHelp_Click(object sender, RoutedEventArgs e)
        {
            var currentLocation = Environment.CurrentDirectory;

            System.Diagnostics.Process.Start(currentLocation + "/Demo_Help.pdf");
        }

        #region Hepler Methods

        private void InitDepartmentsLookup()
        {
            _departments = new ObservableCollection<Department>();
            _departments.Add(new Department { Name = "Human Resource" });
            _departments.Add(new Department { Name = "Finance And Admin" });
            _departments.Add(new Department { Name = "IT Department" });
            _departments.Add(new Department { Name = "Sales And Marketing" });
            cmbDepartments.DisplayMemberPath = "Name";
            cmbDepartments.SelectedValuePath = "Name";
            cmbDepartments.ItemsSource = _departments;
        }

        private void InitEmployeeList()
        {
            _employees = new ObservableCollection<Employee>();
            dgvEmployees.ItemsSource = _employees;

            _tempEmployees = new List<Employee>();
        }

        private bool Validate()
        {
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                valid = false;
                MessageBox.Show("First name is required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                valid = false;
                MessageBox.Show("Surname is required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else if (cmbDepartments.SelectedIndex <= -1)
            {
                valid = false;
                MessageBox.Show("Employee department is required!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                decimal gross = 0;
                if (!decimal.TryParse(txtGrossSalary.Text, out gross))
                {
                    valid = false;
                    MessageBox.Show("Gross salary must be anumber not aplphanumeric", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    if (gross <= 0)
                    {
                        valid = false;
                        MessageBox.Show("Please ener gross salary greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

            return valid;
        }

        private void ClearControls()
        {
            grpBoxEmployeeDetails.IsEnabled = true;
            dgvEmployees.SelectedIndex = -1;

            txtEmployeeNo.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            rbtnMale.IsChecked = true;
            cmbDepartments.SelectedIndex = -1;
            txtGrossSalary.Text = "0.00";
            chkHasPensionFund.IsChecked = false;
        }

        private void DisableControls()
        {
            grpBoxEmployeeDetails.IsEnabled = false;
        }

        private void RefreshList()
        {
            _tempEmployees.Clear();
            foreach (var emp in _employees)
            {
                _tempEmployees.Add(emp);
            }

            _employees.Clear();
            foreach (var emp in _tempEmployees)
            {
                _employees.Add(emp);
            }
        }

        private void Update()
        {
            var selectedIndex = dgvEmployees.SelectedIndex;
            Employee selectedEmployee = _employees[selectedIndex];
            selectedEmployee.FirstName = txtFirstName.Text;
            selectedEmployee.LastName = txtLastName.Text;
            selectedEmployee.Gender = rbtnMale.IsChecked == true ? "Male" : "Female";
            selectedEmployee.Department = cmbDepartments.SelectedValue.ToString();
            selectedEmployee.Gross = decimal.Round(decimal.Parse(txtGrossSalary.Text), 2);
            selectedEmployee.Payee = decimal.Parse(txtPayee.Text);
            selectedEmployee.HasPensionFund = chkHasPensionFund.IsChecked == true;

            RefreshList();

            ClearControls();
            DisableControls();

            dgvEmployees.SelectedIndex = selectedIndex;
        }

        private void AddNew()
        {
            _employees.Add(new Employee
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text,
                Gender = rbtnMale.IsChecked == true ? "Male" : "Female",
                Department = cmbDepartments.SelectedValue.ToString(),
                Gross = decimal.Round(decimal.Parse(txtGrossSalary.Text), 2),
                Payee = decimal.Parse(txtPayee.Text),
                HasPensionFund = chkHasPensionFund.IsChecked == true

            });

            ClearControls();

            DisableControls();
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

    }

    public class Department
    {
        public string Name { get; set; }
    }

    public class Employee
    {
        public Employee()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Department { get; set; }
        public decimal Gross { get; set; }
        public decimal Payee { get; set; }
        public decimal Net { get { return Gross - Payee; } }
        public bool HasPensionFund { get; set; }

    }
}
