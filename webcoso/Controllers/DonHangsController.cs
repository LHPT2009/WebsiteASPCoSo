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
using PagedList;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DataTable = System.Data.DataTable;
using ClosedXML.Excel;
using System.IO;

namespace webcoso.Controllers
{
    public class DonHangsController : Controller
    {
        private WebcosoContext db = new WebcosoContext();
        private MyDataDataContext data = new MyDataDataContext();
        private ApplicationDbContext dataUser = new ApplicationDbContext();

        // GET: DonHangs
        public ActionResult Index(int? page)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            var all_donHang = data.DonHangs.ToList();
            int pageSize = 10;
            int pageNum = page ?? 1;
            return View(all_donHang.ToPagedList(pageNum, pageSize));
        }

        // GET: DonHangs/Details/5
        public ActionResult Details(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.LinQ.DonHang donHang = data.DonHangs.FirstOrDefault(d => d.MaDH == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            List<Models.LinQ.ChiTietDonHang> ctdh = data.ChiTietDonHangs.Where(c => c.MaDH == donHang.MaDH).ToList();
            DonHangViewModel donHangViewModel = new DonHangViewModel()
            {
                DonHang = donHang,
                CTDH = ctdh
            };
            return View(donHangViewModel);
        }

        // GET: DonHangs/Create
        public ActionResult Create()
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            ViewBag.MaKH = new SelectList(data.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: DonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDH,TrangThaiGiaoHang,NgayDat,NgayGiao,MaKH,TrangThaiThanhToan")] Models.DonHang donHang)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.DonHang.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donHang);
        }

        public ActionResult EditTT(int id, FormCollection collection)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            Models.DonHang donHang = db.DonHang.Find(id);
            if (donHang != null)
            {
                if (collection["item.TrangThaiGiaoHang"].ToString() == "true")
                    donHang.TrangThaiGiaoHang = true;
                else
                    donHang.TrangThaiGiaoHang = false;
                Edit(donHang);
            }
            return RedirectToAction("Index");
        }
        public ActionResult EditTTT(int id, FormCollection collection)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            Models.DonHang donHang = db.DonHang.Find(id);
            if (donHang != null)
            {
                if (collection["item.TrangThaiThanhToan"].ToString() == "true")
                    donHang.TrangThaiThanhToan = true;
                else
                    donHang.TrangThaiThanhToan = false;
                Edit(donHang);
            }
            return RedirectToAction("Index");
        }
        // GET: DonHangs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(data.AspNetUsers, "Id", "Email");
            return View(donHang);
        }

        // POST: DonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDH,TrangThaiGiaoHang,NgayDat,NgayGiao,MaKH,TongTien,TrangThaiThanhToan")] Models.DonHang donHang)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donHang);
        }

        // GET: DonHangs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.LinQ.DonHang donHang = data.DonHangs.FirstOrDefault(d => d.MaDH == id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: DonHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            Models.DonHang donHang = db.DonHang.Find(id);
            db.DonHang.Remove(donHang);
            db.SaveChanges();
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

        public ActionResult DoanhThu(int? page)
        {
            if (!AuthAdmin())
                return RedirectToAction("Error401", "Admin");
            var all_donHang = data.DonHangs.ToList();
            int pageSize = 7;
            int pageNum = page ?? 1;
            return View(all_donHang.ToPagedList(pageNum, pageSize));
        }

        public FileResult Export()
        {
            DataTable dt = new DataTable("Grib");
            dt.Columns.AddRange(new DataColumn[] {
                new DataColumn("Ngày"),
                new DataColumn("Tổng số tiền")
            });
            var emps = data.DonHangs.GroupBy(p => p.NgayDat).Distinct().Select(g => new
            {
                Pla = g.Key,
                Total = g.Sum(t => t.TongTien)
            });
            foreach (var emp in emps)
            {
                dt.Rows.Add(emp.Pla, emp.Total);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Doanh-Thu.xlsx");
                }
            }
        }

        public bool AuthAdmin()
        {
            var user = dataUser.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            if (user == null)
                return false;
            var userExist = user.Roles.FirstOrDefault(r => r.UserId == user.Id);
            if (userExist == null)
                return false;
            if (userExist.RoleId != "1")
                return false;
            return true;
        }
    }
}