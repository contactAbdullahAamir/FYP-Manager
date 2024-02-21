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
using System.Text.RegularExpressions;
using System.Security.Policy;

namespace MidTermProject.USERCONTROLS.Group
{
    /// <summary>
    /// Interaction logic for EditGroup.xaml
    /// </summary>
    public partial class EditGroup : UserControl
    {
        string mainAdvisorID = null;
        string coAdvisorID = null;
        string industryAdvisorID = null;
        string projId = null;
        int count;
        int grpid;
        public EditGroup(int id)
        {
            InitializeComponent();
            this.grpid = id;
        }

        private void StDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select S.Id, GS.GroupId, S.RegistrationNo, P.FirstName, P.LastName,  GS.Status, GS.AssignmentDate  \r\nFROM Student as S\r\nJOIN Person As P\r\nON P.Id = S.Id\r\nJOIN GroupStudent as GS\r\nON GS.StudentId = S.Id\r\nWhere GS.Status = 3 And GS.GroupId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", grpid);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            StDataGrid.DataContext = dataTable;
            StDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void removeStButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)StDataGrid.SelectedItem;
                int sTid = (int)selectedRow["ID"];
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("update GroupStudent set Status = 4 where GroupId = @ID AND StudentId = @SID", con);
                cmd.Parameters.AddWithValue("@ID", grpid);
                cmd.Parameters.AddWithValue("@SID", sTid);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student has been removed...");
                StrefreshButton.Visibility = Visibility.Visible;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("select A.Id, FirstName\r\nfrom Advisor as A\r\njoin Person as P\r\non P.Id = A.Id", con);
            cmd1.Connection = con;
            SqlDataReader sqlData = cmd1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(sqlData);

