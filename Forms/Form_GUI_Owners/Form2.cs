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
    public partial class Form2 : Form
    {
        // Загрузка формы 2

        private Owners Current; // Владелец, который авторизировался
        string login; string password; string ans;
        
        public List<string[]> BanedUsers0 = new List<string[]>();
        public List<string[]> BanedAdmins0 = new List<string[]>();

        public List<BannedUsers> banuser = new List<BannedUsers>();
        public List<BannedAdmins> banadmin = new List<BannedAdmins>();

        List<Owners> List_owners = new List<Owners>();
        List<Admins> List_admins = new List<Admins>();
        List<Users> List_users = new List<Users>();

        public Form2(string login, string password, string ans, List<Owners> List_owners, List<Admins> List_admins, List<Users> List_users)
        {
            this.login = login; this.password = password; this.ans = ans;
            this.List_owners = List_owners; this.List_admins = List_admins; this.List_users = List_users;
            foreach (Owners o in List_owners)
            {
                if (o.login == login && o.password == password || o.login == login && o.ans == ans)
                {
                    Current = o; break;
                }
            }
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
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
                    BanedAdmins0.Add(new string[2] { v1, v2 });
                    banadmin.Add(new BannedAdmins(v1, v2));
                }
            }
            con3.Close();

            SQLiteConnection con2 = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con2.Open();
            var com2 = con2.CreateCommand();
            com2.CommandText = @"SELECT Login, Message FROM BannedUsers";
            using (var v = com2.ExecuteReader())
            {
                while (v.Read())
                {
                    string v1 = v.GetString(0); string v2 = v.GetString(1);
                    BanedUsers0.Add(new string[2] { v1, v2 });
                    banuser.Add(new BannedUsers(v1, v2));
                }
            }
            con2.Close();
        }



        // Пользовательские методы

        public void BanUser(string login, string message)
        {
            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            using (var c = new SQLiteCommand("INSERT INTO BannedUsers (Login, Message) VALUES (@Login, @Message)", con))
            {
                c.Parameters.AddWithValue("@Login", login);
                c.Parameters.AddWithValue("@Message", message);
                c.ExecuteNonQuery();
            }
            con.Close();
            BanedUsers0.Add(new string[2] { login, message });
            banuser.Add(new BannedUsers(login, message));
        }

        public void BanAdmin(string login, string message)
        {
            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            using (var c = new SQLiteCommand("INSERT INTO BannedAdmins (Login, Message) VALUES (@Login, @Message)", con))
            {
                c.Parameters.AddWithValue("@Login", login);
                c.Parameters.AddWithValue("@Message", message);
                c.ExecuteNonQuery();
            }
            con.Close();
            BanedAdmins0.Add(new string[2]{login, message});
            banadmin.Add(new BannedAdmins(login, message));
        }


        public void UnBanUser(string login)
        {
            int i = 0;
            foreach (var v in BanedUsers0)
            {
                if (v[0] == login) { break; }
                i++;
            }
            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            using (var c = new SQLiteCommand("DELETE FROM BannedUsers WHERE Login = @Login", con))
            {
                c.Parameters.AddWithValue("@Login", login);
                c.ExecuteNonQuery();
            }
            con.Close();
            BanedUsers0.RemoveAt(i);
            banuser.RemoveAt(i);
        }

        public void UnBanAdmin(string login)
        {
            int i = 0;
            foreach (var v in BanedAdmins0)
            {
                if (v[0] == login) { break; }
                i++;
            }
            SQLiteConnection con = new SQLiteConnection("Integrated Security = SSPI; Data Source = AllUsers.db");
            con.Open();
            using (var c = new SQLiteCommand("DELETE FROM BannedAdmins WHERE Login = @Login", con))
            {
                c.Parameters.AddWithValue("@Login", login);
                c.ExecuteNonQuery();
            }
            con.Close();
            BanedAdmins0.RemoveAt(i);
            banadmin.RemoveAt(i);
        }

        public void SetData(bool x, bool t)
        {
            if (x) 
            {
                if (t)
                {
                    dataGridView1.DataSource = List_admins;
                }
                else
                {
                    dataGridView1.DataSource = List_users;
                }
                 
            }
            else
            {
                if (t)
                {
                    dataGridView1.DataSource = banadmin;
                }
                else
                {
                    dataGridView1.DataSource = banuser;
                }
            }
        }


        // Методы взаимодействия с Элементами

        public void Animation_name(bool x) 
        { 
            tb_name.Enabled = x; tb_name.Visible = x; t_name.Enabled = x;
        }

        public void block_but(bool x)
        {
            but_blockadmin.Enabled = x; but_blockadmin.Visible = x; 
            but_blockuser.Visible = x; but_blockuser.Enabled = x; 
        }

        public void tb_chooseSet(bool x)
        {
            tb_choose.Enabled = x; tb_choose.Visible = x;
        }

        public void back_butblock(bool x)
        {
            but_backblock.Enabled = x; but_backblock.Visible = x;
        }

        public void cb_blockSet(bool x)
        {
            cb_block.Visible = x; cb_block.Enabled = x;
        }

        public void BanbutAdmin(bool x)
        {
            but_ban.Enabled = x; but_ban.Visible = x;
        }

        public void BanbutUser(bool x)
        {
            but_banUser.Enabled = x; but_banUser.Visible = x;
        }

        public void UnBanbutAdmin(bool x)
        {
            but_AdminUnban.Enabled = x; but_AdminUnban.Visible = x;
        }

        public void UnBanbutUser(bool x)
        {
            but_UserUnban.Enabled = x; but_UserUnban.Visible = x;
        }

        public void ReasonText(bool x)
        {
            tb_reason.Enabled = x; tb_reason.Visible = x;
            tb_messBan.Enabled = x; tb_messBan.Visible = x;
            tb_reasonText.Enabled = x; tb_reasonText.Visible = x;
            if (!x) { tb_reasonText.Text = "Подробности в офисе..."; tb_messBan.Clear(); }
        }

        public void but_unban(bool x)
        {
            but_unbanAdmin.Enabled = x; but_unbanAdmin.Visible = x;
            but_unbanUser.Enabled = x; but_unbanUser.Visible = x;
        }

        public void but_lists(bool x)
        {
            but_listbanned.Enabled = x; but_listbanned.Visible = x;
            but_listpeople.Enabled = x; but_listpeople.Visible = x;
        }

        public void dataGrid(bool x)
        {
            dataGridView1.Enabled = x; dataGridView1.Visible = x;
            cb_list.Enabled = x; cb_list.Visible = x;
            if (x) { cb_list.Items.Clear(); cb_list.Items.AddRange(new string[2] { "Список Админов", "Список Пользователей" }); cb_list.Text = "..."; }
        }


        // События "кнопки" + "картинки"

        int k = 0;
        public void pb_account_Click(object sender, EventArgs e)
        {
           tb_name.Text = $"{Current.surname} {Current.name}";
           if (k % 2 == 0) { Animation_name(true); k++; }
           else { tb_name.Enabled = false; tb_name.Visible = false; k++; }
        }

        private void but_blockadmin_Click(object sender, EventArgs e)
        {
            block_but(false); back_butblock(true); cb_blockSet(true); BanbutAdmin(true); but_lists(false); 
            tb_chooseSet(true); tb_choose.Text = "Выберите Админа"; ReasonText(true); but_unban(false);
            foreach (Admins a in List_admins)
            {
                cb_block.Items.Add($"{a.Name}, логин : {a.login}");
            }
        }

        private void but_blockuser_Click(object sender, EventArgs e)
        {
            block_but(false); back_butblock(true); cb_blockSet(true); ReasonText(true); BanbutUser(true); but_unban(false);
            tb_chooseSet(true); tb_choose.Text = "Выберите Пользователя"; but_lists(false);
            foreach (Users u in List_users)
            {
                cb_block.Items.Add($"{u.Name}, логин : {u.Login}");
            }
        }

        private void but_backblock_Click(object sender, EventArgs e)
        {
            block_but(true); back_butblock(false); BanbutAdmin(false); ReasonText(false); BanbutUser(false); but_unban(true);
            tb_choose.Clear(); tb_chooseSet(false); cb_blockSet(false); cb_block.Items.Clear(); cb_block.Text = "..."; 
            UnBanbutUser(false); UnBanbutAdmin(false); but_lists(true); dataGrid(false); dataGridView1.DataSource = null;
        }

        private void but_ban_Click(object sender, EventArgs e)
        {
            int i = cb_block.SelectedIndex; int k0 = 0;
            Admins BlockedAdmin = List_admins[i];
            string login = BlockedAdmin.login;
            string message = tb_reasonText.Text;
            foreach (var l in BanedAdmins0)
            {
                if (l[0] == login)
                {
                    tb_messBan.Text = "Данный пользователь уже забанен!"; tb_messBan.ForeColor = Color.Red; k0++; break;
                }
            }
            if (k0 == 0) 
            {
                tb_messBan.Text = $"Пользователь успешно забанен!"; tb_messBan.ForeColor = Color.Green; BanAdmin(login, message);
                t_banAdmin.Enabled = true;
            }
        }
        
        private void but_banUser_Click(object sender, EventArgs e)
        {
            int i = cb_block.SelectedIndex; int k1 = 0;
            Users BlockedUser = List_users[i];
            string login = BlockedUser.Login;
            string message = tb_reasonText.Text;
            foreach (var l in BanedUsers0)
            {
                if (l[0] == login)
                {
                    tb_messBan.Text = "Данный пользователь уже забанен!"; tb_messBan.ForeColor = Color.Red; k1++; break;
                }
            }
            if (k1 == 0)
            {
                tb_messBan.Text = $"Пользователь успешно забанен!"; tb_messBan.ForeColor = Color.Green; BanUser(login, message);
                t_banAdmin.Enabled = true;
            }
        }

        private void but_unbanAdmin_Click(object sender, EventArgs e)
        {
            but_unban(false); back_butblock(true); tb_messBan.Enabled = true; tb_messBan.Visible = true;
            block_but(false); cb_blockSet(true); UnBanbutAdmin(true); but_lists(false);
            tb_chooseSet(true); tb_choose.Text = "Выберите Админа";
            foreach (Admins a in List_admins)
            {
                cb_block.Items.Add($"{a.Name}, логин : {a.login}");
            }
        }

        private void but_unbanUser_Click(object sender, EventArgs e)
        {
            but_unban(false); back_butblock(true); tb_messBan.Enabled = true; tb_messBan.Visible = true;
            block_but(false); cb_blockSet(true); UnBanbutUser(true); but_lists(false);
            tb_chooseSet(true); tb_choose.Text = "Выберите Пользователя";
            foreach (Users u in List_users)
            {
                cb_block.Items.Add($"{u.Name}, логин : {u.Login}");
            }
        }

        private void but_UserUnban_Click(object sender, EventArgs e)
        {
            int i = cb_block.SelectedIndex; int k1 = 0;
            Users BlockedUser = List_users[i];
            string login = BlockedUser.Login;
            foreach (var l in BanedUsers0)
            {
                if (l[0] == login)
                {
                    k1++; break;
                }
            }
            if (k1 > 0)
            {
                tb_messBan.Text = $"Пользователь успешно разбанен!"; tb_messBan.ForeColor = Color.Green; UnBanUser(login);
                t_banAdmin.Enabled = true;
            }
            else
            {
                tb_messBan.Text = "Данный пользователь не забанен!"; tb_messBan.ForeColor = Color.Red;
            }
        }

        private void but_AdminUnban_Click(object sender, EventArgs e)
        {
            int i = cb_block.SelectedIndex; int k1 = 0;
            Admins BlockedAdmin = List_admins[i];
            string login = BlockedAdmin.login;
            foreach (var l in BanedAdmins0)
            {
                if (l[0] == login)
                {
                    k1++; break;
                }
            }
            if (k1 > 0)
            {
                tb_messBan.Text = $"Пользователь успешно разбанен!"; tb_messBan.ForeColor = Color.Green; UnBanAdmin(login);
                t_banAdmin.Enabled = true;
            }
            else
            {
                tb_messBan.Text = "Данный пользователь не забанен!"; tb_messBan.ForeColor = Color.Red;
            }
        }

        bool b;
        private void but_listpeople_Click(object sender, EventArgs e)
        {
            but_lists(false); back_butblock(true); dataGrid(true); b = true;   
        }

        private void but_listbanned_Click(object sender, EventArgs e)
        {
            but_lists(false); back_butblock(true); dataGrid(true); b = false;
        }

        private void cb_list_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cb_list.SelectedIndex)
            {
                case 0:
                    SetData(b, true); break;
                case 1:
                    SetData(b, false); break;
                default:
                    break;
            }
        }



        // Таймеры 

        private void t_name_Tick(object sender, EventArgs e)
        {
            if (tb_name.Location.X >= 600) { t_name.Enabled = false; }
            else { tb_name.Left += 10; }
        }

        private void t_banAdmin_Tick(object sender, EventArgs e)
        {
            but_backblock_Click(sender, e); t_banAdmin.Enabled = false;
        }       
    }
}
