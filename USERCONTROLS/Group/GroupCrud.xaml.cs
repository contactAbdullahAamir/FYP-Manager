using System;
using System.Collections.Generic;
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

namespace MidTermProject.USERCONTROLS.Group
{
    /// <summary>
    /// Interaction logic for GroupCrud.xaml
    /// </summary>
    public partial class GroupCrud : UserControl
    {
        public GroupCrud()
        {
            InitializeComponent();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            GrpDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select * from [dbo].[Group]");
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            GrpDataGrid.DataContext = dataTable;
            GrpDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addButton.Visibility = Visibility.Collapsed;
            GrpDataGrid.Visibility = Visibility.Collapsed;
            var con = Configuration.getInstance().getConnection();
            SqlCommand group = new SqlCommand("insert into [dbo].[Group](Created_On) values (@Date)", con);
            group.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
            group.ExecuteNonQuery();
            container.Children.Add(new AddGroup());
        }

        private void container_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void GrpDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select * from [dbo].[Group]", con);
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            GrpDataGrid.DataContext = dataTable;
            GrpDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)GrpDataGrid.SelectedItem;
            int id = (int)selectedRow["ID"];
            addButton.Visibility = Visibility.Collapsed;
            GrpDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new EditGroup(id));
        }

        private void View_Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)GrpDataGrid.SelectedItem;
            int id = (int)selectedRow["ID"];
            
            addButton.Visibility = Visibility.Collapsed;
            GrpDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new viewGroup(id));
        }
    }
}
