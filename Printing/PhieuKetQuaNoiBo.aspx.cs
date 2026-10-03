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
    public partial class PhieuKetQuaNoiBo : System.Web.UI.Page
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

        protected string CurrentPageType
        {
            get { return CommonHelper.QueryString("PageType"); }
        }
        private string CurrentViSinhType
        {
            get
            {
                return CommonHelper.QueryString("CType");
            }
        }

        protected string GetKetQuaHienThi(object data)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu chiTieu = data as TblMauXetNghiemChiTieu;
                if (danhGiaMau) rs = chiTieu.KetQuaHienThiToString;
                else rs = chiTieu.KetQuaHienThi;
            }
            return rs;
        }
        bool danhGiaMau = false;
        int columnCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentID.HasValue)
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null)
                    {
                        danhGiaMau = mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value;
                        if (CurrentPageType == "LaboHoaLy" &&
                            mauXN.NgayHoaLy.HasValue &&
                            mauXN.NgayHoaLy.Value != DateTime.MinValue)
                        {
                            ltrNgayThang.Text = string.Format("Khánh Hòa, ngày {0} tháng {1} năm {2}",
                                mauXN.NgayHoaLy.Value.ToString("dd"), mauXN.NgayHoaLy.Value.ToString("MM"), mauXN.NgayHoaLy.Value.ToString("yyyy"));
                        }

                        if (CurrentPageType == "LaboViSinh" &&
                            mauXN.NgayViSinh.HasValue &&
                            mauXN.NgayViSinh.Value != DateTime.MinValue)
                        {
                            ltrNgayThang.Text = string.Format("Khánh Hòa, ngày {0} tháng {1} năm {2}",
                                mauXN.NgayViSinh.Value.ToString("dd"), mauXN.NgayViSinh.Value.ToString("MM"), mauXN.NgayViSinh.Value.ToString("yyyy"));
                        }

                        ltrLoaiMau.Text = mauXN.TenLoaiMau;
                        ltrDieuKienBaoQuan.Text = mauXN.DieuKienBaoQuan;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                        List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");
                        columnCount = chiTieuCol.Max(p => p.MaxKetQua.Value);
                        string maMau = mauXN.MaMau + "/";
                        if (CurrentPageType == "LaboHoaLy")
                        {
                            divKetQuaThuong.Visible = true;
                            ltrNhanXet.Text = mauXN.LaboHoaNhanXet;
                            ltrMaMau.Text = mauXN.MaSoTraHoaLy;
                            repData.DataSource = chiTieuHL;
                            ltrToTruong.Text = "Ngọ Viết Dự";
                            ltrThoiGianKiemNghiem.Text = string.Format("Từ ngày {0} đến ngày {1}", mauXN.ThoiGianHoaBatDauToString, mauXN.ThoiGianHoaKetThucToString);
                            repData.DataBind();
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(CurrentViSinhType))
                            {
                                divKetQuaThuong.Visible = true;
                                ltrNhanXet.Text = mauXN.LaboSinhNhanXet;
                                ltrMaMau.Text = mauXN.MaSoTraViSinh;
                                ltrToTruong.Text = "Dương Thị Hồng Thắm";
                                repData.DataSource = chiTieuVS;
                                ltrThoiGianKiemNghiem.Text = string.Format("Từ ngày {0} đến ngày {1}", mauXN.ThoiGianVSBatDauToString, mauXN.ThoiGianVSKetThucToString);
                                repData.DataBind();
                            }
                            else if (CurrentViSinhType == "M1")
                            {
                                divKetQuaCB.Visible = true;
                                ltrNhanXet.Text = mauXN.LaboSinhNhanXet;
                                ltrMaMau.Text = mauXN.MaSoTraViSinh;
                                ltrToTruong.Text = "Dương Thị Hồng Thắm";
                                ltrThoiGianKiemNghiem.Text = string.Format("Từ ngày {0} đến ngày {1}", mauXN.ThoiGianVSBatDauToString, mauXN.ThoiGianVSKetThucToString);
                                #region build HTML
                                List<TblMauXetNghiemChiTieu> chiTieuVS2 = new List<TblMauXetNghiemChiTieu>();
                                while (chiTieuVS.Count > 0)
                                {
                                    TblMauXetNghiemChiTieu newCT = new TblMauXetNghiemChiTieu();
                                    string name = chiTieuVS[0].TenTaiCacKhoa.Trim();
                                    string[] arr = name.ToUpper().Split(new string[] { "VỊ TRÍ" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (arr.Length > 1)
                                    {
                                        List<TblMauXetNghiemChiTieu> chiTieuTemp = chiTieuVS.FindAll(p => p.TenTaiCacKhoa.Trim().ToUpper().StartsWith(arr[0]));
                                        if (chiTieuTemp != null && chiTieuTemp.Count > 0)
                                        {
                                            newCT = chiTieuTemp[0];

                                            TblMauXetNghiemChiTieu temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 1"));
                                            if (temp != null) newCT.GiaTriVung1 = temp.KetQuaGiaTri.HasValue ? temp.KetQuaGiaTri.Value : temp.Idctqc;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 2"));
                                            if (temp != null) newCT.GiaTriVung2 = temp.KetQuaGiaTri.HasValue ? temp.KetQuaGiaTri.Value : temp.Idctqc;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 3"));
                                            if (temp != null) newCT.GiaTriVung3 = temp.KetQuaGiaTri.HasValue ? temp.KetQuaGiaTri.Value : temp.Idctqc;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 4"));
                                            if (temp != null) newCT.GiaTriVung4 = temp.KetQuaGiaTri.HasValue ? temp.KetQuaGiaTri.Value : temp.Idctqc;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 5"));
                                            if (temp != null) newCT.GiaTriVung5 = temp.KetQuaGiaTri.HasValue ? temp.KetQuaGiaTri.Value : temp.Idctqc;
                                        }
                                        chiTieuVS.RemoveAll(p => p.TenTaiCacKhoa.Trim().ToUpper().StartsWith(arr[0]));
                                        newCT.TenTaiCacKhoa = name.Substring(0, arr[0].Length);
                                    }
                                    else
                                    {
                                        newCT = chiTieuVS[0];
                                        newCT.GiaTriVung1 = chiTieuVS[0].Idctqc;
                                        chiTieuVS.Remove(chiTieuVS[0]);
                                        newCT.TenTaiCacKhoa = name.Substring(0, arr[0].Length);
                                    }
                                    chiTieuVS2.Add(newCT);
                                }
                                #endregion
                                ltrThoiGianKiemNghiem.Text = string.Format("Từ ngày {0} đến ngày {1}", mauXN.ThoiGianVSBatDauToString, mauXN.ThoiGianVSKetThucToString);

                                repDataCB.DataSource = chiTieuVS2;
                                repDataCB.DataBind();
                            }
                            else if (CurrentViSinhType == "M2")
                            {
                                #region build HTML
                                divKetQuaM2.Visible = true;
                                ltrNhanXet.Text = mauXN.LaboSinhNhanXet;
                                ltrMaMau.Text = mauXN.MaSoTraViSinh;
                                ltrToTruong.Text = "Dương Thị Hồng Thắm";
                                #region gen data
                                List<TblMauXetNghiemChiTieu> chiTieuVS2 = new List<TblMauXetNghiemChiTieu>();
                                ltrThoiGianKiemNghiem.Text = string.Format("Từ ngày {0} đến ngày {1}", mauXN.ThoiGianVSBatDauToString, mauXN.ThoiGianVSKetThucToString);
                                while (chiTieuVS.Count > 0)
                                {
                                    TblMauXetNghiemChiTieu newCT = new TblMauXetNghiemChiTieu();
                                    string name = chiTieuVS[0].TenTaiCacKhoa.Trim();
                                    string[] arr = name.ToUpper().Split(new string[] { "VỊ TRÍ" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (arr.Length > 1)
                                    {
                                        List<TblMauXetNghiemChiTieu> chiTieuTemp = chiTieuVS.FindAll(p => p.TenTaiCacKhoa.Trim().ToUpper().StartsWith(arr[0]));
                                        if (chiTieuTemp != null && chiTieuTemp.Count > 0)
                                        {
                                            newCT = chiTieuTemp[0];

                                            TblMauXetNghiemChiTieu temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 1"));
                                            if (temp != null) newCT.GiaTriVung1 = temp.KetQuaGioiHan;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 2"));
                                            if (temp != null) newCT.GiaTriVung2 = temp.KetQuaGioiHan;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 3"));
                                            if (temp != null) newCT.GiaTriVung3 = temp.KetQuaGioiHan;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 4"));
                                            if (temp != null) newCT.GiaTriVung4 = temp.KetQuaGioiHan;
                                            temp = chiTieuTemp.FirstOrDefault(p => p.TenTaiCacKhoa.Trim().ToUpper().EndsWith("VỊ TRÍ 5"));
                                            if (temp != null) newCT.GiaTriVung5 = temp.KetQuaGioiHan;
                                        }
                                        chiTieuVS.RemoveAll(p => p.TenTaiCacKhoa.Trim().ToUpper().StartsWith(arr[0]));
                                        newCT.TenTaiCacKhoa = name.Substring(0, arr[0].Length);
                                        if (newCT.TenTaiCacKhoa.Trim().EndsWith("-")) newCT.TenTaiCacKhoa = newCT.TenTaiCacKhoa.Trim().Substring(0, newCT.TenTaiCacKhoa.Trim().Length - 1);
                                    }
                                    else
                                    {
                                        newCT = chiTieuVS[0];
                                        newCT.GiaTriVung1 = chiTieuVS[0].Idctqc;
                                        chiTieuVS.Remove(chiTieuVS[0]);
                                        newCT.TenTaiCacKhoa = name.Substring(0, arr[0].Length);
                                        if (newCT.TenTaiCacKhoa.Trim().EndsWith("-")) newCT.TenTaiCacKhoa = newCT.TenTaiCacKhoa.Trim().Substring(0, newCT.TenTaiCacKhoa.Trim().Length - 1);
                                    }
                                    chiTieuVS2.Add(newCT);
                                }
                                #endregion
                                string html = "", theader = "", r1 = "<tr><td style='text-align:center;'>Vị trí 1</td>",
                                r2 = "<tr><td style='text-align:center;'>Vị trí 2</td>", r3 = "<tr><td style='text-align:center;'>Vị trí 3</td>",
                                r4 = "<tr><td style='text-align:center;'>Vị trí 4</td>", r5 = "<tr><td style='text-align:center;'>Vị trí 5</td>";
                                html += string.Format("<table class='tbl-data' style='border: solid 1px black; width: 100 %; '>");
                                theader = "<thead><tr><th style='width: 50px; text-align: center;'><div class='resize'>Vị trí</div></th>";
                                decimal vl1 = 0, vl2 = 0, vl3 = 0, vl4 = 0, vl5 = 0;
                                for (int i = 0; i < chiTieuVS2.Count; i++)
                                {
                                    TblMauXetNghiemChiTieu chiTieu = chiTieuVS2[i];
                                    theader += string.Format("<th style='text-align:center;'><div class='resize'>{0} ({1})</div></th>", chiTieu.TenTaiCacKhoa, chiTieu.TenDonViNA);
                                    r1 += string.Format("<td style='text-align:center;width:100px;'>{0}</td>", chiTieu.GiaTriVung1.HasValue ? chiTieu.GiaTriVung1.Value.ToStringWithLanguage() : "0");
                                    r2 += string.Format("<td style='text-align:center;width:100px;'>{0}</td>", chiTieu.GiaTriVung2.HasValue ? chiTieu.GiaTriVung2.Value.ToStringWithLanguage() : "0");
                                    r3 += string.Format("<td style='text-align:center;width:100px;'>{0}</td>", chiTieu.GiaTriVung3.HasValue ? chiTieu.GiaTriVung3.Value.ToStringWithLanguage() : "0");
                                    r4 += string.Format("<td style='text-align:center;width:100px;'>{0}</td>", chiTieu.GiaTriVung4.HasValue ? chiTieu.GiaTriVung4.Value.ToStringWithLanguage() : "0");
                                    r5 += string.Format("<td style='text-align:center;width:100px;'>{0}</td>", chiTieu.GiaTriVung5.HasValue ? chiTieu.GiaTriVung5.Value.ToStringWithLanguage() : "0");
                                    vl1 += chiTieu.GiaTriVung1.HasValue ? chiTieu.GiaTriVung1.Value : 0; vl2 += chiTieu.GiaTriVung2.HasValue ? chiTieu.GiaTriVung2.Value : 0;
                                    vl3 += chiTieu.GiaTriVung3.HasValue ? chiTieu.GiaTriVung3.Value : 0; vl4 += chiTieu.GiaTriVung4.HasValue ? chiTieu.GiaTriVung4.Value : 0;
                                    vl5 += chiTieu.GiaTriVung5.HasValue ? chiTieu.GiaTriVung5.Value : 0;
                                }
                                #region tính giá trị trung bình
                                decimal average = (vl1 + vl2 + vl3 + vl4 + vl5) / 5;
                                if (average > 100)
                                {
                                    int phanChuc = (int)(average / 10);
                                    if (average - (phanChuc * 10) >= 5) average = (phanChuc + 1) * 10;
                                    else average = phanChuc * 10;
                                }
                                #endregion
                                theader += "<th style='text-align:center;font-weight:bold;'><div class='resize'>Trung bình (CFU/m<sup>3</sup>)</div></th>";
                                theader += "<th style='text-align:center;font-weight:bold;'><div class='resize'>Giá trị hướng dẫn theo WHO 2012 (*)</div></th>";
                                r1 += string.Format("<td rowspan='5' style='text-align:center;font-weight:bold;'>{0}</td>", average.ToStringWithLanguage());
                                r1 += string.Format("<td rowspan='5' style='text-align:center;'>A:<1 CFU / m<sup>3</sup><br/>B:10 CFU / m<sup>3</sup><br/>C:100 CFU / m<sup>3</sup><br/>D:200 CFU / m<sup>3</sup></td></tr>");
                                r2 += "</tr>"; r3 += "</tr>"; r4 += "</tr>"; r5 += "</tr>";
                                theader += "</tr></thead><tbody>";
                                html += theader;
                                html += r1; html += r2; html += r3; html += r4; html += r5;
                                html += "</tbody></table>";
                                ltrDataM2.Text = html;
                                #endregion
                            }

                        }


                        int total = chiTieuCol.Count;
                        int hoaLy = chiTieuHL.Count;
                        int viSinh = total - hoaLy;


                        if (mauXN.LuongMauHoa.Value > 0) maMau += "HH-";
                        else maMau += "00-";


                        if ((viSinh) > 0) maMau += "VS";
                        else maMau += "00";
                        ltrMaSoMau.Text = maMau;
                        ltrNgayNhanMau.Text = mauXN.NgayNhanMauToString;
                        ltrThoiGianLuuMau.Text = string.Format("{0} ngày", CurrentPageType == "LaboHoaLy" ? "10" : "5");
                        ltrFooter.Text = "";
                        if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                        {
                            ltrColumnHeaderGroup.Text = string.Format("<td colspan='{0}' {1} style='text-align:center;font-weight:bold;min-width:150px;'>Giới hạn cho phép <sup>(*)</sup></td>", columnCount, columnCount == 1 ? " rowspan='2' " : "");
                            if (columnCount > 0)
                            {
                                string[] arr = { "A", "B", "C", "D" };
                                if (mauXN.MauTheoABCD.HasValue && !mauXN.MauTheoABCD.Value) arr = new string[] { "A1", "A2", "B1", "B2" };
                                ltrColumnHeader.Text = "";
                                if (columnCount > 1)
                                {
                                    for (int i = 0; i < columnCount; i++)
                                        ltrColumnHeader.Text += string.Format("<th style='text-align:center;width:50px;'>{0}</th>", arr[i]);
                                }
                            }


                            ltrFooter.Text += string.Format("(*): Giới hạn tối đa cho phép theo {0} ({1});<br />", mauXN.MieuTaQuyChuan, mauXN.TenQuyChuan);
                        }
                        TblMauXetNghiemChiTieu iso = chiTieuCol.FirstOrDefault(p => p != null && !string.IsNullOrEmpty(p.TenXuatKetQua) && p.TenXuatKetQua.Contains("(a)"));
                        if (iso != null) ltrFooter.Text += "(a): Phép thử được công nhận phù hợp với ISO/IEC 17025:2017<br/>";

                        TblMauXetNghiemChiTieu kqd = chiTieuCol.FirstOrDefault(p => p != null && p.KetQuaHienThiToString.Contains("KQĐ"));
                        if (iso != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value) ltrFooter.Text += "KQĐ: Không Quy Định<br/>";
                    }
                }
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
        protected string GetGioiHanChoPhep(object data)
        {
            string rs = "KQĐ";
            if (data is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu chiTieu = data as TblMauXetNghiemChiTieu;
                if (chiTieu != null && chiTieu.Idctqc.HasValue && chiTieu.Idctqc.Value > 0)
                {
                    TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByID(chiTieu.Idctqc.Value);
                    if (ctqc != null)
                    {
                        rs = "";
                        if (ctqc.LoaiHinhGioiHan == "NhapSo")
                        {
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (ctqc.GioiHan1.HasValue) rs += ctqc.GioiHan1.Value.ToStringWithLanguage();
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (ctqc.GioiHan2.HasValue) rs += ctqc.GioiHan2.Value.ToStringWithLanguage();
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (ctqc.GioiHan3.HasValue) rs += ctqc.GioiHan3.Value.ToStringWithLanguage();
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (ctqc.GioiHan4.HasValue) rs += ctqc.GioiHan4.Value.ToStringWithLanguage();
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc1)) rs += ctqc.TenDanhMuc1;
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc2)) rs += ctqc.TenDanhMuc2;
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc3)) rs += ctqc.TenDanhMuc3;
                            if (!string.IsNullOrEmpty(rs)) rs += "-";
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc4)) rs += ctqc.TenDanhMuc4;
                        }
                    }
                }
            }

            while (rs.EndsWith("-")) rs = rs.Substring(0, rs.Length - 1);
            return rs;
        }

        private string GetSub(TblChiTieuQuyChuan ctqc)
        {
            string rs = "";
            string[] phepTinh = { "LHGH", "BHGH", "LHBGH", "BHBGH", "TKGH", "TKHBGH" };
            //string[] phepDat = { "<span> > </span>", "<span> < </span>", "<span style='text-decoration:underline;'> > </span>", "<span style='text-decoration:underline;'> < </span>", "TKGH", "TKHBGH" };
            //string[] phepKhongDat = { "<span style='text-decoration:underline;'> < </span>", "<span style='text-decoration:underline;'> > </span>", "<span> < </span>", "<span> > </span>", "TKGH", "TKHBGH" };


            return rs;
        }

        protected void repData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu &&
               (e.Item.ItemType == ListItemType.Item ||
               e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);

                if (mauXN != null)
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

                            if (ltrGioiHanChoPhep != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value) ltrGioiHanChoPhep.Text = rs;

                        }
                        else if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                        {
                            ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='text-align:center;'>KQĐ</td>", columnCount);
                        }
                    }
                    else if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                    {
                        ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='text-align:center;'>KQĐ</td>", columnCount);
                    }
                }
            }
        }

        private List<TblCatChiTieuGioiHan> CurrentChiTieuGioiHanList
        {
            get
            {
                List<TblCatChiTieuGioiHan> dvt = null;
                if (Session["MXN2-CTGH"] != null)
                {
                    dvt = Session["MXN2-CTGH"] as List<TblCatChiTieuGioiHan>;

                }
                if (dvt == null) dvt = CategoryManager.GetChiTieuGioiHanAllWithQuyChuan(true);
                Session["MXN2-CTGH"] = dvt;
                return dvt;
            }
        }
        protected void repDataCB_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu &&
              (e.Item.ItemType == ListItemType.Item ||
              e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                if (mauXN != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                {
                    Literal ltrData = e.Item.FindControl("ltrData") as Literal;
                    TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
                    if (chiTieu != null)
                    {

                        //TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByID(chiTieu.Idctqc.Value);
                        //if (ctqc != null)
                        //{ }
                        int rowspan = 0;
                        string rows = "";
                        #region Dòng phụ
                        //if (chiTieu.GiaTriVung1.HasValue && chiTieu.GiaTriVung1.Value > 0)
                        //{
                        //    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung1.Value).ToString());
                        //    if (ctgh != null)
                        //    {
                        //        rowspan += 1;
                        //        rows += string.Format("<td style='text-align:center;'>Vị trí 1</td><td style='text-align:center;'>{0}</td>", ctgh.Ten);
                        //    }
                        //}
                        if (chiTieu.GiaTriVung2.HasValue && chiTieu.GiaTriVung2.Value > 0)
                        {

                            TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung2.Value).ToString());
                            if (ctgh != null)
                            {
                                rowspan += 1;
                                rows += string.Format("<tr><td style='text-align:center;width:70px;'>Vị trí 2</td><td style='text-align:center;width:80px;'>{0}</td></tr>", ctgh.Ten);
                            }
                        }
                        if (chiTieu.GiaTriVung3.HasValue && chiTieu.GiaTriVung3.Value > 0)
                        {
                            TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung3.Value).ToString());
                            if (ctgh != null)
                            {
                                rowspan += 1;
                                rows += string.Format("<tr><td style='text-align:center;'>Vị trí 3</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
                            }
                        }
                        if (chiTieu.GiaTriVung4.HasValue && chiTieu.GiaTriVung4.Value > 0)
                        {
                            TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung4.Value).ToString());
                            if (ctgh != null)
                            {
                                rowspan += 1;
                                rows += string.Format("<tr><td style='text-align:center;'>Vị trí 4</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
                            }
                        }
                        if (chiTieu.GiaTriVung5.HasValue && chiTieu.GiaTriVung5.Value > 0)
                        {
                            TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung5.Value).ToString());
                            if (ctgh != null)
                            {
                                rowspan += 1;
                                rows += string.Format("<tr><td style='text-align:center;'>Vị trí 5</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
                            }
                        }
                        rowspan++;
                        #endregion
                        string rs = string.Format("<tr>");
                        rs += string.Format("<td {0} style='text-align:center;width:30px;'>{1}</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "", (e.Item.ItemIndex + 1));
                        rs += string.Format("<td {0} style='text-align:center;width:150px;'>{1}</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "", GetTen(chiTieu.TenXuatKetQuaAB, chiTieu.Italic));
                        rs += string.Format("<td {0} style='text-align:center;width:150px;'>{1}</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "", chiTieu.TenPhuongPhapHienThi);
                        if (chiTieu.GiaTriVung1.HasValue && chiTieu.GiaTriVung1.Value > 0)
                        {
                            TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung1.Value).ToString());
                            if (ctgh != null)
                            {
                                //rowspan += 1;
                                rs += string.Format("<td style='text-align:center;'>Vị trí 1</td><td style='text-align:center;'>{0}</td>", ctgh.Ten);
                            }
                        }
                        rs += string.Format("<td {0} style='text-align:center;width:150px;'>Không có sự phát triển của vi sinh vật</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "");
                        rs += string.Format("</tr>");
                        rs += rows;

                        ltrData.Text = rs;

                    }
                }
            }
        }
    }
}