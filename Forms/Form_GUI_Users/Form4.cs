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
    public partial class Form4 : Form
    {
        // Загрузка формы 4

        Users CurrentUser = new Users(); List<string[]> BannedUsers = new List<string[]>(); List<Products> products;
        string login; string password; string ans;
        
        public Form4(string login, string password, string ans, List<Users> users)
        {
            this.login = login; this.password = password; this.ans = ans;
            foreach (var user in users)
            {
                if (user.Login == login && user.Password == password || user.Login == login && user.Answer == ans) 
                { 
                    CurrentUser = user; break;
                }
            }
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            pb_products(false); BackBut(false); SetTbMess("", true, false);
            tbName(false);

            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            var com = con.CreateCommand();
            com.CommandText = @"SELECT Login, Message FROM BannedUsers";
            using (var v = com.ExecuteReader())
            {
                while (v.Read())
                {
                    BannedUsers.Add(new string[2] { v.GetString(0), v.GetString(1) });
                }
            }
            con.Close();

            if (BannedUsers.Count > 0)
            {
                foreach (var v in BannedUsers)
                {
                    if (v[0] == login)
                    {
                        BanOn(v[1]);
                    }
                }
            }
        }



        // Методы взаимодействия с Элементами

        public void Animation_name(bool x)
        {
            tb_name.Enabled = x; tb_name.Visible = x; t_name.Enabled = x;
        }

        public void pb_products(bool x)
        {
            pb_cacti.Enabled = x; pb_cacti.Visible = x;
            pb_cleaner.Enabled = x; pb_cleaner.Visible = x;
            pb_light.Enabled = x; pb_light.Visible = x;
            pb_fan.Enabled = x; pb_fan.Visible = x;
            tb_tovari.Enabled = x; tb_tovari.Visible = x;
        }

        public void BackBut(bool x)
        {
            but_back.Enabled = x; but_back.Visible = x;
        }

        public void SetTbMess(string mess, bool t, bool x)
        {
            tb_mess.Text = mess; tb_mess.Enabled = x; tb_mess.Visible = x;
            if (!t) { tb_mess.ForeColor = Color.Red; tb_mess.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_mess.ForeColor = Color.Green; tb_mess.BackColor = Color.FromArgb(192, 255, 192); }
            pb_products(false);
        }

        public void cb(bool x)
        {
            cb_choose.Visible = x; cb_choose.Enabled = x;
        }

        public void tbName(bool x)
        {
            tb_name.Enabled = x; tb_name.Visible = x;
        }

        public void BanOn(string mess)
        {
            tb_ban.Enabled = true; tb_ban.Visible = true; tb_ban.Text = $"Вы были забанены по причине: \n'{mess}'!";
            tb_ann.Enabled = false; cb_choose.Enabled = false; pb_account.Enabled = false;
        }



        // Пользовательские методы

        public List<Products> GetProducts(string a)
        {
            List<Products> products = new List<Products>();
            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            var com = con.CreateCommand();
            com.CommandText = @"SELECT ID, Name, Count FROM Shop" + a;
            using (var v = com.ExecuteReader())
            {
                while (v.Read())
                {
                    products.Add(new Products(v.GetInt32(0), v.GetString(1), v.GetInt32(2)));
                }
            }
            con.Close();
            return products;
        }

        public void ChooseProduct(int id, string a)
        {
            foreach (var v in products)
            {
                if (v.id == id)
                {
                    if (v.Count > 0)
                    {
                        SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
                        con.Open();
                        string m = $"UPDATE Shop{a} SET Count = @Quantity WHERE Id = @Id";
                        using (SQLiteCommand command = new SQLiteCommand(m, con))
                        {
                            command.Parameters.AddWithValue("@Quantity", v.Count - 1);
                            command.Parameters.AddWithValue("@Id", id);
                            
                            command.ExecuteNonQuery();
                        }
                        con.Close();
                        SetTbMess($"Товар '{v.Name}' успешно заказан!", true, true); pb_products(false); timer1.Enabled = true; BackBut(false); break;
                    }
                    else { SetTbMess($"Товара '{v.Name}' нет в наличии!", false, true); pb_products(false); timer1.Enabled = true; BackBut(false); break; }
                }
            }
        }



        // События "кнопки" + "картинки" + "комбобоксы"

        int k = 0;
        private void pb_account_Click(object sender, EventArgs e)
        {
            tb_name.Text = $" {CurrentUser.Name}";
            if (k % 2 == 0) { Animation_name(true); k++; }
            else { tb_name.Enabled = false; tb_name.Visible = false; k++; }
        }

        string I;
        private void cb_choose_SelectedIndexChanged(object sender, EventArgs e)
        {
            string i = $"{cb_choose.SelectedIndex + 1}";
            I = i; BackBut(true);
            products = GetProducts(i);
            cb_choose.Enabled = false;
            pb_products(true);
        }

        private void but_back_Click(object sender, EventArgs e)
        {
            cb(true); cb_choose.Items.Clear(); pb_products(false); SetTbMess("", true, false); BackBut(false);
            cb_choose.Text = "...";
            cb_choose.Items.AddRange(new string[3] { "Пункт выдачи 1 - улица Ленина, дом 12", "Пункт выдачи 2 - улица Пушкина, дом Калатушкина", "Пункт выдачи 3 - Красная Площадь, дом 1" });
        }

        private void pb_cleaner_Click(object sender, EventArgs e)
        {
            ChooseProduct(1, I);
        }

        private void pb_fan_Click(object sender, EventArgs e)
        {
            ChooseProduct(2, I);
        }

        private void pb_light_Click(object sender, EventArgs e)
        {
            ChooseProduct(3, I);
        }

        private void pb_cacti_Click(object sender, EventArgs e)
        {
            ChooseProduct(4, I);
        }



        // Таймеры 

        private void timer1_Tick(object sender, EventArgs e)
        {
            but_back_Click(sender, e); timer1.Enabled = false;
        }

        private void t_name_Tick(object sender, EventArgs e)
        {
            if (tb_name.Location.X >= 725) { t_name.Enabled = false; }
            else { tb_name.Left += 10; }
        }

        
    }
}
