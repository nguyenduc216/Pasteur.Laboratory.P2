using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
// Code: DASHBOARD-PARALLEL-LOAD-20260918 - Dashboard tải thống kê và log qua hai endpoint độc lập.
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2
{
    public partial class Default : BasePage
    {
        public override bool IsUserRole { get { return false; } }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Code: DASHBOARD-PARALLEL-LOAD-20260918 - Request trang chính không chạy truy vấn dashboard.
        }

        #region Web Method
        [WebMethod(EnableSession = true)]
        public static object SearchChiTieuTextByKeyWord(string key)
        {
            List<TblChiTieuXetNghiem> chiTieu = XetNghiemManager.SearchChiTieuTextByKeyWord(key);
            return new { List = chiTieu };
        }
        [WebMethod(EnableSession = true)]
        public static object SearchChiTieuByKeyWord(string key)
        {
            List<TblChiTieuXetNghiem> chiTieu = XetNghiemManager.SearchChiTieuByKeyWord(key, false);
            return new { List = chiTieu };
        }
        [WebMethod(EnableSession = true)]
        public static object SearchChiTieuTTQDTextByKeyWord(string key)
        {
            List<TblChiTieuXetNghiem> chiTieu = XetNghiemManager.SearchChiTieuTextByKeyWord(key, TblChiTieuXetNghiem.Columns.TenTrongTTQD);
            return new { List = chiTieu[0].TenTaiCacKhoa };
        }
        [WebMethod(EnableSession = true)]
        public static object SearchChiTieuSTTextByKeyWord(string key)
        {
            List<TblChiTieuXetNghiem> chiTieu = XetNghiemManager.SearchChiTieuTextByKeyWord(key, TblChiTieuXetNghiem.Columns.Stt);
            return new { List = chiTieu };
        }

        [WebMethod(EnableSession = true)]
        public static object SearchChiTieuPhuongPhapByKeyWord(string key, string type)
        {
            if (type == "ChiTieu")
            {
                List<TblChiTieuXetNghiem> chiTieu = XetNghiemManager.SearchChiTieuPhuongPhapByKeyWord(key);
                return new { List = chiTieu };
            }
            List<TblCatNhomXetNghiem> nhomCol = CategoryManager.SearchNhomXNByText(key);
            return new { List = nhomCol };
        }
        [WebMethod(EnableSession = true)]
        public static object SearchMauXetNghiemByKeyWord(string key, string column)
        {
            List<TblMauXetNghiem> mauXNCol = XetNghiemManager.SearchMauXetNghiemByKeyWord(key, column);

            return new { List = mauXNCol };
        }

        [WebMethod(EnableSession = true)]
        public static object SearchMauXetNghiemByKeyWordDistinct(string key, string column)
        {
            List<TblMauXetNghiem> mauXNCol = XetNghiemManager.SearchMauXetNghiemByKeyWordDistinct(key, column);

            return new { List = mauXNCol };
        }
        [WebMethod(EnableSession = true)]
        public static object SearchKhachHangByKeyWordDistinct(string key, string column)
        {
            List<TblKhachHang> khachHangCol = XetNghiemManager.SearchKhachHangByKeyWordDistinct(key, column);

            return new { List = khachHangCol };
        }

        [WebMethod(EnableSession = true)]
        public static object GetMauXetNghiemThanhLy(string key, string Type)
        {
            List<TblMauXetNghiem> khachHangCol = XetNghiemManager.GetMauXetNghiemThanhLy(key, Type, DateTimeHelper.AddFirstTimeOfDay(DateTime.Now));

            return new { List = khachHangCol };
        }
    }
    #endregion
}
