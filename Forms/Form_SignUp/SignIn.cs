using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_TP
{
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void SignIn_Load(object sender, EventArgs e)
        {
            EnableAll(true);
        }

        public bool Check()
        {
            if (tb_inpname.Text != string.Empty && tb_inplogin.Text != string.Empty && tb_inppassword.Text != string.Empty &&
                tb_inpquestion.Text != string.Empty && tb_inpanswer.Text != string.Empty) { return true; }
            else { return false; }
        }
        public void EnableAll(bool x)
        {
            tb_answer.Enabled = x; tb_inpanswer.Enabled = x;
            tb_login.Enabled = x; tb_inplogin.Enabled = x;
            tb_password.Enabled = x; tb_inppassword.Enabled = x;
            tb_question.Enabled = x; tb_inpquestion.Enabled = x;
            tb_name.Enabled = x; tb_inpname.Enabled = x;
            tb_1.Enabled = x; tb_1.Enabled = x;
            tb_messenter.Enabled = x;
        }
        public void But(bool x)
        {
            but_reg.Enabled = x; but_reg.Visible = x;
        }
        public void SetMess(string mess, bool t)
        {
            tb_messenter.Text = mess;
            if (!t) { tb_messenter.ForeColor = Color.Red; tb_messenter.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_messenter.ForeColor = Color.Green; tb_messenter.BackColor = Color.FromArgb(192, 255, 192); }
        }


        private void but_reg_Click(object sender, EventArgs e)
        {
            if (!Check()) 
            {
                SetMess("Не все данные введены, перепроверьте!", false);
            }
            else
            {
                SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
                con.Open();
                string t1 = tb_inpname.Text; string t2 = tb_inplogin.Text; string t3 = tb_inppassword.Text; string t4 = tb_inpquestion.Text; string t5 = tb_inpanswer.Text;
                using (var c = new SQLiteCommand("INSERT INTO Users (Name, Login, Password, Question, Answer) VALUES (@Name, @Login, @Password, @Question, @Answer)", con))
                {
                    c.Parameters.AddWithValue("@Name", t1);
                    c.Parameters.AddWithValue("@Login", t2);
                    c.Parameters.AddWithValue("@Password", t3);
                    c.Parameters.AddWithValue("@Question", t4);
                    c.Parameters.AddWithValue("@Answer", t5);
                    c.ExecuteNonQuery();
                }
                con.Close();
                SetMess("Регистрация прошла успешно. Вы зарегистрированы, как пользователь!", true); EnableAll(false); close.Enabled = true;
            }
        }

        private void close_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
