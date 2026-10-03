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
using SweetSoft.ExtraControls;

namespace Pasteur.Laboratory.P2.Reports
{
    public partial class BaoCaoDoanhThu : BasePage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_DoanhThu;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_DoanhThu;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Báo cáo doanh thu";

                return rs;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                dateFrom.DateValue = DateTimeHelper.AddFirstDayOfMonth(DateTime.Now);
                dateTo.DateValue = DateTime.Now;
                LoadSource();
            }
        }

        private void LoadData()
        {
            List<TblCatLoaiMau> loaiMauCol = CategoryManager.GetLoaiMauALL(true);
            TblCatLoaiMau lall = new TblCatLoaiMau();
            lall.Id = -1;
            lall.Ten = "Tất cả";
            loaiMauCol.Insert(0, lall);
            cbLoaiMau.DataSource = loaiMauCol;
            cbLoaiMau.DataValueField = TblCatLoaiMau.Columns.Id;
            cbLoaiMau.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbLoaiMau.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            TblCatQuyChuan qall = new TblCatQuyChuan();
            qall.Id = -1;
            qall.Ten = "Tất cả";
            quyChuanCol.Insert(0, qall);
            cbQuyChuan.DataSource = quyChuanCol;
            cbQuyChuan.DataValueField = TblCatLoaiMau.Columns.Id;
            cbQuyChuan.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbQuyChuan.DataBind();
        }

        protected string GetTrangThai(object mau) {
            string rs = "";
            if (mau is TblMauXetNghiem) {
                TblMauXetNghiem m = mau as TblMauXetNghiem;
                if (string.IsNullOrEmpty(m.ThuTienNguoiThu)) rs = "Chưa in phiếu";
                else
                {
                    if (m.GhiNo.HasValue && !m.GhiNo.Value) rs = "Đã thanh toán";
                    else rs = "Ghi nợ";
                }
            }
            return rs;
          
        } 
        private void LoadSource(string LoaiHinh = "")
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
                string addition = " AND 1 = 1 ";
                if (cbLoaiMau.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDLoaiMau, cbLoaiMau.SelectedValue);
                if (cbQuyChuan.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDQuyChuan, cbQuyChuan.SelectedValue);
                if (!string.IsNullOrEmpty(cbTinhTrangThanhToan.SelectedValue))
                {
                    if (cbTinhTrangThanhToan.SelectedValue == "ChuaLapPhieu")
                        addition += string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.ThuTienNguoiThu);
                    if (cbTinhTrangThanhToan.SelectedValue == "DaLapPhieu")
                        addition += string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.ThuTienNguoiThu);
                    if (cbTinhTrangThanhToan.SelectedValue == "DTT")
                        addition += string.Format(" AND  mxn.{0} is not null AND (mxn.{1} is null or mxn.{1} = 0) ", TblMauXetNghiem.Columns.ThuTienNguoiThu, TblMauXetNghiem.Columns.GhiNo);
                    if (cbTinhTrangThanhToan.SelectedValue == "GhiNo")
                        addition += string.Format(" AND  mxn.{0} is not null AND mxn.{1} = 1 ", TblMauXetNghiem.Columns.ThuTienNguoiThu, TblMauXetNghiem.Columns.GhiNo);
                }

                if (string.IsNullOrEmpty(LoaiHinh)) LoaiHinh = "mxn.NgayNhanMau";
                else LoaiHinh = "mxn.ThuTienNgayThu";

                if (dateFrom.DateValue != DateTime.MinValue) addition += string.Format(" AND {0} >= '{1}' ", LoaiHinh, dateFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateTo.DateValue != DateTime.MinValue) addition += string.Format(" AND {0} <= '{1}' ", LoaiHinh, dateTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateVSDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateVSDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));


                List<TblMauXetNghiem> mauXetNghiemCol = XetNghiemManager.ReportMauXetNghiem(dateFrom.DateValue, dateTo.DateValue, addition, LoaiHinh);
                repData.DataSource = mauXetNghiemCol;
                repData.DataBind();

                ltrTongTien.Text = mauXetNghiemCol.Sum(p => p.TongTien.HasValue ? p.TongTien.Value : 0).ToStringWithLanguage();
            }
            else
                ShowValidPromt();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
                LoadSource(btn.CommandArgument);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            RunScript("ExportExcel", string.Format("'tblData','Report_DoanhThu_{0}.xls'", DateTime.Now.ToString("ddMMyyyy")));
        }
    }
}