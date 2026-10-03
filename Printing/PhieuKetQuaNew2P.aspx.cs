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
    public partial class PhieuKetQuaNew2P : System.Web.UI.Page
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

        private int? CurrentPage
        {
            get
            {
                int u = 0;
                if (int.TryParse(CommonHelper.QueryString("CPage"), out u) && u > 0)
                    return u;
                return null;
            }
        }

        private string CurrentType
        {
            get
            {
                return CommonHelper.QueryString("CType");
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
                rs = string.Format("<a class='a-link' href='/phieu-tra-ket-qua-so-{0}-P{2}'>{1}</a>", CurrentID.Value, ten.ToString(), columnIndex);

            }
            return rs;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentID.HasValue)
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null)
                    {
                        ltrPageHeader.Text = mauXN.MaSoTraKQ;
                        danhGiaMau = mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value;
                        ltrNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        if (mauXN.NgayTaoSoTraMau.HasValue && mauXN.NgayTaoSoTraMau.Value != DateTime.MinValue)
                            ltrNgay.Text = mauXN.NgayTaoSoTraMau.Value.ToString("dd/MM/yyyy");
                        ltrLoaiMau.Text = mauXN.TenLoaiMau;
                        ltrThongTin.Text = mauXN.MaMau;
                        ltrDieuKienBaoQuan.Text = mauXN.DieuKienBaoQuan;

                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                        ltr1.Visible = chiTieuCol.FirstOrDefault(p => p.TenXuatKetQuaAB.Trim().Contains("(1)")) != null;
                        ltrA.Visible = chiTieuCol.FirstOrDefault(p => p.TenXuatKetQuaAB.Trim().Contains("(a)")) != null;
                        columnCount = chiTieuCol.Max(p => p.MaxKetQua.Value);

                        List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");

                        List<TblMauXetNghiemChiTieu> dsPage1 = new List<TblMauXetNghiemChiTieu>();
                        List<TblMauXetNghiemChiTieu> dsPage2 = new List<TblMauXetNghiemChiTieu>();
                        //if (CurrentPage.HasValue && CurrentPage.Value < chiTieuCol.Count)
                        {
                            dsPage1 = new List<TblMauXetNghiemChiTieu>();
                            for (int i = 0; i < chiTieuCol.Count; i++)
                            {
                                if (!CurrentPage.HasValue || (CurrentPage.HasValue && i < CurrentPage.Value))
                                    dsPage1.Add(chiTieuCol[i]);
                                else
                                    dsPage2.Add(chiTieuCol[i]);
                            }

                        }
                        if (string.IsNullOrEmpty(CurrentType))
                        {
                            divKetQuaThuong.Visible = true;
                            if (!CurrentPage.HasValue) divBM.Attributes.Add("class", "item-visiblity bottom");
                            repData.DataSource = dsPage1;
                            repData.DataBind();
                            repData2.DataSource = dsPage2;
                            repData2.DataBind();
                        }
                        else if (CurrentType == "M1")
                        {
                            divKetQuaCB.Visible = true;
                            //ltrNhanXet.Text = mauXN.LaboSinhNhanXet;
                            //ltrMaMau.Text = mauXN.MaSoTraViSinh;
                            //ltrToTruong.Text = "Dương Thị Hồng Thắm";
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
                        else if (CurrentType == "M2")
                        {
                            #region build HTML
                            divKetQuaM2.Visible = true;
                            //ltrNhanXet.Text = mauXN.LaboSinhNhanXet;
                            //ltrMaMau.Text = mauXN.MaSoTraViSinh;
                            //ltrToTruong.Text = "Dương Thị Hồng Thắm";
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
                                theader += string.Format("<th style='text-align:center;'><div class='resize'>{0} ({1})</div></th>", chiTieu.TenTaiCacKhoa, chiTieu.TenDonVi);
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
                        ltrKhachHang.Text = string.Format("<span class='t-content' style='display:inline-block;width:410px;text-align: justify;'>: {0}</span>", mauXN.TenKhachHang);
                        ltrDiaChi.Text = string.Format("<span class='t-content' style='display:inline-block;width:410px;text-align: justify;'>: {0}</span>", mauXN.DiaChiKhachHang);
                        ltrNoiLayMau.Text = string.Format("<span  class='t-content' style='display:inline-block;width:410px;text-align: justify;'>: {0}</span>", mauXN.NoiLayMau);
                        ltrMieuTaMau.Text = string.Format("<span class='t-content' style='display:inline-block;width:410px;text-align: justify;'>: {0}</span>", mauXN.MoTaMau);
                        ltrNguoiLayMau.Text = mauXN.TenNguoiLayMau;
                        ltrNguoiGui.Text = mauXN.TenNguoiGuiMau;
                        ltrNgayLayMau.Text = mauXN.NgayLayMauToString;
                        ltrNgayNhanMau.Text = mauXN.NgayNhanMauToString;
                        DateTime dt = DateTime.MinValue;
                        if (mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value > dt) dt = mauXN.NgayHoaLy.Value;
                        if (mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value > dt) dt = mauXN.NgayViSinh.Value;
                        //if(mauXN.NgayKiemNghiemBatDau != mauXN.NgayKiemNghiemKetThuc)
                        // ltrThoiGianKiemNghiem.Text = string.Format("Từ {0} đến {1}", mauXN.NgayKiemNghiemBatDau, mauXN.NgayKiemNghiemKetThuc);
                        //else
                        ltrThoiGianKiemNghiem.Text = string.Format("Từ ngày {0}", mauXN.NgayKiemNghiemBatDau, mauXN.NgayKiemNghiemKetThuc);
                        ltrThoiGianLuuTru.Text = string.Format("{0} ngày kể từ ngày trả kết quả", chiTieuHL.Count > 0 ? 10 : 5);
                        ltrKetQuaThuNghiem.Text = string.Format("Xem các trang sau");
                        if (!string.IsNullOrEmpty(mauXN.KetQuaDanhGiaMau))
                        {
                            ltrGhiChuM1.Text = ltrGhiChuM2.Text = ltrNhanXet.Text = "<b>Nhận xét: </b>" + mauXN.NhanXetCuoiCung.Replace("[KQKT]", string.Format("<b>{0}</b>", mauXN.KetQuaDanhGiaMau == "1" ? "đạt" : "không đạt")); ;

                        }

                        if (string.IsNullOrEmpty(CurrentType) && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                        {
                            ltrColumnHeaderGroup.Text = ltrColumnHeaderGroup1.Text = string.Format("<td colspan='{0}' {1} style='text-align:center;font-weight:bold;width:{2}px;'>Giới hạn tối đa cho phép <sup>(*)</sup> / Maximum limit (*)</td>", columnCount, columnCount == 1 ? " rowspan='2' " : "", columnCount == 1 ? 100 : columnCount * 50);
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

                        //ltrThongTin.Text = string.Format("<tr><td colspan='{0}' style='text-align: right; border: solid 1px white; border-bottom: solid 1px black;'>Mã trả kết quả: {1}</td></tr>", (4 + columnCount), mauXN.MaSoTraKQ);
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

                        if (!mauXN.NgayInKetQua.HasValue || (mauXN.NgayInKetQua.HasValue && mauXN.NgayInKetQua.Value == DateTime.MinValue))
                        {
                            mauXN.NgayInKetQua = DateTime.Now;
                            XetNghiemManager.UpdateMauXetNghiem(mauXN);
                        }
                    }
                }
            }
        }
        private int columnCount = 0;
        private int columnIndex = 1;
        protected string GetPageIndex()
        {
            return string.Format("{1}", CurrentID.Value, columnIndex);
        }

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
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenGioiHan1, columnCount == 1 ? 100 : columnCount * 50);
                                }
                                if (ctqc.GioiHan2.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenGioiHan2, columnCount == 1 ? 100 : columnCount * 50);
                                }
                                if (ctqc.GioiHan3.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenGioiHan3, columnCount == 1 ? 100 : columnCount * 50);
                                }
                                if (ctqc.GioiHan4.HasValue)
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenGioiHan4, columnCount == 1 ? 100 : columnCount * 50);
                                }
                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc1))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenDanhMuc1, columnCount == 1 ? 100 : columnCount * 50);
                                }
                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc2))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenDanhMuc2, columnCount == 1 ? 100 : columnCount * 50);
                                }
                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc3))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenDanhMuc3, columnCount == 1 ? 100 : columnCount * 50);
                                }
                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc4))
                                {
                                    currentColumn++; rs += string.Format("<td style='text-align: center;vertical-align:text-bottom;width:{1}px;'>{0}</td>", ctqc.TenDanhMuc4, columnCount == 1 ? 100 : columnCount * 50);
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
                            ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='vertical-align:text-bottom;text-align:center;width:50px;'>KQĐ</td>", columnCount);
                        }
                    }
                    else
                    {
                        ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='vertical-align:text-bottom;text-align:center;width:50px;'>KQĐ</td>", columnCount);
                    }

                }
                columnIndex++;
            }
        }
        private List<TblCatChiTieuGioiHan> CurrentChiTieuGioiHanList
        {
            get
            {
                List<TblCatChiTieuGioiHan> dvt = null;
                if (Session["MXN-CTGH"] != null)
                {
                    dvt = Session["MXN-CTGH"] as List<TblCatChiTieuGioiHan>;

                }
                if (dvt == null) dvt = CategoryManager.GetChiTieuGioiHanAllWithQuyChuan(true);
                Session["MXN-CTGH"] = dvt;
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

        //protected void repDataCB_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu &&
        //      (e.Item.ItemType == ListItemType.Item ||
        //      e.Item.ItemType == ListItemType.AlternatingItem))
        //    {
        //        TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
        //        if (mauXN != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
        //        {
        //            Literal ltrData = e.Item.FindControl("ltrData") as Literal;
        //            TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
        //            if (chiTieu != null)
        //            {

        //                //TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByID(chiTieu.Idctqc.Value);
        //                //if (ctqc != null)
        //                //{ }
        //                int rowspan = 0;
        //                string rows = "";
        //                #region Dòng phụ
        //                //if (chiTieu.GiaTriVung1.HasValue && chiTieu.GiaTriVung1.Value > 0)
        //                //{
        //                //    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung1.Value).ToString());
        //                //    if (ctgh != null)
        //                //    {
        //                //        rowspan += 1;
        //                //        rows += string.Format("<td style='text-align:center;'>Vị trí 1</td><td style='text-align:center;'>{0}</td>", ctgh.Ten);
        //                //    }
        //                //}
        //                if (chiTieu.GiaTriVung2.HasValue && chiTieu.GiaTriVung2.Value > 0)
        //                {
        //                    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung2.Value).ToString());
        //                    if (ctgh != null)
        //                    {
        //                        rowspan += 1;
        //                        rows += string.Format("<tr><td style='text-align:center;'>Vị trí 2</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
        //                    }
        //                }
        //                if (chiTieu.GiaTriVung3.HasValue && chiTieu.GiaTriVung3.Value > 0)
        //                {
        //                    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung3.Value).ToString());
        //                    if (ctgh != null)
        //                    {
        //                        rowspan += 1;
        //                        rows += string.Format("<tr><td style='text-align:center;'>Vị trí 3</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
        //                    }
        //                }
        //                if (chiTieu.GiaTriVung4.HasValue && chiTieu.GiaTriVung4.Value > 0)
        //                {
        //                    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung4.Value).ToString());
        //                    if (ctgh != null)
        //                    {
        //                        rowspan += 1;
        //                        rows += string.Format("<tr><td style='text-align:center;'>Vị trí 4</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
        //                    }
        //                }
        //                if (chiTieu.GiaTriVung5.HasValue && chiTieu.GiaTriVung5.Value > 0)
        //                {
        //                    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung5.Value).ToString());
        //                    if (ctgh != null)
        //                    {
        //                        rowspan += 1;
        //                        rows += string.Format("<tr><td style='text-align:center;'>Vị trí 5</td><td style='text-align:center;'>{0}</td></tr>", ctgh.Ten);
        //                    }
        //                }
        //                rowspan++;
        //                #endregion
        //                string rs = string.Format("<tr>");
        //                rs += string.Format("<td {0} style='text-align:center;width:30px;'>{1}</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "", (e.Item.ItemIndex + 1));
        //                rs += string.Format("<td {0} style='text-align:center;width:150px;'>{1}</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "", GetTen(chiTieu.TenXuatKetQuaAB, chiTieu.Italic));
        //                rs += string.Format("<td {0} style='text-align:center;width:150px;'>{1}</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "", chiTieu.TenPhuongPhapHienThi);
        //                if (chiTieu.GiaTriVung1.HasValue && chiTieu.GiaTriVung1.Value > 0)
        //                {
        //                    TblCatChiTieuGioiHan ctgh = CurrentChiTieuGioiHanList.FirstOrDefault(p => p.Id.ToString() == Decimal.ToInt32(chiTieu.GiaTriVung1.Value).ToString());
        //                    if (ctgh != null)
        //                    {
        //                        //rowspan += 1;
        //                        rs += string.Format("<td style='text-align:center;'>Vị trí 1</td><td style='text-align:center;'>{0}</td>", ctgh.Ten);
        //                    }
        //                }
        //                rs += string.Format("<td {0} style='text-align:center;width:150px;'>Không có sự phát triển của vi sinh vật</td>", rowspan > 1 ? string.Format(" rowspan='{0}' ", rowspan) : "");
        //                rs += string.Format("</tr>");
        //                rs += rows;

        //                ltrData.Text = rs;

        //            }
        //        }
        //    }
        //}
    }
}