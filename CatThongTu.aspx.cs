using Microsoft.SqlServer.Server;
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using SweetSoft.ExtraControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2
{
    public partial class ThongTu : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Cat_ThongTu;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Cat_ThongTu;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Thông tư";

                return rs;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;

            if (rightName == "Insert") rs = UserHasRight(RightPageName.Cat_ThongTu_Insert);
            else if (rightName == "Update") rs = UserHasRight(RightPageName.Cat_ThongTu_Update);
            else if (rightName == "Delete") rs = UserHasRight(RightPageName.Cat_ThongTu_Delete);

            return rs;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);
            ltrSearchTenThongTu.Text = string.Format("Tên {0}", CurrentName);

            grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);
            txtSearchTenThongTu.EnterSubmitClientID = btnSearchNow.ClientID;
            //btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                grvData.CurrentSortExpression = TblCatThongTu.Columns.Id;
                grvData.CurrentSortDerection = "Desc";
                grvData.Rebind();

                cbbFilterColumns.Items.Clear();
                grvData.FilterColumnsControlClientID = cbbFilterColumns.ClientID;
                for (int i = 0; i < grvData.Columns.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = i.ToString();
                    item.Text = grvData.Columns[i].HeaderText;
                    item.Selected = grvData.CheckColumnVisible(i);
                    if (item.Text == "Thao tác")
                        grvData.Columns[i].HeaderText = string.Empty;
                    cbbFilterColumns.Items.Add(item);
                }
            }

            cbbFilterColumns.EmptyMessage = "Chọn tất cả";
            cbbFilterColumns.MultipleSelectedTextFormat = "Có {count} cột được chọn";
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grvData.InsertItem();
        }



        protected void grvData_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvData.CancelAllCommand();
        }

        protected void grvData_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvData.Rows[e.RowIndex];
            if (row != null)
            {
                TextBox txtEditTen = row.FindControl("txtEditTen") as TextBox;
                ExtraCheckBox chkEditStatus = row.FindControl("chkEditStatus") as ExtraCheckBox;
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;
                ExtraDateTimePicker dateEditNgay = row.FindControl("dateEditNgay") as ExtraDateTimePicker;

                #region Validation
                if (string.IsNullOrEmpty(txtEditTen.Text))
                {
                    AddValidPromt(txtEditTen.ClientID, string.Format("Nhập tên {0}", CurrentName));
                    txtEditTen.Focus();
                }

                #endregion

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (grvData.IsInsert)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Insert, string.Format("Thêm mới {1} <b>{0}</b>", txtEditTen.Text, CurrentName));

                        #region Insert value for ThongTu
                        TblCatThongTu ThongTu = new TblCatThongTu();
                        ThongTu.Ten = txtEditTen.Text.Trim();
                        string path = "/images/no-image-icon-6.png";
                        if (dateEditNgay.DateValue != DateTime.MinValue)
                            ThongTu.NgayThongTu = dateEditNgay.DateValue;
                        ThongTu.TrangThaiSuDung = chkEditStatus.Checked;
                        ThongTu.NguoiTao = ThongTu.NguoiCapNhat = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.Email : "";
                        ThongTu.NgayTao = ThongTu.NgayCapNhat = DateTime.Now;

                        #endregion

                        #region Add log value
                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, string.Format("Tên {0}", CurrentName),
                                                 TblCatThongTu.Columns.Ten,
                                                 txtEditTen.Text);

                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, "Trạng thái",
                                                 TblCatThongTu.Columns.TrangThaiSuDung,
                                                 chkEditStatus.Text);
                        #endregion

                        ThongTu = CategoryManager.InsertThongTu(ThongTu);
                        ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                   string.Format("Thêm mới {1} <b>{0}</b> thành công.",
                                                 ThongTu.Ten, CurrentName),
                                   NotifyType.Success);
                        CurrentLog.SaveLog(RightPageName.Cat_LM_Insert);
                        grvData.Rebind();
                    }
                    else
                    {
                        TblCatThongTu ThongTu = CategoryManager.GetThongTuByID(hdfEditID.Value);
                        if (ThongTu != null)
                        {
                            #region Update value for ThongTu
                            ThongTu.Ten = txtEditTen.Text.Trim();
                            string path = "/images/no-image-icon-6.png";
                            if (dateEditNgay.DateValue != DateTime.MinValue)
                                ThongTu.NgayThongTu = dateEditNgay.DateValue;
                            ThongTu.TrangThaiSuDung = chkEditStatus.Checked;
                            ThongTu.NguoiCapNhat = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.Email : "";
                            ThongTu.NgayCapNhat = DateTime.Now;
                            #endregion

                            #region Add log value
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Delete,
                                                    TblCatThongTu.Columns.Ten,
                                                    txtEditTen.Text);
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Update,
                                                    TblCatThongTu.Columns.TrangThaiSuDung,
                                                    chkEditStatus.Text);
                            #endregion

                            ThongTu = CategoryManager.UpdateThongTu(ThongTu);
                            ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                       string.Format("Cập nhật {1} <b>{0}</b> thành công.",
                                                     ThongTu.Ten, CurrentName),
                                       NotifyType.Success);
                            CurrentLog.SaveLog(RightPageName.Cat_LM_Update);
                            grvData.Rebind();
                        }
                    }
                    grvData.CancelAllCommand();
                }
                if (!PageIsValid)
                    ShowValidPromt();
            }
        }

        protected void grvData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            if (btnEdit != null) btnEdit.Visible = GetButtonRole("Update");

            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null) btnDelete.Visible = GetButtonRole("Delete");

            ExtraTextBox txtEditTen = e.Row.FindControl("txtEditTen") as ExtraTextBox;
            if (txtEditTen != null)
            {
                txtEditTen.PlaceHolder = string.Format("Nhập tên {0}", CurrentName);

            }
            ExtraTextBox txtEditTenVietTat = e.Row.FindControl("txtEditTenVietTat") as ExtraTextBox;
            if (txtEditTenVietTat != null)
            {

            }
            if (grvData.EditIndex == e.Row.RowIndex && !grvData.IsInsert)
            {
                TblCatThongTu ThongTu = e.Row.DataItem as TblCatThongTu;
                if (ThongTu != null)
                {
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Update,
                                                 string.Format("Cập nhật {1} <b>{0}</b>",
                                                                ThongTu.Ten, CurrentName));


                    ExtraCheckBox chkEditStatus
                        = e.Row.FindControl("chkEditStatus") as ExtraCheckBox;
                    HiddenField hdfEditID
                        = e.Row.FindControl("hdfEditID") as HiddenField;
                    ExtraDateTimePicker dateEditNgay = e.Row.FindControl("dateEditNgay") as ExtraDateTimePicker;

                    if (txtEditTen != null)
                        txtEditTen.Text = ThongTu.Ten;
                    if (dateEditNgay != null && ThongTu.NgayThongTu.HasValue && ThongTu.NgayThongTu.Value != DateTime.MinValue)
                        dateEditNgay.DateValue = ThongTu.NgayThongTu.Value;
                    if (chkEditStatus != null)
                        chkEditStatus.Checked = ThongTu.TrangThaiSuDung.HasValue && ThongTu.TrangThaiSuDung.Value;
                    if (hdfEditID != null)
                        hdfEditID.Value = ThongTu.Id.ToString();

                    #region Add log value
                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, string.Format("Tên {0}", CurrentName),
                                             TblCatThongTu.Columns.Ten,
                                             txtEditTen.Text);

                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, "Trạng thái sử dụng",
                                             TblCatThongTu.Columns.TrangThaiSuDung,
                                             chkEditStatus.Text);
                    #endregion
                }
            }
        }

        protected void grvData_NeedDataSource(object sender, SweetSoft.ExtraControls.ExtraGridEventArg e)
        {
            ExtraGrid grv = sender as ExtraGrid;
            if (grv != null)
            {
                int rowIndex = (grv.CurrentPageIndex - 1) * grv.CurrentPageSize;
                int pageSize = grv.CurrentPageSize;

                string filterClause1 = "";
                string filterClause = string.Empty;
                string orderClause = string.Format("{0} desc", TblCatThongTu.Columns.Id);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "*";

                if (!string.IsNullOrEmpty(txtSearchTenThongTu.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblCatThongTu.Columns.Ten,
                                                  txtSearchTenThongTu.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblCatThongTu.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                int total = 0;
                List<TblCatThongTu> lst = CategoryManager.SearchThongTu(rowIndex, pageSize, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblCatThongTu());
                grv.DataSource = lst;
                List<TblCatThongTu> lstEmplty = new List<TblCatThongTu>();
                lstEmplty.Add(new TblCatThongTu());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchTenThongTu.Text = string.Empty;
            cbbSearchStatus.SelectedIndex = 0;
            grvData.Rebind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool resultDelete = false;
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmdName = btn.CommandName;
                if (cmdName == "exec_delete")
                {
                    if (CurrentMessageBoxResult.Submit && CurrentMessageBoxResult.Value != null)
                    {
                        if (CurrentMessageBoxResult.CommandName == "delete_ThongTu")
                        {
                            TblCatThongTu ThongTu = CurrentMessageBoxResult.Value as TblCatThongTu;
                            if (ThongTu != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, string.Format("Tên {0}", CurrentName),
                                                             TblCatThongTu.Columns.Ten,
                                                             ThongTu.Ten);
                                    resultDelete = CategoryManager.DeleteThongTu(ThongTu.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              ThongTu.Ten, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               ThongTu.Ten, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblCatThongTu ThongTu = CategoryManager.GetThongTuByID(btn.CommandArgument);
                    if (ThongTu != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = ThongTu;
                        result.Submit = true;
                        result.CommandName = "delete_ThongTu";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     ThongTu.Ten, CurrentName));
                    }
                }
            }
        }

        protected void btnSearchNow_Click(object sender, EventArgs e)
        {
            grvData.CurrentPageIndex = 1;
            grvData.Rebind();
            dialogSearch.CloseDialog();
        }
    }

}