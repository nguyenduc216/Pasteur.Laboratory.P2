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
    public partial class QLNguoiDung : BaseGridPage
    {
        #region Properties
        public override string FUNCTION_CODE
        {
            get
            {

                return FunctionSystem.Code.Cat_Nhan_Vien;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {

                return FunctionSystem.Name.Cat_Nhan_Vien;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            txtSearchEmail.EnterSubmitClientID = txtSearchHoVaTen.EnterSubmitClientID = btnSearchNow.ClientID;
            btnAddNew.Visible = UserHasRight(RightPageName.User_Admin_Insert);
            if (!IsPostBack)
            {
                LoadPhongBan(cbEditPhongBan);
                LoadPhongBan(cbSearchPhongBan);


                grvUser.CurrentSortExpression = string.Format(" nv.Id ");
                grvUser.CurrentSortDerection = "Desc";
                grvUser.Rebind();

                cbbFilterColumns.Items.Clear();
                grvUser.FilterColumnsControlClientID = cbbFilterColumns.ClientID;
                for (int i = 0; i < grvUser.Columns.Count; i++)
                {
                    ListItem item = new ListItem();
                    item.Value = i.ToString();
                    item.Text = grvUser.Columns[i].HeaderText;
                    item.Selected = grvUser.CheckColumnVisible(i);
                    if (item.Text == "Thao tác")
                        grvUser.Columns[i].HeaderText = string.Empty;
                    cbbFilterColumns.Items.Add(item);
                }
            }

            cbbFilterColumns.EmptyMessage = "Chọn tất cả";
            cbbFilterColumns.MultipleSelectedTextFormat = "Có {count} cột được chọn";
        }

        private void RefreshDetail()
        {
            cbEditToNhanVien.Items.Clear();
            txtDetailHo.Text = txtDetailTen.Text = txtEditEmail.Text = string.Empty;
            dateEditNgaySinh.DateValue = DateTime.MinValue;
            cbEditGioiTinh.SelectedIndex = 0;
            txtEditEmail.Enabled = false;
            if (cbEditPhongBan.Items.Count > 0) cbEditPhongBan.SelectedIndex = 0;
            chkEditCoTaiKhoanNguoiDung.Checked = false;
            chkDetailTrangThaiSuDung.Checked = true;
            dialogDetail.UpdateContentDialog();
        }

        private void LoadPhongBan(ExtraComboBox cb)
        {
            List<TblPhongBan> phongBanCol = CategoryManager.GetPhongBanAll(true);
            if (cb.ClientID == cbSearchPhongBan.ClientID)
            {
                TblPhongBan empty = new TblPhongBan();
                empty.Id = -1;
                empty.TenPhongBan = "Tất cả";
                phongBanCol.Insert(0, empty);
            }
            cb.DataSource = phongBanCol;
            cb.DataValueField = TblPhongBan.Columns.Id;
            cb.DataTextField = TblPhongBan.Columns.TenPhongBan;
            cb.DataBind();
        }

        private void LoadToNhanVien(object phongBanId, ExtraComboBox cb)
        {
            cbEditToNhanVien.Items.Clear();
            List<TblToNhanVien> toNhanVien = CategoryManager.GetToNhanVienAll(true, phongBanId);
            if (cb.ClientID == cbSearchToNhanVien.ClientID)
            {
                TblToNhanVien empty = new TblToNhanVien();
                empty.Id = -1;
                empty.TenTo = "Tất cả";
                toNhanVien.Insert(0, empty);
            }
            cb.DataSource = toNhanVien;
            cb.DataValueField = TblToNhanVien.Columns.Id;
            cb.DataTextField = TblToNhanVien.Columns.TenTo;
            cb.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            RefreshDetail();
            hdfDetailUserID.Value = string.Empty;
            txtDetailHo.Focus();
            btnSave.Text = "Thêm mới";
            lblDialogDetailTitle.Text = "Thêm mới nhân viên";
            dialogDetail.ShowDialog(true);
        }

        protected void grvUser_NeedDataSource(object sender, ExtraGridEventArg e)
        {
            ExtraGrid grv = sender as ExtraGrid;
            if (grv != null)
            {
                int rowIndex = (grv.CurrentPageIndex - 1) * grv.CurrentPageSize;
                int pageSize = grv.CurrentPageSize;

                int ogr = -1;

                string filterClause1 = "";
                string filterClause = string.Empty;
                string filterClauseExtend = string.Empty;
                string orderClause = string.Format(" nv.{0} desc ", TblTaiKhoanNguoiDung.Columns.IDTaiKhoan);
                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    orderClause = string.Format("{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                #region set value for column
                string column = string.Format(" nv.*, nd.Email, pb.{0}, nd.IDTaiKhoan as IDNguoiDung  ", TblPhongBan.Columns.TenPhongBan);

                #endregion

                #region set value for filter clause
                if (!string.IsNullOrEmpty(txtSearchEmail.Text.Trim()))
                    filterClause += string.Format(" nd.{0} like N'%{1}%' AND ",
                                                  TblTaiKhoanNguoiDung.Columns.Email,
                                                  txtSearchEmail.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchHoVaTen.Text.Trim()))
                    filterClause += string.Format(" (nv.{0} like N'%{1}%' or nv.{2} like N'%{1}%' or concat(nv.{2},' ',nv.{0}) like N'%{1}%' or concat(nv.{2},'',nv.{0}) like N'%{1}%' ) AND ",
                                                  TblNhanVien.Columns.Ten,
                                                  txtSearchHoVaTen.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"),
                                                  TblNhanVien.Columns.Ho);
                if (cbSearchGioiTinh.HasValue)
                    filterClause += string.Format(" nv.{0} = N'{1}' AND ",
                                                  TblNhanVien.Columns.GioiTinh,
                                                  cbSearchGioiTinh.SelectedValue);
                if (cbSearchChucVu.HasValue)
                    filterClause += string.Format(" nv.{0} = N'{1}' AND ",
                                                  TblNhanVien.Columns.NhomNhanVien,
                                                  cbSearchChucVu.SelectedValue);
                if (cbSearchTrangThai.HasValue)
                    filterClause += string.Format(" nv.{0} = {1} AND ",
                                                  TblNhanVien.Columns.TrangThaiSuDung,
                                                  cbSearchTrangThai.SelectedValue);
                if (cbSearchPhongBan.HasValue)
                    filterClause += string.Format(" nv.{0} = {1} AND ",
                                                  TblNhanVien.Columns.IDPhongBan,
                                                  cbSearchPhongBan.SelectedValue);
                if (cbSearchToNhanVien.HasValue)
                    filterClause += string.Format(" nv.{0} = {1} AND ",
                                                  TblNhanVien.Columns.IDTo,
                                                  cbSearchToNhanVien.SelectedValue);
                if (cbSearchTaiKhoanDangNhap.HasValue)
                    filterClause += string.Format(" nd.{0} {1} AND ",
                                                  TblTaiKhoanNguoiDung.Columns.IDNhanVien,
                                                  cbSearchTaiKhoanDangNhap.SelectedValue);
                #endregion

                filterClause = filterClause.Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);
                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();


                #region set value for table join
                string tableJoin = string.Format(" left join {0} as nd on nd.{1} = nv.Id left join {2} as pb on nv.{3} = pb.Id ",
                                                 TblTaiKhoanNguoiDung.Schema.TableName,
                                                 TblTaiKhoanNguoiDung.Columns.IDNhanVien,
                                                 TblPhongBan.Schema.TableName,
                                                 TblNhanVien.Columns.IDPhongBan);
                #endregion

                int total = 0;
                List<TblNhanVien> lst = UserManager.SearchNhanVien(rowIndex, pageSize, column, tableJoin,
                                                                       filterClause, orderClause, out total);
                if (grvUser.IsInsert)
                    lst.Insert(0, new TblNhanVien());
                grv.DataSource = lst;
                List<TblNhanVien> lstEmpty = new List<TblNhanVien>();
                lstEmpty.Add(new TblNhanVien());
                grv.DataSourceEmpty = lstEmpty;
                grv.VirtualRecordCount = total;
            }
        }

        protected string CheckAccount(object data)
        {
            string rs = "<span class='label'>Không có TK</span>";
            if (data != null && data is TblNhanVien)
            {
                TblNhanVien nv = data as TblNhanVien;
                if (nv != null)
                {
                    if (nv.HasAccount) rs = "<span class='label label-success'>Có TK</span>";

                }
            }
            return rs;
        }

        protected void btnSearchNow_Click(object sender, EventArgs e)
        {
            grvUser.CurrentPageIndex = 1;
            grvUser.Rebind();
            dialogSearch.CloseDialog();
        }

        protected void chkEditCoTaiKhoanNguoiDung_CheckedChanged(object sender, EventArgs e)
        {
            txtEditEmail.Enabled = chkEditCoTaiKhoanNguoiDung.Checked;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearchEmail.Text = txtSearchHoVaTen.Text = string.Empty;
            txtSearchEmail.Enabled = false;
            cbSearchGioiTinh.SelectedIndex = cbSearchTaiKhoanDangNhap.SelectedIndex = cbSearchTrangThai.SelectedIndex = 0;
            if (cbSearchPhongBan.Items.Count > 0)
                cbSearchPhongBan.SelectedIndex = 0;
            if (cbSearchToNhanVien.Items.Count > 0)
                cbSearchToNhanVien.SelectedIndex = 0;
            grvUser.Rebind();
            dialogDetail.CloseDialog();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            RefreshDetail();
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmd = btn.CommandArgument;
                if (!string.IsNullOrEmpty(cmd))
                {
                    TblNhanVien nv = UserManager.GetNhanVienByID(cmd);
                    if (nv != null)
                    {
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.NV_Update,
                                                     string.Format("Cập nhật nhân viên <b>{0}</b> phòng {1}", nv.HoVaTen, nv.TenPhongBan));

                        txtDetailHo.Text = nv.Ho;
                        txtDetailTen.Text = nv.Ten;
                        if (!string.IsNullOrEmpty(nv.NhomNhanVien))
                        {
                            // duc.dhqn
                            ListItem items = cbEditChucVu.Items.FindByValue(nv.NhomNhanVien);
                            if (items != null) cbEditChucVu.SelectedValue = nv.NhomNhanVien;
                        }
                        ListItem item = cbEditGioiTinh.Items.FindByValue(nv.GioiTinh);
                        if (item != null) cbEditGioiTinh.SelectedValue = nv.GioiTinh;
                        if (nv.NgaySinh.HasValue && nv.NgaySinh.Value != DateTime.MinValue)
                            dateEditNgaySinh.DateValue = nv.NgaySinh.Value;
                        if (nv.IDPhongBan.HasValue)
                        {
                            ListItem itemPB = cbEditPhongBan.Items.FindByValue(nv.IDPhongBan.Value.ToString());
                            if (itemPB != null)
                            {
                                cbEditPhongBan.SelectedValue = nv.IDPhongBan.Value.ToString();
                                LoadToNhanVien(nv.IDPhongBan.Value.ToString(), cbEditToNhanVien);
                            }
                        }

                        if (nv.IDTo.HasValue)
                        {
                            ListItem itemTo = cbEditToNhanVien.Items.FindByValue(nv.IDTo.Value.ToString());
                            if (itemTo != null) cbEditToNhanVien.SelectedValue = nv.IDTo.Value.ToString();
                        }
                        if (!string.IsNullOrEmpty(nv.Email))
                        {
                            chkEditCoTaiKhoanNguoiDung.Checked = true;
                            txtEditEmail.Text = nv.Email;
                            txtEditEmail.Enabled = true;
                        }
                        chkDetailTrangThaiSuDung.Checked = nv.TrangThaiSuDung.HasValue && nv.TrangThaiSuDung.Value;
                        CurrentLog.AddFirstValue(RightPageName.NV_Update, string.Format("Họ tên"),
                                            TblNhanVien.Columns.Ho,
                                            txtDetailHo.Text);
                        CurrentLog.AddFirstValue(RightPageName.NV_Update, "Giới tính",
                                          TblNhanVien.Columns.GioiTinh,
                                          cbEditGioiTinh.DisplayText);
                        CurrentLog.AddFirstValue(RightPageName.NV_Update, "Ngày sinh",
                                          TblNhanVien.Columns.NgaySinh,
                                          dateEditNgaySinh.DateValue != DateTime.MinValue ? dateEditNgaySinh.DateValue.ToString("dd/MM/yyyy") : "");
                        CurrentLog.AddFirstValue(RightPageName.NV_Update, "Phòng ban",
                                          TblNhanVien.Columns.IDPhongBan,
                                          cbEditPhongBan.DisplayText);
                        CurrentLog.AddFirstValue(RightPageName.NV_Update, "Tổ nhân viên",
                                          TblNhanVien.Columns.IDTo,
                                          cbEditToNhanVien.DisplayText);
                        CurrentLog.AddFirstValue(RightPageName.NV_Update, "Tài khoản người dùng",
                                          TblTaiKhoanNguoiDung.Columns.IDTaiKhoan,
                                          txtEditEmail.Text);
                        hdfDetailUserID.Value = nv.Id.ToString();
                        lblDialogDetailTitle.Text = "Cập nhật thông tin nhân viên";
                        dialogDetail.ShowDialog();
                    }
                }
            }
        }

        protected void btnAuthorization_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmd = btn.CommandArgument;
                Guid g = new Guid(cmd);
                if (!string.IsNullOrEmpty(cmd) && g != Guid.Empty)
                {
                    TblTaiKhoanNguoiDung nguoiDung = UserManager.GetCommonUserByID(cmd);
                    if (nguoiDung != null)
                    {
                        ucUserInRole.LoadRole(nguoiDung);
                        dialogRole.ShowDialog();
                    }
                    else
                        ShowNotify("Thông báo", "Không tìm thấy thông tin tài khoản", NotifyType.Error);

                }
                else
                    ShowNotify("Thông báo", "Nhân viên chưa được cấp tài khoản đăng nhập hệ thống", NotifyType.Error);
            }
        }

        protected void btnResetPass_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmd = btn.CommandArgument;
                Guid g = new Guid(cmd);
                if (!string.IsNullOrEmpty(cmd) && g != Guid.Empty)
                {
                    TblTaiKhoanNguoiDung nguoiDung = UserManager.GetCommonUserByID(cmd);
                    if (nguoiDung != null)
                    {
                        nguoiDung.MatKhau = SecurityHelper.Encrypt128("123456");
                        nguoiDung = UserManager.UpdateCommonUser(nguoiDung);
                        ShowNotify("Thông báo", "Tài khoản đã được đổi về mật khẩu mặc định", NotifyType.Success);
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.NV_Role,
                                                     string.Format("Reset mật khẩu tài khoản <b>{0}</b>", nguoiDung.Email));

                        CurrentLog.AddFirstValue(RightPageName.NV_Role, "Người reset", TblTaiKhoanNguoiDung.Columns.MatKhau, ApplicationContext.Current.CommonUser.Email);
                        CurrentLog.SaveLog(RightPageName.NV_Role);

                    }
                    else
                        ShowNotify("Thông báo", "Không tìm thấy thông tin tài khoản", NotifyType.Error);

                }
                else
                    ShowNotify("Thông báo", "Nhân viên chưa được cấp tài khoản đăng nhập hệ thống", NotifyType.Error);
            }
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
                        if (CurrentMessageBoxResult.CommandName == "delete_nhanvien")
                        {
                            TblNhanVien nhanVien = CurrentMessageBoxResult.Value as TblNhanVien;
                            if (nhanVien != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.NV_Delete, "Xóa nhân viên");
                                    CurrentLog.AddFirstValue(RightPageName.NV_Delete, string.Format("Tên {0}", nhanVien.HoVaTen),
                                                             TblNhanVien.Columns.Ten, nhanVien.HoVaTen);
                                    CurrentLog.AddFirstValue(RightPageName.NV_Delete, string.Format("Phòng {0}", nhanVien.TenPhongBan),
                                                             TblNhanVien.Columns.IDPhongBan, nhanVien.TenPhongBan);
                                    resultDelete = UserManager.DeleteNhanVien(nhanVien.Id);
                                    if (nhanVien.IDNguoiDung != Guid.Empty)
                                        UserManager.DeleteCommonUser(nhanVien.IDNguoiDung);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvUser.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", "nhân viên"),
                                                string.Format("Bạn vừa xóa nhân viên <b>{0}</b>",
                                                              nhanVien.HoVaTen),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.DVT_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", "nhân viên"),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               nhanVien.HoVaTen, "nhân viên"),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblNhanVien nhanVien = UserManager.GetNhanVienByID(btn.CommandArgument);
                    if (nhanVien != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = nhanVien;
                        result.Submit = true;
                        result.CommandName = "delete_nhanvien";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", "nhân viên"),
                                       string.Format("Bạn thật sự muốn xóa {1} <b>{0}</b> ?",
                                                     nhanVien.HoVaTen, "nhân viên"));
                    }
                }
            }
        }

        protected void btnRoleForUser_Click(object sender, EventArgs e)
        {
            if (ucUserInRole.Save())
            {
                dialogRole.CloseDialog();
                ShowNotify("Thông báo", "Đã phân quyền cho tài khoản người dùng", NotifyType.Success);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region Validation
            if (string.IsNullOrEmpty(txtDetailTen.Text))
            {
                AddValidPromt(txtDetailTen.ClientID, "Nhập họ tên nhân viên");
                txtDetailTen.Focus();
            }

            if (string.IsNullOrEmpty(txtDetailHo.Text))
            {
                AddValidPromt(txtDetailHo.ClientID, "Nhập họ tên nhân viên");
                txtDetailHo.Focus();
            }
            TblNhanVien nhanVien = new TblNhanVien();
            if (!string.IsNullOrEmpty(hdfDetailUserID.Value)) nhanVien = UserManager.GetNhanVienByID(hdfDetailUserID.Value);
            if (chkEditCoTaiKhoanNguoiDung.Checked)
            {
                if (string.IsNullOrEmpty(txtEditEmail.Text.Trim()))
                {
                    AddValidPromt(txtEditEmail.ClientID, "Nhập email tài khoản đăng nhập");
                    txtEditEmail.Focus();
                }
                else
                {
                    TblTaiKhoanNguoiDung nvCheck = UserManager.GetCommonUserByEmail(txtEditEmail.Text.Trim());
                    if (nvCheck != null)
                    {
                        if (string.IsNullOrEmpty(hdfDetailUserID.Value))
                        {
                            AddValidPromt(txtEditEmail.ClientID, "Email đã tồn tại trên hệ thống");
                            txtEditEmail.Focus();
                        }
                        else
                        {

                            if (nhanVien != null && nhanVien.Id != nvCheck.IDNhanVien)
                            {
                                AddValidPromt(txtEditEmail.ClientID, "Email đã tồn tại trên hệ thống");
                                txtEditEmail.Focus();
                            }
                        }
                    }
                }
            }
            #endregion

            if (PageIsValid)
            {

                if (!string.IsNullOrEmpty(hdfDetailUserID.Value))
                {
                    #region update

                    if (nhanVien != null)
                    {
                        nhanVien.NhomNhanVien = cbEditChucVu.SelectedValue;
                        nhanVien.Ho = txtDetailHo.Text;
                        nhanVien.Ten = txtDetailTen.Text;
                        nhanVien.GioiTinh = cbEditGioiTinh.SelectedValue;
                        if (dateEditNgaySinh.DateValue != DateTime.MinValue)
                            nhanVien.NgaySinh = dateEditNgaySinh.DateValue;
                        if (cbEditPhongBan.HasValue)
                            nhanVien.IDPhongBan = int.Parse(cbEditPhongBan.SelectedValue);
                        if (cbEditToNhanVien.HasValue)
                            nhanVien.IDTo = int.Parse(cbEditToNhanVien.SelectedValue);
                        nhanVien.TrangThaiSuDung = chkDetailTrangThaiSuDung.Checked;
                        if (chkEditCoTaiKhoanNguoiDung.Checked)
                        {
                            TblTaiKhoanNguoiDung user = new TblTaiKhoanNguoiDung();
                            if (nhanVien.IDNguoiDung != Guid.Empty)
                                user = UserManager.GetCommonUserByID(nhanVien.IDNguoiDung);
                            if (user == null) user = new TblTaiKhoanNguoiDung();
                            user.Ho = nhanVien.Ho;
                            user.Ten = nhanVien.Ten;
                            user.Email = txtEditEmail.Text;
                            if (nhanVien.NgaySinh != DateTime.MinValue)
                                user.NgaySinh = nhanVien.NgaySinh;
                            user.TrangThaiKichHoat = nhanVien.TrangThaiSuDung;
                            if (user.IDTaiKhoan == Guid.Empty)
                            {
                                user.MatKhau = SecurityHelper.Encrypt128("123456");
                                user.IDNhanVien = nhanVien.Id;
                                user = UserManager.InsertCommonUser(user);
                            }
                            else
                            {
                                user = UserManager.UpdateCommonUser(user);
                            }
                        }
                        else if (nhanVien.IDNguoiDung != Guid.Empty)
                            UserManager.DeleteCommonUser(nhanVien.IDNguoiDung);
                        nhanVien = UserManager.UpdateNhanVien(nhanVien);

                        #region log
                        CurrentLog.AddLastValue(RightPageName.NV_Update,
                                          TblNhanVien.Columns.Ho,
                                          txtDetailHo.Text);
                        CurrentLog.AddLastValue(RightPageName.NV_Update,
                                          TblNhanVien.Columns.GioiTinh,
                                          cbEditGioiTinh.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.NV_Update,
                                          TblNhanVien.Columns.NgaySinh,
                                          dateEditNgaySinh.DateValue != DateTime.MinValue ? dateEditNgaySinh.DateValue.ToString("dd/MM/yyyy") : "");
                        CurrentLog.AddLastValue(RightPageName.NV_Update,
                                          TblNhanVien.Columns.IDPhongBan,
                                          cbEditPhongBan.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.NV_Update,
                                          TblNhanVien.Columns.IDTo,
                                          cbEditToNhanVien.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.NV_Update,
                                          TblTaiKhoanNguoiDung.Columns.IDTaiKhoan,
                                          txtEditEmail.Text);
                        CurrentLog.SaveLog(RightPageName.NV_Update);
                        #endregion
                        ShowNotify("Thông báo", string.Format("Cập nhật thông tin nhân viên <b>{0}</b> thành công", nhanVien.HoVaTen), NotifyType.Success);
                        dialogDetail.CloseDialog();
                        grvUser.Rebind();
                    }
                    else
                        ShowNotify("Thông báo", "Không tìm thấy thông tin nhân viên trên hệ thống", NotifyType.Error);
                    #endregion
                }
                else
                {
                    #region insert
                    nhanVien.Ho = txtDetailHo.Text;
                    nhanVien.Ten = txtDetailTen.Text;
                    nhanVien.NhomNhanVien = cbEditChucVu.SelectedValue;
                    nhanVien.GioiTinh = cbEditGioiTinh.SelectedValue;
                    if (dateEditNgaySinh.DateValue != DateTime.MinValue)
                        nhanVien.NgaySinh = dateEditNgaySinh.DateValue;
                    if (cbEditPhongBan.HasValue)
                        nhanVien.IDPhongBan = int.Parse(cbEditPhongBan.SelectedValue);
                    if (cbEditToNhanVien.HasValue)
                        nhanVien.IDTo = int.Parse(cbEditToNhanVien.SelectedValue);
                    nhanVien.TrangThaiSuDung = chkDetailTrangThaiSuDung.Checked;

                    nhanVien = UserManager.InsertNhanVien(nhanVien);

                    if (chkEditCoTaiKhoanNguoiDung.Checked)
                    {
                        TblTaiKhoanNguoiDung user = new TblTaiKhoanNguoiDung();
                        if (nhanVien.IDNguoiDung != Guid.Empty)
                            user = UserManager.GetCommonUserByID(nhanVien.IDNguoiDung);
                        if (user == null) user = new TblTaiKhoanNguoiDung();
                        user.Ho = nhanVien.Ho;
                        user.Ten = nhanVien.Ten;
                        user.Email = txtEditEmail.Text;
                        user.NgaySinh = nhanVien.NgaySinh;
                        user.TrangThaiKichHoat = nhanVien.TrangThaiSuDung;

                        user.MatKhau = SecurityHelper.Encrypt128("123456");
                        user.IDNhanVien = nhanVien.Id;
                        user = UserManager.InsertCommonUser(user);

                    }

                    #region log
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.NV_Update,
                                                 string.Format("Cập nhật nhân viên <b>{0}</b> phòng {1}", nhanVien.HoVaTen, nhanVien.TenPhongBan));

                    CurrentLog.AddFirstValue(RightPageName.NV_Update, string.Format("Họ tên"),
                                      TblNhanVien.Columns.Ho,
                                      txtDetailHo.Text);
                    CurrentLog.AddFirstValue(RightPageName.NV_Update, "Giới tính",
                                      TblNhanVien.Columns.GioiTinh,
                                      cbEditGioiTinh.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.NV_Update, "Ngày sinh",
                                      TblNhanVien.Columns.NgaySinh,
                                      dateEditNgaySinh.DateValue != DateTime.MinValue ? dateEditNgaySinh.DateValue.ToString("dd/MM/yyyy") : "");
                    CurrentLog.AddFirstValue(RightPageName.NV_Update, "Phòng ban",
                                      TblNhanVien.Columns.IDPhongBan,
                                      cbEditPhongBan.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.NV_Update, "Tổ nhân viên",
                                      TblNhanVien.Columns.IDTo,
                                      cbEditToNhanVien.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.NV_Update, "Tài khoản người dùng",
                                      TblTaiKhoanNguoiDung.Columns.IDTaiKhoan,
                                      txtEditEmail.Text);
                    #endregion
                    ShowNotify("Thông báo", string.Format("Thêm mới thông tin nhân viên <b>{0}</b> thành công", nhanVien.HoVaTen), NotifyType.Success);
                    dialogDetail.CloseDialog();
                    grvUser.Rebind();
                    #endregion
                }
            }
            else ShowValidPromt();
        }

        protected void cbEditPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadToNhanVien(cbEditPhongBan.SelectedValue, cbEditToNhanVien);
        }

        protected void cbSearchPhongBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadToNhanVien(cbSearchPhongBan.SelectedValue, cbSearchToNhanVien);
        }
    }
}