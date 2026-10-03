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
    public partial class MauXetNghiem : BaseGridPage
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
                if (CurrentPageType == "TLMXN")
                    return FunctionSystem.Code.TLMXN;
                return FunctionSystem.Code.Phieu_Mau;
            }
        }
        protected string CurrentPageType
        {
            get { return CommonHelper.QueryString("PageType"); }
        }

        private string CurrentCode
        {
            get
            {
                return CommonHelper.QueryString("Code");
            }
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
                if (CurrentPageType == "TLMXN")
                    return FunctionSystem.Name.TLMXN;
                return FunctionSystem.Name.Phieu_Mau;
            }
        }

        protected void lnkPrint_Onclick(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                string cmdN = btn.CommandArgument;
                string cmdA = btn.CommandName;
                if (cmdA == "1")
                {

                    string url = string.Format("/phieu-tiep-nhan-yeu-cau-so-{0}", cmdN);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }

                if (cmdA == "2")
                {
                    string url = string.Format("/phieu-chuyen-mau-so-{0}", cmdN);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }
                if (cmdA == "3")
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(cmdN);
                    if (false && mauXN != null && (
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
                        else if (false)
                        {
                            mauXN.MaSoTraViSinh = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraViSinhIndex);
                            mauXN.MaSoTraViSinhIndex = codeIndex;
                        }
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }
                    string url = string.Format("/phieu-tra-ket-qua-noi-bo-{1}-so-{0}", cmdN, CurrentPageType == "LaboHoaLy" ? "hoa" : "vs");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }

                if (cmdA == "4")
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(cmdN);
                    if (mauXN != null && string.IsNullOrEmpty(mauXN.MaSoTraKQ))
                    {
                        string codeIndex = "";
                        mauXN.MaSoTraKQ = XetNghiemManager.GetMauPhieuIndex(ref codeIndex, mauXN.IDLoaiHinhYeuCauMau.Value, TblMauXetNghiem.Columns.MaSoTraKQIndex);
                        mauXN.MaSoTraKQIndex = codeIndex;
                        mauXN = XetNghiemManager.UpdateMauXetNghiem(mauXN);
                    }

                    string url = string.Format("/phieu-tra-ket-qua-so-{0}", cmdN);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }
                if (cmdA == "5")
                {
                    string url = string.Format("/bien-ban-ghi-chep-thu-nghiem-so-{0}", cmdN);
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
                }
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
                if (CurrentPageType == "TLMXN")
                    return "Thanh lý xét nghiệm";
                string rs = "phiếu mẫu xét nghiệm";

                return rs;
            }
        }

        protected bool GetButtonRole
            (string rightName)
        {
            bool rs = false;
            if (CurrentPageType == "LaboHoaLy")
            {
                if (rightName == "Update") rs = UserHasRight(RightPageName.Hoa_TNMSH_Update);
                else if (rightName == "Approve") rs = UserHasRight(RightPageName.Hoa_TNMSH_Chot);
            }
            else if (CurrentPageType == "LaboViSinh")
            {
                if (rightName == "Update") rs = UserHasRight(RightPageName.VS_TNMSH_Update);
                else if (rightName == "Approve") rs = UserHasRight(RightPageName.VS_TNMSH_Chot);
            }
            else if (CurrentPageType == "KeToanTiepNhan")
            {
                if (rightName == "Print") rs = UserHasRight(RightPageName.KeToa_TNMKT_Print);
            }
            else if (CurrentPageType == "TraKQXN")
            {
                if (rightName == "TraKetQua") rs = UserHasRight(RightPageName.PMXN_TraKetQua);
                else if (rightName == "CapNhatThongTin") rs = UserHasRight(RightPageName.PMXN_CapNhatThongTin);
                //else if (rightName == "Delete") rs = UserHasRight(RightPageName.PMXN_Delete);
            }
            else if (CurrentPageType == "TLMXN")
            {
                if (rightName == "TLMXN") rs = UserHasRight(RightPageName.PMXN_TLMXN);
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
        #endregion

        protected string GetNgayThanhLy(object data)
        {
            string rs = "";
            if (data is TblMauXetNghiem)
            {
                TblMauXetNghiem mauXN = data as TblMauXetNghiem;
                DateTime? vs = mauXN.NgayThanhLyThucTeVS;
                DateTime? hoa = mauXN.NgayThanhLyThucTeHoa;
                DateTime? duKien = mauXN.NgayThanhLyDuKien;
                if (CurrentPageType == "LaboViSinh" && vs.HasValue && vs.Value != DateTime.MinValue)
                    rs = vs.Value.ToString("dd/MM/yyyyy HH:mm");
                if (CurrentPageType == "LaboHoaLy" && hoa.HasValue && hoa.Value != DateTime.MinValue)
                    rs = hoa.Value.ToString("dd/MM/yyyyy HH:mm");
                if (string.IsNullOrEmpty(rs) && duKien.HasValue && duKien.Value != DateTime.MinValue)
                    rs = string.Format("Dự kiến: {0}", duKien.Value.ToString("dd/MM/yyyy"));
            }
            return rs;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnShowAll.Visible = CurrentPageType.StartsWith("Labo") || CurrentPageType == "TraKQXN";
            if (hiddenShowAll.Value == "0") btnShowAll.Text = "Xem toàn bộ danh sách";
            else btnShowAll.Text = "Xem công việc cần làm";

            ltrDialogSearchTitle.Text = string.Format("Tìm kiếm {0}", CurrentName);

            grvData.ColumnVisibleDefault = "0,1,2,8,9,12,13,14,16,17,18,19,20,23";
            if (CurrentPageType == "LaboHoaLy") grvData.ColumnVisibleDefault = "0,1,2,8,9,12,13,14,16,17,18,20,23";
            if (CurrentPageType == "LaboViSinh") grvData.ColumnVisibleDefault = "0,1,2,8,9,12,13,14,16,17,19,20,23";
            //btnAddNew.Visible = GetButtonRole("Insert");
            if (!IsPostBack)
            {
                hiddenShowAll.Value = "1";
                if (CurrentPageType.StartsWith("Labo") || CurrentPageType == "TraKQXN") hiddenShowAll.Value = "0";
                cbSearchTrangThaiMau.Enabled = false;
                if (CurrentPageType == "PhieuNhanMau")
                {

                    cbSearchTrangThaiMau.Enabled = true;
                    btnAddNew.Visible = GetButtonRole("Insert");
                }
                else if (CurrentPageType == "KeToanTiepNhan")
                {
                    cbSearchTrangThaiMau.Enabled = true;
                    //cbSearchTrangThaiMau.SelectedValue = "Labo";
                }

                else
                    cbSearchTrangThaiMau.SelectedValue = "Labo";

                if (CurrentPageType.StartsWith("Labo"))
                {
                    ltrTimKiemMaMau.Text = "Mã nội bộ";
                }


                if (CurrentPageType == "TraKQXN")
                {
                    ltrTimKiemMaMau.Text = "Mã trả kết quả";
                    cbSearchTrangThaiMau.SelectedValue = "ChoTraKetQua";
                }

                LoadControl();
                grvData.CurrentSortExpression = TblMauXetNghiem.Columns.MaMauIndex;
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
        protected string GetTongSo(object data)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiem)
            {
                TblMauXetNghiem mau = data as TblMauXetNghiem;
                if (mau.LuongMau.HasValue && mau.LuongMauHoa.HasValue)
                    rs = string.Format("<b>{0}</b> / {1} / {2}",
                        mau.LuongMau.Value.ToStringWithLanguage(),
                        mau.LuongMauHoa.Value.ToStringWithLanguage(),
                        (mau.LuongMau.Value - mau.LuongMauHoa.Value).ToStringWithLanguage());
            }
            return rs;
        }
        protected string GetTrangThai(object data)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiem)
            {
                TblMauXetNghiem mau = data as TblMauXetNghiem;
                if (mau.TrangThaiMau == "New") rs = "Tạo mới";
                if (mau.TrangThaiMau == "Labo") rs = "Đã chuyển Labo";
                if (mau.TrangThaiMau == "DangThucHien") rs = "Đang thực hiện Labo";
                if (mau.TrangThaiMau == "ChoTraKetQua") rs = "Chờ trả kết quả";

                if (mau.KetQuaDanhGiaMau == "0") rs = "<span style='color:white;background-color:red;'>Mẫu kiểm tra lại</span>";
                if (mau.KetQuaDanhGiaMau == "1") rs = "Mẫu chấp nhật chất lượng";
                if (mau.TrangThaiMau == "DaTraKetQua") rs = "Đã trả kết quả";
                if (mau.TrangThaiMau == "Huy") rs = "Hủy";

                if (mau.TrangThaiMau == "ThanhLy") rs = "Mẫu đã thanh lý";
                if (CurrentPageType == "LaboHoaLy" && mau.IDThanhLyChiTietHoa.HasValue && mau.IDThanhLyChiTietHoa.Value > 0)
                    rs = "Mẫu đã thanh lý";
                if (CurrentPageType == "LaboViSinh" && mau.IDThanhLyChiTietVS.HasValue && mau.IDThanhLyChiTietVS.Value > 0)
                    rs = "Mẫu đã thanh lý";
            }
            return rs;
        }
        protected void grvData_RowCreated(object sender, GridViewRowEventArgs e)
        {
            ExtraButton btnEdit = e.Row.FindControl("btnEdit") as ExtraButton;
            if (btnEdit != null)
            {
                ExtraButton lnkEdit = e.Row.FindControl("lnkEdit") as ExtraButton;
                LinkButton lnkInPhieuTiepNhanYC = e.Row.FindControl("lnkInPhieuTiepNhanYC") as LinkButton;
                LinkButton lnkPhieuDeNghiThuTien = e.Row.FindControl("lnkPhieuDeNghiThuTien") as LinkButton;
                LinkButton lnkInPhieuChuyenMau = e.Row.FindControl("lnkInPhieuChuyenMau") as LinkButton;
                LinkButton lnkPhieuTraKetQua = e.Row.FindControl("lnkPhieuTraKetQua") as LinkButton;
                LinkButton lnkInPhieuKetQuaNoiNo = e.Row.FindControl("lnkInPhieuKetQuaNoiNo") as LinkButton;
                LinkButton ltrInBienBanGhiChepThuNghiem = e.Row.FindControl("ltrInBienBanGhiChepThuNghiem") as LinkButton;


                Label lbName = e.Row.FindControl("lbName") as Label;
                //btnEdit.Visible = false;
                if (e.Row.DataItem != null)
                {
                    TblMauXetNghiem mauXN = e.Row.DataItem as TblMauXetNghiem;
                    lnkEdit.Text = lbName.Text = mauXN.MaMau;
                    if (CurrentPageType == "PhieuNhanMau")
                    {
                        lnkEdit.Visible =
                        btnEdit.Visible = GetButtonRole("Update");
                        lnkEdit.NavigateUrl =
                        btnEdit.NavigateUrl = string.Format("mau-xet-nghiem/{0}", mauXN.Id);

                        //btnEdit.NavigateUrl = string.Format("chi-tieu-xet-nghiem/{0}", chiTieu.Id);
                        btnEdit.OnClientClick = "return false;";
                        lnkEdit.Target = btnEdit.Target = "_blank";

                        lnkPhieuDeNghiThuTien.Visible = true;
                        lnkInPhieuTiepNhanYC.Visible = true;
                        lnkInPhieuChuyenMau.Visible = mauXN.NgayChuyenLabo.HasValue && mauXN.NgayChuyenLabo.Value != DateTime.MinValue;
                    }
                    Literal ltrMaNoiBo = e.Row.FindControl("ltrMaNoiBo") as Literal;
                    if (CurrentPageType == "LaboHoaLy")
                    {
                        ltrMaNoiBo.Text = string.Format("<span style='font-weight:bold;display:block;margin:2px 0px;text-align:center;color:#6b3588;'>Mã nội bộ<br/>{0}</span> ", mauXN.MaSoTraHoaLy);
                        lnkEdit.NavigateUrl =
                        btnEdit.NavigateUrl = string.Format("labo-hoa-tiep-nhan/{0}", mauXN.Id);
                        lnkEdit.Target = btnEdit.Target = "_blank";
                        lnkEdit.Visible = btnEdit.Visible = GetButtonRole("Update") || GetButtonRole("Approve");
                    }

                    if (CurrentPageType == "LaboViSinh")
                    {
                        ltrMaNoiBo.Text = string.Format("<span style='font-weight:bold;display:block;margin:2px 0px;text-align:center;color:#6b3588;'>Mã nội bộ<br/>{0}</span> ", mauXN.MaSoTraViSinh);
                        lnkEdit.NavigateUrl =
                        btnEdit.NavigateUrl = string.Format("labo-vs-tiep-nhan/{0}", mauXN.Id);
                        lnkEdit.Target = btnEdit.Target = "_blank";
                        lnkEdit.Visible = btnEdit.Visible = GetButtonRole("Update") || GetButtonRole("Approve");
                    }

                    Literal ltrMaTraKetQua = e.Row.FindControl("ltrMaTraKetQua") as Literal;
                    if (CurrentPageType == "TraKQXN")
                    {
                        lnkPhieuTraKetQua.Visible = true;
                        ltrMaTraKetQua.Text = string.Format("<span style='font-weight:bold;display:block;margin:2px 0px;text-align:center;color:#6b3588;'>Mã trả KQ<br/>{0}</span> ", mauXN.MaSoTraKQ);
                        lnkEdit.NavigateUrl =
                        btnEdit.NavigateUrl = string.Format("tra-ket-qua-xet-nghiem/{0}", mauXN.Id);
                        lnkEdit.Target = btnEdit.Target = "_blank";
                        lnkEdit.Visible = btnEdit.Visible = GetButtonRole("TraKetQua") || GetButtonRole("CapNhatThongTin");
                    }
                    if (CurrentPageType == "TLMXN")
                    {
                        lnkPhieuTraKetQua.Visible = true;
                    }
                    if (CurrentPageType == "KeToanTiepNhan")
                    {

                        btnEdit.Visible = false;
                        lnkPhieuDeNghiThuTien.Visible = true;
                    }

                    if (CurrentPageType.StartsWith("Labo"))
                    {
                        lnkInPhieuKetQuaNoiNo.Visible = ((CurrentPageType == "LaboHoaLy" && mauXN.NgayHoaLy.HasValue && mauXN.NgayHoaLy.Value != DateTime.MinValue) ||
                        (CurrentPageType == "LaboViSinh" && mauXN.NgayViSinh.HasValue && mauXN.NgayViSinh.Value != DateTime.MinValue));
                        ltrInBienBanGhiChepThuNghiem.Visible = CurrentPageType == "LaboViSinh";
                    }


                    btnEdit.OnClientClick = "return false;";
                    btnEdit.Target = "_blank";
                    lbName.Visible = !lnkEdit.Visible;

                    ExtraButton btnThanhLy = e.Row.FindControl("btnThanhLy") as ExtraButton;
                    if (btnThanhLy != null)
                    {
                        if (CurrentPageType.StartsWith("Labo"))
                        {
                            string cmdN = btnThanhLy.CommandName;
                            bool b = false;
                            if (CurrentPageType.EndsWith("HoaLy"))
                            {
                                b = mauXN.LuongMauHoa.HasValue && mauXN.LuongMauHoa.Value > 0 &&
                                    mauXN.NgayThanhLyDuKien.HasValue && mauXN.NgayThanhLyDuKien.Value != DateTime.MinValue &&
                                    (!mauXN.NgayThanhLyThucTeHoa.HasValue || (mauXN.NgayThanhLyThucTeHoa.HasValue && mauXN.NgayThanhLyThucTeHoa.Value == DateTime.MinValue));
                            }
                            else
                            {
                                b = mauXN.LuongMauViSinh > 0 &&
                                        mauXN.NgayThanhLyDuKien.HasValue && mauXN.NgayThanhLyDuKien.Value != DateTime.MinValue &&
                                        (!mauXN.NgayThanhLyThucTeVS.HasValue || (mauXN.NgayThanhLyThucTeVS.HasValue && mauXN.NgayThanhLyThucTeVS.Value == DateTime.MinValue));
                            }
                            btnThanhLy.Visible = b;
                        }
                    }
                }

                if (lnkPhieuTraKetQua != null)
                {

                }
            }

            ExtraButton btnDelete = e.Row.FindControl("btnDelete") as ExtraButton;
            if (btnDelete != null)
            {
                if (CurrentPageType == "PhieuNhanMau")
                    btnDelete.Visible = GetButtonRole("Delete");
            }


        }

        protected string GetMaNoiBo(object data)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiem)
            {
                TblMauXetNghiem mxn = data as TblMauXetNghiem;
                if (CurrentPageType == "LaboHoaLy") rs = mxn.MaSoTraHoaLy;
                else rs = mxn.MaSoTraViSinh;
            }
            return rs;
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
                if (!string.IsNullOrEmpty(txtSearchTenKhachHang.Text.Trim()))
                    filterClause += string.Format(" CONTAINS( mauXN.{0},N'\"{1}\"') AND ",
                                                  TblMauXetNghiem.Columns.TenKhachHang,
                                                  txtSearchTenKhachHang.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                if (!string.IsNullOrEmpty(txtSearchMaMau.Text.Trim()))
                    filterClause += string.Format(" (mauXN.{0} like N'%{1}%') AND ",
                                                 TblMauXetNghiem.Columns.MaMau,
                                                 txtSearchMaMau.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"),
                                                 TblMauXetNghiem.Columns.MaSoTraHoaLy);
                if (!string.IsNullOrEmpty(txtSearchMaNoiBo.Text.Trim()))
                {
                    if (CurrentPageType == "LaboHoaLy")
                        filterClause += string.Format(" (mauXN.{2} like N'%{1}%' ) AND ",
                                                  TblMauXetNghiem.Columns.MaMau,
                                                  txtSearchMaNoiBo.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"),
                                                  TblMauXetNghiem.Columns.MaSoTraHoaLy);
                    else if (CurrentPageType == "LaboViSinh")
                        filterClause += string.Format(" (mauXN.{2} like N'%{1}%' ) AND ",
                                                  TblMauXetNghiem.Columns.MaMau,
                                                  txtSearchMaNoiBo.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"),
                                                  TblMauXetNghiem.Columns.MaSoTraViSinh);
                    else if (CurrentPageType == "TraKQXN")
                        filterClause += string.Format("  (mauXN.{2} like N'%{1}%' )  AND ",
                                                  TblMauXetNghiem.Columns.MaMau,
                                                  txtSearchMaNoiBo.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"),
                                                  TblMauXetNghiem.Columns.MaSoTraKQ);
                    //else
                    //    filterClause += string.Format(" mauXN.{0} like N'%{1}%' AND ",
                    //                          TblMauXetNghiem.Columns.MaMau,
                    //                          txtSearchMaNoiBo.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                }

                if (!string.IsNullOrEmpty(txtSearchDiaChiKhach.Text.Trim()))
                    filterClause += string.Format(" CONTAINS(mauXN.{0} , N'\"{1}\"') AND ",
                                                  TblMauXetNghiem.Columns.DiaChiKhachHang,
                                                  txtSearchDiaChiKhach.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchTenNguoiGuiMau.Text.Trim()))
                    filterClause += string.Format(" CONTAINS(mauXN.{0} , N'\"{1}\"') AND ",
                                                  TblMauXetNghiem.Columns.TenNguoiGuiMau,
                                                  txtSearchTenNguoiGuiMau.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchTenNguoiLayMau.Text.Trim()))
                    filterClause += string.Format(" CONTAINS(mauXN.{0} , N'\"{1}\"') AND ",
                                                  TblMauXetNghiem.Columns.TenNguoiLayMau,
                                                  txtSearchTenNguoiLayMau.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));
                if (!string.IsNullOrEmpty(txtSearchTenNguoiNhanMau.Text.Trim()))
                    filterClause += string.Format(" CONTAINS(mauXN.{0}, N'\"{1}\"') AND ",
                                                  TblMauXetNghiem.Columns.TenNguoiNhanMau,
                                                  txtSearchTenNguoiNhanMau.Text.Trim().Replace("\'", "\'\'").Replace("%", "[%]"));

                else if (hiddenShowAll.Value == "1" && !string.IsNullOrEmpty(cbSearchTrangThaiMau.SelectedValue))
                {
                    if (CurrentPageType == "PhieuNhanMau")
                    {

                        filterClause += string.Format(" mauXN.{0} = '{1}' AND ",
                                                     TblMauXetNghiem.Columns.TrangThaiMau,
                                                     cbSearchTrangThaiMau.SelectedValue);
                    }

                    else if (CurrentPageType == "TLMXN")
                    {

                    }
                    else
                        filterClause += string.Format(" mauXN.{0} != 'New' AND ",
                                                     TblMauXetNghiem.Columns.TrangThaiMau);
                }

                if (cbSearchLoaiHinh.HasValue)
                    filterClause += string.Format(" mauXN.{0} = {1} AND ",
                                                    TblMauXetNghiem.Columns.IDLoaiHinhYeuCauMau,
                                                    cbSearchLoaiHinh.SelectedValue);
                if (cbSearchLoaiMau.HasValue)
                    filterClause += string.Format(" mauXN.{0} = {1} AND ",
                                                    TblMauXetNghiem.Columns.IDLoaiMau,
                                                    cbSearchLoaiMau.SelectedValue);
                if (cbSearchQuyChuan.HasValue)
                    filterClause += string.Format(" mauXN.{0} = {1} AND ",
                                                    TblMauXetNghiem.Columns.IDQuyChuan,
                                                    cbSearchQuyChuan.SelectedValue);
                if (dtSearchNgayLayMauTu.DateValue != DateTime.MinValue)
                    filterClause += string.Format("mauXN.{0} >= '{1}' AND ",
                                                   TblMauXetNghiem.Columns.NgayLayMau,
                                                   dtSearchNgayLayMauTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dtSearchNgayLayMauToi.DateValue != DateTime.MinValue)
                    filterClause += string.Format("mauXN.{0} <= '{1}' AND ",
                                                   TblMauXetNghiem.Columns.NgayLayMau,
                                                   dtSearchNgayLayMauToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dtSearchNgayNhanMauTu.DateValue != DateTime.MinValue)
                    filterClause += string.Format("mauXN.{0} >= '{1}' AND ",
                                                   TblMauXetNghiem.Columns.NgayNhanMau,
                                                   dtSearchNgayNhanMauTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dtSearchNgayNhanMauTu.DateValue != DateTime.MinValue)
                    filterClause += string.Format("mauXN.{0} <= '{1}' AND ",
                                                   TblMauXetNghiem.Columns.NgayNhanMau,
                                                   dtSearchNgayNhanMauTu.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dtSearchNgayTraKQTu.DateValue != DateTime.MinValue)
                    filterClause += string.Format("mauXN.{0} >= '{1}' AND ",
                                                   TblMauXetNghiem.Columns.NgayTraKetQua,
                                                   dtSearchNgayTraKQTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dtSearchNgayTraKQToi.DateValue != DateTime.MinValue)
                    filterClause += string.Format("mauXN.{0} <= '{1}' AND ",
                                                   TblMauXetNghiem.Columns.NgayTraKetQua,
                                                   dtSearchNgayTraKQTu.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateSearchHoaLyFrom.DateValue != DateTime.MinValue)
                    filterClause += string.Format("(mauXN.{0} <= '{2}' AND mauXN.{0} >= '{1}') AND ",
                                                   TblMauXetNghiem.Columns.NgayHoaLy,
                                                   dateSearchHoaLyFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"),
                                                   dateSearchHoaLyFrom.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateSearchViSinhFrom.DateValue != DateTime.MinValue)
                    filterClause += string.Format("(mauXN.{0} <= '{2}' AND mauXN.{0} >= '{1}') AND ",
                                                     TblMauXetNghiem.Columns.NgayViSinh,
                                                   dateSearchViSinhFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"),
                                                   dateSearchViSinhFrom.DateValue.ToString("yyyy-MM-dd 23:59:59"));

                if (CurrentPageType == "LaboHoaLy")
                {
                    filterClause += string.Format("mauXN.{0} > 0 AND ",
                                                       TblMauXetNghiem.Columns.LuongMauHoa);

                    if (hiddenShowAll.Value == "0")
                    {
                        filterClause += string.Format(" (mauXN.{0} is null or mauXN.KetQuaDanhGiaMau = '0' ) AND ",
                                              TblMauXetNghiem.Columns.NgayHoaLy,
                                              "New");
                    }
                }

                if (CurrentPageType == "LaboViSinh")
                {
                    filterClause += string.Format("(mauXN.{0} - mauXN.{1})  > 0 AND ",
                                                       TblMauXetNghiem.Columns.LuongMau, TblMauXetNghiem.Columns.LuongMauHoa);
                    if (hiddenShowAll.Value == "0")
                    {
                        filterClause += string.Format(" (mauXN.{0} is null or mauXN.KetQuaDanhGiaMau = '0' ) AND ",
                                              TblMauXetNghiem.Columns.NgayViSinh,
                                              "New");
                    }
                }

                if (CurrentPageType == "PhieuNhanMau")
                {
                    if (CurrentCode == "New")
                    {
                        filterClause += string.Format(" mauXN.{0} = '{1}' AND ",
                                              TblMauXetNghiem.Columns.TrangThaiMau,
                                              "New");
                    }

                    if (CurrentCode == "ChoTraKQ")
                    {
                        //DateTime d = DateTimeHelper.AddLastTimeOfDay(DateTime.Now.AddDays(2));
                        filterClause += string.Format(" mauXN.{0} is null AND ",
                                              TblMauXetNghiem.Columns.NgayTraKetQuaThucTe,
                                              "New");
                        filterClause += string.Format(" mauXN.{0} <= '{1}' AND ",
                                              TblMauXetNghiem.Columns.NgayTraKetQua,
                                              DateTime.Now.AddDays(2).ToString("yyyy-MM-dd 23:59:59"));
                        filterClause += string.Format(" mauXN.{0} = 'ChoTraKetQua' AND ",
                                            TblMauXetNghiem.Columns.TrangThaiMau,
                                            DateTime.Now.AddDays(2).ToString("yyyy-MM-dd 23:59:59"));
                    }
                }
                if (CurrentPageType == "KeToanTiepNhan")
                {
                    if (CurrentCode == "ThuTien")
                        filterClause += string.Format(" mauXN.{0} is null AND ",
                                          TblMauXetNghiem.Columns.ThuTienNgayThu,
                                          "New");
                }

                if (CurrentPageType == "TraKQXN")
                {
                    if (hiddenShowAll.Value == "0")
                    //filterClause += string.Format(" (mauXN.{0} is null) AND ", TblMauXetNghiem.Columns.NgayInKetQua);
                    {
                        if (UserHasRight(RightPageName.PMXN_CapNhatIn))
                            filterClause += string.Format(" ((mauXN.{0} = 'ChoTraKetQua' and (mauXN.KetQuaDanhGiaMau is null or mauXN.KetQuaDanhGiaMau = '')) or mauXN.KetQuaDanhGiaMau = '0' or (mauXN.{0} = 'DaTraKetQua' AND  (mauXN.{1} is null) ))  AND ",
                                                           TblMauXetNghiem.Columns.TrangThaiMau, TblMauXetNghiem.Columns.NgayInKetQua);
                        else
                            filterClause += string.Format(" ((mauXN.{0} = 'ChoTraKetQua' and (mauXN.KetQuaDanhGiaMau is null or mauXN.KetQuaDanhGiaMau = '')) or mauXN.KetQuaDanhGiaMau = '0')  AND ",
                                                           TblMauXetNghiem.Columns.TrangThaiMau, TblMauXetNghiem.Columns.NgayInKetQua);
                    }

                    else
                        filterClause += string.Format(" (mauXN.{0} = 'ChoTraKetQua' or mauXN.{0} = 'DaTraKetQua' or mauXN.KetQuaDanhGiaMau = '0') AND ",
                                                           TblMauXetNghiem.Columns.TrangThaiMau);

                    if (CurrentCode == "ChuaThanhLy")
                    {
                        filterClause += string.Format(" (mauXN.{0} = 'ChoTraKetQua' or mauXN.{0} = 'DaTraKetQua') AND mauXN.{1} is null AND ",
                                                            TblMauXetNghiem.Columns.TrangThaiMau, TblMauXetNghiem.Columns.NgayThanhLyThucTe);
                        DateTime dateHoa = DateTime.Now.AddDays(-10);
                        DateTime dateSinh = DateTime.Now.AddDays(-5);
                        filterClause += string.Format(" ( (mauXN.{0} - mauXN.{1} > 0 AND mauXN.{2} <= '{3}' AND mauXN.{4} <= '{3}' ) OR (mauXN.{1} > 0 AND mauXN.{2} <= '{5}' AND mauXN.{4} <= '{5}') ) AND ",
                                                            TblMauXetNghiem.Columns.LuongMau, TblMauXetNghiem.Columns.LuongMauHoa,
                                                            TblMauXetNghiem.Columns.NgayTraKetQua, dateSinh.ToString("yyyy-MM-dd 23:59:59"),
                                                            TblMauXetNghiem.Columns.NgayTraKetQuaThucTe, dateHoa.ToString("yyyy-MM-dd 23:59:59"));
                    }
                }


                string orderClause = string.Format("{0} desc", TblMauXetNghiem.Columns.MaMauIndex);
                if (CurrentPageType == "LaboHoaLy" || CurrentPageType == "LaboViSinh") orderClause = string.Format(" KetQuaDanhGiaMau desc ");

                if (!string.IsNullOrEmpty(grv.CurrentSortExpression))
                    if (CurrentPageType == "LaboHoaLy" || CurrentPageType == "LaboViSinh")
                        orderClause = string.Format(" KetQuaDanhGiaMau desc,CAST(SUBSTRING(MaMau, 8, 4) AS INT) DESC,{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);
                    else
                        orderClause = string.Format(" CAST(SUBSTRING(MaMau, 8, 4) AS INT) DESC,{0} {1}",
                                                grv.CurrentSortExpression,
                                                grv.CurrentSortDerection);

                if (hiddenShowAll.Value == "0" && CurrentPageType == "TraKQXN")
                    orderClause = string.Format(" Isnull(mauXN.MaSoTraKQIndex,'') ASC, mauXN.NgayTraKetQua ASC ",
                                              grv.CurrentSortExpression,
                                              grv.CurrentSortDerection);
                if (hiddenShowAll.Value == "1" && CurrentPageType == "LaboHoaLy")
                    orderClause = string.Format(" Isnull(mauXN.MaSoTraHoaLyIndex,'') Desc, mauXN.NgayTraKetQua ASC ",
                                             grv.CurrentSortExpression,
                                             grv.CurrentSortDerection);
                string column = string.Format("mauXN.*,loaimau.Ten as TenLoaiMau,dvt.{0},quyChuan.{1} as TenQuyChuan,lhycm.{6} as TenLoaiHinhYeuCau",
                                                TblDonViTinh.Columns.TenDonVi,
                                                TblCatQuyChuan.Columns.Ten,
                                                TblKhachHang.Columns.TenKhachHang,
                                                TblKhachHang.Columns.DiaChi,
                                                TblKhachHang.Columns.DienThoai,
                                                TblKhachHang.Columns.Email,
                                                TblCatLoaiHinhYeuCau.Columns.Ten);



                grv.BindingWithFiltering = !string.IsNullOrEmpty(filterClause.Trim());
                filterClause = (filterClause1 + filterClause.Trim()).Trim();
                if (filterClause.EndsWith("AND"))
                    filterClause = filterClause.Substring(0, filterClause.Length - 3);

                string tableJoin = string.Format(" left join {0} as loaimau on loaimau.Id = mauXN.{1} ",
                                                  TblCatLoaiMau.Schema.TableName, TblMauXetNghiem.Columns.IDLoaiMau);
                tableJoin += string.Format(" left join {0} as quyChuan on quyChuan.Id = mauXN.{1} ",
                                                  TblCatQuyChuan.Schema.TableName, TblMauXetNghiem.Columns.IDQuyChuan);
                tableJoin += string.Format(" left join {0} as khachHang on khachHang.Id = mauXN.{1} ",
                                                  TblKhachHang.Schema.TableName, TblMauXetNghiem.Columns.IDKhachHang);
                tableJoin += string.Format(" left join {0} as dvt on dvt.Id = mauXN.{1} ",
                                                  TblDonViTinh.Schema.TableName, TblMauXetNghiem.Columns.IDDonViLuongMau);
                tableJoin += string.Format(" left join {0} as lhycm on lhycm.Id = mauXN.{1} ",
                                                  TblCatLoaiHinhYeuCau.Schema.TableName, TblMauXetNghiem.Columns.IDLoaiHinhYeuCauMau);
                int total = 0;
                List<TblMauXetNghiem> lst = XetNghiemManager.SearchMauXetNghiem(rowIndex, pageSize, tableJoin, column,
                                                                filterClause, orderClause, out total);
                if (grvData.IsInsert)
                    lst.Insert(0, new TblMauXetNghiem());
                //var ee =

                grv.DataSource = lst;
                List<TblMauXetNghiem> lstEmplty = new List<TblMauXetNghiem>();
                lstEmplty.Add(new TblMauXetNghiem());
                grvData.DataSourceEmpty = lstEmplty;
                grv.VirtualRecordCount = total;

                //if (CurrentPageType == "TraKQXN")
                //{
                //    foreach (TblMauXetNghiem mau in lst)
                //    {
                //        mau.NgayInKetQua = null;
                //       XetNghiemManager.ResetNgayInKQ(mau.Id);
                //    }
                //}
            }
        }

        private void LoadControl()
        {
            List<TblCatLoaiMau> loaiMauCol = CategoryManager.GetLoaiMauALL(true);
            TblCatLoaiMau empty = new TblCatLoaiMau();
            empty.Id = -1;
            empty.Ten = "Tất cả";
            loaiMauCol.Insert(0, empty);
            cbSearchLoaiMau.DataSource = loaiMauCol;
            cbSearchLoaiMau.DataValueField = TblCatLoaiMau.Columns.Id;
            cbSearchLoaiMau.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbSearchLoaiMau.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            TblCatQuyChuan empty2 = new TblCatQuyChuan();
            empty2.Id = -1;
            empty2.Ten = "Tất cả";
            quyChuanCol.Insert(0, empty2);
            cbSearchQuyChuan.DataSource = quyChuanCol;
            cbSearchQuyChuan.DataValueField = TblCatLoaiMau.Columns.Id;
            cbSearchQuyChuan.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbSearchQuyChuan.DataBind();

            List<TblCatLoaiHinhYeuCau> loaiHinhYeuCauCol = CategoryManager.GetLoaiHinhYeuCauAll(true);
            TblCatLoaiHinhYeuCau empty4 = new TblCatLoaiHinhYeuCau();
            empty4.Id = -1;
            empty4.Ten = "Tất cả";
            loaiHinhYeuCauCol.Insert(0, empty4);
            cbSearchLoaiHinh.DataSource = loaiHinhYeuCauCol;
            cbSearchLoaiHinh.DataValueField = TblCatLoaiHinhYeuCau.Columns.Id;
            cbSearchLoaiHinh.DataTextField = TblCatLoaiHinhYeuCau.Columns.Ten;
            cbSearchLoaiHinh.DataBind();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //txtSearchTenChiTieu.Text = string.Empty;
            //cbbSearchStatus.SelectedIndex = 0;
            txtSearchDiaChiKhach.Text = txtSearchMaMau.Text = txtSearchTenKhachHang.Text =
                txtSearchTenNguoiGuiMau.Text = txtSearchTenNguoiLayMau.Text = txtSearchTenNguoiNhanMau.Text = "";
            cbSearchLoaiHinh.SelectedIndex = cbSearchLoaiMau.SelectedIndex = cbSearchQuyChuan.SelectedIndex = 0;
            dtSearchNgayTraKQToi.DateValue = dtSearchNgayTraKQTu.DateValue =
                dtSearchNgayNhanMauTu.DateValue = dtSearchNgayNhanMauToi.DateValue =
                dtSearchNgayLayMauTu.DateValue = dtSearchNgayLayMauToi.DateValue = DateTime.MinValue;
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
                        if (CurrentMessageBoxResult.CommandName == "delete_MauXN")
                        {
                            TblMauXetNghiem mauXN = CurrentMessageBoxResult.Value as TblMauXetNghiem;
                            if (mauXN != null)
                            {
                                try
                                {
                                    CurrentLog.AddNewLogFunction(RightPageName.PMXN_Delete,
                                                                 string.Format("Xóa {0}", CurrentName));
                                    CurrentLog.AddFirstValue(RightPageName.PMXN_Delete, string.Format("Mã mẫu {0}", CurrentName),
                                                             TblMauXetNghiem.Columns.MaMau,
                                                             mauXN.MaMau);
                                    resultDelete = XetNghiemManager.DeleteMauXetNghiem(mauXN.Id);
                                }
                                catch
                                {
                                }
                                if (resultDelete)
                                {
                                    grvData.Rebind();
                                    ShowNotify(string.Format("Xóa {0} thành công", CurrentName),
                                                string.Format("Bạn vừa xóa {1} <b>{0}</b>",
                                                              mauXN.MaMau, CurrentName),
                                                NotifyType.Success);
                                    CurrentLog.SaveLog(RightPageName.PMXN_Delete);
                                }
                                else
                                {
                                    ShowNotify(string.Format("Xóa {0} thất bại", CurrentName),
                                                string.Format("Có lỗi xảy ra khi xóa {1} <b>{0}</b>",
                                                               mauXN.MaMau, CurrentName),
                                                NotifyType.Error);
                                }
                                CloseMessageBox();
                            }
                        }
                    }
                }
                else
                {
                    TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(btn.CommandArgument);
                    if (mauXN != null)
                    {
                        btnDelete.Visible = true;
                        //btnTraKetQuaSubmit.Visible = false;
                        MessageBoxResult result = new MessageBoxResult();
                        result.Value = mauXN;
                        result.Submit = true;
                        result.CommandName = "delete_MauXN";
                        CurrentMessageBoxResult = result;
                        OpenMessageBox(string.Format("Xóa {0}", CurrentName),
                                       string.Format("Bạn thật sự muốn xóa mẫu xét nghiệm <b>{0}</b> ?",
                                                     mauXN.MaMau, CurrentName));
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

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("mau-xet-nghiem/0");
        }

        protected void btnInPhieuDeNghiThanhToan_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                string cmdName = btn.CommandName;
                string cmdA = btn.CommandArgument;
                TblMauXetNghiem mauXN = XetNghiemManager.GetMauXetNghiemByID(cmdA);
                if (mauXN != null)
                {
                    traMauControl.LoadMau(mauXN);
                    hiddenTraMauID.Value = cmdA;
                    dialogTraMau.ShowDialog();
                }
            }
        }

        protected void btnInPhieuTraMau_Click(object sender, EventArgs e)
        {

            bool b = traMauControl.Save();
            if (b)
            {
                string url = string.Format("/phieu-de-nghi-thu-tien-so-{0}", hiddenTraMauID.Value);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');{1}", url, dialogTraMau.GetScriptClose()), true);
            }
        }

        protected void btnThanhLy_Click(object sender, EventArgs e)
        {
            ExtraButton btn = sender as ExtraButton;
            if (btn != null)
            {
                string cmdA = btn.CommandArgument;
                TblMauXetNghiem mxn = XetNghiemManager.GetMauXetNghiemByID(cmdA);
                if (mxn != null)
                {
                    if (CurrentPageType.EndsWith("HoaLy"))
                    {
                        mxn.NgayThanhLyThucTeHoa = DateTime.Now;
                        mxn.NguoiThanhLyHoa = ApplicationContext.Current.CommonUser.HoVaTen;
                    }
                    else
                    {
                        mxn.NgayThanhLyThucTeVS = DateTime.Now;
                        mxn.NguoiThanLyVS = ApplicationContext.Current.CommonUser.HoVaTen;
                    }

                    mxn = XetNghiemManager.UpdateMauXetNghiem(mxn);
                    ShowNotify("Thông báo", string.Format("Đã thanh lý mẫu xét nghiệm <b>{0}</b>", mxn.MaMau), NotifyType.Success);
                    grvData.Rebind();
                }
            }
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            if (hiddenShowAll.Value == "0")
                hiddenShowAll.Value = "1";
            else
                hiddenShowAll.Value = "0";
            grvData.Rebind();
            if (hiddenShowAll.Value == "0") btnShowAll.Text = "Xem toàn bộ danh sách";
            else btnShowAll.Text = "Xem công việc cần làm";
        }

        protected void lnkPhieuTraKetQuaInPhoi_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                string cmdN = btn.CommandArgument;
                string cmdA = btn.CommandName;
                string url = string.Format("/phieu-tra-ket-qua-in-phoi-so-{0}", cmdN);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", string.Format("window.open('{0}');", url), true);
            }
        }
    }
}