using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Services;
using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Models;
using Newtonsoft.Json;

namespace CBUSA.Controllers
{
    public class ContractController : Controller
    {
        private readonly IContractServices _ObjContractService;

        public ContractController(IContractServices ObjContractService)
        {
            this._ObjContractService = ObjContractService;
        }

        public ActionResult Index()
        {
            IEnumerable<Contract> obj = _ObjContractService.GetContract();
            return View(obj);
        }

        public ActionResult Create()
        {
            //using (var db = new CBUSADbContext())
            //{

            //    var StatusList = db.Status.Select(c => new
            //    {
            //        StatusId = c.StatusId,
            //        StatusName = c.StatusName
            //    }).ToList();
            //    ViewBag.Status = new SelectList(StatusList, "StatusId", "StatusName");

            //    var CategoryList = db.ProductCategorys.Select(c => new
            //    {
            //        AEProductCategoryId = c.AEProductCategoryId,
            //        ProductCategoryName = c.ProductCategoryName
            //    }).ToList();
            //    ViewBag.Category = new SelectList(CategoryList, "AEProductCategoryId", "ProductCategoryName");

            //    var ManufacturerList = db.Manufacturers.Select(c => new
            //    {
            //        ManufacturerId = c.ManufacturerId,
            //        ManufacturerName = c.ManufacturerName
            //    }).ToList();
            //    ViewBag.Manufacturer = new SelectList(ManufacturerList, "ManufacturerId", "ManufacturerName");

            //    var ChilDlist = (from y in db.ProductCategorys
            //                     where y.ProductCategoryParentId != 0
            //                     select new ProductCategoryView
            //                     {
            //                         CatId = y.AEProductCategoryId,
            //                         ParentId = y.ProductCategoryParentId,
            //                         CatName = y.ProductCategoryName

            //                     }).ToList();

            //    var ParentList = (from y in db.ProductCategorys
            //                      where y.ProductCategoryParentId == 0
            //                      select new ProductCategoryView
            //                      {
            //                          CatId = y.AEProductCategoryId,
            //                          ParentId = y.ProductCategoryParentId,
            //                          CatName = y.ProductCategoryName
            //                      }).ToList();

            //    foreach (ProductCategoryView p in ParentList)
            //    {

            //        int dd = p.CatId;
            //        int ff = ChilDlist.Where(x => x.ParentId == dd).Count();
            //        //p.CatName = "Rabi";

            //        p.ChildCategoryList = ChilDlist.Where(x => x.ParentId == dd).Select(x => new ProductCategoryView { CatId = x.CatId, CatName = x.CatName });
            //    }


            //    //var CatList = (from y in db.ProductCategorys
            //    //               where y.ProductCategoryParentId == 0
            //    //               select new ProductCategoryView
            //    //               {
            //    //                   CatName = y.ProductCategoryName,
            //    //                   ChildCategoryList = ChilDlist.Where(x => x.ParentId == y.AEProductCategoryId)

            //    //               });

            //    ViewBag.CategoryList = JsonConvert.SerializeObject(ParentList);

            //    var ProdList = (from x in db.Products
            //                    join y in db.ProductCategorys
            //                    on x.ProductCategoryId equals y.AEProductCategoryId
            //                    select new ProductCategoryView
            //                    {
            //                        AEProductId = x.AEProductId,
            //                        ProductName = x.ProductName,
            //                        CatName = y.ProductCategoryName
            //                    }).ToList();



            //    ViewBag.ProductList = new SelectList(ProdList, "AEProductId", "ProductName", "CatName", 0);

            //}

            return View();
        }


        [HttpGet]
        public JsonResult ProductList(int Id)
        {
            //using (var db = new CBUSADbContext())
            //{
            //    var state = from s in db.Products
            //                where s.ProductCategoryId == Id
            //                select s;
            //    return Json(new SelectList(state.ToArray(), "AEProductId", "ProductName"), JsonRequestBehavior.AllowGet);
            //}

            return Json("");
        }

