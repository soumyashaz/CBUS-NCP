using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    public class ResourceMarketServices : CBUSA.Services.IResourceMarketServices
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public ResourceMarketServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public void SaveResourceMarket(ResourceMarket ObjResourceMarket)
        {
            //_ObjUnitWork.ResourceMarkets.Add(ObjResourceMarket);
            //_ObjUnitWork.Complete();
            //_ObjUnitWork.Dispose();
        }
    }
}
