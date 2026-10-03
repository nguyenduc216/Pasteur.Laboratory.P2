using Pasteur.Core.Helper;
using Pasteur.Core;
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
    public partial class NhomXetNghiem : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Cat_NhomXN;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Cat_NhomXN;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Nhóm xét nghiệm";

                return rs;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;

            if (rightName == "Insert") rs = UserHasRight(RightPageName.Cat_NhomXN_Insert);
            else if (rightName == "Update") rs = UserHasRight(RightPageName.Cat_NhomXN_Update);
            else if (rightName == "Delete") rs = UserHasRight(RightPageName.Cat_NhomXN_Delete);

            return rs;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);
            ltrSearchTenNhomXetNghiem.Text = string.Format("Tên {0}", CurrentName);

            grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);
            txtSearchTenNhomXetNghiem.EnterSubmitClientID = btnSearchNow.ClientID;
            //btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                grvData.CurrentSortExpression = TblCatNhomXetNghiem.Columns.Id;
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
                TextBox txtEditDonGia = row.FindControl("txtEditDonGia") as TextBox;
                ExtraCheckBox chkEditStatus = row.FindControl("chkEditStatus") as ExtraCheckBox;
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;

                #region Validation
                if (string.IsNullOrEmpty(txtEditTen.Text))
                {
                    AddValidPromt(txtEditTen.ClientID, string.Format("Nhập tên {0}", CurrentName));
                    txtEditTen.Focus();
                }

                decimal donGia = 0;
                if (string.IsNullOrEmpty(txtEditDonGia.Text.Trim()))
                {
                    AddValidPromt(txtEditDonGia.ClientID, string.Format("Nhập đơn giá", CurrentName));
                    txtEditDonGia.Focus();
                }
                else
                {
                    if (decimal.TryParse(txtEditDonGia.Text.Trim().Replace(".", "").Replace(",", "."), out donGia) && donGia <= 0)
                    {
                        AddValidPromt(txtEditDonGia.ClientID, string.Format("Đơn giá phải lớn hơn 0 đ.", CurrentName));
                        txtEditDonGia.Focus();
                    }
                }
                #endregion

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (grvData.IsInsert)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Insert, string.Format("Thêm mới {1} <b>{0}</b>", txtEditTen.Text, CurrentName));

                        #region Insert value for NhomXetNghiem
                        TblCatNhomXetNghiem NhomXetNghiem = new TblCatNhomXetNghiem();
                        NhomXetNghiem.Ten = txtEditTen.Text.Trim();
                        NhomXetNghiem.DonGia = donGia;
                        NhomXetNghiem.TrangThaiSuDung = chkEditStatus.Checked;
                        NhomXetNghiem.NguoiTao = NhomXetNghiem.NguoiCapNhat = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.Email : "";
                        NhomXetNghiem.NgayTao = NhomXetNghiem.NgayCapNhat = DateTime.Now;

                        #endregion

                        #region Add log value
                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, string.Format("Tên {0}", CurrentName),
                                                 TblCatNhomXetNghiem.Columns.Ten,
                                                 txtEditTen.Text);
                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, "Đơn giá",
                                                TblCatNhomXetNghiem.Columns.DonGia,
                                                donGia.ToStringWithLanguage());
                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, "Trạng thái",
                                                 TblCatNhomXetNghiem.Columns.TrangThaiSuDung,
                                                 chkEditStatus.Text);
                        #endregion

                        NhomXetNghiem = CategoryManager.InsertNhomXetNghiem(NhomXetNghiem);
                        ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                   string.Format("Thêm mới {1} <b>{0}</b> thành công.",
                                                 NhomXetNghiem.Ten, CurrentName),
                                   NotifyType.Success);
                        CurrentLog.SaveLog(RightPageName.Cat_LM_Insert);
                        grvData.Rebind();
                    }
                    else
                    {
                        TblCatNhomXetNghiem NhomXetNghiem = CategoryManager.GetNhomXetNghiemByID(hdfEditID.Value);
                        if (NhomXetNghiem != null)
                        {
                            #region Update value for NhomXetNghiem
                            NhomXetNghiem.Ten = txtEditTen.Text.Trim();
                            NhomXetNghiem.DonGia = donGia;
                            NhomXetNghiem.TrangThaiSuDung = chkEditStatus.Checked;
                            NhomXetNghiem.NguoiCapNhat = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.Email : "";
                            NhomXetNghiem.NgayCapNhat = DateTime.Now;
                            #endregion

                            #region Add log value
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Delete,
                                                    TblCatNhomXetNghiem.Columns.Ten,
                                                    txtEditTen.Text);
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Delete,
                                                    TblCatNhomXetNghiem.Columns.DonGia,
                                                    donGia.ToStringWithLanguage());
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Update,
                                                    TblCatNhomXetNghiem.Columns.TrangThaiSuDung,
                                                    chkEditStatus.Text);
                            #endregion

                            NhomXetNghiem = CategoryManager.UpdateNhomXetNghiem(NhomXetNghiem);
                            ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                       string.Format("Cập nhật {1} <b>{0}</b> thành công.",
                                                     NhomXetNghiem.Ten, CurrentName),
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

            ExtraTextBox txtEditDonGia = e.Row.FindControl("txtEditDonGia") as ExtraTextBox;


            if (grvData.EditIndex == e.Row.RowIndex && !grvData.IsInsert)
            {
                TblCatNhomXetNghiem NhomXetNghiem = e.Row.DataItem as TblCatNhomXetNghiem;
                if (NhomXetNghiem != null)
                {
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Update,
                                                 string.Format("Cập nhật {1} <b>{0}</b>",
                                                                NhomXetNghiem.Ten, CurrentName));


                    ExtraCheckBox chkEditStatus
                        = e.Row.FindControl("chkEditStatus") as ExtraCheckBox;
                    HiddenField hdfEditID
                        = e.Row.FindControl("hdfEditID") as HiddenField;

                    if (txtEditTen != null)
                        txtEditTen.Text = NhomXetNghiem.Ten;
                    if (txtEditDonGia != null && NhomXetNghiem.DonGia.HasValue)
                        txtEditDonGia.Text = NhomXetNghiem.DonGia.Value.ToStringWithLanguage();

                    if (chkEditStatus != null)
                        chkEditStatus.Checked = NhomXetNghiem.TrangThaiSuDung.HasValue && NhomXetNghiem.TrangThaiSuDung.Value;
                    if (hdfEditID != null)
                        hdfEditID.Value = NhomXetNghiem.Id.ToString();

                    #region Add log value
                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, string.Format("Tên {0}", CurrentName),
                                             TblCatNhomXetNghiem.Columns.Ten,
                                             txtEditTen.Text);
                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, "Đơn giá",
                                            TblCatNhomXetNghiem.Columns.DonGia,
                                            NhomXetNghiem.DonGia.HasValue ? NhomXetNghiem.DonGia.Value.ToStringWithLanguage() : "0");

                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, "Trạng thái sử dụng",
                                             TblCatNhomXetNghiem.Columns.TrangThaiSuDung,
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
                string orderClause = string.Format("{0} desc", TblCatNhomXetNghiem.Columns.Id);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "*";

                if (!string.IsNullOrEmpty(txtSearchTenNhomXetNghiem.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblCatNhomXetNghiem.Columns.Ten,
                                                  txtSearchTenNhomXetNghiem.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblCatNhomXetNghiem.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                int total = 0;
                List<TblCatNhomXetNghiem> lst = CategoryManager.SearchNhomXetNghiem(rowIndex, pageSize, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblCatNhomXetNghiem());
                grv.DataSource = lst;
                List<TblCatNhomXetNghiem> lstEmplty = new List<TblCatNhomXetNghiem>();
                lstEmplty.Add(new TblCatNhomXetNghiem());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchTenNhomXetNghiem.Text = string.Empty;
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
                        if (CurrentMessageBoxResult.CommandName == "delete_NhomXetNghiem")
                        {
                            TblCatNhomXetNghiem NhomXetNghiem = CurrentMessageBoxResult.Value as TblCatNhomXetNghiem;
                            if (NhomXetNghiem != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, string.Format("Tên {0}", CurrentName),
                                                             TblCatNhomXetNghiem.Columns.Ten,
                                                             NhomXetNghiem.Ten);
                                    resultDelete = CategoryManager.DeleteNhomXetNghiem(NhomXetNghiem.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              NhomXetNghiem.Ten, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               NhomXetNghiem.Ten, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblCatNhomXetNghiem NhomXetNghiem = CategoryManager.GetNhomXetNghiemByID(btn.CommandArgument);
                    if (NhomXetNghiem != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = NhomXetNghiem;
                        result.Submit = true;
                        result.CommandName = "delete_NhomXetNghiem";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     NhomXetNghiem.Ten, CurrentName));
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