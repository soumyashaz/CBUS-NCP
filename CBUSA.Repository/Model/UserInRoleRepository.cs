using CBUSA.Domain;
using CBUSA.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Repository.Model
{
    public class UserInRoleRepository : Repository<UserInRole>, IUserInRoleRepository
    {

        public UserInRoleRepository(CBUSADbContext Context)
            : base(Context)
        {
        }
        //public IEnumerable<UserInRole> GetUserInRoleByUserId(int UserId)
        //{

        //    //var UserAgencyList= (from _OfffenderList in Context.DbsOffender.Where(w => w.RowStatusId == (Int32)RowActiveStatus.Active) 
        //    //                     join _ObjUserList in Context.DbsUser.Where(w => w.RowStatusId == (Int32)RowActiveStatus.Active && w.CourtId != null)
        //    //                                                 on _ObjCourtList.CourtId equals _ObjUserList.CourtId
        //    //                     )
        //    from _ObjUserList in Context.DbsUser


        //    return ObjGetUsersCountPerCourt;
        //}

        public bool UserIsAthorishedRole(string SystemRole, int UserId)
        {
            GetRoleName RoleName = (GetRoleName)Enum.Parse(typeof(GetRoleName), SystemRole);
            UserInRole ObjUserInRole = Context.DbsUserInRole.Where(x => x.UserId == UserId && x.Role.SystemRole == (int)RoleName).FirstOrDefault();
            if (ObjUserInRole != null && ObjUserInRole.UserRoleId > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public CBUSADbContext Context
        {
            get
            {
                return _Context as CBUSADbContext;
            }
        }
    }
}
