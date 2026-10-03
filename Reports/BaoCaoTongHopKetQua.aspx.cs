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
using System.Data;

namespace Pasteur.Laboratory.P2.Reports
{
    public partial class BaoCaoTongHopKetQua : BasePage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_TongHop;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_TongHop;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Báo cáo tổng hợp kết quả";
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
                DataTable table = XetNghiemManager.GetColumns();
                DataView view = table.DefaultView;
                view.Sort = "Index asc";
                DataTable result = view.ToTable();
                foreach (DataRow row in result.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = row["Value"].ToString();
                    item.Text = row["Name"].ToString();
                    cbThongTinHienThi.Items.Add(item);
                    item.Selected = row["Selected"].ToString() == "1";
                }

            }
        }

        protected void chkCheckAllThongTin_CheckedChanged(object sender, EventArgs e)
        {
            ltrThongTinHienThi.Text = "Thông tin hiển thị";
            if (chkCheckAllThongTin.Checked) ltrThongTinHienThi.Text = "Thông tin hiển thị (tất cả)";
            foreach (ListItem item in cbThongTinHienThi.Items)
                item.Selected = chkCheckAllThongTin.Checked;
        }

        private void LoadDSXetNghiem()
        {
            cbDanhSachXetNghiem.Items.Clear();
            if (dateFrom.DateValue != DateTime.MinValue && dateTo.DateValue != DateTime.MinValue)
            {
                List<TblChiTieuXetNghiem> chiTieuCol = XetNghiemManager.GetChiTieuXetNghiem(dateFrom.DateValue, dateTo.DateValue, txtMamMau.Text, cbTraKQ.SelectedValue);
                foreach (TblChiTieuXetNghiem chiTieu in chiTieuCol)
                {
                    ListItem item = new ListItem();
                    item.Text = chiTieu.TenTaiCacKhoa;
                    item.Value = chiTieu.Code;
                    string subText = string.Format("<span>Đơn vị tính: {0}</span>", chiTieu.TenDonVi);
                    item.Attributes.Add("data-subtext", subText);
                    cbDanhSachXetNghiem.Items.Add(item);
                }
                //cbDanhSachXetNghiem.DataSource = chiTieuCol;
                //cbDanhSachXetNghiem.DataValueField = "Code";
                //cbDanhSachXetNghiem.DataValueField = "TenTaiCacKhoa";
                //cbDanhSachXetNghiem.DataBind();
            }
            else ShowNotify("Thông báo", "Vui lòng chọn ngày bắt đầu và kết thúc", SweetSoft.ExtraControls.NotifyType.Error);
        }

        protected void lnkGetDSXetNghiem_Click(object sender, EventArgs e)
        {
            LoadDSXetNghiem();
        }

        protected void chkCheckAllXetNghiem_CheckedChanged(object sender, EventArgs e)
        {
            ltrXetNghiem.Text = "Danh sách xét nghiệm";
            if (chkCheckAllXetNghiem.Checked) ltrThongTinHienThi.Text = "Danh sách xét nghiệm (tất cả)";
            foreach (ListItem item in cbDanhSachXetNghiem.Items)
                item.Selected = chkCheckAllXetNghiem.Checked;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            List<TblDonViTinh> dvtCol = CategoryManager.GetDonViTinhALL(null);

            string html = "<table id='tblData' style='width: 100%; border-collapse: collapse;'>";
            html += "<thead>";
            #region Columns
            string trHeader1 = "<tr><th rowspan='2' style='width:30px;text-align: center; font-weight: bold; border: solid 1px #000000; padding: 5px;'>STT</th>";
            foreach (ListItem item in cbThongTinHienThi.SelectedItems)
            {
                trHeader1 += string.Format("<th rowspan='2' style='max-width:150px;text-align: center; font-weight: bold; border: solid 1px #000000; padding: 5px'>{0}</th>", item.Text);
            }
            string trHeader2 = "";
            int dx = 0;
            foreach (ListItem item in cbDanhSachXetNghiem.SelectedItems)
            {
                dx++;
                string[] id = item.Value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
                string dvt = "";
                if (id.Length > 1)
                {

                    if (!string.IsNullOrEmpty(id[1]))
                    {
                        TblDonViTinh dvtT = dvtCol.FirstOrDefault(p => p.Id.ToString() == id[1].Trim());
                        if (dvtT != null) dvt = string.Format(" ({0})", dvtT.TenDonVi);
                    }
                }

                trHeader2 += string.Format("<th style='max-width:150px;text-align: center; font-weight: bold; border: solid 1px #000000; padding: 5px'>{0}{1}</th>", item.Text, dvt);
            }
            if (!string.IsNullOrEmpty(trHeader2)) trHeader2 = "<tr>" + trHeader2 + "</tr>";

            trHeader1 += string.Format("<th{0} style='font-weight: bold; border: solid 1px #000000; padding: 5px;'>Kết quả kiệm nghiệm</th>", dx > 1 ? string.Format(" colspan='{0}'", dx) : "");
            trHeader1 += "<th rowspan='2' style='width:30px;text-align: center; font-weight: bold; border: solid 1px #000000; padding: 5px;'>Nhận xét</th></tr>";
            html += trHeader1;
            html += trHeader2;
            #endregion

            html += "</thead><tbody>";
            #region Body
            string addition = "";
            if (dateFrom.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
            if (dateTo.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));
            if (dateDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
            if (dateDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));
            if (dateVSDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
            if (dateVSDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));


            DataTable mxnCol = XetNghiemManager.GetMauXetNghiemBaoCaoTongHop(dateFrom.DateValue, dateTo.DateValue, txtMamMau.Text, cbTraKQ.SelectedValue, addition);
            List<TblMauXetNghiem> mxnCol2 = mxnCol.ToList<TblMauXetNghiem>();
            List<TblMauXetNghiemChiTieu> mxnCTCol = XetNghiemManager.GetMauXetNghiemChiTietBaoCaoTongHop(dateFrom.DateValue, dateTo.DateValue, txtMamMau.Text, cbTraKQ.SelectedValue,addition);
            int vt = 1;
            foreach (DataRow _row in mxnCol.Rows)
            {
                string _ID = _row["ID"].ToString();
                TblMauXetNghiem mauXN = mxnCol2.FirstOrDefault(p => p.Id.ToString() == _ID);
                string trLine = "<tr>";
                trLine += "<td style='text-align: center;  border: solid 1px #000000; padding: 5px;'>" + vt++ + "</td>";
                foreach (ListItem item in cbThongTinHienThi.SelectedItems)
                {
                    string value = "";
                    if (mxnCol.Columns.Contains(item.Value))
                        value = GetDataObject(_row[item.Value]);
                    trLine += string.Format("<td style=' border: solid 1px #000000; padding: 5px;'>{0}</td>", value);
                }
                if (cbDanhSachXetNghiem.SelectedItems.Count > 0)
                {
                    foreach (ListItem item2 in cbDanhSachXetNghiem.SelectedItems)
                    {
                        string valueX = "";
                        string ctid = item2.Value.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        TblMauXetNghiemChiTieu chiTieu = mxnCTCol.FirstOrDefault(p => p.IDMau.HasValue && p.IDMau.Value.ToString() == _ID && p.IDChiTieu.HasValue && p.IDChiTieu.Value.ToString() == ctid);
                        if (chiTieu != null)
                        {
                            valueX = chiTieu.KetQuaHienThiToString;
                            //if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                            //    valueX = chiTieu.KetQuaHienThiToString;
                            //else valueX = chiTieu.KetQuaHienThi;
                        }

                        trLine += string.Format("<td style='text-align:center; border: solid 1px #000000; padding: 5px;'>{0}</td>", valueX);
                    }
                }
                else trLine += string.Format("<td style='text-align:center; border: solid 1px #000000; padding: 5px;'>{0}</td>", "");

                trLine += string.Format("<td style='text-align:center; border: solid 1px #000000; padding: 5px;'>{0}</td>", mauXN.KetQuaDanhGiaMau == "1" ? "đạt" : "không đạt");
                trLine += "</tr>";
                html += trLine;
            }
            #endregion
            html += "</tobdy></table>";
            ltrData.Text = html;
        }

        private string GetDataObject(object data)
        {
            string rs = "";
            if (data != null)
            {
                rs = data.ToString();
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(data.ToString(), out dt) && dt != DateTime.MinValue) rs = dt.ToString("dd/MM/yyyy");
            }
            return rs;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            RunScript("ExportExcel", string.Format("'tblData','Bao_cao_tong_hop_ket_qau_{0}.xls'", DateTime.Now.ToString("ddMMyyyy")));
        }
    }
}