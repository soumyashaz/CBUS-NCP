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
    public class MarketService : IMarketService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public MarketService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<Market> GetMarket()
        {
            return _ObjUnitWork.Market.GetAll();
        }
    }
}
