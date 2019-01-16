using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public interface IEulaServices
    {
        Eula FindEula(Int64 Id);
        IEnumerable<Eula> GetEula();
        void EditEula(Eula ObjEula);
        void SaveEula(Eula ObjEula);
        void DeleteEula(Eula ObjEula);
        string GetRoleWiseEula(int RoleId);
    }
}
