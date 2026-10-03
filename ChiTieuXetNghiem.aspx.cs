using Microsoft.SqlServer.Server;
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
    public partial class ChiTieuXetNghiem : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.ChiTieu_QuanLy;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.ChiTieu_QuanLy;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "chỉ tiêu xét nghiệm";

                return rs;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;

            if (rightName == "Insert") rs = UserHasRight(RightPageName.ChiTieu_Insert);
            else if (rightName == "Update") rs = UserHasRight(RightPageName.ChiTieu_Update);
            else if (rightName == "Delete") rs = UserHasRight(RightPageName.ChiTieu_Delete);
            else if (rightName == "Setting") rs = UserHasRight(RightPageName.ChiTieu_Setting);
            return rs;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);


            grvData.Columns[1].HeaderText = string.Format("Tên {0}", CurrentName);

            //btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                LoadControl();
                grvData.CurrentSortExpression = TblChiTieuXetNghiem.Columns.Id;
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

        protected string GetPhuongPhap(object id)
        {
            string rs = "";
            List<TblCatPhuongPhap> phuongPhapCol = CategoryManager.GetPhuongPhapByChiTieuID(id);
            if (phuongPhapCol.Count > 0)
            {
                rs = "<ul class='cl-data'>";
                foreach (TblCatPhuongPhap pp in phuongPhapCol)
                {
                    string li = string.Format("<li>+ {0}</li>", !string.IsNullOrEmpty(pp.TenTaiLieuGoc) ? pp.TenTaiLieuGoc : pp.Ten);
                    rs += li;
                }
                rs += "</ul>";
            }

            return rs;
        }

        protected string GetChungNhan(object id)
        {
            string rs = "";
            List<TblChiTieuQuyChuanChungChi> chiTieuCC = XetNghiemManager.GetChiTieuChungChiByChiTieuID(id.ToString());
            List<TblChiTieuQuyChuanChungChi> chiTieuCC2 = chiTieuCC.GroupBy(c => new
            {
                c.TenQuyChuan,
                c.TenTrongTTQD
            }).Select(gcs => new TblChiTieuQuyChuanChungChi()
            {
                TenQuyChuan = gcs.Key.TenQuyChuan,
                TenTrongTTQD = gcs.Key.TenTrongTTQD,
                Children = gcs.ToList()
            }).ToList<TblChiTieuQuyChuanChungChi>();
            for (int i = 0; i < chiTieuCC2.Count; i++)
            {

                TblChiTieuQuyChuanChungChi cc = chiTieuCC2[i];
                rs += cc.TenQuyChuan;
                if (cc.Children.Count > 0)
                {
                    rs += "<ul class='cl-data'>";
                    foreach (TblChiTieuQuyChuanChungChi pp in cc.Children)
                    {
                        string li = string.Format("<li>+ {0}</li>", !string.IsNullOrEmpty(pp.TenChungChi) ? pp.TenChungChi : "");
                        rs += li;
                    }
                    rs += "</ul>";
                }

            }
            return rs;
        }
        protected void grvData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            if (btnEdit != null)
            {

                btnEdit.Visible = GetButtonRole("Update");
                if (e.Row.DataItem != null)
                {
                    TblChiTieuXetNghiem chiTieu = e.Row.DataItem as TblChiTieuXetNghiem;
                    btnEdit.NavigateUrl = string.Format("chi-tieu-xet-nghiem/{0}", chiTieu.Id);
                    btnEdit.OnClientClick = "return false;";
                    btnEdit.Target = "_blank";
                }

            }

            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null) btnDelete.Visible = GetButtonRole("Delete");
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
                if (!string.IsNullOrEmpty(txtSearchTenTaiPhongBan.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                  txtSearchTenTaiPhongBan.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchTenThongTu.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                  txtSearchTenThongTu.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchSTT.Text.Trim()))
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.Stt,
                                                  txtSearchSTT.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                if (cbSearchQuyChuan.HasValue)
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.QuyChuanList,
                                                  cbSearchQuyChuan.DisplayText.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (cbSearchNhomXN.HasValue)
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.NhomList,
                                                  cbSearchNhomXN.DisplayText.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (cbSearchChungNhan.HasValue)
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.ChungNhanList,
                                                  cbSearchChungNhan.DisplayText.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (cbSearchPhuongPhap.HasValue)
                    filterClause += string.Format(" {0} like N'%{1}%' AND ",
                                                  TblChiTieuXetNghiem.Columns.PhuongPhapList,
                                                  cbSearchPhuongPhap.DisplayText.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (cbSearchDonViTinh.HasValue)
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                                  cbSearchDonViTinh.SelectedValue);


                string orderClause = string.Format("{0} desc", TblChiTieuXetNghiem.Columns.Id);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                string column = string.Format("chiTieu.*,dvt.{0},dvt2.{0} as TenDonViTinhTien,quyChuan.{1} as TenQuyChuan,phuongPhap.{2} as TenPhuongPhap",
                                                TblDonViTinh.Columns.TenDonVi,
                                                TblCatQuyChuan.Columns.Ten,
                                                TblCatPhuongPhap.Columns.Ten);

                if (!string.IsNullOrEmpty(cbbSearchStatus.SelectedValue))
                    filterClause += string.Format(" {0} = {1} AND ",
                                                  TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                                  cbbSearchStatus.SelectedValue);

                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                string tableJoin = string.Format(" left join {0} as dvt on dvt.Id = chiTieu.{1} ",
                                                  TblDonViTinh.Schema.TableName, TblChiTieuXetNghiem.Columns.IDDonViTinh);
                tableJoin += string.Format(" left join {0} as dvt2 on dvt2.Id = chiTieu.{1} ",
                                                  TblDonViTinh.Schema.TableName, TblChiTieuXetNghiem.Columns.IDDonViTinhTien);
                tableJoin += string.Format(" left join {0} as quyChuan on quyChuan.Id = chiTieu.{1} ",
                                                  TblCatQuyChuan.Schema.TableName, TblChiTieuXetNghiem.Columns.IDQuyTrinh);
                tableJoin += string.Format(" left join {0} as phuongPhap on phuongPhap.Id = chiTieu.{1} ",
                                                  TblCatPhuongPhap.Schema.TableName, TblChiTieuXetNghiem.Columns.IDPhuongPhap);
                int total = 0;
                List<TblChiTieuXetNghiem> lst = ChiTieuManager.SearchChiTieuXetNghiem(rowIndex, pageSize, tableJoin, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblChiTieuXetNghiem());
                grv.DataSource = lst;
                List<TblChiTieuXetNghiem> lstEmplty = new List<TblChiTieuXetNghiem>();
                lstEmplty.Add(new TblChiTieuXetNghiem());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //txtSearchTenChiTieu.Text = string.Empty;
            //cbbSearchStatus.SelectedIndex = 0;
            txtSearchSTT.Text = txtSearchTenTaiPhongBan.Text = txtSearchTenThongTu.Text = "";
            cbSearchPhuongPhap.SelectedIndex = cbSearchQuyChuan.SelectedIndex = cbSearchDonViTinh.SelectedIndex = cbSearchNhomXN.SelectedIndex = cbSearchChungNhan.SelectedIndex = 0;
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
                        if (CurrentMessageBoxResult.CommandName == "delete_ChiTieu")
                        {
                            TblChiTieuXetNghiem ChiTieu = CurrentMessageBoxResult.Value as TblChiTieuXetNghiem;
                            if (ChiTieu != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));

                                    resultDelete = ChiTieuManager.DeleteChiTieuXetNghiem(ChiTieu.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              ChiTieu.TenTaiCacKhoa, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               ChiTieu.TenTaiCacKhoa, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblChiTieuXetNghiem ChiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(btn.CommandArgument);
                    if (ChiTieu != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = ChiTieu;
                        result.Submit = true;
                        result.CommandName = "delete_ChiTieu";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     ChiTieu.TenTaiCacKhoa, CurrentName));
                    }
                }
            }
        }

        private void LoadControl()
        {
            List<TblDonViTinh> dvtCol = CategoryManager.GetDonViTinhALL(true);
            TblDonViTinh empty = new TblDonViTinh();
            empty.Id = -1;
            empty.TenDonVi = "Tất cả";
            dvtCol.Insert(0, empty);
            cbSearchDonViTinh.DataSource = dvtCol;
            cbSearchDonViTinh.DataValueField = TblDonViTinh.Columns.Id;
            cbSearchDonViTinh.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbSearchDonViTinh.DataBind();

            List<TblCatNhomXetNghiem> nhomXN = CategoryManager.GetNhomXetNghiemAll(true);
            TblCatNhomXetNghiem emptyN = new TblCatNhomXetNghiem();
            emptyN.Id = -1;
            emptyN.Ten = "Tất cả";
            nhomXN.Insert(0, emptyN);
            cbSearchNhomXN.DataSource = nhomXN;
            cbSearchNhomXN.DataValueField = TblCatNhomXetNghiem.Columns.Id;
            cbSearchNhomXN.DataTextField = TblCatNhomXetNghiem.Columns.Ten;
            cbSearchNhomXN.DataBind();

            List<TblCatChungChi> chungNhanCOl = CategoryManager.GetChungChiALL(true);
            TblCatChungChi emptyCC = new TblCatChungChi();
            emptyCC.Id = -1;
            emptyCC.Ten = "Tất cả";
            chungNhanCOl.Insert(0, emptyCC);
            cbSearchChungNhan.DataSource = chungNhanCOl;
            cbSearchChungNhan.DataValueField = TblCatChungChi.Columns.Id;
            cbSearchChungNhan.DataTextField = TblCatChungChi.Columns.Ten;
            cbSearchChungNhan.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            TblCatQuyChuan emptyQC = new TblCatQuyChuan();
            emptyQC.Id = -1;
            emptyQC.Ten = "Tất cả";
            quyChuanCol.Insert(0, emptyQC);
            cbSearchQuyChuan.DataSource = quyChuanCol;
            cbSearchQuyChuan.DataValueField = TblCatQuyChuan.Columns.Id;
            cbSearchQuyChuan.DataTextField = TblCatQuyChuan.Columns.Ten;
            cbSearchQuyChuan.DataBind();

            cbSearchPhuongPhap.Items.Clear();
            List<TblCatPhuongPhap> phuongPhapCol = CategoryManager.GetPhuongPhapAll(true);
            ListItem item1 = new ListItem();
            item1.Text = "Tất cả";
            item1.Value = "-1";
            cbSearchPhuongPhap.Items.Add(item1);
            foreach (TblCatPhuongPhap phuongPhap in phuongPhapCol)
            {
                ListItem item = new ListItem();
                item.Text = phuongPhap.Ten;
                item.Value = phuongPhap.Id.ToString();
                string subText = string.Format("<br/><span class='pp-item'></span><span style='margin-left:5px;font-size:15px;vertical-align: bottom;'> {0}</span>", phuongPhap.Ten);
                item.Attributes.Add("data-subtext", subText);
                cbSearchPhuongPhap.Items.Add(item);
            }
        }

        protected string GetList(object data)
        {
            string rs = "";
            if (data != null && !string.IsNullOrEmpty(data.ToString()))
            {
                rs = "<ul class='cl-data'>";
                string[] arr = data.ToString().Trim().Split(new string[] { ".::." }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string str in arr)
                    rs += string.Format("<li>+ {0}</li>", str);
                rs += "</ul>";
            }
            return rs;
        }

        protected void btnSearchNow_Click(object sender, EventArgs e)
        {
            grvData.CurrentPageIndex = 1;
            grvData.Rebind();
            dialogSearch.CloseDialog();
        }


        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("chi-tieu-xet-nghiem/0");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //if (sender as ExtraButton) {
            //    ExtraButton btn = sender as ExtraButton;
            //    string cmd = btn.CommandArgument;
            //    if (!string.IsNullOrEmpty(cmd)) {
            //        Response.Redirect(string.Format());
            //    }
            //}
        }
    }
}