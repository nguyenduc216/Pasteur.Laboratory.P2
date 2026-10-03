using SweetSoft.ExtraControls;
using Pasteur.Laboratory.P2.Common;
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Pasteur.Laboratory.P2
{
    public partial class QLVaiTro : BaseGridPage
    {
        #region Properties
        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.User_Role;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.User_Role;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                grvRole.CurrentSortExpression = TblHangHoa.Columns.Id;
                grvRole.CurrentSortDerection = "Desc";
                grvRole.Rebind();

                cbbFilterColumns.Items.Clear();
                grvRole.FilterColumnsControlClientID = cbbFilterColumns.ClientID;
                for (int i = 0; i < grvRole.Columns.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = i.ToString();
                    item.Text = grvRole.Columns[i].HeaderText;
                    item.Selected = grvRole.CheckColumnVisible(i);
                    if (item.Text == "Thao tác")
                        grvRole.Columns[i].HeaderText = string.Empty;
                    cbbFilterColumns.Items.Add(item);
                }
            }

            cbbFilterColumns.EmptyMessage = "Chọn tất cả";
            cbbFilterColumns.MultipleSelectedTextFormat = "Có {count} cột được chọn";
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            grvRole.InsertItem();
        }

        protected void grvRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvRole.CancelAllCommand();
        }

        protected void grvRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvRole.Rows[e.RowIndex];
            if (row != null)
            {
                TextBox txtEditTitle
                    = row.FindControl("txtEditTitle") as TextBox;
                TextBox txtEditDescription
                    = row.FindControl("txtEditDescription") as TextBox;
                ExtraCheckBox chkEditStatus
                    = row.FindControl("chkEditStatus") as ExtraCheckBox;
                HiddenField hdfEditID
                    = row.FindControl("hdfEditID") as HiddenField;
               

                #region Validation
                if (string.IsNullOrEmpty(txtEditTitle.Text))
                {
                    AddValidPromt(txtEditTitle.ClientID, "Nhập tên vai trò");
                    txtEditTitle.Focus();
                }
                if (txtEditTitle.Text.Length > 250)
                {
                    AddValidPromt(txtEditTitle.ClientID, "Tên vai trò vượt quá độ dài cho phép");
                    txtEditTitle.Focus();
                }
                #endregion

                e.Cancel = !PageIsValid;

                if (PageIsValid)
                {
                    if (grvRole.IsInsert)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.Role_Insert,
                                                     string.Format("Thêm vai trò <b>{0}</b>",
                                                                    txtEditTitle.Text));
                        TblVaiTro role = new TblVaiTro();
                        role.TenVaiTro = txtEditTitle.Text;
                        role.NguoiTao = ApplicationContext.Current.CommonUser.Email;
                        role.NgayTao = DateTime.Now;
                        role.MieuTa = txtEditDescription.Text;
                        
                        role.TrangThaiSuDung = chkEditStatus.Checked;

                        #region Add log value
                        CurrentLog.AddFirstValue(RightPageName.Role_Insert, "Tên vai trò",
                                                 TblVaiTro.Columns.TenVaiTro,
                                                 txtEditTitle.Text);
                        CurrentLog.AddFirstValue(RightPageName.Role_Insert, "Mô tả",
                                                 TblVaiTro.Columns.MieuTa,
                                                 txtEditDescription.Text);
                        CurrentLog.AddFirstValue(RightPageName.Role_Insert, "Trạng thái",
                                                 TblVaiTro.Columns.TrangThaiSuDung,
                                                 chkEditStatus.Text);
                        #endregion

                        role = RoleManager.InsertRole(role);
                        ShowNotify("Lưu thông tin vai trò thành công",
                                    string.Format("Bạn vừa thêm mới vai trò thành công."),
                                    NotifyType.Success);
                        CurrentLog.SaveLog(RightPageName.Role_Insert);
                        grvRole.Rebind();
                    }
                    else
                    {
                        TblVaiTro role = RoleManager.GetRoleByID(hdfEditID.Value);
                        if (role != null)
                        {
                            role.TenVaiTro = txtEditTitle.Text;
                            role.TrangThaiSuDung = chkEditStatus.Checked;
                            role.NguoiCapNhatCuoi = ApplicationContext.Current.CommonUser.Email;
                            role.NgayCapNhatCuoi = DateTime.Now;
                            role.MieuTa = txtEditDescription.Text;

                            #region Add log value
                            CurrentLog.AddLastValue(RightPageName.Role_Update,
                                                    TblVaiTro.Columns.TenVaiTro,
                                                    role.TenVaiTro);
                            CurrentLog.AddLastValue(RightPageName.Role_Update,
                                                    TblVaiTro.Columns.MieuTa,
                                                    role.MieuTa);
                            CurrentLog.AddLastValue(RightPageName.Role_Update,
                                                    TblVaiTro.Columns.TrangThaiSuDung,
                                                    role.TrangThaiSuDung.Value ? "Kích hoạt" : "Khóa");
                            #endregion

                            role = RoleManager.UpdateRole(role);
                            ShowNotify("Lưu thông tin vai trò thành công",
                                        string.Format("Bạn vừa cập nhật vai trò <b>{0}</b> thành công.",role.TenVaiTro),
                                        NotifyType.Success);
                            CurrentLog.SaveLog(RightPageName.Role_Update);
                            grvRole.Rebind();
                        }
                    }
                    grvRole.CancelAllCommand();
                }

                if (!PageIsValid)
                    ShowValidPromt();
            }
        }

        protected void grvRole_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            if (btnEdit != null) btnEdit.Visible = UserHasRight(RightPageName.Role_Update);

            ExtraButton btnAuthorization = e.Row.FindControl("btnAuthorization") as ExtraButton;
            if (btnAuthorization != null) btnAuthorization.Visible = UserHasRight(RightPageName.Role_Setting);

            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null) btnDelete.Visible = UserHasRight(RightPageName.Role_Delete);

            if (grvRole.EditIndex == e.Row.RowIndex && !grvRole.IsInsert)
            {
                TblVaiTro role = e.Row.DataItem as TblVaiTro;
                if (role != null)
                {
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.Role_Update,
                                                 string.Format("Cập nhật vai trò <b>{0}</b>",
                                                 role.TenVaiTro));
                    TextBox txtEditTitle
                        = e.Row.FindControl("txtEditTitle") as TextBox;
                    TextBox txtEditDescription
                       = e.Row.FindControl("txtEditDescription") as TextBox;
                    ExtraCheckBox chkEditStatus
                        = e.Row.FindControl("chkEditStatus") as ExtraCheckBox;
                    HiddenField hdfEditID
                        = e.Row.FindControl("hdfEditID") as HiddenField;
                 

                    
                    if (txtEditDescription != null)
                        txtEditDescription.Text = role.MieuTa;
                    if (txtEditTitle != null)
                        txtEditTitle.Text = role.TenVaiTro;
                    if (chkEditStatus != null)
                        chkEditStatus.Checked = role.TrangThaiSuDung.HasValue && role.TrangThaiSuDung.Value;
                    if (hdfEditID != null)
                        hdfEditID.Value = role.Id.ToString();

                    #region Add log value
                    CurrentLog.AddFirstValue(RightPageName.Role_Update, "Tên vai trò",
                                             TblVaiTro.Columns.TenVaiTro,
                                             txtEditTitle.Text);
                    CurrentLog.AddFirstValue(RightPageName.Role_Update, "Miêu tả",
                                             TblVaiTro.Columns.MieuTa,
                                             txtEditTitle.Text);
                   
                    CurrentLog.AddFirstValue(RightPageName.Role_Update, "Trạng thái",
                                             TblVaiTro.Columns.TrangThaiSuDung,
                                             chkEditStatus.Text);
                    #endregion
                }
            }
        }

        protected void grvRole_NeedDataSource(object sender, ExtraGridEventArg e)
        {
            ExtraGrid grv = sender as ExtraGrid;
            if (grv != null)
            {
                int rowIndex = (grv.CurrentPageIndex - 1) * grv.CurrentPageSize;
                int pageSize = grv.CurrentPageSize;

                string filterClause1 = "";
                string filterClause = string.Empty;
                string orderClause = string.Format("ro.{0} desc ", TblVaiTro.Columns.TenVaiTro);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "*";

                if (!string.IsNullOrEmpty(txtSearchTitle.Text.Trim()))
                    filterClause += string.Format(" ro.{0} like N'%{1}%' AND ",
                                                  TblVaiTro.Columns.TenVaiTro,
                                                  txtSearchTitle.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchDescription.Text.Trim()))
                    filterClause += string.Format(" ro.{0} like N'%{1}%' AND ",
                                                  TblVaiTro.Columns.MieuTa,
                                                  txtSearchDescription.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" ro.{0} = {1} ",
                                                  TblVaiTro.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);
                
                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                int total = 0;
                List<TblVaiTro> lst = RoleManager.SearchRole(rowIndex, pageSize, column,
                                                                 filterClause, orderClause, out total);
                if (grvRole.IsInsert)
                    lst.Insert(0, new TblVaiTro());
                grv.DataSource = lst;
                List<TblVaiTro> lstEmpty = new List<TblVaiTro>();
                lstEmpty.Add(new TblVaiTro());
                grv.DataSourceEmpty = lstEmpty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchTitle.Text = string.Empty;
            cbbSearchStatus.SelectedIndex = 0;
            
            grvRole.Rebind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmdName = btn.CommandName;
                if (cmdName == "exec_delete")
                {
                    if (CurrentMessageBoxResult.Submit && CurrentMessageBoxResult.Value != null)
                    {
                        if (CurrentMessageBoxResult.CommandName == "delete_role")
                        {
                            TblVaiTro role = CurrentMessageBoxResult.Value as TblVaiTro;
                            if (role != null)
                            {
                                CurrentLog.AddNewLogFunction(RightPageName.Role_Delete,
                                                             "Xóa vai trò");
                                CurrentLog.AddFirstValue(RightPageName.Role_Delete, "Tên vai trò",
                                                         TblVaiTro.Columns.TenVaiTro,
                                                         role.TenVaiTro);
                                RoleManager.DeleteRole(role.Id);
                                grvRole.Rebind();
                                CloseMessageBox();
                                ShowNotify("Xóa dữ liệu thành công",
                                            string.Format("Bạn vừa xóa vai trò <b>{0}</b>", role.TenVaiTro),
                                            NotifyType.Success);
                                CurrentLog.SaveLog(RightPageName.Role_Delete);
                            }
                        }
                    }
                }
                else
                {
                    string roleId = btn.CommandArgument;
                    TblVaiTro role = RoleManager.GetRoleByID(roleId);

                    if (role != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = role;
                        result.Submit = true;
                        result.CommandName = "delete_role";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox("Xóa vai trò",
                                        string.Format("Bạn thật sự muốn xóa vai trò <b>{0}</b> ?",
                                                       role.TenVaiTro));
                    }
                }
            }
        }

        protected void btnSearchNow_Click(object sender, EventArgs e)
        {
            
            grvRole.CurrentPageIndex = 1;
            grvRole.Rebind();
            dialogSearch.CloseDialog();
        }

        protected void btnAuthorization_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                TblVaiTro role = RoleManager.GetRoleByID(btn.CommandArgument);
                if (role != null)
                {
                    lblFunctionTitle.Text = role.TenVaiTro;
                    ucRightInRole.LoadRightFunction(role);
                    dialogFunction.ShowDialog(true);
                    RunScript("BuildTableRole", string.Empty);
                }
            }
        }

        protected void btnRightForRole_Click(object sender, EventArgs e)
        {
            bool save = ucRightInRole.Save();
            dialogFunction.CloseDialog();
            if (save)
                ShowNotify("Phân quyền thành công",
                            string.Format("Bạn đã cập nhật quyền cho vai trò <b>{0}</b>",
                                           lblFunctionTitle.Text),
                            NotifyType.Success);
            else
                ShowNotify("Phân quyền thất bại",
                            string.Format("Cập nhật quyền thất bại"),
                            NotifyType.Error);
        }

    }
}