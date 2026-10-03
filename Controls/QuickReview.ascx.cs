using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
// Code: HOME-STATS-DEFER-20260918 - Cho phép trì hoãn truy vấn thống kê đến async postback sau 5 giây.
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Controls
{
    public partial class QuickReview : BaseUserControl
    {
        /// <summary>Cho phép control thực hiện truy vấn thống kê.</summary>
        public bool AllowLoadData { get; set; }

        /// <summary>Bắt buộc tải dữ liệu trong async postback dù IsPostBack bằng true.</summary>
        public bool ForceLoadData { get; set; }

        public QuickReview()
        {
            // Code: HOME-STATS-DEFER-20260918 - Mặc định giữ nguyên quy trình cũ nếu control được dùng ở trang khác.
            AllowLoadData = true;
            ForceLoadData = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Code: HOME-STATS-DEFER-20260918 - Request đầu bị trì hoãn; Timer bật ForceLoadData sau 5 giây.
            if (AllowLoadData && (!IsPostBack || ForceLoadData))
            {
                try
                {
                    LoadData();
                }
                catch (Exception ex)
                {
                    // Code: DASHBOARD-PARALLEL-LOAD-20260918 - Hiển thị lỗi riêng của truy vấn thống kê.
                    ltrHtml.Text = string.Format(
                        "<div class='alert alert-danger dashboard-load-error'><i class='ace-icon fa fa-exclamation-triangle'></i> Không tải được thống kê: {0}</div>",
                        HttpUtility.HtmlEncode(ex.Message));
                }
            }
        }

        private void LoadData()
        {
            int totalMauSapToiNgayTra = 0;

            List<TblMauXetNghiem> mauXNCol = new List<TblMauXetNghiem>();
            #region Xử lý dữ liệu
            string addition = "";
            DateTime dateFrom = new DateTime(DateTime.Now.Year - 1, 1, 1);
            DateTime ngayTraKetQUa = DateTimeHelper.AddLastTimeOfDay(DateTime.Now.AddDays(2));
            addition = string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.NgayTraKetQuaThucTe);
            List<TblMauXetNghiem> mauChuaTraKQ = XetNghiemManager.ReportMauXetNghiem(dateFrom, DateTime.Now, addition, string.Format("mxn.{0}", TblMauXetNghiem.Columns.NgayLayMau));
            #endregion
            if (UserHasRight(RightPageName.Home_Mau_New))
                ltrHtml.Text += string.Format(
                          "<a target='_blank' href='mau-xet-nghiem/new'><div class=\"infobox infobox-blue\">" +
                                   "<div class=\"infobox-icon\">" +
                                       "<i class=\"ace-icon fa fa-file\"></i>" +
                                   "</div>" +
                                   "<div class=\"infobox-data\">" +
                                       "<span class=\"infobox-data-number\">{0}    mẫu</span>" +
                                       "<div class=\"infobox-content\"> chưa chuyển labo</div>" +
                                   "</div>" +
                               "</div></a>", mauChuaTraKQ.FindAll(p => p.TrangThaiMau == "New").Count.ToString("N0"));
            if (UserHasRight(RightPageName.Home_Cho_Tra_Ket_Qua))
                ltrHtml.Text += string.Format(
                          "<a target='_blank' href='mau-xet-nghiem/cho-tra-kq'><div class=\"infobox infobox-green\">" +
                                   "<div class=\"infobox-icon\">" +
                                       "<i class=\"ace-icon fa fa-file\"></i>" +
                                   "</div>" +
                                   "<div class=\"infobox-data\">" +
                                       "<span class=\"infobox-data-number\"> {0}   mẫu </span>" +
                                       "<div class=\"infobox-content\">Còn 2 ngày phải trả KQ</div>" +
                                   "</div>" +
                               "</div></a>", mauChuaTraKQ.FindAll(p =>  p.NgayTraKetQua.HasValue && p.NgayTraKetQua.Value <= ngayTraKetQUa).Count.ToString("N0"));

            if (UserHasRight(RightPageName.Home_Cho_Tra_Ket_Qua_Hoa))
                ltrHtml.Text += string.Format(
                          "<a target='_blank' href='labo-hoa-tiep-nhan/cho-danh-gia-kq'><div class=\"infobox infobox-green\">" +
                                   "<div class=\"infobox-icon\">" +
                                       "<i class=\"ace-icon fa fa-file\"></i>" +
                                   "</div>" +
                                   "<div class=\"infobox-data\">" +
                                       "<span class=\"infobox-data-number\"> {0}   mẫu hóa lý </span>" +
                                       "<div class=\"infobox-content\">chưa đánh giá KQ</div>" +
                                   "</div>" +
                               "</div></a>", mauChuaTraKQ.FindAll(p => p.TrangThaiMau != "New" && p.LuongMauHoa.Value > 0 && (!p.NgayHoaLy.HasValue || (p.NgayHoaLy.HasValue && p.NgayHoaLy.Value == DateTime.MinValue))).Count.ToString("N0"));
            if (UserHasRight(RightPageName.Home_Cho_Tra_Ket_Qua_Sinh))
                ltrHtml.Text += string.Format(
                          "<a target='_blank' href='labo-vs-tiep-nhan/cho-danh-gia-kq'><div class=\"infobox infobox-green\">" +
                                   "<div class=\"infobox-icon\">" +
                                       "<i class=\"ace-icon fa fa-file\"></i>" +
                                   "</div>" +
                                   "<div class=\"infobox-data\">" +
                                       "<span class=\"infobox-data-number\"> {0}   mẫu vi sinh </span>" +
                                       "<div class=\"infobox-content\">chưa đánh giá KQ</div>" +
                                   "</div>" +
                               "</div></a>", mauChuaTraKQ.FindAll(p => p.TrangThaiMau != "New" && p.LuongMauViSinh > 0 && (!p.NgayViSinh.HasValue || (p.NgayViSinh.HasValue && p.NgayViSinh.Value == DateTime.MinValue))).Count.ToString("N0"));

            addition = string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.ThuTienNgayThu);
            List<TblMauXetNghiem> chuaThanhToanCol = XetNghiemManager.ReportMauXetNghiem(dateFrom, DateTime.Now, addition, string.Format("mxn.{0}", TblMauXetNghiem.Columns.NgayLayMau));
            if (UserHasRight(RightPageName.Home_Chua_Thanh_Toan))
                ltrHtml.Text += string.Format(
                          "<a target='_blank' href='ke-toan-tiep-nhan/thu-tien'><div class=\"infobox infobox-red\">" +
                                   "<div class=\"infobox-icon\">" +
                                       "<i class=\"ace-icon fa fa-file\"></i>" +
                                   "</div>" +
                                   "<div class=\"infobox-data\">" +
                                       "<span class=\"infobox-data-number\"> {0}   mẫu  </span>" +
                                       "<div class=\"infobox-content\">chưa thu tiền</div>" +
                                   "</div>" +
                               "</div></a>", chuaThanhToanCol.Count.ToString("N0"));

            addition = string.Format(" AND (mxn.{0} = 'ChoTraKetQua' or mxn.{0} = 'DaTraKetQua') AND mxn.{1} is null  ",
                                                            TblMauXetNghiem.Columns.TrangThaiMau, TblMauXetNghiem.Columns.NgayThanhLyThucTe);
            DateTime dateHoa = DateTime.Now.AddDays(-10);
            DateTime dateSinh = DateTime.Now.AddDays(-5);
            addition += string.Format(" AND ( (mxn.{0} - mxn.{1} > 0 AND mxn.{2} <= '{3}' AND mxn.{4} <= '{3}' ) OR (mxn.{1} > 0 AND mxn.{2} <= '{5}' AND mxn.{4} <= '{5}') ) ",
                                                            TblMauXetNghiem.Columns.LuongMau, TblMauXetNghiem.Columns.LuongMauHoa,
                                                            TblMauXetNghiem.Columns.NgayTraKetQua, dateSinh.ToString("yyyy-MM-dd 23:59:59"),
                                                            TblMauXetNghiem.Columns.NgayTraKetQuaThucTe, dateHoa.ToString("yyyy-MM-dd 23:59:59"));
            List<TblMauXetNghiem> chuaThanhLyCol = XetNghiemManager.ReportMauXetNghiem(dateFrom, DateTime.Now, addition, string.Format("mxn.{0}", TblMauXetNghiem.Columns.NgayLayMau));
            if (UserHasRight(RightPageName.Home_Thanh_Ly))
                ltrHtml.Text += string.Format(
                          "<a target='_blank' href='tra-ket-qua-xet-nghiem/chua-thanh-ly'><div class=\"infobox infobox-yellow\">" +
                                   "<div class=\"infobox-icon\">" +
                                       "<i class=\"ace-icon fa fa-file\"></i>" +
                                   "</div>" +
                                   "<div class=\"infobox-data\">" +
                                       "<span class=\"infobox-data-number\"> {0}   mẫu  </span>" +
                                       "<div class=\"infobox-content\">chưa thanh lý</div>" +
                                   "</div>" +
                               "</div></a>", chuaThanhLyCol.Count.ToString("N0"));

        }
    }
}
