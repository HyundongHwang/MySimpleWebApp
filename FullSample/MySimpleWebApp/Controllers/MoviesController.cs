using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MySimpleWebApp.Models;
using System.IO;

namespace MySimpleWebApp.Controllers
{
    public class MoviesController : Controller
    {
        private mydbEntities db = new mydbEntities();

        // GET: Movies
        public async Task<ActionResult> Index()
        {
            var list = await db.Movies.ToListAsync();
            return View(list);
        }

        // GET: Movies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 http://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Movie movie, HttpPostedFileBase thumbnailImg)
        {
            if (thumbnailImg != null)
            {
                var dir = HttpContext.Server.MapPath("/UploadedFiles");

                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                var fileName = Path.Combine(dir, Path.GetFileName(thumbnailImg.FileName));
                fileName = _MakeUniqueFilePath(fileName);

                using (var fs = System.IO.File.Create(fileName))
                {
                    thumbnailImg.InputStream.Seek(0, SeekOrigin.Begin);
                    await thumbnailImg.InputStream.CopyToAsync(fs);
                }

                movie.ThumbnailImgUrl = $"https://{HttpContext.Request.Url.Host}:{HttpContext.Request.Url.Port}/UploadedFiles/{HttpUtility.UrlPathEncode(Path.GetFileName(fileName))}";
            }

            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }




            return View(movie);
        }

        private string _MakeUniqueFilePath(string path)
        {
            var dir = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);

            for (int i = 2; ; i++)
            {
                if (!System.IO.File.Exists(path))
                    break;

                path = Path.Combine(dir, $"{fileName} {i}{fileExt}");
            }

            return path;
        }



        // GET: Movies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // 초과 게시 공격으로부터 보호하려면 바인딩하려는 특정 속성을 사용하도록 설정하십시오. 
        // 자세한 내용은 http://go.microsoft.com/fwlink/?LinkId=317598을(를) 참조하십시오.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,ReleaseDate,Genre,Price")] Movie movie, HttpPostedFileBase thumbnailImg)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie movie = await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Movie movie = await db.Movies.FindAsync(id);
            db.Movies.Remove(movie);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
