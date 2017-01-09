using Assimil.Core;
using System;
using System.Web.Mvc;

namespace Assimil.Web.Controllers
{
    public class AssimilController : Controller
    {
        //
        //// GET: /Assimil/
        private IAssimil _assRepo;

        public AssimilController(IAssimil assRepo)
        {
            _assRepo = assRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAssimils(int page, int itemsByPage)
        {
            Assimil.Domain.PagingAssimil list = null;

            try
            {
                list = _assRepo.Get(page, itemsByPage);

                int numTotalPage = (int)Math.Ceiling((double)(list.Assimil_count / itemsByPage));

                if (page > numTotalPage + 1)
                {
                   page = numTotalPage + 1;

                   list = _assRepo.Get(page, itemsByPage);
                }
            }
            catch (Exception ex)
            {
            }

            return Json(new { Data = list.Assimils, NumItems = list.Assimil_count, CurrentP = page, ItemsByPage = itemsByPage }, JsonRequestBehavior.AllowGet);
        }

    }
}
