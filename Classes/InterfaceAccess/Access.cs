using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_TP
{
    public interface Access
    {
        void Log_in(Users user);
        void LowerAccess(Access access);
        void ShowQuestion(Users users);
    }
}
