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
    public partial class Form3 : Form
    {
        // Загрузка формы
        private Admins Current = new Admins();
        private List<Admins> admins = new List<Admins>();
        private List<BannedAdmins> bannedadmins = new List<BannedAdmins>();
      
        public Form3(string login, string password, string ans, List<Admins> admins)
        {
            this.admins = admins; 
            foreach (Admins a in admins) 
            {
                if (a.login == login && a.password == password || a.login == login && a.Answer == ans)
                {
                    Current = a; break;
                }
            }
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            SQLiteConnection con3 = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con3.Open();
            var com3 = con3.CreateCommand();
            com3.CommandText = @"SELECT Login, Message FROM BannedAdmins";
            using (var v = com3.ExecuteReader())
            {
                while (v.Read())
                {
                    string v1 = v.GetString(0); string v2 = v.GetString(1);
                    bannedadmins.Add(new BannedAdmins(v1, v2));
                }
            }
            con3.Close();

            foreach (BannedAdmins a in bannedadmins)
            {
                if (a.Login == Current.Login)
                {
                    Ban(a.Message); break;
                }
            }
        }



        // Пользовательские методы

        public List<Products> GetProductsFromShop(int x)
        {
            List<Products> products = new List<Products>();
            string s = x.ToString();
            SQLiteConnection con1 = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con1.Open();
            var com1 = con1.CreateCommand();
            com1.CommandText = $@"SELECT Name, Price, Count FROM Shop{s}";
            
            using (var v = com1.ExecuteReader())
            {
                while (v.Read())
                {
                    products.Add(new Products(v.GetString(0), v.GetInt32(1), v.GetInt32(2)));
                }
            }
            con1.Close();
            return products;
        }

        public Products GetProductFromCb(int ishop, int iproduct)
        {
            List<Products> p = GetProductsFromShop(ishop);
            return p[iproduct];
        }

        public void UpdateCountProduct(int ishop, Products p, int count, bool x)
        {
            string s = ishop.ToString(); int q = 1; if (!x) { q = -1; }
            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            string m = $"UPDATE Shop{s} SET Count = @Quantity WHERE Name = @Name";
            using (SQLiteCommand command = new SQLiteCommand(m, con))
            {
                command.Parameters.AddWithValue("@Quantity", p.Count + (count * q));
                command.Parameters.AddWithValue("@Name", p.Name);
                command.ExecuteNonQuery();
            }
            con.Close();
        }

        public void Ban(string mess)
        {
            pb_account.Enabled = false;
            but_tovari.Enabled = false; but_control.Enabled = false;
            tb_ban.Enabled = true; tb_ban.Visible = true;
            tb_ban.Text = $"Вы были забанены Владельцем по причине: '{mess}'!";
        }




        // Методы взаимодействия между элементами

        public void Animation_name(bool x)
        {
            tb_name.Enabled = x; tb_name.Visible = x; t_name.Enabled = x;
        }

        public void dataGrid(bool x)
        {
            dataGridView1.Enabled = x; dataGridView1.Visible = x;
            cb_list.Enabled = x; cb_list.Visible = x;
            if (x)
            {
                cb_list.Items.Clear(); cb_list.Text = "...";
                cb_list.Items.AddRange(new string[3] { "Пункт выдачи 1 - улица Ленина, дом 12", "Пункт выдачи 2 - улица Пушкина, дом Калатушкина", "Пункт выдачи 3 - Красная Площадь, дом 1" });
            }
            else
            {
                dataGridView1.DataSource = null;
            }
        }

        public void ButTovari(bool x)
        {
            but_tovari.Enabled = x; but_tovari.Visible = x;
        }

        public void ButBack(bool x)
        {
            but_backblock.Enabled = x; but_backblock.Visible = x;
        }

        public void ButControl(bool x)
        {
            but_control.Enabled = x; but_control.Visible = x;
        }

        public void Control(bool x)
        {
            cb_control.Enabled = x; cb_control.Visible = x; tb_info.Visible = x; tb_info.Enabled = x; tb_mess.Visible = x; tb_mess.Enabled = x;
            if (!x) 
            { 
                cb_control.Items.Clear(); cb_control.Text = "..."; cb_control.Items.AddRange(new string[2] { "Приходная накладная", "Расходная накладная" });
                tb_info.Text = "Выберите тип накладной"; tb_info.BackColor = Color.BlanchedAlmond; tb_mess.Clear(); tb_mess.BackColor = Color.Black;
            }
        }
        public void SetTbMess(string mess, bool t)
        {
            tb_mess.Text = mess;
            if (!t) { tb_mess.ForeColor = Color.Red; tb_mess.BackColor = Color.FromArgb(255, 192, 192); }
            else { tb_mess.ForeColor = Color.Green; tb_mess .BackColor = Color.FromArgb(192, 255, 192); }
        }

        public void Shop(bool x)
        {
            cb_shop.Enabled = x; cb_shop.Visible = x; tb_shop.Visible = x; tb_shop.Enabled = x;
            if (!x)
            {
                cb_shop.Items.Clear(); cb_shop.Text = "..."; 
                cb_shop.Items.AddRange(new string[3] { "Пункт выдачи 1 - улица Ленина, дом 12", "Пункт выдачи 2 - улица Пушкина, дом Калатушкина", "Пункт выдачи 3 - Красная Площадь, дом 1" });
                tb_shop.Text = "Выберите магазин"; tb_shop.BackColor = Color.SandyBrown;
            }
        }

        public void Sproduct(bool x)
        {
            cb_product.Enabled = x; cb_product.Visible = x; tb_product.Visible = x; tb_product.Enabled = x;
            if (!x)
            {
                cb_product.Items.Clear(); cb_product.Text = "...";
                cb_product.Items.AddRange(new string[4] { "Пылесос", "Вентилятор", "Светильник", "Кактус" });
                tb_product.Text = "Выберите продукт"; tb_product.BackColor = Color.Gold;
            }
        }

        public void ButExec(bool x)
        {
            but_exec.Enabled = x; but_exec.Visible = x; tb_input.Enabled = x; tb_input.Visible = x; tb_count.Enabled = x; tb_count.Visible = x; 
            if (!x)
            {
                tb_input.Clear(); 
            }
        }




        // События "картинки" и "кнопки"

        int k = 0;
        private void pb_account_Click(object sender, EventArgs e)
        {
            tb_name.Text = Current.Name;
            if (k % 2 == 0) { Animation_name(true); k++; }
            else { tb_name.Enabled = false; tb_name.Visible = false; k++; }
        }

        private void but_backblock_Click(object sender, EventArgs e)
        {
            dataGrid(false); ButBack(false); ButTovari(true); ButControl(true); Control(false); Shop(false); Sproduct(false);
            ButExec(false);
        }
        
        private void cb_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_list.SelectedIndex)
            {
                case 0:
                    dataGridView1.DataSource = GetProductsFromShop(1); break;
                case 1:
                    dataGridView1.DataSource = GetProductsFromShop(2); break;
                case 2:
                    dataGridView1.DataSource = GetProductsFromShop(3); break;
                default: break;
            }
        }
        
        private void but_tovari_Click(object sender, EventArgs e)
        {
            dataGrid(true); ButBack(true); ButTovari(false); ButControl(false);
        }

        private void but_control_Click(object sender, EventArgs e)
        {
            ButBack(true); ButControl(false); ButTovari(false); Control(true); 
        }

        private void cb_control_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_control.Enabled = false; Shop(true);
        }

        private void cb_shop_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_shop.Enabled = false; Sproduct(true);
        }

        private void cb_product_SelectedIndexChanged(object sender, EventArgs e)
        {
            cb_product.Enabled = false; ButExec(true);
        }

        private void but_exec_Click(object sender, EventArgs e)
        {
            try
            {
                int count = Convert.ToInt16(tb_input.Text);
                if (count <= 0) { count = Convert.ToInt16("error"); }
                bool type = cb_control.SelectedIndex == 0;
                Products p = GetProductFromCb(cb_shop.SelectedIndex + 1, cb_product.SelectedIndex);
                if (!type && count > p.Count && count > 0)
                {
                    SetTbMess("Нельзя отдать больше товара, чем есть на этом складе!", false);
                }
                else
                {
                    UpdateCountProduct(cb_shop.SelectedIndex + 1, p, count, type);
                    SetTbMess("Операция прошла успешно!", true); but_exec.Enabled = false; t_exec.Enabled = true;
                }
            }
            catch (Exception)
            {
                SetTbMess("Вы ввели не целое число (или отрицательное) или вообще не число, попробуйте ещё раз!", false);
                tb_input.Clear();
            }
        }


        // Таймеры

        private void t_name_Tick(object sender, EventArgs e)
        {
            if (tb_name.Location.X >= 600) { t_name.Enabled = false; }
            else { tb_name.Left += 10; }
        }

        private void t_exec_Tick(object sender, EventArgs e)
        {
            but_backblock_Click(sender, e); t_exec.Enabled = false;
        }
    }
}
