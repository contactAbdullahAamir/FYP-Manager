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

namespace MidTermProject.USERCONTROLS.Project
{
    /// <summary>
    /// Interaction logic for ProjectCrud.xaml
    /// </summary>
    public partial class ProjectCrud : UserControl
    {
        public ProjectCrud()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addButton.Visibility = Visibility.Collapsed;
            ProjDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddProject());
        }

        private void ProjDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id, Title, Description from Project", con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            ProjDataGrid.DataContext = dataTable;
            ProjDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ProjDataGrid.SelectedItem;
            addButton.Visibility = Visibility.Collapsed;
            ProjDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddProject(selectedRow));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ProjDataGrid.SelectedItem;
            int id = (int)selectedRow["ID"];
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select ProjectId\r\nFrom GroupProject\r\nwhere ProjectId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            List<string> IdList = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string s = dataTable.Rows[i]["ProjectId"].ToString();
                IdList.Add(s);
            }
            if (IdList == null || IdList.Count == 0)
            {
                SqlCommand cmd1 = new SqlCommand("delete Project where Id = @id;");
            cmd1.Parameters.AddWithValue("@id", id);
            cmd1.Connection = con;
            cmd1.ExecuteNonQuery();
            MessageBox.Show("Record Has Been Deleted Successfully...");
            refreshButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("This Project is associated with a group. Please remove it from there first");
            }
        }
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            ProjDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select Id, Title, Description from Project", con);
            SqlDataReader sqlDataReader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlDataReader);
            ProjDataGrid.DataContext = dataTable;
            ProjDataGrid.ItemsSource = dataTable.DefaultView;
            refreshButton.Visibility = Visibility.Hidden;
        }

        private void container_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
