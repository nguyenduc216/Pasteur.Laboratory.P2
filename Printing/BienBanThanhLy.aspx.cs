using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using SweetSoft.ExtraControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Printing
{
    public partial class BienBanThanhLy : System.Web.UI.Page
    {
        public int? CurrentID
        {
            get
            {
                int? vl = null;
                int u = 0;
                if (int.TryParse(CommonHelper.QueryString("ID"), out u) && u > 0) vl = u;
                return vl;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentID.HasValue)
                {
                    TblThanhLy thanhLy = CategoryManager.GetThanhLyByID(CurrentID.Value);
                    if (thanhLy != null)
                    {
                        ltrTai.Text = "Labo vi sinh môi trường, ";
                        if (thanhLy.LoaiThanhLy == "LaboHoaLy") ltrTai.Text = "Labo hóa môi trường, ";
                        ltrChungToGom.Text = thanhLy.ChungToiGom.Replace("\n","<br/>");
                        ltrHomNay.Text = string.Format("Hôm nay vào lúc {0} giờ, ngày {1} tháng {2} năm {3}",
                                        thanhLy.NgayThucHien.Value.ToString("HH"), thanhLy.NgayThucHien.Value.ToString("dd"), thanhLy.NgayThucHien.Value.ToString("MM"),
                                        thanhLy.NgayThucHien.Value.ToString("yyyy"));
                        if (thanhLy.ThoiGianKetThuc.HasValue && thanhLy.ThoiGianKetThuc.Value != DateTime.MinValue)
                            ltrKetThuc.Text = string.Format("Công việc kết thúc lúc {0} giờ, ngày {1} tháng {2} năm {3}",
                                thanhLy.ThoiGianKetThuc.Value.ToString("HH"), thanhLy.ThoiGianKetThuc.Value.ToString("dd"), thanhLy.ThoiGianKetThuc.Value.ToString("MM"),
                                        thanhLy.ThoiGianKetThuc.Value.ToString("yyyy"));
                        List<TblThanhLyChiTiet> chiTietcol = CategoryManager.GetThanhLyChiTietByThanhLyID(CurrentID.Value);
                        repData.DataSource = chiTietcol;
                        repData.DataBind();
                    }
                }
            }
        }
    }
}