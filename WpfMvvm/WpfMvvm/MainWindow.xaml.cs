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
            btnNew_Click(null, null);
            grpBoxEmployeeDetails.IsEnabled = false;
        }


        /// <summary>
        /// Executed when  window loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Disable emmployee group box
            grpBoxEmployeeDetails.IsEnabled = false;


        }

        /// <summary>
        /// Executed when Button New Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            grpBoxEmployeeDetails.IsEnabled = true;
            dgvEmployees.SelectedIndex = -1;

            txtEmployeeNo.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            rbtnMale.IsChecked = true;
            cmbDepartments.SelectedIndex = -1;
            txtGrossSalary.Text = "0.00";

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void rbtnMale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rntFemale_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void txtGrossSalary_TextChanged(object sender, TextChangedEventArgs e)
        {

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
