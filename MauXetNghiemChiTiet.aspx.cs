using Microsoft.SqlServer.Server;
using Pasteur.Core;
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

    public partial class MauXetNghiemChiTiet : BaseGridPage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                if (CurrentPageType == "LaboHoaLy")
                    return FunctionSystem.Code.Labo_HoaLy;
                if (CurrentPageType == "LaboViSinh")
                    return FunctionSystem.Code.Labo_ViSinh;
                if (CurrentPageType == "KeToanTiepNhan")
                    return FunctionSystem.Code.KeToan_TiepNhan;
                if (CurrentPageType == "TraKQXN")
                    return FunctionSystem.Code.Tra_KQXN;
                return FunctionSystem.Code.Phieu_Mau;
            }
        }

        protected string CurrentPageType
        {
            get { return CommonHelper.QueryString("PageType"); }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                if (CurrentPageType == "LaboHoaLy")
                    return FunctionSystem.Name.Labo_HoaLy;
                if (CurrentPageType == "LaboViSinh")
                    return FunctionSystem.Name.Labo_ViSinh;
                if (CurrentPageType == "KeToanTiepNhan")
                    return FunctionSystem.Name.KeToan_TiepNhan;
                if (CurrentPageType == "TraKQXN")
                    return FunctionSystem.Name.Tra_KQXN;

                return FunctionSystem.Name.Phieu_Mau;
            }
        }


        protected string CurrentName
        {
            get
            {
                if (CurrentPageType == "LaboHoaLy")
                    return "Labo Hóa tiếp nhận mẫu";
                if (CurrentPageType == "LaboViSinh")
                    return "Labo Vi sinh tiếp nhận mẫu";
                if (CurrentPageType == "KeToanTiepNhan")
                    return "Kế toán in phiếu";
                if (CurrentPageType == "TraKQXN")
                    return "Trả kết quả xét nghiệm";
                string rs = "phiếu mẫu xét nghiệm";

                return rs;
            }
        }

        private int? CurrentID
        {
            get
            {
                int? result = null;
                if (!string.IsNullOrEmpty(CommonHelper.QueryString("ID")))
                {
                    int y = -1;
                    if (int.TryParse(CommonHelper.QueryString("ID"), out y) && y > 0) result = y;
                }
                return result;
            }
        }

        protected bool GetButtonRole(string rightName)
        {
            bool rs = false;
            if (CurrentPageType == "LaboHoaLy")
            {
                if (rightName == "Update") rs = UserHasRight(RightPageName.Hoa_TNMSH_Update);
                else if (rightName == "Approve") rs = UserHasRight(RightPageName.Hoa_TNMSH_Chot);
            }
            if (CurrentPageType == "LaboViSinh")
            {
                if (rightName == "Update") rs = UserHasRight(RightPageName.VS_TNMSH_Update);
                else if (rightName == "Approve") rs = UserHasRight(RightPageName.VS_TNMSH_Chot);
            }
            if (CurrentPageType == "KeToanTiepNhan")
            {
                if (rightName == "Print") rs = UserHasRight(RightPageName.KeToa_TNMKT_Print);
            }
            if (CurrentPageType == "TraKQXN")
            {
                if (rightName == "TraKetQua") rs = UserHasRight(RightPageName.PMXN_TraKetQua);
                else if (rightName == "CapNhatThongTin") rs = UserHasRight(RightPageName.PMXN_CapNhatThongTin);
                else if (rightName == "Delete") rs = UserHasRight(RightPageName.PMXN_Delete);
            }
            else
            {
                if (rightName == "Insert") rs = UserHasRight(RightPageName.PMXN_Insert);
                else if (rightName == "Update") rs = UserHasRight(RightPageName.PMXN_Update);
                else if (rightName == "Delete") rs = UserHasRight(RightPageName.PMXN_Delete);
                else if (rightName == "ChuyenMau") rs = UserHasRight(RightPageName.PMXN_ChuyenMau);
            }
            return rs;
        }
        int columnCount = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCopyMaMau.EnterSubmitClientID = btnCopy.ClientID;
            if (!IsPostBack)
            {
                // Code: CTGH-SESSION-20260918 - Page_Load đầu tiên phải xóa cache cũ trước khi LoadData bind danh sách giới hạn.
                Session.Remove("MXN-CTGH");
                dateNgayTaoPhieu.DateValue = DateTime.Now;

                Session["ChiTieuQuyChuan_Temp"] = null;
                lnkDoiMaMau.OnClientClick = string.Format("{0};return false;", dialogDoiMaMau.GetScriptOpen());
                btnCopyCode.OnClientClick = string.Format("{0};return false;", dialogCopy.GetScriptOpen());
                lnkCopySingle.OnClientClick = string.Format("{0};return false;", dialogCopy.GetScriptOpen());
                Session["Insert_MauXN_ChiTieu"] = null;
                Session.Remove("Insert_MauXN_ChiTieu");
                LoadControl();

                btnDuyetLabo.Visible = GetButtonRole("Approve") && CurrentPageType.StartsWith("Labo");
                btnAddGroup.Visible = GetButtonRole("Insert") && CurrentPageType == "PhieuNhanMau";

                if (Session["MauXetNghiem_CreateNew"] != null &&
                   Session["MauXetNghiem_CreateNew"] is TblMauXetNghiem)
                {
                    Session["MauXetNghiem_CreateNew"] = null;
                    Session.Remove("MauXetNghiem_CreateNew");
                    ShowNotifyStartUp("Thông báo", "Tạo mới mẫu xét nghiệm thành công. Vui lòng cập nhật danh sách chỉ tiêu xét nghiệm", false, NotifyType.Success);
                }

                LoadThangNam();
                LoadData();

                if (string.IsNullOrEmpty(txtEditMaPhieuNoiBo.Text.Trim()))
                {

                    string rf2 = "";
                    if (CurrentPageType == "LaboHoaLy")
                        ltrMaNoiBo.Text = string.Format("Mã nội bộ: <b>{0}</b>", XetNghiemManager.GetMauPhieuIndex(ref rf2, 0, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex));
                    else if (CurrentPageType == "LaboViSinh")
                        ltrMaNoiBo.Text = string.Format("Mã nội bộ: <b>{0}</b>", XetNghiemManager.GetMauPhieuIndex(ref rf2, 0, TblMauXetNghiem.Columns.MaSoTraViSinhIndex));

                }
            }
            if (!string.IsNullOrEmpty(cbKetQuaDatMau.SelectedValue) && cbKetQuaDatMau.SelectedValue == "1")
                cbKetQuaDatMau.ComboStyle = SweetSoft.ExtraControls.icons.ButtonStyle.Success;
            if (!string.IsNullOrEmpty(cbKetQuaDatMau.SelectedValue) && cbKetQuaDatMau.SelectedValue == "0")
                cbKetQuaDatMau.ComboStyle = SweetSoft.ExtraControls.icons.ButtonStyle.Danger;

            if (!string.IsNullOrEmpty(cbMauDatChatLuong.SelectedValue) && cbMauDatChatLuong.SelectedValue == "1")
                cbMauDatChatLuong.ComboStyle = SweetSoft.ExtraControls.icons.ButtonStyle.Success;
            if (!string.IsNullOrEmpty(cbMauDatChatLuong.SelectedValue) && cbMauDatChatLuong.SelectedValue == "0")
                cbMauDatChatLuong.ComboStyle = SweetSoft.ExtraControls.icons.ButtonStyle.Danger;
        }
        private void LoadData()
        {
            if (CurrentPageType == "PhieuNhanMau")
            {
                divThongTinFull.Visible = true;
                btnSave.Visible = GetButtonRole("Update");
            }

            if (CurrentID.HasValue)
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                LoadData(mauXN);
            }
        }
        private void LoadData(TblMauXetNghiem mauXN)
        {
            divThongTinFull.Visible = true;
            divThongTinCoBan.Visible = false;
            if (mauXN != null)
            {
                if (!string.IsNullOrEmpty(mauXN.DanhGiaDatMau)) cbKetQuaDatMau.SelectedValue = mauXN.DanhGiaDatMau.Trim();
                chkLaboDanhGiaMau.Checked = mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value;
                btnPrintGroup.Visible = true;
                txtEditMaTraKetQua.Text = mauXN.MaSoTraKQ;
                //hiddenMaTraKetQuaIndex.Value = mauXN.MaSoTraKQIndex;

                ///*lnkPhieuTraKetQua.Visible*/ = mauXN.KetQuaDanhGiaMau == "1";

                if (mauXN.CreatedDate.HasValue && mauXN.CreatedDate.Value != DateTime.MinValue)
                    dateNgayTaoPhieu.DateValue = mauXN.CreatedDate.Value;
                if (mauXN.NgayChuyenLabo.HasValue && mauXN.NgayChuyenLabo.Value != DateTime.MinValue)
                    dateNgayChuyenMau.DateValue = mauXN.NgayChuyenLabo.Value;
                if (CurrentPageType == "PhieuNhanMau")
                {

                    lnkPhieuDeNghiThuTien.Visible = true;
                    divXuLyMau.Visible = divAddChiTieu.Visible = true;
                    chkCheckSearh.Enabled = txtAddTenChiTieu.Enabled = GetButtonRole("Update");
                    linkChuyenMau.Visible = !mauXN.NgayChuyenLabo.HasValue || (mauXN.NgayChuyenLabo.HasValue && mauXN.NgayChuyenLabo.Value == DateTime.MinValue);
                    btnSave.Visible = GetButtonRole("Update");
                    linkChuyenMau.Visible = GetButtonRole("ChuyenMau");

                    lnkInPhieuTiepNhanYC.Visible = true;
                    lnkInPhieuChuyenMau.Visible = mauXN.NgayChuyenLabo.HasValue && mauXN.NgayChuyenLabo.Value != DateTime.MinValue;
                }
                else if (CurrentPageType.StartsWith("Labo"))
                {
                    divThongTinFull.Visible = false;
                    divThongTinCoBan.Visible = true;
                    chkCheckAllGoKetQua.Visible = btnGoKetQua.Visible = true;
                    divUpdateChitieu.Visible = true;
                    btnGoKetQua.Visible = mauXN.TrangThaiMau != "DaTraKetQua";
                    btnUpdateChiTieu.Visible = mauXN.TrangThaiMau != "New";
                    btnDuyetLabo.Visible &= !((CurrentPageType == "LaboHoaLy" && mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value != DateTime.MinValue) ||
                    (CurrentPageType == "LaboViSinh" && mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value != DateTime.MinValue));
                    lnkInPhieuKetQuaNoiNo.Visible = ((CurrentPageType == "LaboHoaLy" && mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value != DateTime.MinValue) ||
                    (CurrentPageType == "LaboViSinh" && mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value != DateTime.MinValue));
                    if (CurrentPageType == "LaboViSinh" && lnkInPhieuKetQuaNoiNo.Visible)
                    {
                        ltrLI.Text = string.Format("<li><a href='{0}' target='blank'>Phiếu KQKNNB vi sinh BITEST</a></li>", string.Format("/phieu-tra-ket-qua-noi-bo-vs-so-{0}-m1", CurrentID.Value));
                        ltrLI.Text += string.Format("<li><a href='{0}' target='blank'>Phiếu KQKNNB vi sinh không khí</a></li>", string.Format("/phieu-tra-ket-qua-noi-bo-vs-so-{0}-m2", CurrentID.Value));
                    }
                    txtNhanXetLaboHoa.Visible = CurrentPageType == "LaboHoaLy";
                    txtNhanXetLaboHoa.Text = mauXN.LaboHoaNhanXet;
                    if (CurrentPageType == "LaboHoaLy")
                        ltrNhanXetTitle.Text = "Nhận xét của Labo hóa lý";
                    else
                        ltrNhanXetTitle.Text = "Nhận xét của Labo vi sinh";


                    txtNhanXetLaboVS.Visible = CurrentPageType == "LaboViSinh";
                    txtNhanXetLaboVS.Text = mauXN.LaboSinhNhanXet;
                }
                else if (CurrentPageType == "TraKQXN")
                {
                    hiddenGiaTriNhapVS.Value = "binhthuong"; // mauXN.LoaiLaboVisinh;
                    //lnkPhieuDeNghiThuTien.Visible = true;
                    divUpdateChitieu.Visible = true;
                    txtNhanXetCuoiCung.Visible = true;
                    //btnGhiNhanKetQuaGroup.Visible = true;
                    //btnCapNhatThongTinTraMau.Visible = true;
                    divKQ.Visible = true;
                    lnkGhiNhanKetQua.Visible = GetButtonRole("CapNhatThongTin");

                    //divCapNhat.Visible = 
                    divChatLuong.Visible = GetButtonRole("TraKetQua");
                    divKetQua2.Visible = GetButtonRole("CapNhatThongTin");
                    lnkPhieuTraKetQua.Visible = lnkPhieuTraKetQuaM1.Visible = lnkPhieuTraKetQuaM2.Visible = true;

                    if (string.IsNullOrEmpty(txtEditMaTraKetQua.Text))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraKQ = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraKQIndex);
                        mauXN.MaSoTraKQIndex = codeIndex;

                    }
                    txtEditMaTraKetQua.Text = mauXN.MaSoTraKQ;
                    //hiddenMaTraKetQuaIndex.Value = mauXN.MaSoTraKQIndex;
                }
                else if (CurrentPageType == "KeToanTiepNhan")
                    lnkPhieuDeNghiThuTien.Visible = true;



                txtGhiChuKetQua.Text = mauXN.GhiChuKetQua;
                txtNhanXetCuoiCung.Text = mauXN.NhanXetCuoiCung;
                if (string.IsNullOrEmpty(txtNhanXetCuoiCung.Text)) txtNhanXetCuoiCung.Text = mauXN.LaboHoaNhanXet;
                if (string.IsNullOrEmpty(txtNhanXetCuoiCung.Text)) txtNhanXetCuoiCung.Text = mauXN.LaboSinhNhanXet;

                if (!string.IsNullOrEmpty(mauXN.KetQuaDanhGiaMau))
                    cbMauDatChatLuong.SelectedValue = mauXN.KetQuaDanhGiaMau;

                ltrChangeMa.Text = string.Format("<a><i class='fa fa-'></i></a>");

                ListItem it = cbTrangThaiMau.Items.FindByValue(mauXN.TrangThaiMau);
                ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>{1}</b>", mauXN.MaMau, it.Text, dialogDoiMaMau.GetScriptOpen());
                if ((CurrentPageType == "LaboHoaLy" && mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value != DateTime.MinValue) ||
                    (CurrentPageType == "LaboViSinh" && mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value != DateTime.MinValue))
                    ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>chờ trả kết quả</b>", mauXN.MaMau, it.Text);

                hiddenKhachHangID.Value = mauXN.IDKhachHang.Value.ToString();
                txtTenKhachHang.Text = mauXN.TenKhachHang;
                txtDiaChiKhach.Text = mauXN.DiaChiKhachHang;
                txtDienThoaiKhach.Text = mauXN.DienThoaiKhachHang;
                txtEmailKhach.Text = mauXN.EmailKhachHang;
                if (mauXN.IDNoiNhanMau.HasValue)
                    cbNoiLayMauGoiY.SelectedValue = mauXN.IDNoiNhanMau.Value.ToString();
                txtLoaiMau.Text = mauXN.TenLoaiMau;
                txtQuyChuan.Text = mauXN.TenQuyChuan;
                txtDieuKienBaoQuan2.Text = mauXN.DieuKienBaoQuan;
                txtNgayNhan.Text = mauXN.NgayNhanMauToString;
                txtNgayChuyenMau.Text = mauXN.NgayChuyenLaboToString;
                txtNgayTraKQ.Text = mauXN.NgayTraKetQuaDuKienToString;
                txtTongLuongMau.Text = (mauXN.LuongMau.HasValue ? mauXN.LuongMau.Value.ToStringWithLanguage() : "0") + " " + mauXN.TenDonVi;
                txtLuongMauHoa.Text = (mauXN.LuongMauHoa.HasValue ? mauXN.LuongMauHoa.Value.ToStringWithLanguage() : "0") + " " + mauXN.TenDonVi;
                txtLuongMauViSinh.Text = mauXN.LuongMauViSinh.ToStringWithLanguage() + " " + mauXN.TenDonVi;

                txtTenNguoiGui.Text = mauXN.TenNguoiGuiMau;

                chkLayMauNgoai.Checked = mauXN.LayMauNgoai.HasValue && mauXN.LayMauNgoai.Value;
                chkDanhGiaMau.Checked = mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value;
                chkMauTheoABCD.Checked = mauXN.MauTheoABCD.HasValue && mauXN.MauTheoABCD.Value;

                txtTenNguoiLayMau.Text = mauXN.TenNguoiLayMau;

                txtDiaChiNguoiGui.Text = mauXN.DiaChiNguoiGuiMau;
                txtDienThoaiNguoiGui.Text = mauXN.DienThoaiNguoiGuiMau;
                txtEmailNguoiGui.Text = mauXN.EmailNguoiGuiMau;
                txtDienThoaiNguoiLayMau.Text = mauXN.DienThoaiNguoiLayMau;
                txtEmailNguoiLayMau.Text = mauXN.EmailNguoiLayMau;
                txtDienThoaiNguoiNhan.Text = mauXN.DienThoaiNguoiNhanMau;
                txtEmailNguoiNhanMau.Text = mauXN.EmailNguoiNhanMau;

                txtTenNguoiNhanMau.Text = mauXN.TenNguoiNhanMau;


                if (CurrentPageType == "LaboHoaLy")
                {
                    string codeIndex = "";
                    if (false && string.IsNullOrEmpty(mauXN.MaSoTraHoaLy))
                    {
                        mauXN.MaSoTraHoaLy = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex);
                        mauXN.MaSoTraHoaLyIndex = codeIndex;
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }


                    if (mauXN.ThoiGianHoaBatDau.HasValue && mauXN.ThoiGianHoaBatDau.Value != DateTime.MinValue)
                        dateLaboBatDau.DateValue = mauXN.ThoiGianHoaBatDau.Value;

                    if (mauXN.ThoiGianHoaKetThuc.HasValue && mauXN.ThoiGianHoaKetThuc.Value != DateTime.MinValue)
                        dateLaboKetThuc.DateValue = mauXN.ThoiGianHoaKetThuc.Value;
                    txtEditMaPhieuNoiBo.Text = mauXN.MaSoTraHoaLy;
                }
                else if (CurrentPageType == "LaboViSinh")
                {
                    hiddenGiaTriNhapVS.Value = "binhthuong";// mauXN.LoaiLaboVisinh;
                    string codeIndex = "";
                    if (false && string.IsNullOrEmpty(mauXN.MaSoTraViSinh))
                    {
                        mauXN.MaSoTraViSinh = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraViSinhIndex);
                        mauXN.MaSoTraViSinhIndex = codeIndex;
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }

                    if (mauXN.ThoiGianVSBatDau.HasValue && mauXN.ThoiGianVSBatDau.Value != DateTime.MinValue)
                        dateLaboBatDau.DateValue = mauXN.ThoiGianVSBatDau.Value;
                    else if (mauXN.NgayChuyenLabo.HasValue && mauXN.NgayChuyenLabo.Value != DateTime.MinValue)
                        dateLaboBatDau.DateValue = mauXN.NgayChuyenLabo.Value;

                    if (mauXN.ThoiGianVSKetThuc.HasValue && mauXN.ThoiGianVSKetThuc.Value != DateTime.MinValue)
                        dateLaboKetThuc.DateValue = mauXN.ThoiGianVSKetThuc.Value;
                    txtEditMaPhieuNoiBo.Text = mauXN.MaSoTraViSinh;
                    //btnGiaTriNhapVS.Visible = true;
                }

                ListItem item = cbDonViTinh.Items.FindByValue(mauXN.IDDonViLuongMau.Value.ToString());
                if (item != null) cbDonViTinh.SelectedValue = mauXN.IDDonViLuongMau.Value.ToString();

                ListItem itemLHM = cbLoaiHinhYeuCauMau.Items.FindByValue(mauXN.IDLoaiHinhYeuCauMau.Value.ToString());
                if (itemLHM != null) cbLoaiHinhYeuCauMau.SelectedValue = mauXN.IDLoaiHinhYeuCauMau.Value.ToString();

                ListItem itemLM = cbLoaiMau.Items.FindByValue(mauXN.IDLoaiMau.Value.ToString());
                if (itemLM != null) cbLoaiMau.SelectedValue = mauXN.IDLoaiMau.Value.ToString();

                ListItem itemQC = cbQUyChuan.Items.FindByValue(mauXN.IDQuyChuan.Value.ToString());
                if (itemQC != null)
                {
                    cbQUyChuan.SelectedValue = mauXN.IDQuyChuan.Value.ToString();
                    cbUpdateDVT.Visible = btnUpdateDVT.Visible = itemQC.Text.Trim().ToUpper() == "KHÔNG ĐÁNH GIÁ";
                }

                txtNoiLayMau.Text = mauXN.NoiLayMau;
                txtGhiChuMau.Text = mauXN.GhiChuMau;
                txtDieuKienBaoQuan.Text = mauXN.DieuKienBaoQuan;
                txtMoTaMau.Text = mauXN.MoTaMau;
                txtYeuCauKhac.Text = mauXN.YeuCauKhac;

                if (mauXN.LuongMau.HasValue) txtDungLuongMau.Text = mauXN.LuongMau.Value.ToStringWithLanguage();
                if (mauXN.LuongMauHoa.HasValue) txtDungLuongMauHoa.Text = mauXN.LuongMauHoa.Value.ToStringWithLanguage();

                if (mauXN.NgayLayMau.HasValue && mauXN.NgayLayMau.Value != DateTime.MinValue) dtNgayLayMau.DateValue = mauXN.NgayLayMau.Value;
                if (mauXN.NgayTraKetQua.HasValue && mauXN.NgayTraKetQua.Value != DateTime.MinValue) dateNgayTraKetQua.DateValue = mauXN.NgayTraKetQua.Value;
                if (mauXN.NgayNhanMau.HasValue && mauXN.NgayNhanMau.Value != DateTime.MinValue) dateNgayNhanMau.DateValue = mauXN.NgayNhanMau.Value;

                cbTrangThaiMau.SelectedValue = mauXN.TrangThaiMau;

                chkNguoiGuiLaNV.Checked = mauXN.NguoiGuiLNV.HasValue && mauXN.NguoiGuiLNV.Value;
                chkNguoiLayLaNV.Checked = mauXN.LayMauLNV.HasValue && mauXN.LayMauLNV.Value;
                chkNguoiNhanLaNV.Checked = mauXN.NhanMauLNV.HasValue && mauXN.NhanMauLNV.Value;


                if (CurrentPageType == "PhieuNhanMau")
                    LoadMauTieuChi();
                else
                    LoadMauTieuChiUpdate(mauXN);
                mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
            }
        }

        protected string GetTabVisible(string tabName, string dfClass)
        {
            string defaulCt = "";
            if (tabName == "ChiTieu" && !CurrentID.HasValue) defaulCt = "hide";
            return string.Format("{0} {1}", dfClass, hiddenTab.Value == tabName ? "active" : defaulCt);
        }

        private void LoadControl()
        {
            List<TblCatLoaiMau> loaiMauCol = CategoryManager.GetLoaiMauALL(true);
            cbLoaiMau.DataSource = loaiMauCol;
            cbLoaiMau.DataValueField = TblCatLoaiMau.Columns.Id;
            cbLoaiMau.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbLoaiMau.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            cbQUyChuan.DataSource = quyChuanCol;
            cbQUyChuan.DataValueField = TblCatLoaiMau.Columns.Id;
            cbQUyChuan.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbQUyChuan.DataBind();

            List<TblHasTag> hasTagAll = CategoryManager.GetHasTagAll(true);
            TblHasTag empty = new TblHasTag();
            empty.Id = -1;
            empty.TenTag = "Chọn phạm vi mẫu";
            hasTagAll.Insert(0, empty);
            cbNoiLayMauGoiY.DataSource = hasTagAll;
            cbNoiLayMauGoiY.DataValueField = TblHasTag.Columns.Id;
            cbNoiLayMauGoiY.DataTextField = TblHasTag.Columns.TenTag;
            cbNoiLayMauGoiY.DataBind();


            List<TblDonViTinh> donViTinhCol = CategoryManager.GetDonViTinhALL(true);
            cbDonViTinh.DataSource = donViTinhCol;
            cbDonViTinh.DataValueField = TblDonViTinh.Columns.Id;
            cbDonViTinh.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbDonViTinh.DataBind();

            List<TblDonViTinh> donViTinhCol2 = new List<TblDonViTinh>(donViTinhCol);
            cbUpdateDVT.DataSource = donViTinhCol2;
            cbUpdateDVT.DataValueField = TblDonViTinh.Columns.Id;
            cbUpdateDVT.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbUpdateDVT.DataBind();

            List<TblCatLoaiHinhYeuCau> loaiHinhYeuCauCol = CategoryManager.GetLoaiHinhYeuCauAll(true);
            cbLoaiHinhYeuCauMau.DataSource = loaiHinhYeuCauCol;
            cbLoaiHinhYeuCauMau.DataValueField = TblCatLoaiHinhYeuCau.Columns.Id;
            cbLoaiHinhYeuCauMau.DataTextField = TblCatLoaiHinhYeuCau.Columns.Ten;
            cbLoaiHinhYeuCauMau.DataBind();
        }

        private void Save(bool forceLoaiHinh = false)
        {
            DateTime ngayLayMau = DateTime.MinValue, ngayNhanMau = DateTime.MinValue, ngayTraKetQua = DateTime.MinValue;
            decimal dungLuong = 0, dungLuonghoa = 0;
            #region Validation
            if (string.IsNullOrEmpty(txtTenKhachHang.Text.Trim()))
            {
                AddValidPromt(txtTenKhachHang.ClientID, "Nhập tên khách hàng");
                txtTenKhachHang.Focus();
            }
            if (string.IsNullOrEmpty(txtDiaChiKhach.Text.Trim()))
            {
                AddValidPromt(txtDiaChiKhach.ClientID, "Nhập địa chỉ khách hàng");
                txtDiaChiKhach.Focus();
            }
            if (string.IsNullOrEmpty(txtTenNguoiGui.Text.Trim()))
            {
                AddValidPromt(txtTenNguoiGui.ClientID, "Nhập tên người gửi mẫu");
                txtTenNguoiGui.Focus();
            }
            if (string.IsNullOrEmpty(txtTenNguoiLayMau.Text.Trim()))
            {
                AddValidPromt(txtTenNguoiLayMau.ClientID, "Nhập tên người lấy mẫu");
                txtTenNguoiLayMau.Focus();
            }
            if (string.IsNullOrEmpty(txtTenNguoiNhanMau.Text.Trim()))
            {
                AddValidPromt(txtTenNguoiNhanMau.ClientID, "Nhập tên người nhận mẫu");
                txtTenNguoiNhanMau.Focus();
            }
            if (string.IsNullOrEmpty(txtNoiLayMau.Text.Trim()))
            {
                AddValidPromt(txtNoiLayMau.ClientID, "Nhập nơi lấy mẫu");
                txtNoiLayMau.Focus();
            }
            if (string.IsNullOrEmpty(txtDieuKienBaoQuan.Text.Trim()))
            {
                AddValidPromt(txtDieuKienBaoQuan.ClientID, "Nhập điều kiện bảo quản");

            }
            if (string.IsNullOrEmpty(txtMoTaMau.Text.Trim()))
            {
                AddValidPromt(txtMoTaMau.ClientID, "Nhập mô tả mẫu");
                txtMoTaMau.Focus();
            }

            if (!cbLoaiHinhYeuCauMau.HasValue)
            {
                AddValidPromt(cbLoaiHinhYeuCauMau.ClientID, "Chọn loại hình yêu cầu");
                cbLoaiHinhYeuCauMau.Focus();
            }

            if (!cbLoaiMau.HasValue)
            {
                AddValidPromt(cbLoaiMau.ClientID, "Chọn loại mẫu");
                cbLoaiMau.Focus();
            }

            if (!cbDonViTinh.HasValue)
            {
                AddValidPromt(cbDonViTinh.ClientID, "Chọn đơn vị tính");
                cbDonViTinh.Focus();
            }

            if (!cbQUyChuan.HasValue)
            {
                AddValidPromt(cbQUyChuan.ClientID, "Chọn quy chuẩn đánh giá");
                cbQUyChuan.Focus();
            }

            if (dtNgayLayMau.DateValue == DateTime.MinValue)
            {
                AddValidPromt(dtNgayLayMau.ClientID, "Chọn ngày lấy mẫu");
                dtNgayLayMau.Focus();
            }
            else ngayLayMau = dtNgayLayMau.DateValue;

            if (dateNgayNhanMau.DateValue == DateTime.MinValue)
            {
                AddValidPromt(dateNgayNhanMau.ClientID, "Chọn ngày nhận mẫu");
                dateNgayNhanMau.Focus();
            }
            else ngayNhanMau = dateNgayNhanMau.DateValue;

            if (dateNgayTraKetQua.DateValue == DateTime.MinValue)
            {
                AddValidPromt(dateNgayTraKetQua.ClientID, "Chọn ngày trả kết quả");
                dateNgayTraKetQua.Focus();
            }
            else ngayTraKetQua = dateNgayTraKetQua.DateValue;

            if (dateNgayChuyenMau.DateValue == DateTime.MinValue)
            {
                AddValidPromt(dateNgayChuyenMau.ClientID, "Chọn ngày chuyển mẫu");
                dateNgayChuyenMau.Focus();
            }
            else ngayTraKetQua = dateNgayTraKetQua.DateValue;

            if (string.IsNullOrEmpty(txtDungLuongMau.Text))
            {
                AddValidPromt(txtDungLuongMau.ClientID, "Nhập dung lượng mẫu");
                txtDungLuongMau.Focus();
            }
            else
            {
                if (!decimal.TryParse(txtDungLuongMau.Text.Trim().Replace(".", "").Replace(",", "."), out dungLuong) && dungLuong < 1)
                {
                    AddValidPromt(txtDungLuongMau.ClientID, "Nhập dung lượng mẫu");
                    txtDungLuongMau.Focus();
                }
                else if (decimal.TryParse(txtDungLuongMauHoa.Text.Trim().Replace(".", "").Replace(",", "."), out dungLuonghoa) && dungLuonghoa > dungLuong)
                {
                    AddValidPromt(txtDungLuongMauHoa.ClientID, "Lượng mẫu hóa không được vượt quá lượng tổng");
                    txtDungLuongMauHoa.Focus();
                }
            }


            #endregion

            if (PageIsValid)
            {
                TblMauXetNghiem mauXN = new TblMauXetNghiem();
                if (CurrentID.HasValue)
                {
                    mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null)
                    {
                        int khID = 0;
                        TblKhachHang khachHang = null;
                        #region khách hàng
                        //if (!string.IsNullOrEmpty(hiddenKhachHangID.Value))
                        //{
                        //    int.TryParse(hiddenKhachHangID.Value, out khID);
                        //    khachHang = XetNghiemManager.GetKhachHangByID(khID);
                        //    if (khachHang != null)
                        //    {
                        //        khachHang.TenKhachHang = txtTenKhachHang.Text;
                        //        khachHang.DiaChi = txtDiaChiKhach.Text;
                        //        khachHang.DienThoai = txtDienThoaiKhach.Text;
                        //        khachHang.Email = txtEmailKhach.Text;
                        //        khachHang = XetNghiemManager.UpdateKhachHang(khachHang);
                        //    }
                        //}
                        //else
                        //{
                        //    khachHang = new TblKhachHang();
                        //    khachHang.TenKhachHang = txtTenKhachHang.Text;
                        //    khachHang.DiaChi = txtDiaChiKhach.Text;
                        //    khachHang.DienThoai = txtDienThoaiKhach.Text;
                        //    khachHang.Email = txtEmailKhach.Text;
                        //    khachHang.CreatedBy = ApplicationContext.Current.CommonUser.Email;
                        //    khachHang = XetNghiemManager.InsertKhachHang(khachHang);
                        //    khID = khachHang.Id;
                        //    hiddenKhachHangID.Value = khID.ToString();
                        //}
                        #endregion
                        mauXN.IDKhachHang = khID;

                        int loaiHinhYeuCau = int.Parse(cbLoaiHinhYeuCauMau.SelectedValue);
                        if (loaiHinhYeuCau != mauXN.IDLoaiHinhYeuCauMau.Value)
                        {
                            TblCatLoaiHinhYeuCau loaiHinh = CategoryManager.GetLoaiHinhYeuCauByID(loaiHinhYeuCau);
                            TblCatLoaiHinhYeuCau loaiHinhOld = CategoryManager.GetLoaiHinhYeuCauByID(mauXN.IDLoaiHinhYeuCauMau.Value);
                            if (loaiHinh != null && loaiHinh != null && loaiHinh.Code != loaiHinhOld.Code)
                            {
                                if (!forceLoaiHinh)
                                {
                                    btnSubmitTrans.Visible = false;
                                    btnSubmitDuyetLabo.Visible = false;
                                    btnSubmitThayDoiLoaiHinhYeuCau.Visible = true;
                                    MessageBoxResult result = new MessageBoxResult();
                                    result.Value = mauXN;
                                    result.Submit = true;
                                    result.CommandName = "YeuCauThayDoi";
                                    CurrentMessageBoxResult = result;
                                    OpenMessageBox(string.Format("Loại hình yêu cầu bị thay đổi", CurrentName),
                                                   string.Format("Loại hình yêu cầu bị thay đổi có thể phát sinh mã phiếu mới, bạn có muốn tiếp tục ?"));
                                    return;
                                }
                                string rf2 = "";
                                mauXN.MaMau = XetNghiemManager.GetMauPhieuIndex(ref rf2, loaiHinhYeuCau);
                                mauXN.MaMauIndex = rf2;
                            }
                        }


                        mauXN.TenKhachHang = txtTenKhachHang.Text;
                        mauXN.DiaChiKhachHang = txtDiaChiKhach.Text;
                        mauXN.DienThoaiKhachHang = txtDienThoaiKhach.Text;
                        mauXN.EmailKhachHang = txtEmailKhach.Text;
                        mauXN.IDLoaiHinhYeuCauMau = loaiHinhYeuCau;
                        mauXN.TenNguoiGuiMau = txtTenNguoiGui.Text;
                        mauXN.DiaChiNguoiGuiMau = txtDiaChiNguoiGui.Text;
                        mauXN.DienThoaiNguoiGuiMau = txtDienThoaiNguoiGui.Text;
                        mauXN.EmailNguoiGuiMau = txtEmailNguoiGui.Text;
                        mauXN.IDNoiNhanMau = null;
                        if (cbNoiLayMauGoiY.HasValue)
                            mauXN.IDNoiNhanMau = int.Parse(cbNoiLayMauGoiY.SelectedValue);
                        mauXN.NhanMauLNV = chkNguoiNhanLaNV.Checked;
                        mauXN.LayMauLNV = chkNguoiLayLaNV.Checked;
                        mauXN.NguoiGuiLNV = chkNguoiGuiLaNV.Checked;
                        mauXN.LayMauNgoai = chkLayMauNgoai.Checked;
                        mauXN.MauTheoABCD = chkMauTheoABCD.Checked;
                        mauXN.TenNguoiLayMau = txtTenNguoiLayMau.Text;
                        mauXN.DienThoaiNguoiLayMau = txtDienThoaiNguoiLayMau.Text;
                        mauXN.EmailNguoiLayMau = txtEmailNguoiLayMau.Text;

                        mauXN.TenNguoiNhanMau = txtTenNguoiNhanMau.Text;
                        mauXN.DienThoaiNguoiNhanMau = txtDienThoaiNguoiNhan.Text;
                        mauXN.EmailNguoiNhanMau = txtEmailNguoiNhanMau.Text;

                        mauXN.NoiLayMau = txtNoiLayMau.Text;
                        mauXN.GhiChuMau = txtGhiChuMau.Text;
                        mauXN.DieuKienBaoQuan = txtDieuKienBaoQuan.Text;
                        mauXN.MoTaMau = txtMoTaMau.Text;
                        mauXN.YeuCauKhac = txtYeuCauKhac.Text;

                        mauXN.IDLoaiMau = int.Parse(cbLoaiMau.SelectedValue);
                        mauXN.IDQuyChuan = int.Parse(cbQUyChuan.SelectedValue);

                        mauXN.NgayLayMau = ngayLayMau;
                        mauXN.NgayNhanMau = ngayNhanMau;
                        mauXN.NgayTraKetQua = ngayTraKetQua;

                        mauXN.LuongMau = dungLuong;
                        mauXN.LuongMauHoa = dungLuonghoa;
                        mauXN.IDDonViLuongMau = int.Parse(cbDonViTinh.SelectedValue);

                        mauXN.TrangThaiMau = cbTrangThaiMau.SelectedValue;
                        if (dateNgayTaoPhieu.DateValue != DateTime.MinValue)
                            mauXN.CreatedDate = mauXN.UpdatedDate = dateNgayTaoPhieu.DateValue;

                        if (dateNgayChuyenMau.DateValue != DateTime.MinValue)
                            mauXN.NgayChuyenLabo = dateNgayChuyenMau.DateValue;
                        mauXN.DanhGiaMau = chkDanhGiaMau.Checked;

                        if (false && mauXN.LuongMauHoa.HasValue && mauXN.LuongMauHoa.Value > 0 && string.IsNullOrEmpty(mauXN.MaSoTraHoaLy))
                        {
                            string codeIndex = "";
                            mauXN.MaSoTraHoaLy = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex);
                            mauXN.MaSoTraHoaLyIndex = codeIndex;
                        }


                        if (false && mauXN.LuongMau.HasValue && mauXN.LuongMauHoa.HasValue && (mauXN.LuongMau.Value - mauXN.LuongMauHoa.Value) > 0 && string.IsNullOrEmpty(mauXN.MaSoTraViSinh))
                        {
                            string codeIndex = "";
                            mauXN.MaSoTraViSinh = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraViSinhIndex);
                            mauXN.MaSoTraViSinhIndex = codeIndex;
                        }

                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                        CloseMessageBox();
                        ListItem it = cbTrangThaiMau.Items.FindByValue(mauXN.TrangThaiMau);
                        ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>{1}</b>", mauXN.MaMau, it.Text);
                        //if ((CurrentPageType == "LaboHoaLy" && mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value != DateTime.MinValue) ||
                        //    (CurrentPageType == "LaboViSinh" && mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value != DateTime.MinValue))
                        //    ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>chờ trả kết quả</b>", mauXN.MaMau, it.Text);

                        ShowNotify("Thông báo", "Cập nhật thành công mẫu phiếu xét nghiệm", NotifyType.Success);
                    }
                    else
                        ShowNotify("Thông báo", "Không thấy thông tin mẫu xét nghiệm", NotifyType.Error);
                }
                else
                {
                    mauXN = new TblMauXetNghiem();
                    int khID = 0;
                    TblKhachHang khachHang = null;
                    #region khách hàng
                    //if (!string.IsNullOrEmpty(hiddenKhachHangID.Value))
                    //{
                    //    int.TryParse(hiddenKhachHangID.Value, out khID);
                    //    khachHang = XetNghiemManager.GetKhachHangByID(khID);
                    //    if (khachHang != null)
                    //    {
                    //        khachHang.TenKhachHang = txtTenKhachHang.Text;
                    //        khachHang.DiaChi = txtDiaChiKhach.Text;
                    //        khachHang.DienThoai = txtDienThoaiKhach.Text;
                    //        khachHang.Email = txtEmailKhach.Text;
                    //        khachHang = XetNghiemManager.UpdateKhachHang(khachHang);
                    //    }
                    //}
                    //else
                    //{
                    //    khachHang = new TblKhachHang();
                    //    khachHang.TenKhachHang = txtTenKhachHang.Text;
                    //    khachHang.DiaChi = txtDiaChiKhach.Text;
                    //    khachHang.DienThoai = txtDienThoaiKhach.Text;
                    //    khachHang.Email = txtEmailKhach.Text;
                    //    khachHang.CreatedBy = ApplicationContext.Current.CommonUser.Email;
                    //    khachHang = XetNghiemManager.InsertKhachHang(khachHang);
                    //    khID = khachHang.Id;
                    //    hiddenKhachHangID.Value = khID.ToString();
                    //}
                    #endregion

                    mauXN.IDLoaiHinhYeuCauMau = int.Parse(cbLoaiHinhYeuCauMau.SelectedValue);

                    string rf2 = "";
                    mauXN.MaMau = XetNghiemManager.GetMauPhieuIndex(ref rf2, mauXN.IDLoaiHinhYeuCauMau.Value);
                    mauXN.MaMauIndex = rf2;

                    mauXN.TenKhachHang = txtTenKhachHang.Text;
                    mauXN.DiaChiKhachHang = txtDiaChiKhach.Text;
                    mauXN.DienThoaiKhachHang = txtDienThoaiKhach.Text;
                    mauXN.EmailKhachHang = txtEmailKhach.Text;

                    //mauXN.IDKhachHang = khID;
                    mauXN.TenNguoiGuiMau = txtTenNguoiGui.Text;
                    mauXN.DiaChiNguoiGuiMau = txtDiaChiNguoiGui.Text;
                    mauXN.DienThoaiNguoiGuiMau = txtDienThoaiNguoiGui.Text;
                    mauXN.EmailNguoiGuiMau = txtEmailNguoiGui.Text;
                    mauXN.LayMauNgoai = chkLayMauNgoai.Checked;
                    mauXN.MauTheoABCD = chkMauTheoABCD.Checked;
                    mauXN.IDNoiNhanMau = null;
                    if (cbNoiLayMauGoiY.HasValue)
                        mauXN.IDNoiNhanMau = int.Parse(cbNoiLayMauGoiY.SelectedValue);

                    mauXN.NhanMauLNV = chkNguoiNhanLaNV.Checked;
                    mauXN.LayMauLNV = chkNguoiLayLaNV.Checked;
                    mauXN.NguoiGuiLNV = chkNguoiGuiLaNV.Checked;

                    mauXN.TenNguoiLayMau = txtTenNguoiLayMau.Text;
                    mauXN.DienThoaiNguoiLayMau = txtDienThoaiNguoiLayMau.Text;
                    mauXN.EmailNguoiLayMau = txtEmailNguoiLayMau.Text;

                    mauXN.TenNguoiNhanMau = txtTenNguoiNhanMau.Text;
                    mauXN.DienThoaiNguoiNhanMau = txtDienThoaiNguoiNhan.Text;
                    mauXN.EmailNguoiNhanMau = txtEmailNguoiNhanMau.Text;

                    mauXN.NoiLayMau = txtNoiLayMau.Text;
                    mauXN.GhiChuMau = txtGhiChuMau.Text;
                    mauXN.DieuKienBaoQuan = txtDieuKienBaoQuan.Text;
                    mauXN.MoTaMau = txtMoTaMau.Text;
                    mauXN.YeuCauKhac = txtYeuCauKhac.Text;

                    mauXN.IDLoaiMau = int.Parse(cbLoaiMau.SelectedValue);
                    mauXN.IDQuyChuan = int.Parse(cbQUyChuan.SelectedValue);

                    mauXN.NgayLayMau = ngayLayMau;
                    mauXN.NgayNhanMau = ngayNhanMau;
                    mauXN.NgayTraKetQua = ngayTraKetQua;

                    mauXN.LuongMau = dungLuong;
                    mauXN.LuongMauHoa = dungLuonghoa;
                    mauXN.IDDonViLuongMau = int.Parse(cbDonViTinh.SelectedValue);

                    mauXN.TrangThaiMau = "New";
                    mauXN.CreatedDate = mauXN.UpdatedDate = DateTime.Now;
                    if (dateNgayTaoPhieu.DateValue != DateTime.MinValue)
                        mauXN.CreatedDate = mauXN.UpdatedDate = dateNgayTaoPhieu.DateValue;
                    if (dateNgayChuyenMau.DateValue != DateTime.MinValue)
                        mauXN.NgayChuyenLabo = dateNgayChuyenMau.DateValue;
                    mauXN.DanhGiaMau = chkDanhGiaMau.Checked;

                    mauXN.CreatedBy = mauXN.UpdatedBy = ApplicationContext.Current.CommonUser.Email;
                    //mauXN.MaMau = XetNghiemManager.GetMa();
                    if (false && mauXN.LuongMauHoa.HasValue && mauXN.LuongMauHoa.Value > 0 && string.IsNullOrEmpty(mauXN.MaSoTraHoaLy))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraHoaLy = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex);
                        mauXN.MaSoTraHoaLyIndex = codeIndex;
                    }

                    if (false && mauXN.LuongMau.HasValue && mauXN.LuongMauHoa.HasValue && (mauXN.LuongMau.Value - mauXN.LuongMauHoa.Value) > 0 && string.IsNullOrEmpty(mauXN.MaSoTraViSinh))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraViSinh = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraViSinhIndex);
                        mauXN.MaSoTraViSinhIndex = codeIndex;
                    }

                    mauXN = XetNghiemManager.InsertMauXetNghiem(mauXN);
                    Session["MauXetNghiem_CreateNew"] = mauXN;
                    Response.Redirect(string.Format("/mau-xet-nghiem/{0}", mauXN.Id));
                }
            }

            if (!PageIsValid) ShowValidPromt();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect(@"\mau-xet-nghiem\0");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Save();

        }

        protected string GetNhom(object data)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu ct = data as TblMauXetNghiemChiTieu;
                if (!string.IsNullOrEmpty(ct.TenNhomXetNghiem))
                {
                    rs += string.Format(" <span class='span-item'>&raquo;</span><span class='span-item'><b>{0}</b></span>", ct.TenNhomXetNghiem);
                }
            }
            return rs;
        }



        protected void btnThemChiTieu_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddChiTieuSingle_Click(object sender, EventArgs e)
        {
            if (hiddenCheckSearch.Value == "ChiTieu")
            {
                if (!string.IsNullOrEmpty(hiddenChiTieuPhuongPhapAddID.Value))
                {
                    TblChiTieuPhuongPhap ctpp = XetNghiemManager.GetChiTieuPhuongPhapByID(hiddenChiTieuPhuongPhapAddID.Value);
                    if (ctpp != null)
                    {
                        List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                        //TblMauXetNghiemChiTieu ct_old = chiTieuCol.FirstOrDefault(p => p.IDChiTieu == ctpp.IDChiTieu);
                        TblMauXetNghiemChiTieu ct_old = chiTieuCol.FirstOrDefault(p => p.Idctpp == ctpp.Id);
                        if (ct_old == null)
                        {

                            TblMauXetNghiemChiTieu mxnCT = new TblMauXetNghiemChiTieu();
                            mxnCT.IDMau = CurrentID.Value;
                            mxnCT.IDChiTieu = ctpp.IDChiTieu;
                            mxnCT.IDPhuongPhap = ctpp.IDPhuongPhap;
                            mxnCT.Idctpp = ctpp.Id;
                            string tenDonViNA = ChiTieuManager.GetTenDonViTinhNA(ctpp.IDChiTieu.Value, CurrentID.Value);
                            mxnCT.TenDonViNA = tenDonViNA;
                            mxnCT = XetNghiemManager.InsertMauChiTieu(mxnCT);
                            ShowNotify("Thông báo", "Thêm mới chỉ tiêu vào mẫu thành công", NotifyType.Success);
                            hiddenChiTieuPhuongPhapAddID.Value = "";
                            LoadMauTieuChi();
                            txtAddTenChiTieu.Text = "";
                            txtAddTenChiTieu.Focus();
                        }
                        else
                        {
                            txtAddTenChiTieu.Focus();

                            ShowNotify("Thông báo", "Đã tồn tại chỉ tiêu này trong mẫu. Vui lòng chọn chỉ tiêu khác", NotifyType.Warning);
                        }
                    }
                    else
                        ShowNotify("Thông báo", "Hệ thống không trích xuất được thông tin bạn chọn. Vui lòng thử lại", NotifyType.Error);
                }
                else
                    ShowNotify("Thông báo", "Hệ thống không trích xuất được thông tin bạn chọn. Vui lòng thử lại", NotifyType.Error);
            }
            else
            {
                TblCatNhomXetNghiem nhomXN = CategoryManager.GetNhomXetNghiemByID(hiddenChiTieuPhuongPhapAddID.Value);
                if (nhomXN != null)
                {
                    List<TblChiTieuPhuongPhap> chiTieuCol = XetNghiemManager.GetChiTieuPhuongPhapByNhomXNID(hiddenChiTieuPhuongPhapAddID.Value);
                    repDataConfig.DataSource = chiTieuCol;
                    repDataConfig.DataBind();
                    ltrDialogSearchTitle.Text = string.Format("Danh sách chỉ tiêu trong nhóm <b>{0}</b>", nhomXN.Ten);
                    dialogConfig.ShowDialog();
                    hiddenNhomXNAddID.Value = nhomXN.Id.ToString();
                }
            }
        }
        protected void btnGoKetQua_Click(object sender, EventArgs e)
        {
            int count = 0, error = 0;
            foreach (RepeaterItem item in repUpdateChiTieu.Items)
            {
                HiddenField hiddenItemID = item.FindControl("hiddenMCTID") as HiddenField;
                ExtraCheckBox chkItem = item.FindControl("chkItem") as ExtraCheckBox;
                if (hiddenItemID != null && chkItem != null && chkItem.Checked)
                {
                    if (hiddenItemID.Value != "0")
                    {
                        TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemID.Value);
                        if (chiTieu != null)
                        {
                            if (!string.IsNullOrEmpty(chiTieu.KetQuaHienThi))
                            {
                                XetNghiemManager.GoKetQuaChiTieu(hiddenItemID.Value);
                                count++;
                            }
                        }

                    }
                }
            }
            LoadMauTieuChiUpdate(null);
            ShowNotify("Thông báo", string.Format("Đã gỡ duyệt <b>{0}</b> chỉ tiêu.", count), NotifyType.Success);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int count = 0, error = 0;
            foreach (RepeaterItem item in repDataFinal.Items)
            {
                HiddenField hiddenItemID = item.FindControl("hiddenItemID") as HiddenField;
                ExtraCheckBox chkItem = item.FindControl("chkItem") as ExtraCheckBox;
                if (hiddenItemID != null && chkItem != null && chkItem.Checked)
                {
                    if (hiddenItemID.Value != "0")
                    {
                        TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemID.Value);
                        if (chiTieu != null)
                        {
                            if (string.IsNullOrEmpty(chiTieu.KetQuaHienThi))
                            {
                                XetNghiemManager.DeleteMauChiTieu(hiddenItemID.Value);
                                count++;
                            }
                            else
                                error++;
                        }

                    }
                }
                Repeater repDataFinalSub = item.FindControl("repDataFinalSub") as Repeater;
                if (repDataFinalSub != null)
                {
                    foreach (RepeaterItem item2 in repDataFinalSub.Items)
                    {
                        HiddenField hiddenItemSubID = item2.FindControl("hiddenItemSubID") as HiddenField;
                        ExtraCheckBox chkItemSub = item2.FindControl("chkItemSub") as ExtraCheckBox;
                        if (hiddenItemSubID != null && chkItemSub != null && chkItemSub.Checked)
                        {
                            if (hiddenItemSubID.Value != "0")
                            {
                                TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemSubID.Value);
                                if (chiTieu != null)
                                {
                                    if (string.IsNullOrEmpty(chiTieu.KetQuaHienThi))
                                    {
                                        XetNghiemManager.DeleteMauChiTieu(hiddenItemSubID.Value);
                                        count++;
                                    }
                                    else
                                        error++;
                                }
                            }
                        }
                    }
                }
            }
            LoadMauTieuChi();
            if (error == 0)
                ShowNotify("Thông báo", string.Format("Đã xóa <b>{0}</b> chỉ tiêu ra khỏi mẫu.", count), NotifyType.Success);
            else
                ShowNotify("Thông báo", string.Format("Đã xóa <b>{0}</b> chỉ tiêu ra khỏi mẫu. Có <b>{1}</b> chỉ tiêu không thể xóa vì đã nhập kết quả.", count, error), NotifyType.Warning);
        }

        protected void chkCheckSearh_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCheckSearh.Checked)
            {
                hiddenCheckSearch.Value = "Nhom";
                chkCheckSearh.Text = "Bỏ tick chọn để tìm theo tên chỉ tiêu";
                txtAddTenChiTieu.PlaceHolder = "Nhập tên nhóm cần tìm và chọn nhóm để thêm các chỉ tiêu vào danh sách";
            }
            else
            {
                hiddenCheckSearch.Value = "ChiTieu";
                chkCheckSearh.Text = "Tick chọn để tìm theo tên nhóm xét nghiệm";
                txtAddTenChiTieu.PlaceHolder = "Nhập tên chỉ tiêu cần tìm và chọn chỉ tiêu thêm vào danh sách";
            }
            txtAddTenChiTieu.Focus();
        }

        private void LoadMauTieuChiUpdate(TblMauXetNghiem mauXN)
        {
            if (CurrentID.HasValue)
            {
                if (mauXN == null) mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                if (mauXN != null)
                {
                    List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                    if (chiTieuCol.Count > 0)
                    {
                        columnCount = chiTieuCol.Max(p => p.MaxKetQua.HasValue ? p.MaxKetQua.Value : 0);
                        #region Update yêu cầu bổ sung giới hạn cho phép 04/06/2023
                        if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                        {
                            ltrColumnHeaderGroup.Text = string.Format("<td colspan='{0}' {1} style='text-align:center;font-weight:bold;min-width:150px;'>Giới hạn cho phép <sup>(*)</sup></td>", columnCount, columnCount == 1 ? " rowspan='2' " : "");
                            if (columnCount > 0)
                            {
                                string[] arr = { "A", "B", "C", "D" };
                                if (mauXN.MauTheoABCD.HasValue && !mauXN.MauTheoABCD.Value) arr = new string[] { "A1", "A2", "B1", "B2" };
                                ltrColumnHeader.Text = "";
                                if (columnCount > 1)
                                {
                                    for (int i = 0; i < columnCount; i++)
                                        ltrColumnHeader.Text += string.Format("<td style='text-align:center;width:50px;'>{0}</td>", arr[i]);
                                }
                            }
                        }
                        #endregion



                        List<TblMauXetNghiemChiTieu> chiTieuSub = chiTieuCol;
                        if (CurrentPageType == "LaboHoaLy")
                            chiTieuSub = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy");
                        if (CurrentPageType == "LaboViSinh")
                            chiTieuSub = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh");
                        List<TblChiTieuQuyChuan> chiTieuQuyChuan = ChiTieuManager.GetChiTieuQuyChuanByQuyChuanID(mauXN.IDQuyChuan);
                        Session["ChiTieuQuyChuan_Temp"] = chiTieuQuyChuan;
                        bool b = (chiTieuSub.FindAll(p => string.IsNullOrEmpty(p.KetQuaHienThi)).Count == 0);

                        btnDuyetLabo.Visible = GetButtonRole("Approve") && b;

                        #region Build giới hạn cho phép
                        //if (mauXN.DanhGiaMau.HasValue && mauXN.DanhGiaMau.Value)
                        //{
                        //    ltrHeaderGioiHanChoPhep.Text = string.Format("<td colspan='{0}' {1} style='text-align:center;font-weight:bold;min-width:150px;'>Giới hạn cho phép <sup>(*)</sup></td>", columnCount, columnCount == 1 ? " rowspan='2' " : "");
                        //    if (columnCount > 0)
                        //    {
                        //        string[] arr = { "A", "B", "C", "D" };
                        //        if (mauXN.MauTheoABCD.HasValue && !mauXN.MauTheoABCD.Value) arr = new string[] { "A1", "A2", "B1", "B2" };
                        //        ltrColumnHeader.Text = "";
                        //        if (columnCount > 1)
                        //        {
                        //            for (int i = 0; i < columnCount; i++)
                        //                ltrColumnHeader.Text += string.Format("<th style='text-align:center;width:50px;'>{0}</th>", arr[i]);
                        //        }
                        //    }


                        //    ltrFooter.Text += string.Format("(*): Giới hạn tối đa cho phép theo {0} ({1});<br />", mauXN.MieuTaQuyChuan, mauXN.TenQuyChuan);
                        //}
                        #endregion

                        repUpdateChiTieu.DataSource = chiTieuSub;
                        repUpdateChiTieu.DataBind();
                    }
                }
            }
        }
        private void LoadMauTieuChi()
        {
            if (CurrentID.HasValue)
            {
                List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                foreach (TblMauXetNghiemChiTieu ct in chiTieuCol) ct.ECT = "";
                List<TblMauXetNghiemChiTieu> groupByTitleCol = chiTieuCol
                .GroupBy(c => new
                {

                    c.MappingTitle,
                    c.MappingDonGia,
                    c.TenNhomXetNghiem,
                    c.MappingDonGiaNhomToString,
                    c.IDNhomXetNghiem
                })
                .Select(gcs => new TblMauXetNghiemChiTieu()
                {
                    MappingTitle = gcs.Key.MappingTitle,
                    MappingDonGia = gcs.Key.MappingDonGia,
                    TenNhomXetNghiem = gcs.Key.TenNhomXetNghiem,
                    MappingDonGiaNhomToString = gcs.Key.MappingDonGiaNhomToString,
                    IDNhomXetNghiem = gcs.Key.IDNhomXetNghiem,
                    Children = gcs.ToList()
                }).ToList<TblMauXetNghiemChiTieu>();
                repDataFinal.DataSource = groupByTitleCol;
                repDataFinal.DataBind();

                decimal tongTien = groupByTitleCol.Sum(p => p.MappingDonGia);
                ltrTongTien.Text = tongTien.ToStringWithLanguage() + " đ";
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                mauXN.TongTien = tongTien;
                mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                int hoa = chiTieuCol.FindAll(p => p.HoaSinh == "HoaLy").Count, vi = chiTieuCol.FindAll(p => p.HoaSinh == "ViSinh").Count;
                ltrHoaSinh.Text = string.Format("<b>{0}</b> hóa sinh / <b>{1}</b> vi sinh ", hoa, vi);
            }
        }
        protected void btnAddConfig_Click(object sender, EventArgs e)
        {
            int total = 0, success = 0;
            List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
            foreach (RepeaterItem item in repDataConfig.Items)
            {
                HiddenField hiddenItemID = item.FindControl("hiddenItemID") as HiddenField;
                ExtraCheckBox chkitem = item.FindControl("chkitem") as ExtraCheckBox;
                if (hiddenItemID != null && chkitem != null && chkitem.Checked)
                {
                    total++;
                    TblChiTieuPhuongPhap ctpp = XetNghiemManager.GetChiTieuPhuongPhapByID(hiddenItemID.Value);
                    if (ctpp != null)
                    {

                        //TblMauXetNghiemChiTieu ct_old = chiTieuCol.FirstOrDefault(p => p.IDChiTieu == ctpp.IDChiTieu);
                        TblMauXetNghiemChiTieu ct_old = chiTieuCol.FirstOrDefault(p => p.Id == ctpp.Id);
                        if (ct_old == null)
                        {
                            TblMauXetNghiemChiTieu mxnCT = new TblMauXetNghiemChiTieu();
                            mxnCT.IDMau = CurrentID.Value;
                            mxnCT.IDChiTieu = ctpp.IDChiTieu;
                            mxnCT.IDPhuongPhap = ctpp.IDPhuongPhap;
                            mxnCT.Idctpp = ctpp.Id;
                            mxnCT.IDNhomAdd = int.Parse(hiddenNhomXNAddID.Value);
                            string tenDonViNA = ChiTieuManager.GetTenDonViTinhNA(ctpp.IDChiTieu.Value, CurrentID.Value);
                            mxnCT.TenDonViNA = tenDonViNA;
                            mxnCT = XetNghiemManager.InsertMauChiTieu(mxnCT);
                            chiTieuCol.Add(mxnCT);
                            success++;
                        }
                    }
                }
            }

            if (total > 0)
            {
                if (success > 0)
                {
                    hiddenChiTieuPhuongPhapAddID.Value = "";
                    LoadMauTieuChi();
                    txtAddTenChiTieu.Text = "";
                    txtAddTenChiTieu.Focus();
                    ShowNotify("Thông báo", string.Format("Đã thêm <b>{0}</b> chỉ tiêu xét nghiệm vào mẫu.", success), NotifyType.Success);
                    dialogConfig.CloseDialog();
                    LoadMauTieuChi();
                }
                else
                    ShowNotify("Thông báo", string.Format("Các chỉ tiêu xét nghiệm đã có sẵn trong mẫu, vui lòng chọn lại chỉ tiêu.", success), NotifyType.Error);
            }
            else
                ShowNotify("Thông báo", "Vui lòng chọn chỉ tiêu để thêm vào danh sách", NotifyType.Warning);


        }

        protected void repDataFinal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item ||
               e.Item.ItemType == ListItemType.AlternatingItem) &&
               e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu)
            {
                Literal LtrTRBegin = e.Item.FindControl("LtrTRBegin") as Literal;
                Literal LtrTREnd = e.Item.FindControl("LtrTREnd") as Literal;
                Literal ltrContent = e.Item.FindControl("ltrContent") as Literal;
                ExtraCheckBox chkItem = e.Item.FindControl("chkItem") as ExtraCheckBox;


                Repeater repDataFinalSub = e.Item.FindControl("repDataFinalSub") as Repeater;
                HiddenField hiddenItemID = e.Item.FindControl("hiddenItemID") as HiddenField;

                TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
                chiTieu.ECT = "";
                string tr = "<tr><td style='width:90%'>";
                string content = string.Format("{0}</td><td class='price'>{1} đ</td>", chiTieu.MappingTitle.Replace("<br />", "").Replace("<br>", ""), chiTieu.MappingDonGiaNhomToString);
                if (!string.IsNullOrEmpty(chiTieu.TenNhomXetNghiem))
                {
                    hiddenItemID.Value = "0";
                    chkItem.EnableClickGroup = true;
                    chkItem.ValidationGroup = "gr-" + chiTieu.IDNhomXetNghiem.Value;
                    chkItem.GroupName = "gr-" + chiTieu.IDNhomXetNghiem.Value;
                    tr = "<tr class='tr-group-title'><td colspan='2'>";
                    content = string.Format("{0}</td>", chiTieu.MappingTitle.Replace("<br />", "").Replace("<br>", ""));
                    repDataFinalSub.DataSource = chiTieu.Children;
                    repDataFinalSub.DataBind();
                }
                else hiddenItemID.Value = chiTieu.Children[0].Id.ToString();

                LtrTRBegin.Text = tr;
                ltrContent.Text = content;
                LtrTREnd.Text = "</tr>";

            }
        }

        protected void repDataFinalSub_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item ||
              e.Item.ItemType == ListItemType.AlternatingItem) &&
              e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu)
            {
                Literal LtrTRBegin = e.Item.FindControl("LtrTRBegin") as Literal;
                Literal LtrTREnd = e.Item.FindControl("LtrTREnd") as Literal;
                Literal ltrContent = e.Item.FindControl("ltrContent") as Literal;
                ExtraCheckBox chkItem = e.Item.FindControl("chkItemSub") as ExtraCheckBox;
                Repeater repDataFinalSub = e.Item.FindControl("repDataFinalSub") as Repeater;
                HiddenField hiddenItemID = e.Item.FindControl("hiddenItemSubID") as HiddenField;

                TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
                chiTieu.ECT = "";
                string tr = "<tr><td>";
                string content = string.Format("<span class='title'>|- {0}</span><span class='sub-title'>[ {1} ]</span></td>", chiTieu.TenTaiCacKhoa, chiTieu.TenPhuongPhap.Replace("<br>", ""));
                if (chiTieu.RowSpan.HasValue && chiTieu.RowSpan.Value > 0)
                {
                    content += string.Format("<td rowspan='{0}' class='price'>{1} đ</td>", chiTieu.RowSpan.Value, chiTieu.MappingDonGiaNhomToString);
                }
                hiddenItemID.Value = chiTieu.Id.ToString();
                chkItem.CssClass += " gr-" + chiTieu.IDNhomXetNghiem.Value;
                LtrTRBegin.Text = tr;
                ltrContent.Text = content;
                LtrTREnd.Text = "</tr>";
            }
        }

        protected void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCopyMaMau.Text))
            {
                TblMauXetNghiem mauOld = XetNghiemManager.GetMauXetNghiemByCode(txtCopyMaMau.Text.Trim());
                if (mauOld != null)
                {

                    TblMauXetNghiem mauXN = new TblMauXetNghiem();
                    mauOld.CopyTo(mauXN);
                    string pCode = "NA0";
                    string rf = "";

                    mauXN.MaMau = XetNghiemManager.GetMauPhieuIndex(ref rf, mauXN.IDLoaiHinhYeuCauMau.Value);
                    mauXN.MaMauIndex = rf;
                    mauXN.TrangThaiMau = "New";
                    mauXN.MaSoTraHoaLy = mauXN.MaSoTraHoaLyIndex = mauXN.MaSoTraViSinh = mauXN.MaSoTraViSinhIndex = string.Empty;
                    mauXN.NgayThanhLyThucTeHoa = mauXN.NgayThanhLyThucTeVS = mauXN.NgayChuyenLabo = mauXN.NgayHoaLy = mauXN.NgayKetQua = mauXN.NgayViSinh =
                         mauXN.NgayTraKetQuaThucTe = mauXN.NgayInPhieuTiepNhan = mauXN.NgayThanhLyDuKien = mauXN.NgayThanhLyThucTe = mauXN.NgayInKetQua = mauXN.NgayTaoSoTraMau =
                         mauXN.ThoiGianHoaBatDau = mauXN.ThoiGianHoaKetThuc = mauXN.ThoiGianVSBatDau = mauXN.ThoiGianVSKetThuc = null;
                    mauXN.GhiChuKetQua = mauXN.NguoiDanhKetQua =
                        mauXN.KetQuaDanhGiaMau = mauXN.NguoiTraKetQuaThucTe = mauXN.NguoiChuyenLabo = null;
                    mauXN.LuongMau = mauXN.LuongMauHoa = 0;
                    mauXN.ThuTienDiaChi = mauXN.ThuTienDienThoai = mauXN.ThuTienDonVi = mauXN.ThuTienHoVaTen = mauXN.ThuTienMaSoThue = mauXN.ThuTienNguoiThu = null;
                    mauXN.ThuTienNgayThu = null;
                    mauXN.CreatedBy = mauXN.UpdatedBy = ApplicationContext.Current.CommonUser.HoVaTen;

                    mauXN.DanhGiaDatMau = mauXN.NguoiDanhKetQua = mauXN.LaboHoaNhanXet = mauXN.LaboSinhNhanXet =
                        mauXN.SoHoaDon = mauXN.GhiChuKetQua = mauXN.MaSoTraKQ = mauXN.MaSoTraKQIndex = string.Empty;
                    mauXN.DanhGiaMau = mauXN.LayMauLNV = mauXN.LayMauNgoai = mauXN.MauTheoABCD = null;

                    mauXN.GhiNo = false;


                    mauXN.CreatedDate = mauXN.UpdatedDate = DateTime.Now;
                    if (false && mauXN.LuongMauHoa.HasValue && mauXN.LuongMauHoa.Value > 0 && string.IsNullOrEmpty(mauXN.MaSoTraHoaLy))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraHoaLy = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex);
                        mauXN.MaSoTraHoaLyIndex = codeIndex;
                    }

                    if (false && mauXN.LuongMau.HasValue && mauXN.LuongMauHoa.HasValue && (mauXN.LuongMau.Value - mauXN.LuongMauHoa.Value) > 0 && string.IsNullOrEmpty(mauXN.MaSoTraViSinh))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraViSinh = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraViSinhIndex);
                        mauXN.MaSoTraViSinhIndex = codeIndex;
                    }

                    mauXN = XetNghiemManager.InsertMauXetNghiem(mauXN);

                    List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(mauOld.Id, "", false, "asc");
                    foreach (TblMauXetNghiemChiTieu chiTieu in chiTieuCol)
                    {
                        TblMauXetNghiemChiTieu ct = new TblMauXetNghiemChiTieu();
                        chiTieu.CopyTo(ct);
                        ct.IDMau = mauXN.Id;
                        ct.KetQuaGiaTri = null;
                        ct.KetQuaGioiHan = null;
                        ct.DanhGia = null;
                        ct.TuDanhGia = null;
                        ct.NguoiDanhGia = null;
                        ct.NgayDanhGia = null;
                        ct.Idctqc = null;
                        ct.GiaTriSoSanhKetQua = null;
                        ct.KetQuaHienThi = null;
                        ct = XetNghiemManager.InsertMauChiTieu(ct);
                    }

                    Session["Insert_MauXN_ChiTieu"] = "0";
                    Response.Redirect(string.Format(@"\mau-xet-nghiem\{0}", mauXN.Id));
                }
                else
                {
                    AddValidPromt(txtCopyMaMau.ClientID, string.Format("Không tồn tại mẫu xét nghiệm với mã <b>{0}</b>", txtCopyMaMau.Text));
                    txtCopyMaMau.Focus();
                    ShowValidPromt();
                }
            }
            else
            {
                AddValidPromt(txtCopyMaMau.ClientID, "Nhập mã mẫu xét nghiệm cần copy");
                txtCopyMaMau.Focus();
                ShowValidPromt();
            }
        }

        protected void lnkPrint_Onclick(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                string cmdA = btn.CommandArgument;
                if (cmdA == "1")
                {
                    lnkPhieuDeNghiThuTien.Visible = true;
                    string url = string.Format("/phieu-tiep-nhan-yeu-cau-so-{0}", CurrentID.Value);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }

                if (cmdA == "2")
                {
                    string url = string.Format("/phieu-chuyen-mau-so-{0}", CurrentID.Value);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }
                if (cmdA == "3")
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null && (
                         (CurrentPageType == "LaboHoaLy" && string.IsNullOrEmpty(mauXN.MaSoTraHoaLy)) ||
                         (CurrentPageType != "LaboHoaLy" && string.IsNullOrEmpty(mauXN.MaSoTraViSinh))
                        )
                     )
                    {
                        string codeIndex = "";
                        if (false && CurrentPageType == "LaboHoaLy" && string.IsNullOrEmpty(mauXN.MaSoTraHoaLy))
                        {
                            mauXN.MaSoTraHoaLy = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex);
                            mauXN.MaSoTraHoaLyIndex = codeIndex;
                        }
                        else if (false && CurrentPageType == "LaboViSinh" && string.IsNullOrEmpty(mauXN.MaSoTraViSinh))
                        {
                            mauXN.MaSoTraViSinh = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraViSinhIndex);
                            mauXN.MaSoTraViSinhIndex = codeIndex;
                        }
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }
                    string url = string.Format("/phieu-tra-ket-qua-noi-bo-{1}-so-{0}", CurrentID.Value, CurrentPageType == "LaboHoaLy" ? "hoa" : "vs");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }

                if (cmdA == "4")
                {
                    string cmdN = btn.CommandName;
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null && string.IsNullOrEmpty(mauXN.MaSoTraKQ))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraKQ = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraKQIndex);
                        mauXN.MaSoTraKQIndex = codeIndex;
                        //hiddenMaTraKetQuaIndex.Value = codeIndex;
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }

                    string url = string.Format("/phieu-tra-ket-qua-so-{0}{1}", CurrentID.Value, !string.IsNullOrEmpty(cmdN) ? string.Format("-{0}", cmdN) : "");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }
            }
        }

        protected void btnChuyenLabo_Click(object sender, EventArgs e)
        {
            if (CurrentID.HasValue)
            {

                bool resultDelete = false;
                LinkButton btn = sender as LinkButton;
                if (btn != null)
                {
                    string cmdName = btn.CommandName;
                    if (cmdName == "trans_confirm")
                    {
                        if (CurrentMessageBoxResult.Submit && CurrentMessageBoxResult.Value != null)
                        {
                            if (CurrentMessageBoxResult.CommandName == "trans_confirm")
                            {
                                TblMauXetNghiem mauXN = CurrentMessageBoxResult.Value as TblMauXetNghiem;
                                if (mauXN != null)
                                {
                                    mauXN.ThoiGianVSBatDau = DateTime.Now;
                                    mauXN.NguoiChuyenLabo = ApplicationContext.Current.CommonUser.HoVaTen;
                                    mauXN.NgayChuyenLabo = DateTime.Now;
                                    mauXN.TrangThaiMau = "Labo";
                                    mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                                    ShowNotify("Thông báo", "Đã chuyển mẫu sang labo thành công", NotifyType.Success);
                                    lnkInPhieuChuyenMau.Visible = true;
                                    linkChuyenMau.Visible = false;
                                    ListItem it = cbTrangThaiMau.Items.FindByValue(mauXN.TrangThaiMau);
                                    ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>{1}</b>", mauXN.MaMau, it.Text);
                                    cbTrangThaiMau.SelectedValue = mauXN.TrangThaiMau;
                                    CloseMessageBox();
                                }
                            }
                        }
                    }
                    else
                    {
                        btnSubmitTrans.Visible = true;
                        btnSubmitDuyetLabo.Visible = false;
                        btnSubmitThayDoiLoaiHinhYeuCau.Visible = false;
                        TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                        if (mauXN != null)
                        {
                            if (mauXN.LuongMau.HasValue && mauXN.LuongMau.Value > 0 && mauXN.LuongMauHoa.HasValue)
                            {
                                List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                                if (chiTieuCol.Count > 0)
                                {
                                    MessageBoxResult result = new MessageBoxResult();
                                    result.Value = mauXN;
                                    result.Submit = true;
                                    result.CommandName = "trans_confirm";
                                    CurrentMessageBoxResult = result;
                                    OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                                   string.Format("Bạn muốn chuyển mẫu này sang Labo  ?"));
                                }
                                else ShowNotify("Thông báo", "Mẫu chưa có chỉ tiêu. Vui lòng cập nhật danh sách chỉ tiêu xét nghiệm.", NotifyType.Error);
                            }
                            else ShowNotify("Thông báo", "Vui lòng nhập lượng mẫu.", NotifyType.Error);

                        }
                    }
                }


            }
        }

        private List<TblDonViTinh> CurrentDonViTinhList
        {
            get
            {
                List<TblDonViTinh> dvt = null;
                if (Session["MXN-DVT"] != null)
                {
                    dvt = Session["MXN-DVT"] as List<TblDonViTinh>;
                    if (dvt != null) return dvt;

                }
                if (dvt == null) dvt = CategoryManager.GetDonViTinhALL(true);
                Session["MXN-DVT"] = dvt;
                return dvt;
            }
        }

        private List<TblCatChiTieuGioiHan> CurrentChiTieuGioiHanList
        {
            get
            {
                // Code: CTGH-SESSION-20260918 - Sau khi Page_Load xóa cache, lần dùng đầu tiên nạp DB và khởi tạo lại Session.
                List<TblCatChiTieuGioiHan> gioiHan = Session["MXN-CTGH"] as List<TblCatChiTieuGioiHan>;
                if (gioiHan == null)
                {
                    gioiHan = CategoryManager.GetChiTieuGioiHanAll(true);
                    Session["MXN-CTGH"] = gioiHan;
                }

                return gioiHan;
            }
        }

        int _CXD_count = 0;
        protected void repUpdateChiTieu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item ||
             e.Item.ItemType == ListItemType.AlternatingItem) &&
             e.Item.DataItem != null && e.Item.DataItem is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu chiTieu = e.Item.DataItem as TblMauXetNghiemChiTieu;
                chiTieu.ECT = "";
                List<TblChiTieuQuyChuan> chiTieuQuyChuan = new List<TblChiTieuQuyChuan>();
                if (Session["ChiTieuQuyChuan_Temp"] != null)
                {
                    chiTieuQuyChuan = Session["ChiTieuQuyChuan_Temp"] as List<TblChiTieuQuyChuan>;
                    if (chiTieuQuyChuan == null) chiTieuQuyChuan = new List<TblChiTieuQuyChuan>();
                }

                Literal ltrTenChiTieu = e.Item.FindControl("ltrTenChiTieu") as Literal;
                Literal ltrDonViTinh = e.Item.FindControl("ltrDonViTinh") as Literal;
                HiddenField hiddenDonViTinh = e.Item.FindControl("hiddenDonViTinh") as HiddenField;
                HiddenField hiddenDonViTinhID = e.Item.FindControl("hiddenDonViTinhID") as HiddenField;
                ExtraTextBox txtInput = e.Item.FindControl("txtInput") as ExtraTextBox;
                ExtraTextBox txtViTri1 = e.Item.FindControl("txtViTri1") as ExtraTextBox;
                ExtraTextBox txtViTri2 = e.Item.FindControl("txtViTri2") as ExtraTextBox;
                ExtraTextBox txtViTri3 = e.Item.FindControl("txtViTri3") as ExtraTextBox;
                ExtraTextBox txtViTri4 = e.Item.FindControl("txtViTri4") as ExtraTextBox;
                ExtraTextBox txtViTri5 = e.Item.FindControl("txtViTri5") as ExtraTextBox;
                ExtraComboBox cbInput = e.Item.FindControl("cbInput") as ExtraComboBox;
                ExtraComboBox cbViTri1 = e.Item.FindControl("cbViTri1") as ExtraComboBox;
                ExtraComboBox cbViTri2 = e.Item.FindControl("cbViTri2") as ExtraComboBox;
                ExtraComboBox cbViTri3 = e.Item.FindControl("cbViTri3") as ExtraComboBox;
                ExtraComboBox cbViTri4 = e.Item.FindControl("cbViTri4") as ExtraComboBox;
                ExtraComboBox cbViTri5 = e.Item.FindControl("cbViTri5") as ExtraComboBox;
                ExtraComboBox cbPhepSoSanh = e.Item.FindControl("cbPhepSoSanh") as ExtraComboBox;
                Literal ltrNA = e.Item.FindControl("ltrNA") as Literal;
                Literal ltrKetQua = e.Item.FindControl("ltrKetQua") as Literal;
                HiddenField hiddenMCTID = e.Item.FindControl("hiddenMCTID") as HiddenField;
                HiddenField hiddenChiTieuID = e.Item.FindControl("hiddenChiTieuID") as HiddenField;
                
                HiddenField hiddenCTQCID = e.Item.FindControl("hiddenCTQCID") as HiddenField;
                ExtraCheckBox chkTuDanhGia = e.Item.FindControl("chkTuDanhGia") as ExtraCheckBox;
                ExtraCheckBox chkItem = e.Item.FindControl("chkItem") as ExtraCheckBox;
                Panel panelBinhThuong = e.Item.FindControl("panelBinhThuong") as Panel; panelBinhThuong.Visible = hiddenGiaTriNhapVS.Value == "binhthuong";
                Panel panelNhapkhoang = e.Item.FindControl("panelNhapkhoang") as Panel; panelNhapkhoang.Visible = !panelBinhThuong.Visible;

                string tendonviTinh = !string.IsNullOrEmpty(chiTieu.TenDonViQuyChuan) ? chiTieu.TenDonViQuyChuan : chiTieu.TenDonVi;
                ExtraComboBox cbDonViTinh = e.Item.FindControl("cbDonViTinh") as ExtraComboBox;
                if (cbDonViTinh != null)
                {
                    cbDonViTinh.DataSource = new List<TblDonViTinh>(CurrentDonViTinhList);
                    cbDonViTinh.DataTextField = TblDonViTinh.Columns.TenDonVi;
                    cbDonViTinh.DataValueField = TblDonViTinh.Columns.Id;
                    cbDonViTinh.DataBind();

                    cbDonViTinh.Enabled = chkDanhGiaMau.Checked;

                    if (!chkDanhGiaMau.Checked && !string.IsNullOrEmpty(chiTieu.TenDonViNA))
                    {
                        ListItem item = cbDonViTinh.Items.FindByText(chiTieu.TenDonViNA);
                        if (item != null) cbDonViTinh.SelectedValue = item.Value;
                    }
                    else if (!chkDanhGiaMau.Checked && !string.IsNullOrEmpty(tendonviTinh))
                    {
                        ListItem item = cbDonViTinh.Items.FindByText(tendonviTinh);
                        if (item != null) cbDonViTinh.SelectedValue = item.Value;
                    }
                }
                hiddenChiTieuID.Value = chiTieu.IDChiTieu.Value.ToString();
                hiddenMCTID.Value = chiTieu.Id.ToString();
                if (chiTieu.CTQCID.HasValue && chiTieu.CTQCID.Value > 0) hiddenCTQCID.Value = chiTieu.CTQCID.Value.ToString();
                ltrTenChiTieu.Text = string.Format("<span class='title'>{0}</span><span class='sub-title'>[ {1} ]</span>", chiTieu.TenTaiCacKhoa, chiTieu.TenPhuongPhap);
                ltrDonViTinh.Text = !string.IsNullOrEmpty(chiTieu.TenDonViQuyChuan) ? chiTieu.TenDonViQuyChuan : chiTieu.TenDonVi;
                hiddenDonViTinh.Value = ltrDonViTinh.Text.Trim().Replace("<",".::").Replace(">", "::.");
                string kieuNhap = chiTieu.HinhThucGioiHanChuan;


                TblChiTieuQuyChuan ctqc = chiTieuQuyChuan.FirstOrDefault(p => p.IDChiTieu.Value == chiTieu.IDChiTieu.Value);
                if (ctqc != null)
                {
                    kieuNhap = ctqc.LoaiHinhGioiHan;
                    hiddenDonViTinhID.Value = ctqc.IDDonViTinh.HasValue ? ctqc.IDDonViTinh.Value.ToString() : "";
                }
                else if (chiTieu.CTXNDonViID.HasValue)
                    hiddenDonViTinhID.Value = chiTieu.CTXNDonViID.HasValue ? chiTieu.CTXNDonViID.Value.ToString() : "";

                chkTuDanhGia.Visible = chkLaboDanhGiaMau.Checked && ctqc != null && !string.IsNullOrEmpty(chiTieu.KetQuaHienThi) && chiTieu.KetQuaHienThi != ".";
                chkTuDanhGia.Checked = chiTieu.TuDanhGia.HasValue && chiTieu.TuDanhGia.Value;
                //chkTuDanhGia.Visible =  && !string.IsNullOrEmpty(chiTieu.KetQuaHienThi.Trim());

                chkItem.Visible = CurrentPageType.StartsWith("Labo");
                //bool _showNormal = true;

                if (kieuNhap == "NhapSo")
                {
                    if (hiddenGiaTriNhapVS.Value == "binhthuong")
                    {
                        #region nhập số
                        cbPhepSoSanh.Visible = true;
                        cbPhepSoSanh.SelectedValue = "=";
                        if (!string.IsNullOrEmpty(chiTieu.GiaTriSoSanhKetQua))
                        {
                            ListItem item = cbPhepSoSanh.Items.FindByValue(chiTieu.GiaTriSoSanhKetQua);
                            if (item != null) cbPhepSoSanh.SelectedValue = chiTieu.GiaTriSoSanhKetQua;
                        }

                        txtInput.Visible = true;
                        ltrKetQua.Text = "<span style='color:#ffb752;font-weight:bold;'>CXĐ</span>";
                        if (chiTieu.KetQuaGioiHan.HasValue)
                        {
                            if (chiTieu.DanhGia.HasValue && ctqc != null && chkLaboDanhGiaMau.Checked)
                            {
                                if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                            }
                            else ltrKetQua.Text = "<b>KQĐ</b>";
                            txtInput.Text = chiTieu.KetQuaGioiHan.Value.ToStringWithLanguage();
                            if (chiTieu.HienThiThapPhanNguyen.HasValue && chiTieu.HienThiThapPhanNguyen.Value)
                                txtInput.Text = chiTieu.KetQuaGioiHan.Value.ToStringWithLanguage() + ",0";
                        }
                        else
                        {
                            _CXD_count++;
                            ltrChiTieuUpdateCount.Text = string.Format("Còn <span style='color:#ffb752;font-weight:bold;'> {0} [ CXĐ ]</span> chỉ tiêu chưa được cập nhật kết quả", _CXD_count);
                        }
                        #endregion 
                    }
                    else
                    {
                        if (txtViTri1 != null)
                        {
                            txtViTri1.Visible = true;
                            if (chiTieu.GiaTriVung1.HasValue) txtViTri1.Text = chiTieu.GiaTriVung1.Value.ToStringWithLanguage();
                        }
                        if (txtViTri2 != null)
                        {
                            txtViTri2.Visible = true;
                            if (chiTieu.GiaTriVung2.HasValue) txtViTri2.Text = chiTieu.GiaTriVung2.Value.ToStringWithLanguage();
                        }
                        if (txtViTri3 != null)
                        {
                            txtViTri3.Visible = true;
                            if (chiTieu.GiaTriVung3.HasValue) txtViTri3.Text = chiTieu.GiaTriVung3.Value.ToStringWithLanguage();
                        }
                        if (txtViTri4 != null)
                        {
                            txtViTri4.Visible = true;
                            if (chiTieu.GiaTriVung4.HasValue) txtViTri4.Text = chiTieu.GiaTriVung4.Value.ToStringWithLanguage();
                        }
                        if (txtViTri5 != null)
                        {
                            txtViTri5.Visible = true;
                            if (chiTieu.GiaTriVung5.HasValue) txtViTri5.Text = chiTieu.GiaTriVung5.Value.ToStringWithLanguage();
                        }
                        if (chiTieu.DanhGia.HasValue && ctqc != null && chkLaboDanhGiaMau.Checked)
                        {
                            if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                            if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                        }
                        else ltrKetQua.Text = "<b>KQĐ</b>";
                    }
                }
                else
                {
                    if (hiddenGiaTriNhapVS.Value == "binhthuong")
                    {
                        #region nhập cb
                        cbInput.Visible = true;
                        cbPhepSoSanh.Visible = true;
                        ltrKetQua.Text = "<span style='color:#ffb752;font-weight:bold;'>CXĐ</span>";

                        cbPhepSoSanh.Enabled = false;
                        cbPhepSoSanh.SelectedValue = "=";
                        if (!string.IsNullOrEmpty(chiTieu.GiaTriSoSanhKetQua))
                        {
                            ListItem item = cbPhepSoSanh.Items.FindByValue(chiTieu.GiaTriSoSanhKetQua);
                            if (item != null) cbPhepSoSanh.SelectedValue = chiTieu.GiaTriSoSanhKetQua;
                        }
                        List<TblCatChiTieuGioiHan> gioiHan = CurrentChiTieuGioiHanList;

                        // Code: COMBO-POPUP-LAYER-20260918 - Dữ liệu của popup cbInput được bind tại đây; CSS xử lý lớp hiển thị trong MauXetNghiemChiTiet.aspx.
                        cbInput.DataSource = new List<TblCatChiTieuGioiHan>(gioiHan);
                        cbInput.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
                        cbInput.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
                        cbInput.DataBind();
                        if (chiTieu.KetQuaGiaTri.HasValue && chiTieu.KetQuaGiaTri.Value > 0)
                        {
                            ListItem it = cbInput.Items.FindByValue(chiTieu.KetQuaGiaTri.Value.ToString());
                            if (it != null)
                            {
                                if (chiTieu.DanhGia.HasValue && ctqc != null)
                                {
                                    if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                    if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                }
                                else ltrKetQua.Text = "<b>KQĐ</b>";
                                cbInput.SelectedValue = chiTieu.KetQuaGiaTri.Value.ToString();
                            }
                        }
                        else
                        {
                            _CXD_count++;
                            ltrChiTieuUpdateCount.Text = string.Format("Còn <span style='color:#ffb752;font-weight:bold;'> {0} [ CXĐ ]</span> chỉ tiêu chưa được cập nhật kết quả", _CXD_count);
                        }
                        #endregion
                    }
                    else
                    {
                        bool b = true;
                        #region Vị trí
                        if (ctqc != null && ctqc.GiaTri1.HasValue && ctqc.GiaTri1.Value > 0)
                        {
                            if (cbViTri1 != null)
                            {
                                cbViTri1.DataSource = new List<TblCatChiTieuGioiHan>(CurrentChiTieuGioiHanList);
                                cbViTri1.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
                                cbViTri1.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
                                cbViTri1.DataBind();
                                cbViTri1.Visible = true;
                                b = false;
                            }
                        }
                        else if (txtViTri1 != null) txtViTri1.Visible = b;

                        if (ctqc != null && ctqc.GiaTri2.HasValue && ctqc.GiaTri2.Value > 0)
                        {
                            if (cbViTri2 != null)
                            {
                                cbViTri2.DataSource = new List<TblCatChiTieuGioiHan>(CurrentChiTieuGioiHanList);
                                cbViTri2.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
                                cbViTri2.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
                                cbViTri2.DataBind();
                                cbViTri2.Visible = true;
                                b = false;
                            }
                        }
                        else if (txtViTri2 != null) txtViTri2.Visible = b;

                        if (ctqc != null && ctqc.GiaTri3.HasValue && ctqc.GiaTri3.Value > 0)
                        {
                            if (cbViTri3 != null)
                            {
                                cbViTri3.DataSource = new List<TblCatChiTieuGioiHan>(CurrentChiTieuGioiHanList);
                                cbViTri3.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
                                cbViTri3.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
                                cbViTri3.DataBind();
                                cbViTri3.Visible = true;
                                b = false;
                            }
                        }
                        else if (txtViTri3 != null) txtViTri3.Visible = b;

                        if (ctqc != null && ctqc.GiaTri4.HasValue && ctqc.GiaTri4.Value > 0)
                        {
                            if (cbViTri4 != null)
                            {
                                cbViTri4.DataSource = new List<TblCatChiTieuGioiHan>(CurrentChiTieuGioiHanList);
                                cbViTri4.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
                                cbViTri4.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
                                cbViTri4.DataBind();
                                cbViTri4.Visible = true;
                                b = false;
                            }
                        }
                        else if (txtViTri4 != null) txtViTri4.Visible = b;

                        if (txtViTri5 != null) txtViTri5.Visible = b;
                        #endregion

                        #region bind data
                        //cbInput.Visible = true;
                        //cbPhepSoSanh.Visible = true;
                        ltrKetQua.Text = "<span style='color:#ffb752;font-weight:bold;'>CXĐ</span>";

                        //cbPhepSoSanh.Enabled = false;
                        //cbPhepSoSanh.SelectedValue = "=";
                        //if (!string.IsNullOrEmpty(chiTieu.GiaTriSoSanhKetQua))
                        //{
                        //    ListItem item = cbPhepSoSanh.Items.FindByValue(chiTieu.GiaTriSoSanhKetQua);
                        //    if (item != null) cbPhepSoSanh.SelectedValue = chiTieu.GiaTriSoSanhKetQua;
                        //}

                        //List<TblCatChiTieuGioiHan> gioiHan = CurrentChiTieuGioiHanList;
                        #region bind cb value
                        if (cbViTri1.Visible && chiTieu.GiaTriVung1.HasValue)
                        {
                            int valuey = Convert.ToInt32(chiTieu.GiaTriVung1.Value);
                            ListItem item = cbViTri1.Items.FindByValue(valuey.ToString());
                            if (item != null) cbViTri1.SelectedValue = valuey.ToString();
                        }
                        if (cbViTri2.Visible && chiTieu.GiaTriVung2.HasValue)
                        {
                            int valuey = Convert.ToInt32(chiTieu.GiaTriVung2.Value);
                            ListItem item = cbViTri2.Items.FindByValue(valuey.ToString());
                            if (item != null) cbViTri2.SelectedValue = valuey.ToString();
                        }
                        if (cbViTri3.Visible && chiTieu.GiaTriVung3.HasValue)
                        {
                            int valuey = Convert.ToInt32(chiTieu.GiaTriVung3.Value);
                            ListItem item = cbViTri3.Items.FindByValue(valuey.ToString());
                            if (item != null) cbViTri3.SelectedValue = valuey.ToString();
                        }
                        if (cbViTri4.Visible && chiTieu.GiaTriVung4.HasValue)
                        {
                            int valuey = Convert.ToInt32(chiTieu.GiaTriVung4.Value);
                            ListItem item = cbViTri4.Items.FindByValue(valuey.ToString());
                            if (item != null) cbViTri4.SelectedValue = valuey.ToString();
                        }
                        if (cbViTri5.Visible && chiTieu.GiaTriVung5.HasValue)
                        {
                            int valuey = Convert.ToInt32(chiTieu.GiaTriVung5.Value);
                            ListItem item = cbViTri5.Items.FindByValue(valuey.ToString());
                            if (item != null) cbViTri5.SelectedValue = valuey.ToString();
                        }
                        #endregion

                        if (chiTieu.KetQuaGiaTri.HasValue && chiTieu.KetQuaGiaTri.Value > 0)
                        {
                            if (chiTieu.DanhGia.HasValue && ctqc != null)
                            {
                                if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                            }
                            else ltrKetQua.Text = "<b>KQĐ</b>";

                        }
                        else
                        {
                            _CXD_count++;
                            ltrChiTieuUpdateCount.Text = string.Format("Còn <span style='color:#ffb752;font-weight:bold;'> {0} [ CXĐ ]</span> chỉ tiêu chưa được cập nhật kết quả", _CXD_count);
                        }
                        #endregion
                    }
                }

                if (!CurrentPageType.StartsWith("Labo"))
                {
                    txtInput.Visible = cbInput.Visible = cbPhepSoSanh.Visible = chkTuDanhGia.Visible = false;
                    Literal ltrKetQuaTuDanhGia = e.Item.FindControl("ltrKetQuaTuDanhGia") as Literal;
                    Literal ltrKetQuaHienThi = e.Item.FindControl("ltrKetQuaHienThi") as Literal;
                    ltrKetQuaHienThi.Text = chiTieu.KetQuaHienThi;
                    ltrKetQuaTuDanhGia.Text = "KQĐ";
                    if (ctqc != null)
                    {
                        if (chiTieu.TuDanhGia.HasValue && chiTieu.TuDanhGia.Value) ltrKetQuaTuDanhGia.Text = "<span style='color:#87b87f ;font-weight:bold;'>Đạt</span>";
                        if (chiTieu.TuDanhGia.HasValue && !chiTieu.TuDanhGia.Value) ltrKetQuaTuDanhGia.Text = "<span style='color:#d15b47 ;font-weight:bold;'>Không đạt</span>";
                    }

                    if (hiddenGiaTriNhapVS.Value != "binhthuong")
                    {
                        Literal ltrViTri1 = e.Item.FindControl("ltrViTri1") as Literal;
                        Literal ltrViTri2 = e.Item.FindControl("ltrViTri2") as Literal;
                        Literal ltrViTri3 = e.Item.FindControl("ltrViTri3") as Literal;
                        Literal ltrViTri4 = e.Item.FindControl("ltrViTri4") as Literal;
                        Literal ltrViTri5 = e.Item.FindControl("ltrViTri5") as Literal;
                        if (kieuNhap == "NhapSo") { }
                        else
                        {
                            if (cbViTri1 != null) cbViTri1.Visible = false; if (cbViTri2 != null) cbViTri2.Visible = false;
                            if (cbViTri3 != null) cbViTri3.Visible = false; if (cbViTri4 != null) cbViTri4.Visible = false;
                            if (cbViTri5 != null) cbViTri5.Visible = false;
                            if (ltrViTri1 != null && cbViTri1 != null && !string.IsNullOrEmpty(cbViTri1.SelectedValue)) ltrViTri1.Text = string.Format("<span class='title-small' style='font-weight:bold;'>{0}</span>", cbViTri1.SelectedItem.Text);
                            if (ltrViTri2 != null && cbViTri2 != null && !string.IsNullOrEmpty(cbViTri2.SelectedValue)) ltrViTri2.Text = string.Format("<span class='title-small' style='font-weight:bold;'>{0}</span>", cbViTri2.SelectedItem.Text);
                            if (ltrViTri3 != null && cbViTri3 != null && !string.IsNullOrEmpty(cbViTri3.SelectedValue)) ltrViTri3.Text = string.Format("<span class='title-small' style='font-weight:bold;'>{0}</span>", cbViTri3.SelectedItem.Text);
                            if (ltrViTri4 != null && cbViTri4 != null && !string.IsNullOrEmpty(cbViTri4.SelectedValue)) ltrViTri3.Text = string.Format("<span class='title-small' style='font-weight:bold;'>{0}</span>", cbViTri4.SelectedItem.Text);
                            if (ltrViTri5 != null && cbViTri5 != null && !string.IsNullOrEmpty(cbViTri5.SelectedValue)) ltrViTri5.Text = string.Format("<span class='title-small' style='font-weight:bold;'>{0}</span>", cbViTri5.SelectedItem.Text);
                        }
                    }
                }

                Literal ltrGioiHanChoPhep = e.Item.FindControl("ltrGioiHanChoPhep") as Literal;

                if (ltrGioiHanChoPhep != null && chiTieu != null && chiTieu.Idctqc.HasValue && chiTieu.Idctqc.Value > 0)
                {

                    if (ctqc != null)
                    {
                        string rs = "";
                        int currentColumn = 0;
                        if (ctqc.LoaiHinhGioiHan == "NhapSo")
                        {
                            if (ctqc.GioiHan1.HasValue)
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan1);
                            }
                            if (ctqc.GioiHan2.HasValue)
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan2);
                            }
                            if (ctqc.GioiHan3.HasValue)
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan3);
                            }
                            if (ctqc.GioiHan4.HasValue)
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenGioiHan4);
                            }
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc1))
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc1);
                            }
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc2))
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc2);
                            }
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc3))
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc3);
                            }
                            if (!string.IsNullOrEmpty(ctqc.TenDanhMuc4))
                            {
                                currentColumn++; rs += string.Format("<td style='text-align: center;'>{0}</td>", ctqc.TenDanhMuc4);
                            }
                        }

                        while (currentColumn < columnCount)
                        {
                            rs += "<td></td>";
                            currentColumn++;
                        }

                        if (ltrGioiHanChoPhep != null && chkDanhGiaMau.Checked) ltrGioiHanChoPhep.Text = rs;

                    }
                    else if (chkDanhGiaMau.Checked)
                    {
                        ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='text-align:center;'>KQĐ</td>", columnCount);
                    }
                }
                else if (chkDanhGiaMau.Checked)
                {
                    ltrGioiHanChoPhep.Text = string.Format("<td colspan='{0}' style='text-align:center;'>KQĐ</td>", columnCount);
                }
            }
        }

        protected void btnUpdateChiTieu_Click(object sender, EventArgs e)
        {
            #region Validation
            if (string.IsNullOrEmpty(txtEditMaPhieuNoiBo.Text.Trim()))
            {
                AddValidPromt(txtEditMaPhieuNoiBo.ClientID, "Nhập Mã phiếu nội bộ");
                txtEditMaPhieuNoiBo.Focus();
                hiddenTab.Value = "ThongTin";
            }
            #endregion

            if (PageIsValid)
            {
                ltrChiTieuUpdateCount.Text = "";
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                if (mauXN != null)
                {
                    TblMauXetNghiem mauXN2 = XetNghiemManager.GetMauXetNghiemByMaNoiBo(txtEditMaPhieuNoiBo.Text, CurrentPageType == "LaboHoaLy");
                    if (mauXN2 != null && mauXN2.Id != mauXN.Id)
                    {
                        AddValidPromt(txtEditMaPhieuNoiBo.ClientID, "Mã phiếu nội bộ này đã tồn tại trên hệ thống. Vui lòng thử mã khác!");
                        txtEditMaPhieuNoiBo.Focus();
                        ShowValidPromt();
                        return;
                    }
                    if (CurrentPageType == "LaboHoaLy")
                    {
                        mauXN.ThoiGianHoaBatDau = null;
                        if (dateLaboBatDau.DateValue != null && dateLaboBatDau.DateValue != DateTime.MinValue)
                            mauXN.ThoiGianHoaBatDau = dateLaboBatDau.DateValue;

                        mauXN.ThoiGianHoaKetThuc = null;
                        if (dateLaboKetThuc.DateValue != null && dateLaboKetThuc.DateValue != DateTime.MinValue)
                            mauXN.ThoiGianHoaKetThuc = dateLaboKetThuc.DateValue;
                        mauXN.MaSoTraHoaLy = txtEditMaPhieuNoiBo.Text;
                        string[] arr = mauXN.MaSoTraHoaLy.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length == 3)
                            mauXN.MaSoTraHoaLyIndex = string.Format("{0}/{1}/{2}", arr[1], arr[2], arr[0]);
                        else mauXN.MaSoTraHoaLy = "";
                    }
                    else
                        if (CurrentPageType == "LaboViSinh")
                    {
                        mauXN.ThoiGianVSBatDau = null;
                        if (dateLaboBatDau.DateValue != null && dateLaboBatDau.DateValue != DateTime.MinValue)
                            mauXN.ThoiGianVSBatDau = dateLaboBatDau.DateValue;

                        mauXN.ThoiGianVSKetThuc = null;
                        if (dateLaboKetThuc.DateValue != null && dateLaboKetThuc.DateValue != DateTime.MinValue)
                            mauXN.ThoiGianVSKetThuc = dateLaboKetThuc.DateValue;
                        mauXN.MaSoTraViSinh = txtEditMaPhieuNoiBo.Text;
                        string[] arr = mauXN.MaSoTraViSinh.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length == 3)
                            mauXN.MaSoTraViSinhIndex = string.Format("{0}/{1}/{2}", arr[1], arr[2], arr[0]);
                        else mauXN.MaSoTraViSinh = "";
                    }

                    ltrMaNoiBo.Text = "Mã phiếu nội bộ";
                    if (string.IsNullOrEmpty(txtEditMaPhieuNoiBo.Text.Trim()))
                    {

                        string rf2 = "";
                        if (CurrentPageType == "LaboHoaLy")
                            ltrMaNoiBo.Text = string.Format("Mã nội bộ: <b>{0}</b>", XetNghiemManager.GetMauPhieuIndex(ref rf2, 0, TblMauXetNghiem.Columns.MaSoTraHoaLyIndex));
                        else if (CurrentPageType == "LaboViSinh")
                            ltrMaNoiBo.Text = string.Format("Mã nội bộ: <b>{0}</b>", XetNghiemManager.GetMauPhieuIndex(ref rf2, 0, TblMauXetNghiem.Columns.MaSoTraViSinhIndex));

                    }

                    List<TblChiTieuQuyChuan> chiTieuQuyChuan = ChiTieuManager.GetChiTieuQuyChuanByQuyChuanID(mauXN.IDQuyChuan);
                    int total = 0, success = 0;
                    bool MauDat = true;
                    string msg = "";

                    bool nhapLaiMoi = false;
                    List<TblMauXetNghiemChiTieu> mauChiTieuMoi = new List<TblMauXetNghiemChiTieu>();
                    decimal vl1 = 0, vl2 = 0, vl3 = 0, vl4 = 0, vl5 = 0;
                    foreach (RepeaterItem item in repUpdateChiTieu.Items)
                    {
                        HiddenField hiddenMCTID = item.FindControl("hiddenMCTID") as HiddenField;
                        ExtraComboBox cbPhepSoSanh = item.FindControl("cbPhepSoSanh") as ExtraComboBox;
                        ExtraComboBox cbInput = item.FindControl("cbInput") as ExtraComboBox;
                        ExtraTextBox txtInput = item.FindControl("txtInput") as ExtraTextBox;
                        Literal ltrKetQua = item.FindControl("ltrKetQua") as Literal;
                        ExtraCheckBox chkTuDanhGia = item.FindControl("chkTuDanhGia") as ExtraCheckBox;
                        ExtraComboBox cbDonViTinh = item.FindControl("cbDonViTinh") as ExtraComboBox;
                        if (hiddenMCTID != null)
                        {
                            total++;
                            bool? _kiemTra = null, _giaTriDat = null;
                            string ketQuaHienThi = "";
                            TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenMCTID.Value);
                            if (chiTieu != null)
                            {


                                string kieuNhap = chiTieu.HinhThucGioiHanChuan;
                                string noiDungDanhGia = "";
                                TblChiTieuQuyChuan quyChuan = chiTieuQuyChuan.FirstOrDefault(p => p.IDChiTieu.Value == chiTieu.IDChiTieu.Value);
                                if (quyChuan != null)
                                {
                                    chiTieu.Idctqc = quyChuan.Id;
                                    kieuNhap = quyChuan.LoaiHinhGioiHan;
                                    _giaTriDat = quyChuan.GiaTriDat.Value;
                                }

                                if (hiddenGiaTriNhapVS.Value == "binhthuong")
                                {
                                    if (kieuNhap == "NhapSo")
                                    {
                                        #region txtInput
                                        if (txtInput != null)
                                        {
                                            decimal d = 0;
                                            string ip_vl = txtInput.Text.Trim().Replace(".", "").Replace(",", ".");
                                            if (decimal.TryParse(ip_vl, out d) && d >= 0)
                                            {

                                                if (quyChuan != null)
                                                {
                                                    _giaTriDat = quyChuan.GiaTriDat.Value;
                                                    chiTieu.Idctqc = quyChuan.Id;
                                                    List<decimal> lstValue = new List<decimal>();
                                                    if (quyChuan.GioiHan1.HasValue) lstValue.Add(quyChuan.GioiHan1.Value);
                                                    if (quyChuan.GioiHan2.HasValue) lstValue.Add(quyChuan.GioiHan2.Value);
                                                    if (quyChuan.GioiHan3.HasValue) lstValue.Add(quyChuan.GioiHan3.Value);
                                                    if (quyChuan.GioiHan4.HasValue) lstValue.Add(quyChuan.GioiHan4.Value);

                                                    #region Nội dung đánh giá
                                                    noiDungDanhGia = "Kết quả ";
                                                    ListItem it = cbPhepSoSanhCompare.Items.FindByValue(quyChuan.PhepSoSanh);
                                                    if (it != null) noiDungDanhGia += it.Text + " [ ";

                                                    if (quyChuan.GioiHan1.HasValue) noiDungDanhGia += quyChuan.GioiHan1.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan1P.HasValue) noiDungDanhGia += "-" + quyChuan.GioiHan1P.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan2.HasValue) noiDungDanhGia += "/" + quyChuan.GioiHan2.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan2P.HasValue) noiDungDanhGia += "-" + quyChuan.GioiHan2P.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan3.HasValue) noiDungDanhGia += "/" + quyChuan.GioiHan3.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan3P.HasValue) noiDungDanhGia += "-" + quyChuan.GioiHan3P.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan4.HasValue) noiDungDanhGia += "/" + quyChuan.GioiHan4.Value.ToStringWithLanguage();
                                                    if (quyChuan.GioiHan4P.HasValue) noiDungDanhGia += "-" + quyChuan.GioiHan4P.Value.ToStringWithLanguage();

                                                    noiDungDanhGia += " ] thì " + (quyChuan.GiaTriDat.HasValue && quyChuan.GiaTriDat.Value ? "Đạt" : "Không đạt") + "";
                                                    #endregion

                                                    List<decimal> lstKiemTra = lstValue.OrderBy(p => p).ToList<decimal>();
                                                    if (lstKiemTra.Count > 0)
                                                    {
                                                        _kiemTra = false;
                                                        decimal minVL = lstKiemTra[0];
                                                        decimal maxVL = lstKiemTra[lstKiemTra.Count - 1];
                                                        if (cbPhepSoSanh.SelectedValue == "=")
                                                        {
                                                            if (quyChuan.PhepSoSanh == "LHGH") _kiemTra = d > maxVL;
                                                            if (quyChuan.PhepSoSanh == "LHBGH") _kiemTra = d >= maxVL;
                                                            if (quyChuan.PhepSoSanh == "BHGH") _kiemTra = d < minVL;
                                                            if (quyChuan.PhepSoSanh == "BHBGH") _kiemTra = d <= minVL;
                                                            if (quyChuan.PhepSoSanh == "TKGH" ||
                                                                quyChuan.PhepSoSanh == "TKHBGH")
                                                            {
                                                                if (quyChuan.GioiHan1.HasValue && quyChuan.GioiHan1P.HasValue)
                                                                    _kiemTra |= (d > quyChuan.GioiHan1.Value && d < quyChuan.GioiHan1P.Value) ||
                                                                                (quyChuan.PhepSoSanh == "TKHBGH" && (d == quyChuan.GioiHan1.Value || d == quyChuan.GioiHan1P.Value));
                                                                if (quyChuan.GioiHan2.HasValue && quyChuan.GioiHan2P.HasValue)
                                                                    _kiemTra |= (d > quyChuan.GioiHan2.Value && d < quyChuan.GioiHan2P.Value) ||
                                                                                (quyChuan.PhepSoSanh == "TKHBGH" && (d == quyChuan.GioiHan2.Value || d == quyChuan.GioiHan2P.Value));
                                                                if (quyChuan.GioiHan3.HasValue && quyChuan.GioiHan3P.HasValue)
                                                                    _kiemTra |= (d > quyChuan.GioiHan3.Value && d < quyChuan.GioiHan3P.Value) ||
                                                                                (quyChuan.PhepSoSanh == "TKHBGH" && (d == quyChuan.GioiHan3.Value || d == quyChuan.GioiHan3P.Value));
                                                                if (quyChuan.GioiHan4.HasValue && quyChuan.GioiHan4P.HasValue)
                                                                    _kiemTra |= (d > quyChuan.GioiHan4.Value && d < quyChuan.GioiHan4P.Value) ||
                                                                                (quyChuan.PhepSoSanh == "TKHBGH" && (d == quyChuan.GioiHan4.Value || d == quyChuan.GioiHan4P.Value));
                                                            }
                                                            if (quyChuan.PhepSoSanh == "BGH")
                                                                foreach (decimal t in lstKiemTra) _kiemTra |= d == t;
                                                        }
                                                        else if (cbPhepSoSanh.SelectedValue == "<" &&
                                                            (quyChuan.PhepSoSanh == "BHGH" ||
                                                                 quyChuan.PhepSoSanh == "BHBGH")
                                                                 )
                                                            _kiemTra = d <= minVL;
                                                        else if (cbPhepSoSanh.SelectedValue == ">" &&
                                                                 (quyChuan.PhepSoSanh == "LHGH" ||
                                                                 quyChuan.PhepSoSanh == "LHBGH"))
                                                            _kiemTra = d >= maxVL;

                                                        if (_kiemTra.HasValue)
                                                        {
                                                            bool c = false;
                                                            if (_kiemTra.Value)
                                                                c = _giaTriDat.Value;
                                                            else
                                                                c = !_giaTriDat.Value;
                                                            bool change = !chiTieu.DanhGia.HasValue || (chiTieu.DanhGia.HasValue && chiTieu.DanhGia.Value != c);
                                                            chiTieu.DanhGia = c;
                                                            //if (!string.IsNullOrEmpty(chiTieu.KetQuaHienThi)) chiTieu.TuDanhGia = chkTuDanhGia.Checked;
                                                            if (change)
                                                            {
                                                                chiTieu.NgayDanhGia = DateTime.Now;
                                                                chiTieu.TuDanhGia = chiTieu.DanhGia;
                                                            }
                                                            else chiTieu.TuDanhGia = chkTuDanhGia.Checked;
                                                            if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                                            if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                                        }
                                                    }
                                                }
                                                else
                                                    chiTieu.DanhGia = null;

                                                chiTieu.KetQuaGioiHan = d;
                                                chiTieu.HienThiThapPhanNguyen = false;
                                                chiTieu.KetQuaHienThi = "";
                                                if (cbPhepSoSanh.SelectedValue != "=")
                                                    chiTieu.KetQuaHienThi += cbPhepSoSanh.SelectedValue + " ";
                                                if (txtInput.Text.Trim().EndsWith(",0"))
                                                {
                                                    chiTieu.HienThiThapPhanNguyen = true;
                                                    chiTieu.KetQuaHienThi += d.ToStringWithLanguage() + ",0";
                                                }
                                                else
                                                    chiTieu.KetQuaHienThi += d.ToStringWithLanguage();
                                                chiTieu.NoiDungDanhGia = noiDungDanhGia;
                                                chiTieu.GiaTriSoSanhKetQua = cbPhepSoSanh.SelectedValue;
                                                chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                                                MauDat &= chiTieu.TuDanhGia.HasValue && chiTieu.TuDanhGia.Value;
                                                success++;
                                            }
                                            else
                                                AddValidPromt(txtInput.ClientID, "Nhập vào giá trị số");
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        #region CbInput
                                        if (cbInput != null)
                                        {
                                            if (!string.IsNullOrEmpty(cbInput.SelectedValue))
                                            {
                                                _kiemTra = false;
                                                string value = cbInput.SelectedValue;

                                                if (quyChuan != null)
                                                {
                                                    #region Kiểm tra
                                                    _kiemTra |= quyChuan.GiaTri1.HasValue && quyChuan.GiaTri1.Value.ToString() == value;
                                                    _kiemTra |= quyChuan.GiaTri2.HasValue && quyChuan.GiaTri2.Value.ToString() == value;
                                                    _kiemTra |= quyChuan.GiaTri3.HasValue && quyChuan.GiaTri3.Value.ToString() == value;
                                                    _kiemTra |= quyChuan.GiaTri4.HasValue && quyChuan.GiaTri4.Value.ToString() == value;

                                                    #endregion

                                                    #region Nội dung đánh giá
                                                    noiDungDanhGia = "Kết quả ";
                                                    ListItem it = cbPhepSoSanhCompare.Items.FindByValue(quyChuan.PhepSoSanh);
                                                    if (it != null) noiDungDanhGia += it.Text + " [ ";

                                                    if (quyChuan.GiaTri1.HasValue)
                                                    {
                                                        ListItem Litem = cbInput.Items.FindByValue(quyChuan.GiaTri1.Value.ToString());
                                                        if (Litem != null) noiDungDanhGia += Litem.Text;
                                                    }
                                                    if (quyChuan.GiaTri2.HasValue)
                                                    {
                                                        ListItem Litem = cbInput.Items.FindByValue(quyChuan.GiaTri2.Value.ToString());
                                                        if (Litem != null) noiDungDanhGia += "/" + Litem.Text;
                                                    }
                                                    if (quyChuan.GiaTri3.HasValue)
                                                    {
                                                        ListItem Litem = cbInput.Items.FindByValue(quyChuan.GiaTri3.Value.ToString());
                                                        if (Litem != null) noiDungDanhGia += "/" + Litem.Text;
                                                    }
                                                    if (quyChuan.GiaTri4.HasValue)
                                                    {
                                                        ListItem Litem = cbInput.Items.FindByValue(quyChuan.GiaTri4.Value.ToString());
                                                        if (Litem != null) noiDungDanhGia += "/" + Litem.Text;
                                                    }
                                                    noiDungDanhGia += " ] thì " + (quyChuan.GiaTriDat.HasValue && quyChuan.GiaTriDat.Value ? "Đạt" : "Không đạt");
                                                    #endregion
                                                    if (_kiemTra.HasValue)
                                                    {
                                                        bool c = false;
                                                        if (_kiemTra.Value)
                                                            c = _giaTriDat.Value;
                                                        else
                                                            c = !_giaTriDat.Value;
                                                        bool change = !chiTieu.DanhGia.HasValue || (chiTieu.DanhGia.HasValue && chiTieu.DanhGia.Value != c);
                                                        chiTieu.DanhGia = c;

                                                        //if (!string.IsNullOrEmpty(chiTieu.KetQuaHienThi)) chiTieu.TuDanhGia = chkTuDanhGia.Checked;
                                                        if (change)
                                                        {
                                                            chiTieu.NgayDanhGia = DateTime.Now;
                                                            chiTieu.TuDanhGia = chiTieu.DanhGia;
                                                        }
                                                        else chiTieu.TuDanhGia = chkTuDanhGia.Checked;
                                                        if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                                        if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                                    }
                                                }
                                                else
                                                    chiTieu.DanhGia = null;

                                                chiTieu.NoiDungDanhGia = noiDungDanhGia;
                                                chiTieu.KetQuaGiaTri = int.Parse(value);
                                                chiTieu.KetQuaHienThi = cbInput.DisplayText;
                                                chiTieu.GiaTriSoSanhKetQua = cbPhepSoSanh.SelectedValue;
                                                chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                                                MauDat &= chiTieu.TuDanhGia.HasValue && chiTieu.TuDanhGia.Value;
                                                success++;
                                            }
                                            else
                                                AddValidPromt(cbInput.ClientID, "Chọn giá trị kết quả");
                                        }
                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (kieuNhap == "NhapSo")
                                    {
                                        if (!nhapLaiMoi)
                                        {
                                            #region txt Data
                                            ExtraTextBox txtViTri1 = item.FindControl("txtViTri1") as ExtraTextBox;
                                            ExtraTextBox txtViTri2 = item.FindControl("txtViTri2") as ExtraTextBox;
                                            ExtraTextBox txtViTri3 = item.FindControl("txtViTri3") as ExtraTextBox;
                                            ExtraTextBox txtViTri4 = item.FindControl("txtViTri4") as ExtraTextBox;
                                            ExtraTextBox txtViTri5 = item.FindControl("txtViTri5") as ExtraTextBox;

                                            if (txtViTri1 != null && !string.IsNullOrEmpty(txtViTri1.Text))
                                            {
                                                decimal vl = -1;
                                                if (decimal.TryParse(txtViTri1.Text.Trim(), out vl)) { chiTieu.GiaTriVung1 = vl; vl1 += vl; }
                                                else
                                                {
                                                    AddValidPromt(txtViTri1.ClientID, "Giá trị không đúng định dạng");
                                                    nhapLaiMoi = true;
                                                }
                                            }
                                            else
                                            {
                                                AddValidPromt(txtViTri1.ClientID, "Nhập giá trị vùng");
                                                nhapLaiMoi = true;
                                            }
                                            if (txtViTri2 != null && !string.IsNullOrEmpty(txtViTri2.Text))
                                            {
                                                decimal vl = -1;
                                                if (decimal.TryParse(txtViTri2.Text.Trim(), out vl)) { chiTieu.GiaTriVung2 = vl; vl2 += vl; }
                                                else
                                                {
                                                    AddValidPromt(txtViTri2.ClientID, "Giá trị không đúng định dạng");
                                                    nhapLaiMoi = true;
                                                }
                                            }
                                            else
                                            {
                                                AddValidPromt(txtViTri2.ClientID, "Nhập giá trị vùng");
                                                nhapLaiMoi = true;
                                            }
                                            if (txtViTri3 != null && !string.IsNullOrEmpty(txtViTri3.Text))
                                            {
                                                decimal vl = -1;
                                                if (decimal.TryParse(txtViTri3.Text.Trim(), out vl)) { chiTieu.GiaTriVung3 = vl; vl3 += vl; }
                                                else
                                                {
                                                    AddValidPromt(txtViTri3.ClientID, "Giá trị không đúng định dạng");
                                                    nhapLaiMoi = true;
                                                }
                                            }
                                            else
                                            {
                                                AddValidPromt(txtViTri3.ClientID, "Nhập giá trị vùng");
                                                nhapLaiMoi = true;
                                            }

                                            if (txtViTri4 != null && !string.IsNullOrEmpty(txtViTri4.Text))
                                            {
                                                decimal vl = -1;
                                                if (decimal.TryParse(txtViTri4.Text.Trim(), out vl)) { chiTieu.GiaTriVung4 = vl; vl4 += vl; }
                                                else
                                                {
                                                    AddValidPromt(txtViTri4.ClientID, "Giá trị không đúng định dạng");
                                                    nhapLaiMoi = true;
                                                }
                                            }
                                            else
                                            {
                                                AddValidPromt(txtViTri4.ClientID, "Nhập giá trị vùng");
                                                nhapLaiMoi = true;
                                            }
                                            if (txtViTri5 != null && !string.IsNullOrEmpty(txtViTri5.Text))
                                            {
                                                decimal vl = -1;
                                                if (decimal.TryParse(txtViTri5.Text.Trim(), out vl))
                                                {
                                                    chiTieu.GiaTriVung5 = vl; vl5 += vl;
                                                }
                                                else
                                                {
                                                    AddValidPromt(txtViTri5.ClientID, "Giá trị không đúng định dạng");
                                                    nhapLaiMoi = true;
                                                }
                                            }
                                            else
                                            {
                                                AddValidPromt(txtViTri5.ClientID, "Nhập giá trị vùng");
                                                nhapLaiMoi = true;
                                            }

                                            #endregion

                                            mauChiTieuMoi.Add(chiTieu);
                                            _kiemTra = true;
                                        }

                                    }
                                    else
                                    {
                                        #region cb Data
                                        bool _isvalid = true;
                                        ExtraComboBox cbViTri1 = item.FindControl("cbViTri1") as ExtraComboBox;
                                        if (cbViTri1 != null && cbViTri1.Visible && string.IsNullOrEmpty(cbViTri1.SelectedValue)) { AddValidPromt(cbViTri1.ClientID, "Chọn giá trị"); _isvalid = false; }
                                        ExtraComboBox cbViTri2 = item.FindControl("cbViTri2") as ExtraComboBox;
                                        if (cbViTri2 != null && cbViTri2.Visible && string.IsNullOrEmpty(cbViTri2.SelectedValue)) { AddValidPromt(cbViTri2.ClientID, "Chọn giá trị"); _isvalid = false; }
                                        ExtraComboBox cbViTri3 = item.FindControl("cbViTri3") as ExtraComboBox;
                                        if (cbViTri3 != null && cbViTri3.Visible && string.IsNullOrEmpty(cbViTri3.SelectedValue)) { AddValidPromt(cbViTri3.ClientID, "Chọn giá trị"); _isvalid = false; }
                                        ExtraComboBox cbViTri4 = item.FindControl("cbViTri4") as ExtraComboBox;
                                        if (cbViTri4 != null && cbViTri4.Visible && string.IsNullOrEmpty(cbViTri4.SelectedValue)) { AddValidPromt(cbViTri4.ClientID, "Chọn giá trị"); _isvalid = false; }
                                        ExtraComboBox cbViTri5 = item.FindControl("cbViTri5") as ExtraComboBox;
                                        if (cbViTri5 != null && cbViTri5.Visible && string.IsNullOrEmpty(cbViTri5.SelectedValue)) { AddValidPromt(cbViTri5.ClientID, "Chọn giá trị"); _isvalid = false; }
                                        if (_isvalid)
                                        {
                                            if (quyChuan != null)
                                            {
                                                _kiemTra = false;
                                                #region Kiểm tra
                                                if (cbViTri1.Visible)
                                                    _kiemTra |= quyChuan.GiaTri1.HasValue && !string.IsNullOrEmpty(cbViTri1.SelectedValue) && quyChuan.GiaTri1.Value.ToString() == cbViTri1.SelectedValue;
                                                if (cbViTri2.Visible)
                                                    _kiemTra |= quyChuan.GiaTri2.HasValue && !string.IsNullOrEmpty(cbViTri2.SelectedValue) && quyChuan.GiaTri2.Value.ToString() == cbViTri2.SelectedValue;
                                                if (cbViTri3.Visible)
                                                    _kiemTra |= quyChuan.GiaTri3.HasValue && !string.IsNullOrEmpty(cbViTri3.SelectedValue) && quyChuan.GiaTri3.Value.ToString() == cbViTri3.SelectedValue;
                                                if (cbViTri4.Visible)
                                                    _kiemTra |= quyChuan.GiaTri4.HasValue && !string.IsNullOrEmpty(cbViTri4.SelectedValue) && quyChuan.GiaTri4.Value.ToString() == cbViTri4.SelectedValue;

                                                #endregion

                                                #region Nội dung đánh giá
                                                noiDungDanhGia = "Kết quả ";
                                                ListItem it = cbPhepSoSanhCompare.Items.FindByValue(quyChuan.PhepSoSanh);
                                                if (it != null) noiDungDanhGia += it.Text + " [ ";

                                                if (cbViTri1.Visible && quyChuan.GiaTri1.HasValue)
                                                {
                                                    ListItem Litem = cbViTri1.Items.FindByValue(quyChuan.GiaTri1.Value.ToString());
                                                    if (Litem != null) noiDungDanhGia += Litem.Text;
                                                }
                                                if (cbViTri2.Visible && quyChuan.GiaTri2.HasValue)
                                                {
                                                    ListItem Litem = cbViTri2.Items.FindByValue(quyChuan.GiaTri2.Value.ToString());
                                                    if (Litem != null) noiDungDanhGia += "/" + Litem.Text;
                                                }
                                                if (cbViTri3.Visible && quyChuan.GiaTri3.HasValue)
                                                {
                                                    ListItem Litem = cbViTri3.Items.FindByValue(quyChuan.GiaTri3.Value.ToString());
                                                    if (Litem != null) noiDungDanhGia += "/" + Litem.Text;
                                                }
                                                if (cbViTri4.Visible && quyChuan.GiaTri4.HasValue)
                                                {
                                                    ListItem Litem = cbViTri4.Items.FindByValue(quyChuan.GiaTri4.Value.ToString());
                                                    if (Litem != null) noiDungDanhGia += "/" + Litem.Text;
                                                }
                                                noiDungDanhGia += " ] thì " + (quyChuan.GiaTriDat.HasValue && quyChuan.GiaTriDat.Value ? "Đạt" : "Không đạt");
                                                #endregion
                                                if (_kiemTra.HasValue)
                                                {
                                                    bool c = false;
                                                    if (_kiemTra.Value)
                                                        c = _giaTriDat.Value;
                                                    else
                                                        c = !_giaTriDat.Value;
                                                    bool change = !chiTieu.DanhGia.HasValue || (chiTieu.DanhGia.HasValue && chiTieu.DanhGia.Value != c);
                                                    chiTieu.DanhGia = c;

                                                    //if (!string.IsNullOrEmpty(chiTieu.KetQuaHienThi)) chiTieu.TuDanhGia = chkTuDanhGia.Checked;
                                                    if (change)
                                                    {
                                                        chiTieu.NgayDanhGia = DateTime.Now;
                                                        chiTieu.TuDanhGia = chiTieu.DanhGia;
                                                    }
                                                    else chiTieu.TuDanhGia = chkTuDanhGia.Checked;
                                                    if (chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#87b87f;font-weight:bold;' onclick='alert(\"{0}\")' >Đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                                    if (!chiTieu.DanhGia.Value) ltrKetQua.Text = string.Format("<span ><a style='color:#d15b47;font-weight:bold;' onclick='alert(\"{0}\")' >Không đạt</a></span>", !string.IsNullOrEmpty(chiTieu.NoiDungDanhGia) ? chiTieu.NoiDungDanhGia : "Không có thông tin");
                                                }
                                            }
                                            else
                                                chiTieu.DanhGia = null;

                                            chiTieu.NoiDungDanhGia = noiDungDanhGia;
                                            chiTieu.KetQuaGiaTri = 1000;
                                            //chiTieu.KetQuaHienThi = cbInput.DisplayText;
                                            if (cbViTri1.Visible && !string.IsNullOrEmpty(cbViTri1.SelectedValue)) chiTieu.GiaTriVung1 = decimal.Parse(cbViTri1.SelectedValue);
                                            if (cbViTri2.Visible && !string.IsNullOrEmpty(cbViTri2.SelectedValue)) chiTieu.GiaTriVung2 = decimal.Parse(cbViTri2.SelectedValue);
                                            if (cbViTri3.Visible && !string.IsNullOrEmpty(cbViTri3.SelectedValue)) chiTieu.GiaTriVung3 = decimal.Parse(cbViTri3.SelectedValue);
                                            if (cbViTri4.Visible && !string.IsNullOrEmpty(cbViTri4.SelectedValue)) chiTieu.GiaTriVung4 = decimal.Parse(cbViTri4.SelectedValue);
                                            if (cbViTri5.Visible && !string.IsNullOrEmpty(cbViTri5.SelectedValue)) chiTieu.GiaTriVung5 = decimal.Parse(cbViTri5.SelectedValue);
                                            chiTieu.GiaTriSoSanhKetQua = cbPhepSoSanh.SelectedValue;
                                            chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                                            MauDat &= chiTieu.TuDanhGia.HasValue && chiTieu.TuDanhGia.Value;
                                            success++;
                                        }
                                        #endregion
                                    }

                                }

                                if (!_kiemTra.HasValue && quyChuan != null)
                                {
                                    _CXD_count++;
                                    ltrChiTieuUpdateCount.Text = string.Format("Còn <span style='color:#ffb752;font-weight:bold;'> {0} [ CXĐ ]</span> chỉ tiêu chưa được cập nhật kết quả", _CXD_count);
                                }

                            }
                        }
                    }
                    if (!PageIsValid)
                    {
                        mauChiTieuMoi.Clear();
                        vl1 = vl2 = vl3 = vl4 = vl5 = 0;
                        ShowValidPromt();
                        ShowNotify("Thông báo", "1 số chỉ tiêu chưa đúng, vui lòng hoàn thiện kết quả", NotifyType.Warning);
                    }
                    else
                    {
                        #region Cập nhật cho mẫu 2 vi sinh
                        if (!nhapLaiMoi)
                        {
                            bool datMoi = false;
                            #region tính giá trị trung bình
                            decimal average = (vl1 + vl2 + vl3 + vl4 + vl5) / 5;
                            if (average > 100)
                            {
                                int phanChuc = (int)(average / 10);
                                if (average - (phanChuc * 10) >= 5) average = (phanChuc + 1) * 10;
                                else average = phanChuc * 10;
                            }
                            #endregion
                            foreach (TblMauXetNghiemChiTieu chiTieu in mauChiTieuMoi)
                            {

                                {
                                    bool c = average <= 200;
                                    bool change = !chiTieu.DanhGia.HasValue || (chiTieu.DanhGia.HasValue && chiTieu.DanhGia.Value != c);
                                    chiTieu.DanhGia = c;

                                    if (change)
                                    {
                                        chiTieu.NgayDanhGia = DateTime.Now;
                                        chiTieu.TuDanhGia = chiTieu.DanhGia;
                                    }

                                    chiTieu.HienThiThapPhanNguyen = false;
                                    chiTieu.KetQuaHienThi = ".";

                                    chiTieu.NoiDungDanhGia = string.Format("Chỉ số trung bình {0}", average);
                                    chiTieu.GiaTriSoSanhKetQua = "BGH";
                                    XetNghiemManager.UpdateMauChiTieu(chiTieu);
                                    MauDat = c;

                                }
                            }
                        }
                        #endregion

                        LoadMauTieuChiUpdate(mauXN);
                        ShowNotify("Thông báo", "Đã cập nhật kết quả các chỉ tiêu xét nghiệm", NotifyType.Success);
                    }
                    if (mauXN.TrangThaiMau == "Labo")
                        mauXN.TrangThaiMau = "DangThucHien";
                    if (CurrentPageType == "LaboHoaLy")
                        mauXN.LaboHoaNhanXet = txtNhanXetLaboHoa.Text;
                    if (CurrentPageType == "LaboViSinh")
                    {
                        mauXN.LaboSinhNhanXet = txtNhanXetLaboVS.Text;

                    }
                    mauXN.LoaiLaboVisinh = hiddenGiaTriNhapVS.Value;
                    //if(mauXN.DanhGiaDatMau)
                    mauXN.DanhGiaDatMau = MauDat ? "1" : "0";

                    if (string.IsNullOrEmpty(mauXN.NhanXetCuoiCung))
                        mauXN.NhanXetCuoiCung = !string.IsNullOrEmpty(mauXN.LaboHoaNhanXet) ? mauXN.LaboHoaNhanXet : mauXN.LaboSinhNhanXet;
                    mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    lnkInPhieuKetQuaNoiNo.Visible = true;
                    if (CurrentPageType == "LaboViSinh" && lnkInPhieuKetQuaNoiNo.Visible)
                    {
                        ltrLI.Text = string.Format("<li><a href='{0}' target='blank'>Phiếu KQKNNB vi sinh BITEST</a></li>", string.Format("/phieu-tra-ket-qua-noi-bo-vs-so-{0}-m1", CurrentID.Value));
                        ltrLI.Text += string.Format("<li><a href='{0}' target='blank'>Phiếu KQKNNB vi sinh không khí</a></li>", string.Format("/phieu-tra-ket-qua-noi-bo-vs-so-{0}-m2", CurrentID.Value));
                    }
                    if (mauXN.TrangThaiMau == "DangThucHien")
                        ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>Đang thực hiện Labo</b>", mauXN.MaMau);
                }

            }
            else ShowValidPromt();

        }

        protected void btnDuyetLabo_Click(object sender, EventArgs e)
        {
            if (CurrentID.HasValue)
            {

                bool resultDelete = false;
                ExtraButton btn = sender as ExtraButton;
                if (btn != null)
                {
                    string cmdName = btn.CommandName;
                    if (cmdName == "duyet_labo")
                    {
                        if (CurrentMessageBoxResult.Submit && CurrentMessageBoxResult.Value != null)
                        {
                            if (CurrentMessageBoxResult.CommandName == "duyet_labo")
                            {
                                TblMauXetNghiem mauXN = CurrentMessageBoxResult.Value as TblMauXetNghiem;
                                if (mauXN != null)
                                {
                                    if (CurrentPageType == "LaboHoaLy")
                                    {
                                        if (!mauXN.NgayHoaLy.HasValue || (mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value == DateTime.MinValue))
                                            mauXN.NgayHoaLy = DateTime.Now;
                                        if (mauXN.LuongMauViSinh == 0 || (mauXN.LuongMauViSinh > 0 && mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value != DateTime.MinValue && mauXN.TrangThaiMau == "DangThucHien"))
                                            mauXN.TrangThaiMau = "ChoTraKetQua";
                                        cbTrangThaiMau.SelectedValue = mauXN.TrangThaiMau;
                                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                                    }
                                    else if (CurrentPageType == "LaboViSinh")
                                    {
                                        if (!mauXN.NgayViSinh.HasValue || (mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value == DateTime.MinValue))
                                            mauXN.NgayViSinh = DateTime.Now;
                                        if (mauXN.LuongMauHoa.Value == 0 || (mauXN.LuongMauHoa > 0 && mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value != DateTime.MinValue && mauXN.TrangThaiMau == "DangThucHien"))
                                            mauXN.TrangThaiMau = "ChoTraKetQua";
                                        cbTrangThaiMau.SelectedValue = mauXN.TrangThaiMau;
                                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                                    }
                                    lnkInPhieuKetQuaNoiNo.Visible = true;
                                    if (CurrentPageType == "LaboViSinh" && lnkInPhieuKetQuaNoiNo.Visible)
                                    {
                                        ltrLI.Text = string.Format("<li><a href='{0}' target='blank'>Phiếu KQKNNB vi sinh BITEST</a></li>", string.Format("/phieu-tra-ket-qua-noi-bo-vs-so-{0}-m1", CurrentID.Value));
                                        ltrLI.Text += string.Format("<li><a href='{0}' target='blank'>Phiếu KQKNNB vi sinh không khí</a></li>", string.Format("/phieu-tra-ket-qua-noi-bo-vs-so-{0}-m2", CurrentID.Value));
                                    }
                                    ShowNotify("Thông báo", "Đã duyệt kết quả labo thành công", NotifyType.Success);
                                    btnDuyetLabo.Visible = false;
                                    ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>chờ trả kết quả</b>", mauXN.MaMau);

                                    CloseMessageBox();
                                }
                            }
                        }
                    }
                    else
                    {
                        btnSubmitTrans.Visible = false;
                        btnSubmitThayDoiLoaiHinhYeuCau.Visible = false;
                        btnSubmitDuyetLabo.Visible = true;
                        TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                        if (mauXN != null)
                        {
                            List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.GetMauChiTieuBMauID(CurrentID.Value);
                            if (chiTieuCol.Count > 0)
                            {
                                MessageBoxResult result = new MessageBoxResult();
                                result.Value = mauXN;
                                result.Submit = true;
                                result.CommandName = "duyet_labo";
                                CurrentMessageBoxResult = result;
                                OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                               string.Format("Bạn muốn duyệt kết quả labo này  ?"));
                            }
                            else ShowNotify("Thông báo", "Mẫu chưa có chỉ tiêu. Vui lòng cập nhật danh sách chỉ tiêu xét nghiệm.", NotifyType.Error);
                        }
                    }
                }
            }
        }

        protected void btnGhiNhanKetQua_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                string cmN = btn.CommandName;
                if (CurrentID.HasValue)
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                    if (mauXN != null)
                    {
                        string msg = "";
                        if (cmN.StartsWith("CapNhat"))
                        {
                            if (string.IsNullOrEmpty(txtEditMaTraKetQua.Text))
                            {
                                AddValidPromt(txtEditMaTraKetQua.ClientID, "Cập nhật mã trả kết quả");
                                txtEditMaTraKetQua.Focus();
                                ShowValidPromt();
                                return;
                            }
                            else
                            {
                                msg += "Mẫu đã được cập nhật nhận xét ";
                                TblMauXetNghiem mxnOld = XetNghiemManager.GetMauXetNghiemByMaTraKQ(txtEditMaTraKetQua.Text);
                                if (mxnOld != null)
                                {
                                    if (mxnOld.Id != mauXN.Id)
                                    {
                                        AddValidPromt(txtEditMaTraKetQua.ClientID, "Mã trả kết quả đã tồn tại. Vui lòng chọn mã khác.");
                                        txtEditMaTraKetQua.Focus();
                                        ShowValidPromt();
                                        return;
                                    }
                                }
                                else msg += " và mã trả kết quả";
                                //mauXN.MaSoTraKQ = txtEditMaTraKetQua.Text;
                                if (!mauXN.NgayTaoSoTraMau.HasValue ||
                                    (mauXN.NgayTaoSoTraMau.HasValue && mauXN.NgayTaoSoTraMau.Value == DateTime.MinValue))
                                    mauXN.NgayTaoSoTraMau = DateTime.Now;
                                mauXN.GhiChuKetQua = txtGhiChuKetQua.Text;
                                mauXN.NhanXetCuoiCung = txtNhanXetCuoiCung.Text;
                                mauXN.DanhGiaDatMau = cbKetQuaDatMau.SelectedValue;

                            }


                        }
                        if (GetButtonRole("TraKetQua"))//(cmN.EndsWith("TraKQ"))
                        {
                            mauXN.KetQuaDanhGiaMau = null;
                            if (!string.IsNullOrEmpty(cbMauDatChatLuong.SelectedValue))
                                mauXN.KetQuaDanhGiaMau = cbMauDatChatLuong.SelectedValue;
                            mauXN.TrangThaiMau = "ChoTraKetQua";
                            mauXN.NgayTraKetQuaThucTe = null;
                            mauXN.NgayThanhLyDuKien = null;
                            mauXN.NguoiTraKetQuaThucTe = string.Empty;
                            if (mauXN.KetQuaDanhGiaMau == "1")
                            {
                                if (!mauXN.NgayTraKetQuaThucTe.HasValue || (
                                    mauXN.NgayTraKetQuaThucTe.HasValue && mauXN.NgayTraKetQuaThucTe.Value == DateTime.MinValue))
                                {
                                    mauXN.NgayTraKetQuaThucTe = DateTime.Now;
                                    mauXN.NguoiTraKetQuaThucTe = ApplicationContext.Current.CommonUser.HoVaTen;
                                }
                                mauXN.TrangThaiMau = "DaTraKetQua";
                                int date = 10;
                                if (mauXN.LuongMauViSinh > 0) date = 5;
                                if (!mauXN.NgayThanhLyDuKien.HasValue || (mauXN.NgayThanhLyDuKien.HasValue && mauXN.NgayThanhLyDuKien.Value == DateTime.MinValue))
                                {
                                    mauXN.NgayThanhLyDuKien = mauXN.NgayTraKetQuaThucTe.Value.AddDays(date);
                                    mauXN.NgayThanhLyDuKien = DateTimeHelper.AddFirstTimeOfDay(mauXN.NgayThanhLyDuKien.Value);
                                }


                                if (string.IsNullOrEmpty(msg)) msg = "Mẫu đã ghi nhận thông tin trả kết quả thành công.";
                                else msg = "Mẫu đã được cập nhật và trả kết quả thành công.";
                                ltrThongTin.Text = string.Format("Mẫu <b>{0}</b> trạng thái <b>Đã trả kết quả</b>", mauXN.MaMau);
                                lnkPhieuTraKetQua.Visible = lnkPhieuTraKetQuaM1.Visible = lnkPhieuTraKetQuaM2.Visible = true;
                            }
                            else if (mauXN.KetQuaDanhGiaMau == "0")
                                msg = "Mẫu đã được trả về Labo";
                            //CurrentLog.SaveLog(RightPageName.PMXN_TraKetQua);
                        }

                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                        ShowNotify("Thông báo", msg, NotifyType.Success);
                    }
                }
            }

        }

        protected void lnkCapNhatVaTraKetQua_Click(object sender, EventArgs e)
        {

        }

        protected void lnkPhieuDeNghiThuTien_Click(object sender, EventArgs e)
        {
            if (CurrentID.HasValue)
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                if (mauXN != null)
                {
                    traMauControl.LoadMau(mauXN);
                    dialogTraMau.ShowDialog();
                }
            }
        }

        protected void btnInPhieuTraMau_Click(object sender, EventArgs e)
        {
            bool b = traMauControl.Save();
            if (b)
            {
                string url = string.Format("/phieu-de-nghi-thu-tien-so-{0}", CurrentID.Value);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');{1}", url, dialogTraMau.GetScriptClose()), true);
                CloseMessageBox();
            }
        }

        protected void btnSubmitThayDoiLoaiHinhYeuCau_Click(object sender, EventArgs e)
        {
            Save(true);
        }

        protected void btnDoiMaMau_Click(object sender, EventArgs e)
        {
            if (CurrentID.HasValue)
            {
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(CurrentID.Value);
                if (mauXN != null)
                {
                    if (mauXN.TrangThaiMau == "New")
                    {
                        if (!string.IsNullOrEmpty(txtMaDieuChinh.Text.Trim()))
                        {
                            string[] arr = txtMaDieuChinh.Text.Trim().Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr.Length == 3)
                            {
                                TblMauXetNghiem xetNghiem = XetNghiemManager.GetMauXetNghiemByCode(txtMaDieuChinh.Text.Trim());
                                if (xetNghiem == null)
                                {
                                    mauXN.MaMau = txtMaDieuChinh.Text.Trim();
                                    mauXN.MaMauIndex = string.Format("{0}/{1}/{2}", arr[1], arr[2], arr[0]);
                                    mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                                    ShowNotify("Thông báo", "Đổi mã mẫu thành công", NotifyType.Success);
                                    dialogDoiMaMau.CloseDialog();
                                    LoadData();
                                }
                                else
                                {
                                    AddValidPromt(txtMaDieuChinh.ClientID, "Đã tồn tại mẫu với mã này trên hệ thống. Bạn không thể đổi mã.");
                                    txtMaDieuChinh.Focus();
                                    ShowValidPromt();
                                }
                            }
                            else
                            {
                                AddValidPromt(txtMaDieuChinh.ClientID, "Mã điều chỉnh không hợp lệ. Vui lòng kiểm tra lại");
                                txtMaDieuChinh.Focus();
                                ShowValidPromt();
                            }
                        }
                        else
                        {
                            int month = 0, year = 0;
                            if (!int.TryParse(cbThang.SelectedValue, out month) || month < 1)
                            {
                                AddValidPromt(cbThang.ClientID, "Chọn tháng cần lùi dữ liệu.");
                                cbThang.Focus();

                            }
                            if (!int.TryParse(cbNam.SelectedValue, out year) || year < 1)
                            {
                                AddValidPromt(cbNam.ClientID, "Chọn năm cần lùi dữ liệu.");
                                cbNam.Focus();

                            }

                            if (PageIsValid)
                            {
                                DateTime dt = new DateTime(year, month, 1);
                                string rf2 = "";
                                mauXN.MaMau = XetNghiemManager.GetMauPhieuIndex(ref rf2, mauXN.IDLoaiHinhYeuCauMau.Value, "", dt);
                                mauXN.MaMauIndex = rf2;

                                mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                                ShowNotify("Thông báo", "Đổi mã mẫu thành công", NotifyType.Success);
                                dialogDoiMaMau.CloseDialog();
                                LoadData();
                            }
                            else ShowValidPromt();
                        }
                    }
                    else
                    {
                        AddValidPromt(txtMaDieuChinh.ClientID, "Hệ thống chỉ cho phép đổi mã với mẫu mới tạo. Vui lòng kiểm tra lại.");
                        txtMaDieuChinh.Focus();
                        ShowValidPromt();
                    }
                }
            }
        }

        private void LoadThangNam()
        {
            cbThang.Items.Add(new ListItem("", "Không chọn"));
            cbNam.Items.Add(new ListItem("", "Không chọn"));
            for (int i = 1; i <= 12; i++) cbThang.Items.Add(new ListItem("Tháng " + i, i.ToString()));
            for (int i = 2020; i <= DateTime.Now.Year; i++) cbNam.Items.Add(new ListItem("Năm " + i, i.ToString()));

        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {

        }

        protected void btnViewFromQuyChuan_Click(object sender, EventArgs e)
        {
            List<TblChiTieuXetNghiem> xetNghiemCol = XetNghiemManager.SearchChiTieuByQuyChuanID(cbQUyChuan.SelectedValue, CurrentID);
            repChiTieuQuyChuan.DataSource = xetNghiemCol;
            repChiTieuQuyChuan.DataBind();
            dialogChiTieuQuyChuan.ShowDialog();
        }

        protected void btnThemChiTieuQuyChuan_Click(object sender, EventArgs e)
        {
            int total = 0;
            foreach (RepeaterItem item in repChiTieuQuyChuan.Items)
            {
                ExtraCheckBox chkItem = item.FindControl("chkItem") as ExtraCheckBox;
                HiddenField hiddenItemID = item.FindControl("hiddenItemID") as HiddenField;
                HiddenField hiddenChiTieuPP = item.FindControl("hiddenChiTieuPP") as HiddenField;
                HiddenField hiddenPhuongPhapID = item.FindControl("hiddenPhuongPhapID") as HiddenField;
                if (chkItem.Checked)
                {
                    total++;
                    TblMauXetNghiemChiTieu mxnCT = new TblMauXetNghiemChiTieu();
                    mxnCT.IDMau = CurrentID.Value;
                    mxnCT.IDChiTieu = int.Parse(hiddenItemID.Value);
                    mxnCT.IDPhuongPhap = int.Parse(hiddenPhuongPhapID.Value);
                    mxnCT.Idctpp = int.Parse(hiddenChiTieuPP.Value);
                    string tenDonViNA = ChiTieuManager.GetTenDonViTinhNA(mxnCT.IDChiTieu, CurrentID.Value);
                    mxnCT.TenDonViNA = tenDonViNA;
                    mxnCT = XetNghiemManager.InsertMauChiTieu(mxnCT);
                }
            }

            if (total == 0) ShowNotify("Thông báo", "Vui lòng chọn chỉ tiêu đưa vào mẫu", NotifyType.Warning);
            else
            {
                ShowNotify("Thông báo", string.Format("Thêm mới {0} chỉ tiêu vào mẫu thành công", total), NotifyType.Success);
                dialogChiTieuQuyChuan.CloseDialog();
                LoadMauTieuChi();
            }
        }

        protected void btnUpdateDVT_Click(object sender, EventArgs e)
        {
            int total = 0;
            foreach (RepeaterItem item in repDataFinal.Items)
            {
                HiddenField hiddenItemID = item.FindControl("hiddenItemID") as HiddenField;
                ExtraCheckBox chkItem = item.FindControl("chkItem") as ExtraCheckBox;
                if (hiddenItemID != null && chkItem != null && chkItem.Checked)
                {
                    if (hiddenItemID.Value != "0")
                    {
                        TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemID.Value);
                        if (chiTieu != null)
                        {
                            chiTieu.TenDonViNA = cbUpdateDVT.SelectedItem.Text;
                            chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                            total++;
                        }

                    }
                }
                Repeater repDataFinalSub = item.FindControl("repDataFinalSub") as Repeater;
                if (repDataFinalSub != null)
                {
                    foreach (RepeaterItem item2 in repDataFinalSub.Items)
                    {
                        HiddenField hiddenItemSubID = item2.FindControl("hiddenItemSubID") as HiddenField;
                        ExtraCheckBox chkItemSub = item2.FindControl("chkItemSub") as ExtraCheckBox;
                        if (hiddenItemSubID != null && chkItemSub != null && chkItemSub.Checked)
                        {
                            if (hiddenItemSubID.Value != "0")
                            {
                                TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemSubID.Value);
                                if (chiTieu != null)
                                {
                                    chiTieu.TenDonViNA = cbUpdateDVT.SelectedItem.Text;
                                    chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                                    total++;
                                }
                            }
                        }
                    }
                }
            }
            if (total == 0) ShowNotify("Thông báo", "Vui lòng chọn chỉ tiêu cần cập nhật đơn vị tính", NotifyType.Warning);
            else
            {
                LoadMauTieuChi();
                ShowNotify("Thông báo", string.Format("Đã cập nhật {0} chỉ tiêu", total), NotifyType.Success);
            }
        }

        protected void btnGiaTriNhapVS_Click(object sender, EventArgs e)
        {
            if (hiddenGiaTriNhapVS.Value == "binhthuong") hiddenGiaTriNhapVS.Value = "nhapkhoang";
            else hiddenGiaTriNhapVS.Value = "binhthuong";
            ltrGiaTriNhap.Text = "Nhập giá trị vùng";
            if (hiddenGiaTriNhapVS.Value == "binhthuong") ltrGiaTriNhap.Text = "Nhập giá trị";
            LoadMauTieuChiUpdate(null);
        }

        protected string GetDivGiaTriNhapVS(string type)
        {
            return type == hiddenGiaTriNhapVS.Value ? "display:block;" : "display:none;";
        }

        protected void chkVimcert_CheckedChanged(object sender, EventArgs e)
        {

            int total = 0;
            foreach (RepeaterItem item in repDataFinal.Items)
            {
                HiddenField hiddenItemID = item.FindControl("hiddenItemID") as HiddenField;
                ExtraCheckBox chkItem = item.FindControl("chkItem") as ExtraCheckBox;
                if (hiddenItemID != null && chkItem != null && chkItem.Checked)
                {
                    if (hiddenItemID.Value != "0")
                    {
                        TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemID.Value);
                        if (chiTieu != null)
                        {
                            chiTieu.Vimcert = chkVimcert.Checked;
                            chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                            total++;
                        }

                    }
                }
                Repeater repDataFinalSub = item.FindControl("repDataFinalSub") as Repeater;
                if (repDataFinalSub != null)
                {
                    foreach (RepeaterItem item2 in repDataFinalSub.Items)
                    {
                        HiddenField hiddenItemSubID = item2.FindControl("hiddenItemSubID") as HiddenField;
                        ExtraCheckBox chkItemSub = item2.FindControl("chkItemSub") as ExtraCheckBox;
                        if (hiddenItemSubID != null && chkItemSub != null && chkItemSub.Checked)
                        {
                            if (hiddenItemSubID.Value != "0")
                            {
                                TblMauXetNghiemChiTieu chiTieu = XetNghiemManager.GetMauChiTieuByID(hiddenItemSubID.Value);
                                if (chiTieu != null)
                                {
                                    chiTieu.Vimcert = chkVimcert.Checked;
                                    chiTieu = XetNghiemManager.UpdateMauChiTieu(chiTieu);
                                    total++;
                                    // update
                                }
                            }
                        }
                    }
                }
            }
            if (total == 0) ShowNotify("Thông báo", "Vui lòng chọn chỉ tiêu cần cập nhật vimcert", NotifyType.Warning);
            else
            {
                LoadMauTieuChi();
                ShowNotify("Thông báo", string.Format("Đã cập nhật {0} chỉ tiêu", total), NotifyType.Success);
            }
        }

        protected void cbPhepSoSanh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnGiaTriBe_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item2 in repUpdateChiTieu.Items)
            {

                HiddenField hiddenCTQCID = item2.FindControl("hiddenCTQCID") as HiddenField;
                ExtraComboBox cbPhepSoSanh = item2.FindControl("cbPhepSoSanh") as ExtraComboBox;
                ExtraCheckBox chkItem = item2.FindControl("chkItem") as ExtraCheckBox;
                ExtraTextBox txtInput = item2.FindControl("txtInput") as ExtraTextBox;
                //if (cbPhepSoSanh != null) cbPhepSoSanh.SelectedValue = "=";
                if (chkItem != null && chkItem.Checked)
                {
                    if (string.IsNullOrEmpty(hiddenCTQCID.Value)) hiddenCTQCID.Value = "0";
                    TblChiTieuQuyChuan CT = ChiTieuManager.GetChiTieuQuyChuanByID(hiddenCTQCID.Value);
                    if (CT != null && CT.GiaTriBe.HasValue && txtInput != null)
                    {
                        txtInput.Text = CT.GiaTriBe.Value.ToStringWithLanguage();
                        if (!txtInput.Text.Contains(",")) txtInput.Text += ",0";
                    }

                    if (cbPhepSoSanh != null) cbPhepSoSanh.SelectedValue = "<";
                }
            }
        }

        protected void btnUpdateGiaTriBe_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (RepeaterItem item2 in repUpdateChiTieu.Items)
            {

                HiddenField hiddenCTQCID = item2.FindControl("hiddenCTQCID") as HiddenField;
                ExtraComboBox cbPhepSoSanh = item2.FindControl("cbPhepSoSanh") as ExtraComboBox;
                ExtraCheckBox chkItem = item2.FindControl("chkItem") as ExtraCheckBox;
                ExtraTextBox txtInput = item2.FindControl("txtInput") as ExtraTextBox;
                HiddenField hiddenDonViTinh = item2.FindControl("hiddenDonViTinh") as HiddenField;
                HiddenField hiddenDonViTinhID = item2.FindControl("hiddenDonViTinhID") as HiddenField;
                HiddenField hiddenChiTieuID = item2.FindControl("hiddenChiTieuID") as HiddenField;
                
                if (chkItem != null && chkItem.Checked && cbPhepSoSanh.SelectedValue == "<")
                {
                    if (string.IsNullOrEmpty(hiddenCTQCID.Value)) hiddenCTQCID.Value = "0";
                    TblChiTieuQuyChuan CT = ChiTieuManager.GetChiTieuQuyChuanByID(hiddenCTQCID.Value);
                    if (CT != null)
                    {
                        CT.GiaTriBe = decimal.Parse(txtInput.Text.Replace(".", "").Replace(",", "."));
                        CT = ChiTieuManager.UpdateChiTieuQuyChuan(CT);
                        ChiTieuManager.UpdateChiTieuQuyChuanChoCacMau(CT, hiddenDonViTinh.Value.Replace(".::", "<").Replace( "::.", ">"));
                        count++;
                    }
                    else
                    {
                        CT = new TblChiTieuQuyChuan();
                        CT.LoaiHinhGioiHan = "NhapSo";
                        CT.IDChiTieu = int.Parse(hiddenChiTieuID.Value);
                        CT.IDQuyChuan = int.Parse(cbQUyChuan.SelectedValue);
                        CT.IDDonViTinh = int.Parse(hiddenDonViTinhID.Value);
                        CT.GiaTriBe = decimal.Parse(txtInput.Text.Replace(".", "").Replace(",", "."));
                        CT = ChiTieuManager.InsertChiTieuQuyChuan(CT);
                        hiddenCTQCID.Value = CT.Id.ToString();
                        ChiTieuManager.UpdateChiTieuQuyChuanChoCacMau(CT, hiddenDonViTinh.Value.Replace(".::", "<").Replace("::.", ">"));
                        count++;
                    }
                }
            }
            ShowNotify("Thông báo", string.Format("Đã cập nhật <b>{0}</b> giá trị bé cho chỉ tiêu quy chuẩn", count), NotifyType.Success);
        }
    }
}
