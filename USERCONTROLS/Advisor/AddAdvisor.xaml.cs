using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using System.Reflection;

namespace MidTermProject.USERCONTROLS.Advisor
{
    /// <summary>
    /// Interaction logic for AddAdvisor.xaml
    /// </summary>
    public partial class AddAdvisor : UserControl
    {
        int id;
        public AddAdvisor()
        {
            InitializeComponent();
        }
        public AddAdvisor(DataRowView selectedRow)
        {
                InitializeComponent();
                AddButton.Visibility = Visibility.Hidden;
                EditButton.Visibility = Visibility.Visible;
                AdDesComBox.SelectedItem = (string)selectedRow["Designation"];
                AdSalTxtBox.Text = (string)selectedRow["Salary"].ToString();
                Ad1NameTxtBox.Text = (string)selectedRow["FirstName"];
                Ad2nameTxtBox.Text = (string)selectedRow["LastName"];
                AdContactTxtBox.Text = (string)selectedRow["Contact"];
                AdEmailTxtBox.Text = (string)selectedRow["Email"];
                AdDOBDatepicker.Text = selectedRow["DateOfBirth"].ToString();
                AdGenComBox.SelectedItem = (string)selectedRow["Gender"];
                id = (int)selectedRow["id"];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (Ad1NameTxtBox.Text == "" || Ad2nameTxtBox.Text == "" || AdContactTxtBox.Text == "" || AdDesComBox.Text == "" || AdEmailTxtBox.Text == "" || AdGenComBox.Text == "" || AdSalTxtBox.Text == "")
            {
                MessageBox.Show("Please fill all Queries First");
            }
            else
            {
                DateTime? selectedDate = AdDOBDatepicker.SelectedDate;
                if (DateTime.Compare(selectedDate.Value , DateTime.Now) > 0)
                {
                    MessageBox.Show("Please enter the valid date of birth");
                }
                else
                {
                    int designation;
                    int gender;
                    if (AdDesComBox.Text == "Professor")
                    {
                        designation = 6;
                    }
                    else if (AdDesComBox.Text == "Associate Professor")
                    {
                        designation = 7;
                    }
                    else if (AdDesComBox.Text == "Assisstant Professor")
                    {
                        designation = 8;
                    }
                    else if (AdDesComBox.Text == "Lecturer")
                    {
                        designation = 9;
                    }
                    else
                    {
                        designation = 10;
                    }

                    if (AdGenComBox.Text == "Male")
                    {
                        gender = 1;
                    }
                    else
                    {
                        gender = 2;
                    }

                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Person](FirstName,LastName,Contact,Email,DateOfBirth,Gender) VALUES (@FirstName,@LastName, @Contact,@Email,@DOB, @Gender) " +
                        "insert into [dbo].[Advisor](Id, Salary, Designation) VALUES((SELECT Id FROM Person WHERE FirstName = @FirstName AND LastName = @LastName AND Contact = @Contact AND Email = @Email AND DateOfBirth = @DOB AND Gender = @Gender), @Salary, @Designation); ", con);
                    cmd.Parameters.AddWithValue("@Salary", int.Parse(AdSalTxtBox.Text));
                    cmd.Parameters.AddWithValue("@Designation", designation);
                    cmd.Parameters.AddWithValue("@FirstName", Ad1NameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@LastName", Ad2nameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Contact", AdContactTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Email", AdEmailTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(AdDOBDatepicker.Text).ToString("yyyy/MM/dd"));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully saved");

                    txtBlockAdInfo.Visibility = Visibility.Collapsed;
                    container.Children.Add(new AdvisorCRUD());
                }
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (Ad1NameTxtBox.Text == "" || Ad2nameTxtBox.Text == "" || AdContactTxtBox.Text == "" || AdDesComBox.Text == "" || AdEmailTxtBox.Text == "" || AdGenComBox.Text == "" || AdSalTxtBox.Text == "")
            {
                MessageBox.Show("Please fill all Queries First");
            }
            else
            {
                DateTime? selectedDate = AdDOBDatepicker.SelectedDate;
                if (DateTime.Compare(selectedDate.Value, DateTime.Now) > 0)
                {
                    MessageBox.Show("Please enter the valid date of birth");
                }
                else
                {
                    int designation;
                    int gender;
                    if (AdDesComBox.Text == "Professor")
                    {
                        designation = 6;
                    }
                    else if (AdDesComBox.Text == "Associate Professor")
                    {
                        designation = 7;
                    }
                    else if (AdDesComBox.Text == "Assisstant Professor")
                    {
                        designation = 8;
                    }
                    else if (AdDesComBox.Text == "Lecturer")
                    {
                        designation = 9;
                    }
                    else
                    {
                        designation = 10;
                    }

                    if (AdGenComBox.Text == "Male")
                    {
                        gender = 1;
                    }
                    else
                    {
                        gender = 2;
                    }

                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("update [dbo].[Person] " +
                    " set FirstName = @FirstName, LastName = @LastName, Contact = @Contact, Email = @Email, DateOfBirth = @DOB, Gender = @Gender " +
                    " where Id = @id; " +
                    " update [dbo].[Advisor]" +
                    " set Designation = @Designation, Salary = @Salary" +
                    " where Id = @id ", con);
                    cmd.Parameters.AddWithValue("@Salary", int.Parse(AdSalTxtBox.Text));
                    cmd.Parameters.AddWithValue("@Designation", designation);
                    cmd.Parameters.AddWithValue("@FirstName", Ad1NameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@LastName", Ad2nameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Contact", AdContactTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Email", AdEmailTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(AdDOBDatepicker.Text).ToString("yyyy/MM/dd"));
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated");
                    txtBlockAdInfo.Visibility = Visibility.Collapsed;
                    container.Children.Add(new AdvisorCRUD());
                }
            }
        }

        private void AdDesComBox_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select Value from Lookup where Category = 'DESIGNATION'");
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            List<string> list = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string s = dataTable.Rows[i]["Value"].ToString();
                list.Add(s);
            }
            AdDesComBox.ItemsSource = list;
        }

        private void AdGenComBox_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select Value from Lookup where Category = 'GENDER'");
            cmd.Connection = con;
            SqlDataReader sqlData1 = cmd.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(sqlData1);
            List<string> list = new List<string>();
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                string s = dataTable1.Rows[i]["Value"].ToString();
                list.Add(s);
            }
            AdGenComBox.ItemsSource = list;
        }
    }
}
