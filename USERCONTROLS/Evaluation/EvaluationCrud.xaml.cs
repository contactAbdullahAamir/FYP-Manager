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

namespace MidTermProject.USERCONTROLS.Evaluation
{
    /// <summary>
    /// Interaction logic for EvaluationCrud.xaml
    /// </summary>
    public partial class EvaluationCrud : UserControl
    {
        public EvaluationCrud()
        {
            InitializeComponent();
        }

        private void EvalDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select * from Evaluation");
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            EvalDataGrid.DataContext = dataTable;
            EvalDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)EvalDataGrid.SelectedItem;
            addButton.Visibility = Visibility.Collapsed;
            EvalDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddEvaluation(selectedRow));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)EvalDataGrid.SelectedItem;
            int id = (int)selectedRow["ID"];
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select EvaluationId\r\nFrom GroupEvaluation\r\nwhere EvaluationId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            List<string> IdList = new List<string>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                string s = dataTable.Rows[i]["EvaluationId"].ToString();
                IdList.Add(s);
            }
            if (IdList == null || IdList.Count == 0)
            {
                SqlCommand cmd1 = new SqlCommand("delete [dbo].[Evaluation] where Id = @id;");
                cmd1.Parameters.AddWithValue("@id", id);
                cmd1.Connection = con;
                cmd1.ExecuteNonQuery();
                MessageBox.Show("Record Has Been Deleted Successfully...");
                refreshButton.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("This Evaluation is associated with a group. Please remove it from there first");
            }
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            EvalDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select * from Evaluation");
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            EvalDataGrid.DataContext = dataTable;
            EvalDataGrid.ItemsSource = dataTable.DefaultView;
            refreshButton.Visibility = Visibility.Hidden;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addButton.Visibility = Visibility.Collapsed;
            EvalDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new AddEvaluation());
        }

        private void container_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
