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
    public partial class CatPhuongPhap : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Cat_PhuongPhap;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Cat_PhuongPhap;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Phương pháp xét nghiệm";

                return rs;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;

            if (rightName == "Insert") rs = UserHasRight(RightPageName.Cat_PhuongPhap_Insert);
            else if (rightName == "Update") rs = UserHasRight(RightPageName.Cat_PhuongPhap_Update);
            else if (rightName == "Delete") rs = UserHasRight(RightPageName.Cat_PhuongPhap_Delete);

            return rs;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);
            ltrSearchTenPhuongPhap.Text = string.Format("Tên {0}", CurrentName);

            grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);
            txtSearchTenPhuongPhap.EnterSubmitClientID = btnSearchNow.ClientID;
            //btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                grvData.CurrentSortExpression = TblCatPhuongPhap.Columns.Id;
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

                ExtraMathTextbox txtMEditTen = row.FindControl("txtMEditTen") as ExtraMathTextbox;
                ExtraMathTextbox txtMEditTenTaiLieuGoc = row.FindControl("txtMEditTenTaiLieuGoc") as ExtraMathTextbox;
                ExtraCheckBox chkEditStatus = row.FindControl("chkEditStatus") as ExtraCheckBox;
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;

                #region Validation
                string ten1 = txtMEditTen.Text;
                string goc = txtMEditTenTaiLieuGoc.Text;
                if (string.IsNullOrEmpty(ten1) && string.IsNullOrEmpty(goc))
                {
                    if (string.IsNullOrEmpty(ten1))
                    {
                        AddValidPromt(txtMEditTen.ClientID, string.Format("Nhập ký hiệu SOP hoặc tên tài liệu gốc", CurrentName));
                        txtMEditTen.Focus();
                    }
                }

                #endregion

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (grvData.IsInsert)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Insert, string.Format("Thêm mới {1} <b>{0}</b>", txtMEditTen.Text, CurrentName));

                        #region Insert value for PhuongPhap
                        TblCatPhuongPhap PhuongPhap = new TblCatPhuongPhap();
                        string ten = txtMEditTen.Text;
                        //TblCatPhuongPhap.CreateName(txtEditTen.Text.Trim());

                        PhuongPhap.Ten = ten;
                        PhuongPhap.TenPhuongPhapSearch = txtMEditTen.MValue;
                        PhuongPhap.TenTaiLieuGoc = txtMEditTenTaiLieuGoc.Text;
                        PhuongPhap.TenTaiLieuGocSearch = txtMEditTenTaiLieuGoc.MValue;
                        PhuongPhap.TrangThaiSuDung = chkEditStatus.Checked;
                        PhuongPhap.NguoiTao = PhuongPhap.NguoiCapNhat = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.Email : "";
                        PhuongPhap.NgayTao = PhuongPhap.NgayCapNhat = DateTime.Now;

                        #endregion

                        #region Add log value
                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, string.Format("Tên {0}", CurrentName),
                                                 TblCatPhuongPhap.Columns.Ten,
                                                 txtMEditTen.Text);

                        CurrentLog.AddFirstValue(RightPageName.Cat_LM_Insert, "Trạng thái",
                                                 TblCatPhuongPhap.Columns.TrangThaiSuDung,
                                                 chkEditStatus.Text);
                        #endregion

                        PhuongPhap = CategoryManager.InsertPhuongPhap(PhuongPhap);
                        ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                   string.Format("Thêm mới {1} <b>{0}</b> thành công.",
                                                 PhuongPhap.Ten, CurrentName),
                                   NotifyType.Success);
                        CurrentLog.SaveLog(RightPageName.Cat_LM_Insert);
                        grvData.Rebind();
                    }
                    else
                    {
                        TblCatPhuongPhap PhuongPhap = CategoryManager.GetPhuongPhapByID(hdfEditID.Value);
                        if (PhuongPhap != null)
                        {
                            #region Update value for PhuongPhap
                            string ten = txtMEditTen.Text;

                            PhuongPhap.Ten = ten;
                            PhuongPhap.TenPhuongPhapSearch = txtMEditTen.MValue;
                            PhuongPhap.TenTaiLieuGoc = txtMEditTenTaiLieuGoc.Text;
                            PhuongPhap.TenTaiLieuGocSearch = txtMEditTenTaiLieuGoc.MValue;

                            PhuongPhap.TrangThaiSuDung = chkEditStatus.Checked;
                            PhuongPhap.NguoiCapNhat = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.Email : "";
                            PhuongPhap.NgayCapNhat = DateTime.Now;
                            #endregion

                            #region Add log value
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Delete,
                                                    TblCatPhuongPhap.Columns.Ten,
                                                    txtMEditTen.Text);
                            CurrentLog.AddLastValue(RightPageName.Cat_LM_Update,
                                                    TblCatPhuongPhap.Columns.TrangThaiSuDung,
                                                    chkEditStatus.Text);
                            #endregion

                            PhuongPhap = CategoryManager.UpdatePhuongPhap(PhuongPhap);
                            ShowNotify(string.Format("Lưu {0} thành công!", CurrentName),
                                       string.Format("Cập nhật {1} <b>{0}</b> thành công.",
                                                     PhuongPhap.Ten, CurrentName),
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

            ExtraMathTextbox txtMEditTen = e.Row.FindControl("txtMEditTen") as ExtraMathTextbox;
            ExtraMathTextbox txtMEditTenTaiLieuGoc = e.Row.FindControl("txtMEditTenTaiLieuGoc") as ExtraMathTextbox;

            if (grvData.EditIndex == e.Row.RowIndex && !grvData.IsInsert)
            {
                TblCatPhuongPhap PhuongPhap = e.Row.DataItem as TblCatPhuongPhap;
                if (PhuongPhap != null)
                {
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Update,
                                                 string.Format("Cập nhật {1} <b>{0}</b>",
                                                                PhuongPhap.Ten, CurrentName));


                    ExtraCheckBox chkEditStatus
                        = e.Row.FindControl("chkEditStatus") as ExtraCheckBox;
                    HiddenField hdfEditID
                        = e.Row.FindControl("hdfEditID") as HiddenField;

                    if (txtMEditTen != null)
                    {

                        txtMEditTen.Text = PhuongPhap.Ten;
                    }
                    if (txtMEditTenTaiLieuGoc != null)
                    {

                        txtMEditTenTaiLieuGoc.Text = PhuongPhap.TenTaiLieuGoc;
                    }


                    if (chkEditStatus != null)
                        chkEditStatus.Checked = PhuongPhap.TrangThaiSuDung.HasValue && PhuongPhap.TrangThaiSuDung.Value;
                    if (hdfEditID != null)
                        hdfEditID.Value = PhuongPhap.Id.ToString();

                    #region Add log value
                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, string.Format("Ký hiệu SOP", CurrentName),
                                             TblCatPhuongPhap.Columns.Ten,
                                             txtMEditTen.Text);

                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, string.Format("Tài liệu gốc", CurrentName),
                                             TblCatPhuongPhap.Columns.TenTaiLieuGoc,
                                             txtMEditTenTaiLieuGoc.Text);

                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Update, "Trạng thái sử dụng",
                                             TblCatPhuongPhap.Columns.TrangThaiSuDung,
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
                string orderClause = string.Format("{0} desc", TblCatPhuongPhap.Columns.Id);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "*";

                if (!string.IsNullOrEmpty(txtSearchTenPhuongPhap.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblCatPhuongPhap.Columns.Ten,
                                                  txtSearchTenPhuongPhap.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblCatPhuongPhap.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                int total = 0;
                List<TblCatPhuongPhap> lst = CategoryManager.SearchPhuongPhap(rowIndex, pageSize, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblCatPhuongPhap());
                grv.DataSource = lst;
                List<TblCatPhuongPhap> lstEmplty = new List<TblCatPhuongPhap>();
                lstEmplty.Add(new TblCatPhuongPhap());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchTenPhuongPhap.Text = string.Empty;
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
                        if (CurrentMessageBoxResult.CommandName == "delete_PhuongPhap")
                        {
                            TblCatPhuongPhap PhuongPhap = CurrentMessageBoxResult.Value as TblCatPhuongPhap;
                            if (PhuongPhap != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, string.Format("Tên {0}", CurrentName),
                                                             TblCatPhuongPhap.Columns.Ten,
                                                             PhuongPhap.Ten);
                                    resultDelete = CategoryManager.DeletePhuongPhap(PhuongPhap.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              PhuongPhap.Ten, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               PhuongPhap.Ten, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblCatPhuongPhap PhuongPhap = CategoryManager.GetPhuongPhapByID(btn.CommandArgument);
                    if (PhuongPhap != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = PhuongPhap;
                        result.Submit = true;
                        result.CommandName = "delete_PhuongPhap";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     PhuongPhap.Ten, CurrentName));
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