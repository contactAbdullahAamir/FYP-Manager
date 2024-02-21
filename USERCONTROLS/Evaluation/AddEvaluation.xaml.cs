using MidTermProject.USERCONTROLS.Project;
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
using System.Xml.Linq;

namespace MidTermProject.USERCONTROLS.Evaluation
{
    /// <summary>
    /// Interaction logic for AddEvaluation.xaml
    /// </summary>
    public partial class AddEvaluation : UserControl
    {
        int id;
        public AddEvaluation()
        {
            InitializeComponent();
        }
        
        public AddEvaluation(DataRowView selectedRow)
        {
            InitializeComponent();
            AddButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            EvalNameTxtBox.Text = (string)selectedRow["Name"];
            EvalTMarksTxtBox.Text = (string)selectedRow["TotalMarks"].ToString();
            EvalTWeightageTxtBox.Text = (string)selectedRow["TotalWeightage"].ToString();
            id = (int)selectedRow["id"];
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (EvalNameTxtBox.Text == "" || EvalTMarksTxtBox.Text == "" || EvalTWeightageTxtBox.Text == "")
            {
                MessageBox.Show("Please fill all Queries First");
            }
            else
            {
                if ((int.Parse(EvalTMarksTxtBox.Text) <= 100 && int.Parse(EvalTMarksTxtBox.Text) >= 0) && (int.Parse(EvalTWeightageTxtBox.Text) <= 100 && int.Parse(EvalTWeightageTxtBox.Text) >= 0))
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("insert into [dbo].[Evaluation] (Name , TotalMarks , TotalWeightage) values (@Name , @TotalMarks , @TotalWeightage)", con);
                    cmd.Parameters.AddWithValue("@Name", EvalNameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@TotalMarks", EvalTMarksTxtBox.Text);
                    cmd.Parameters.AddWithValue("@TotalWeightage", EvalTWeightageTxtBox.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully saved");
                    txtBlockEvalInfo.Visibility = Visibility.Collapsed;
                    container.Children.Add(new EvaluationCrud());
                }
                else
                {
                    MessageBox.Show("Please Enter a Valid Mark or Weightage");
                }
                
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (EvalNameTxtBox.Text == "" || EvalTMarksTxtBox.Text == "" || EvalTWeightageTxtBox.Text == "")
            {
                MessageBox.Show("Please fill all Queries First");
            }
            else
            {
                if ((int.Parse(EvalTMarksTxtBox.Text) <= 100 && int.Parse(EvalTMarksTxtBox.Text) >= 0) && (int.Parse(EvalTWeightageTxtBox.Text) <= 100 && int.Parse(EvalTWeightageTxtBox.Text) >= 0))
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("update [dbo].[Evaluation] set Name = @Name , TotalMarks = @TotalMarks , TotalWeightage = @TotalWeightage where Id = @ID", con);
                    cmd.Parameters.AddWithValue("@Name", EvalNameTxtBox.Text);
                    cmd.Parameters.AddWithValue("@TotalMarks", EvalTMarksTxtBox.Text);
                    cmd.Parameters.AddWithValue("@TotalWeightage", EvalTWeightageTxtBox.Text);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully Updated");
                    txtBlockEvalInfo.Visibility = Visibility.Collapsed;
                    container.Children.Add(new EvaluationCrud());
                }
                else
                {
                    MessageBox.Show("Please Enter a Valid Mark or Weightage");
                }
            }

        }
    }
    
}
