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
    public class ResourceController : Controller
    {
        private readonly IResourceServices _ObjResourceService;
        public ResourceController(IResourceServices ObjResourceService)
        {
            this._ObjResourceService = ObjResourceService;
        }

        // GET: Resource
        public ActionResult Index()
        {
            //using (var db = new CBUSADbContext())
            //{
            //    var CategoryList = db.Categories.Select(c => new
            //    {
            //        CategoryId = c.CategoryId,
            //        CategoryName = c.CategoryName
            //    }).ToList();
            //    ViewBag.Category = new SelectList(CategoryList, "CategoryId", "CategoryName");

            //    //var MarketList = db.Markets.Select(c => new
            //    //{
            //    //    AELLCId = c.AELLCId,
            //    //    LLCName = c.LLCName
            //    //}).ToList();
            //   // ViewBag.Market = new SelectList(MarketList, "AELLCId", "LLCName");

            //}
            return View();
        }

        [HttpPost]
        public ActionResult Create(Resource model, string Market)
        {
            Resource obj = new Resource();
            try
            {
                if (ModelState.IsValid)
                {
                    //obj.CategoryId = Convert.ToInt32(model.CategoryId);
                    //obj.FileLocation = model.FileLocation;
                    //obj.FileName = model.FileName;
                    //obj.Title = model.Title;
                    //obj.Description = model.Description;
                    //_ObjResourceService.SaveResource(model);

                    string s = Market;
                    string[] values = s.Split(',');
                    foreach (string items in values)
                    {
                        // Add to 
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}