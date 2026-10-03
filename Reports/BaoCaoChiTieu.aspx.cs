using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.Core;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;

namespace Pasteur.Laboratory.P2.Reports
{
    public partial class BaoCaoChiTieu : BasePage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_ChiTieu;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_ChiTieu;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Báo cáo chỉ tiêu";

                return rs;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dateFrom.DateValue = DateTimeHelper.AddFirstDayOfMonth(DateTime.Now);
                dateTo.DateValue = DateTime.Now;
                LoadData();
            }
        }

        private void LoadData()
        {
            //if (dateTo.DateValue == DateTime.MinValue)
            //{
            //    AddValidPromt(dateTo.ClientID, "Chọn ngày tới");
            //    dateTo.Focus();
            //}

            //if (dateFrom.DateValue == DateTime.MinValue)
            //{
            //    AddValidPromt(dateFrom.ClientID, "Chọn ngày bắt đầu");
            //    dateFrom.Focus();
            //}

            if (PageIsValid)
            {
                string html = "";
                string addition = "";// "AND mxn.TrangThaiMau in ('DaTraKetQua')";
                if (!string.IsNullOrEmpty(cbHoaSinh.SelectedValue))
                {
                    if (cbHoaSinh.SelectedValue == "Hoa")
                        addition = " and mxn.LuongMauHoa > 0 ";
                    else
                        addition = " and (mxn.LuongMau - mxn.LuongMauHoa) > 0 ";
                }

                if (!string.IsNullOrEmpty(cbTinhTrangDuyet.SelectedValue))
                {
                    if (cbTinhTrangDuyet.SelectedValue == "2") addition += string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau, cbTinhTrangDuyet.SelectedValue);
                    else if (cbTinhTrangDuyet.SelectedValue == "3") addition += string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau, cbTinhTrangDuyet.SelectedValue);
                    else if (cbTinhTrangDuyet.SelectedValue == "4") addition += string.Format(" AND mxn.NgayHoaLy is not null ");
                    else if (cbTinhTrangDuyet.SelectedValue == "5") addition += string.Format(" AND mxn.NgayViSinh is not null ");
                    else addition += string.Format(" AND mxn.{0} = '{1}' ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau, cbTinhTrangDuyet.SelectedValue);
                }

                if (dateFrom.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateTo.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateVSDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateVSDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));

                List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.ReportGetChiTieu(dateFrom.DateValue, dateTo.DateValue, addition);
                List<TblCatLoaiHinhYeuCau> loaiHinh = CategoryManager.GetLoaiHinhYeuCauAll(true);



                string[] header = { "HoaLy", "ViSinh", "" };
                string[] headerText = { "Hóa", "Vi sinh", "Tổng (Hóa + Vi Sinh)" };
                decimal total = 0;
                for (int j = 0; j < headerText.Length; j++)
                {
                    string str = headerText[j];
                    string str2 = header[j];
                    for (int i = 0; i < loaiHinh.Count; i++)
                    {
                        html += "<tr>";
                        if (i == 0)
                            html += string.Format("<td style=';border:solid 1px #000000;padding-left:10px;' rowspan='{0}'>{1}</td>", loaiHinh.Count, str);
                        html += string.Format("<td style=';border:solid 1px #000000;padding-left:10px;'>{0}</td>", loaiHinh[i].Ten);

                        int chiTieu = 0; decimal tien = 0;
                        List<TblMauXetNghiemChiTieu> chiTieuSub = new List<TblMauXetNghiemChiTieu>();
                        if (!string.IsNullOrEmpty(cbHoaSinh.SelectedValue) && cbHoaSinh.SelectedValue == "Hoa" && str2 == "HoaLy")
                            chiTieuSub = chiTieuCol.FindAll(p => p.IDLoaiHinhYeuCauMau.HasValue &&
                                                            p.IDLoaiHinhYeuCauMau.Value.ToString() == loaiHinh[i].Id.ToString() &&
                                                            (string.IsNullOrEmpty(str2) ||
                                                            (!string.IsNullOrEmpty(str2) && p.HoaSinh == "HoaLy")));
                        if (!string.IsNullOrEmpty(cbHoaSinh.SelectedValue) && cbHoaSinh.SelectedValue != "Hoa" && str2 == "ViSinh")
                            chiTieuSub = chiTieuCol.FindAll(p => p.IDLoaiHinhYeuCauMau.HasValue &&
                                                            p.IDLoaiHinhYeuCauMau.Value.ToString() == loaiHinh[i].Id.ToString() &&
                                                            (string.IsNullOrEmpty(str2) ||
                                                            (!string.IsNullOrEmpty(str2) && p.HoaSinh == "ViSinh")));
                        else if (string.IsNullOrEmpty(cbHoaSinh.SelectedValue))
                            chiTieuSub = chiTieuCol.FindAll(p => p.IDLoaiHinhYeuCauMau.HasValue &&
                                                            p.IDLoaiHinhYeuCauMau.Value.ToString() == loaiHinh[i].Id.ToString() &&
                                                            (string.IsNullOrEmpty(str2) ||
                                                            (!string.IsNullOrEmpty(str2) && p.HoaSinh == str2)));
                        List<TblMauXetNghiemChiTieu> mau = chiTieuSub.GroupBy(c => new
                        {
                            c.MaMau,
                            c.TongTien
                        })
                       .Select(gcs => new TblMauXetNghiemChiTieu()
                       {
                           MaMau = gcs.Key.MaMau,
                           TongTien = gcs.Key.TongTien,
                           Children = gcs.ToList()
                       }).ToList<TblMauXetNghiemChiTieu>();
                        chiTieu = chiTieuSub.Count;
                        tien = mau.Sum(p => p.TongTien.HasValue ? p.TongTien.Value : 0); //chiTieuSub.Sum(p => p.DonGia.HasValue ? p.DonGia.Value : 0);
                        html += string.Format("<td style=';border:solid 1px #000000;text-align:center;'>{0}</td>", mau.Count);
                        html += string.Format("<td style=';border:solid 1px #000000;text-align:center;'>{0}</td>", chiTieu.ToString("N0"));
                        html += string.Format("<td style=';border:solid 1px #000000;text-align:right;padding-right:10px;'>{0}</td>", tien.ToStringWithLanguage());
                        html += "</tr>";
                        if (j == 2)
                            total += mau.Sum(p => p.TongTien.HasValue ? p.TongTien.Value : 0);
                    }
                }

                ltrFooter.Text = total.ToStringWithLanguage();
                ltrBody.Text = html;
            }
            else
                ShowValidPromt();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            RunScript("ExportExcel", string.Format("'tblData','Report_ChiTieu_{0}.xls'", DateTime.Now.ToString("ddMMyyyy")));
        }
    }
}