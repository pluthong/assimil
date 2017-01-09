using Assimil.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assimil.Web.Controllers
{
    public class LessonController : Controller
    {
        //
        // GET: /Lesson/

        private ILesson _lessRepo;

        public LessonController(ILesson lessRepo)
        {
            _lessRepo = lessRepo;
        }
      
        public ActionResult Index(int id, string title)
        {
            ViewBag.lessonId = id;
            ViewBag.lessonTitle = title;
           
            return View();
        }


        public JsonResult GetLesson(int lessonId)
        {
            Assimil.Domain.Lesson lesson = null;

            try
            {
                lesson = _lessRepo.GetLesson(lessonId);
            }
            catch (Exception ex)
            {
            }

            return Json(lesson, JsonRequestBehavior.AllowGet);
        }
    }
}
