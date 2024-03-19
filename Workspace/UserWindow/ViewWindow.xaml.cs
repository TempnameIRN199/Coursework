using Coursework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Word;

namespace Coursework.Workspace.UserWindow
{
    /// <summary>
    /// Логика взаимодействия для ViewWindow.xaml
    /// </summary>
    public partial class ViewWindow : System.Windows.Window
    {
        Student student;

        public ViewWindow(Ticket _ticket, Student _student)
        {
            InitializeComponent();
            if (_ticket != null)
            {
                LoadData(_ticket);
            }
            else
            {
                MessageBox.Show("Помилка завантаження даних");
                this.Close();
            }
            student = _student;
            _txt1.IsReadOnly = true;
            _txt2.IsReadOnly = true;
            _txt3.IsReadOnly = true;
        }

        private void LoadData(Ticket _ticket)
        {
            using (NintendoContext context = new NintendoContext())
            {
                var _issues = context.Issues.Where(x => x.TicketId == _ticket.Id).ToList();
                if (_issues.Count > 0)
                {
                    _txt1.Text = _issues[0].EssenceOfIssue;
                    if (_issues.Count > 1)
                    {
                        _txt2.Text = _issues[1].EssenceOfIssue;
                        if (_issues.Count > 2)
                        {
                            _txt3.Text = _issues[2].EssenceOfIssue;
                        }
                    }
                }
            }
        }

        private void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Document doc = app.Documents.Add();
            using (NintendoContext db = new NintendoContext())
            {
                var ticketName = db.Tickets.
                    Join(db.Issues, ticket => ticket.Id, issue => issue.TicketId, (ticket, issue) => new { ticket, issue }).
                    Where(x => x.issue.EssenceOfIssue == _txt1.Text || x.issue.EssenceOfIssue == _txt2.Text || x.issue.EssenceOfIssue == _txt3.Text).
                    Select(x => x.ticket.Name).
                    FirstOrDefault();

                var firstEssenceOfIssue = db.Issues.Where(x => x.EssenceOfIssue == _txt1.Text).FirstOrDefault();
                var secondEssenceOfIssue = db.Issues.Where(x => x.EssenceOfIssue == _txt2.Text).FirstOrDefault();
                var thirdEssenceOfIssue = db.Issues.Where(x => x.EssenceOfIssue == _txt3.Text).FirstOrDefault();

                var imageInIssue = db.Issues.Where(x => x.EssenceOfIssue == _txt1.Text).Select(x => x.ImagePath).FirstOrDefault();
                var imageInIssue2 = db.Issues.Where(x => x.EssenceOfIssue == _txt2.Text).Select(x => x.ImagePath).FirstOrDefault();
                var imageInIssue3 = db.Issues.Where(x => x.EssenceOfIssue == _txt3.Text).Select(x => x.ImagePath).FirstOrDefault();

                var studentName = db.Students.Where(x => x.UserId == student.UserId).Select(x => x.Name).FirstOrDefault();
                var studentSurname = db.Students.Where(x => x.UserId == student.UserId).Select(x => x.Surname).FirstOrDefault();
                var studentPatronymic = db.Students.Where(x => x.UserId == student.UserId).Select(x => x.Patronymic).FirstOrDefault();

                Microsoft.Office.Interop.Word.Table table = doc.Tables.Add(doc.Range(), 12, 1);
                table.Borders.Enable = 1;

                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleDouble;
                table.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth150pt;
                table.Borders.OutsideColor = WdColor.wdColorBrown;

                table.Cell(1, 1).Range.Text = "Білет:";
                table.Cell(2, 1).Range.Text = ticketName;
                table.Cell(3, 1).Range.Text = "Вопрос 1:";
                table.Cell(4, 1).Range.Text = _txt1.Text;
                table.Cell(5, 1).Range.Text = "Вопрос 2:";
                table.Cell(6, 1).Range.Text = _txt2.Text;
                table.Cell(7, 1).Range.Text = "Вопрос 3:";
                table.Cell(8, 1).Range.Text = _txt3.Text;
                table.Cell(9, 1).Range.Text = "Студент:";
                table.Cell(10, 1).Range.Text = studentName + " " + studentSurname + " " + studentPatronymic;
                table.Cell(11, 1).Range.Text = "Группа:";
                table.Cell(12, 1).Range.Text = student.Group.Name;
            }
            doc.SaveAs2(Environment.CurrentDirectory + "\\Report.docx");
            // відкриття документу
            app.Visible = true;
        }

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserWindow.User user = new UserWindow.User(student);
            user.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
