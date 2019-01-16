using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Services.Interface;
namespace CBUSA.Services.Model
{
    public class QuarterContractStatusService: IQuarterContractStatusService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public QuarterContractStatusService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<QuarterContractStatus> GetQuarterContractStatus()
        {
            return _ObjUnitWork.QuarterContractStatus.GetAll();
        }
    }
}
