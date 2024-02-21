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
    /// Interaction logic for MarkAEvaluation.xaml
    /// </summary>
    public partial class MarkAEvaluation : UserControl
    {
        string evalId;
        string grpId;
        public MarkAEvaluation()
        {
            InitializeComponent();
        }
        
        public MarkAEvaluation(DataRowView selectedRow)
        {
            InitializeComponent();
            txtBlockEvalInfo.Text = "Evaluation Information";
            txtBlockEvalInfo.Visibility = Visibility.Collapsed;
            txtBlockGrpInfo.Text = "Group Information";
            txtBlockGrpInfo.Visibility = Visibility.Collapsed;
            AddButton.Content = "Edit";
            GrpCombox.Visibility = Visibility.Collapsed;
            EvalCombox.Visibility = Visibility.Collapsed;
            AddGrpButton.Visibility = Visibility.Collapsed;
            obMarksTxtBox.Text = (string)selectedRow["obtainedMarks"].ToString();
            evalId = (string)selectedRow["EvaluationId"].ToString();
            grpId = (string)selectedRow["GroupId"].ToString();
        }
        private void GrpCombox_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("Select id \r\nfrom [dbo].[Group]\r\njoin GroupStudent\r\non id = GroupId\r\nwhere status = 3", con);
            cmd2.Connection = con;
            SqlDataReader sqlData2 = cmd2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(sqlData2);

            List<string> grpList = new List<string>();
            for (int i = 0; i < dataTable2.Rows.Count; i++)
            {
                string s = dataTable2.Rows[i]["Id"].ToString();
                grpList.Add(s);
            }
            GrpCombox.ItemsSource = grpList;
        }

        private void EvalCombox_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
                if (GrpCombox.Text != "" && evalId == null)
                {
                    string[] txt1 = EvalCombox.Text.Split('-');
                    string eID = txt1[0];
                    var con1 = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("Select TotalMarks From Evaluation Where id = @eId", con1);
                    cmd1.Parameters.AddWithValue("@eId", int.Parse(eID));
                    SqlDataReader sqlData = cmd1.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlData);
                    string s = "";
                    List<string> list = new List<string>();
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        s = dataTable.Rows[i]["TotalMarks"].ToString();
                    }
                MessageBox.Show("Total marks of this evaluation is " + s);
                    if (int.Parse(obMarksTxtBox.Text) < int.Parse(s) && int.Parse(obMarksTxtBox.Text) > 0)
                    {

                        string[] txt = EvalCombox.Text.Split('-');
                        evalId = txt[0];
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("insert into dbo.GroupEvaluation Values(@Gid, @Eid, @obtainMarks, @date);", con);
                        cmd.Parameters.AddWithValue("@Gid", int.Parse(GrpCombox.SelectedItem.ToString()));
                        cmd.Parameters.AddWithValue("@Eid", int.Parse(evalId));
                        cmd.Parameters.AddWithValue("@obtainMarks", int.Parse(obMarksTxtBox.Text));
                        cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully saved");
                        txtBlockEvalInfo.Visibility = Visibility.Collapsed;
                        container.Children.Add(new MarkEvaluationCrud());

                    }
                    else
                    {
                        MessageBox.Show("Please Enter a Value greater than 0 and less than " + s);
                    }
                }
                else
                {
                    string[] txt1 = EvalCombox.Text.Split('-');
                    string eID = txt1[0];
                    var con1 = Configuration.getInstance().getConnection();
                    SqlCommand cmd1 = new SqlCommand("Select TotalMarks\r\nFrom Evaluation\r\njoin GroupEvaluation\r\nON Id = EvaluationId\r\nWhere id = @eId", con1);
                    cmd1.Parameters.AddWithValue("@eId", int.Parse(evalId));
                    SqlDataReader sqlData = cmd1.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlData);
                    string s = "";
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        s = dataTable.Rows[i]["TotalMarks"].ToString();
                    }
                    if (int.Parse(obMarksTxtBox.Text) < int.Parse(s) && int.Parse(obMarksTxtBox.Text) > 0 && s != "")
                    {
                        var con = Configuration.getInstance().getConnection();
                        SqlCommand cmd = new SqlCommand("Update dbo.GroupEvaluation set ObtainedMarks = @obtainedMarks Where EvaluationId = @evalID and GroupId = @GrpID", con);
                        cmd.Parameters.AddWithValue("@obtainedMarks", int.Parse(obMarksTxtBox.Text));
                        cmd.Parameters.AddWithValue("@evalID", int.Parse(evalId));
                        cmd.Parameters.AddWithValue("@GrpID", int.Parse(grpId));
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Edited");
                        txtBlockEvalInfo.Visibility = Visibility.Collapsed;
                        container.Children.Add(new MarkEvaluationCrud());
                    }
                    else
                    {
                        MessageBox.Show("Please Enter a Value greater than 0 and less than " + s);
                    }
                }
            
            
        }

        private void AddGrpButton_Click(object sender, RoutedEventArgs e)
        {
            if (GrpCombox.Text != "")
            {
                EvalCombox.IsEnabled = true;
                AddevalButton.IsEnabled = true;
                grpId = GrpCombox.Text;
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("Select id , Name From Evaluation except(select id , Name From Evaluation join GroupEvaluation on id = EvaluationId where GroupId = @grpID)", con);
                cmd.Parameters.AddWithValue("@grpID", int.Parse(grpId));
                SqlDataReader sqlData = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(sqlData);

                List<string> evalList = new List<string>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    string s = dataTable.Rows[i]["Id"].ToString();
                    string s1 = (string)dataTable.Rows[i]["Name"];
                    s = s + "-" + s1;
                    evalList.Add(s);
                }
                EvalCombox.ItemsSource = evalList;
                EvalCombox.IsEnabled = true;
                MessageBox.Show("Group is selected");
            }

        }

        private void AddevalButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Evaluation is selected");
        }
    }
}
