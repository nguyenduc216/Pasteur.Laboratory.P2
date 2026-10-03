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
using BaseUserControl = Pasteur.Laboratory.P2.Common.BaseUserControl;

namespace Pasteur.Laboratory.P2.Controls
{
    public partial class ThanhLy : BaseUserControl
    {
        public event EventHandler mUpdateE;
        protected void Page_Load(object sender, EventArgs e)
        {
            txtSearchMaMau.EnterSubmitClientID = btnThem.ClientID;
        }


        public void EditThanhLy(string pageType, object id)
        {
            RefreshDetail(pageType);
            if (id != null)
            {
                TblThanhLy thanhLy = CategoryManager.GetThanhLyByID(id.ToString());
                if (thanhLy != null)
                {
                    hiddenEditID.Value = thanhLy.Id.ToString();
                    hiddenPageType.Value = pageType;
                    if (thanhLy.NgayThucHien.HasValue && thanhLy.NgayThucHien.Value != DateTime.MinValue)
                        dateNgayThucHien.DateValue = thanhLy.NgayThucHien.Value;
                    if (thanhLy.ThoiGianKetThuc.HasValue && thanhLy.ThoiGianKetThuc.Value != DateTime.MinValue)
                        dateNgayKetThuc.DateValue = thanhLy.ThoiGianKetThuc.Value;
                    txtChungToiGom.Text = thanhLy.ChungToiGom;
                    LoadChiTiet(thanhLy);
                }
            }
          
        }
        public void RefreshDetail(string pageType)
        {
            hiddenPageType.Value = pageType;
            hiddenEditID.Value = hiddenMauID.Value = "";
            dateNgayThucHien.DateValue = DateTime.Now;
            txtChungToiGom.Text ="";
            dateNgayKetThuc.DateValue = DateTime.MinValue;
            txtSearchMaMau.Text = "";
            //repData.DataSource = new List<TblThanhLyChiTiet>();
            //repData.DataBind();
            cbTinhTrang.ClearSelection();
        }
        protected void btnThem_Click(object sender, EventArgs e)
        {
            LinkButton lnk = sender as LinkButton;
            if (lnk != null)
            {
                bool isUpdate = lnk.CommandArgument == "update";
                string msg = "";
                TblMauXetNghiem mau = null;

                if (!isUpdate && string.IsNullOrEmpty(hiddenMauID.Value))
                {
                    AddValidPromt(txtSearchMaMau.ClientID, "Nhập mã mẫu cần thanh lý");
                    txtSearchMaMau.Focus();
                }
                else if (!isUpdate)
                {
                    mau = XetNghiemManager.GetMauXetNghiemByID(hiddenMauID.Value);
                    if (mau == null)
                    {
                        AddValidPromt(txtSearchMaMau.ClientID, "Mẫu không tồn tại");
                        txtSearchMaMau.Focus();
                    }
                }

                //if (!isUpdate && string.IsNullOrEmpty(cbTinhTrang.SelectedValue))
                //{
                //    AddValidPromt(cbTinhTrang.ClientID, "Chọn tình trạng mẫu");
                //    cbTinhTrang.Focus();
                //}

                if (dateNgayThucHien.DateValue == DateTime.MinValue)
                {
                    AddValidPromt(dateNgayThucHien.ClientID, "Chọn ngày thực hiện");
                    dateNgayThucHien.Focus();
                }
                else if (dateNgayThucHien.DateValue > DateTime.Now)
                {
                    AddValidPromt(dateNgayThucHien.ClientID, "Ngày thực hiện không lớn hơn ngày hiện tại");
                    dateNgayThucHien.Focus();
                }
                else if (mau != null && mau.NgayThanhLyDuKien.HasValue && mau.NgayThanhLyDuKien.Value != DateTime.MinValue &&
                    DateTimeHelper.AddFirstTimeOfDay(dateNgayThucHien.DateValue) < mau.NgayThanhLyDuKien.Value)
                {
                    AddValidPromt(dateNgayThucHien.ClientID, string.Format("Ngày thực hiện nhỏ hơn ngày thanh lý dự kiến {0}", mau.NgayThanhLyDuKien.Value.ToString("dd/MM/yyyy")));
                    dateNgayThucHien.Focus();
                }
                //if (isUpdate && dateNgayKetThuc.DateValue == DateTime.MinValue)
                //{
                //    AddValidPromt(dateNgayKetThuc.ClientID, "Chọn ngày kết thúc thanh lý");
                //    dateNgayKetThuc.Focus();
                //}
                //else if (dateNgayKetThuc.DateValue < dateNgayThucHien.DateValue)
                //{
                //    AddValidPromt(dateNgayKetThuc.ClientID, "Ngày kết thúc không nhỏ hơn ngày thực hiện");
                //    dateNgayKetThuc.Focus();
                //}

                if (ControlIsValid)
                {
                    TblThanhLy thanhLy = null;
                    if (string.IsNullOrEmpty(hiddenEditID.Value))
                    {
                        thanhLy = new TblThanhLy();
                        thanhLy.NgayThucHien = dateNgayThucHien.DateValue;
                        thanhLy.NguoiThucHien = thanhLy.NguoiCapNhat = ApplicationContext.Current.CommonUser.HoVaTen;
                        thanhLy.NgayCapNhat = DateTime.Now;
                        thanhLy.LoaiThanhLy = hiddenPageType.Value;
                       
                        thanhLy = CategoryManager.InsertThanhLy(thanhLy);
                    }
                    else thanhLy = CategoryManager.GetThanhLyByID(hiddenEditID.Value);

                    if (dateNgayKetThuc.DateValue != DateTime.MinValue)
                        thanhLy.ThoiGianKetThuc = dateNgayKetThuc.DateValue;
                    thanhLy.ChungToiGom = txtChungToiGom.Text;
                    thanhLy = CategoryManager.UpdateThanhLy(thanhLy);
                    msg = "Cập nhật ngày kết thúc thanh lý thành công";
                    if (!isUpdate)
                    {
                        TblThanhLyChiTiet chiTiet = new TblThanhLyChiTiet();
                        chiTiet.IDMauXN = int.Parse(hiddenMauID.Value);
                        chiTiet.IDThanhLy = thanhLy.Id;
                        chiTiet.TinhTrangMauTruocHuy = cbTinhTrang.SelectedValue;
                        chiTiet = CategoryManager.InsertThanhLyChiTiet(chiTiet);
                      
                        thanhLy.ChungToiGom = txtChungToiGom.Text;
                        #region Cập nhật mẫu xét nghiệm
                        if (hiddenPageType.Value == "LaboHoaLy")
                        {
                            mau.NgayThanhLyThucTeHoa = dateNgayThucHien.DateValue;
                            mau.IDThanhLyChiTietHoa = chiTiet.Id;
                            mau = XetNghiemManager.UpdateMauXetNghiem(mau);
                        }
                        else
                        {
                            mau.NgayThanhLyThucTeVS = dateNgayThucHien.DateValue;
                            mau.IDThanhLyChiTietVS = chiTiet.Id;
                            mau = XetNghiemManager.UpdateMauXetNghiem(mau);
                        }
                        #endregion

                        hiddenMauID.Value = txtSearchMaMau.Text = "";

                        hiddenEditID.Value = thanhLy.Id.ToString();
                        LoadChiTiet(thanhLy);
                        msg = "Thêm mẫu vào danh sách thanh lý thành công";
                    }

                    ShowNotify("Thông báo", msg, SweetSoft.ExtraControls.NotifyType.Success);

                    if (mUpdateE != null) mUpdateE(this, e);
                    txtSearchMaMau.Focus();
                }
                else ShowValidPromt();
            }
        }

