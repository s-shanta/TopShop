using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TopShop.Models.Data;
using TopShop.Models.ViewModels.Pages;

namespace TopShop.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index()
        {
            List<PageVM> pagesList;
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
                return View(pagesList);
        }
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();

        }
        
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                String slug;

                PageDTO dto = new PageDTO();

                dto.Title = model.Title;

                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                if(db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exists");
                    return View(model);
                }

                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                db.Pages.Add(dto);
                db.SaveChanges();
            }

            TempData["SM"] = "Added a new Page !";

            return RedirectToAction("AddPage");
        }
    }
}