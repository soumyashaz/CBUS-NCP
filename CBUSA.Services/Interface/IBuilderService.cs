using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
namespace CBUSA.Services.Interface
{
    public interface IBuilderService
    {
        IEnumerable<Builder> GetBuilder();

        IEnumerable<Builder> FindBuilderMarket(Int64 MarketId);

        Builder IsBuilderAuthenticate(Int64 BuilderId);
        Builder GetSpecificBuilder(Int64 BuilderId);
        IEnumerable<Builder> BuilderDetails(Int64 BuilderId);

        IEnumerable<BuilderUser> GetBuliderAllUser(Int64 BuilderId);

        BuilderUser IsUserAuthenticate(Int64 UserId);


    }
}
