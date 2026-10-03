using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.Controls
{
    public partial class QLXN_ChiTieuXetNghiem : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadControl();
        }

        private void LoadControl()
        {
            List<TblDonViTinh> donViTinhCol = CategoryManager.GetDonViTinhALL(true);
            cbDonViTinh.DataSource = donViTinhCol;
            cbDonViTinh.DataValueField = TblDonViTinh.Columns.Id;
            cbDonViTinh.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbDonViTinh.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            cbQuyChuan.DataSource = quyChuanCol;
            cbQuyChuan.DataValueField = TblCatQuyChuan.Columns.Id;
            cbQuyChuan.DataTextField = TblCatQuyChuan.Columns.Ten;
            cbQuyChuan.DataBind();

            List<TblCatPhuongPhap> phuongPhapCol = CategoryManager.GetPhuongPhapAll(true);
            cbPhuongPhap.DataSource = phuongPhapCol;
            cbPhuongPhap.DataValueField = TblCatPhuongPhap.Columns.Id;
            cbPhuongPhap.DataTextField = TblCatPhuongPhap.Columns.Ten;
            cbPhuongPhap.DataBind();
        }

        public void Refresh()
        {
            txtSTT.Text = txtTenTaiKhoa.Text = txtTenTaiCacKhoa.Text = txtTenTrongThongTu.Text = string.Empty;
            hiddenEditID.Value = string.Empty;
        }

        public bool LoadChiTieu(object id)
        {
            bool result = false;
            hiddenEditID.Value = "";
            if (id != null)
            {
                TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(id);
                if (chiTieu != null)
                {


                    hiddenEditID.Value = chiTieu.Id.ToString();
                    txtTenTaiCacKhoa.Text = chiTieu.TenTaiCacKhoa;
                    txtTenTaiKhoa.Text = chiTieu.TenTaiCacKhoa;
                    txtTenTrongThongTu.Text = chiTieu.TenTrongTTQD;
                    txtSTT.Text = chiTieu.Stt;

                    if (chiTieu.IDDonViTinh.HasValue)
                    {
                        ListItem item = cbDonViTinh.Items.FindByValue(chiTieu.IDDonViTinh.Value.ToString());
                        if (item != null) cbDonViTinh.SelectedValue = chiTieu.IDDonViTinh.Value.ToString();
                    }

                    if (chiTieu.IDQuyTrinh.HasValue)
                    {
                        ListItem item = cbQuyChuan.Items.FindByValue(chiTieu.IDQuyTrinh.Value.ToString());
                        if (item != null) cbQuyChuan.SelectedValue = chiTieu.IDQuyTrinh.Value.ToString();
                    }

                    chkTrangThaiSuDung.Checked = chiTieu.TrangThaiSuDung.HasValue && chiTieu.TrangThaiSuDung.Value;

                    #region Log
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.ChiTieu_Update,
                                                 string.Format("Cập nhật chỉ tiêu xét nghiệm <b>{0}</b> ", chiTieu.TenTaiCacKhoa));

                   
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Tên tại khoa",
                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                             txtTenTaiKhoa.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Tên theo thông tư",
                                             TblChiTieuXetNghiem.Columns.TenTrongTTQD,
                                             txtTenTrongThongTu.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "STT",
                                             TblChiTieuXetNghiem.Columns.Stt,
                                             txtSTT.Text);

                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Đơn vị tính",
                                             TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                             cbDonViTinh.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Quy chuẩn",
                                             TblChiTieuXetNghiem.Columns.IDQuyTrinh,
                                             cbQuyChuan.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Phương pháp",
                                             TblChiTieuXetNghiem.Columns.IDPhuongPhap,
                                             cbPhuongPhap.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Trạng thái sử dụng",
                                             TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                             chkTrangThaiSuDung.Checked ? "Có" : "Không");
                    #endregion
                    result = true;
                }
            }
            return result;
        }

        public bool IsCreateNew { get { return string.IsNullOrEmpty(hiddenEditID.Value) || hiddenEditID.Value == "0"; } }

        public bool Save()
        {
            bool result = false;

            #region Validation
            if (string.IsNullOrEmpty(txtTenTaiCacKhoa.Text))
            {
                AddValidPromt(txtTenTaiCacKhoa.ClientID, "Nhập tên thông thường");
                txtTenTaiCacKhoa.Focus();
            }

            if (string.IsNullOrEmpty(txtTenTaiKhoa.Text))
            {
                AddValidPromt(txtTenTaiKhoa.ClientID, "Nhập tên tại khoa");
                txtTenTaiKhoa.Focus();
            }

            if (string.IsNullOrEmpty(txtTenTrongThongTu.Text))
            {
                AddValidPromt(txtTenTrongThongTu.ClientID, "Nhập tên theo thông tư");
                txtTenTrongThongTu.Focus();
            }

            if (!cbDonViTinh.HasValue)
            {
                AddValidPromt(cbDonViTinh.ClientID, "Chọn đơn vị tính");
                cbDonViTinh.Focus();
            }

            if (!cbQuyChuan.HasValue)
            {
                AddValidPromt(cbQuyChuan.ClientID, "Chọn quy chuẩn xét nghiệm");
                cbQuyChuan.Focus();
            }

            if (!cbPhuongPhap.HasValue)
            {
                AddValidPromt(cbPhuongPhap.ClientID, "Chọn phương pháp");
                cbPhuongPhap.Focus();
            }
            #endregion

            if (ControlIsValid)
            {
                if (!string.IsNullOrEmpty(hiddenEditID.Value))
                {
                    TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(hiddenEditID.Value);
                    if (chiTieu != null)
                    {

                        chiTieu.TenTaiCacKhoa = txtTenTaiCacKhoa.Text;
                        chiTieu.TenTaiCacKhoa = txtTenTaiKhoa.Text;
                        chiTieu.TenTrongTTQD = txtTenTrongThongTu.Text;

                        chiTieu.IDQuyTrinh = int.Parse(cbQuyChuan.SelectedValue);
                        chiTieu.IDPhuongPhap = int.Parse(cbPhuongPhap.SelectedValue);
                        chiTieu.IDDonViTinh = int.Parse(cbDonViTinh.SelectedValue);
                        chiTieu.TrangThaiSuDung = chkTrangThaiSuDung.Checked;

                        chiTieu = ChiTieuManager.UpdateChiTieuXetNghiem(chiTieu);
                        #region Log


                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                 txtTenTaiCacKhoa.Text);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                 txtTenTaiKhoa.Text);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TenTrongTTQD,
                                                 txtTenTrongThongTu.Text);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.Stt,
                                                 txtSTT.Text);

                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                                 cbDonViTinh.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.IDQuyTrinh,
                                                 cbQuyChuan.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.IDPhuongPhap,
                                                 cbPhuongPhap.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                                 chkTrangThaiSuDung.Checked ? "Có" : "Không");
                        #endregion
                        result = true;
                    }
                }
                else
                {
                    TblChiTieuXetNghiem chiTieu = new TblChiTieuXetNghiem();

                    chiTieu.TenTaiCacKhoa = txtTenTaiCacKhoa.Text;
                    chiTieu.TenTaiCacKhoa = txtTenTaiKhoa.Text;
                    chiTieu.TenTrongTTQD = txtTenTrongThongTu.Text;

                    chiTieu.IDQuyTrinh = int.Parse(cbQuyChuan.SelectedValue);
                    chiTieu.IDPhuongPhap = int.Parse(cbPhuongPhap.SelectedValue);
                    chiTieu.IDDonViTinh = int.Parse(cbDonViTinh.SelectedValue);
                    chiTieu.TrangThaiSuDung = chkTrangThaiSuDung.Checked;

                    chiTieu = ChiTieuManager.InsertChiTieuXetNghiem(chiTieu);

                    #region Log
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.ChiTieu_Insert,
                                                 string.Format("Cập nhật chỉ tiêu xét nghiệm <b>{0}</b> ", chiTieu.TenTaiCacKhoa));

                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Tên thông thường",
                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                             txtTenTaiCacKhoa.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Tên tại khoa",
                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                             txtTenTaiKhoa.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Tên theo thông tư",
                                             TblChiTieuXetNghiem.Columns.TenTrongTTQD,
                                             txtTenTrongThongTu.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "STT",
                                             TblChiTieuXetNghiem.Columns.Stt,
                                             txtSTT.Text);

                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Đơn vị tính",
                                             TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                             cbDonViTinh.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Quy chuẩn",
                                             TblChiTieuXetNghiem.Columns.IDQuyTrinh,
                                             cbQuyChuan.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Phương pháp",
                                             TblChiTieuXetNghiem.Columns.IDPhuongPhap,
                                             cbPhuongPhap.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Trạng thái sử dụng",
                                             TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                             chkTrangThaiSuDung.Checked ? "Có" : "Không");
                    CurrentLog.SaveLog(RightPageName.ChiTieu_Insert);
                    #endregion
                    result = true;
                }
            }

            return result;
        }
    }
}