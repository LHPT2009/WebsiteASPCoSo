using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using webcoso.Models;
using webcoso.Models.LinQ;

namespace webcoso.Controllers
{
    public class LienHesController : Controller
    {
        private WebcosoContext db = new WebcosoContext();
        private ApplicationDbContext data = new ApplicationDbContext();

        public bool AuthAdmin()
        {
            var user = data.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return false;
            var userExist = user.Roles.FirstOrDefault(r => r.UserId == user.Id);
            if (userExist == null)
                return false;
            if (userExist.RoleId != "1")
                return false;
            return true;
        }
        // GET: LienHes
        public ActionResult LienHeAdmin(int? page)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            int pageSize = 10;
            int pageNum = page ?? 1;
            return View(db.LienHe.ToList().OrderByDescending(p => p.NgayGui).ToPagedList(pageNum, pageSize));
        }

        // GET: LienHes/Details/5
        public ActionResult Details(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienHe lienHe = db.LienHe.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // GET: LienHes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LienHes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string content, [Bind(Include = "IdLienHe,NgayGui,NoiDung,IdUser")] LienHe lienHe)
        {

            // khong xet valid IdUser vi bang user dang nhap
            ModelState.Remove("IdUser");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("LienHeAdmin");
            }

            // lay login user id
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            lienHe.IdUser = user.Id;
            lienHe.NgayGui = DateTime.Now;
            lienHe.NoiDung = content;
            db.LienHe.Add(lienHe);
            db.SaveChanges();
            return RedirectToAction("LienHeAdmin");
        }

        // GET: LienHes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienHe lienHe = db.LienHe.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // POST: LienHes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdLienHe,NgayGui,NoiDung,IdUser")] LienHe lienHe)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.Entry(lienHe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LienHeAdmin");
            }
            return View(lienHe);
        }

        // GET: LienHes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LienHe lienHe = db.LienHe.Find(id);
            if (lienHe == null)
            {
                return HttpNotFound();
            }
            return View(lienHe);
        }

        // POST: LienHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            LienHe lienHe = db.LienHe.Find(id);
            db.LienHe.Remove(lienHe);
            db.SaveChanges();
            return RedirectToAction("LienHeAdmin");
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
