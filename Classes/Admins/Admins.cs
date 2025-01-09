using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_TP
{
    public class Admins : Access
    {
        // Поля класса

        private Access a; 
        private TextBox tb; private Timer t; private List<Admins> admins; private TextBox q;
        private string name; public string Name { get => name; }
        public string login; public string Login { get => login; }
        public string password; public string Password { get => password; }
        string que; public string Question { get => que; }
        string ans; public string Answer { get => ans; }



        // Конструкторы класса

        public Admins(string name, string login, string password, string que, string ans)
        {
            this.name = name; this.password = password; this.login = login; this.que = que; this.ans = ans;
        }

        public Admins(TextBox tb, Timer t, List<Admins> admins, TextBox q)
        {
            this.tb = tb; this.t = t; this.admins = admins; this.q = q;
        }

        public Admins()
        {

        }



        // Методы интерфейса

        public void Log_in(Users user)
        {
            int k = 0;
            foreach (var v in admins)
            {
                if (v.login == user.Login && v.password == user.Password || v.ans == user.Answer && v.login == user.Login)
                {
                    k++; tb.Text = $"Добро пожаловать Администратор - {v.name}!"; tb.ForeColor = Color.SpringGreen; 
                    t.Enabled = true;
                    break;
                }
            }
            if (k == 0) a.Log_in(user);
        }

        public void ShowQuestion(Users user)
        {
            int k = 0;
            foreach (var v in admins)
            {
                if (v.login == user.Login)
                {
                    q.Text = "Контрольный вопрос: " + v.que; k++;
                    tb.Text = "Введите ответ на вопрос"; tb.ForeColor = Color.LightGreen;
                }
            }
            if (k == 0) a.ShowQuestion(user);
        }

        public void LowerAccess(Access access)
        {
            a = access;
        }
    }
}