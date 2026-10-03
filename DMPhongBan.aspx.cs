using SweetSoft.ExtraControls;

using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;

namespace Pasteur.Laboratory.P2
{
    public partial class DMPhongBan : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                if (CurrentType == "kho")
                    return FunctionSystem.Code.Kho_List;
                return FunctionSystem.Code.Cat_Phong_Ban;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                if (CurrentType == "kho")
                    return FunctionSystem.Name.Kho_List;
                return FunctionSystem.Name.Cat_Phong_Ban;
            }
        }

        protected string CurrentType
        {
            get
            {
                return "phongban";
            }
        }

        protected string CurrentName
        {
            get
            {
                string rs = string.Empty;
                if (CurrentType == "phongban") rs = "phòng ban";
                else if (CurrentType == "kho") rs = "kho";
                return rs;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;
            if (CurrentType == "phongban")
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.PB_Insert);
                else if (rightName == "Update") rs = UserHasRight(RightPageName.PB_Update);
                else if (rightName == "Delete") rs = UserHasRight(RightPageName.PB_Delete);
            }
            else if (CurrentType == "kho")
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.Kho_Insert);
                else if (rightName == "Update") rs = UserHasRight(RightPageName.Kho_Update);
                else if (rightName == "Delete") rs = UserHasRight(RightPageName.Kho_Delete);
            }
            return rs;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);
            ltrSearchTenPhongBan.Text = string.Format("Tên {0}", CurrentName);
            headerTitle.Text = CurrentName;
            grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);
            txtSearchTenPhongBan.EnterSubmitClientID = txtSearchTenVietTat.EnterSubmitClientID = btnSearchNow.ClientID;
            btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                grvData.CurrentSortExpression = TblPhongBan.Columns.Id;
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
                TextBox txtEditTenVietTat = row.FindControl("txtEditTenVietTat") as TextBox;
                ExtraCheckBox chkEditStatus = row.FindControl("chkEditStatus") as ExtraCheckBox;
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;

                #region Validation
                if (string.IsNullOrEmpty(txtEditTen.Text))
                {
                    AddValidPromt(txtEditTen.ClientID, string.Format("Nhập tên {0}", CurrentName));
                    txtEditTen.Focus();
                }

                if (string.IsNullOrEmpty(txtEditTenVietTat.Text.Trim()))
                {
                    AddValidPromt(txtEditTenVietTat.ClientID, "Nhập tên viết tắt");
                    txtEditTenVietTat.Focus();
                }

                #endregion

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (grvData.IsInsert)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.PB_Insert, string.Format("Thêm mới {1} <b>{0}</b>", txtEditTen.Text, CurrentName));

                        #region Insert value for phongban
                        TblPhongBan phongban = new TblPhongBan();
                        phongban.TenPhongBan = txtEditTen.Text.Trim();
                        phongban.TenVietTat = txtEditTenVietTat.Text.Trim();
                        phongban.TrangThaiSuDung = chkEditStatus.Checked;
                        phongban.NguoiTao = phongban.NguoiCapNhat = ApplicationContext.Current.CommonUser.Email;
                        phongban.NgayTao = phongban.NgayCapNhat = DateTime.Now;
                        phongban.IsWarehourse = CurrentType == "kho";
                        #endregion

                        #region Add log value
                        CurrentLog.AddFirstValue(RightPageName.PB_Insert, string.Format("Tên {0}", CurrentName),
                                                 TblPhongBan.Columns.TenPhongBan,
                                                 txtEditTen.Text);
                        CurrentLog.AddFirstValue(RightPageName.PB_Insert, "Tên viết tắt",
                                                 TblPhongBan.Columns.TenVietTat,
                                                 txtEditTenVietTat.Text);
                        CurrentLog.AddFirstValue(RightPageName.PB_Insert, "Trạng thái",
                                                 TblPhongBan.Columns.TrangThaiSuDung,
                                                 chkEditStatus.Text);
                        #endregion

                        phongban = CategoryManager.InsertPhongBan(phongban);
                        ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                   string.Format("Thêm mới {1} <b>{0}</b> thành công.",
                                                 phongban.TenPhongBan, CurrentName),
                                   NotifyType.Success);
                        CurrentLog.SaveLog(RightPageName.PB_Insert);
                        grvData.Rebind();
                    }
                    else
                    {
                        TblPhongBan phongban = CategoryManager.GetPhongBanByID(hdfEditID.Value);
                        if (phongban != null)
                        {
                            #region Update value for phongban
                            phongban.TenPhongBan = txtEditTen.Text.Trim();
                            phongban.TenVietTat = txtEditTenVietTat.Text.Trim();
                            phongban.TrangThaiSuDung = chkEditStatus.Checked;
                            phongban.NguoiCapNhat = ApplicationContext.Current.CommonUser.Email;
                            phongban.NgayCapNhat = DateTime.Now;
                            #endregion

                            #region Add log value
                            CurrentLog.AddLastValue(RightPageName.PB_Update,
                                                    TblPhongBan.Columns.TenPhongBan,
                                                    txtEditTen.Text);
                            CurrentLog.AddLastValue(RightPageName.PB_Update,
                                                    TblPhongBan.Columns.TenVietTat,
                                                    txtEditTenVietTat.Text);
                            CurrentLog.AddLastValue(RightPageName.PB_Update,
                                                    TblPhongBan.Columns.TrangThaiSuDung,
                                                    chkEditStatus.Text);
                            #endregion

                            phongban = CategoryManager.UpdatePhongBan(phongban);
                            ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                       string.Format("Cập nhật {1} <b>{0}</b> thành công.",
                                                     phongban.TenPhongBan, CurrentName),
                                       NotifyType.Success);
                            CurrentLog.SaveLog(RightPageName.PB_Update);
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
                TblPhongBan phongban = e.Row.DataItem as TblPhongBan;
                if (phongban != null)
                {
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.PB_Update,
                                                 string.Format("Cập nhật {1} <b>{0}</b>",
                                                                phongban.TenPhongBan, CurrentName));


                    ExtraCheckBox chkEditStatus
                        = e.Row.FindControl("chkEditStatus") as ExtraCheckBox;
                    HiddenField hdfEditID
                        = e.Row.FindControl("hdfEditID") as HiddenField;

                    if (txtEditTen != null)
                        txtEditTen.Text = phongban.TenPhongBan;
                    if (txtEditTenVietTat != null)
                        txtEditTenVietTat.Text = phongban.TenVietTat;
                    if (chkEditStatus != null)
                        chkEditStatus.Checked = phongban.TrangThaiSuDung.HasValue && phongban.TrangThaiSuDung.Value;
                    if (hdfEditID != null)
                        hdfEditID.Value = phongban.Id.ToString();

                    #region Add log value
                    CurrentLog.AddFirstValue(RightPageName.PB_Update, string.Format("Tên {0}", CurrentName),
                                             TblPhongBan.Columns.TenPhongBan,
                                             txtEditTen.Text);
                    CurrentLog.AddFirstValue(RightPageName.PB_Update, "Tên viết tắt",
                                             TblPhongBan.Columns.TenVietTat,
                                             txtEditTenVietTat.Text);
                    CurrentLog.AddFirstValue(RightPageName.PB_Update, "Trạng thái sử dụng",
                                             TblPhongBan.Columns.TrangThaiSuDung,
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
                string orderClause = string.Format("{0} desc", TblPhongBan.Columns.Id);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "*";

                if (!string.IsNullOrEmpty(txtSearchTenPhongBan.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblPhongBan.Columns.TenPhongBan,
                                                  txtSearchTenPhongBan.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchTenVietTat.Text.Trim()))
                    filterClause += string.Format(" {0} like '%{1}%' AND ",
                                                  TblPhongBan.Columns.TenVietTat,
                                                  txtSearchTenVietTat.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblPhongBan.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);
                filterClause = string.Format(" {0}.{1} = {2} AND ({3}) ",
                                              TblPhongBan.Schema.TableName, TblPhongBan.Columns.IsWarehourse,
                                              CurrentType == "kho" ? "1" : "0",
                                              !string.IsNullOrEmpty(filterClause) ? filterClause : " 1=1 ");
                int total = 0;
                List<TblPhongBan> lst = CategoryManager.SearchPhongBan(rowIndex, pageSize, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblPhongBan());
                grv.DataSource = lst;
                List<TblPhongBan> lstEmplty = new List<TblPhongBan>();
                lstEmplty.Add(new TblPhongBan());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchTenPhongBan.Text = string.Empty;
            txtSearchTenVietTat.Text = string.Empty;
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
                        if (CurrentMessageBoxResult.CommandName == "delete_phongban")
                        {
                            TblPhongBan phongban = CurrentMessageBoxResult.Value as TblPhongBan;
                            if (phongban != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.PB_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.PB_Delete, string.Format("Tên {0}", CurrentName),
                                                             TblPhongBan.Columns.TenPhongBan,
                                                             phongban.TenPhongBan);
                                    resultDelete = CategoryManager.DeletePhongBan(phongban.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              phongban.TenPhongBan, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.PB_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               phongban.TenPhongBan, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblPhongBan phongban = CategoryManager.GetPhongBanByID(btn.CommandArgument);
                    if (phongban != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = phongban;
                        result.Submit = true;
                        result.CommandName = "delete_phongban";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     phongban.TenPhongBan, CurrentName));
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