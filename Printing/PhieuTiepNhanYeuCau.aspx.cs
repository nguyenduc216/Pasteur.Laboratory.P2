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
    public partial class PhieuTiepNhanYeuCau : System.Web.UI.Page
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
                    if (mauXN != null)
                    {
                        ltrMaMau.Text = mauXN.MaMau;
                        ltrTenKhachHang.Text = mauXN.TenKhachHang;
                        ltrDiaChiKhachHang.Text = mauXN.DiaChiKhachHang;
                        ltrDienThoaiKhachHang.Text = mauXN.DienThoaiKhachHang;
                        if (!string.IsNullOrEmpty(mauXN.EmailKhachHang))
                        {
                            if (!string.IsNullOrEmpty(ltrDienThoaiKhachHang.Text)) ltrDienThoaiKhachHang.Text += @" \ ";
                            ltrDienThoaiKhachHang.Text += mauXN.EmailKhachHang;
                        }

                        ltrTenNguoiGuiMau.Text = mauXN.TenNguoiGuiMau;
                        ltrDiaChiNguoiGuiMau.Text = mauXN.DiaChiNguoiGuiMau;
                        ltrDienThoaiNguoiGuiMau.Text = mauXN.DienThoaiNguoiGuiMau;
                        if (!string.IsNullOrEmpty(mauXN.EmailNguoiGuiMau))
                        {
                            if (!string.IsNullOrEmpty(ltrDienThoaiNguoiGuiMau.Text)) ltrDienThoaiNguoiGuiMau.Text += @" \ ";
                            ltrDienThoaiNguoiGuiMau.Text += mauXN.EmailNguoiGuiMau;
                        }

                        ltrTenNguoiLayMau.Text = mauXN.TenNguoiLayMau;
                        //ltrDiaChiNguoiLayMau.Text = mauXN.DiaChiNguoiLayMau;
                        ltrDienThoaiNguoiLayMau.Text = mauXN.DienThoaiNguoiLayMau;
                        if (!string.IsNullOrEmpty(mauXN.EmailNguoiLayMau))
                        {
                            if (!string.IsNullOrEmpty(ltrDienThoaiNguoiLayMau.Text)) ltrDienThoaiNguoiLayMau.Text += @" \ ";
                            ltrDienThoaiNguoiLayMau.Text += mauXN.EmailNguoiLayMau;
                        }

                        ltrTenNguoiNhanMau.Text = mauXN.TenNguoiNhanMau;
                        //ltrDiaChiNguoiNhanMau.Text = mauXN.DiaChiNguoiNhanMau;
                        ltrDienThoaiNguoiNhanMau.Text = mauXN.DienThoaiNguoiNhanMau;
                        if (!string.IsNullOrEmpty(mauXN.EmailNguoiNhanMau))
                        {
                            if (!string.IsNullOrEmpty(ltrDienThoaiNguoiNhanMau.Text)) ltrDienThoaiNguoiNhanMau.Text += @" \ ";
                            ltrDienThoaiNguoiNhanMau.Text += mauXN.EmailNguoiNhanMau;
                        }
                        ltrNoiLayMau.Text = mauXN.NoiLayMau.Replace(@"\n", "<br/>");
                        ltrDieuKienBaoQuan.Text = mauXN.DieuKienBaoQuan;

                        ltrLoaiMau.Text = mauXN.TenLoaiMau;
                        ltrNgayGioNhanMau.Text = mauXN.NgayNhanMauToString;

                        ltrNgayLayMau.Text = mauXN.NgayLayMauToString;
                        ltrMoTaMau.Text = mauXN.MoTaMau;

                        ltrNgayTraKetQua.Text = mauXN.NgayTraKetQuaToString;
                        ltrYeuCauKhac.Text = mauXN.YeuCauKhac;
                        ltrGhiChu.Text = mauXN.GhiChuMau;
                        ltrQuyChuan.Text = mauXN.TenQuyChuan;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                        int total = chiTieuCol.Count;
                        int hoaLy = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy").Count;
                        int viSinh = total - hoaLy;

                        //if (chiTieuCol.Count % 2 != 0)
                        //{
                        //    TblMauXetNghiemChiTieu last = new TblMauXetNghiemChiTieu();
                        //    last.TenTaiCacKhoa = last.TenPhuongPhap = " ";
                        //    chiTieuCol.Add(last);
                        //}
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
                        repChiTieu.DataSource = bindingData;
                        repChiTieu.DataBind();
                        decimal tongTien = groupByTitleCol.Sum(p => p.MappingDonGia);
                        ltrTongCong.Text = tongTien.ToStringWithLanguage();

                        if (mauXN.MaMau.Trim().ToUpper().StartsWith("V") && false)
                        {
                            divVimcert.Visible = true;
                            repVimcert.DataSource = chiTieuCol;
                            repVimcert.DataBind();
                        }


                        ltrTongSoChiTieu.Text = total.ToString("N0", new CultureInfo("vi-VN"));
                        ltrTongSoHoaLy.Text = hoaLy.ToString("N0", new CultureInfo("vi-VN"));
                        ltrTongSoViSinh.Text = viSinh.ToString("N0", new CultureInfo("vi-VN"));
                        //ltrTongCong.Text = chiTieuCol.Sum(p => p.DonGia.HasValue ? p.DonGia.Value : 0).ToStringWithLanguage();
                        ltrKhanhHoa.Text = string.Format("Khánh Hòa, ngày {0} tháng {1} năm {2}",
                            DateTime.Now.ToString("dd"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("yyyy"));
                        if (mauXN.CreatedDate.HasValue && mauXN.CreatedDate.Value != DateTime.MinValue)
                            ltrKhanhHoa.Text = string.Format("Khánh Hòa, ngày {0} tháng {1} năm {2}",
                                mauXN.CreatedDate.Value.ToString("dd"), mauXN.CreatedDate.Value.ToString("MM"), mauXN.CreatedDate.Value.ToString("yyyy"));

                        ltrNguoiNhanMau.Text = mauXN.TenNguoiNhanMau;

                        mauXN.NgayInPhieuTiepNhan = DateTime.Now;
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }
                }
            }
        }

        protected void repChiTieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item ||
              e.Item.ItemType == ListItemType.AlternatingItem) &&
              e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;

                Literal ltrLastColumnThanhTien = e.Item.FindControl("ltrLastColumnThanhTien") as Literal;

                if (string.IsNullOrEmpty(chiTieu.TenNhomXetNghiem))
                {

                    ltrLastColumnThanhTien.Text = string.Format("<td style='text-align:right;'>{0}</td>", chiTieu.DonGia.HasValue ? chiTieu.DonGia.Value.ToStringWithLanguage() : "0");
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