        private void LoadChiTiet(TblThanhLy thanhLy)
        {
            List<TblThanhLyChiTiet> chiTietcol = new List<TblThanhLyChiTiet>();
            if (!string.IsNullOrEmpty(hiddenEditID.Value))
                chiTietcol = CategoryManager.GetThanhLyChiTietByThanhLyID(hiddenEditID.Value);
            repData.DataSource = chiTietcol;
            repData.DataBind();

            #region Cập nhật danh sách thanh lý
            string lst = "";
            foreach (TblThanhLyChiTiet tl in chiTietcol) lst += tl.MaMau + ", ";
            lst = lst.Trim();
            if (lst.EndsWith(",")) lst = lst.Substring(0, lst.Length - 1);
            thanhLy.MaMauList = lst;
            thanhLy = CategoryManager.UpdateThanhLy(thanhLy);
            #endregion
            
        }

        public void Delete()
        {
            if (CurrentMessageBoxResult.Value != null)
            {
                TblThanhLyChiTiet chiTiet = CurrentMessageBoxResult.Value as TblThanhLyChiTiet;
                if (chiTiet != null)
                {
                    bool b = CategoryManager.DeleteThanhLyChiTiet(chiTiet.Id);
                    if (b) ShowNotify("Thông báo", string.Format("Gỡ thanh lý mẫu <b>{0}</b> thành công", chiTiet.MaMau), NotifyType.Success);
                    TblThanhLy thanhLy = CategoryManager.GetThanhLyByID(chiTiet.IDThanhLy);
                    LoadChiTiet(thanhLy);
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(chiTiet.IDMauXN);
                    if (mauXN != null)
                        XetNghiemManager.UpdateThongTinThanhLy(chiTiet.IDMauXN, thanhLy.LoaiThanhLy);
                    if (mUpdateE != null) mUpdateE(this, null);
                    CloseMessageBox();
                }
                else
                    ShowNotify("Thông báo", "Không tìm thấy thông tin chi tiết thanh lý", NotifyType.Error);

            }

        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            bool resultDelete = false;
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmdA = btn.CommandArgument;

                TblThanhLyChiTiet TLCT = CategoryManager.GetThanhLyChiTietByID(btn.CommandArgument);
                if (TLCT != null)
                {
                    MessageBoxResult result = new MessageBoxResult();
                    result.Value = TLCT;
                    result.Submit = true;
                    result.CommandName = "delete_TLCT";
                    CurrentMessageBoxResult = result;
                    OpenMessageBox(string.Format("Xóa chi tiết thanh lý"),
                                   string.Format("Bạn thật sự muốn xóa mẫu {0} ra khỏi danh sách thanh lý ?",
                                                 TLCT.MaMau));
                }
            }
        }

        protected void repData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.AlternatingItem ||
            //    e.Item.ItemType == ListItemType.Item)
            //{
            //    TblThanhLyChiTiet ct = e.Item.DataItem as TblThanhLyChiTiet;
            //    if (ct != null)
            //    {
            //        Label lbTitle = e.Item.FindControl("lbTitle") as Label;
            //        if (lbTitle != null) lbTitle.Text = ct.MaMau;
            //    }
            //}
        }
    }
}