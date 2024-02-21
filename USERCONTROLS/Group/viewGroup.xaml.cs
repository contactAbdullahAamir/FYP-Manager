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

namespace MidTermProject.USERCONTROLS.Group
{
    /// <summary>
    /// Interaction logic for viewGroup.xaml
    /// </summary>
    public partial class viewGroup : UserControl
    {
        int id;
        public viewGroup(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            txtBlockAdInfo.Visibility= Visibility.Collapsed;
            txtBlockProjInfo.Visibility= Visibility.Collapsed;
            txtBlockStInfo.Visibility= Visibility.Collapsed;
            StDataGrid.Visibility= Visibility.Collapsed;
            ProjDataGrid.Visibility= Visibility.Collapsed;
            AdDataGrid.Visibility= Visibility.Collapsed;   
            container.Children.Add(new GroupCrud());
        }

        private void StDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select A2.RegistrationNo , A2.FirstName , A2.LastName , A2.Contact , A2.Email , A2.Gender , L.Value as Status , A2.AssignmentDate\r\nfrom Lookup as L\r\njoin (select A.RegistrationNo , A.FirstName , A.LastName , A.Contact , A.Email , L.Value as Gender , A.Status , A.AssignmentDate\r\nfrom Lookup as L\r\njoin (select S.RegistrationNo , P.FirstName , P.LastName , P.Contact , P.Email , P.Gender , Gs.Status , Gs.AssignmentDate\r\nfrom [dbo].[Group] as G\r\njoin [dbo].[GroupStudent] as Gs\r\non G.Id = Gs.GroupId\r\njoin Student as S\r\non S.Id = Gs.StudentId\r\njoin Person as P\r\non P.Id = S.Id\r\nwhere G.Id = @ID\r\ngroup by S.RegistrationNo , P.FirstName , P.LastName , P.Contact , P.Email , P.Gender , Gs.Status , Gs.AssignmentDate) as A\r\non A.Gender = L.Id) as A2\r\non A2.Status = L.Id", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            StDataGrid.DataContext = dataTable;
            StDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void ProjDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select P.Id, P.Title , P.Description\r\nfrom [dbo].[Group] as G\r\njoin GroupProject as Gp\r\non G.Id = Gp.GroupId\r\njoin Project as P\r\non P.Id = Gp.ProjectId\r\nwhere G.Id = @ID", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            ProjDataGrid.DataContext = dataTable;
            ProjDataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void AdDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("select P.FirstName , P.LastName, A1.Value as Role\r\nfrom Person as P\r\njoin (select A.AdvisorId , A.AdvisorRole , L.Value\r\nfrom Lookup as L\r\njoin (select Pa.AdvisorId , Pa.AdvisorRole\r\nfrom [dbo].[Group] as G\r\njoin GroupProject as Gp\r\non G.Id = Gp.GroupId\r\njoin Project as P\r\non P.Id = Gp.ProjectId\r\njoin ProjectAdvisor as Pa\r\non Pa.ProjectId = P.Id\r\nwhere G.Id = @ID) as A\r\non A.AdvisorRole = L.Id) as A1\r\non A1.AdvisorId = P.Id", con);
            cmd.Parameters.AddWithValue("@ID", id);
            SqlDataReader sqlData = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(sqlData);
            AdDataGrid.DataContext = dataTable;
            AdDataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
