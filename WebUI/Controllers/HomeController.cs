using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebUI.Data.Entities;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private UygunWebAdresiBlogEntities _db = new UygunWebAdresiBlogEntities();
      
        [Route("")]
        [Route("anasayfa")]
        public ActionResult Index(int page=1)
        {
            var result = _db.Posts.ToList().OrderByDescending(x => x.PostId).ToPagedList(page,6);
            return View(result);
        }

        [Route("{PostTitle}-{id:int}")]
        [HttpGet]
        public ActionResult ViewPost(int id)
        {
            var result = _db.Posts.Find(id);
            if (result!=null)
            {
                return View(result);
            }

            return View();
        }

        [HttpGet]
        public ActionResult PartialCategory()
        {
            var result = _db.Categories.Include("Posts").ToList().OrderBy(x => x.CategoryName);
            return PartialView(result);
        }

        [Route("kategori/{CategoryName}-{id:int}")]
        [HttpGet]
        public ActionResult Category(int id, int page=1)
        {
            var result = _db.Posts.Include("Categories").OrderByDescending(x => x.PostId)
                .Where(x => x.Categories.CategoryId == id).ToList().ToPagedList(page, 6);
            return View(result);
        }

        [HttpGet]
        public ActionResult PartialArchive()
        {
            var result = _db.Archives.Include("Posts").ToList().OrderBy(x => x.ArchiveName);
            return PartialView(result);
        }

        [Route("arsiv/{CategoryName}-{id:int}")]
        [HttpGet]
        public ActionResult Archive(int id, int page = 1)
        {
            var result = _db.Posts.Include("Archives").OrderByDescending(x => x.PostId)
                .Where(x => x.Archives.ArchiveId == id).ToList().ToPagedList(page, 6);
            return View(result);
        }
    }
}