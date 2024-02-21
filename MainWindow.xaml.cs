using MidTermProject.USERCONTROLS;
using MidTermProject.USERCONTROLS.Advisor;
using MidTermProject.USERCONTROLS.Evaluation;
using MidTermProject.USERCONTROLS.GeneratePDF;
using MidTermProject.USERCONTROLS.Group;
using MidTermProject.USERCONTROLS.Project;
using System;
using System.Collections.Generic;
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

namespace MidTermProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else if (WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ManageStBtn_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new StudentCRUD());
        }

        private void ManageAdBtn_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new AdvisorCRUD());
        }

        private void ManageProjButton_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new ProjectCrud());
        }

        private void ManageEvalButton_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new EvaluationCrud());
        }

        private void ManageGrpButton_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new GroupCrud());
        }

        private void MarkEvalButton_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new MarkEvaluationCrud());
        }

        private void PDFs_Button_Click(object sender, RoutedEventArgs e)
        {
            container.Children.Clear();
            container.Children.Add(new GeneratePDF());
        }
    }
}
