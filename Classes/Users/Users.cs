using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_TP
{
    public class Users : Access
    {
        // Поля класса

        private Access a; 
        private TextBox tb; private Timer t; private List<Users> users; private TextBox q;
        string login; public string Login { get => login; }
        string password; public string Password { get => password; }
        string name; public string Name { get => name; }
        string que; public string Question { get => que; } 
        string ans; public string Answer { get => ans; }

        

        // Конструкторы класса

        public Users(string name, string login, string password, string que, string ans)
        {
            this.name = name; this.login = login; this.password = password; this.que = que; this.ans = ans;
        }

        public Users(TextBox tb, Timer t, List<Users> users, TextBox q)
        {
            this.tb = tb; this.t = t; this.users = users; this.q = q;
        }
        
        public Users()
        {

        }



        // Методы интерфейса

        public void Log_in(Users user)
        {
            int k = 0;
            foreach (var v in users)
            {
                if (v == user)
                {
                    tb.ForeColor = Color.GreenYellow; tb.Text = $"Добро пожаловать Пользователь - {v.name}!"; k++;
                    t.Enabled = true;
                    break;
                }
            }
            if (k == 0) { tb.Text = "Данный пользователь не найден! Попробуйте перепроверить информацию!"; tb.ForeColor = Color.IndianRed; }
        }
        public void LowerAccess(Access access)
        {
            a = access;
        }

        public void ShowQuestion(Users user)
        {
            int k = 0;
            foreach (var v in users)
            {
                if (v.login == user.Login)
                {
                    q.Text = "Контрольный вопрос: " + v.que; k++;
                    tb.Text = "Введите ответ на вопрос"; tb.ForeColor = Color.LightGreen; 
                }
            }
            if (k == 0) tb.Text = "Пользователь не найден!"; tb.ForeColor = Color.IndianRed;
        }

        



        // Переопределение операторов 

        public static bool operator ==(Users a, Users b)
        {
            return a.login == b.login && (a.password == b.password || a.ans == b.ans);
        }
        public static bool operator !=(Users a, Users b)
        {
            return a.login != b.login || (a.password != b.password && a.ans != b.ans);
        }
    }
}
