using C.Core.Common;
using C.Core.ExModel;
using C.Core.Model;
using C.Core.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace C.Core.Helper
{
    public class EmailHelper
    {
        public static void ThongBaoEmailDonHangMoi(string email, string noidung)
        {
            string noidungmail = "Bạn có một đơn hàng mới cần xử lý:" + noidung + "<br /> Vui lòng truy cập vào hệ thống để xử lý đơn hàng . <a href=''></a>";
            //EmailHelper.SendEmail(email, "Thông báo đơn hàng mới trên hệ thống", noidungmail, "Hệ thống quản lý đơn hàng");
        }

        public static void ThongBaoEmailDonHangMoiToiNguoiDatHang(string email, string noidung)
        {
            string noidungmail = "Thông báo đơn hàng:" + noidung + "<br /> Vui lòng kiểm tra lại thông tin đơn hàng.";
            //EmailHelper.SendEmail(email, "Thông báo đơn hàng mới trên hệ thống", noidungmail, "Hệ thống quản lý đơn hàng");
        }

        public static bool SendEmail(string toAddress, string subject, string body, string displayName)//, string smtpServer, int smtpPort, string userName, string pass, bool enableSsl
        {
            bool checkOk = true;
            string IsSendMail = System.Configuration.ConfigurationManager.AppSettings["IsSendMail"].ToString();
            if (Convert.ToBoolean(IsSendMail))
            {
                //Read from Web.config.xml
                string smtpServer = System.Configuration.ConfigurationManager.AppSettings["smtpServer"].ToString();
                int smtpPort = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["smtpPort"].ToString());
                string email = System.Configuration.ConfigurationManager.AppSettings["email"].ToString();
                string userName = System.Configuration.ConfigurationManager.AppSettings["urs"].ToString();
                string pass = System.Configuration.ConfigurationManager.AppSettings["pwd"].ToString();
                bool enableSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["enableSsl"].ToString());
                string fromAddress = userName;
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress(email, displayName);
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = body;

                //message.Attachments.Add(new Attachment());
                foreach (string s in toAddress.Split(';'))
                    message.To.Add(new MailAddress(s));

                message.BodyEncoding = Encoding.UTF8;

                SmtpClient smtp = new SmtpClient(smtpServer, smtpPort);
                smtp.Credentials = new System.Net.NetworkCredential(userName, pass);
                smtp.EnableSsl = enableSsl;
                try
                {
                    smtp.Send(message);
                }
                catch (Exception ex)
                {
                    checkOk = false;
                }
            }
            else
            {
                checkOk = false;
            }
            return checkOk;
        }

        #region Thông báo trong thời gian người dùng đặt hàng
        public static string NoiDungMailThongBaoQuanTri(string UnitName, int tongcongviec, string noidung)
        {
            string html = String.Empty;
            html += "<div class='mail-title'>";
            html += "<b>PHẦN MỀM QUẢN LÝ ĐƠN HÀNG<br/></b>";
            html += "</div>";
            html += "<div class='mail-notify' style='margin-top:10px'><b>Thông báo:</b><br/></div>";
            html += "<div class='mail-body'>";
            html += "<p class='greating-mail'>Xin chào " + UnitName + "</p>";
            html += "<p class='content-mail'>Hiện tại đơn vị của bạn có " + tongcongviec + " đơn hàng đang chờ đợi xử lý.<br/>";

            if (noidung != "")
                html += noidung;
            html += "<p class='end-mail'><b>Trân trọng</b></p>";
            html += "</div>";
            return html;
        }

        public static string NoiDungDonHang(shOrder order, List<CartItem> cart)
        {
            shSectionService _section = new shSectionService();
            shSizeService _size = new shSizeService();
            string noidung = string.Empty;

            noidung += "<p content-attach> <i>Thông tin người nhận hàng: " + "</i> ";
            noidung += order.FullName;
            if (string.IsNullOrWhiteSpace(order.Phone))
                noidung += " / " + order.Phone;
            if (string.IsNullOrWhiteSpace(order.Email))
                noidung += " / " + order.Email;
            if (string.IsNullOrWhiteSpace(order.Address))
                noidung += " / " + order.Address;
            noidung += " </p>";

            noidung += "<p content-attach><i>Giá trị đơn hàng: " + Format.FormatDecimalToString(order.Total.GetValueOrDefault(0)) + "</i></p>";
            noidung += "<p content-attach><i>Phí ship: " + Format.FormatDecimalToString(order.FeeShip.GetValueOrDefault(0)) + "</i></p>";
            noidung += "<p content-attach><i>Ngày đặt: " + order.NgayDat.GetValueOrDefault(DateTime.Now).ToString("dd/MM/yyyy") + "</i></p>";

            noidung += "<p content-attach><i>Thông tin đơn hàng: ";

            foreach (var item in cart)
            {
                noidung += "<p>" + item.Product.ProductName;
                noidung += _section.SectionName(item.SectionGuid);
                noidung += _size.SizeName(item.SizeGuid) + "</p>";
            }

            noidung += "</i></p>";

            return noidung;
        }

        public static string NoiDungMailThongBaoNguoiDatHang(string noidung)
        {
            string html = String.Empty;
            html += "<div class='mail-title'>";
            html += "<b>PHẦN MỀM QUẢN LÝ ĐƠN HÀNG<br/></b>";
            html += "</div>";
            html += "<div class='mail-notify' style='margin-top:10px'><b>Thông báo:</b><br/></div>";
            html += "<div class='mail-body'>";
            html += "<p class='greating-mail'>Xin chào </p>";
            html += "<p class='content-mail'>Bạn đã đặt đơn hàng thành công trên hệ thống.<br/>";

            html += "<p class='content-mail'>Thông tin chi tiết bao gồm: <br/>";

            if (noidung != "")
                html += noidung;

            html += "<p class='end-mail'><b>Trân trọng</b></p>";

            html += "</div>";
            return html;
        }

        public static string NoiDungMailThongBaoHuyDatHang(string noidung)
        {
            string html = String.Empty;
            html += "<div class='mail-title'>";
            html += "<b>PHẦN MỀM QUẢN LÝ ĐƠN HÀNG<br/></b>";
            html += "</div>";
            html += "<div class='mail-notify' style='margin-top:10px'><b>Thông báo:</b><br/></div>";
            html += "<div class='mail-body'>";
            html += "<p class='greating-mail'>Xin chào </p>";
            html += "<p class='content-mail'>Đơn hàng của bạn đã bị hủy . <br/>";

            html += "<p class='content-mail'>Thông tin chi tiết đơn hàng bị hủy: <br/>";

            if (noidung != "")
                html += noidung;

            noidung += "<p content-attach><i>Vui lòng truy cập hệ thống để kiểm tra. ";

            html += "<p class='end-mail'><b>Trân trọng</b></p>";

            html += "</div>";
            return html;
        }


        public static string NoiDungMailThongBaoXuLyDatHang(string noidung)
        {
            string html = String.Empty;
            html += "<div class='mail-title'>";
            html += "<b>PHẦN MỀM QUẢN LÝ ĐƠN HÀNG<br/></b>";
            html += "</div>";
            html += "<div class='mail-notify' style='margin-top:10px'><b>Thông báo:</b><br/></div>";
            html += "<div class='mail-body'>";
            html += "<p class='greating-mail'>Xin chào </p>";
            html += "<p class='content-mail'>Thông báo đơn hàng đang trong quá trình xử lý <br/>";

            html += "<p class='content-mail'>Thông tin chi tiết đơn hàng <br/>";

            if (noidung != "")
                html += noidung;

            noidung += "<p content-attach><i>Vui lòng truy cập hệ thống để theo dõi. ";

            html += "<p class='end-mail'><b>Trân trọng</b></p>";

            html += "</div>";
            return html;
        }
        #endregion

    }
}
