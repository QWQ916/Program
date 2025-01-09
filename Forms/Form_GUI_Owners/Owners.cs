using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_TP
{
    public class Owners : Access
    {
        // Поля класса

        private Access a;
        private TextBox tb; private Timer t; private List<Owners> owners; private TextBox q;
        public string surname; public string name;
        public string login; public string password;
        string que; public string ans;
       


        // Конструкторы класса

        public Owners(string surname, string name, string login, string password, string que, string ans)
        {
            this.surname = surname; this.name = name; this.login = login; this.password = password; this.ans = ans; this.que = que;
        }

        public Owners(TextBox tb, Timer t, List<Owners> owners, TextBox q)
        {
            this.tb = tb; this.t = t; this.owners = owners; this.q = q;
        }

        public Owners()
        {

        }



        // Методы интерфейса

        public void Log_in(Users user)
        {
            int k = 0;
            foreach (var v in owners)
            {
                if (v.login == user.Login && v.password == user.Password || v.login == user.Login && v.ans == user.Answer)
                { 
                    k++; tb.Text = $"Добро пожаловать Владелец - {v.name}!"; tb.ForeColor = Color.LawnGreen;
                    t.Enabled = true;
                    break;
                }
            }
            if (k == 0) a.Log_in(user);
        }
        public void LowerAccess(Access access)
        {
            a = access;
        }

        public void ShowQuestion(Users user)
        {
            int k = 0;
            foreach (var v in owners)
            {
                if (v.login == user.Login)
                {
                    q.Text = "Контрольный вопрос: " + v.que; k++;
                    tb.Text = "Введите ответ на вопрос"; tb.ForeColor = Color.LightGreen;
                }
            }
            if (k == 0) a.ShowQuestion(user);
        }

        
    }
}
