using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services
{
    public interface IPasswordLogServices
    {
        System.Collections.Generic.IEnumerable<CBUSA.Domain.PasswordLog> GetPasswordLog();
        void SavePasswordLog(CBUSA.Domain.PasswordLog ObjPasswordLog);

        bool IsPreviousPassword(int UserId,string Password);
        
    }
}
