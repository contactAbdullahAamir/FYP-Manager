using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Xml.Linq;

namespace MidTermProject.USERCONTROLS
{
    /// <summary>
    /// Interaction logic for StudentCRUD.xaml
    /// </summary>
    public partial class StudentCRUD : UserControl
    {
        public StudentCRUD()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addButton.Visibility = Visibility.Collapsed;
            StDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddStudent());
        }

        private void container_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select S.id, S.RegistrationNo , P.FirstName , P.LastName , P.Contact, P.DateOfBirth , P.Email , L.Value as Gender from Person As P join Student as S on P.Id = S.Id join Lookup as L on L.Id = P.Gender", con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            StDataGrid.DataContext= dataTable;
            StDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)StDataGrid.SelectedItem;
            addButton.Visibility = Visibility.Collapsed;
            StDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddStudent(selectedRow));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)StDataGrid.SelectedItem;
            int id = (int)selectedRow["ID"];
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select StudentId\r\nFrom GroupStudent\r\nwhere StudentId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            List<string> IdList = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string s = dataTable.Rows[i]["StudentId"].ToString();
                IdList.Add(s);
            }
            if (IdList == null || IdList.Count == 0)
            {
                SqlCommand cmd1 = new SqlCommand("delete Student where Id = @id;\r\ndelete Person where Id = @id;\r\ndelete dbo.GroupStudent where StudentId = @id;");
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.Connection = con;
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Record Has Been Deleted Successfully...");
                refreshButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("This student is associated with a group. Please remove it from there first");
            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            StDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select S.id, S.RegistrationNo , P.FirstName , P.LastName , P.Contact , P.Email, P.DateOfBirth , L.Value as Gender from Person As P join Student as S on P.Id = S.Id join Lookup as L on L.Id = P.Gender", con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            StDataGrid.DataContext = dataTable;
            StDataGrid.ItemsSource = dataTable.DefaultView;
            refreshButton.Visibility= Visibility.Hidden;
        }
    }
}
