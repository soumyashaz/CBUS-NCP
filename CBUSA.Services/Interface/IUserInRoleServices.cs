using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Interface
{
    public interface IUserInRoleServices
    {

        UserInRole FindUserInRole(Int64 Id);
        IEnumerable<UserInRole> GetUserInRole();
        void EditUserInRole(UserInRole ObjUserInRole);
        void SaveUserInRole(UserInRole ObjUserInRole);

       bool UserIsAthorishedRole(string SystemRole, int UserId);

        // New for Reduce Loading Time coded by Debraj Sil 31-05-2017
       UserInRole RoleInformationByUserId(Int32 UserId);

        IEnumerable<UserInRole> FindUserInRoleByCourtId(int CourtId);

        Task<IEnumerable<UserInRole>> RoleInformation_ByUserId(int UserId);
    }
}
