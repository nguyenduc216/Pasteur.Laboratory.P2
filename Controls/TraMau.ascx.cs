using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Controls
{
    public partial class TraMau : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dateNgayThuTien.DateValue = DateTime.Now;
                txtNguoiThuTien.Text = ApplicationContext.Current.CommonUser.HoVaTen;
            }
        }

        public void LoadMau(TblMauXetNghiem mauXN)
        {
            chkGhiNo.Checked = false;
            txtSoHoaDon.Text = "";
            if (mauXN != null)
            {
                //TblKhachHang kh = XetNghiemManager.GetKhachHangByID(mauXN.IDKhachHang.Value);

                hiddenMauID.Value = mauXN.Id.ToString();
                txtDiaChi.Text = !string.IsNullOrEmpty(mauXN.ThuTienDiaChi) ? mauXN.ThuTienDiaChi : !string.IsNullOrEmpty(mauXN.DiaChiKhachHang) ? mauXN.DiaChiKhachHang : "";
                txtThuTienDonVi.Text = !string.IsNullOrEmpty(mauXN.ThuTienDonVi) ? mauXN.ThuTienDonVi : !string.IsNullOrEmpty(mauXN.TenDonVi) ? mauXN.TenDonVi : "";
                txtMaSoThue.Text = mauXN.ThuTienMaSoThue;
                txtThuTienHoVaTen.Text = !string.IsNullOrEmpty(mauXN.ThuTienHoVaTen) ? mauXN.ThuTienHoVaTen : !string.IsNullOrEmpty(mauXN.TenKhachHang) ? mauXN.TenKhachHang : "";
                txtThuTienSoDienThoai.Text = !string.IsNullOrEmpty(mauXN.ThuTienDienThoai) ? mauXN.ThuTienDienThoai : !string.IsNullOrEmpty(mauXN.DienThoaiKhachHang) ? mauXN.DienThoaiKhachHang : "";
                //dateNgayThuTien.DateValue = DateTime.Now;
                if (mauXN.ThuTienNgayThu.HasValue && mauXN.ThuTienNgayThu.Value != DateTime.MinValue)
                    dateNgayThuTien.DateValue = mauXN.ThuTienNgayThu.Value;
                txtNguoiThuTien.Text = !string.IsNullOrEmpty(mauXN.ThuTienNguoiThu) ? mauXN.ThuTienNguoiThu : ApplicationContext.Current.CommonUser.HoVaTen;
                chkGhiNo.Checked = mauXN.GhiNo.HasValue && mauXN.GhiNo.Value;
                txtSoHoaDon.Text = mauXN.SoHoaDon;
            }
        }

        public bool Save()
        {
            if (!string.IsNullOrEmpty(hiddenMauID.Value))
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(hiddenMauID.Value);
                if (mauXN != null)
                {
                    mauXN.ThuTienDiaChi = txtDiaChi.Text;
                    mauXN.ThuTienHoVaTen = txtThuTienHoVaTen.Text;
                    mauXN.ThuTienDienThoai = txtThuTienSoDienThoai.Text;
                    mauXN.ThuTienDonVi = txtThuTienDonVi.Text;
                    mauXN.ThuTienMaSoThue = txtMaSoThue.Text;
                    if ((!mauXN.ThuTienNgayThu.HasValue || (mauXN.ThuTienNgayThu.HasValue && mauXN.ThuTienNgayThu.Value == DateTime.MinValue)) && dateNgayThuTien.DateValue != DateTime.MinValue)
                        mauXN.ThuTienNgayThu = dateNgayThuTien.DateValue;
                    mauXN.GhiNo = chkGhiNo.Checked;
                    mauXN.SoHoaDon = txtSoHoaDon.Text;
                    mauXN.ThuTienNguoiThu = string.IsNullOrEmpty(txtNguoiThuTien.Text) ? ApplicationContext.Current.CommonUser.HoVaTen : txtNguoiThuTien.Text;
                    mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    return true;
                }
            }
            return false;
        }
    }
}