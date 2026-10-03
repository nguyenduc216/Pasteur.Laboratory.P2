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
    public partial class DMDonViTinh : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                if (CurrentType == "QC")
                    return FunctionSystem.Code.Cat_Quy_Cach;
                return FunctionSystem.Code.Cat_Don_Vi_Tinh;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                if (CurrentType == "QC")
                    return FunctionSystem.Name.Cat_Quy_Cach;
                return FunctionSystem.Name.Cat_Don_Vi_Tinh;
            }
        }

        protected string CurrentType
        {
            get
            {
                return "DV";
            }
        }

        protected string CurrentName
        {
            get
            {
                string rs = string.Empty;
                if (CurrentType == "DV") rs = "đơn vị tính";
                else if (CurrentType == "QC") rs = "quy cách";
                return rs;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;
            if (CurrentType == "DV")
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.DVT_Insert);
                else if (rightName == "Update") rs = UserHasRight(RightPageName.DVT_Update);
                else if (rightName == "Delete") rs = UserHasRight(RightPageName.DVT_Delete);
            }
            else if (CurrentType == "QC")
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.QC_Insert);
                else if (rightName == "Update") rs = UserHasRight(RightPageName.QC_Update);
                else if (rightName == "Delete") rs = UserHasRight(RightPageName.QC_Delete);
            }
            return rs;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);
            ltrSearchTenDonVi.Text = string.Format("Tên {0}", CurrentName);
             headerTitle.Text = CurrentName;
            grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);
            txtSearchTenDonVi.EnterSubmitClientID =
            txtSearchTenVietTat.EnterSubmitClientID = btnSearchNow.ClientID;
            btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                grvData.CurrentSortExpression = TblDonViTinh.Columns.Id;
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
                ExtraMathTextbox txtEditTenDonVi = row.FindControl("txtEditTenDonVi") as ExtraMathTextbox;
                TextBox txtEditTenVietTat = row.FindControl("txtEditTenVietTat") as TextBox;
                ExtraCheckBox chkEditStatus = row.FindControl("chkEditStatus") as ExtraCheckBox;
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;

                #region Validation
                if (string.IsNullOrEmpty(txtEditTenDonVi.Text))
                {
                    AddValidPromt(txtEditTenDonVi.ClientID, string.Format("Nhập tên {0}", CurrentName));
                    txtEditTenDonVi.Focus();
                }

              

                #endregion

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (grvData.IsInsert)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.PB_Insert, string.Format("Thêm mới {1} <b>{0}</b>", txtEditTenDonVi.Text, CurrentName));

                        #region Insert value for donvitinh
                        TblDonViTinh donvitinh = new TblDonViTinh();
                        donvitinh.TenDonVi = txtEditTenDonVi.Text.Trim();
                        donvitinh.TenVietTat = txtEditTenVietTat.Text.Trim();
                        donvitinh.TrangThaiSuDung = chkEditStatus.Checked;
                        donvitinh.NguoiTao = donvitinh.NguoiCapNhap = ApplicationContext.Current.CommonUser.Email;
                        donvitinh.NgayTao = donvitinh.NgayCapNhat = DateTime.Now;
                        donvitinh.Type = CurrentType;
                        #endregion

                        #region Add log value
                        CurrentLog.AddFirstValue(RightPageName.DVT_Insert, string.Format("Tên {0}", CurrentName),
                                                 TblDonViTinh.Columns.TenDonVi,
                                                 txtEditTenDonVi.Text);
                        CurrentLog.AddFirstValue(RightPageName.DVT_Insert, "Tên viết tắt",
                                                 TblDonViTinh.Columns.TenVietTat,
                                                 txtEditTenVietTat.Text);
                        CurrentLog.AddFirstValue(RightPageName.DVT_Insert, "Trạng thái",
                                                 TblDonViTinh.Columns.TrangThaiSuDung,
                                                 chkEditStatus.Text);
                        #endregion

                        donvitinh = CategoryManager.InsertDonViTinh(donvitinh);
                        ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                   string.Format("Thêm mới {1} <b>{0}</b> thành công.",
                                                 donvitinh.TenDonVi, CurrentName),
                                   NotifyType.Success);
                        CurrentLog.SaveLog(RightPageName.DVT_Insert);
                        grvData.Rebind();
                    }
                    else
                    {
                        TblDonViTinh donvitinh = CategoryManager.GetDonViTinhByID(hdfEditID.Value);
                        if (donvitinh != null)
                        {
                            #region Update value for donvitinh
                            donvitinh.TenDonVi = txtEditTenDonVi.Text.Trim();
                            donvitinh.TenVietTat = txtEditTenVietTat.Text.Trim();
                            donvitinh.TrangThaiSuDung = chkEditStatus.Checked;
                            donvitinh.NguoiCapNhap = ApplicationContext.Current.CommonUser.Email;
                            donvitinh.NgayCapNhat = DateTime.Now;
                           
                            #endregion

                            #region Add log value
                            CurrentLog.AddLastValue(RightPageName.DVT_Update,
                                                    TblDonViTinh.Columns.TenDonVi,
                                                    txtEditTenDonVi.Text);
                            CurrentLog.AddLastValue(RightPageName.DVT_Update,
                                                    TblDonViTinh.Columns.TenVietTat,
                                                    txtEditTenVietTat.Text);
                            CurrentLog.AddLastValue(RightPageName.DVT_Update,
                                                    TblDonViTinh.Columns.TrangThaiSuDung,
                                                    chkEditStatus.Text);
                            #endregion

                            donvitinh = CategoryManager.UpdateDonViTinh(donvitinh);
                            ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                       string.Format("Cập nhật {1} <b>{0}</b> thành công.",
                                                     donvitinh.TenDonVi, CurrentName),
                                       NotifyType.Success);
                            CurrentLog.SaveLog(RightPageName.DVT_Update);
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

            if (grvData.EditIndex == e.Row.RowIndex && !grvData.IsInsert)
            {
                TblDonViTinh donvitinh = e.Row.DataItem as TblDonViTinh;
                if (donvitinh != null)
                {
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.DVT_Update,
                                                 string.Format("Cập nhật {1} <b>{0}</b>",
                                                                donvitinh.TenDonVi, CurrentName));
                    ExtraMathTextbox txtEditTenDonVi
                        = e.Row.FindControl("txtEditTenDonVi") as ExtraMathTextbox;
                    TextBox txtEditTenVietTat
                        = e.Row.FindControl("txtEditTenVietTat") as TextBox;
                    ExtraCheckBox chkEditStatus
                        = e.Row.FindControl("chkEditStatus") as ExtraCheckBox;
                    HiddenField hdfEditID
                        = e.Row.FindControl("hdfEditID") as HiddenField;

                    if (txtEditTenDonVi != null)
                        txtEditTenDonVi.Text = donvitinh.TenDonVi;
                    if (txtEditTenVietTat != null)
                        txtEditTenVietTat.Text = donvitinh.TenVietTat;
                    if (chkEditStatus != null)
                        chkEditStatus.Checked = donvitinh.TrangThaiSuDung.HasValue && donvitinh.TrangThaiSuDung.Value;
                    if (hdfEditID != null)
                        hdfEditID.Value = donvitinh.Id.ToString();

                    #region Add log value
                    CurrentLog.AddFirstValue(RightPageName.DVT_Update, string.Format("Tên {0}", CurrentName),
                                             TblDonViTinh.Columns.TenDonVi,
                                             txtEditTenDonVi.Text);
                    CurrentLog.AddFirstValue(RightPageName.DVT_Update, "Tên viết tắt",
                                             TblDonViTinh.Columns.TenVietTat,
                                             txtEditTenVietTat.Text);
                    CurrentLog.AddFirstValue(RightPageName.DVT_Update, "Trạng thái sử dụng",
                                             TblDonViTinh.Columns.TrangThaiSuDung,
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
                string orderClause = string.Format("{0} desc", TblDonViTinh.Columns.Id);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "*";

                if (!string.IsNullOrEmpty(txtSearchTenDonVi.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblDonViTinh.Columns.TenDonVi,
                                                  txtSearchTenDonVi.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchTenVietTat.Text.Trim()))
                    filterClause += string.Format(" {0} like '%{1}%' AND ",
                                                  TblDonViTinh.Columns.TenVietTat,
                                                  txtSearchTenVietTat.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblDonViTinh.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);
                filterClause = string.Format(" {0}.{1} = '{2}' AND ({3}) ",
                                              TblDonViTinh.Schema.TableName, TblDonViTinh.Columns.Type,
                                              CurrentType,
                                              !string.IsNullOrEmpty(filterClause) ? filterClause : " 1=1 ");
                int total = 0;
                List<TblDonViTinh> lst = CategoryManager.SearchDonViTinh(rowIndex, pageSize, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblDonViTinh());
                grv.DataSource = lst;
                List<TblDonViTinh> lstEmplty = new List<TblDonViTinh>();
                lstEmplty.Add(new TblDonViTinh());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchTenDonVi.Text = string.Empty;
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
                        if (CurrentMessageBoxResult.CommandName == "delete_donvitinh")
                        {
                            TblDonViTinh donvitinh = CurrentMessageBoxResult.Value as TblDonViTinh;
                            if (donvitinh != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.DVT_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.DVT_Delete, string.Format("Tên {0}", CurrentName),
                                                             TblDonViTinh.Columns.TenDonVi,
                                                             donvitinh.TenDonVi);
                                    resultDelete = CategoryManager.DeleteDonViTinh(donvitinh.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              donvitinh.TenDonVi, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.DVT_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               donvitinh.TenDonVi, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblDonViTinh donvitinh = CategoryManager.GetDonViTinhByID(btn.CommandArgument);
                    if (donvitinh != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = donvitinh;
                        result.Submit = true;
                        result.CommandName = "delete_donvitinh";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     donvitinh.TenDonVi, CurrentName));
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