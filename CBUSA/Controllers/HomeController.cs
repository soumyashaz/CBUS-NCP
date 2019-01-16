using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CBUSA.Services;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Models;
using System.Security.Claims;
namespace CBUSA.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        readonly IProductCategoryService _ObjProductCategoryService;
        readonly IZoneService _ObjZoneService;
        readonly ISurveyService _ObjSurveyService;
        readonly IResourceService _ObjResourceService;
        public HomeController(IProductCategoryService ObjProductCategoryService, IZoneService ObjZoneService, ISurveyService ObjSurveyService
            , IResourceService ObjResourceService
            )
        {
            _ObjProductCategoryService = ObjProductCategoryService;
            _ObjZoneService = ObjZoneService;
            _ObjSurveyService = ObjSurveyService;
            _ObjResourceService = ObjResourceService;
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {

            // int vv = 0;
            //int v = 3 / vv;

            //string Surbject = "Test Subject current";
            //String Body = "<h2>This is test email current</h2>";
            //string MailTo = "adas@medullus.com";
            //IEmailSendApi Obj = new EmailSendApi();
            //Obj.Send(Surbject, Body, MailTo);

            return View();
        }

        public JsonResult ParseProductTreeViewControl(int? CategoryId)
        {
            var ProductSubCategory = _ObjProductCategoryService.GetProductCategory()

                                  .Where(x => x.ParentId != 0);

            var ProductCategory = _ObjProductCategoryService.GetProductCategory()

                                    .Where(x => CategoryId.HasValue ? x.ParentId == CategoryId : x.ParentId == 0).OrderBy(x => x.ProductCategoryName);
            // Project the results to avoid JSON serialization errors
            var result = ProductCategory.Select(x => new CategoryTreeViewModel
            {
                CategoryId = x.ProductCategoryId,
                CategoryName = x.ProductCategoryName,
                HasSubCategory = ProductSubCategory.Any(y => y.ParentId == x.ProductCategoryId)
            })
            .ToList();


            //IEnumerable<CategoryTreeViewModel> ob = new List<CategoryTreeViewModel> {
            // new CategoryTreeViewModel{CategoryId=1,CategoryName="t1",HasSubCategory=true},
            // new CategoryTreeViewModel{CategoryId=1,CategoryName="t1",HasSubCategory=false}
            //};

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult ParseProductControl(List<Int64> SubCategories, List<Int64> SubCategoryHistory, int Flag)
        {


            var ProductSubCategoryTemp = SubCategories.Where(x => !SubCategoryHistory.Contains(x));
            // var ProductSubCategoryTemp = SubCategories.Except(SubCategoryHistory);

            var RemoveList = SubCategoryHistory.Except(SubCategories);

            if (ProductSubCategoryTemp.Count() > 0)
            {
                var ObjProductCategory = _ObjProductCategoryService.GetProductCategory().Join(ProductSubCategoryTemp, x => x.ProductCategoryId, y => y,
                                             (x, y) => x);
                ProductControlVM ObjVm = new ProductControlVM();
                ObjVm.ObjProductCategory = ObjProductCategory.OrderBy(x => x.ProductCategoryName).ToList();
                ObjVm.Flag = Flag;
                return Json(new { IsAppendRequired = true, ProductCustomControl = PartialView("_ProductCustomcontrol", ObjVm).RenderToString(), RemoveList = RemoveList }
                    , JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsAppendRequired = false, RemoveList = RemoveList }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult ParseZoneStateControl(string Flag, Int64? Id)
        {
            var ZoneList = _ObjZoneService.GetZoneAll();

            switch (Flag)
            {
                case "SurveyMarket":
                    var SurveyMarket = _ObjSurveyService.GetSurveyMarket(Id.GetValueOrDefault());
                    if (SurveyMarket != null)
                    {
                        ViewBag.SelectedMarket = string.Join(",", SurveyMarket.Select(x => x.MarketId).ToList());
                    }
                    else
                    {
                        ViewBag.SelectedMarket = 0;
                    }
                    break;
                case "ResourceMarket":
                    var ResourceMarket = _ObjResourceService.GetResourceMarket(Id.GetValueOrDefault());
                    if (ResourceMarket != null)
                    {
                        ViewBag.SelectedMarket = string.Join(",", ResourceMarket.Select(x => x.MarketId).ToList());
                    }
                    else
                    {
                        ViewBag.SelectedMarket = 0;
                    }

                    break;
            }




            return Json(new { ProductCustomControl = PartialView("_ZoneCustomControl", ZoneList).RenderToString() }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [Authorize(Roles = "Builder")]
        public ActionResult GetUserName()
        {
            string UserName = string.Empty;
            var identity = (ClaimsIdentity)User.Identity;

            //if (identity != null)
            //{
            IEnumerable<Claim> claims = identity.Claims;
            UserName = claims.FirstOrDefault(p => p.Type == ClaimTypes.Name).Value;
            // }

            return PartialView("_UserNameView", UserName);
        }
        [Authorize(Roles = "Builder")]
        [ChildActionOnly]
        public ActionResult GetUserId()
        {
            string UserName = string.Empty;
            var identity = (ClaimsIdentity)User.Identity;
            //if (identity != null)
            //{
            IEnumerable<Claim> claims = identity.Claims;
            UserName = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            //   }
            return PartialView("_UserNameView", UserName);
        }

    }
}