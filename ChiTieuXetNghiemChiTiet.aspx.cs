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
    public partial class ChiTieuXetNghiemChiTiet : BaseGridPage
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

            if (rightName == "Insert") rs = UserHasRight(RightPageName.ChiTieu_Insert);
            else if (rightName == "Update") rs = UserHasRight(RightPageName.ChiTieu_Update);
            else if (rightName == "Delete") rs = UserHasRight(RightPageName.ChiTieu_Delete);
            else if (rightName == "Setting") rs = UserHasRight(RightPageName.ChiTieu_Setting);
            return rs;
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Session["ChungChiTanSuat"] = null;
                btnSave.Visible = GetButtonRole("Update");
                divDetail.Visible = false;
                LoadControl();

                if (Session["ChiTieu_CreateNew"] != null &&
                    Session["ChiTieu_CreateNew"] is TblChiTieuXetNghiem)
                {
                    TblChiTieuXetNghiem chiTieu = Session["ChiTieu_CreateNew"] as TblChiTieuXetNghiem;
                    LoadChiTieu(chiTieu);
                    ShowNotifyStartUp("Thông báo", "Tạo mới thông tin chỉ tiêu thành công. Vui lòng cập nhật các nội dung bổ sung", false, NotifyType.Success);
                    Session["ChiTieu_CreateNew"] = null;
                }
                else
                    LoadChiTieu();
            }
        }

        private void LoadControl()
        {
            List<TblDonViTinh> donViTinhCol = CategoryManager.GetDonViTinhALL(true);
            TblDonViTinh empty4 = new TblDonViTinh();
            empty4.Id = -1;
            empty4.TenDonVi = "Không chọn đơn vị";
            donViTinhCol.Insert(0, empty4);
            cbDonViTinh.DataSource = donViTinhCol;
            cbDonViTinh.DataValueField = TblDonViTinh.Columns.Id;
            cbDonViTinh.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbDonViTinh.DataBind();

            List<TblDonViTinh> donViTinhCol2 = new List<TblDonViTinh>(donViTinhCol);
            donViTinhCol2.RemoveAt(0);
            cbDonViTinhTien.DataSource = donViTinhCol2;
            cbDonViTinhTien.DataValueField = TblDonViTinh.Columns.Id;
            cbDonViTinhTien.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbDonViTinhTien.DataBind();

            List<TblDonViTinh> donViTinhCol3 = new List<TblDonViTinh>(donViTinhCol);

            cbDonViTinhQuyChuan.DataSource = donViTinhCol3;
            cbDonViTinhQuyChuan.DataValueField = TblDonViTinh.Columns.Id;
            cbDonViTinhQuyChuan.DataTextField = TblDonViTinh.Columns.TenDonVi;
            cbDonViTinhQuyChuan.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            cbQuyChuan.DataSource = quyChuanCol;
            cbQuyChuan.DataValueField = TblCatQuyChuan.Columns.Id;
            cbQuyChuan.DataTextField = TblCatQuyChuan.Columns.Ten;
            cbQuyChuan.DataBind();

            List<TblCatChiTieuGioiHan> chiTieuGioiHan = CategoryManager.GetChiTieuGioiHanAll(true);
            TblCatChiTieuGioiHan empty = new TblCatChiTieuGioiHan();
            empty.Id = -1;
            empty.Ten = "--- Không chọn ---";
            chiTieuGioiHan.Insert(0, empty);
            cbGioiHan1.DataSource = chiTieuGioiHan;
            cbGioiHan1.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
            cbGioiHan1.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
            cbGioiHan1.DataBind();

            cbGioiHan2.DataSource = new List<TblCatChiTieuGioiHan>(chiTieuGioiHan);
            cbGioiHan2.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
            cbGioiHan2.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
            cbGioiHan2.DataBind();

            cbGioiHan3.DataSource = new List<TblCatChiTieuGioiHan>(chiTieuGioiHan);
            cbGioiHan3.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
            cbGioiHan3.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
            cbGioiHan3.DataBind();

            cbGioiHan4.DataSource = new List<TblCatChiTieuGioiHan>(chiTieuGioiHan);
            cbGioiHan4.DataValueField = TblCatChiTieuGioiHan.Columns.Id;
            cbGioiHan4.DataTextField = TblCatChiTieuGioiHan.Columns.Ten;
            cbGioiHan4.DataBind();
            //List<TblCatPhuongPhap> phuongPhapCol = ChiTieuManager.GetPhuongPhapAll(true);
            //foreach (TblCatPhuongPhap phuongPhap in phuongPhapCol)
            //{
            //    ListItem item = new ListItem();
            //    item.Text = phuongPhap.Ten;
            //    item.Value = phuongPhap.Id.ToString();
            //    string subText = string.Format("<br/><span class='pp-item'></span><span style='margin-left:5px;font-size:15px;vertical-align: bottom;'> {0}</span>", phuongPhap.Ten);
            //    item.Attributes.Add("data-subtext", subText);
            //    cbPhuongPhap.Items.Add(item);
            //}
            //cbPhuongPhap.DataSource = phuongPhapCol;
            //cbPhuongPhap.DataValueField = TblCatPhuongPhap.Columns.Id;
            //cbPhuongPhap.DataTextField = TblCatPhuongPhap.Columns.Ten;
            //cbPhuongPhap.DataBind();

            List<TblCatNhomXetNghiem> nhomCol = CategoryManager.GetNhomXetNghiemAll(true);
            TblCatNhomXetNghiem empty2 = new TblCatNhomXetNghiem();
            empty2.Id = -1;
            empty2.Ten = "Không chọn nhóm xét nghiệm";
            nhomCol.Insert(0, empty2);
            cbNhomXetNghiem.DataSource = nhomCol;
            cbNhomXetNghiem.DataValueField = TblCatNhomXetNghiem.Columns.Id;
            cbNhomXetNghiem.DataTextField = TblCatNhomXetNghiem.Columns.Ten;
            cbNhomXetNghiem.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            SaveDetail();

            if (!PageIsValid)
                ShowValidPromt();
            else
            {
                ShowNotify("Thông báo", "Lưu thông tin chỉ tiêu xét nghiệm thành công", NotifyType.Success);

            }
            CurrentLog.Clear();
        }

        private void SaveDetail()
        {

            #region Validation

            if (string.IsNullOrEmpty(txtMTenTaiKhoa.Text))
            {
                AddValidPromt(txtTenTaiKhoa.ClientID, "Nhập tên tại khoa");
                txtMTenTaiKhoa.Focus();
            }

            if (string.IsNullOrEmpty(txtMTenTrongThongTu.Text))
            {
                AddValidPromt(txtMTenTrongThongTu.ClientID, "Nhập tên theo thông tư");
                txtMTenTrongThongTu.Focus();
            }




            #endregion

            if (PageIsValid)
            {
                if (!string.IsNullOrEmpty(hiddenEditID.Value))
                {
                    TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(hiddenEditID.Value);
                    if (chiTieu != null)
                    {
                        #region Log
                        CurrentLog.Clear();
                        CurrentLog.AddNewLogFunction(RightPageName.ChiTieu_Update,
                                                     string.Format("Cập nhật chỉ tiêu xét nghiệm <b>{0}</b> ", chiTieu.TenTaiCacKhoa));


                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Số thông tư",
                                                 TblChiTieuXetNghiem.Columns.SoThongTu,
                                                 chiTieu.SoThongTu);
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Nhóm xét nghiệm",
                                               TblChiTieuXetNghiem.Columns.IDNhomXN,
                                               chiTieu.TenNhomXetNghiem);
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Ngày thông tư ",
                                                 TblChiTieuXetNghiem.Columns.NgayThongTu,
                                                 chiTieu.NgayThongTu.HasValue && chiTieu.NgayThongTu.Value != DateTime.MinValue ? chiTieu.NgayThongTu.Value.ToString("dd/MM/yyyy") : "");
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Tên tại khoa",
                                                 TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                 chiTieu.TenTaiCacKhoa);
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Tên theo thông tư",
                                                 TblChiTieuXetNghiem.Columns.TenTrongTTQD,
                                                 chiTieu.TenTrongTTQD);
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "STT",
                                                 TblChiTieuXetNghiem.Columns.Stt,
                                                 chiTieu.Stt);
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Hóa / vi sinh",
                                                          TblChiTieuXetNghiem.Columns.HoaSinh,
                                                          chiTieu.HoaSinh == "HoaLy" ? "Hóa sinh" : "Vi sinh");
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Đơn giá",
                                                 TblChiTieuXetNghiem.Columns.DefaultPrice,
                                                 chiTieu.DefaultPrice.HasValue ? chiTieu.DefaultPrice.Value.ToStringWithLanguage() : "0");
                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Đơn vị tính",
                                                 TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                                 chiTieu.TenDonVi);

                        CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Trạng thái sử dụng",
                                                 TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                                 chiTieu.TrangThaiSuDung.HasValue && chiTieu.TrangThaiSuDung.Value ? "Có" : "Không");
                        #endregion

                        //chiTieu.TenTaiCacKhoa = CategoryManager.CreateName(txtTenTaiKhoa.Text);
                        chiTieu.TenTaiCacKhoa = txtMTenTaiKhoa.Text;
                        chiTieu.TenTaiCacKhoaSearch = txtMTenTaiKhoa.MValue;
                        chiTieu.TenTrongTTQD = txtMTenTrongThongTu.Text;
                        chiTieu.TenTrongTTQDSearch = txtMTenTrongThongTu.MValue;
                        chiTieu.Stt = txtSTT.Text;
                        chiTieu.HoaSinh = cbHoaSinh.SelectedValue;
                        decimal price = 0;
                        if (!string.IsNullOrEmpty(txtDonGia.Text.Trim()))
                        {
                            if (!decimal.TryParse(txtDonGia.Text.Trim().Replace(".", ""), out price))
                                price = 0;
                        }

                        chiTieu.DefaultPrice = price;
                        chiTieu.IDNhomXN = null;
                        if (cbNhomXetNghiem.HasValue) chiTieu.IDNhomXN = int.Parse(cbNhomXetNghiem.SelectedValue);
                        chiTieu.HinhThucGioiHanChuan = cbHinhThucGioiHanChuan.SelectedValue;
                        chiTieu.IDDonViTinh = null;
                        bool changeDonVi = !chiTieu.IDDonViTinh.HasValue || (chiTieu.IDDonViTinh.HasValue && chiTieu.IDDonViTinh.Value.ToString() != cbDonViTinh.SelectedValue);
                        if (cbDonViTinh.HasValue)
                            chiTieu.IDDonViTinh = int.Parse(cbDonViTinh.SelectedValue);
                        chiTieu.IDDonViTinhTien = null;
                        if (cbDonViTinhTien.HasValue)
                            chiTieu.IDDonViTinhTien = int.Parse(cbDonViTinhTien.SelectedValue);
                        chiTieu.TrangThaiSuDung = chkTrangThaiSuDung.Checked;
                        chiTieu.SoThongTu = txtSoThongTu.Text;
                        chiTieu.NgayThongTu = null;
                        chiTieu.Italic = chkInNghieng.Checked;
                        if (!string.IsNullOrEmpty(txtGiaTriBe.Text))
                        {
                            chiTieu.GiaTriBe = decimal.Parse(txtGiaTriBe.Text.Replace(".", "").Replace(",", "."));
                        }
                        if (dateNgayThongTu.DateValue != DateTime.MinValue) chiTieu.NgayThongTu = dateNgayThongTu.DateValue;
                        chiTieu = ChiTieuManager.UpdateChiTieuXetNghiem(chiTieu);
                        #region Update tên đơn vị trong chỉ tiêu mẫu 16.08.22
                        if (changeDonVi)
                        {

                            List<TblMauXetNghiemChiTieu> chiTieuCok = XetNghiemManager.Temp_GetMauXetNghiemChiTieu(chiTieu.Id, null, null, true, false);
                            foreach (TblMauXetNghiemChiTieu ct in chiTieuCok)
                            {
                                ct.TenDonViNA = ct.TempDVTCT;
                                XetNghiemManager.UpdateMauChiTieu(ct);
                            }
                        }
                        #endregion
                        #region Log

                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.HoaSinh,
                                                 cbHoaSinh.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.IDNhomXN,
                                                 cbNhomXetNghiem.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.DefaultPrice,
                                                 txtDonGia.Text);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                        TblChiTieuXetNghiem.Columns.SoThongTu,
                                        txtSoThongTu.Text);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.NgayThongTu,
                                                 chiTieu.NgayThongTu.HasValue && chiTieu.NgayThongTu.Value != DateTime.MinValue ? chiTieu.NgayThongTu.Value.ToString("dd/MM/yyyy") : "");
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                 chiTieu.TenTaiCacKhoa);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TenTrongTTQD,
                                                 chiTieu.TenTrongTTQD);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.Stt,
                                                 txtSTT.Text);

                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                                 cbDonViTinh.DisplayText);
                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                              TblChiTieuXetNghiem.Columns.IDDonViTinhTien,
                                              cbDonViTinhTien.DisplayText);

                        CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                 TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                                 chkTrangThaiSuDung.Checked ? "Có" : "Không");

                        #endregion

                        //SavePhuongPhap(chiTieu);
                        //SaveNhom(chiTieu);
                        SaveChungNhan(chiTieu);
                        SaveChungChiQuyChuan();
                        chiTieu = ChiTieuManager.UpdateChiTieuXetNghiem(chiTieu);
                        CurrentLog.SaveLog(RightPageName.ChiTieu_Update);
                    }
                }
                else
                {
                    TblChiTieuXetNghiem chiTieu = new TblChiTieuXetNghiem();


                    chiTieu.TenTaiCacKhoa = txtMTenTaiKhoa.Text;
                    chiTieu.TenTaiCacKhoaSearch = txtMTenTaiKhoa.MValue;
                    chiTieu.TenTrongTTQD = txtMTenTrongThongTu.Text;
                    chiTieu.TenTrongTTQDSearch = txtMTenTrongThongTu.MValue;

                    chiTieu.HoaSinh = cbHoaSinh.SelectedValue;
                    decimal price = 0;
                    if (!string.IsNullOrEmpty(txtDonGia.Text.Trim()))
                    {
                        if (!decimal.TryParse(txtDonGia.Text.Trim().Replace(".", ""), out price))
                            price = 0;
                    }

                    chiTieu.DefaultPrice = price;
                    if (cbNhomXetNghiem.HasValue) chiTieu.IDNhomXN = int.Parse(cbNhomXetNghiem.SelectedValue);
                    chiTieu.HinhThucGioiHanChuan = cbHinhThucGioiHanChuan.SelectedValue;
                    chiTieu.IDDonViTinh = null;
                    if (cbDonViTinh.HasValue)
                        chiTieu.IDDonViTinh = int.Parse(cbDonViTinh.SelectedValue);
                    chiTieu.IDDonViTinhTien = null;
                    if (cbDonViTinhTien.HasValue)
                        chiTieu.IDDonViTinhTien = int.Parse(cbDonViTinhTien.SelectedValue);
                    chiTieu.TrangThaiSuDung = chkTrangThaiSuDung.Checked;
                    chiTieu.SoThongTu = txtSoThongTu.Text;
                    if (dateNgayThongTu.DateValue != DateTime.MinValue)
                        chiTieu.NgayThongTu = dateNgayThongTu.DateValue;
                    chiTieu.Italic = chkInNghieng.Checked;
                    if (!string.IsNullOrEmpty(txtGiaTriBe.Text))
                    {
                        chiTieu.GiaTriBe = decimal.Parse(txtGiaTriBe.Text.Replace(".", "").Replace(",", "."));
                    }
                    chiTieu = ChiTieuManager.InsertChiTieuXetNghiem(chiTieu);

                    #region Log
                    CurrentLog.Clear();
                    CurrentLog.AddNewLogFunction(RightPageName.ChiTieu_Insert,
                                                 string.Format("Cập nhật chỉ tiêu xét nghiệm <b>{0}</b> ", chiTieu.TenTaiCacKhoa));

                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Nhóm xét nghiệm",
                                               TblChiTieuXetNghiem.Columns.IDNhomXN,
                                               chiTieu.TenNhomXetNghiem);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Tên tại khoa",
                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                             chiTieu.TenTaiCacKhoa);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Số thông tư",
                                        TblChiTieuXetNghiem.Columns.SoThongTu,
                                        txtSoThongTu.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Ngày thông tư ",
                                             TblChiTieuXetNghiem.Columns.NgayThongTu,
                                             chiTieu.NgayThongTu.HasValue && chiTieu.NgayThongTu.Value != DateTime.MinValue ? chiTieu.NgayThongTu.Value.ToString("dd/MM/yyyy") : "");
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Tên theo thông tư",
                                             TblChiTieuXetNghiem.Columns.TenTrongTTQD,
                                             chiTieu.TenTrongTTQD);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "STT",
                                             TblChiTieuXetNghiem.Columns.Stt,
                                             txtSTT.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Hóa / vi sinh",
                                                TblChiTieuXetNghiem.Columns.HoaSinh,
                                                cbHoaSinh.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Đơn giá",
                                             TblChiTieuXetNghiem.Columns.DefaultPrice,
                                             txtDonGia.Text);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Đơn vị tính",
                                             TblChiTieuXetNghiem.Columns.IDDonViTinh,
                                             cbDonViTinh.DisplayText);
                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Đơn vị tính tiền",
                                             TblChiTieuXetNghiem.Columns.IDDonViTinhTien,
                                             cbDonViTinhTien.DisplayText);

                    CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, "Trạng thái sử dụng",
                                             TblChiTieuXetNghiem.Columns.TrangThaiSuDung,
                                             chkTrangThaiSuDung.Checked ? "Có" : "Không");
                    CurrentLog.SaveLog(RightPageName.ChiTieu_Insert);
                    #endregion
                    Session["ChiTieu_CreateNew"] = chiTieu;
                    Response.Redirect(string.Format("/chi-tieu-xet-nghiem/{0}", chiTieu.Id)); ;
                }
            }
        }

        private void SavePhuongPhap(TblChiTieuXetNghiem chiTieu)
        {
            if (chiTieu != null)
            {
                string phuongPhap = "";
                bool update = false;
                foreach (GridViewRow row in grvPhuongPhap.Rows)
                {
                    HiddenField hiddenID = row.FindControl("hiddenID") as HiddenField;
                    HiddenField hiddenPhuongPhapID = row.FindControl("hiddenPhuongPhapID") as HiddenField;
                    HiddenField hiddenName = row.FindControl("hiddenName") as HiddenField;
                    ExtraCheckBox chkItem = row.FindControl("chkItem") as ExtraCheckBox;

                    //CurrentLog.Clear();


                    if (chkItem.Checked)
                    {
                        if (hiddenID != null && string.IsNullOrEmpty(hiddenID.Value))
                        {
                            update = true;
                            TblChiTieuDanhMuc dm = new TblChiTieuDanhMuc();
                            dm.CreatedBy = ApplicationContext.Current.CommonUser.Email;
                            dm.CreatedDate = DateTime.Now;
                            dm.IDChiTieu = chiTieu.Id;
                            dm.IDDanhMuc = int.Parse(hiddenPhuongPhapID.Value);
                            dm.LoaiDanhMuc = "PhuongPhap";
                            dm = ChiTieuManager.InsertChiTieuDanhMuc(dm);
                            hiddenID.Value = dm.Id.ToString();
                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Thêm phương pháp",
                                                hiddenPhuongPhapID.Value,
                                                hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                        }
                        phuongPhap += string.Format("{0} .::. ", hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                    }
                    else
                    {
                        if (hiddenID != null && !string.IsNullOrEmpty(hiddenID.Value))
                        {
                            TblChiTieuDanhMuc dm = ChiTieuManager.GetChiTieuDanhMucByID(hiddenID.Value);
                            if (dm != null && dm.IDChiTieu.HasValue && dm.IDChiTieu.Value == chiTieu.Id)
                            {
                                update = true;
                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Bỏ phương pháp",
                                               hiddenPhuongPhapID.Value,
                                               hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                                ChiTieuManager.DeleteChiTieuDanhMuc(dm.Id);
                            }
                        }
                    }
                }
                chiTieu.PhuongPhapList = phuongPhap;
            }
        }

        private void SaveNhom(TblChiTieuXetNghiem chiTieu)
        {
            if (chiTieu != null)
            {
                string nhom = "";
                bool update = false;
                foreach (GridViewRow row in grvNhom.Rows)
                {
                    HiddenField hiddenID = row.FindControl("hiddenID") as HiddenField;
                    HiddenField hiddenNhomID = row.FindControl("hiddenNhomID") as HiddenField;
                    HiddenField hiddenName = row.FindControl("hiddenName") as HiddenField;
                    ExtraCheckBox chkItem = row.FindControl("chkItem") as ExtraCheckBox;

                    //CurrentLog.Clear();


                    if (chkItem.Checked)
                    {
                        if (hiddenID != null && string.IsNullOrEmpty(hiddenID.Value))
                        {
                            update = true;
                            TblChiTieuDanhMuc dm = new TblChiTieuDanhMuc();
                            dm.CreatedBy = ApplicationContext.Current.CommonUser.Email;
                            dm.CreatedDate = DateTime.Now;
                            dm.IDChiTieu = chiTieu.Id;
                            dm.IDDanhMuc = int.Parse(hiddenNhomID.Value);
                            dm.LoaiDanhMuc = "Nhom";
                            dm = ChiTieuManager.InsertChiTieuDanhMuc(dm);
                            hiddenID.Value = dm.Id.ToString();
                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Thêm nhóm xét nghiệm",
                                                hiddenNhomID.Value,
                                                hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                        }
                        nhom += string.Format("{0} .::. ", hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                    }
                    else
                    {
                        if (hiddenID != null && !string.IsNullOrEmpty(hiddenID.Value))
                        {
                            TblChiTieuDanhMuc dm = ChiTieuManager.GetChiTieuDanhMucByID(hiddenID.Value);
                            if (dm != null && dm.IDChiTieu.HasValue && dm.IDChiTieu.Value == chiTieu.Id)
                            {
                                update = true;
                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Bỏ nhóm xét nghiệm",
                                               hiddenNhomID.Value,
                                               hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                                ChiTieuManager.DeleteChiTieuDanhMuc(dm.Id);
                            }
                        }
                    }
                }
                chiTieu.NhomList = nhom;
            }
        }

        private void SaveChungNhan(TblChiTieuXetNghiem chiTieu)
        {
            if (chiTieu != null)
            {
                string chungNhan = "";
                bool update = false;
                foreach (GridViewRow row in grvChungNhan.Rows)
                {
                    HiddenField hiddenID = row.FindControl("hiddenID") as HiddenField;
                    HiddenField hiddenChungNhanID = row.FindControl("hiddenChungNhanID") as HiddenField;
                    HiddenField hiddenName = row.FindControl("hiddenName") as HiddenField;
                    ExtraCheckBox chkItem = row.FindControl("chkItem") as ExtraCheckBox;

                    //CurrentLog.Clear();


                    if (chkItem.Checked)
                    {
                        if (hiddenID != null && string.IsNullOrEmpty(hiddenID.Value))
                        {
                            update = true;
                            TblChiTieuDanhMuc dm = new TblChiTieuDanhMuc();
                            dm.CreatedBy = ApplicationContext.Current.CommonUser.Email;
                            dm.CreatedDate = DateTime.Now;
                            dm.IDChiTieu = chiTieu.Id;
                            dm.IDDanhMuc = int.Parse(hiddenChungNhanID.Value);
                            dm.LoaiDanhMuc = "ChungNhan";
                            dm = ChiTieuManager.InsertChiTieuDanhMuc(dm);
                            hiddenID.Value = dm.Id.ToString();
                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Thêm chứng nhận",
                                                hiddenChungNhanID.Value,
                                                hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                        }
                        chungNhan += string.Format("{0} .::. ", hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                    }
                    else
                    {
                        if (hiddenID != null && !string.IsNullOrEmpty(hiddenID.Value))
                        {
                            TblChiTieuDanhMuc dm = ChiTieuManager.GetChiTieuDanhMucByID(hiddenID.Value);
                            if (dm != null && dm.IDChiTieu.HasValue && dm.IDChiTieu.Value == chiTieu.Id)
                            {
                                update = true;
                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, "Bỏ chứng nhận",
                                               hiddenChungNhanID.Value,
                                               hiddenName.Value.Replace(".::", "<").Replace("::.", ">"));
                                ChiTieuManager.DeleteChiTieuDanhMuc(dm.Id);
                            }
                        }
                    }
                }
                chiTieu.ChungNhanList = chungNhan;
            }
        }

        private void LoadChiTieu(TblChiTieuXetNghiem chiTieu)
        {
            if (chiTieu != null)
            {

                divDetail.Visible = true;
                hiddenEditID.Value = chiTieu.Id.ToString();
                txtMTenTaiKhoa.Text = chiTieu.TenTaiCacKhoa;
                //txtTenTaiKhoa.Text = CategoryManager.ReadName(chiTieu.TenTaiCacKhoa);
                txtMTenTrongThongTu.Text = chiTieu.TenTrongTTQD;
                txtSTT.Text = chiTieu.Stt;
                cbHoaSinh.SelectedValue = chiTieu.HoaSinh;
                txtDonGia.Text = "0";
                if (chiTieu.DefaultPrice.HasValue) txtDonGia.Text = chiTieu.DefaultPrice.Value.ToStringWithLanguage("N0");

                if (chiTieu.IDDonViTinh.HasValue)
                {
                    ListItem item = cbDonViTinh.Items.FindByValue(chiTieu.IDDonViTinh.Value.ToString());
                    if (item != null) cbDonViTinh.SelectedValue = chiTieu.IDDonViTinh.Value.ToString();
                }

                if (chiTieu.IDDonViTinhTien.HasValue)
                {
                    ListItem item = cbDonViTinhTien.Items.FindByValue(chiTieu.IDDonViTinhTien.Value.ToString());
                    if (item != null) cbDonViTinhTien.SelectedValue = chiTieu.IDDonViTinhTien.Value.ToString();
                }

                if (chiTieu.IDNhomXN.HasValue)
                {
                    ListItem item = cbNhomXetNghiem.Items.FindByValue(chiTieu.IDNhomXN.Value.ToString());
                    if (item != null) cbNhomXetNghiem.SelectedValue = chiTieu.IDNhomXN.Value.ToString();
                }

                cbHinhThucGioiHanChuan.SelectedValue = chiTieu.HinhThucGioiHanChuan;

                txtSoThongTu.Text = chiTieu.SoThongTu;

                if (chiTieu.NgayThongTu.HasValue && chiTieu.NgayThongTu.Value != DateTime.MinValue)
                    dateNgayThongTu.DateValue = chiTieu.NgayThongTu.Value;

                chkTrangThaiSuDung.Checked = chiTieu.TrangThaiSuDung.HasValue && chiTieu.TrangThaiSuDung.Value;
                chkInNghieng.Checked = chiTieu.Italic.HasValue && chiTieu.Italic.Value;
                if (chiTieu.GiaTriBe.HasValue) txtGiaTriBe.Text = chiTieu.GiaTriBe.Value.ToStringWithLanguage();
                //LoadNhom(chiTieu.Id);
                LoadPhuongPhap(chiTieu.Id);
                LoadChungChi(chiTieu.Id);
                LoadBienBan(chiTieu.Id);
                LoadChiTieuQuyChuan();
            }
        }
        private void LoadChiTieu()
        {

            hiddenEditID.Value = "";
            if (CurrentID.HasValue)
            {
                TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(CurrentID.Value);
                LoadChiTieu(chiTieu);
                LoadCQuyChuanChungChi();
            }
        }

        private void LoadNhom(int idChiTieu)
        {
            List<TblCatNhomXetNghiem> nhomXN = CategoryManager.GetNhomXetNghiemChiTieu(idChiTieu, true);
            grvNhom.DataSource = nhomXN;
            grvNhom.DataBind();
        }

        private void LoadPhuongPhap(int idChiTieu)
        {

            List<TblChiTieuPhuongPhap> phuongPhapCol = XetNghiemManager.GetChiTieuPhuongPhapByChiTieuID(idChiTieu, null, null);

            if (grvPhuongPhap.IsInsert)
                phuongPhapCol.Insert(0, new TblChiTieuPhuongPhap());
            grvPhuongPhap.RenderLoading = false;
            string phuongPhap = "";
            if (phuongPhapCol.Count > 0)
            {

                foreach (TblChiTieuPhuongPhap phuongPhap2 in phuongPhapCol)
                {
                    if (!string.IsNullOrEmpty(phuongPhap2.TenPhuongPhap))
                        phuongPhap += string.Format("{0} .::. ", phuongPhap2.TenPhuongPhap.Replace(".::", "<").Replace("::.", ">"));
                }

                grvPhuongPhap.DataSource = phuongPhapCol;
                grvPhuongPhap.RenderLoading = true;
                grvPhuongPhap.DataBind();
                grvPhuongPhap.Visible = phuongPhapCol.Count >= 0;
            }
            if (!grvPhuongPhap.IsInsert)
            {
                TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(idChiTieu);
                if (chiTieu != null)
                {
                    chiTieu.PhuongPhapList = phuongPhap;
                    chiTieu = ChiTieuManager.UpdateChiTieuXetNghiem(chiTieu);
                }
            }

            divPhuongPhapFake.Visible = phuongPhapCol.Count == 0;

        }

        private void LoadBienBan(int idChiTieu)
        {

            List<TblChiTieuBienBanGhiChep> phuongPhapCol = ChiTieuManager.GetGhiChepByChiTieu(idChiTieu);

            if (grvBienBanGhiChep.IsInsert)
                phuongPhapCol.Insert(0, new TblChiTieuBienBanGhiChep());
            grvBienBanGhiChep.RenderLoading = false;
            string phuongPhap = "";
            //if (phuongPhapCol.Count > 0)
            {

                //foreach (TblChiTieuPhuongPhap phuongPhap2 in phuongPhapCol)
                //{
                //    if (!string.IsNullOrEmpty(phuongPhap2.TenPhuongPhap))
                //        phuongPhap += string.Format("{0} .::. ", phuongPhap2.TenPhuongPhap.Replace(".::", "<").Replace("::.", ">"));
                //}

                grvBienBanGhiChep.DataSource = phuongPhapCol;
                grvBienBanGhiChep.RenderLoading = true;
                grvBienBanGhiChep.DataBind();
                grvBienBanGhiChep.Visible = phuongPhapCol.Count >= 0;
            }
            //if (!grvBienBanGhiChep.IsInsert)
            //{
            //    TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(idChiTieu);
            //    if (chiTieu != null)
            //    {
            //        chiTieu.PhuongPhapList = phuongPhap;
            //        chiTieu = ChiTieuManager.UpdateChiTieuXetNghiem(chiTieu);
            //    }
            //}

            divBienBanPhaKe.Visible = phuongPhapCol.Count == 0;

        }

        private void LoadChungChi(int idChiTieu)
        {
            List<TblCatChungChi> chungNhanCol = CategoryManager.GetChungChiChoChiTieu(idChiTieu, true);
            grvChungNhan.DataSource = chungNhanCol;
            grvChungNhan.DataBind();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("/chi-tieu-xet-nghiem/0");
        }

        protected string GetTabVisible(string tabName, string dfClass)
        {
            return string.Format("{0} {1}", dfClass, hiddenTab.Value == tabName ? "active" : "");
        }

        protected void grvNhom_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null && e.Row.DataItem is TblCatNhomXetNghiem)
            {
                TblCatNhomXetNghiem nhomXN = e.Row.DataItem as TblCatNhomXetNghiem;
                HiddenField hiddenID = e.Row.FindControl("hiddenID") as HiddenField;
                HiddenField hiddenNhomID = e.Row.FindControl("hiddenNhomID") as HiddenField;
                HiddenField hiddenName = e.Row.FindControl("hiddenName") as HiddenField;

                ExtraCheckBox chkItem = e.Row.FindControl("chkItem") as ExtraCheckBox;


                if (hiddenNhomID != null) hiddenNhomID.Value = nhomXN.Id.ToString();
                if (hiddenID != null && nhomXN.IDRef.HasValue && nhomXN.IDRef.Value > 0)
                    hiddenID.Value = nhomXN.IDRef.Value.ToString();
                if (hiddenName != null) hiddenName.Value = nhomXN.Ten;
                if (chkItem != null) chkItem.Checked = nhomXN.IDRef.HasValue && nhomXN.IDRef.Value > 0;
            }
        }

        protected void grvChungNhan_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null && e.Row.DataItem is TblCatChungChi)
            {
                TblCatChungChi chungNhan = e.Row.DataItem as TblCatChungChi;
                HiddenField hiddenID = e.Row.FindControl("hiddenID") as HiddenField;
                HiddenField hiddenChungNhanID = e.Row.FindControl("hiddenChungNhanID") as HiddenField;
                HiddenField hiddenName = e.Row.FindControl("hiddenName") as HiddenField;

                ExtraCheckBox chkItem = e.Row.FindControl("chkItem") as ExtraCheckBox;


                if (hiddenChungNhanID != null) hiddenChungNhanID.Value = chungNhan.Id.ToString();
                if (hiddenID != null && chungNhan.IDRef.HasValue && chungNhan.IDRef.Value > 0)
                    hiddenID.Value = chungNhan.IDRef.Value.ToString();
                if (hiddenName != null) hiddenName.Value = chungNhan.Ten;
                if (chkItem != null) chkItem.Checked = chungNhan.IDRef.HasValue && chungNhan.IDRef.Value > 0;
            }
        }

        protected void grvPhuongPhap_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvPhuongPhap.CancelAllCommand();
            LoadPhuongPhap(CurrentID.Value);
        }
        protected void grvPhuongPhap_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null && e.Row.DataItem is TblChiTieuPhuongPhap)
            {
                TblChiTieuPhuongPhap phuongPhap = e.Row.DataItem as TblChiTieuPhuongPhap;
                ExtraComboBox cbEditThongTu = e.Row.FindControl("cbEditThongTu") as ExtraComboBox;
                ExtraComboBox cbEditPhuongPhap = e.Row.FindControl("cbEditPhuongPhap") as ExtraComboBox;
                ExtraTextBox txtEditSTT = e.Row.FindControl("txtEditSTT") as ExtraTextBox;
                ExtraTextBox txtEditDonGia = e.Row.FindControl("txtEditDonGia") as ExtraTextBox;
                HiddenField hdfEditID = e.Row.FindControl("hdfEditID") as HiddenField;

                if (hdfEditID != null)
                    hdfEditID.Value = phuongPhap.Id.ToString();

                if (cbEditThongTu != null)
                {
                    List<TblCatThongTu> thongTuCol = CategoryManager.GetThongTuAll(true);
                    cbEditThongTu.DataSource = thongTuCol;
                    cbEditThongTu.DataValueField = TblCatThongTu.Columns.Id;
                    cbEditThongTu.DataTextField = TblCatThongTu.Columns.Ten;
                    cbEditThongTu.DataBind();

                    if (phuongPhap.IDThongTu.HasValue)
                    {
                        ListItem item = cbEditThongTu.Items.FindByValue(phuongPhap.IDThongTu.Value.ToString());
                        if (item != null) cbEditThongTu.SelectedValue = phuongPhap.IDThongTu.Value.ToString();
                    }
                }

                if (cbEditPhuongPhap != null)
                {
                    List<TblCatPhuongPhap> phuongPhapCol = CategoryManager.GetPhuongPhapAll(true);

                    foreach (TblCatPhuongPhap pp in phuongPhapCol)
                    {
                        string ten = pp.TenTaiLieuGoc;
                        if (string.IsNullOrEmpty(ten)) ten = pp.Ten;
                        ListItem item = new ListItem();
                        item.Text = ten;
                        item.Value = pp.Id.ToString();
                        string subText = string.Format("<br/><span class='pp-item'></span><span style='margin-left:5px;font-size:15px;vertical-align: bottom;'> {0}</span>", ten);
                        item.Attributes.Add("data-subtext", subText);
                        cbEditPhuongPhap.Items.Add(item);
                    }

                    if (phuongPhap.IDPhuongPhap.HasValue)
                    {
                        ListItem item = cbEditPhuongPhap.Items.FindByValue(phuongPhap.IDPhuongPhap.Value.ToString());
                        if (item != null) cbEditPhuongPhap.SelectedValue = phuongPhap.IDPhuongPhap.Value.ToString();
                    }
                }

                if (phuongPhap.DonGia.HasValue && txtEditDonGia != null) txtEditDonGia.Text = phuongPhap.DonGia.Value.ToStringWithLanguage();
                if (txtEditSTT != null) txtEditSTT.Text = phuongPhap.Stt;
            }
        }
        protected void grvChiTieuQuyChuan_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEditChiTieuQuyChuan = e.Row.FindControl("btnEditChiTieuQuyChuan") as ExtraButton;
            if (btnEditChiTieuQuyChuan != null)
                btnEditChiTieuQuyChuan.Visible = GetButtonRole("Setting") && string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value);
            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null)
                btnDelete.Visible = GetButtonRole("Setting") && string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value);

            if (e.Row.DataItem != null && e.Row.DataItem is TblChiTieuQuyChuan)
            {
                TblChiTieuQuyChuan ctqc = e.Row.DataItem as TblChiTieuQuyChuan;
                if (ctqc.Id.ToString() == hiddenChiTieuQuyChuanID.Value)
                    e.Row.CssClass = "edit";
            }
        }

        protected void grvPhuongPhap_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            if (btnEdit != null)
                btnEdit.Visible = GetButtonRole("Setting") && string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value);
            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null)
                btnDelete.Visible = GetButtonRole("Setting") && string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value);

        }

        protected void grvPhuongPhap_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvPhuongPhap.Rows[e.RowIndex];
            if (row != null)
            {
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;
                ExtraComboBox cbEditThongTu = row.FindControl("cbEditThongTu") as ExtraComboBox;
                ExtraComboBox cbEditPhuongPhap = row.FindControl("cbEditPhuongPhap") as ExtraComboBox;
                ExtraTextBox txtEditSTT = row.FindControl("txtEditSTT") as ExtraTextBox;
                ExtraTextBox txtEditDongia = row.FindControl("txtEditDonGia") as ExtraTextBox;

                if (cbEditThongTu != null)
                {
                    if (string.IsNullOrEmpty(cbEditThongTu.SelectedValue))
                    {
                        AddValidPromt(cbEditThongTu.ClientID, "Chọn thông tư");
                        cbEditThongTu.Focus();
                    }
                }

                if (cbEditPhuongPhap != null)
                {
                    if (string.IsNullOrEmpty(cbEditPhuongPhap.SelectedValue))
                    {
                        AddValidPromt(cbEditPhuongPhap.ClientID, "Chọn phương pháp xét nghiệm");
                        cbEditPhuongPhap.Focus();
                    }
                }

                if (string.IsNullOrEmpty(txtEditSTT.Text))
                {
                    AddValidPromt(txtEditSTT.ClientID, "Nhập phương pháp STT");
                    txtEditSTT.Focus();
                }

                decimal donGia = 0;
                if (string.IsNullOrEmpty(txtEditDongia.Text.Trim()))
                {
                    AddValidPromt(txtEditDongia.ClientID, "Nhập đơn giá xét nghiệm");
                    txtEditDongia.Focus();
                }
                else
                {
                    if (!decimal.TryParse(txtEditDongia.Text.Trim().Replace(".", "").Replace(",", "."), out donGia)) donGia = 0;
                    if (donGia <= 0)
                    {
                        AddValidPromt(txtEditDongia.ClientID, "Đơn giá phải > 0");
                        txtEditDongia.Focus();
                    }
                }

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (hdfEditID != null)
                    {
                        if (!grvPhuongPhap.IsInsert)
                        {
                            TblChiTieuPhuongPhap ctpp = XetNghiemManager.GetChiTieuPhuongPhapByID(hdfEditID.Value);
                            if (ctpp != null)
                            {
                                #region Log
                                CurrentLog.Clear();
                                CurrentLog.AddNewLogFunction(RightPageName.ChiTieu_Update, string.Format("Cập nhật phương pháp chỉ tiêu  <b>{0}</b>", ctpp.TenTaiCacKhoa));

                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, string.Format("Tên thông tư"),
                                                TblChiTieuPhuongPhap.Columns.IDThongTu,
                                                ctpp.TenThongTu);
                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, string.Format("Tên phương pháp"),
                                                TblChiTieuPhuongPhap.Columns.IDPhuongPhap,
                                                ctpp.TenPhuongPhap);
                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, string.Format("STT"),
                                                TblChiTieuPhuongPhap.Columns.Stt,
                                                ctpp.Stt);
                                CurrentLog.AddFirstValue(RightPageName.ChiTieu_Update, string.Format("Đơn giá"),
                                                TblChiTieuPhuongPhap.Columns.DonGia,
                                                ctpp.DonGia.Value.ToStringWithLanguage());
                                #endregion

                                List<TblChiTieuPhuongPhap> ppCol = XetNghiemManager.GetChiTieuPhuongPhapByChiTieuID(CurrentID.Value, null, cbEditPhuongPhap.SelectedValue);
                                if (ppCol != null && ppCol.Count > 0)
                                {
                                    TblChiTieuPhuongPhap pp = ppCol.FirstOrDefault(p => p.Id != ctpp.Id);
                                    if (pp != null)
                                    {
                                        ShowNotify("Thông báo", "Đã tồn tại cấu hình này trên hệ thống", NotifyType.Error);
                                        return;
                                    }
                                }
                                ctpp.IDThongTu = int.Parse(cbEditThongTu.SelectedValue);
                                ctpp.IDPhuongPhap = int.Parse(cbEditPhuongPhap.SelectedValue);
                                ctpp.DonGia = donGia;
                                ctpp.Stt = txtEditSTT.Text;
                                ctpp.UpdatedBy = ApplicationContext.Current.CommonUser.Email;
                                ctpp.UpdatedDate = DateTime.Now;

                                ctpp = XetNghiemManager.UpdateChiTieuPhuongPhap(ctpp);
                                #region Log
                                CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                           TblChiTieuPhuongPhap.Columns.IDThongTu,
                                           cbEditThongTu.DisplayText);
                                CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                TblChiTieuPhuongPhap.Columns.IDPhuongPhap,
                                                cbEditPhuongPhap.DisplayText);
                                CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                TblChiTieuPhuongPhap.Columns.Stt,
                                                ctpp.Stt);
                                CurrentLog.AddLastValue(RightPageName.ChiTieu_Update,
                                                TblChiTieuPhuongPhap.Columns.DonGia,
                                                ctpp.DonGia.Value.ToStringWithLanguage());

                                CurrentLog.SaveLog(RightPageName.ChiTieu_Update);
                                #endregion
                                ShowNotify("Thông báo", "Cập nhật thông tin phương pháp thành công", NotifyType.Success);
                            }
                            else
                                ShowNotify("Thông báo", "Không tìm thấy thông tin cập nhật", NotifyType.Error);
                        }
                        else
                        {
                            TblChiTieuPhuongPhap ctpp = new TblChiTieuPhuongPhap();


                            List<TblChiTieuPhuongPhap> ppCol = XetNghiemManager.GetChiTieuPhuongPhapByChiTieuID(CurrentID.Value, null, cbEditPhuongPhap.SelectedValue);
                            if (ppCol != null && ppCol.Count > 0)
                            {
                                ShowNotify("Thông báo", "Đã tồn tại cấu hình phương pháp này trên hệ thống", NotifyType.Error);
                                return;
                            }

                            ctpp.IDChiTieu = CurrentID.Value;
                            ctpp.IDThongTu = int.Parse(cbEditThongTu.SelectedValue);
                            ctpp.IDPhuongPhap = int.Parse(cbEditPhuongPhap.SelectedValue);
                            ctpp.DonGia = donGia;
                            ctpp.Stt = txtEditSTT.Text;
                            ctpp.UpdatedBy = ApplicationContext.Current.CommonUser.Email;
                            ctpp.UpdatedDate = DateTime.Now;

                            ctpp = XetNghiemManager.InsertChiTieuPhuongPhap(ctpp);


                            #region Log
                            CurrentLog.Clear();
                            CurrentLog.AddNewLogFunction(RightPageName.ChiTieu_Insert, string.Format("Thêm mới phương pháp cho chỉ tiêu <b>{0}</b>", ctpp.TenTaiCacKhoa));

                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, string.Format("Tên thông tư"),
                                            TblChiTieuPhuongPhap.Columns.IDThongTu,
                                            cbEditThongTu.DisplayText);
                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, string.Format("Tên phương pháp"),
                                            TblChiTieuPhuongPhap.Columns.IDPhuongPhap,
                                            cbEditPhuongPhap.DisplayText);
                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, string.Format("STT"),
                                            TblChiTieuPhuongPhap.Columns.Stt,
                                            ctpp.Stt);
                            CurrentLog.AddFirstValue(RightPageName.ChiTieu_Insert, string.Format("Đơn giá"),
                                            TblChiTieuPhuongPhap.Columns.DonGia,
                                            ctpp.DonGia.Value.ToStringWithLanguage());
                            CurrentLog.SaveLog(RightPageName.ChiTieu_Insert);
                            #endregion
                            ShowNotify("Thông báo", "Thêm mới phương pháp thông tin thành công", NotifyType.Success);

                        }
                    }
                }


            }
            if (PageIsValid)
            {
                grvPhuongPhap.CancelAllCommand();
                LoadPhuongPhap(CurrentID.Value);
            }
            else ShowValidPromt();
        }
        protected void cbHinhThucGioiHan_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtGioiHan1.Visible = txtGioiHan2.Visible = txtGioiHan3.Visible = txtGioiHan4.Visible =
            txtGioiHan1P.Visible = txtGioiHan2P.Visible = txtGioiHan3P.Visible = txtGioiHan4P.Visible = cbHinhThucGioiHan.SelectedValue == "NhapSo";
            cbGioiHan1.Visible = cbGioiHan2.Visible = cbGioiHan3.Visible = cbGioiHan4.Visible = cbHinhThucGioiHan.SelectedValue != "NhapSo";
            cbPhepSoSanh.Enabled = cbHinhThucGioiHan.SelectedValue == "NhapSo";
            cbPhepSoSanh.SelectedValue = "BGH";
        }

        protected string GetGioiHan(object data, int gioiHan)
        {
            string rs = "";
            if (data != null && data is TblChiTieuQuyChuan)
            {
                TblChiTieuQuyChuan chiTieu = data as TblChiTieuQuyChuan;
                if (gioiHan == 1 && chiTieu.LoaiHinhGioiHan == "NhapSo" && chiTieu.GioiHan1.HasValue) rs = chiTieu.TenGioiHan1;//.ToString().Replace(".", ",");
                if (gioiHan == 2 && chiTieu.LoaiHinhGioiHan == "NhapSo" && chiTieu.GioiHan2.HasValue) rs = chiTieu.TenGioiHan2;//.ToString().Replace(".", ",");
                if (gioiHan == 3 && chiTieu.LoaiHinhGioiHan == "NhapSo" && chiTieu.GioiHan3.HasValue) rs = chiTieu.TenGioiHan3;//.ToString().Replace(".", ",");
                if (gioiHan == 4 && chiTieu.LoaiHinhGioiHan == "NhapSo" && chiTieu.GioiHan4.HasValue) rs = chiTieu.TenGioiHan4;//.ToString().Replace(".", ",");

                if (gioiHan == 1 && chiTieu.LoaiHinhGioiHan != "NhapSo") rs = chiTieu.TenDanhMuc1;
                if (gioiHan == 2 && chiTieu.LoaiHinhGioiHan != "NhapSo") rs = chiTieu.TenDanhMuc2;
                if (gioiHan == 3 && chiTieu.LoaiHinhGioiHan != "NhapSo") rs = chiTieu.TenDanhMuc3;
                if (gioiHan == 4 && chiTieu.LoaiHinhGioiHan != "NhapSo") rs = chiTieu.TenDanhMuc4;
            }
            return rs;
        }

        protected string GetTrangThaiDat(object data)
        {
            string rs = "";
            if (data != null && data is TblChiTieuQuyChuan)
            {
                TblChiTieuQuyChuan chiTieu = data as TblChiTieuQuyChuan;
                rs = "Kết quả ";
                ListItem it = cbPhepSoSanh.Items.FindByValue(chiTieu.PhepSoSanh);
                if (it != null) rs += "<b>" + it.Text + "</b>";
                rs += " thì <b>" + (chiTieu.GiaTriDat.HasValue && chiTieu.GiaTriDat.Value ? "Đạt" : "Không đạt") + "</b>";
            }
            return rs;

        }

        protected void btnSaveChiTieuQuyChuan_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(cbDonViTinhQuyChuan.SelectedValue) || cbDonViTinhQuyChuan.SelectedValue == "-1")
            //{
            //    ShowNotify("Thông báo", "Vui lòng chọn đơn vị tính theo quy chuẩn", NotifyType.Warning);
            //    cbDonViTinhQuyChuan.Focus();
            //    return;
            //}
            decimal gh1 = 0, gh2 = 0, gh3 = 0, gh4 = 0, gh1P = 0, gh2P = 0, gh3P = 0, gh4P = 0;
            int? gh1t = null, gh2t = null, gh3t = null, gh4t = null;

            int maxColumn = 0;
            #region validation
            if (cbHinhThucGioiHan.SelectedValue == "NhapSo")
            {
                if (!decimal.TryParse(txtGioiHan1.Text.Replace(".", "").Replace(",", "."), out gh1)) gh1 = -1;
                if (!decimal.TryParse(txtGioiHan2.Text.Replace(".", "").Replace(",", "."), out gh2)) gh2 = -1;
                if (!decimal.TryParse(txtGioiHan3.Text.Replace(".", "").Replace(",", "."), out gh3)) gh3 = -1;
                if (!decimal.TryParse(txtGioiHan4.Text.Replace(".", "").Replace(",", "."), out gh4)) gh4 = -1;

                if (!decimal.TryParse(txtGioiHan1P.Text.Replace(".", "").Replace(",", "."), out gh1P)) gh1P = -1;
                if (!decimal.TryParse(txtGioiHan2P.Text.Replace(".", "").Replace(",", "."), out gh2P)) gh2P = -1;
                if (!decimal.TryParse(txtGioiHan3P.Text.Replace(".", "").Replace(",", "."), out gh3P)) gh3P = -1;
                if (!decimal.TryParse(txtGioiHan4P.Text.Replace(".", "").Replace(",", "."), out gh4P)) gh4P = -1;
            }
            else
            {
                if (cbGioiHan1.HasValue)
                    gh1t = int.Parse(cbGioiHan1.SelectedValue);
                if (cbGioiHan2.HasValue)
                    gh2t = int.Parse(cbGioiHan2.SelectedValue);
                if (cbGioiHan3.HasValue)
                    gh3t = int.Parse(cbGioiHan3.SelectedValue);
                if (cbGioiHan4.HasValue)
                    gh4t = int.Parse(cbGioiHan4.SelectedValue);
            }
            #endregion
            if (!string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value))
            {
                TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByChiTieuID(CurrentID.Value, cbQuyChuan.SelectedValue);
                if (ctqc != null && ctqc.Id.ToString() != hiddenChiTieuQuyChuanID.Value)
                {
                    ShowNotify("Thông báo", "Đã tồn tại cài đặt giới hạn cho quy chuẩn này", NotifyType.Error);
                    return;
                }
                if (ctqc == null)
                {
                    ShowNotify("Thông báo", "Không tìm thấy cài đặt giới hạn cho quy chuẩn này", NotifyType.Error);
                    return;
                }
                if (ctqc != null)
                {
                    ctqc.IDQuyChuan = int.Parse(cbQuyChuan.SelectedValue);
                    if (cbHinhThucGioiHan.SelectedValue == "NhapSo")
                    {
                        ctqc.GioiHan1 = null;
                        if (gh1 > -1)
                        {
                            maxColumn++;
                            ctqc.GioiHan1 = gh1;
                        }

                        ctqc.GioiHan2 = null;
                        if (gh2 > -1) { maxColumn++; ctqc.GioiHan2 = gh2; }

                        ctqc.GioiHan3 = null;
                        if (gh3 > -1) { maxColumn++; ctqc.GioiHan3 = gh3; }

                        ctqc.GioiHan4 = null;
                        if (gh4 > -1) { maxColumn++; ctqc.GioiHan4 = gh4; }


                        ctqc.GioiHan1P = null;
                        if (gh1P > -1)
                            ctqc.GioiHan1P = gh1P;
                        ctqc.GioiHan2P = null;
                        if (gh2P > -1)
                            ctqc.GioiHan2P = gh2P;
                        ctqc.GioiHan3P = null;
                        if (gh3P > -1)
                            ctqc.GioiHan3P = gh3P;
                        ctqc.GioiHan4P = null;
                        if (gh4P > -1)
                            ctqc.GioiHan4P = gh4P;
                    }
                    else
                    {
                        ctqc.GiaTri1 = null;
                        if (gh1t.HasValue)
                        {
                            maxColumn++; ctqc.GiaTri1 = gh1t;
                        }
                        if (gh2t.HasValue)
                        {
                            maxColumn++; ctqc.GiaTri2 = gh2t;
                        }
                        if (gh3t.HasValue)
                        {
                            maxColumn++; ctqc.GiaTri3 = gh3t;
                        }
                        if (gh4t.HasValue)
                        {
                            maxColumn++; ctqc.GiaTri4 = gh4t;
                        }
                    }
                    ctqc.IDDonViTinh = null;
                    if (cbDonViTinhQuyChuan.HasValue)
                        ctqc.IDDonViTinh = int.Parse(cbDonViTinhQuyChuan.SelectedValue);
                    ctqc.LoaiHinhGioiHan = cbHinhThucGioiHan.SelectedValue;
                    ctqc.PhepSoSanh = cbPhepSoSanh.SelectedValue;
                    ctqc.HienThiDau = chkHienThiDau.Checked;
                    ctqc.GiaTriDat = cbGiaTriDat.SelectedValue == "1";
                    ctqc.UpdatedBy = ApplicationContext.Current.CommonUser.Email;
                    ctqc.UpdatedDate = DateTime.Now;
                    ctqc.GiaTriBe = 0;
                    if (!string.IsNullOrEmpty(txtGiaTriDauBe.Text))
                    {
                        decimal d = -1;
                        if (decimal.TryParse(txtGiaTriDauBe.Text.Trim().Replace(".", "").Replace(",", "."), out d)) ctqc.GiaTriBe = d;
                    }
                    ctqc = ChiTieuManager.UpdateChiTieuQuyChuan(ctqc);
                    ChiTieuManager.UpdateChiTieuQuyChuanChoCacMau(ctqc, cbDonViTinhQuyChuan.SelectedItem != null ? cbDonViTinhQuyChuan.SelectedItem.Text : "");
                    RefreshCTQC();
                    LoadChiTieuQuyChuan(maxColumn);
                    lnkCanCelChiTieuQuyChuan.Visible = false;
                    ShowNotify("Thông báo", "Cập nhật Quy chuẩn cho chỉ tiêu xét nghiệm thành công", NotifyType.Success);
                    cbQuyChuan.Enabled = true;
                }
            }
            else
            {
                if (CurrentID.HasValue)
                {
                    TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByChiTieuID(CurrentID.Value, cbQuyChuan.SelectedValue);
                    if (ctqc == null)
                    {
                        ctqc = new TblChiTieuQuyChuan();
                        ctqc.IDChiTieu = CurrentID.Value;
                        ctqc.IDQuyChuan = int.Parse(cbQuyChuan.SelectedValue);
                        if (cbHinhThucGioiHan.SelectedValue == "NhapSo")
                        {
                            ctqc.GioiHan1 = null;
                            if (gh1 > -1) { maxColumn++; ctqc.GioiHan1 = gh1; }

                            ctqc.GioiHan2 = null;
                            if (gh2 > -1) { maxColumn++; ctqc.GioiHan2 = gh2; }

                            ctqc.GioiHan3 = null;
                            if (gh3 > -1) { maxColumn++; ctqc.GioiHan3 = gh3; }

                            ctqc.GioiHan4 = null;
                            if (gh4 > -1) { maxColumn++; ctqc.GioiHan4 = gh4; }


                            ctqc.GioiHan1P = null;
                            if (gh1P > -1)
                                ctqc.GioiHan1P = gh1P;
                            ctqc.GioiHan2P = null;
                            if (gh2P > -1)
                                ctqc.GioiHan2P = gh2P;
                            ctqc.GioiHan3P = null;
                            if (gh3P > -1)
                                ctqc.GioiHan3P = gh3P;
                            ctqc.GioiHan4P = null;
                            if (gh4P > -1)
                                ctqc.GioiHan4P = gh4P;
                        }
                        else
                        {
                            ctqc.GiaTri1 = null;
                            if (gh1t.HasValue)
                            {
                                maxColumn++; ctqc.GiaTri1 = gh1t;
                            }
                            if (gh2t.HasValue)
                            {
                                maxColumn++; ctqc.GiaTri2 = gh1t;
                            }
                            if (gh3t.HasValue)
                            {
                                maxColumn++; ctqc.GiaTri3 = gh1t;
                            }
                            if (gh4t.HasValue)
                            {
                                maxColumn++; ctqc.GiaTri4 = gh1t;
                            }
                        }
                        ctqc.IDDonViTinh = null;
                        if (cbDonViTinhQuyChuan.HasValue)
                            ctqc.IDDonViTinh = int.Parse(cbDonViTinhQuyChuan.SelectedValue);
                        ctqc.LoaiHinhGioiHan = cbHinhThucGioiHan.SelectedValue;
                        ctqc.PhepSoSanh = cbPhepSoSanh.SelectedValue;
                        ctqc.HienThiDau = chkHienThiDau.Checked;
                        ctqc.GiaTriDat = cbGiaTriDat.SelectedValue == "1";
                        ctqc.CreatedBy = ctqc.UpdatedBy = ApplicationContext.Current.CommonUser.Email;
                        ctqc.CreatedDate = ctqc.UpdatedDate = DateTime.Now;
                        ctqc.GiaTriBe = 0;
                        if (!string.IsNullOrEmpty(txtGiaTriDauBe.Text))
                        {
                            decimal d = -1;
                            if (decimal.TryParse(txtGiaTriDauBe.Text.Trim().Replace(".", "").Replace(",", "."), out d)) ctqc.GiaTriBe = d;
                        }
                        ctqc = ChiTieuManager.InsertChiTieuQuyChuan(ctqc);
                        ChiTieuManager.UpdateChiTieuQuyChuanChoCacMau(ctqc, cbDonViTinhQuyChuan.SelectedItem != null ? cbDonViTinhQuyChuan.SelectedItem.Text : "");
                        RefreshCTQC();
                        LoadChiTieuQuyChuan(maxColumn);
                        lnkCanCelChiTieuQuyChuan.Visible = false;
                        ShowNotify("Thông báo", "Thêm mới Quy chuẩn cho chỉ tiêu xét nghiệm thành công", NotifyType.Success);
                    }
                    else
                        ShowNotify("Thông báo", "Đã tồn tại cài đặt giới hạn cho quy chuẩn này", NotifyType.Error);
                }
            }
            LoadChiTieuQuyChuan();
        }

        private void RefreshCTQC()
        {
            cbQuyChuan.Enabled = true;
            cbPhepSoSanh.Enabled = true;
            chkHienThiDau.Checked = false;
            hiddenChiTieuQuyChuanID.Value = string.Empty;
            txtGioiHan1.Visible = txtGioiHan2.Visible = txtGioiHan3.Visible = txtGioiHan4.Visible =
            txtGioiHan1P.Visible = txtGioiHan2P.Visible = txtGioiHan3P.Visible = txtGioiHan4P.Visible = true;
            cbGioiHan1.Visible = cbGioiHan2.Visible = cbGioiHan3.Visible = cbGioiHan4.Visible = false;
            cbHinhThucGioiHan.SelectedIndex = 0;
            txtGioiHan1.Text = txtGioiHan2.Text = txtGioiHan3.Text = txtGioiHan4.Text =
            txtGioiHan1P.Text = txtGioiHan2P.Text = txtGioiHan3P.Text = txtGioiHan4P.Text = "";
        }

        private void LoadChiTieuQuyChuan(int maxColumn = -1)
        {
            if (CurrentID.HasValue)
            {
                List<TblChiTieuQuyChuan> quyChuanCol = ChiTieuManager.GetChiTieuQuyChuanByChiTieuIDL(CurrentID.Value);
                if (quyChuanCol.Count > 0)
                {

                    List<TblChiTieuQuyChuan> lstEmplty = new List<TblChiTieuQuyChuan>();
                    lstEmplty.Add(new TblChiTieuQuyChuan());
                    grvChiTieuQuyChuan.DataSourceEmpty = lstEmplty;
                    grvChiTieuQuyChuan.DataSource = quyChuanCol;
                    grvChiTieuQuyChuan.DataBind();
                    grvChiTieuQuyChuan.Visible = true;
                    divGridEmpty.Visible = false;

                    string quuyChuan = "";
                    foreach (TblChiTieuQuyChuan qc in quyChuanCol)
                        quuyChuan += string.Format("{0} .::. ", qc.TenQuyChuan.Replace(".::", "<").Replace("::.", ">"));
                    TblChiTieuXetNghiem chiTieu = ChiTieuManager.GetChiTieuXetNghiemByID(CurrentID.Value);
                    if (chiTieu != null)
                    {
                        chiTieu.QuyChuanList = quuyChuan;
                        int oldMax = 0;
                        if (chiTieu.MaxKetQua.HasValue) oldMax = chiTieu.MaxKetQua.Value;
                        if (maxColumn > oldMax) chiTieu.MaxKetQua = maxColumn;

                        chiTieu = ChiTieuManager.UpdateChiTieuXetNghiem(chiTieu);

                        //List<TblChiTieuQuyChuan> qcc = quyChuanCol.FindAll(p => !p.IDDonViTinh.HasValue || (p.IDDonViTinh.HasValue && p.IDDonViTinh.Value <= 0));
                        //if (qcc.Count > 0 && chiTieu.IDDonViTinh.HasValue && chiTieu.IDDonViTinh.Value > 0)
                        //{
                        //    foreach (TblChiTieuQuyChuan qc in qcc) {
                        //        qc.IDDonViTinh = chiTieu.IDDonViTinh;
                        //        ChiTieuManager.UpdateChiTieuQuyChuan(qc);
                        //    }
                        //}
                    }
                }
                else
                {
                    grvChiTieuQuyChuan.Visible = false;
                    divGridEmpty.Visible = true;
                }

            }
        }

        protected void btnEditChiTieuQuyChuan_Click(object sender, EventArgs e)
        {
            if (sender is ExtraButton)
            {
                ExtraButton btn = sender as ExtraButton;
                string cmdA = btn.CommandArgument;
                TblChiTieuQuyChuan ctqc = ChiTieuManager.GetChiTieuQuyChuanByID(cmdA);
                if (ctqc != null)
                {
                    hiddenChiTieuQuyChuanID.Value = ctqc.Id.ToString();
                    ListItem itQC = cbQuyChuan.Items.FindByValue(ctqc.IDQuyChuan.Value.ToString());
                    if (itQC != null) cbQuyChuan.SelectedValue = ctqc.IDQuyChuan.Value.ToString();
                    cbQuyChuan.Enabled = false;
                    cbHinhThucGioiHan.SelectedValue = ctqc.LoaiHinhGioiHan;
                    if (ctqc.IDDonViTinh.HasValue && ctqc.IDDonViTinh.Value > 0)
                    {
                        ListItem item = cbDonViTinhQuyChuan.Items.FindByValue(ctqc.IDDonViTinh.Value.ToString());
                        if (item != null) cbDonViTinhQuyChuan.SelectedValue = ctqc.IDDonViTinh.Value.ToString();
                    }
                    else if (!string.IsNullOrEmpty(cbDonViTinh.SelectedValue)) cbDonViTinhQuyChuan.SelectedValue = cbDonViTinh.SelectedValue;
                    cbGiaTriDat.SelectedIndex = ctqc.GiaTriDat.HasValue && ctqc.GiaTriDat.Value ? 0 : 1;
                    cbHinhThucGioiHan_SelectedIndexChanged(cbHinhThucGioiHan, e);
                    cbPhepSoSanh.SelectedValue = ctqc.PhepSoSanh;
                    if (ctqc.GiaTriBe.HasValue) txtGiaTriDauBe.Text = ctqc.GiaTriBe.Value.ToStringWithLanguage();
                    chkHienThiDau.Checked = ctqc.HienThiDau.HasValue && ctqc.HienThiDau.Value;
                    if (ctqc.LoaiHinhGioiHan == "NhapSo")
                    {
                        cbPhepSoSanh.Enabled = true;
                        if (ctqc.GioiHan1.HasValue) txtGioiHan1.Text = ctqc.GioiHan1.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");
                        if (ctqc.GioiHan2.HasValue) txtGioiHan2.Text = ctqc.GioiHan2.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");
                        if (ctqc.GioiHan3.HasValue) txtGioiHan3.Text = ctqc.GioiHan3.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");
                        if (ctqc.GioiHan4.HasValue) txtGioiHan4.Text = ctqc.GioiHan4.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");

                        if (ctqc.GioiHan1P.HasValue) txtGioiHan1P.Text = ctqc.GioiHan1P.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");
                        if (ctqc.GioiHan2P.HasValue) txtGioiHan2P.Text = ctqc.GioiHan2P.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");
                        if (ctqc.GioiHan3P.HasValue) txtGioiHan3P.Text = ctqc.GioiHan3P.Value.ToStringWithLanguage();//.ToString().Replace(".", ",");
                        if (ctqc.GioiHan4P.HasValue) txtGioiHan4P.Text = ctqc.GioiHan4P.Value.ToStringWithLanguage();
                    }
                    else
                    {
                        cbPhepSoSanh.Enabled = false;
                        if (ctqc.GiaTri1.HasValue)
                        {
                            ListItem it1 = cbGioiHan1.Items.FindByValue(ctqc.GiaTri1.Value.ToString());
                            if (it1 != null) cbGioiHan1.SelectedValue = ctqc.GiaTri1.Value.ToString();
                        }

                        if (ctqc.GiaTri2.HasValue)
                        {
                            ListItem it1 = cbGioiHan2.Items.FindByValue(ctqc.GiaTri2.Value.ToString());
                            if (it1 != null) cbGioiHan2.SelectedValue = ctqc.GiaTri2.Value.ToString();
                        }

                        if (ctqc.GiaTri3.HasValue)
                        {
                            ListItem it1 = cbGioiHan3.Items.FindByValue(ctqc.GiaTri3.Value.ToString());
                            if (it1 != null) cbGioiHan3.SelectedValue = ctqc.GiaTri3.Value.ToString();
                        }

                        if (ctqc.GiaTri4.HasValue)
                        {
                            ListItem it1 = cbGioiHan4.Items.FindByValue(ctqc.GiaTri4.Value.ToString());
                            if (it1 != null) cbGioiHan4.SelectedValue = ctqc.GiaTri4.Value.ToString();
                        }

                    }
                    lnkCanCelChiTieuQuyChuan.Visible = true;
                    LoadChiTieuQuyChuan();
                }
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
                        if (CurrentMessageBoxResult.CommandName == "delete_ChiTieu")
                        {

                            TblChiTieuQuyChuan ChiTieu = CurrentMessageBoxResult.Value as TblChiTieuQuyChuan;
                            if (ChiTieu != null)
                            {

                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa cài đặt quy chuẩn của chỉ tiêu <b>{0}</b>", ChiTieu.TenTaiCacKhoa));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, string.Format("Tên quy chuẩn"),
                                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                             ChiTieu.TenQuyChuan);
                                    resultDelete = ChiTieuManager.DeleteChiTieuQuyChuan(ChiTieu.Id);

                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    LoadChiTieuQuyChuan();
                                    ShowNotify(string.Format("Xóa cài đặt quy chuẩn thành công", CurrentName),
                                                string.Format("Bạn vừa xóa quy chuẩn <b>{0}</b>",
                                                              ChiTieu.TenQuyChuan, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa cài đặt quy chuẩn {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa quy chuẩn <b>{0}</b>",
                                                               ChiTieu.TenQuyChuan, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                        else if (CurrentMessageBoxResult.CommandName == "delete_PhuongPhap")
                        {
                            TblChiTieuPhuongPhap phuongPhap = CurrentMessageBoxResult.Value as TblChiTieuPhuongPhap;
                            if (phuongPhap != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa cài đặt phương pháp của chỉ tiêu <b>{0}</b>", phuongPhap.TenTaiCacKhoa));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, string.Format("Tên phương pháp"),
                                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                             phuongPhap.TenPhuongPhap);
                                    resultDelete = XetNghiemManager.DeleteChiTieuPhuongPhap(phuongPhap.Id);

                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    LoadChiTieuQuyChuan();
                                    ShowNotify(string.Format("Xóa cài đặt phương pháp thành công", CurrentName),
                                                string.Format("Bạn vừa xóa phương pháp <b>{0}</b>",
                                                              phuongPhap.TenPhuongPhap, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                    LoadPhuongPhap(CurrentID.Value);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa cài đặt phương pháp không thành công", CurrentName),
                                               string.Format("Bạn không thể xóa phương pháp <b>{0}</b>",
                                                             phuongPhap.TenPhuongPhap, CurrentName),
                                               NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                        else if (CurrentMessageBoxResult.CommandName == "delete_bienBan")
                        {
                            int idChiTieu = 0;
                            TblChiTieuBienBanGhiChep bienBan = CurrentMessageBoxResult.Value as TblChiTieuBienBanGhiChep;
                            if (bienBan != null)
                            {
                                try
                                {
                                    idChiTieu = bienBan.IDChiTieu.Value;
                                    CurrentLog.AddNewLogFunction(RightPageName.Cat_LM_Delete,
                                                                 string.Format("Xóa cài đặt biên bản ghi chép <b>{0}</b>", bienBan.Ten));
                                    CurrentLog.AddFirstValue(RightPageName.Cat_LM_Delete, string.Format("Tên phương pháp"),
                                                             TblChiTieuXetNghiem.Columns.TenTaiCacKhoa,
                                                             bienBan.Ten);
                                    resultDelete = ChiTieuManager.DeleteChiTieuBienBanGhiChep(bienBan.Id);

                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {

                                    ShowNotify(string.Format("Xóa cài đặt biên bản ghi chép thành công", CurrentName),
                                                string.Format("Bạn vừa xóa đặt biên bản ghi chép"),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.Cat_LM_Delete);
                                    LoadBienBan(CurrentID.Value);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa cài đặt biên bản ghi chép không thành công", CurrentName),
                                               string.Format("Bạn không thể xóa cài đặt biên bản ghi chép"),
                                               NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }

                    }
                }
                else if (cmdName == "tieuChi_confirm")
                {
                    TblChiTieuQuyChuan ChiTieu = ChiTieuManager.GetChiTieuQuyChuanByID(btn.CommandArgument);
                    if (ChiTieu != null)
                    {
                        List<TblMauXetNghiemChiTieu> chiTieuColl = XetNghiemManager.GetMauXetNghiemChiTieuByIDCTQC(ChiTieu.Id);
                        if (chiTieuColl.Count > 0)
                        {
                            ShowNotify("Thông báo", string.Format("Đã có mẫu xét nghiệm sử dụng cấu hình này. Tham khảo mẫu <b>{0}</b>", chiTieuColl[0].MaMau), NotifyType.Error);
                            return;
                        }
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = ChiTieu;
                        result.Submit = true;
                        result.CommandName = "delete_ChiTieu";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa cài đặt quy chuẩn  <b>{0}</b> ?",
                                                     ChiTieu.TenQuyChuan));
                    }
                }
                else if (cmdName == "phuongPhap_confirm")
                {
                    TblChiTieuPhuongPhap pp = XetNghiemManager.GetChiTieuPhuongPhapByID(btn.CommandArgument);
                    if (pp != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = pp;
                        result.Submit = true;
                        result.CommandName = "delete_PhuongPhap";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa cài đặt phương pháp", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa phương pháp  <b>{0}</b> ?",
                                                     pp.TenPhuongPhap));
                    }
                }
                else if (cmdName == "bienBan_confirm")
                {
                    TblChiTieuBienBanGhiChep pp = ChiTieuManager.GetGhiChepByID(btn.CommandArgument);
                    if (pp != null)
                    {
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = pp;
                        result.Submit = true;
                        result.CommandName = "delete_bienBan";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa cài đặt biên bản ghi chép", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa biên bản ghi chép  <b>{0}</b> ?",
                                                     pp.Ten));
                    }
                }
            }
        }

        protected void lnkCanCelChiTieuQuyChuan_Click(object sender, EventArgs e)
        {
            RefreshCTQC();
            lnkCanCelChiTieuQuyChuan.Visible = false;
            LoadChiTieuQuyChuan();

        }

        protected void btnDelete_Click1(object sender, EventArgs e)
        {

        }

        protected void btnAddNewPhuongPhap_Click(object sender, EventArgs e)
        {
            grvPhuongPhap.InsertItem();
            LoadPhuongPhap(CurrentID.Value);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmd = btn.CommandArgument;
                int idx = int.Parse(cmd);
                grvPhuongPhap.EditIndex = idx;
                LoadPhuongPhap(CurrentID.Value);
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            grvPhuongPhap.CancelAllCommand();
            LoadPhuongPhap(CurrentID.Value);

        }



        private List<TblChiTieuQuyChuanChungChi> CHUNGCHIQUYCHUAN
        {
            get
            {
                List<TblChiTieuQuyChuanChungChi> rs = new List<TblChiTieuQuyChuanChungChi>();
                if (CurrentID.HasValue)
                {
                    if (Session["ChungChiQuyChuan"] == null)
                        Session["ChungChiQuyChuan"] = XetNghiemManager.GetQuyChuanChungChiTheoChiTieuID(CurrentID.Value, null, null);
                    rs = Session["ChungChiQuyChuan"] as List<TblChiTieuQuyChuanChungChi>;
                }
                return rs;
            }
        }

        private List<TblChiTieuQuyChuanTanSuat> CHUNGCHITANSUAT
        {
            get
            {
                List<TblChiTieuQuyChuanTanSuat> rs = new List<TblChiTieuQuyChuanTanSuat>();
                if (CurrentID.HasValue)
                {
                    if (Session["ChungChiTanSuat"] == null)
                        Session["ChungChiTanSuat"] = XetNghiemManager.GetTanSuatChungChiTheoChiTieuID(CurrentID.Value, null, null);
                    rs = Session["ChungChiTanSuat"] as List<TblChiTieuQuyChuanTanSuat>;
                }
                return rs;
            }
        }

        private List<TblCatChungChi> GetChungChi(object quyChuanID)
        {
            List<TblCatChungChi> chungChi = CategoryManager.GetChungChiALL(true);
            if (quyChuanID != null)
            {
                List<TblChiTieuQuyChuanChungChi> quyChuan = CHUNGCHIQUYCHUAN.FindAll(p => p.IDQuyChuan.HasValue && p.IDQuyChuan.Value.ToString() == quyChuanID.ToString());
                if (quyChuan.Count > 0)
                {
                    foreach (TblCatChungChi cc in chungChi)
                    {
                        TblChiTieuQuyChuanChungChi r = quyChuan.FirstOrDefault(q => q.IDChungChi.HasValue && q.IDChungChi.Value.ToString() == cc.Id.ToString());
                        if (r != null) cc.IDQuyChuanChungChi = r.Id;
                    }
                }
            }

            return chungChi;
        }

        private List<TanSuatChiTieu> GetTanSuat(object quyChuanID)
        {
            List<TanSuatChiTieu> tanSuatCol = new List<TanSuatChiTieu>();
            string[] tanSuat = { "A", "B", "C" };
            foreach (string str in tanSuat)
            {
                TanSuatChiTieu ts = new TanSuatChiTieu();
                ts.TanSuat = str;
                ts.IDChiTieu = CurrentID;
                ts.IDQuyChuan = int.Parse(quyChuanID.ToString());
                TblChiTieuQuyChuanTanSuat item = CHUNGCHITANSUAT.FirstOrDefault(p => p.IDQuyChuan.HasValue && p.IDQuyChuan.Value.ToString() == quyChuanID.ToString() && p.TanSuat.Trim().ToUpper() == str.Trim().ToUpper());
                if (item != null)
                {
                    ts.Checked = true;
                    ts.IDChiTiet = item.Id;
                }
                tanSuatCol.Add(ts);
            }
            return tanSuatCol;
        }
        private void LoadCQuyChuanChungChi()
        {
            Session["ChungChiQuyChuan"] = null;
            Session["ChungChiTanSuat"] = null;
            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            grvQuyChuan.DataSource = quyChuanCol;
            grvQuyChuan.DataBind();
        }
        protected void repDataChungChi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is TblCatChungChi)
            {
                TblCatChungChi chungChi = e.Item.DataItem as TblCatChungChi;
                HiddenField hiddenChungChiEditID = e.Item.FindControl("hiddenChungChiEditID") as HiddenField;
                if (hiddenChungChiEditID != null) hiddenChungChiEditID.Value = chungChi.Id.ToString();

                CheckBox chkItem = e.Item.FindControl("chkItem") as CheckBox;
                chkItem.Text = chungChi.Ten;
                HiddenField hiddenChiTietID = e.Item.FindControl("hiddenChiTietID") as HiddenField;
                if (hiddenChiTietID != null && chungChi.IDQuyChuanChungChi.HasValue && chungChi.IDQuyChuanChungChi.Value > 0)
                {
                    chkItem.Checked = true;
                    hiddenChiTietID.Value = chungChi.IDQuyChuanChungChi.ToString();
                }
            }
        }

        protected void repDataTanSuat_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null && e.Item.DataItem is TanSuatChiTieu)
            {
                TanSuatChiTieu tanSuat = e.Item.DataItem as TanSuatChiTieu;
                HiddenField hiddenTanSuatEditID = e.Item.FindControl("hiddenTanSuatEditID") as HiddenField;
                if (hiddenTanSuatEditID != null) hiddenTanSuatEditID.Value = tanSuat.TanSuat;

                CheckBox chkItem = e.Item.FindControl("chkItem") as CheckBox;
                chkItem.Text = tanSuat.TanSuat;
                HiddenField hiddenChiTietID = e.Item.FindControl("hiddenChiTietID") as HiddenField;
                if (hiddenChiTietID != null && tanSuat.IDChiTiet.HasValue && tanSuat.IDChiTiet.Value > 0)
                {
                    chkItem.Checked = true;
                    hiddenChiTietID.Value = tanSuat.IDChiTiet.ToString();
                }
            }
        }

        protected void grvQuyChuan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null && e.Row.DataItem is TblCatQuyChuan)
            {
                TblCatQuyChuan qc = e.Row.DataItem as TblCatQuyChuan;

                HiddenField hiddenQuyChuanID = e.Row.FindControl("hiddenQuyChuanID") as HiddenField;
                if (hiddenQuyChuanID != null) hiddenQuyChuanID.Value = qc.Id.ToString();

                Repeater repDataChungChi = e.Row.FindControl("repDataChungChi") as Repeater;
                if (repDataChungChi != null)
                {
                    repDataChungChi.DataSource = GetChungChi(qc.Id);
                    repDataChungChi.DataBind();
                }

                Repeater repDataTanSuat = e.Row.FindControl("repDataTanSuat") as Repeater;
                if (repDataTanSuat != null)
                {
                    repDataTanSuat.DataSource = GetTanSuat(qc.Id);
                    repDataTanSuat.DataBind();
                }
            }
        }

        private void SaveChungChiQuyChuan()
        {
            foreach (GridViewRow row in grvQuyChuan.Rows)
            {
                HiddenField hiddenQuyChuanID = row.FindControl("hiddenQuyChuanID") as HiddenField;
                Repeater repDataChungChi = row.FindControl("repDataChungChi") as Repeater;
                if (repDataChungChi != null)
                {
                    foreach (RepeaterItem item in repDataChungChi.Items)
                    {
                        HiddenField hiddenChungChiEditID = item.FindControl("hiddenChungChiEditID") as HiddenField;
                        CheckBox chkItem = item.FindControl("chkItem") as CheckBox;
                        HiddenField hiddenChiTietID = item.FindControl("hiddenChiTietID") as HiddenField;

                        if (!chkItem.Checked && hiddenChiTietID != null && !string.IsNullOrEmpty(hiddenChiTietID.Value))
                            XetNghiemManager.DeleteChiTieuQuyChuanChungChi(hiddenChiTietID.Value);
                        else if (chkItem.Checked && string.IsNullOrEmpty(hiddenChiTietID.Value))
                        {
                            TblChiTieuQuyChuanChungChi qccc = new TblChiTieuQuyChuanChungChi();
                            qccc.IDChiTieuXetNghiem = CurrentID.Value;
                            qccc.IDQuyChuan = int.Parse(hiddenQuyChuanID.Value);
                            qccc.IDChungChi = int.Parse(hiddenChungChiEditID.Value);

                            qccc = XetNghiemManager.InsertChiTieuQuyChuanChungChi(qccc);
                        }
                    }
                }

                Repeater repDataTanSuat = row.FindControl("repDataTanSuat") as Repeater;
                if (repDataTanSuat != null)
                {
                    foreach (RepeaterItem item in repDataTanSuat.Items)
                    {
                        HiddenField hiddenTanSuatEditID = item.FindControl("hiddenTanSuatEditID") as HiddenField;
                        CheckBox chkItem = item.FindControl("chkItem") as CheckBox;
                        HiddenField hiddenChiTietID = item.FindControl("hiddenChiTietID") as HiddenField;

                        if (!chkItem.Checked && hiddenChiTietID != null && !string.IsNullOrEmpty(hiddenChiTietID.Value))
                            XetNghiemManager.DeleteChiTieuQuyChuanChungChi(hiddenChiTietID.Value);
                        else if (chkItem.Checked && string.IsNullOrEmpty(hiddenChiTietID.Value))
                        {
                            TblChiTieuQuyChuanTanSuat qccc = new TblChiTieuQuyChuanTanSuat();
                            qccc.IDChiTieuXetNghiem = CurrentID.Value;
                            qccc.IDQuyChuan = int.Parse(hiddenQuyChuanID.Value);
                            qccc.TanSuat = hiddenTanSuatEditID.Value;

                            qccc = XetNghiemManager.InsertChiTieuQuyChuanTanSuat(qccc);
                        }
                    }
                }
            }
            LoadCQuyChuanChungChi();
        }

        protected void btnAddBienBanFake_Click(object sender, EventArgs e)
        {
            grvBienBanGhiChep.InsertItem();
            LoadBienBan(CurrentID.Value);
        }

        protected void grvBienBanGhiChep_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = grvBienBanGhiChep.Rows[e.RowIndex];
            if (row != null)
            {
                HiddenField hdfEditID = row.FindControl("hdfEditID") as HiddenField;
                ExtraComboBox cbEditLoai = row.FindControl("cbEditLoai") as ExtraComboBox;
                CheckBox chkEditStatus = row.FindControl("chkEditStatus") as CheckBox;
                ExtraTextBox txtEditTen = row.FindControl("txtEditTen") as ExtraTextBox;

                if (txtEditTen != null)
                {
                    if (string.IsNullOrEmpty(txtEditTen.Text))
                    {
                        AddValidPromt(txtEditTen.ClientID, "Nhập nội dung");
                        txtEditTen.Focus();
                    }
                }

                e.Cancel = !PageIsValid;
                if (PageIsValid)
                {
                    if (hdfEditID != null)
                    {
                        if (!grvBienBanGhiChep.IsInsert)
                        {
                            TblChiTieuBienBanGhiChep ghiChep = ChiTieuManager.GetGhiChepByID(hdfEditID.Value);
                            if (ghiChep != null)
                            {
                                #region Log

                                #endregion

                                ghiChep.Ten = txtEditTen.Text;
                                ghiChep.Loai = cbEditLoai.SelectedValue;
                                ghiChep.TrangThaiSuDung = chkEditStatus.Checked;

                                ghiChep = ChiTieuManager.UpdateChiTieuBienBanGhiChep(ghiChep);

                                ShowNotify("Thông báo", "Cập nhật thông tin biên bản ghi chép thành công", NotifyType.Success);
                            }
                            else
                                ShowNotify("Thông báo", "Không tìm thấy thông tin cập nhật", NotifyType.Error);
                        }
                        else
                        {
                            TblChiTieuBienBanGhiChep ghiChep = new TblChiTieuBienBanGhiChep();
                            ghiChep.IDChiTieu = CurrentID.Value;
                            ghiChep.Ten = txtEditTen.Text;
                            ghiChep.Loai = cbEditLoai.SelectedValue;
                            ghiChep.TrangThaiSuDung = chkEditStatus.Checked;
                            ghiChep = ChiTieuManager.InsertChiTieuBienBanGhiChep(ghiChep);


                            ShowNotify("Thông báo", "Thêm mới biên bản ghi chép thành công", NotifyType.Success);

                        }
                    }
                }
            }
            if (PageIsValid)
            {
                grvBienBanGhiChep.CancelAllCommand();
                LoadBienBan(CurrentID.Value);
            }
            else ShowValidPromt();
        }

        protected void grvBienBanGhiChep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.DataItem != null && e.Row.DataItem is TblChiTieuPhuongPhap)
            {
                TblChiTieuBienBanGhiChep phuongPhap = e.Row.DataItem as TblChiTieuBienBanGhiChep;
                HiddenField hdfEditID = e.Row.FindControl("hdfEditID") as HiddenField;
                ExtraComboBox cbEditLoai = e.Row.FindControl("cbEditLoai") as ExtraComboBox;
                CheckBox chkEditStatus = e.Row.FindControl("chkEditStatus") as CheckBox;
                ExtraTextBox txtEditTen = e.Row.FindControl("txtEditTen") as ExtraTextBox;

                if (hdfEditID != null)
                    hdfEditID.Value = phuongPhap.Id.ToString();

                if (cbEditLoai != null)
                {
                    ListItem item = cbEditLoai.Items.FindByValue(phuongPhap.Loai);
                    if (item != null) cbEditLoai.SelectedValue = phuongPhap.Loai;
                }
                if (chkEditStatus != null) chkEditStatus.Checked = phuongPhap.TrangThaiSuDung.HasValue && phuongPhap.TrangThaiSuDung.Value;
                if (txtEditTen != null) txtEditTen.Text = phuongPhap.Ten;
            }
        }

        protected void grvBienBanGhiChep_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            if (btnEdit != null)
                btnEdit.Visible = GetButtonRole("Setting") && string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value);
            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null)
                btnDelete.Visible = GetButtonRole("Setting") && string.IsNullOrEmpty(hiddenChiTieuQuyChuanID.Value);
        }

        protected void btnAddNewBienBan_Click(object sender, EventArgs e)
        {
            grvBienBanGhiChep.InsertItem();
            LoadBienBan(CurrentID.Value);
        }

        protected void btnCloseBienBan_Click(object sender, EventArgs e)
        {
            grvBienBanGhiChep.CancelAllCommand();
            LoadBienBan(CurrentID.Value);
        }

        protected void grvBienBanGhiChep_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grvBienBanGhiChep.CancelAllCommand();
            LoadBienBan(CurrentID.Value);
        }
    }
}