            List<string> advList = new List<string>();
            for (int i = 0; i < dataTable1.Rows.Count; i++)
            {
                string s = dataTable1.Rows[i]["Id"].ToString();
                string s1 = (string)dataTable1.Rows[i]["FirstName"];
                s = s + "-" + s1;
                advList.Add(s);
            }
            mainAdComBox.ItemsSource = advList;
        }

        private void StrefreshButton_Click(object sender, RoutedEventArgs e)
        {
            StDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("Select S.Id, GS.GroupId, S.RegistrationNo, P.FirstName, P.LastName,  GS.Status, GS.AssignmentDate  \r\nFROM Student as S\r\nJOIN Person As P\r\nON P.Id = S.Id\r\nJOIN GroupStudent as GS\r\nON GS.StudentId = S.Id\r\nWhere GS.Status = 3 And GS.GroupId = @ID", con);
            cmd.Parameters.AddWithValue("@ID", grpid);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            StDataGrid.DataContext = dataTable;
            StDataGrid.ItemsSource = dataTable.DefaultView;
            StrefreshButton.Visibility= Visibility.Hidden;

            NewStDataGrid.ItemsSource = null;
            var con1 = Configuration.getInstance().getConnection();
            SqlCommand cmd1 = new SqlCommand("select S.Id, S.RegistrationNo , FirstName , LastName , Contact , Email\r\nfrom Student as S\r\njoin Person as P\r\non S.Id = P.Id EXCEPT select S.Id , S.RegistrationNo , P.FirstName , P.LastName , P.Contact , P.Email\r\nfrom [dbo].[Group] as G\r\njoin [dbo].GroupStudent as Gs\r\non Gs.GroupId = G.Id\r\njoin Lookup as L\r\non L.Id = Gs.Status\r\njoin Person as P\r\non P.Id = Gs.StudentId\r\njoin Student as S\r\non S.Id = P.Id\r\nwhere L.Value = 'Active'");
            cmd1.Connection = con1;
            SqlDataReader sqlData1 = cmd.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable.Load(sqlData1);
            NewStDataGrid.DataContext = dataTable1;
            NewStDataGrid.ItemsSource = dataTable1.DefaultView;
            StnewrefreshButton.Visibility = Visibility.Collapsed;
        }

        

        private void AdvAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainAdvisorID == null && projId != null)
                {
                    string[] txt = mainAdComBox.Text.Split('-');
                    mainAdvisorID = txt[0];
                    List<string> lst = new List<string>();
                    lst = (List<string>)mainAdComBox.ItemsSource;
                    lst.RemoveAt(mainAdComBox.SelectedIndex);
                    CoAdComBox.ItemsSource = lst;
                    CoAdComBox.IsEnabled = true;
                    mainAdComBox.IsEnabled = false;
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand ProjectAdvisor = new SqlCommand("update [dbo].[ProjectAdvisor] set AdvisorId = @AdvisorId where ProjectId = (select ProjectId from GroupProject where groupid = @id) and AdvisorRole = 11\r\n", con);
                    ProjectAdvisor.Parameters.AddWithValue("@AdvisorId", mainAdvisorID);
                    ProjectAdvisor.Parameters.AddWithValue("@id", grpid);
                    ProjectAdvisor.ExecuteNonQuery();
                    MessageBox.Show("main advisor is edited sucessfully");
                    EditButton.Visibility = Visibility.Collapsed;
                    AdvAddButton.Content = "Select Co Advisor";
                    count = 1;
                }
                else if (count == 1 && coAdvisorID == null && projId != null)
                {
                    string[] txt = CoAdComBox.Text.Split('-');
                    coAdvisorID = txt[0];
                    List<string> lst = new List<string>();
                    lst = (List<string>)CoAdComBox.ItemsSource;
                    lst.RemoveAt(CoAdComBox.SelectedIndex);
                    IndusAdComBox.ItemsSource = lst;
                    IndusAdComBox.IsEnabled = true;
                    CoAdComBox.IsEnabled = false;
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand ProjectAdvisor = new SqlCommand("update [dbo].[ProjectAdvisor] set AdvisorId = @AdvisorId where ProjectId = (select ProjectId from GroupProject where groupid = @id) and AdvisorRole = 12\r\n", con);
                    ProjectAdvisor.Parameters.AddWithValue("@AdvisorId", coAdvisorID);
                    ProjectAdvisor.Parameters.AddWithValue("@id", grpid);
                    ProjectAdvisor.ExecuteNonQuery();
                    MessageBox.Show("Co advisor is added sucessfully");
                    AdvAddButton.Content = "Select Industry Advisor";
                    count = 2;
                }
                else if (count == 2 && industryAdvisorID == null && projId != null)
                {
                    string[] txt = IndusAdComBox.Text.Split('-');
                    industryAdvisorID = txt[0];
                    IndusAdComBox.IsEnabled = false;
                    AdvAddButton.IsEnabled = false;
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand ProjectAdvisor = new SqlCommand("update [dbo].[ProjectAdvisor] set AdvisorId = @AdvisorId where ProjectId = (select ProjectId from GroupProject where groupid = @id) and AdvisorRole = 14\r\n", con);
                    ProjectAdvisor.Parameters.AddWithValue("@AdvisorId", industryAdvisorID);
                    ProjectAdvisor.Parameters.AddWithValue("@id", grpid);
                    ProjectAdvisor.ExecuteNonQuery();
                    MessageBox.Show("Industrial advisor is added sucessfully");
                    EditButton.Visibility = Visibility.Visible;
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ProjAddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (projId == null)
                {
                    string[] txt = projCombox.Text.Split('-');
                    projId = txt[0];
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand ProjectGroup = new SqlCommand("update [dbo].[GroupProject] set ProjectId = @ProjectId where GroupId  = @id\r\n", con);
                    ProjectGroup.Parameters.AddWithValue("@ProjectId", projId);
                    ProjectGroup.Parameters.AddWithValue("@id", grpid);
                    ProjectGroup.ExecuteNonQuery();
                    MessageBox.Show("Project is Added Sucessfully");
                    ProjAddButton.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }

        private void projCombox_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd2 = new SqlCommand("select Id , Title from Project EXCEPT select P.Id , P.Title\r\nfrom [dbo].[Group] as G\r\njoin GroupProject as Gp\r\non G.Id = Gp.GroupId\r\njoin Project as P\r\non P.Id = Gp.ProjectId", con);
            cmd2.Connection = con;
            SqlDataReader sqlData2 = cmd2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(sqlData2);

            List<string> projList = new List<string>();
            for (int i = 0; i < dataTable2.Rows.Count; i++)
            {
                string s = dataTable2.Rows[i]["Id"].ToString();
                string s1 = (string)dataTable2.Rows[i]["Title"];
                s = s + "-" + s1;
                projList.Add(s);
            }
            projCombox.ItemsSource = projList;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(mainAdComBox.Text == "" || CoAdComBox.Text == "" || IndusAdComBox.Text == "" || projCombox.Text == "")
            {
                MessageBox.Show("Please select all the options first");
            }
            else
            {
                ProjAddButton.Visibility = Visibility.Collapsed;
                txtBlockStInfo.Visibility = Visibility.Collapsed;
                txtBlockProjInfo.Visibility = Visibility.Collapsed;
                AddButton.Visibility = Visibility.Collapsed;
                AdvAddButton.Visibility = Visibility.Collapsed;
                container.Children.Add(new GroupCrud());
            }
        }

        private void NewStDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select S.Id, S.RegistrationNo , FirstName , LastName , Contact , Email\r\nfrom Student as S\r\njoin Person as P\r\non S.Id = P.Id EXCEPT select S.Id , S.RegistrationNo , P.FirstName , P.LastName , P.Contact , P.Email\r\nfrom [dbo].[Group] as G\r\njoin [dbo].GroupStudent as Gs\r\non Gs.GroupId = G.Id\r\njoin Lookup as L\r\non L.Id = Gs.Status\r\njoin Person as P\r\non P.Id = Gs.StudentId\r\njoin Student as S\r\non S.Id = P.Id\r\nwhere L.Value = 'Active'");
            cmd.Connection = con;
            SqlDataReader sqlData1 = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData1);
            NewStDataGrid.DataContext = dataTable;
            NewStDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewStDataGrid.Visibility = Visibility.Visible;
            remButton.Visibility = Visibility.Visible;

            AddButton.Visibility = Visibility.Collapsed;
            StDataGrid.Visibility = Visibility.Collapsed;


        }

        private void remButton_Click(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Visible;
            StDataGrid.Visibility = Visibility.Visible;

            NewStDataGrid.Visibility = Visibility.Collapsed;
            remButton.Visibility = Visibility.Collapsed;
        }

        private void addStButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRowView selectedRow = (DataRowView)NewStDataGrid.SelectedItem;
                int stid = (int)selectedRow["ID"];
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("insert into [dbo].[GroupStudent](GroupId , StudentId , Status ,AssignmentDate) values (@grpID , @ID , 3, @Date)");
                cmd.Parameters.AddWithValue("@grpID", grpid);
                cmd.Parameters.AddWithValue("@id", stid);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Student is added Successfully...");
                StrefreshButton.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                string sbstr = ex.ToString()[..4];
                if (sbstr == "Syst")
                {
                    DataRowView selectedRow = (DataRowView)NewStDataGrid.SelectedItem;
                    int stid = (int)selectedRow["ID"];
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("update [dbo].[GroupStudent] set status = 3 where GroupId = @grpID And StudentId = @ID");
                    cmd.Parameters.AddWithValue("@grpID", grpid);
                    cmd.Parameters.AddWithValue("@id", stid);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Student is added Successfully...");
                    StrefreshButton.Visibility = Visibility.Visible;

                }
                else
                {
                    MessageBox.Show(sbstr);
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void StnewrefreshButton_Click(object sender, RoutedEventArgs e)
        {
            NewStDataGrid.ItemsSource = null;
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select S.Id, S.RegistrationNo , FirstName , LastName , Contact , Email\r\nfrom Student as S\r\njoin Person as P\r\non S.Id = P.Id EXCEPT select S.Id , S.RegistrationNo , P.FirstName , P.LastName , P.Contact , P.Email\r\nfrom [dbo].[Group] as G\r\njoin [dbo].GroupStudent as Gs\r\non Gs.GroupId = G.Id\r\njoin Lookup as L\r\non L.Id = Gs.Status\r\njoin Person as P\r\non P.Id = Gs.StudentId\r\njoin Student as S\r\non S.Id = P.Id\r\nwhere L.Value = 'Active'");
            cmd.Connection = con;
            SqlDataReader sqlData1 = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData1);
            NewStDataGrid.DataContext = dataTable;
            NewStDataGrid.ItemsSource = dataTable.DefaultView;
            StnewrefreshButton.Visibility = Visibility.Collapsed;
        }
    }
}
