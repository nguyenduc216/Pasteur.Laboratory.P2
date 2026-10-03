using Pasteur.Core;
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Printing
{
    public partial class PhieuDeNghiThuTien : BasePage
    {
        public override bool IsUserRole
        {
            get { return false; }
        }
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
            return rs;
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
                        ltrDiaChi.Text = mauXN.ThuTienDiaChi;
                        ltrDienThoai.Text = mauXN.ThuTienDienThoai;
                        ltrDonVi.Text = mauXN.ThuTienDonVi;
                        ltrHoTenNguoiDongTien.Text = mauXN.ThuTienHoVaTen;
                        ltrMaSoThue.Text = mauXN.ThuTienMaSoThue;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value, "", true);
                        List<TblMauXetNghiemChiTieu> groupByTitleCol = chiTieuCol
                        .GroupBy(c => new
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
                        repData.DataSource = bindingData;
                        repData.DataBind();

                        decimal tongTien = groupByTitleCol.Sum(p => p.MappingDonGia);
                        ltrTongTien.Text = tongTien.ToStringWithLanguage();
                        ltrTongTienBangChu.Text = "( " + ReadNumber.ByWords(tongTien) + " đồng )";

                        ltrNguoiLapPhieu.Text = mauXN.ThuTienNguoiThu;
                        ltrNhaTrang.Text = string.Format("Khánh Hòa, ngày {0} tháng {1} năm {2}",
                                                          mauXN.ThuTienNgayThu.Value.ToString("dd"),
                                                          mauXN.ThuTienNgayThu.Value.ToString("MM"),
                                                          mauXN.ThuTienNgayThu.Value.ToString("yyyy"));

                        List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");
                        string hl = "";
                        if (chiTieuHL.Count > 0) hl = "hóa lý";
                        if (chiTieuVS.Count > 0) hl = "vi sinh";
                        if (chiTieuVS.Count > 0 && chiTieuHL.Count > 0) hl = "hóa lý - vi sinh";

                        ltrNoiDungXetNghiem.Text = string.Format("Xét nghiệm {0} {1}",
                                                            hl, mauXN.TenLoaiMau);
                        ltrSo.Text = mauXN.MaMau;
                    }
                }
            }
        }

        protected void repData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item ||
               e.Item.ItemType == ListItemType.AlternatingItem) &&
               e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
                //Literal ltrLastColumnDonGia = e.Item.FindControl("ltrLastColumnDonGia") as Literal;
                Literal ltrLastColumnThanhTien = e.Item.FindControl("ltrLastColumnThanhTien") as Literal;

                if (string.IsNullOrEmpty(chiTieu.TenNhomXetNghiem))
                {
                    //ltrLastColumnDonGia.Text = string.Format("<td style='text-align:center;'>{0}</td>", chiTieu.DonGia.Value.ToStringWithLanguage());
                    ltrLastColumnThanhTien.Text = string.Format("<td style='text-align:right;'>{0}</td>", chiTieu.DonGia.Value.ToStringWithLanguage());
                }
                else if (chiTieu.RowSpan.HasValue && chiTieu.RowSpan.Value > 0)
                {
                    //ltrLastColumnDonGia.Text = string.Format("<td {0} style='text-align:center;'>{1}</td>",
                    //                                           string.Format("rowspan='{0}'", chiTieu.RowSpan.Value),
                    //                                           chiTieu.MappingDonGia.ToStringWithLanguage());
                    ltrLastColumnThanhTien.Text = string.Format("<td {0} style='text-align:right;'>{1}</td>",
                                                              string.Format("rowspan='{0}'", chiTieu.RowSpan.Value),
                                                              chiTieu.MappingDonGia.ToStringWithLanguage());
                }
            }
        }
    }
}