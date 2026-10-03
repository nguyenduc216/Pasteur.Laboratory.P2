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
    public partial class ReportThanhLy : BasePage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_MauThanhLy;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_MauThanhLy;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Thống kê mẫu thanh lý";

                return rs;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserHasRight(RightPageName.Hoa_TNMSH_Update))
                    cbHoaSinh.SelectedValue = "Hoa";
                else
                    cbHoaSinh.SelectedValue = "VS";
                dateFrom.DateValue = DateTimeHelper.AddFirstDayOfMonth(DateTime.Now);
                dateTo.DateValue = DateTime.Now;
                LoadData();
            }
        }

        private void LoadData()
        {
            if (dateTo.DateValue == DateTime.MinValue)
            {
                AddValidPromt(dateTo.ClientID, "Chọn ngày tới");
                dateTo.Focus();
            }

            if (dateFrom.DateValue == DateTime.MinValue)
            {
                AddValidPromt(dateFrom.ClientID, "Chọn ngày bắt đầu");
                dateFrom.Focus();
            }

            if (PageIsValid)
            {
                string addition = string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.NgayThanhLyDuKien);
                if (cbHoaSinh.SelectedValue == "Hoa")
                    addition = string.Format(" AND mxn.{0} > 0 ", TblMauXetNghiem.Columns.LuongMauHoa);
                else
                    addition = string.Format(" AND (mxn.{0} - mxn.{1}) > 0 ", TblMauXetNghiem.Columns.LuongMau, TblMauXetNghiem.Columns.LuongMauHoa);

                if (!string.IsNullOrEmpty(txtMTenChiTieu.Value))
                    addition = string.Format(" AND mxn.{0} like N'%{1}%' ", TblMauXetNghiem.Columns.MaMau, txtMTenChiTieu.MValue);

                if (dateFrom.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateTo.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));

                if (!string.IsNullOrEmpty(cbTinhTrang.SelectedValue))
                {
                    if (cbTinhTrang.SelectedValue == "1")
                    {
                        if (cbHoaSinh.SelectedValue == "Hoa")
                            addition = string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.NgayThanhLyThucTeHoa);
                        else
                            addition = string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.NgayThanhLyThucTeVS);
                    }

                    else
                    {
                        if (cbHoaSinh.SelectedValue == "Hoa")
                            addition = string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.NgayThanhLyThucTeHoa);
                        else
                            addition = string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.NgayThanhLyThucTeVS);
                    }
                }

                List<TblMauXetNghiem> chiTieuCol = XetNghiemManager.ReportMauXetNghiem(dateFrom.DateValue, dateTo.DateValue, addition, string.Format("mxn.{0}", TblMauXetNghiem.Columns.NgayTraKetQuaThucTe));

                repData.DataSource = chiTieuCol;
                repData.DataBind();
            }
            else
                ShowValidPromt();
        }

        protected string GetNgayThanhLyThucTe(object data)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiem)
            {
                TblMauXetNghiem mxn = data as TblMauXetNghiem;
                if (cbHoaSinh.SelectedValue == "Hoa" && mxn.NgayThanhLyThucTeHoa.HasValue && mxn.NgayThanhLyThucTeHoa.Value != DateTime.MinValue)
                    rs = mxn.NgayThanhLyThucTeHoa.Value.ToString("dd/MM/yyyy HH:mm");
                if (cbHoaSinh.SelectedValue == "VS" && mxn.NgayThanhLyThucTeVS.HasValue && mxn.NgayThanhLyThucTeVS.Value != DateTime.MinValue)
                    rs = mxn.NgayThanhLyThucTeVS.Value.ToString("dd/MM/yyyy HH:mm");
            }
            return rs;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            RunScript("ExportExcel", string.Format("'tblData','Report_ThanhLy_{0}.xls'", DateTime.Now.ToString("ddMMyyyy")));

        }
    }
}