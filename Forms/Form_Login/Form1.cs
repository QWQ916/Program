using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace Project_TP
{
    public partial class Autoriz : Form
    {
        // Загрузка формы 1

        public Autoriz()
        {
            InitializeComponent();
        }

        public List<Owners> List_owners = new List<Owners>();
        public List<Admins> List_admins = new List<Admins>();
        public List<Users> List_users = new List<Users>();

        private void Autoriz_Load(object sender, EventArgs e)
        {
            SQLiteConnection con1 = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con1.Open();
            var com1 = con1.CreateCommand();
            com1.CommandText = @"SELECT Surname, Name, Login, Password, Question, Answer FROM Owners";
            using (var v = com1.ExecuteReader())
            {
                while (v.Read())
                {
                    List_owners.Add(new Owners(v.GetString(0), v.GetString(1), v.GetString(2), v.GetString(3), v.GetString(4), v.GetString(5)));
                }
            }
            con1.Close();

            SQLiteConnection con2 = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con2.Open();
            var com2 = con2.CreateCommand();
            com2.CommandText = @"SELECT Name, Login, Password, Question, Answer FROM Admins";
            using (var v = com2.ExecuteReader())
            {
                while (v.Read())
                {
                    List_admins.Add(new Admins(v.GetString(0), v.GetString(1), v.GetString(2), v.GetString(3), v.GetString(4)));
                }
            }
            con2.Close();

            SQLiteConnection con3 = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con3.Open();
            var com3 = con3.CreateCommand();
            com3.CommandText = @"SELECT Name, Login, Password, Question, Answer FROM Users";
            using (var v = com3.ExecuteReader())
            {
                while (v.Read())
                {
                    List_users.Add(new Users(v.GetString(0), v.GetString(1), v.GetString(2), v.GetString(3), v.GetString(4)));
                }
            }
            con3.Close();
        }



        // Методы взаимодействия с Элементами

        public void Default_autoriz(bool x)
        {
            but_forgotpass.Enabled = x; but_forgotpass.Visible = x; but_enter.Enabled = x; but_enter.Visible = x; but_try.Visible = !x; but_try.Enabled = !x;
            but_back.Enabled = !x; but_back.Visible = !x; tb_password.Visible = x; tb_password.Enabled = x; tb_pass.Visible = x; tb_pass.Enabled = x; tb_password.Clear(); tb_login.Clear();
            if (x) { tb_messenter.Text = "Введите логин и пароль для входа"; tb_pass.Text = "Пароль"; tb_pass.BackColor = Color.Aquamarine; }
            else { tb_messenter.Text = "Введите Ваш логин"; tb_pass.Text = "Ответ"; tb_pass.BackColor = Color.MediumSeaGreen; }
        }

        public void Show_Question(bool x)
        {
            tb_question.Visible = x; tb_question.Enabled = x;
        }

        public void ButSignIn(bool x)
        {
            but_signin.Visible = x; but_signin.Enabled = x;
        }



        // События "кнопки"

        int k = 0;
        string logincurrent = "";
        string passwordcurrent = "";
        string anscurrent = "";

        private void but_enter_Click(object sender, EventArgs e)
        {
            string login = tb_login.Text; 
            string password = tb_password.Text;
            if (login != "" && password != "")
            {
                Access Role1 = new Owners(tb_messenter, before_enter, List_owners, tb_question);
                Access Role2 = new Admins(tb_messenter, before_enter_admin, List_admins, tb_question);
                Access Role3 = new Users(tb_messenter, before_enter_user, List_users, tb_question);

                Role1.LowerAccess(Role2);
                Role2.LowerAccess(Role3);

                Users input = new Users("", login, password, "", "");
                Role1.Log_in(input);
                logincurrent = login;
                passwordcurrent = password; 
            }
            else
            {
                if (k < 6 && k > 0) { tb_messenter.Text += "!"; k++; }
                else { k = 0; tb_messenter.Text = "Вы ввели не все данные!"; tb_messenter.ForeColor = Color.Red; k++; }
                k++;
            }
        }

        private void but_try_Click(object sender, EventArgs e)
        {
            string log = tb_login.Text; string ans = tb_password.Text;

            Access Role1 = new Owners(tb_messenter, before_enter, List_owners, tb_question);
            Access Role2 = new Admins(tb_messenter, before_enter_admin, List_admins, tb_question);
            Access Role3 = new Users(tb_messenter, before_enter_user, List_users, tb_question);

            Role1.LowerAccess(Role2);
            Role2.LowerAccess(Role3);

            if (log == "")
            {
                tb_messenter.Text = "Логин не может быть пустым!"; tb_messenter.ForeColor = Color.IndianRed;
            }
            else if (log != "" && ans == "")
            {
                Users input = new Users("", log, "", "", "");
                Role1.ShowQuestion(input);
                if (tb_messenter.Text != "Пользователь не найден!")
                {
                    Show_Question(true); tb_pass.Enabled = true; tb_pass.Visible = true; tb_password.Enabled = true; tb_password.Visible = true;
                }  
            }
            else
            {
                Users input = new Users("", log, "", "", ans);
                Role1.Log_in(input);
                logincurrent = log;
                anscurrent = ans;
            }
            
        }

        private void but_back_Click(object sender, EventArgs e)
        {
            Default_autoriz(true); Show_Question(false); ButSignIn(true);
        }

        private void but_forgotpass_Click(object sender, EventArgs e)
        {
            Default_autoriz(false); ButSignIn(false);
        }

        private void but_signin_Click(object sender, EventArgs e)
        {
            this.Hide(); SignIn S = new SignIn(); S.Show();
        }



        // Таймеры открытия форм (2 - 4)
        // 2 - форма ВЛАДЕЛЕЦ
        // 3 - форма АДМИНИМТРАТОР
        // 4 - форма ПОЛЬЗОВАТЕЛЬ

        private void before_enter_Tick(object sender, EventArgs e)
        {
            this.Hide(); Form2 F2 = new Form2(logincurrent, passwordcurrent, anscurrent, List_owners, List_admins, List_users); 
            F2.Show();
            before_enter.Enabled = false;
        }

        private void before_enter_admin_Tick(object sender, EventArgs e)
        {
            this.Hide(); Form3 F3 = new Form3(logincurrent, passwordcurrent, anscurrent, List_admins); F3.Show();
            before_enter_admin.Enabled = false;
        }

        private void before_enter_user_Tick(object sender, EventArgs e)
        {
            this.Hide(); Form4 F4 = new Form4(logincurrent, passwordcurrent, anscurrent, List_users); F4.Show();
            before_enter_user.Enabled = false;
        }

        
    }
}
