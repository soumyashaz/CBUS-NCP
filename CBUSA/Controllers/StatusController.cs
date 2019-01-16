using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Services;
using CBUSA.Domain;
using CBUSA.Repository;
using CBUSA.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Data.Entity;

namespace CBUSA.Controllers
{
    public class StatusController : Controller
    {
        private readonly IStatusServices _ObjStatusServices;

        public StatusController(IStatusServices ObjStatusServices)
        {
            this._ObjStatusServices = ObjStatusServices;
        }

        public ActionResult Index()
        {
            return View();
        }


        //public ActionResult Status_Read([DataSourceRequest]DataSourceRequest request)
        //{
        //    IEnumerable<Status> obj = _ObjStatusServices.GetStatus();
        //    DataSourceResult result = obj.ToDataSourceResult(request, s => new
        //    {
        //        StatusId = s.StatusId,
        //        StatusName = s.StatusName
        //    });

        //    return Json(result);
        //}

        //// GET: Status/Details/5
        ////public ActionResult Details(int id)
        ////{
        ////    return View();
        ////}

        //// GET: Status/Create
        ////public ActionResult Create()
        ////{
        ////    return View();
        ////}

        //// POST: Status/Create
        ////[HttpPost]
        ////public ActionResult Create(ViewStatus model)
        ////{
        ////    try
        ////    {
        ////        Status status = new Status();
        ////        status.StatusName = model.StatusName;
        ////        // TODO: Add insert logic here
        ////        _ObjStatusServices.SaveStatus(status);
        ////        return RedirectToAction("Index");
        ////    }
        ////    catch
        ////    {
        ////        return View();
        ////    }
        ////}

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Status_Create([DataSourceRequest] DataSourceRequest request, Status model)
        //{
        //    if (model != null && ModelState.IsValid)
        //    {
        //        Status obj = new Status();
        //        obj.StatusName = model.StatusName;
        //        _ObjStatusServices.SaveStatus(obj);
        //    }

        //    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        //}

        //public ActionResult Status_Update([DataSourceRequest]DataSourceRequest request, Status model)
        //{
        //    if (model != null & ModelState.IsValid)
        //    {
        //        Status obj = new Status();
        //        obj.StatusId = model.StatusId;
        //        obj.StatusName = model.StatusName;
        //        _ObjStatusServices.EditStatus(obj);
        //    }
        //    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        //}

        //public ActionResult Status_Delete([DataSourceRequest]DataSourceRequest request, Status model)
        //{
        //    if (model != null & ModelState.IsValid)
        //    {
        //        Status obj = new Status();
        //        obj.StatusId = model.StatusId;
        //        _ObjStatusServices.DeleteStatus(obj);
        //    }
        //    return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        //}

        //// GET: Status/Edit/5
        ////public ActionResult Edit(int id)
        ////{
        ////    return View();
        ////}

        ////// POST: Status/Edit/5
        ////[HttpPost]
        ////public ActionResult Edit(int id, FormCollection collection)
        ////{
        ////    try
        ////    {
        ////        // TODO: Add update logic here

        ////        return RedirectToAction("Index");
        ////    }
        ////    catch
        ////    {
        ////        return View();
        ////    }
        ////}

        //// GET: Status/Delete/5
        ////public ActionResult Delete(int id)
        ////{
        ////    return View();
        ////}

        ////// POST: Status/Delete/5
        ////[HttpPost]
        ////public ActionResult Delete(int id, FormCollection collection)
        ////{
        ////    try
        ////    {
        ////        // TODO: Add delete logic here

        ////        return RedirectToAction("Index");
        ////    }
        ////    catch
        ////    {
        ////        return View();
        ////    }
        ////}
    }
}
