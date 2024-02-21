using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
using System.Xml.Linq;

namespace MidTermProject.USERCONTROLS
{
    /// <summary>
    /// Interaction logic for AddStudent.xaml
    /// </summary>
    public partial class AddStudent : UserControl
    {
        int id;
        public AddStudent()
        {
            InitializeComponent();

        }
        public AddStudent(DataRowView selectedRow)
        {
            
            InitializeComponent();
            AddButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            StRegTxtBox.Text = (string)selectedRow["RegistrationNo"];
            St1NameTxtBox.Text = (string)selectedRow["FirstName"];
            St2nameTxtBox.Text = (string)selectedRow["LastName"];
            StContactTxtBox.Text = (string)selectedRow["Contact"];
            StEmailTxtBox.Text = (string)selectedRow["Email"];
            StDOBDatepicker.Text = selectedRow["DateOfBirth"].ToString();
            stGenComBox.SelectedItem = (string)selectedRow["Gender"];
            id = (int)selectedRow["id"];
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (St1NameTxtBox.Text == "" || St2nameTxtBox.Text == "" || StContactTxtBox.Text == "" || StRegTxtBox.Text == "" || StEmailTxtBox.Text == "" || stGenComBox.Text == "")
            {
                MessageBox.Show("Please fill all Queries First");
            }
            else
            {
                DateTime? selectedDate = StDOBDatepicker.SelectedDate;
                if (DateTime.Compare(selectedDate.Value, DateTime.Now) > 0)
                {
                    MessageBox.Show("Please enter the valid date of birth");
                }
                else
                {
                    int gender;
                    if (stGenComBox.Text == "Male")
                    {
                        gender = 1;
                    }
                    else
                    {
                        gender = 2;
                    }
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Person VALUES( @FirstName, @lastName, @Contact, @Email, @Date, @Gender);\r\nINSERT INTO Student VALUES((select Id from Person where FirstName = @FirstName AND LastName =  @lastName AND Contact =@Contact ), @RegistrationNumber);\r\n", con);
                    cmd.Parameters.AddWithValue("@FirstName", St1NameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@LastName", St2nameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Contact", StContactTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Email", StEmailTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(StDOBDatepicker.Text).ToString("yyyy/MM/dd"));
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", StRegTxtBox.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully saved");

                    txtBlockStInfo.Visibility = Visibility.Collapsed;
                    container.Children.Add(new StudentCRUD());
                }
            }

        }

        private void stGenComBox_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("select Value from Lookup where Category = 'GENDER'");
            cmd1.Connection = con;
            SqlDataReader sqlData1 = cmd1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(sqlData1);
            List<string> genList = new List<string>();
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                string s = dataTable1.Rows[i]["Value"].ToString();
                genList.Add(s);
            }
            stGenComBox.ItemsSource = genList;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (St1NameTxtBox.Text == "" || St2nameTxtBox.Text == "" || StContactTxtBox.Text == "" || StRegTxtBox.Text == "" || StEmailTxtBox.Text == "" || stGenComBox.Text == "")
            {
                MessageBox.Show("Please fill all Queries First");
            }
            else
            {
                DateTime? selectedDate = StDOBDatepicker.SelectedDate;
                if (DateTime.Compare(selectedDate.Value, DateTime.Now) > 0)
                {
                    MessageBox.Show("Please enter the valid date of birth");
                }
                else
                {
                    int gender;
                    if (stGenComBox.Text == "Male")
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
                    " where Id = @id " +
                    " update [dbo].[Student]" +
                    " set RegistrationNo = @RegistrationNumber" +
                    " where Id = @id ", con);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@RegistrationNumber", StRegTxtBox.Text);
                    cmd.Parameters.AddWithValue("@FirstName", St1NameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@LastName", St2nameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Contact", StContactTxtBox.Text);
                    cmd.Parameters.AddWithValue("@Email", StEmailTxtBox.Text);
                    cmd.Parameters.AddWithValue("@DOB", StDOBDatepicker.Text);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated");
                    txtBlockStInfo.Visibility = Visibility.Collapsed;
                    container.Children.Add(new StudentCRUD());
                }
            }
        }
    }


}
