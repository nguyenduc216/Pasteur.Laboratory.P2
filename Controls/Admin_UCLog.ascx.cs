using Pasteur.DataAccess;

using Pasteur.Core;
// Code: HOME-STATS-DEFER-20260918 - Cho phép trì hoãn truy vấn nhật ký đến async postback sau 5 giây.
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;

namespace Pasteur.Laboratory.P2.Controls
{
    public partial class Admin_UCLog : BaseUserControl
    {
        /// <summary>Cho phép control truy vấn nhật ký tại PreRender.</summary>
        public bool AllowLoadData { get; set; }

        public Admin_UCLog()
        {
            // Code: HOME-STATS-DEFER-20260918 - Mặc định giữ nguyên quy trình cũ nếu control được dùng ở trang khác.
            AllowLoadData = true;
        }

        #region Properties
        private int CurrentPageIndex
        {
            get
            {
                int pi = 0;
                if (this.ViewState["RptPageIndex"] != null)
                {
                    int u = 0;
                    if (int.TryParse(this.ViewState["RptPageIndex"].ToString(), out u))
                        pi = u;
                }
                return pi;
            }
            set
            {
                this.ViewState["RptPageIndex"] = value;
            }
        }

        private int TotalRow
        {
            get
            {
                int tp = 0;
                if (this.ViewState["RptTotalPage"] != null)
                {
                    int u = 0;
                    if (int.TryParse(this.ViewState["RptTotalPage"].ToString(), out u))
                        tp = u;
                }
                return tp;
            }
            set
            {
                this.ViewState["RptTotalPage"] = value;
            }
        }

        private bool IsLastPage
        {
            get
            {
                return CurrentPageIndex + 1 == TotalPage;
            }
        }

        private bool IsFirstPage
        {
            get
            {
                return CurrentPageIndex == 0;
            }
        }

        private int TotalPage
        {
            get
            {
                return TotalRow % PageSize == 0 ? TotalRow / PageSize : 
                                                  TotalRow / PageSize + 1;
            }
        }

        private int PageSize
        {

            get
            {
                int pi = 0;
                if (this.ViewState["RptPageSize"] != null)
                {
                    int u = 0;
                    if (int.TryParse(this.ViewState["RptPageSize"].ToString(), out u))
                        pi = u;
                }
                return pi;
            }
            set
            {
                this.ViewState["RptPageSize"] = value;
            }
        }

        private int CurrentPageSize { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitProperties();
            }
        }

        public void InitProperties()
        {
            CurrentPageIndex = 0;
            TotalRow = 0;
            PageSize = 10;
            CurrentPageSize = PageSize;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Code: HOME-STATS-DEFER-20260918 - Request đầu bỏ qua truy vấn; async postback bật lại sau 5 giây.
            if (!AllowLoadData) return;

            try
            {
                LoadLog();
                btnViewMore.Enabled = !IsLastPage;
                btnViewPrevious.Enabled = !IsFirstPage;
            }
            catch (Exception ex)
            {
                // Code: DASHBOARD-PARALLEL-LOAD-20260918 - Hiển thị lỗi riêng của truy vấn nhật ký.
                ltrLoadError.Text = string.Format(
                    "<div class='alert alert-danger dashboard-load-error'><i class='ace-icon fa fa-exclamation-triangle'></i> Không tải được nhật ký: {0}</div>",
                    HttpUtility.HtmlEncode(ex.Message));
            }
        }

        public void LoadLog()
        {
            int rowIndex = CurrentPageIndex * CurrentPageSize;
            string column = string.Format("*");
            string orderClause = string.Format("{0} desc", TblCommonLogging.Columns.CreatedDate);
            string filterClause = "";
          
            if (!string.IsNullOrEmpty(txtSearchLog.Text.Trim()))
                filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                TblCommonLogging.Columns.LogTitle,
                                                txtSearchLog.Text.Trim());
            filterClause = filterClause.Trim();
            if (filterClause.EndsWith("AND"))
                filterClause = filterClause.Substring(0, filterClause.Length - 3);

            int total = 0;
            var lstLog = LogManager.SearchLog(rowIndex, PageSize, column, filterClause, 
                                              orderClause, out total);
            TotalRow = total;
            rptLog.DataSource = lstLog;
            rptLog.DataBind();
        }

        protected string GetLogContent(object logContent)
        {
            if (logContent.ToString().Length > 0)
            {
                string end = logContent.ToString().Substring(4);//lay chuoi sau <ul>
                return "<ul style='list-style: none'>" + end;
            }
            return logContent.ToString();
        }

        protected void btnViewMore_Click(object sender, EventArgs e)
        {
            ++CurrentPageIndex;
            CurrentPageSize += PageSize;
        }

        protected void btnSearchLog_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearchLog.Text.Trim()))
            {
                btnCancelSearch.Visible = true;
                InitProperties();
            }
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtSearchLog.Text = string.Empty;
            btnCancelSearch.Visible = false;
            InitProperties();
        }

        protected void btnDeleteLog_Click(object sender, EventArgs e)
        {
            var btn = sender as SweetSoft.ExtraControls.ExtraButton;
            if (btn != null)
            {
                TblCommonLogging log = LogManager.GetLogByID(btn.CommandArgument);
                if (log != null)
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result.Value = log;
                    result.Submit = true;
                    result.CommandName = "delete_log";
                    CurrentMessageBoxResult = result;
                    OpenMessageBox("Xóa lịch sử",
                                   string.Format("Bạn thật sự muốn xóa lịch sử <b>{0}</b> ?",
                                                 log.LogTitle));
                }
            }
        }

        protected void btnViewPrevious_Click(object sender, EventArgs e)
        {
            --CurrentPageIndex;
            CurrentPageSize += PageSize;
        }
    }
}
