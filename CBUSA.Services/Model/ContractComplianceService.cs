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
    public class ContractComplianceService : IContractComplianceService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ContractComplianceService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public IEnumerable<Builder> GetBuilder()
        {
            return _ObjUnitWork.Builder.GetAll();
        }
        public IEnumerable<Builder> FindBuilderMarket(Int64 MarketId)
        {
            return _ObjUnitWork.Builder.Search(x => x.MarketId == MarketId);

        }

        public IEnumerable<ContractCompliance> GetContractComplianceAll()
        {
            throw new NotImplementedException();
        }

        public ContractCompliance GetContractCompliance(long ContractComplianceId)
        {
            throw new NotImplementedException();
        }



        public void SaveContractCompliance(ContractCompliance ObjContractCompliance)
        {
            _ObjUnitWork.ContractCompliance.Add(ObjContractCompliance);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void UpdateContractCompliance(ContractCompliance ObjContractCompliance)
        {
            _ObjUnitWork.ContractCompliance.Update(ObjContractCompliance);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public void DeleteContractCompliance(ContractCompliance ObjContractCompliance)
        {
            _ObjUnitWork.ContractCompliance.Remove(ObjContractCompliance);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        //public bool IsContractEstimateComplianceExist(long ContractId)
        //{
        //    return _ObjUnitWork.ContractCompliance.Find(x => x.EstimatedValue == true && x.ContractId == ContractId
        //        && x.RowStatusId == (int)RowActiveStatus.Active).Count() > 0;
        //}

        //public bool IsContractActualComplianceExist(long ContractId)
        //{
        //    return _ObjUnitWork.ContractCompliance.Find(x => x.ActualValue == true && x.ContractId == ContractId
        //        && x.RowStatusId == (int)RowActiveStatus.Active).Count() > 0;
        //}

        public ContractCompliance ContractEstimateCompliance(long ContractId)
        {
            return _ObjUnitWork.ContractCompliance.Search(x => x.EstimatedValue == true && x.ContractId == ContractId
                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
        }

        public ContractCompliance ContractActualCompliance(long ContractId)
        {
            return _ObjUnitWork.ContractCompliance.Search(x => x.ActualValue == true && x.ContractId == ContractId
                && x.RowStatusId == (int)RowActiveStatus.Active, q => q.OrderByDescending(d => d.ContractComplianceId)).FirstOrDefault();
        }

        public IEnumerable<ContractComplianceBuilder> GetContractBuilderComplianceAll(Int64 ContractId)
        {
            return _ObjUnitWork.ContractComplianceBuilder.Search(x => x.ContractId == ContractId);
        }

        public ContractComplianceBuilder GetContractBuilderCompliance(Int64 ContractId, Int64 BuilderId)
        {
            return _ObjUnitWork.ContractComplianceBuilder.Search(x => x.ContractId == ContractId && x.BuilderId == BuilderId).FirstOrDefault();
        }

        public void SaveContractComplianceBuilder(List<ContractComplianceBuilder> ObjInsertist, List<ContractComplianceBuilder> ObjUpdateList)
        {

            foreach (var Item in ObjInsertist)
            {
                _ObjUnitWork.ContractComplianceBuilder.Add(Item);

            }

            foreach (var Item in ObjUpdateList)
            {
                _ObjUnitWork.ContractComplianceBuilder.Add(Item);

            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public decimal[] GetBuilderComplianceFactor(Int64 ContractId, Int64 BuilderId, bool IsOverrideConsider)
        {
            decimal EstimateValue = 0;
            decimal ActualValue = 0;
            int FlagEstimate = 0;
            int FlagActual = 0;

            ContractCompliance ObjEstimate = _ObjUnitWork.ContractCompliance.Search(x => x.ContractId == ContractId
                && x.RowStatusId == (int)RowActiveStatus.Active && x.EstimatedValue == true).FirstOrDefault();

            List<ContractCompliance> ObjActual = _ObjUnitWork.ContractCompliance.Search(x => x.ContractId == ContractId
                && x.RowStatusId == (int)RowActiveStatus.Active && x.ActualValue == true).ToList();

            if (ObjEstimate != null)
            {
                if (IsOverrideConsider)
                {
                    ContractComplianceBuilder ObjComplianceBuilder = _ObjUnitWork.ContractComplianceBuilder.
                        Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId).FirstOrDefault();

                    if (ObjComplianceBuilder != null)
                    {
                        EstimateValue = ObjComplianceBuilder.EstimatedValue;
                        FlagEstimate = 1;
                    }

                }
                if (FlagEstimate == 0)
                {
                    if (ObjEstimate.IsDirectQuestion) //direct question estimate compliance
                    {
                        SurveyResult ObjResult = _ObjUnitWork.SurveyResult.Find(x => x.SurveyId == ObjEstimate.SurveyId && x.QuestionId == ObjEstimate.QuestionId
                            && x.BuilderId == BuilderId
                            && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                        if (ObjResult != null)
                        {
                            EstimateValue = Convert.ToDecimal(ObjResult.Answer);
                        }

                    }
                    else  // we will calculate latter for equation - direct question compliance
                    {

                    }
                }
            }
            if (ObjActual != null)
            {
                if (IsOverrideConsider)
                {
                    ContractComplianceBuilder ObjComplianceBuilder = _ObjUnitWork.ContractComplianceBuilder.
                        Search(x => x.BuilderId == BuilderId && x.ContractId == ContractId).FirstOrDefault();

                    if (ObjComplianceBuilder != null)
                    {
                        ActualValue = ObjComplianceBuilder.ActualValue;
                        FlagActual = 1;
                    }

                }

                if (FlagActual == 0)
                {
                    List<Int64> QuestionIdList = ObjActual.Where(x => x.IsDirectQuestion == true).Select(x => x.QuestionId.GetValueOrDefault()).ToList<Int64>();
                    decimal TotalActualCompliance = _ObjUnitWork.ContractCompliance.GetBuilderTotalActualValue(BuilderId, ContractId, QuestionIdList);

                    ActualValue = TotalActualCompliance;

                    //if (ObjActual.IsDirectQuestion) //direct question actual compliance
                    //{
                    //    SurveyResult ObjResult = _ObjUnitWork.SurveyResult.Search(x => x.SurveyId == ObjActual.SurveyId && x.BuilderId == BuilderId &&
                    //        x.QuestionId == ObjActual.QuestionId
                    //        && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();

                    //    if (ObjResult != null)
                    //    {
                    //        ActualValue = Convert.ToDecimal(ObjResult.Answer);
                    //    }
                    //}
                    //else  // we will calculate latter equation - actual compliance
                    //{

                    //}
                }
            }
            return new decimal[] { EstimateValue, ActualValue };
        }
        public IEnumerable<ContractCompliance> GetEstimatedValueCompliance(Int64 ContractId)
        {
            return _ObjUnitWork.ContractCompliance.Search(x => x.ContractId == ContractId && x.EstimatedValue == true && x.RowStatusId == (int)RowActiveStatus.Active);
            
        }
        public IEnumerable<ContractCompliance> GetActualValueCompliance(Int64 ContractId)
        {
            return _ObjUnitWork.ContractCompliance.Search(x => x.ContractId == ContractId && x.ActualValue == true && x.RowStatusId == (int)RowActiveStatus.Active);

        }
        public IEnumerable<ContractCompliance> GetActualValueCompliancePerQuater(Int64 ContractId, Int64 QuaterId)
        {
            return _ObjUnitWork.ContractCompliance.Search(x => x.ContractId == ContractId && x.QuaterId==QuaterId && x.ActualValue == true && x.RowStatusId == (int)RowActiveStatus.Active);
        }
        public IEnumerable<ContractComplianceBuilder> GetContractBuilderComplianceNew(Int64 ContractId, Int64 BuilderId)
        {
            return _ObjUnitWork.ContractComplianceBuilder.Search(x => x.ContractId == ContractId && x.BuilderId == BuilderId);
        }
        public ContractCompliance ContractActualComplianceWithQuater(long ContractId, long QuaterId)
        {
            return _ObjUnitWork.ContractCompliance.Search(x => x.ActualValue == true && x.ContractId == ContractId && x.QuaterId == QuaterId
                && x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
        }
       public decimal GetBuilderActualValueWithQuater(Int64 BuilderId, Int64 ContractId, List<Int64> QuestionidList, List<Int64> QuaterIdList)
        {
            return _ObjUnitWork.ContractCompliance.GetBuilderActualValueWithQuater(BuilderId, ContractId, QuestionidList, QuaterIdList);
        }
    }
}