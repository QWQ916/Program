using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TP
{
    public class BannedAdmins
    {
        private string login; public string Login { get => login; }
        private string mess; public string Message { get => mess; }
        public BannedAdmins(string login, string mess)
        {
            this.login = login; this.mess = mess;
        }
        public BannedAdmins()
        {

        }
    }
}
