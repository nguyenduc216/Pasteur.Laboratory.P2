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

namespace Pasteur.Laboratory.P2.Printing
{
    public partial class PhieuChuyenMau : System.Web.UI.Page
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
        protected string GetTen(object ten, object InNghieng)
        {
            string rs = "";
            if (ten != null)
            {
                if (InNghieng != null && InNghieng.ToString().Trim().ToUpper() == "TRUE")
                    rs = string.Format("<span style='font-style:italic;'>{0}</span>", ten.ToString());
                else
                    rs = ten.ToString();
            }
            return rs.Replace("<br/>", " ").Replace("<br>", " ");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentID.HasValue)
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null && mauXN.NgayChuyenLabo.HasValue && mauXN.NgayChuyenLabo.Value != DateTime.MinValue)
                    {
                        ltrLoaiMau.Text = ltrVSLoaiMau.Text = mauXN.TenLoaiMau;
                        ltrNgayNhanMau.Text = ltrVSNgayNhanMau.Text = mauXN.NgayNhanMauToString;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                        List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");
                        int total = chiTieuCol.Count;
                        int hoaLy = chiTieuHL.Count;
                        int viSinh = total - hoaLy;

                        string maMau = mauXN.MaMau + "/";
                        if (mauXN.LuongMauHoa.Value > 0) maMau += "HH-";
                        else maMau += "00-";

                        ltrTongSoChiTieu.Text = ltrVSTongSoChiTieu.Text = total.ToString("N0");
                        ltrTongSoHoaLy.Text = ltrVSTongSoHoaLy.Text = hoaLy.ToString("N0");
                        ltrTongSoViSinh.Text = ltrVSTongSoViSinh.Text = viSinh.ToString("N0");

                        if ((viSinh) > 0) maMau += "VS";
                        else maMau += "00";
                        ltrMaSoMau.Text = ltrVSMaSoMau.Text = maMau;


                        ltrLuongMau.Text = ltrVSLuongMau.Text = mauXN.LuongMau.Value.ToStringWithLanguage() + " " + mauXN.TenDonVi;
                        ltrLuongHoaLy.Text = ltrVSLuongHoaLy.Text = mauXN.LuongMauHoa.Value.ToStringWithLanguage() + " " + mauXN.TenDonVi;
                        ltrLuongViSinh.Text = ltrVSLuongViSinh.Text = (mauXN.LuongMau.Value - mauXN.LuongMauHoa.Value).ToStringWithLanguage() + " " + mauXN.TenDonVi;

                        ltrDieuKienBaoQuan.Text = ltrVSDieuKienBaoQuan.Text = mauXN.DieuKienBaoQuan;
                        ltrNgayChuyenMau.Text = ltrVSNgayChuyenMau.Text = mauXN.NgayChuyenLaboToString;
                        ltrQuyChuanDanhGia.Text = ltrVSQuyChuanDanhGia.Text = mauXN.TenQuyChuan;
                        ltrNgayTraKetQua.Text = ltrVSNgayTraKetQua.Text = mauXN.NgayTraKetQuaToString;
                        ltrNguoiChuyenMau.Text = ltrVSNguoiChuyenMau.Text = mauXN.TenNguoiNhanMau;
                        ltrBan.Text = LltrBanVS.Text = hoaLy > 0 && viSinh > 0 ? "3" : hoaLy > 0 || viSinh > 0 ? "2" : "1";
                        divVS.Visible = viSinh > 0;
                        divHL.Visible = hoaLy > 0;

                        //if (chiTieuCol.Count % 2 != 0)
                        //{
                        //    TblMauXetNghiemChiTieu last = new TblMauXetNghiemChiTieu();
                        //    last.TenTaiCacKhoa = last.TenPhuongPhap = "";
                        //    chiTieuCol.Add(last);
                        //}

                        if (chiTieuHL.Count % 2 != 0)
                        {
                            TblMauXetNghiemChiTieu last = new TblMauXetNghiemChiTieu();
                            last.TenTaiCacKhoa = last.TenPhuongPhap = "";
                            chiTieuHL.Add(last);
                        }



                        if (chiTieuVS.Count % 2 != 0)
                        {
                            TblMauXetNghiemChiTieu last = new TblMauXetNghiemChiTieu();
                            last.TenTaiCacKhoa = last.TenPhuongPhap = "";
                            chiTieuVS.Add(last);
                        }

                        List<TblMauXetNghiemChiTieu> groupByTitleCol = chiTieuCol.GroupBy(c => new
                        {
                            c.MappingTitle,
                            c.MappingDonGia,
                            c.TenNhomXetNghiem,
                            c.MappingDonGiaNhomToString,
                            c.IDNhomXetNghiem
                        })
                         .Select(gcs => new TblMauXetNghiemChiTieu()
                         {
                             MappingTitle = gcs.Key.MappingTitle,
                             MappingDonGia = gcs.Key.MappingDonGia,
                             TenNhomXetNghiem = gcs.Key.TenNhomXetNghiem,
                             MappingDonGiaNhomToString = gcs.Key.MappingDonGiaNhomToString,
                             IDNhomXetNghiem = gcs.Key.IDNhomXetNghiem,
                             Children = gcs.ToList()
                         }).ToList<TblMauXetNghiemChiTieu>();

                        List<TblMauXetNghiemChiTieu> bindingData = new List<TblMauXetNghiemChiTieu>();
                        foreach (TblMauXetNghiemChiTieu chiTieu in groupByTitleCol)
                        {
                            if (chiTieu.Children == null || (
                               chiTieu.Children != null && chiTieu.Children.Count == 0))
                                bindingData.Add(chiTieu);
                            else if (chiTieu.Children != null && chiTieu.Children.Count > 0)
                            {
                                foreach (TblMauXetNghiemChiTieu chiTieuSub in chiTieu.Children)
                                    bindingData.Add(chiTieuSub);
                            }
                        }

                        repChiTieuHoaLy.DataSource = bindingData;
                        repChiTieuHoaLy.DataBind();

                        repChiTieuViSinh.DataSource = bindingData;
                        repChiTieuViSinh.DataBind();

                        decimal tongTien = groupByTitleCol.Sum(p => p.MappingDonGia);
                        ltrTongCong.Text = ltrTongCongViSinh.Text = tongTien.ToStringWithLanguage();
                    }
                }
            }
        }

        protected void repChiTieuHoaLy_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item ||
              e.Item.ItemType == ListItemType.AlternatingItem) &&
              e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;

                Literal ltrLastColumnThanhTien = e.Item.FindControl("ltrLastColumnThanhTien") as Literal;

                if (string.IsNullOrEmpty(chiTieu.TenNhomXetNghiem))
                {

                    ltrLastColumnThanhTien.Text = string.Format("<td style='text-align:right;'>{0}</td>", chiTieu.DonGia.HasValue ? chiTieu.DonGia.Value.ToStringWithLanguage():"0");
                }
                else if (chiTieu.RowSpan.HasValue && chiTieu.RowSpan.Value > 0)
                {

                    ltrLastColumnThanhTien.Text = string.Format("<td {0} style='text-align:right;vertical-align:middle;'>{1}</td>",
                                                              string.Format("rowspan='{0}'", chiTieu.RowSpan.Value),
                                                              chiTieu.MappingDonGia.ToStringWithLanguage());
                }
            }
        }
    }
}