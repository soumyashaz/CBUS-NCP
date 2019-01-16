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
    public class ContractServices : CBUSA.Services.IContractServices
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public ContractServices(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Contract> GetContract()
        {
            var list = _ObjUnitWork.Contracts.GetAll();
            return list;
        }

        //public void SaveContract(Contract ObjContract)
        //{

        //}

        //public void SaveContract(Contract ObjContract)
        //{
        //    _ObjUnitWork.Contracts.Add(ObjContract);
        //    _ObjUnitWork.Complete();
        //}

        //public void EditContract(Contract ObjContract)
        //{
        //    _ObjUnitWork.Contracts.Equals(ObjContract);
        //    _ObjUnitWork.Complete();
        //}


        public bool IsContractNameAvailable(string ContractName)
        {
            return _ObjUnitWork.Contracts.IsContractNameAvailable(ContractName);
        }

        public bool IsContractLabelAvailable(string ContractLabelName)
        {
            return _ObjUnitWork.Contracts.IsContractLabelAvailable(ContractLabelName);
        }


        public void SaveContract(Contract ObjContract, List<string> ObjContractProduct,
            ContractStatusHistory ObjContractStatusHistory, List<Resource> ObjResource)
        {
            _ObjUnitWork.Contracts.Add(ObjContract);
            //ObjContractStatusHistory.ContractId = ObjContract.ContractId;
            _ObjUnitWork.ContractStatusHistory.Add(ObjContractStatusHistory);
            //  _ObjUnitWork.ContractProduct.AddRange(ObjContractProduct);

            ///Added by Rabi on 29 March
            ///

            foreach (var Product in ObjContractProduct)
            {
                var ExistingProduct = _ObjUnitWork.Product.Search(x => x.ProductName.ToLower() == Product.ToLower()).FirstOrDefault();
                var ExistingProductcategory = _ObjUnitWork.ProductCategory.Search(x => x.ProductCategoryName.ToLower() == "all").FirstOrDefault();
                ContractProduct ObjContractproduct = null;
                if (ExistingProduct != null)
                {
                    ObjContractproduct = new ContractProduct { ContractId = ObjContract.ContractId, ProductId = ExistingProduct.ProductId };

                    //_ObjUnitWork.ContractStatusHistory.Add(ObjContractproduct);
                }
                else
                {
                    Domain.Product ObjNewProduct = new Domain.Product
                    {
                        ProductName = Product,
                        ProductCategoryId = ExistingProductcategory.ProductCategoryId
                       ,
                        RowStatusId = (int)RowActiveStatus.Active,
                        CreatedBy = 1,
                        ModifiedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        RowGUID = Guid.NewGuid()
                    };
                    SaveProductInDb(ref ObjNewProduct);
                    ObjContractproduct = new ContractProduct { ContractId = ObjContract.ContractId, ProductId = ObjNewProduct.ProductId };
                }


                _ObjUnitWork.ContractProduct.Add(ObjContractproduct);

            }
            
            //foreach (var Item in ObjContractProduct)
            //{
            //    _ObjUnitWork.ContractProduct.Add(Item);
            //}
            _ObjUnitWork.Complete();

            //_ObjUnitWork.Contracts.Detach(ObjContract);
            foreach (var Item in ObjResource)
            {

                Item.ContractId = ObjContract.ContractId;
                _ObjUnitWork.Resource.Update(Item);
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        void SaveProductInDb(ref Domain.Product Objproduct)
        {
            _ObjUnitWork.Product.Add(Objproduct);
            _ObjUnitWork.Complete();
        }


        public IEnumerable<Resource> GetResourceofDump(string DumpId)
        {
            return _ObjUnitWork.Resource.Find(x => x.DumpId == DumpId);
        }


        public IEnumerable<Contract> GetActiveContract()
        {
            return _ObjUnitWork.Contracts.GetActiveContract();
        }
        public IEnumerable<Contract> GetActiveContractDescending()
        {
            return _ObjUnitWork.Contracts.GetActiveContractDescending();
        }
        public IEnumerable<Contract> GetArchievedContractDescending()
        {
            return _ObjUnitWork.Contracts.GetArchivedontractDescending();
        }

        public Contract GetContract(long ContractId)
        {
            return _ObjUnitWork.Contracts.Get(ContractId);
        }
        public Contract GetBuilderCount(long ContractId)
        {
            return _ObjUnitWork.Contracts.Get(ContractId);
        }

        public void EditContract(Contract ObjContract, ContractStatusHistory ObjContractStatusHistory)
        {
            _ObjUnitWork.Contracts.Update(ObjContract);
            _ObjUnitWork.ContractStatusHistory.Add(ObjContractStatusHistory);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }


        public void SaveContractProduct(List<string> ObjContractProduct,Int64 ContractId)
        {
            /*Added by Rabi on 29 Mar 2017
            foreach (var Item in ObjContractProduct)
            {
                if (!_ObjUnitWork.ContractProduct.Find(x => x.ProductId == Item.ProductId && x.ContractId == Item.ContractId).Any())
                {
                    _ObjUnitWork.ContractProduct.Add(Item);
                }
            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
            */

            foreach (var Product in ObjContractProduct)
            {
                var ExistingProduct = _ObjUnitWork.Product.Search(x => x.ProductName.ToLower() == Product.ToLower()).FirstOrDefault();
                var ExistingProductcategory = _ObjUnitWork.ProductCategory.Search(x => x.ProductCategoryName.ToLower() == "all").FirstOrDefault();
                ContractProduct ObjContractproduct = null;
                if (ExistingProduct != null)
                {
                    if (!_ObjUnitWork.ContractProduct.Search(x => x.ProductId == ExistingProduct.ProductId && x.ContractId == ContractId).Any())
                    {
                        ObjContractproduct = new ContractProduct { ContractId = ContractId, ProductId = ExistingProduct.ProductId };
                        _ObjUnitWork.ContractProduct.Add(ObjContractproduct);
                    }
                    //_ObjUnitWork.ContractStatusHistory.Add(ObjContractproduct);
                }
                else
                {
                    Domain.Product ObjNewProduct = new Domain.Product
                    {
                        ProductName = Product,
                        ProductCategoryId = ExistingProductcategory.ProductCategoryId
                       ,
                        RowStatusId = (int)RowActiveStatus.Active,
                        CreatedBy = 1,
                        ModifiedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now,
                        RowGUID = Guid.NewGuid()
                    };
                    SaveProductInDb(ref ObjNewProduct);
                    ObjContractproduct = new ContractProduct { ContractId = ContractId, ProductId = ObjNewProduct.ProductId };
                    _ObjUnitWork.ContractProduct.Add(ObjContractproduct);
                }
               
            }

            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();

        }

        public void UpdateContractProduct(List<ContractProduct> ObjContractProduct)
        {
            Int64 ContractId = ObjContractProduct.FirstOrDefault().ContractId;

            Int64[] ProductIdList = ObjContractProduct.Select(x => x.ProductId).ToArray();

            if (ProductIdList.Count() > 0)
            {
                List<ContractProduct> DeletedLis = _ObjUnitWork.ContractProduct.Find(x => ProductIdList.Contains(x.ProductId) && x.ContractId == ContractId).ToList();
                foreach (var Item in DeletedLis)
                {
                    _ObjUnitWork.ContractProduct.Remove(Item);

                }
                _ObjUnitWork.Complete();
                _ObjUnitWork.Dispose();
            }


            //ContractProduct _ObjContractProduct = _ObjUnitWork.ContractProduct.Find(x => ObjContractProduct.Select(y=>y.ContractId).
            //    //Contains(z=>z.co  && x.ContractId == Item.ContractId).FirstOrDefault();


            //foreach (var Item in ObjContractProduct)
            //{
            //    ContractProduct _ObjContractProduct = _ObjUnitWork.ContractProduct.Find(x => x.ProductId == Item.ProductId && x.ContractId == Item.ContractId).FirstOrDefault();

            //    if (_ObjContractProduct != null)
            //    {
            //        _ObjUnitWork.ContractProduct.Remove(Item);
            //    }



            //}
            //_ObjUnitWork.Complete();
            //_ObjUnitWork.Dispose();
        }

        public void UpdateContractBuilder(List<ContractBuilder> ObjContractBuilder)
        {
            foreach (var Item in ObjContractBuilder)
            {
                ContractBuilder _ObjContractBuilder = _ObjUnitWork.ContractBuilder.Find(x => x.ContractId == Item.ContractId &&
                    x.BuilderId == Item.BuilderId).Where(x => x.RowStatusId == (int)RowActiveStatus.Active).FirstOrDefault();
                if (_ObjContractBuilder != null)
                {
                    _ObjContractBuilder.RowStatusId = (int)RowActiveStatus.Deleted;
                    _ObjUnitWork.ContractBuilder.Update(_ObjContractBuilder);
                }
            }
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }

        public int GetMarketCount()
        {
            return _ObjUnitWork.Market.GetAll().Where(x => x.RowStatusId == (int)RowActiveStatus.Active).Count();
        }




        public IEnumerable<Contract> GetActivePendingContract()
        {
            return _ObjUnitWork.Contracts.GetPendingContract();
            //return _ObjUnitWork.Contracts.Find(x => (x.ContrctTo == null || x.ContrctTo.GetValueOrDefault().Date >
            //    DateTime.Now.Date) && x.RowStatusId == (int)RowActiveStatus.Active && x.ContractStatus<>1);
        }

        public IEnumerable<Contract> GetActivePendingContractList()
        {
            return _ObjUnitWork.Contracts.Find(x => x.RowStatusId == (int)RowActiveStatus.Active);
        }



        public IEnumerable<Builder> GetAssociateBuilderWithContract(Int64 ContractId)
        {
            return _ObjUnitWork.Contracts.GetAssocaitedBuilderWithContract(ContractId);
        }
        public IEnumerable<Contract> GetNonAssociateContractWithBuilder(Int64 BuilderId, string Flag)
        {
            return _ObjUnitWork.Contracts.GetNonAssociatedContract(BuilderId, Flag);
        }

        //public IEnumerable<Contract> GetActiveContractwithBuilderCount()
        //{
        //    return _ObjUnitWork.Contracts.GetActiveContract();
        //}
    }
}
