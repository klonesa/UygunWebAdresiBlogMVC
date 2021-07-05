using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebUI.Data.Entities;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private UygunWebAdresiBlogEntities _db = new UygunWebAdresiBlogEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Blogs()
        {
            var result = _db.Posts.ToList().OrderByDescending(x => x.PostId);
            return View(result);
        }

        [HttpGet]
        public ActionResult AddBlog()
        {
            @ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            @ViewBag.ArchiveId = new SelectList(_db.Archives, "ArchiveId", "ArchiveName");
            return View();
        }

        [HttpPost]
        public ActionResult AddBlog(Posts entity, HttpPostedFileBase image)
        {
            if (image != null)
            {
                WebImage img = new WebImage(image.InputStream);
                FileInfo imginfo = new FileInfo(image.FileName);

                string blogImg = Guid.NewGuid().ToString() + imginfo.Extension;
                img.Save("~/Img/" + blogImg);

                entity.PostPhoto = "/Img/" + blogImg;
            }

            var result = _db.Posts.Add(entity);
            if (result != null)
            {
                _db.SaveChanges();
                return RedirectToAction("Blogs", "Admin");
            }

            return View();
        }

        [HttpGet]
        public ActionResult UpdateBlog(int id)
        {
            var result = _db.Posts.SingleOrDefault(x => x.PostId == id);
            @ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            @ViewBag.ArchiveId = new SelectList(_db.Archives, "ArchiveId", "ArchiveName");
            return View(result);
        }

        [HttpPost]
        public ActionResult UpdateBlog(Posts entity, HttpPostedFileBase image, int id)
        {
            if (ModelState.IsValid)
            {
                var result = _db.Posts.SingleOrDefault(x => x.PostId == id);
                if (image != null)
                {
                    if (result != null && System.IO.File.Exists(Server.MapPath(result.PostPhoto)))
                    {
                        System.IO.File.Delete(Server.MapPath(result.PostPhoto));
                    }

                    WebImage img = new WebImage(image.InputStream);
                    FileInfo imginfo = new FileInfo(image.FileName);

                    string blogImg = Guid.NewGuid().ToString() + imginfo.Extension;
                    img.Save("~/Img/" + blogImg);

                    if (result != null) result.PostPhoto= "/Img/" + blogImg;
                }

                if (result != null)
                {
                    result.PostTitle = entity.PostTitle;
                    result.PostContent = entity.PostContent;
                    result.ArchiveId = entity.ArchiveId;
                    result.CategoryId = entity.CategoryId;
                    result.PostDateTime = entity.PostDateTime;
                }

                _db.SaveChanges();
                return RedirectToAction("Blogs", "Admin");
            }

            return View(entity);
        }

        public ActionResult DeleteBlog(int id)
        {
            var result = _db.Posts.Find(id);
            if (result!=null)
            {
                _db.Posts.Remove(result);
                _db.SaveChanges();
                return RedirectToAction("Blogs", "Admin");
            }

            return null;
        }

        [HttpGet]
        public ActionResult Categories()
        {
            var result = _db.Categories.ToList().OrderByDescending(x => x.CategoryId);
            return View(result);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Categories entity)
        {
            var result = _db.Categories.Add(entity);
            if (result!=null)
            {
                _db.SaveChanges();
                return RedirectToAction("Categories", "Admin");
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            var result = _db.Categories.SingleOrDefault(x => x.CategoryId == id);
            return View(result);
        }

        [HttpPost]
        public ActionResult UpdateCategory(int id, Categories entity)
        {
            if (ModelState.IsValid)
            {
                var result = _db.Categories.SingleOrDefault(x => x.CategoryId == id);
                if (result!=null)
                {
                    result.CategoryName = entity.CategoryName;
                }

                _db.SaveChanges();
                return RedirectToAction("Categories", "Admin");
            }

            return View();
        }

        public ActionResult DeleteCategory(int id)
        {
            var result = _db.Categories.Find(id);
            if (result!=null)
            {
                _db.Categories.Remove(result);
                _db.SaveChanges();
                return RedirectToAction("Categories", "Admin");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Archives()
        {
            var result = _db.Archives.ToList().OrderByDescending(x => x.ArchiveId);
            return View(result);
        }

        [HttpGet]
        public ActionResult AddArchive()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddArchive(Archives entity)
        {
            var result = _db.Archives.Add(entity);
            if (result!=null)
            {
                _db.SaveChanges();
                return RedirectToAction("Archives", "Admin");
            }

            return View();
        }

        [HttpGet]
        public ActionResult UpdateArchive(int id)
        {
            var result = _db.Archives.SingleOrDefault(x => x.ArchiveId == id);
            return View(result);
        }

        [HttpPost]
        public ActionResult UpdateArchive(int id, Archives entity)
        {
            if (ModelState.IsValid)
            {
                var result = _db.Archives.SingleOrDefault(x => x.ArchiveId == id);
                if (result!=null)
                {
                    result.ArchiveName = entity.ArchiveName;
                }

                _db.SaveChanges();
                return RedirectToAction("Archives", "Admin");
            }

            return View();
        }

        public ActionResult DeleteArchive(int id)
        {
            var result = _db.Archives.Find(id);
            if (result!=null)
            {
                _db.Archives.Remove(result);
                _db.SaveChanges();
                return RedirectToAction("Archives", "Admin");
            }

            return View();
        }
    }
}