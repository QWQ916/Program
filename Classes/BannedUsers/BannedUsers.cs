using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TP
{
    public class BannedUsers
    {
        private string login; public string Login { get => login; }
        private string mess; public string Message { get => mess; }
        public BannedUsers(string login, string mess)
        {
            this.login = login; this.mess = mess;
        }
        public BannedUsers()
        {
            
        }
    }
}