        [HttpPost]
        public ActionResult Create(ContractView model)
        {

            try
            {
                ModelState.Remove("EstimatedStartDate");
                ModelState.Remove("EntryDeadline");
                ModelState.Remove("ContrctFrom");
                ModelState.Remove("ContrctTo");
                ModelState.Remove("ContractDeliverables");

                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here
                    Contract contract = new Contract();
                    contract.ContractName = model.ContractName.ToString();
                    contract.Label = model.Label.ToString();
                    // contract.StatusId = Convert.ToInt16(model.StatusId);
                    //contract.EstimatedStartDate = Convert.ToDateTime(model.EstimatedStartDate);
                    if (Convert.ToString(model.EstimatedStartDate) == "" || model.EstimatedStartDate == null)
                    {
                        // contract.EstimatedStartDate = null;
                    }
                    else
                    {
                        contract.EstimatedStartDate = Convert.ToDateTime(model.EstimatedStartDate);
                    }

                    //contract.EntryDeadline = Convert.ToDateTime(model.EntryDeadline);
                    if (Convert.ToString(model.EntryDeadline) == "" || model.EntryDeadline == null)
                    {
                        // contract.EntryDeadline = null;
                    }
                    else
                    {
                        contract.EntryDeadline = Convert.ToDateTime(model.EntryDeadline);
                    }

                    //contract.ContrctFrom = Convert.ToString(model.ContrctFrom) == "" ? null : Convert.ToDateTime(model.ContrctFrom);
                    if (Convert.ToString(model.ContrctFrom) == "" || model.ContrctFrom == null)
                    {
                        contract.ContrctFrom = null;
                    }
                    else
                    {
                        contract.ContrctFrom = Convert.ToDateTime(model.ContrctFrom);
                    }

                    //contract.ContrctTo = Convert.ToDateTime(model.ContrctTo);
                    if (Convert.ToString(model.ContrctTo) == "" || model.ContrctTo == null)
                    {
                        contract.ContrctTo = null;
                    }
                    else
                    {
                        contract.ContrctTo = Convert.ToDateTime(model.ContrctTo);
                    }

                    contract.ContractDeliverables = Convert.ToString(model.ContractDeliverables);
                   // contract.Manufacturer = Convert.ToString(model.Manufacturer);

                    contract.ManufacturerId = Convert.ToInt32(model.ManufacturerId);
                    contract.Website = Convert.ToString(model.Website);
                    //   contract.AEProductCategoryId = Convert.ToInt32(model.AEProductCategoryId);
                    //  contract.AEProductId = Convert.ToInt32(model.AEProductId);
                   // _ObjContractService.SaveContract(contract);
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ResourceView(int Id)
        {
            return PartialView("_Resource");
        }

        //public ActionResult CreateResource()
        //{

        //}
        // [HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // TODO: Add insert logic here
        //            Contract contract = new Contract();
        //            contract.ContractName = collection["ContractName"].ToString();
        //            contract.Label = collection["Label"].ToString();
        //            contract.StatusId = Convert.ToInt16(collection["StatusId"]);

        //            //contract.EstimatedStartDate = Convert.ToDateTime(collection["EstimatedStartDate"]);
        //            if (collection["EstimatedStartDate"] != null || collection["EstimatedStartDate"] != "")
        //            {
        //                contract.EstimatedStartDate = Convert.ToDateTime(collection["EstimatedStartDate"]);
        //            }
        //            else
        //            {
        //                contract.EstimatedStartDate = null;
        //            }


        //            //contract.EntryDeadline = Convert.ToDateTime(collection["EntryDeadline"]);
        //            if (collection["EntryDeadline"] != null || collection["EntryDeadline"] != "")
        //            {
        //                contract.EntryDeadline = Convert.ToDateTime(collection["EntryDeadline"]);
        //            }
        //            else
        //            {
        //                contract.EntryDeadline = null;
        //            }

        //            //contract.ContrctFrom = Convert.ToDateTime(collection["ContrctFrom"]);
        //            if (collection["ContrctFrom"] != null || collection["ContrctFrom"] != "")
        //            {
        //                contract.ContrctFrom = Convert.ToDateTime(collection["ContrctFrom"]);
        //            }
        //            else
        //            {
        //                contract.ContrctFrom = null;
        //            }


        //            //contract.ContrctTo = Convert.ToDateTime(collection["ContrctTo"]);
        //            if (collection["ContrctTo"] != null || collection["ContrctTo"] != "")
        //            {
        //                contract.ContrctTo = Convert.ToDateTime(collection["ContrctTo"]);
        //            }
        //            else
        //            {
        //                contract.ContrctTo = null;
        //            }


        //            contract.ContractDeliverables = Convert.ToString(collection["ContractDeliverables"]);
        //            contract.Manufacturer = Convert.ToString(collection["Manufacturer"]);
        //            contract.ManufacturerId = Convert.ToInt32(collection["ManufacturerId"]);
        //            contract.Website = Convert.ToString(collection["Website"]);
        //            contract.AEProductCategoryId = Convert.ToInt32(collection["AEProductCategoryId"]); ;
        //            contract.AEProductId = Convert.ToInt32(collection["AEProductId"]); ;
        //            _ObjContractService.SaveContract(contract);
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

    }
}