using MidTermProject.USERCONTROLS.Advisor;
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

namespace MidTermProject.USERCONTROLS.Project
{
    /// <summary>
    /// Interaction logic for AddProject.xaml
    /// </summary>
    public partial class AddProject : UserControl
    {
        int id;
        public AddProject()
        {
            InitializeComponent();
        }

        public AddProject(DataRowView selectedRow)
        {
            InitializeComponent();
            AddButton.Visibility = Visibility.Hidden;
            EditButton.Visibility = Visibility.Visible;
            ProjTitleTxtBox.Text = (string)selectedRow["Title"];
            projDesTxtBox.Text = (string)selectedRow["Description"];
            id = (int)selectedRow["id"];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjTitleTxtBox.Text != "" || projDesTxtBox.Text != "")
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("INSERT INTO Project(Title , Description) values (@Title, @Description)", con);
                cmd.Parameters.AddWithValue("@Title", ProjTitleTxtBox.Text);
                cmd.Parameters.AddWithValue("@Description", projDesTxtBox.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully saved");

                txtBlockProjInfo.Visibility = Visibility.Collapsed;
                container.Children.Add(new ProjectCrud());
            }
            else
            {
                MessageBox.Show("Please Fill all the Queries");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjTitleTxtBox.Text != "" || projDesTxtBox.Text != "")
            {
                var con = Configuration.getInstance().getConnection();
                SqlCommand cmd = new SqlCommand("update [dbo].[Project] set Title = @Title, Description = @Description where id = @ID", con);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Title", ProjTitleTxtBox.Text);
                cmd.Parameters.AddWithValue("@Description", projDesTxtBox.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Successfully Updated");
                txtBlockProjInfo.Visibility = Visibility.Collapsed;
                container.Children.Add(new ProjectCrud());
            }
            else
            {
                MessageBox.Show("Please Fill all the Queries");
            }
        }
    }
}
