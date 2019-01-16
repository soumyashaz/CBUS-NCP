using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services.Model
{
    public class UserInRoleServices:IUserInRoleServices
    {
        public UserInRoleServices()
        {}
        private readonly IUnitOfWork _ObjUnitWork;
        public UserInRoleServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<UserInRole> GetUserInRole()
        {
            return _ObjUnitWork.UserInRole.GetAll();
        }

        public UserInRole FindUserInRole(Int64 Id)
        {
            return _ObjUnitWork.UserInRole.Get(Id);
        }

        public void EditUserInRole(UserInRole ObjUserInRole)
        {

            _ObjUnitWork.UserInRole.Update(ObjUserInRole);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveUserInRole(UserInRole ObjUserInRole)
        {
            _ObjUnitWork.UserInRole.Add(ObjUserInRole);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public bool UserIsAthorishedRole(string SystemRole, int UserId)
        {
            return _ObjUnitWork.UserInRole.UserIsAthorishedRole(SystemRole, UserId);
        }
        public UserInRole RoleInformationByUserId(Int32 UserId)
        {
           return _ObjUnitWork.UserInRole.Search(w => w.UserId == UserId).FirstOrDefault();
          //  return _ObjUnitWork.UserInRole.GetAllQueryable(w => w.UserId == UserId).FirstOrDefault();
        }

       

        public  IEnumerable<UserInRole> FindUserInRoleByCourtId(int CourtId)
        {
            //return  _ObjUnitWork.UserInRole.SearchWithInclude(e => e.User.CourtId == CourtId && (e.Role.SystemRole == (Int32)GetRoleName.PO || e.Role.SystemRole == (Int32)GetRoleName.CourtEmployee || e.Role.SystemRole == (Int32)GetRoleName.CourtClerk || e.Role.SystemRole == (Int32)GetRoleName.Judge));
            return _ObjUnitWork.UserInRole.SearchWithInclude(e => e.User.CourtId == CourtId && (e.Role.SystemRole == (Int32)GetRoleName.SuperAdmin));
        }


        public async Task<IEnumerable <UserInRole>>RoleInformation_ByUserId(Int32 UserId)
        {
            return await _ObjUnitWork.UserInRole.SearchAsyn(w => w.UserId == UserId);
            
        }

       


    }
}
