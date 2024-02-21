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
    /// Interaction logic for MarkEvaluationCrud.xaml
    /// </summary>
    public partial class MarkEvaluationCrud : UserControl
    {
        public MarkEvaluationCrud()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            addNewButton.Visibility = Visibility.Collapsed;
            MarkEvalDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new MarkAEvaluation());
        }

        private void MarkEvalDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select * from GroupEvaluation");
            cmd.Connection = con;
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            MarkEvalDataGrid.DataContext = dataTable;
            MarkEvalDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)MarkEvalDataGrid.SelectedItem;
            addNewButton.Visibility = Visibility.Collapsed;
            MarkEvalDataGrid.Visibility = Visibility.Collapsed;
            container.Children.Add(new MarkAEvaluation(selectedRow));
        }
    }
}
