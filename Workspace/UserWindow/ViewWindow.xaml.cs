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

        }

        private void _btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserWindow.User user = new UserWindow.User(student);
            user.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Hidden;
        }
    }
}
