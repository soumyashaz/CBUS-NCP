using CBUSA.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class ContractRebateService : IContractRebateService
    {

        private readonly IUnitOfWork _ObjUnitWork;

        public ContractRebateService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<ContractRebate> GetContractReabteAll()
        {
            return _ObjUnitWork.ContractRebate.GetAll();
        }

        public ContractRebate GetContractReabte(long ContractReabteId)
        {
            return _ObjUnitWork.ContractRebate.Get(ContractReabteId);

        }
        public void SaveContractReabte(ContractRebate ObjContractReabte)
        {
            _ObjUnitWork.ContractRebate.Add(ObjContractReabte);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void EditContractReabte(ContractRebate ObjContractReabte)
        {
            _ObjUnitWork.ContractRebate.Update(ObjContractReabte);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteContractReabte(ContractRebate ObjContractReabte)
        {
            _ObjUnitWork.ContractRebate.Update(ObjContractReabte);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void SaveContractRebate(List<ContractRebate> ObjContractRebate)
        {
            foreach (var Item in ObjContractRebate)
            {
                var ItemChild = _ObjUnitWork.ContractRebate.Search(x => (x.ContractId == Item.ContractId && x.ContractStatusId == Item.ContractStatusId));

                if (ItemChild.Count() == 0)
                {
                    _ObjUnitWork.ContractRebate.Add(Item);
                }
                else
                {
                    var UpdatedContractRebate = ItemChild.FirstOrDefault();
                    if (UpdatedContractRebate != null)
                    {
                        UpdatedContractRebate.RebatePercentage = Item.RebatePercentage;
                        UpdatedContractRebate.ModifiedOn = DateTime.Now;
                    }
                    _ObjUnitWork.ContractRebate.Update(UpdatedContractRebate);
                }
            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public IEnumerable<ContractRebateBuilder> GetContactBuilderRebateAll(long ContractId, long BuilderId)
        {
           return _ObjUnitWork.ContractRebateBuilder.Search(x => (x.ContractId == ContractId && x.BuilderId == BuilderId)).OrderByDescending(x=>x.ModifiedOn);
        }




        public void OverrideContractRebate(ContractRebateBuilder ObjContractRebateBuilder)
        {
            _ObjUnitWork.ContractRebateBuilder.Add(ObjContractRebateBuilder);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
    }
}
