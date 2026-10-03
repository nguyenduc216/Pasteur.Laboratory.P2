using Pasteur.Core.Helper;
using Pasteur.Core;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Pasteur.Laboratory.P2.Printing
{
    public partial class BienBanGhiChepThuNghiem : System.Web.UI.Page
    {
        private int? CurrentID
        {
            get
            {
                int u = 0;
                if (int.TryParse(CommonHelper.QueryString("ID"), out u) && u > 0)
                    return u;
                return null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentID.HasValue)
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null)
                    {
                        ltrMaMau.Text = mauXN.MaMau;
                        ltrLoaiMau.Text = mauXN.TenLoaiMau;
                        ltrNgayNhan.Text = mauXN.NgayNhanMauToString;
                        ltrNgayTra.Text = mauXN.NgayTraKetQuaDuKienToString;
                        ltrQuyChan.Text = mauXN.TenQuyChuan;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);

                        List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");

                        repData.DataSource = chiTieuVS;
                        repData.DataBind();
                    }
                }
            }
        }

        protected void repData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (e.Item.DataItem != null)
                {
                    TblMauXetNghiemChiTieu mau = e.Item.DataItem as TblMauXetNghiemChiTieu;
                    List<TblChiTieuBienBanGhiChep> bienBam = ChiTieuManager.GetGhiChepByChiTieu(mau.IDChiTieu);
                    if (bienBam != null)
                    {
                        List<TblChiTieuBienBanGhiChep> chungDuong = bienBam.FindAll(p => p.Loai == "Chứng dương");
                        string html = "";
                        foreach (TblChiTieuBienBanGhiChep item in chungDuong)
                            html += string.Format("<br/>☐ {0}", item.Ten);

                        Literal ltrChungDuong1 = e.Item.FindControl("ltrChungDuong1") as Literal;
                        Literal ltrChungDuong2 = e.Item.FindControl("ltrChungDuong2") as Literal;
                        Literal ltrChungDuong3 = e.Item.FindControl("ltrChungDuong3") as Literal;
                        ltrChungDuong1.Text = ltrChungDuong2.Text = ltrChungDuong3.Text = html;
                        html = "";
                        List<TblChiTieuBienBanGhiChep> MT = bienBam.FindAll(p => p.Loai == "MT");
                        foreach (TblChiTieuBienBanGhiChep item in MT)
                            html += string.Format("<br/>☐ {0}", item.Ten);
                        Literal ltrMT = e.Item.FindControl("ltrMT") as Literal; ltrMT.Text = html;
                    }
                }
            }
        }
    }
}