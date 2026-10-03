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
    public partial class ThanhLy : BaseGridPage
    {

        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                if (CurrentPageType == "LaboHoaLy") return FunctionSystem.Code.ThanhLy_Hoa;
                return FunctionSystem.Code.ThanhLy_ViSinh;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                if (CurrentPageType == "LaboHoaLy") return FunctionSystem.Name.ThanhLy_Hoa;
                return FunctionSystem.Name.ThanhLy_ViSinh;

            }
        }

        protected string CurrentPageType
        {
            get { return CommonHelper.QueryString("PageType"); }
        }
        protected string CurrentName
        {
            get
            {
                return "thanh lý";
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;
            if (CurrentPageType == "LaboHoaLy")
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.TLHL_Insert);

                else if (rightName == "Delete") rs = UserHasRight(RightPageName.TLHL_Delete);
            }
            else
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.TLVS_Insert);

                else if (rightName == "Delete") rs = UserHasRight(RightPageName.TLVS_Delete);
            }


            return rs;
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);
            thanhLy.mUpdateE += ThanhLy_mUpdateE;
            //ltrSearchTenTL.Text = string.Format("Tên {0}", CurrentName);

            // grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);
            //txtSearchTenTL.EnterSubmitClientID = btnSearchNow.ClientID;
            //btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                grvData.CurrentSortExpression = TblThanhLy.Columns.Id;
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

        private void ThanhLy_mUpdateE(object sender, EventArgs e)
        {
            grvData.Rebind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            thanhLy.RefreshDetail(CurrentPageType);
            dialogDetail.ShowDialog();
        }

        protected void grvData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            //if (btnEdit != null) btnEdit.Visible = GetButtonRole("Update");

            //ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            //if (btnDelete != null) btnDelete.Visible = GetButtonRole("Delete");

        }

        protected void grvData_NeedDataSource(object sender, SweetSoft.ExtraControls.ExtraGridEventArg e)
        {
            ExtraGrid grv = sender as ExtraGrid;
            if (grv != null)
            {
                int rowIndex = (grv.CurrentPageIndex - 1) * grv.CurrentPageSize;
                int pageSize = grv.CurrentPageSize;

                string filterClause1 = string.Format(" tl.{0} = '{1}' AND  ", TblThanhLy.Columns.LoaiThanhLy, CurrentPageType);
                string filterClause = string.Empty;
                string orderClause = string.Format("tl.{0} desc", TblThanhLy.Columns.NgayThucHien);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format(" tl.{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = "tl.*";

                if (!string.IsNullOrEmpty(txtSearchMamau.Text.Trim()))
                    filterClause += string.Format(" tl.{0} like N'%{1}%' AND ",
                                                  TblThanhLy.Columns.MaMauList,
                                                  txtSearchMamau.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                //if (!string.IsNullOrEmpty(cbSearchTinhTrang.SelectedValue))
                //    filterClause += string.Format(" tlct.{0} = N'{1}' AND ",
                //                                  TblThanhLyChiTiet.Columns.TinhTrangMauTruocHuy,
                //                                  cbSearchTinhTrang.SelectedValue);

                if (dateSearchThucHienFrom.DateValue != DateTime.MinValue)
                    filterClause += string.Format(" tl.{0} >= '{1}' AND ",
                                                  TblThanhLy.Columns.NgayThucHien,
                                                  dateSearchThucHienFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateSearchThucHienTo.DateValue != DateTime.MinValue)
                    filterClause += string.Format(" tl.{0} <= '{1}' AND ",
                                                  TblThanhLy.Columns.NgayThucHien,
                                                  dateSearchThucHienTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateSearchKetThucFrom.DateValue != DateTime.MinValue)
                    filterClause += string.Format(" tl.{0} >= '{1}' AND ",
                                                  TblThanhLy.Columns.ThoiGianKetThuc,
                                                  dateSearchKetThucFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateSearchKetThucTo.DateValue != DateTime.MinValue)
                    filterClause += string.Format(" tl.{0} <= '{1}' AND ",
                                                  TblThanhLy.Columns.ThoiGianKetThuc,
                                                  dateSearchKetThucTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                int total = 0;
                List<TblThanhLy> lst = CategoryManager.SearchThanhLy(rowIndex, pageSize, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblThanhLy());
                grv.DataSource = lst;
                List<TblThanhLy> lstEmplty = new List<TblThanhLy>();
                lstEmplty.Add(new TblThanhLy());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool resultDelete = false;
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmdName = btn.CommandName;
                if (CurrentMessageBoxResult.CommandName == "delete_TLCT")
                    thanhLy.Delete();
                else if (cmdName == "exec_delete")
                {
                    if (CurrentMessageBoxResult.Submit && CurrentMessageBoxResult.Value != null)
                    {
                        if (CurrentMessageBoxResult.CommandName == "delete_TL")
                        {
                            TblThanhLy TL = CurrentMessageBoxResult.Value as TblThanhLy;
                            if (TL != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, "Ngày thanh lý",
                                                             TblThanhLy.Columns.NgayThucHien, TL.NgayThucHienToString);
                                    #region Xử lý các phiếu con
                                    List<TblThanhLyChiTiet> chiTiet = CategoryManager.GetThanhLyChiTietByThanhLyID(TL.Id);
                                    if (chiTiet.Count > 0)
                                    {
                                        foreach (TblThanhLyChiTiet ct in chiTiet)
                                        {
                                            TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(ct.IDMauXN);
                                            if (mauXN != null)
                                                XetNghiemManager.UpdateThongTinThanhLy(ct.IDMauXN, TL.LoaiThanhLy);
                                        }
                                    }
                                    #endregion
                                    resultDelete = CategoryManager.DeleteThanhLy(TL.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} ngày <b>{0}</b>",
                                                              TL.NgayThucHienToString, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} ngày <b>{0}</b>",
                                                               TL.NgayThucHienToString, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }

                else
                {
                    TblThanhLy TL = CategoryManager.GetThanhLyByID(btn.CommandArgument);
                    if (TL != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = TL;
                        result.Submit = true;
                        result.CommandName = "delete_TL";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} ngày <b>{0}</b> ?",
                                                     TL.NgayThucHienToString, CurrentName));
                    }
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //txtSearchTenChungChi.Text = string.Empty;
            //cbbSearchStatus.SelectedIndex = 0;
            grvData.Rebind();
        }

        protected void btnSearchNow_Click(object sender, EventArgs e)
        {
            grvData.CurrentPageIndex = 1;
            grvData.Rebind();
            dialogSearch.CloseDialog();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmdA = btn.CommandArgument;
                thanhLy.EditThanhLy(CurrentPageType, cmdA);
                dialogDetail.ShowDialog();
            }
        }
    }

}