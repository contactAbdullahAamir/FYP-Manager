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

namespace MidTermProject.USERCONTROLS.Advisor
{
    /// <summary>
    /// Interaction logic for AdvisorCRUD.xaml
    /// </summary>
    public partial class AdvisorCRUD : UserControl
    {
        public AdvisorCRUD()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addButton.Visibility = Visibility.Collapsed;
            AdDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddAdvisor());
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            AdDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select P1.Id , P1.FirstName , P1.LastName ,L.Value as Gender, P1.Contact , P1.Email , P1.DateOfBirth , P1.Designation , P1.Salary from Lookup as L join (select A.Id, A.Salary, P.FirstName , P.LastName , P.Contact , P.Email , P.DateOfBirth , L.Value as Designation , P.Gender from Advisor as A join Person  as P on P.Id = A.Id join Lookup as L on L.Id = A.Designation) as P1 on P1.Gender = L.Id");
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            AdDataGrid.DataContext = dataTable;
            AdDataGrid.ItemsSource = dataTable.DefaultView;
            refreshButton.Visibility = Visibility.Hidden;
        }

        private void AdDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select P1.Id , P1.FirstName , P1.LastName ,L.Value as Gender, P1.Contact , P1.Email , P1.DateOfBirth , P1.Designation , P1.Salary from Lookup as L join (select A.Id, A.Salary, P.FirstName , P.LastName , P.Contact , P.Email , P.DateOfBirth , L.Value as Designation , P.Gender from Advisor as A join Person  as P on P.Id = A.Id join Lookup as L on L.Id = A.Designation) as P1 on P1.Gender = L.Id");
            cmd.Connection = con;
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            AdDataGrid.DataContext = dataTable;
            AdDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void container_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)AdDataGrid.SelectedItem;
            addButton.Visibility = Visibility.Collapsed;
            AdDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddAdvisor(selectedRow));
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)AdDataGrid.SelectedItem;
            int id = (int)selectedRow["ID"];
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select AdvisorId\r\nFrom ProjectAdvisor\r\nwhere AdvisorId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            List<string> IdList = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string s = dataTable.Rows[i]["AdvisorId"].ToString();
                IdList.Add(s);
            }
            if (IdList == null || IdList.Count == 0)
            {
              SqlCommand cmd1 = new SqlCommand("delete Advisor where Id = @id; delete Person where Id = @ID;");
            cmd1.Parameters.AddWithValue("@ID", id);
            cmd1.Connection = con;
            cmd1.ExecuteNonQuery();
            MessageBox.Show("Record Has Been Deleted Successfully...");
            refreshButton.Visibility= Visibility.Visible;
            }
            else
            {
                MessageBox.Show("This Advisor is associated with a group. Please remove it from there first");
            }
        }
    }
}
