using webcoso.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace webcoso.Controllers
{
    public class ReportController : Controller
    {
        private WebcosoContext db = new WebcosoContext();

        // GET: Report
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPham.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            string ssrsUrl = ConfigurationManager.AppSettings["SSRSReportsUrl"].ToString();
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Remote;
            viewer.SizeToReportContent = true;
            viewer.AsyncRendering = true;
            viewer.ServerReport.ReportServerUrl = new Uri(ssrsUrl);
            viewer.ServerReport.ReportPath = "/TESTReport1";

            List<ReportParameter> parameters = new List<ReportParameter>();
            parameters.Add(new ReportParameter("Id", id.ToString()));
            viewer.ServerReport.SetParameters(parameters);

            ViewBag.ReportViewer = viewer;
            return View();
        }
    }
}