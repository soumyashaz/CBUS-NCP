using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public class PasswordLogServices : CBUSA.Services.IPasswordLogServices
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public PasswordLogServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<PasswordLog> GetPasswordLog()
        {
            return _ObjUnitWork.PasswordLogs.GetAll();
        }



        public void SavePasswordLog(PasswordLog ObjPasswordLog)
        {
            _ObjUnitWork.PasswordLogs.Add(ObjPasswordLog);
            _ObjUnitWork.Complete();
        }

        public bool IsPreviousPassword(int UserId, string Password)
        {
            bool IsPrevPassword = false;
            List<PasswordLog> ObjPasswordLog = _ObjUnitWork.PasswordLogs.Find(x => x.UserId == UserId).OrderByDescending(x => x.CreateDate).Take(5).ToList();
            foreach (PasswordLog Obj in ObjPasswordLog)
            {
                if (Encrypt.DecryptString(Obj.PasswordHash) == Password)
                {
                    IsPrevPassword = true;
                    break;
                }
            }

            return IsPrevPassword;
        }
    }
}
