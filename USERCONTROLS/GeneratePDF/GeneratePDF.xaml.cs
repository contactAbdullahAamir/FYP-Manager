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
using Microsoft.Win32;
using System.IO;
using System.Reflection.Metadata;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;



namespace MidTermProject.USERCONTROLS.GeneratePDF
{
    /// <summary>
    /// Interaction logic for GeneratePDF.xaml
    /// </summary>
    public partial class GeneratePDF : UserControl
    {
        public GeneratePDF()
        {
            InitializeComponent();
        }

        private void Gen1Pdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "PDF (*.pdf)|*.pdf";
            sv.FileName = "Report.pdf";
            // Create a new PDF document
            Nullable<bool> result =  sv.ShowDialog();
            if (result==true) {
                string str = sv.FileName;
            }
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            PdfWriter pdfw =  PdfWriter.GetInstance(document, new FileStream(sv.FileName, FileMode.Create));
            // Open the PDF document
            document.Open();

            // Adding the heading
            Chunk heading = new Chunk("Poject Report", new Font(Font.FontFamily.HELVETICA));
            iTextSharp.text.Paragraph headingParagraph = new iTextSharp.text.Paragraph(heading);
            headingParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(headingParagraph);

            string paragraphText = "\nThis is List of projects along with advisory board and list of students:\n\n ";
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(paragraphText);
            paragraph.Font = new Font(Font.FontFamily.HELVETICA);
            document.Add(paragraph);

            // Get Table from SQL Server using Required Query
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT  p.id AS project_id,  p.title AS project_title, \r\n    CONCAT(per1.firstname, ' ', per1.lastname) AS advisor_name, \r\n    l1.value AS advisor_designation, \r\n    l2.value AS advisor_role,\r\n    adv.salary AS advisor_salary, \r\n    s.id AS student_id, \r\n    CONCAT(per2.firstname, ' ', per2.lastname) AS student_name \r\nFROM \r\n    project AS p \r\n    INNER JOIN projectAdvisor AS pa ON p.id = pa.projectid \r\n    INNER JOIN advisor AS adv ON pa.advisorId = adv.id \r\n    INNER JOIN person AS per1 ON adv.id = per1.id \r\n    INNER JOIN lookup AS l1 ON adv.designation = l1.id \r\n    INNER JOIN lookup AS l2 ON pa.advisorRole = l2.id\r\n    INNER JOIN groupProject AS gp ON p.id = gp.ProjectId \r\n    INNER JOIN groupStudent AS gs ON gp.groupId = gs.groupId \r\n    INNER JOIN student AS s ON gs.studentId = s.id \r\n    INNER JOIN person AS per2 ON s.id = per2.id \r\nWHERE \r\n    l1.category = 'designation' AND l2.category = 'advisor_role'\r\nORDER BY \r\n    p.id, adv.id, s.id;", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Create a new table
            PdfPTable table = new PdfPTable(dt.Columns.Count);

            // Add table columns with the font
            foreach (DataColumn column in dt.Columns)
            {
                // Add the phrase to the cell and set background color
                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                table.AddCell(cell);
            }

            // Add table rows with the font
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    var cell = new PdfPCell(new Phrase(row[column].ToString()));
                    table.AddCell(cell);
                }
            }

            // Add the table to the PDF document
            document.Add(table);

            // Close the PDF document
            document.Close();

        }
        
        private void Gen2Pdf_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "PDF (*.pdf)|*.pdf";
            sv.FileName = "Report.pdf";
            // Create a new PDF document
            Nullable<bool> result = sv.ShowDialog();
            if (result == true)
            {
                string str = sv.FileName;
            }
            iTextSharp.text.Document document = new iTextSharp.text.Document();
            PdfWriter pdfw = PdfWriter.GetInstance(document, new FileStream(sv.FileName, FileMode.Create));
            // Open the PDF document
            document.Open();

            // Adding the heading
            Chunk heading = new Chunk("PDF Report", new Font(Font.FontFamily.HELVETICA));
            iTextSharp.text.Paragraph headingParagraph = new iTextSharp.text.Paragraph(heading);
            headingParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(headingParagraph);

            string paragraphText = "\nThis is Marks sheet of projects that shows the marks in each evaluation against each student and project.\n\n ";
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(paragraphText);
            paragraph.Font = new Font(Font.FontFamily.HELVETICA);
            document.Add(paragraph);

            // Get Table from SQL Server using Required Query
            var con = Configuration.getInstance().getConnection();
            SqlCommand cmd = new SqlCommand("SELECT \r\n p.id AS project_id, \r\n p.title AS project_title, \r\n s.id AS student_id, \r\n CONCAT(per.firstname, ' ', per.lastname) AS student_name,\r\n e.id AS evaluation_id,\r\n e.name AS evaluation_name,\r\n ge.obtainedmarks AS marks_obtained,\r\n SUM(ge.obtainedmarks) OVER(PARTITION BY p.id, s.id) AS total_marks\r\nFROM \r\n project AS p \r\n INNER JOIN groupProject AS gp ON p.id = gp.ProjectId \r\n INNER JOIN groupStudent AS gs ON gp.groupId = gs.groupId \r\n INNER JOIN student AS s ON gs.studentId = s.id \r\n INNER JOIN person AS per ON s.id = per.id \r\n INNER JOIN GroupEvaluation AS ge ON gp.groupId = ge.groupid \r\n INNER JOIN evaluation AS e ON ge.evaluationID = e.id\r\nORDER BY \r\n p.id, s.id, e.id;", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Create a new table
            PdfPTable table = new PdfPTable(dt.Columns.Count);

            // Add table columns with the font
            foreach (DataColumn column in dt.Columns)
            {
                // Add the phrase to the cell and set background color
                PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName));
                table.AddCell(cell);
            }

            // Add table rows with the font
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    var cell = new PdfPCell(new Phrase(row[column].ToString()));
                    table.AddCell(cell);
                }
            }

            // Add the table to the PDF document
            document.Add(table);

            // Close the PDF document
            document.Close();
        }
    }
}
