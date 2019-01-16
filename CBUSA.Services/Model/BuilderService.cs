using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
namespace CBUSA.Services.Model
{
    public class BuilderService : IBuilderService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public BuilderService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Builder> GetBuilder()
        {
            return _ObjUnitWork.Builder.GetAll();
        }

        public Builder GetSpecificBuilder(Int64 BuilderId)
        {
            return _ObjUnitWork.Builder.Get(BuilderId);
        }

        public IEnumerable<Builder> FindBuilderMarket(Int64 MarketId)
        {
            return _ObjUnitWork.Builder.Search(x => x.MarketId == MarketId);
        }

        public Builder IsBuilderAuthenticate(Int64 BuilderId)
        {
            return _ObjUnitWork.Builder.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
        }

        public BuilderUser IsUserAuthenticate(Int64 UserId)
        {
            return _ObjUnitWork.BuilderUser.Search(x => x.BuilderUserId == UserId && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
        }        

        public IEnumerable<Builder> BuilderDetails(Int64 BuilderId)
        {
            return _ObjUnitWork.Builder.Search(x => x.BuilderId == BuilderId);
        }

        public IEnumerable<BuilderUser> GetBuliderAllUser(Int64 BuilderId)
        {
            return _ObjUnitWork.BuilderUser.Search(x => x.BuilderId == BuilderId && x.RowStatusId == (int)RowActiveStatus.Active);
        }
    }
}
