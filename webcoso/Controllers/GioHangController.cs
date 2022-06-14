using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webcoso.Models;
using MoMo;
using Newtonsoft.Json.Linq;
using System.Configuration;
using common;
using webcoso.Message;
namespace webcoso.Controllers
{
    public class GioHangController : Controller
    {
        WebcosoContext data = new WebcosoContext();

        public List<GioHang> layGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sanPham = lstGioHang.Find(n => n.MaSP == id);
            if (sanPham == null)
            {
                sanPham = new GioHang(id);
                lstGioHang.Add(sanPham);
                Notification.set_flash("Đã thêm sản phẩm vào giỏ hàng!", "success");
                return Redirect(strURL);
            }
            else
            {
                sanPham.SoLuong++;
                Notification.set_flash("Đã thêm sản phẩm vào giỏ hàng!", "success");
                return Redirect(strURL);
            }
        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.SoLuong);
            }
            return tsl;
        }

        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.ThanhTien);
            }
            return tt;
        }

        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = layGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sanPham = lstGioHang.SingleOrDefault(n => n.MaSP == id);
            if (sanPham != null)
            {
                lstGioHang.RemoveAll(n => n.MaSP == id);
                Notification.set_flash("Đã xoá sản phẩm khỏi giỏ hàng!", "success");
                //Notification.set_flash("Deleted form cart!", "success");
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> lstGioHang = layGioHang();
            GioHang sanPham = lstGioHang.SingleOrDefault(n => n.MaSP == id);
            if (sanPham != null)
            {
                SanPham sp = data.SanPham.FirstOrDefault(s => s.MaSP == id);
                var soLuong = int.Parse(collection["txtSoLg"].ToString());
                if (sp.SoLuong < soLuong)
                    return RedirectToAction("GioHang");
                sanPham.SoLuong = soLuong;
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> lstGioHang = layGioHang();
            lstGioHang.Clear();
            Notification.set_flash("Đã xoá tất cả sản phẩm trong giỏ hàng!", "success");
            return RedirectToAction("GioHang");
        }


        //Dặt hàng
        [HttpGet]
        public ActionResult DatHang()
        {
            if (TongTien() == 0)
            {
                var tong = TongTien();
                return RedirectToAction("GioHang");
            }
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SanPhams");
            }

            List<GioHang> lstGioHang = layGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            return View(lstGioHang);
        }
        public ActionResult DatHang(FormCollection collection)
        {

            DonHang dh = new DonHang();
            Models.LinQ.AspNetUser kh = (Models.LinQ.AspNetUser)Session["TaiKhoan"];
            //SanPham s = new SanPham();
            List<GioHang> gh = layGioHang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.MaKH = kh.Id;
            dh.NgayDat = DateTime.Now;
            dh.NgayGiao = DateTime.Parse(ngaygiao);
            dh.TrangThaiGiaoHang = 0;
            dh.TrangThaiThanhToan = false;
            dh.TongTien = TongTien();

            data.DonHang.Add(dh);
            data.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDH = dh.MaDH;
                ctdh.MaSP = item.MaSP;
                ctdh.Soluong = item.SoLuong;
                ctdh.Gia = item.ThanhTien;
                SanPham sanPham = data.SanPham.Single(n => n.MaSP == item.MaSP);
                sanPham.SoLuong -= item.SoLuong;
                data.SaveChanges();
                data.ChiTietDonHang.Add(ctdh);
                data.SaveChanges();
            }
            Session["GioHang"] = null;

            String content = System.IO.File.ReadAllText(Server.MapPath("~/Content/sendmail.html"));
            content = content.Replace("{{CustomerName}}", kh.Name);
            content = content.Replace("{{Phone}}", kh.PhoneNumber);
            content = content.Replace("{{Email}}", kh.Email);
            content = content.Replace("{{Address}}", kh.Address);
            content = content.Replace("{{NgayDat}}", dh.NgayDat.ToString());
            content = content.Replace("{{NgayGiao}}", dh.NgayGiao.ToString());
            content = content.Replace("{{Total}}", dh.TongTien.ToString());
            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

            new common.MailHelper().sendMail(kh.Email, "Đơn hàng mới từ WebsiteLaptop của Tùng, An, Chuẩn", content);
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            Notification.set_flash("Đặt hàng thành công!", "success");
            return Redirect("/");
        }
        public ActionResult ThanhToan()
        {
            List<GioHang> gioHang = Session["GioHang"] as List<GioHang>;
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectKey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Đơn hàng của bạn";
            string returnUrl = "https://localhost:44373/GioHang/ReturnUrl";
            string notifyurl = "http://ba1adf48beba.ngrok.io/GioHang/NotifyUrl";

            string amount = gioHang.Sum(n => n.ThanhTien).ToString();
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            string rawHash =
                "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, serectKey);
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };
            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }

        public ActionResult ReturnUrl(FormCollection collection)
        {
            string param = Request.QueryString.ToString().Substring(0, Request.QueryString.ToString().IndexOf("signature") - 1);
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string signature = crypto.signSHA256(param, serectkey);
            if (signature != Request["signature"].ToString())
            {
                ViewBag.message = "Thông tin Request không hợp lệ";
                return View();
            }
            if (!Request.QueryString["errorCode"].Equals("0"))
            {
                ViewBag.message = "Thanh toán thất bại";
            }
            else
            {
                DonHang dh = new DonHang();
                Models.LinQ.AspNetUser kh = (Models.LinQ.AspNetUser)Session["TaiKhoan"];
                //SanPham s = new SanPham();
                List<GioHang> gh = layGioHang();
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
                dh.MaKH = kh.Id;
                dh.NgayDat = DateTime.Now;
                dh.NgayGiao = DateTime.Now;
                dh.TrangThaiGiaoHang = 0;
                dh.TrangThaiThanhToan = true;
                dh.TongTien = TongTien();
                if (dh.TongTien != 0)
                {
                    data.DonHang.Add(dh);
                    data.SaveChanges();
                }
                foreach (var item in gh)
                {
                    ChiTietDonHang ctdh = new ChiTietDonHang();
                    ctdh.MaDH = dh.MaDH;
                    ctdh.MaSP = item.MaSP;
                    ctdh.Soluong = item.SoLuong;
                    ctdh.Gia = item.ThanhTien;
                    SanPham sanPham = data.SanPham.Single(n => n.MaSP == item.MaSP);
                    sanPham.SoLuong -= item.SoLuong;
                    data.SaveChanges();
                    data.ChiTietDonHang.Add(ctdh);
                    data.SaveChanges();
                }
                Session["GioHang"] = null;
                if (dh.TongTien != 0)
                {
                    String content = System.IO.File.ReadAllText(Server.MapPath("~/Content/sendmail.html"));
                    content = content.Replace("{{CustomerName}}", kh.Name);
                    content = content.Replace("{{Phone}}", kh.PhoneNumber);
                    content = content.Replace("{{Email}}", kh.Email);
                    content = content.Replace("{{Address}}", kh.Address);
                    content = content.Replace("{{NgayDat}}", dh.NgayDat.ToString());
                    content = content.Replace("{{NgayGiao}}", dh.NgayGiao.ToString());
                    content = content.Replace("{{Total}}", dh.TongTien.ToString());
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new common.MailHelper().sendMail(kh.Email, "Đơn hàng mới từ WebsiteLaptop của Tùng, An, Chuẩn (MoMo)", content);
                }
                Notification.set_flash("Thanh toán thành công!", "success");
                return RedirectToAction("XacNhanDonHang", "GioHang");
            }
            return RedirectToAction("DatHang", "GioHang");
        }

        [HttpPost]
        public JsonResult NotifyUrl()
        {
            string param = "";
            param =
                "partner_code=" + Request["partner_code"] +
                "&access_key=" + Request["access_key"] +
                "&amount=" + Request["amount"] +
                "&order_id=" + Request["order_id"] +
                "&order_info=" + Request["order_info"] +
                "&order_type=" + Request["order_type"] +
                "&transaction_id=" + Request["transaction_id"] +
                "&message=" + Request["message"] +
                "&response_time=" + Request["response_time"] +
                "&status_code=" + Request["status_code"];
            param = Server.UrlDecode(param);
            MoMoSecurity crypto = new MoMoSecurity();
            string serectkey = ConfigurationManager.AppSettings["serectkey"].ToString();
            string signature = crypto.signSHA256(param, serectkey);
            if (signature != Request["signature"].ToString())
            {

            }
            string status_code = Request["status_code"].ToString();
            if ((status_code != "0"))
            {

            }
            else
            {

            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult thanhtoancanhan()
        {
            return View();
        }
    }
}