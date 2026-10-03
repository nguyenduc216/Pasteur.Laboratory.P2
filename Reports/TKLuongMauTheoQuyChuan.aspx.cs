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
    public partial class TKLuongMauTheoQuyChuan : BasePage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_ChiTieu_LMQC;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_ChiTieu_LMQC;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Thống kê theo loại mẫu và quy chuẩn";

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
        private void LoadSource()
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
                if (!string.IsNullOrEmpty(cbHoaSinh.SelectedValue))
                {
                    if (cbHoaSinh.SelectedValue == "Hoa")
                        addition = " and mxn.LuongMauHoa > 0 ";
                    else
                        addition = " and (mxn.LuongMau - mxn.LuongMauHoa) > 0 ";
                }
                if (cbKQDG.SelectedValue == "1")
                    addition += string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau);
                else if (cbKQDG.SelectedValue == "0")
                    addition += string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau);
                if (cbLoaiMau.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDLoaiMau, cbLoaiMau.SelectedValue);
                if (cbQuyChuan.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDQuyChuan, cbQuyChuan.SelectedValue);
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


                List<TblMauXetNghiem> mauXetNghiemCol = XetNghiemManager.ReportMauXetNghiem(dateFrom.DateValue, dateTo.DateValue, addition);

                List<TblMauXetNghiem> mauXetNghiemResult = mauXetNghiemCol.GroupBy(c => new
                {
                    c.TenLoaiMau,
                    c.TenQuyChuan
                })
                .Select(gcs => new TblMauXetNghiem()
                {
                    TenLoaiMau = gcs.Key.TenLoaiMau,
                    TenQuyChuan = gcs.Key.TenQuyChuan,
                    Children = gcs.ToList()
                }).ToList<TblMauXetNghiem>();

                repData.DataSource = mauXetNghiemResult;
                repData.DataBind();
            }
            else
                ShowValidPromt();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ltrTongMau.Text = ltrTongMauDanhGia.Text = ltrTongTien.Text = "0";
            LoadSource();
        }

        int tongMau = 0, tongKhongDat = 0, tongDanhGia = 0, tongKhongDanhGia = 0;
        protected string TinhTong(object data, string code)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiem)
            {
                TblMauXetNghiem mauXN = data as TblMauXetNghiem;
                if (code == "tm")
                {
                    rs = mauXN.Children.Count.ToString("N0");
                    tongMau += mauXN.Children.Count;
                    ltrTongMau.Text = tongMau.ToString("N0");
                }
                else if (code == "1")
                {
                    List<TblMauXetNghiem> mau = mauXN.Children.FindAll(p => !string.IsNullOrEmpty(p.DanhGiaDatMau) && p.DanhGiaDatMau == "1"); //p.DanhGiaMau.HasValue && p.DanhGiaMau.Value && 
                    rs = mau.Count.ToString("N0");
                    tongDanhGia += mau.Count;
                    ltrTongMauDanhGia.Text = tongDanhGia.ToString("N0");
                }
                else if (code == "2")
                {
                    List<TblMauXetNghiem> mau = mauXN.Children.FindAll(p => !string.IsNullOrEmpty(p.DanhGiaDatMau) && p.DanhGiaDatMau == "0");
                    rs = mau.Count.ToString("N0");
                    tongKhongDat += mau.Count;
                    ltrTongTien.Text = tongKhongDat.ToString("N0");
                }
                else if (code == "3")
                {
                    //List<TblMauXetNghiem> mau = mauXN.Children.FindAll(p => !p.DanhGiaMau.HasValue || (p.DanhGiaMau.HasValue && !p.DanhGiaMau.Value));
                    List<TblMauXetNghiem> mau = mauXN.Children.FindAll(p => !string.IsNullOrEmpty(p.DanhGiaDatMau) && p.DanhGiaDatMau == "2");
                    rs = mau.Count.ToString("N0");
                    tongKhongDanhGia += mau.Count;
                    ltrTongKhongDanhGia.Text = tongKhongDanhGia.ToString("N0");
                }
            }
            return rs;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            RunScript("ExportExcel", string.Format("'tblData','Report_LuongMauTheoQuyChuan_{0}.xls'", DateTime.Now.ToString("ddMMyyyy")));
        }
    }
}