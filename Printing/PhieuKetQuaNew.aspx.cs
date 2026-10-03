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
    public partial class PhieuKetQuaNew : System.Web.UI.Page
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
                //if (InNghieng != null && InNghieng.ToString().Trim().ToUpper() == "TRUE")
                //    rs = string.Format("<span style='font-style:italic;'>{0}</span>", ten.ToString());
                //else
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

                        ltrNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        ltrLoaiMau.Text = mauXN.TenLoaiMau;
                        ltrDieuKienBaoQuan.Text = mauXN.DieuKienBaoQuan;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                        columnCount = chiTieuCol.Max(p => p.MaxKetQua.Value);

                        List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");

                        repData.DataSource = chiTieuCol;
                        repData.DataBind();

                        List<TblCatChungChi> chungChiCol = XetNghiemManager.GetChungNhanImageByMauID(mauXN.Id);
                        repImage.DataSource = chungChiCol;
                        repImage.DataBind();

                        int total = chiTieuCol.Count;
                        int hoaLy = chiTieuHL.Count;
                        int viSinh = total - hoaLy;

                        string maMau = mauXN.MaMau + "/";
                        if (mauXN.LuongMauHoa.Value > 0) maMau += "HH-";
                        else maMau += "00-";

                        if ((viSinh) > 0) maMau += "VS";
                        else maMau += "00";
                        ltrMaMau.Text = ltrMaSoMau.Text = maMau;
                        ltrSo.Text = mauXN.MaSoTraKQ;
                        ltrKhachHang.Text = mauXN.TenKhachHang;
                        ltrDiaChi.Text = string.Format("<span style='display:inline-block;width:395px;'>: {0}</span>", mauXN.DiaChiKhachHang);
                        ltrNoiLayMau.Text = mauXN.NoiLayMau;
                        ltrMieuTaMau.Text = mauXN.MoTaMau;
                        ltrNguoiLayMau.Text = mauXN.TenNguoiLayMau;
                        ltrNguoiGui.Text = mauXN.TenNguoiGuiMau;
                        ltrNgayLayMau.Text = mauXN.NgayLayMauToString;
                        ltrNgayNhanMau.Text = mauXN.NgayNhanMauToString;
                        DateTime dt = DateTime.MinValue;
                        if (mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value > dt) dt = mauXN.NgayHoaLy.Value;
                        if (mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value > dt) dt = mauXN.NgayViSinh.Value;
                        ltrThoiGianKiemNghiem.Text = string.Format("Từ {0} đến {1}", mauXN.NgayKiemNghiemBatDau, mauXN.NgayKiemNghiemKetThuc);
                        ltrThoiGianLuuTru.Text = string.Format("{0} ngày kể từ ngày trả kết quả", chiTieuHL.Count > 0 ? 10 : 5);
                        ltrKetQuaThuNghiem.Text = string.Format("Xem các trang sau");
                        if (!string.IsNullOrEmpty(mauXN.KetQuaDanhGiaMau))
                            ltrNhanXet.Text = "<b>Nhận xét: </b>" + mauXN.NhanXetCuoiCung.Replace("[KQKT]", string.Format("<b>{0}</b>", mauXN.KetQuaDanhGiaMau == "1" ? "đạt" : "không đạt")); ;
                        if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                        {
                            ltrColumnHeaderGroup.Text = ltrColumnHeaderGroup1.Text = string.Format("<td colspan='{0}' {1} style='text-align:center;font-weight:bold;'>Giới hạn tối đa cho phép <sup>(*)</sup> / Maximum limit (*)</td>", columnCount, columnCount == 1 ? " rowspan='2' " : "");
                            if (columnCount > 0)
                            {
                                string[] arr = { "A", "B", "C", "D" };
                                if (mauXN.MauTheoABCD.HasValue && !mauXN.MauTheoABCD.Value) arr = new string[] { "A1", "A2", "B1", "B2" };
                                ltrColumnHeader.Text = ltrColumnHeader1.Text = "";
                                if (columnCount > 1)
                                {
                                    for (int i = 0; i < columnCount; i++)
                                    {
                                        ltrColumnHeader.Text += string.Format("<th style='text-align:center;width:50px;'>{0}</th>", arr[i]);
                                        ltrColumnHeader1.Text += string.Format("<th style='text-align:center;width:50px;'>{0}</th>", arr[i]);
                                    }
                                }
                            }


                            ltrFooter.Text += string.Format("(*): Giới hạn tối đa cho phép theo {0} ({1});<br />", mauXN.MieuTaQuyChuan, mauXN.TenQuyChuan);
                        }
                        TblMauXetNghiemChiTieu iso = chiTieuCol.FirstOrDefault(p => p.TenXuatKetQua.Contains("(**)"));
                        if (iso != null) ltrFooter.Text += "(a): Phép thử được công nhận phù hợp với ISO/IEC 17025:2017<br/>";

                        TblMauXetNghiemChiTieu kqd = chiTieuCol.FirstOrDefault(p => p.KetQuaHienThiToString.Contains("KQĐ"));
                        if (iso != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value) ltrFooter.Text += "KQĐ: Không Quy Định<br/>";

                        if (mauXN.MaMau.Trim().ToUpper().StartsWith("V"))
                        {
                            divVimcert.Visible = true;
                            repDataVimcert.DataSource = chiTieuCol;
                            repDataVimcert.DataBind();
                        }
                    }
                }
            }
        }
        private int columnCount = 0;

        protected void repImage_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is TblCatChungChi &&
                (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TblCatChungChi chungChi = e.Item.DataItem as TblCatChungChi;
                Literal ltrImage = e.Item.FindControl("ltrImage") as Literal;
                ltrImage.Text = string.Format("<img src='{0}' style='margin-right:5px;width:100px;height:70px;display:inline-table;'/>", chungChi.ImagePath);
            }
        }

        protected void repData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu &&
               (e.Item.ItemType == ListItemType.Item ||
               e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                if (mauXN != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                {
                    Literal ltrGioiHanChoPhep = e.Item.FindControl("ltrGioiHanChoPhep") as Literal;
                    TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
                    if (chiTieu != null && chiTieu.Idctqc.HasValue && chiTieu.Idctqc.Value > 0)
                    {

                        TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByID(chiTieu.Idctqc.Value);
                        if (ctqc != null)
                        {
                            string rs = "";
                            int currentColumn = 0;
                            if (ctqc.LoaiHinhGioiHan == "NhapSo")
                            {
                                if (ctqc.GioiHan1.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan1);
                                }
                                if (ctqc.GioiHan2.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan2);
                                }
                                if (ctqc.GioiHan3.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan3);
                                }
                                if (ctqc.GioiHan4.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan4);
                                }
                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc1))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc1);
                                }
                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc2))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc2);
                                }
                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc3))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc3);
                                }
                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc4))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc4);
                                }
                            }

                            while (currentColumn < columnCount)
                            {
                                rs += "<td></td>";
                                currentColumn++;
                            }

                            if (ltrGioiHanChoPhep != null) ltrGioiHanChoPhep.Text = rs;

                        }
                        else
                        {
                            ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='text-align:center;'>KQĐ</td>", columnCount);
                        }
                    }
                    else
                    {
                        ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='text-align:center;'>KQĐ</td>", columnCount);
                    }
                }
            }
        }
    }
}