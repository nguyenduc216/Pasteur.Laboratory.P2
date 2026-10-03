using Microsoft.Reporting.WebForms;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Printing
{
    public partial class PrintPhieuKetQuaKiermNghiemPhoi : System.Web.UI.Page
    {
        private int? CurrentID
        {
            get
            {
                int ID = 0;
                if (Request.QueryString["ID"] != null)
                {
                    int.TryParse(Request.QueryString["ID"], out ID);
                }
                return ID;
            }
        }

        ReportViewer viewer = new ReportViewer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentID > 0)
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null)

                    {
                        // Setup the report viewer object and get the array of bytes
                        viewer.ProcessingMode = ProcessingMode.Local;

                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType = string.Empty;
                        string encoding = string.Empty;
                        string extension = string.Empty;

                        {
                            List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                            int columnCount = chiTieuCol.Max(p => p.MaxKetQua.Value);

                            List<TblMauXetNghiemChiTieu> chiTieuHL = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                            List<TblMauXetNghiemChiTieu> chiTieuVS = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");
                            int total = chiTieuCol.Count;
                            int hoaLy = chiTieuHL.Count;
                            int viSinh = total - hoaLy;

                            string maMau = mauXN.MaMau + "/";
                            if (mauXN.LuongMauHoa.Value > 0) maMau += "HH-";
                            else maMau += "00-";
                            if ((viSinh) > 0) maMau += "VS";
                            else maMau += "00";

                            ReportParameter NgayThangNam, ghichuSao, truongKhoa, maTraKetQua;

                            NgayThangNam = new ReportParameter("NGAYTHANG", DateTime.Now.Year.ToString("dd/MM/yyyy"));
                            ghichuSao = new ReportParameter("GhiChuSAO", "");
                            truongKhoa = new ReportParameter("TENTRUONGKHOA", "");
                            maTraKetQua = new ReportParameter("MATRAKQ", mauXN.MaSoTraKQ);

                            {
                                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PhieuKetQuaKiemNghiemPhoi.rdlc");
                                if (mauXN != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PhieuKetQuaKiemNghiemPhoiKhongDanhGia.rdlc");
                                else if (columnCount == 1)
                                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PhieuKetQuaKiemNghiemPhoi01.rdlc");
                                else if (columnCount == 2)
                                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PhieuKetQuaKiemNghiemPhoi02.rdlc");
                                else if (columnCount == 3)
                                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PhieuKetQuaKiemNghiemPhoi03.rdlc");
                                else if (columnCount == 4)
                                    viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/PhieuKetQuaKiemNghiemPhoi04.rdlc");
                                viewer.LocalReport.DataSources.Clear();
                            }

                            #region Data Header
                            DataTable tableHeader = new DataTable("TableHeader");
                            tableHeader.Columns.Add(new DataColumn("MaMau", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("MaSoTraKQ", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("NgayTraKQ", typeof(string)));
                            DataRow r = tableHeader.NewRow();
                            r["MaMau"] = maMau;
                            r["MaSoTraKQ"] = mauXN.MaSoTraKQ;
                            r["NgayTraKQ"] = DateTime.Now.ToString("dd/MM/yyyy");
                            tableHeader.Rows.Add(r);
                            viewer.LocalReport.DataSources.Add(new ReportDataSource("TableHeader", tableHeader));
                            #endregion

                            #region Data Header
                            DataTable pkq_Header = new DataTable("PKQ_Header");
                            pkq_Header.Columns.Add(new DataColumn("LabelName", typeof(string)));
                            pkq_Header.Columns.Add(new DataColumn("LabelValue", typeof(string)));

                            DataRow r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Khách hàng / Client";
                            r2["LabelValue"] = mauXN.TenKhachHang;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Địa chỉ / Address";
                            r2["LabelValue"] = mauXN.DiaChiKhachHang;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Loại mẫu / Type of sample";
                            r2["LabelValue"] = mauXN.TenLoaiMau;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Nơi lấy mẫu / Place of sampling";
                            r2["LabelValue"] = mauXN.NoiLayMau;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Mô tả mẫu / Sample description";
                            r2["LabelValue"] = mauXN.MoTaMau;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Ngày lấy mẫu / Date of sampling";
                            r2["LabelValue"] = mauXN.NgayLayMauToString;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Ngày nhận mẫu / Sample received on";
                            r2["LabelValue"] = mauXN.NgayNhanMauToString;
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Ngày kiểm nghiệm / Sample tested on";
                            r2["LabelValue"] = string.Format("Từ {0} đến {1}", mauXN.NgayKiemNghiemBatDau, mauXN.NgayKiemNghiemKetThuc);
                            pkq_Header.Rows.Add(r2);
                            r2 = pkq_Header.NewRow();
                            r2["LabelName"] = "Thời gian lưu mẫu / Sample storage period";
                            r2["LabelValue"] = string.Format("{0} ngày kể từ ngày trả kết quả", chiTieuHL.Count > 0 ? 10 : 5);
                            pkq_Header.Rows.Add(r2);

                            viewer.LocalReport.DataSources.Add(new ReportDataSource("PKQ_Header", pkq_Header));
                            #endregion

                            #region Data Center
                            DataTable tableCenter = new DataTable("PKQ_DATA");
                            tableHeader.Columns.Add(new DataColumn("STT", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("TenXuatKetQuaAB", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("Italic", typeof(bool)));
                            tableHeader.Columns.Add(new DataColumn("TenTaiCacKhoa", typeof(string)));


                            tableHeader.Columns.Add(new DataColumn("TenPhuongPhapTLGOC", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("KetQuaHienThiToString", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("KetQua01", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("KetQua02", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("KetQua03", typeof(string)));
                            tableHeader.Columns.Add(new DataColumn("KetQua04", typeof(string)));

                            for (int i = 0; i < chiTieuCol.Count; i++)
                            {
                                TblMauXetNghiemChiTieu chiTieu = chiTieuCol[i];
                                chiTieu.STT = (i + 1);
                                if (mauXN != null && mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                                {
                                    if (chiTieu != null && chiTieu.Idctqc.HasValue && chiTieu.Idctqc.Value > 0)
                                    {
                                        TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByID(chiTieu.Idctqc.Value);
                                        if (ctqc != null)
                                        {
                                            //string rs = "";
                                            //int currentColumn = 0;
                                            if (ctqc.LoaiHinhGioiHan == "NhapSo")
                                            {
                                                if (ctqc.GioiHan1.HasValue)
                                                    chiTieu.KetQua01 = ctqc.TenGioiHan1;
                                                if (ctqc.GioiHan2.HasValue)
                                                    chiTieu.KetQua02 = ctqc.TenGioiHan2;
                                                if (ctqc.GioiHan3.HasValue)
                                                    chiTieu.KetQua03 = ctqc.TenGioiHan3;
                                                if (ctqc.GioiHan4.HasValue)
                                                    chiTieu.KetQua04 = ctqc.TenGioiHan4;
                                            }
                                            else
                                            {

                                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc1))
                                                    chiTieu.KetQua01 = ctqc.TenGioiHan1;
                                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc2))
                                                    chiTieu.KetQua02 = ctqc.TenDanhMuc2;
                                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc3))
                                                    chiTieu.KetQua03 = ctqc.TenDanhMuc3;
                                                if (!string.IsNullOrEmpty(ctqc.TenDanhMuc4))
                                                    chiTieu.KetQua04 = ctqc.TenDanhMuc4;
                                            }


                                        }
                                        else
                                        {
                                            chiTieu.KetQua01 = "KQĐ"; chiTieu.KetQua02 = "KQĐ"; chiTieu.KetQua03 = "KQĐ"; chiTieu.KetQua04 = "KQĐ";
                                        }
                                    }
                                    else
                                    {
                                        chiTieu.KetQua01 = "KQĐ"; chiTieu.KetQua02 = "KQĐ"; chiTieu.KetQua03 = "KQĐ"; chiTieu.KetQua04 = "KQĐ";
                                    }
                                }
                            }
                            viewer.LocalReport.DataSources.Add(new ReportDataSource("PKQ_DATA", chiTieuCol));
                            #endregion

                            viewer.LocalReport.SetParameters(new ReportParameter[] { NgayThangNam, ghichuSao, truongKhoa, maTraKetQua });
                            //Chuyển sang Excel
                            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                            //if (extension.Trim().ToUpper() == "XLS") extension = "xlsx";
                            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
                            Response.Write("a");
                            Response.Buffer = true;
                            Response.Clear();
                            Response.ContentType = mimeType;
                            Response.AddHeader("content-disposition", "content-disposition: inline; filename=PhieuKetQuaKiemNghiemPhoi." + extension);
                            Response.BinaryWrite(bytes); // create the file
                            Response.Flush(); // send it to the client to download
                        }
                    }
                }
            }
        }
    }
}