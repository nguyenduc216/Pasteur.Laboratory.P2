<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="MauXetNghiemChiTiet.aspx.cs" Inherits="Pasteur.Laboratory.P2.MauXetNghiemChiTiet" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<%@ Register Src="~/Controls/TraMau.ascx" TagName="TraMau" TagPrefix="SweetSoft1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .hide {
            display: none;
        }

        /*
         * Code: COMBO-POPUP-LAYER-20260918
         * Liên quan: repUpdateChiTieu/cbInput.
         * Đưa popup giới hạn chỉ tiêu lên trên header và chừa khoảng cuộn để item cuối hiển thị đầy đủ.
         */
        .bootstrap-select.criteria-limit-popup.open {
            position: relative;
            z-index: 20050 !important;
        }

        .bootstrap-select.criteria-limit-popup.open > .dropdown-menu {
            z-index: 20051 !important;
            max-height: calc(100vh - 100px) !important;
            overflow: hidden !important;
        }

        .bootstrap-select.criteria-limit-popup.open > .dropdown-menu > .inner {
            max-height: calc(100vh - 115px) !important;
            overflow-y: auto !important;
            padding-bottom: 0 !important;
        }

        /*
         * Code: COMBO-POPUP-LAYER-20260918
         * Bootstrap-select 1.7.2 tính scrollHeight theo các li; đặt khoảng đệm trên li cuối
         * để có thể cuộn toàn bộ link cuối lên khỏi nút combobox.
         */
        .bootstrap-select.criteria-limit-popup.open > .dropdown-menu > .inner > li:last-child {
            padding-bottom: 48px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hiddenTab" runat="server" Value="ThongTin" />
            <div class="widget-box ui-sortable-handle">
                <div class="widget-header ">
                    <div class="widget-title" style="position: relative; float: left;">
                        <ul class="nav nav-tabs" id="myTab" style="margin-top: 5px;">
                            <li>
                                <asp:Literal ID="ltrChangeMa" runat="server"></asp:Literal>
                            </li>
                            <li class='<%= GetTabVisible("ThongTin","") %>'>
                                <a data-toggle="tab" onclick="SetTab('ThongTin')" href="#thongTin" aria-expanded="true">
                                    <asp:Literal ID="ltrThongTin" runat="server" Text="Thông tin mẫu"></asp:Literal>
                                </a>
                            </li>
                            <li id="liChiTieu" class='<%= GetTabVisible("ChiTieu","") %>'>
                                <a data-toggle="tab" onclick="SetTab('ChiTieu')" href="#chiTieu" aria-expanded="false">Danh sách chỉ tiêu</a>
                            </li>

                        </ul>
                    </div>
                    <div class="widget-toolbar ">

                        <SweetSoft:ExtraButton Visible="false" ID="btnAddNew" Transparent="true" runat="server" EIcon="A_Add" Text="Thêm mới" OnClick="btnAddNew_Click">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton Visible="false" ID="btnCopyCode" Transparent="true" runat="server" EIcon="A_Clone" Text="Copy từ mẫu cũ">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton ID="btnSave" Visible="false" EStyle="Success" CssClass="btn-xs" runat="server" EIcon="A_Save" Text="Lưu mẫu" OnClick="btnSave_Click">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton ID="btnUpdateChiTieu" Visible="false" EStyle="Success" CssClass="btn-xs" runat="server" EIcon="A_Save" Text="Labo cập nhật" OnClick="btnUpdateChiTieu_Click">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton ID="btnDuyetLabo" Visible="false" EStyle="Danger" CssClass="btn-xs" runat="server" EIcon="A_Check" Text="Duyệt nội bộ Labo" OnClick="btnDuyetLabo_Click">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton ID="btnCapNhatThongTinTraMau" Visible="false" EStyle="Success" CssClass="btn-xs" runat="server" EIcon="A_Save" Text="Cập nhật thông tin" OnClick="btnGhiNhanKetQua_Click">
                        </SweetSoft:ExtraButton>
                        <div class="btn-group alright" id="btnAddGroup" runat="server" visible="false">
                            <button data-toggle="dropdown" class="btn btn-default  btn-xs dropdown-toggle" style="font-size: 15px;" aria-expanded="false">
                                <span class="ace-icon fa fa-plus"></span>Thêm mẫu XN
													<span class="ace-icon fa fa-caret-down icon-on-right"></span>
                            </button>

                            <ul class="dropdown-menu dropdown-inverse">
                                <li>
                                    <asp:LinkButton ID="lnkAddSingle" runat="server" OnClick="btnAddNew_Click">
                                        <span class="ace-icon fa fa-plus"></span> Tạo mẫu mới
                                    </asp:LinkButton>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton ID="lnkCopySingle" runat="server" OnClick="btnAddNew_Click">
                                        <span class="ace-icon fa fa-clone"></span>Tạo mẫu mới từ mẫu cũ
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                        <div class="btn-group alright" id="divXuLyMau" runat="server" visible="false">
                            <button data-toggle="dropdown" class="btn btn-danger  btn-xs dropdown-toggle" style="font-size: 15px;" aria-expanded="false">
                                <span class="ace-icon fa fa-cog"></span>Xử lý mẫu XN
													<span class="ace-icon fa fa-caret-down icon-on-right"></span>
                            </button>

                            <ul class="dropdown-menu dropdown-inverse">
                                <li>
                                    <asp:LinkButton ID="lnkDoiMaMau" runat="server">
                                        <span class="ace-icon fa fa-retweet"></span> Đổi mã mẫu
                                    </asp:LinkButton>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton ID="linkChuyenMau" runat="server" OnClick="btnChuyenLabo_Click">
                                        <span class="ace-icon fa fa-send"></span> Chuyển mẫu lên labo
                                    </asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                        <SweetSoft:ExtraButton ID="btnChuyenLabo" Visible="false" EStyle="Danger" CssClass="btn-xs" runat="server" EIcon="A_Send" Text="Chuyển mẫu" OnClick="btnChuyenLabo_Click">
                        </SweetSoft:ExtraButton>
                        <div class="btn-group alright" id="btnPrintGroup" runat="server" visible="false">
                            <button data-toggle="dropdown" class="btn btn-warning  btn-xs dropdown-toggle" style="font-size: 15px;" aria-expanded="false">
                                <span class="ace-icon fa fa-print"></span>In ấn
													<span class="ace-icon fa fa-caret-down icon-on-right"></span>
                            </button>

                            <ul class="dropdown-menu dropdown-inverse">
                                <li>
                                    <asp:LinkButton ID="lnkInPhieuTiepNhanYC" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandArgument="1" OnClientClick="document.forms[0].target = '_blank';">
                                        <span class="ace-icon fa fa-print"></span> Phiếu tiếp nhận yêu cầu
                                    </asp:LinkButton>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton ID="lnkPhieuDeNghiThuTien" Visible="false" runat="server" OnClick="lnkPhieuDeNghiThuTien_Click" CommandArgument="2">
                                        <span class="ace-icon fa fa-print"></span> Phiếu đề nghị thu tiền
                                    </asp:LinkButton>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton ID="lnkInPhieuChuyenMau" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandArgument="2">
                                        <span class="ace-icon fa fa-print"></span> Phiếu chuyển mẫu
                                    </asp:LinkButton>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton ID="lnkPhieuTraKetQua" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandArgument="4">
                                        <span class="ace-icon fa fa-print"></span> Phiếu trả kết quả
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkPhieuTraKetQuaM1" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandArgument="4" CommandName="M1">
                                        <span class="ace-icon fa fa-print"></span> Phiếu trả kết quả BITEST
                                    </asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkPhieuTraKetQuaM2" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandArgument="4" CommandName="M2">
                                        <span class="ace-icon fa fa-print"></span> Phiếu trả kết quả không khí
                                    </asp:LinkButton>
                                </li>
                                <li class="divider"></li>
                                <li>
                                    <asp:LinkButton ID="lnkInPhieuKetQuaNoiNo" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandArgument="3">
                                        <span class="ace-icon fa fa-print"></span> Phiếu kết quả kiểm nghiệm nội bộ
                                    </asp:LinkButton>
                                </li>
                                <asp:Literal ID="ltrLI" runat="server"></asp:Literal>
                            </ul>
                        </div>


                    </div>
                </div>
                <div class="widget-body">
                    <div class="tab-content">
                        <div id="thongTin" class='<%= GetTabVisible("ThongTin","tab-pane") %>'>
                            <div id="divThongTinFull" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Khách hàng</span>
                                            <SweetSoft:ExtraTextBox ID="txtTenKhachHang" runat="server" PlaceHolder="Tên khách hàng"></SweetSoft:ExtraTextBox>
                                            <asp:HiddenField ID="hiddenKhachHangID" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-lg-5 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Địa chỉ khách</span>
                                            <SweetSoft:ExtraTextBox ID="txtDiaChiKhach" runat="server" PlaceHolder="Địa chỉ khách hàng"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Điện thoại</span>
                                            <SweetSoft:ExtraTextBox ID="txtDienThoaiKhach" runat="server" PlaceHolder="Điện thoại khách"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Email khách</span>
                                            <SweetSoft:ExtraTextBox ID="txtEmailKhach" Text="/" runat="server" PlaceHolder="Email khách hàng"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Tên người gửi</span>
                                            <div>
                                                <SweetSoft:ExtraTextBox ID="txtTenNguoiGui" runat="server" PlaceHolder="Tên người gửi"></SweetSoft:ExtraTextBox>
                                                <span class="input-group-btn" style="display: none; padding: 2px; border: solid 1px #c1c1c1; background-color: #a7a7a7;">
                                                    <SweetSoft:ExtraCheckBox ID="chkNguoiGuiLaNV" runat="server"></SweetSoft:ExtraCheckBox>
                                                    <span style="font-size: 13px; color: white;">Nhân viên Pasteur</span>
                                                </span>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-lg-5 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Địa chỉ người gửi</span>
                                            <SweetSoft:ExtraTextBox ID="txtDiaChiNguoiGui" runat="server" PlaceHolder="Địa chỉ người gửi"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Điện thoại người gửi mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtDienThoaiNguoiGui" runat="server" PlaceHolder="Điện thoại người gửi mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Email người gửi mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtEmailNguoiGui" Text="/" runat="server" PlaceHolder="Email người gửi mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Tên người lấy mẫu</span>
                                            <div>
                                                <SweetSoft:ExtraTextBox ID="txtTenNguoiLayMau" runat="server" PlaceHolder="Tên người lấy mẫu"></SweetSoft:ExtraTextBox>
                                                <span class="input-group-btn" style="display: none; padding: 2px; border: solid 1px #c1c1c1; background-color: #a7a7a7;">
                                                    <SweetSoft:ExtraCheckBox ID="chkNguoiLayLaNV" runat="server"></SweetSoft:ExtraCheckBox>
                                                    <span style="font-size: 13px; color: white;">Nhân viên Pasteur</span>
                                                </span>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày lấy mẫu</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dtNgayLayMau" runat="server" DateTimeMask="Date" PlaceHolder="Ngày lấy mẫu" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Loại mẫu</span>
                                            <SweetSoft:ExtraComboBox ID="cbLoaiMau" runat="server" EmptyMessage="Chọn loại mẫu"></SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Điện thoại người lấy mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtDienThoaiNguoiLayMau" runat="server" PlaceHolder="Điện thoại người lấy mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Email người lấy mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtEmailNguoiLayMau" Text="/" runat="server" PlaceHolder="Email người lấy mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Tên người nhận mẫu</span>
                                            <div>
                                                <SweetSoft:ExtraTextBox ID="txtTenNguoiNhanMau" runat="server" PlaceHolder="Tên người nhận mẫu"></SweetSoft:ExtraTextBox>
                                                <span class="input-group-btn" style="display: none; padding: 2px; border: solid 1px #c1c1c1; background-color: #a7a7a7;">
                                                    <SweetSoft:ExtraCheckBox ID="chkNguoiNhanLaNV" runat="server"></SweetSoft:ExtraCheckBox>
                                                    <span style="font-size: 13px; color: white;">Nhân viên Pasteur</span>
                                                </span>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày nhận mẫu</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dateNgayNhanMau" DateTimeMask="Date" runat="server" PlaceHolder="Ngày nhận mẫu" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Quy chuẩn</span>
                                            <SweetSoft:ExtraComboBox ID="cbQUyChuan" runat="server" ComboStyle="Warning" EmptyMessage="Chọn quy chuẩn"></SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">ĐT người nhận mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtDienThoaiNguoiNhan" runat="server" PlaceHolder="Điện thoại người nhận mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Email người nhận mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtEmailNguoiNhanMau" Text="/" runat="server" PlaceHolder="Email người nhận mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Yêu cầu khác</span>
                                            <SweetSoft:ExtraTextBox ID="txtYeuCauKhac" runat="server" PlaceHolder="Nhập yêu cầu khác"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày trả kết quả</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dateNgayTraKetQua" DateTimeMask="Date" runat="server" PlaceHolder="Ngày trả kết quả" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-3 col-xs-12 ">
                                        <div class="form-group">
                                            <span class="extra-label">Loại hình yêu cầu</span>
                                            <SweetSoft:ExtraComboBox ID="cbTrangThaiMau" Visible="false" runat="server" ComboStyle="Success">
                                                <asp:ListItem Value="New" Text="Tạo mới"></asp:ListItem>
                                                <asp:ListItem Value="Labo" Text="Đã chuyển Labo"></asp:ListItem>
                                                <asp:ListItem Value="DangThucHien" Text="Đang thực hiện Labo"></asp:ListItem>
                                                <asp:ListItem Value="ChoTraKetQua" Text="Chờ trả kết quả"></asp:ListItem>
                                                <asp:ListItem Value="DaTraKetQua" Text="Đã trả kết quả"></asp:ListItem>
                                                <asp:ListItem Value="Huy" Text="Hủy"></asp:ListItem>
                                            </SweetSoft:ExtraComboBox>
                                            <SweetSoft:ExtraComboBox ID="cbLoaiHinhYeuCauMau" runat="server" ComboStyle="Success">
                                            </SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-1 col-xs-12 no-padding-right">
                                        <div class="form-group">
                                            <span class="extra-label">T/L mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtDungLuongMau" runat="server" PlaceHolder="Dung lượng mẫu" CssClass="mask-float" Text="0"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-1 col-xs-12  no-padding-right">
                                        <div class="form-group">
                                            <span class="extra-label">T/L Hóa</span>
                                            <SweetSoft:ExtraTextBox ID="txtDungLuongMauHoa" runat="server" PlaceHolder="Dung lượng mẫu hóa" CssClass="mask-float" Text="0"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Đơn vị tính</span>
                                            <SweetSoft:ExtraComboBox ID="cbDonViTinh" runat="server" EmptyMessage="Chọn đơn vị tính"></SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Mô tả mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtMoTaMau" runat="server" PlaceHolder="Mô tả mẫu"></SweetSoft:ExtraTextBox>
                                        </div>

                                    </div>
                                    <div class="col-lg-5 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Điều kiện bảo quản</span>

                                            <SweetSoft:ExtraMathTextbox ID="txtDieuKienBaoQuan" runat="server" PlaceHolder="Nhập điều kiện bảo quản" Visible="true" />
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Nơi lấy mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtNoiLayMau" TextMode="MultiLine" Style="height: 35px;" Text="/" runat="server" PlaceHolder="Nơi lấy mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <SweetSoft:ExtraCheckBox ID="chkDanhGiaMau" runat="server" Checked="true" Text="  Đánh giá mẫu"></SweetSoft:ExtraCheckBox><br />
                                            <SweetSoft:ExtraCheckBox ID="chkLayMauNgoai" runat="server" Text="  Lấy mẫu ngoài"></SweetSoft:ExtraCheckBox><br />
                                            <SweetSoft:ExtraCheckBox ID="chkMauTheoABCD" Checked="true" runat="server" Text="  Đánh giá theo ABCD"></SweetSoft:ExtraCheckBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày tạo phiếu</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dateNgayTaoPhieu" DateTimeMask="Date" runat="server" PlaceHolder="Ngày tạo phiếu" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày chuyển mẫu</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dateNgayChuyenMau" DateTimeMask="Date" runat="server" PlaceHolder="Ngày tạo phiếu" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Phạm vi lấy mẫu</span>
                                            <SweetSoft:ExtraComboBox ID="cbNoiLayMauGoiY" runat="server">
                                            </SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ghi chú mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtGhiChuMau" TextMode="MultiLine" Style="height: 35px;" Text="" runat="server" PlaceHolder="Ghi chú mẫu"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divThongTinCoBan" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Loại mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtLoaiMau" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Quy chuẩn</span>
                                            <SweetSoft:ExtraTextBox ID="txtQuyChuan" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Điều kiện bảo quản</span>
                                            <SweetSoft:ExtraMathTextbox ID="txtDieuKienBaoQuan2" runat="server" Enabled="false" />

                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày nhận</span>
                                            <SweetSoft:ExtraTextBox ID="txtNgayNhan" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày chuyển mẫu</span>
                                            <SweetSoft:ExtraTextBox ID="txtNgayChuyenMau" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày trả kết quả:</span>
                                            <SweetSoft:ExtraTextBox ID="txtNgayTraKQ" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Lượng mẫu:</span>
                                            <SweetSoft:ExtraTextBox ID="txtTongLuongMau" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Lượng mẫu Hóa lý:</span>
                                            <SweetSoft:ExtraTextBox ID="txtLuongMauHoa" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Lượng mẫu Vi sinh:</span>
                                            <SweetSoft:ExtraTextBox ID="txtLuongMauViSinh" runat="server" Enabled="false"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày bắt đầu làm</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dateLaboBatDau" runat="server" DateTimeMask="Date" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Ngày kết thúc làm</span>
                                            <SweetSoft:ExtraDateTimePicker ID="dateLaboKetThuc" runat="server" DateTimeMask="Date" Width="100%" />
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">
                                                <asp:Literal ID="ltrMaNoiBo" runat="server" Text="Mã phiếu nội bộ"></asp:Literal>
                                            </span>
                                            <SweetSoft:ExtraTextBox ID="txtEditMaPhieuNoiBo" runat="server"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-2 col-xs-12">
                                        <div class="form-group">
                                            <span class="extra-label">Đánh giá mẫu</span>
                                            <SweetSoft:ExtraCheckBox ID="chkLaboDanhGiaMau" Enabled="false" runat="server"></SweetSoft:ExtraCheckBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="chiTieu" class='<%= GetTabVisible("ChiTieu","tab-pane") %>'>
                            <div id="divAddChiTieu" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-lg-12 col-xs-12">
                                        <div class="form-group">
                                            <SweetSoft:ExtraCheckBox ID="chkCheckSearh" runat="server" AutoPostBack="true" OnCheckedChanged="chkCheckSearh_CheckedChanged" Text="Tick chọn để tìm theo tên nhóm xét nghiệm"></SweetSoft:ExtraCheckBox>
                                            <asp:HiddenField ID="hiddenCheckSearch" runat="server" Value="ChiTieu" />
                                            <asp:HiddenField ID="hiddenChiTieuPhuongPhapAddID" runat="server" />
                                            <asp:HiddenField ID="hiddenChiTieuAddID" runat="server" />
                                            <asp:HiddenField ID="hiddenNhomXNAddID" runat="server" />
                                            <asp:Button ID="btnAddChiTieuSingle" runat="server" OnClick="btnAddChiTieuSingle_Click" Style="display: none" />
                                            <div class="input-group">
                                                <SweetSoft:ExtraTextBox ID="txtAddTenChiTieu" runat="server" PlaceHolder="Nhập tên chỉ tiêu cần tìm và chọn chỉ tiêu thêm vào danh sách"></SweetSoft:ExtraTextBox>
                                                <div class="input-group-btn">
                                                    <SweetSoft:ExtraButton ID="btnViewFromQuyChuan" runat="server" EIcon="A_List" Text="  " EStyle="Warning" OnClick="btnViewFromQuyChuan_Click"></SweetSoft:ExtraButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 20px">
                                    <div class="col-lg-12">
                                        <div class="div-data">
                                            <div style="float: left; width: 50%;">
                                                <span style="margin-left: 6px; font-weight: bold;">
                                                    <SweetSoft:ExtraCheckBox ID="chAll" EnableClickGroup="true" CssClass="XAll" GroupName="XAll" runat="server"></SweetSoft:ExtraCheckBox>
                                                    Chọn tất cả</span>
                                                <span class="control-red">
                                                    <SweetSoft:ExtraButton ID="btnDelete" runat="server" EIcon="A_Delete" Transparent="true" Text="Xóa các dòng đang chọn" OnClick="btnDelete_Click"></SweetSoft:ExtraButton>
                                                </span>
                                            </div>
                                            <div style="float: right; width: 50%; text-align: right;">
                                                <asp:Literal ID="ltrHoaSinh" runat="server"></asp:Literal>
                                            </div>
                                            <table class="tab-chiTieu">
                                                <tbody>
                                                    <asp:Repeater ID="repDataFinal" runat="server" OnItemDataBound="repDataFinal_ItemDataBound">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="LtrTRBegin" runat="server"></asp:Literal>
                                                            <SweetSoft:ExtraCheckBox ID="chkItem" ValidationGroup="xAll" CssClass="XAll" GroupName="XAll" runat="server"></SweetSoft:ExtraCheckBox>
                                                            <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                                                            <asp:HiddenField runat="server" ID="hiddenItemID" />
                                                            <asp:Repeater ID="repDataFinalSub" runat="server" OnItemDataBound="repDataFinalSub_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:Literal ID="LtrTRBegin" runat="server"></asp:Literal>
                                                                    <SweetSoft:ExtraCheckBox ID="chkItemSub" ValidationGroup="xAll" CssClass="XAll" GroupName="XAll" runat="server"></SweetSoft:ExtraCheckBox>
                                                                    <asp:Literal ID="ltrContent" runat="server"></asp:Literal>
                                                                    <asp:Literal ID="LtrTREnd" runat="server"></asp:Literal>
                                                                    <asp:HiddenField runat="server" ID="hiddenItemSubID" />
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <asp:Literal ID="LtrTREnd" runat="server"></asp:Literal>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <tr>
                                                        <td style="">
                                                            <div style="float: left;">
                                                                <div style="margin-right: 10px">
                                                                    <SweetSoft:ExtraCheckBox ID="chkVimcert" OnCheckedChanged="chkVimcert_CheckedChanged" AutoPostBack="true" runat="server" Text="Chọn là vimcert"></SweetSoft:ExtraCheckBox>
                                                                </div>
                                                                <div class="input-group">
                                                                    <div style="">
                                                                        <SweetSoft:ExtraComboBox Visible="false" EnableLiveSearch="true" Width="250px" EnableDropUp="false" ShowArrowPopup="false" ID="cbUpdateDVT" runat="server" ComboStyle="Success_Bold" EmptyMessage="Chọn đơn vị tính"></SweetSoft:ExtraComboBox>
                                                                    </div>
                                                                    <div class="input-group-btn">
                                                                        <SweetSoft:ExtraButton Height="33px" Visible="false" ID="btnUpdateDVT" runat="server" EStyle="Success" EIcon="A_Save" Text="  " OnClick="btnUpdateDVT_Click"></SweetSoft:ExtraButton>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div style="float: right; width: 100px; text-align: right; padding-right: 10px; font-weight: bold; padding-top: 6px;">Tổng cộng:</div>
                                                            <div style="clear: both"></div>
                                                        </td>
                                                        <td style="text-align: right; font-weight: bold;">
                                                            <asp:Literal ID="ltrTongTien" runat="server"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="divUpdateChitieu" runat="server" visible="false">
                                <div class="row">
                                    <div class="col-lg-6">
                                        <div class="form-group">
                                            <span class="extra-label">Ghi chú kết quả kiểm tra</span>
                                            <SweetSoft:ExtraTextBox ID="txtGhiChuKetQua" TextMode="MultiLine" Height="100px" runat="server" PlaceHolder="Ghi chú kết quả kiểm tra"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-6" id="divNhanXet" runat="server">
                                        <div class="form-group">
                                            <asp:Literal ID="ltrNhanXetTitle" runat="server" Text="Nhận xét cuối cùng  [KQKT]"></asp:Literal>
                                            <SweetSoft:ExtraTextBox ID="txtNhanXetCuoiCung" Visible="false" TextMode="MultiLine" Height="100px" runat="server" PlaceHolder="Nhận xét cuối cùng"></SweetSoft:ExtraTextBox>
                                            <SweetSoft:ExtraTextBox ID="txtNhanXetLaboHoa" Visible="false" TextMode="MultiLine" Height="100px" runat="server" PlaceHolder="Nhận xét labo hóa"></SweetSoft:ExtraTextBox>
                                            <SweetSoft:ExtraTextBox ID="txtNhanXetLaboVS" Visible="false" TextMode="MultiLine" Height="100px" runat="server" PlaceHolder="Nhận xét labo vi sinh"></SweetSoft:ExtraTextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="divKQ" runat="server" visible="false">
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label class="extra-label">
                                                Mã trả kết quả
                                                <asp:Literal ID="ltrMaTraKetQua" runat="server"></asp:Literal></label>
                                            <SweetSoft:ExtraTextBox ID="txtEditMaTraKetQua" runat="server" PlaceHolder="Nhập mã trả kết quả"></SweetSoft:ExtraTextBox>

                                        </div>
                                    </div>
                                    <div class="col-lg-3" id="divKetQua2" runat="server">
                                        <div class="form-group">
                                            <label class="extra-label">Kết quả mẫu</label>
                                            <SweetSoft:ExtraComboBox ID="cbKetQuaDatMau" runat="server">
                                                <asp:ListItem Value="" Text="Chưa chọn kết quả"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Mẫu đạt"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Mẫu không đạt"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Không đánh giá"></asp:ListItem>
                                            </SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>
                                    <div class="col-lg-3" id="divChatLuong" runat="server">
                                        <div class="form-group">
                                            <label class="extra-label">Trưởng khoa đánh giá</label>
                                            <SweetSoft:ExtraComboBox ID="cbMauDatChatLuong" runat="server">
                                                <asp:ListItem Value="" Text="Chưa chọn kết quả"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Chấp nhận chất lượng"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="Mẫu kiểm tra lại"></asp:ListItem>
                                            </SweetSoft:ExtraComboBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-3" style="padding-top: 26px;">
                                        <div class="btn-group" id="btnGhiNhanKetQuaGroup" runat="server">
                                            <asp:LinkButton ID="lnkGhiNhanKetQua" runat="server" CssClass="btn btn-success" CommandName="CapNhat" OnClick="btnGhiNhanKetQua_Click" Text="">
                                                <span class="ace-icon fa fa-floppy-o"></span> <span>Cập nhật thông tin</span>
                                            </asp:LinkButton>
                                            <button type="button" id="divCapNhat" visible="false" runat="server" class="px-2 btn  btn-success dropdown-toggle dropdown-toggle-split" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <span class="sr-only">Toggle Dropdown</span>
                                                <i class="fa fa-caret-down"></i>
                                            </button>
                                            <div class=" dropdown-menu">
                                                <ul style="list-style: none; margin: 0px;">
                                                    <li style="padding: 5px;">
                                                        <asp:LinkButton ID="lnkCapNhatVaTraKetQua" runat="server" CssClass="dropdown-item" CommandName="CapNhatVaTraKQ" OnClick="btnGhiNhanKetQua_Click" Text="Cập nhật và trả kết quả"></asp:LinkButton></li>
                                                    <li class="divider"></li>
                                                    <li style="padding: 5px;">
                                                        <asp:LinkButton ID="lnkTraKetQua" runat="server" CssClass="dropdown-item" CommandName="TraKQ" OnClick="btnGhiNhanKetQua_Click" Text="Trả kết quả"></asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div class="row" style="margin-top: 20px">
                                    <div class="col-lg-12">
                                        <div class="div-data">
                                            <div style="float: left; width: 50%;">
                                                <span style="margin-left: 6px; font-weight: bold;">
                                                    <SweetSoft:ExtraCheckBox ID="chkCheckAllGoKetQua" Visible="false" EnableClickGroup="true" CssClass="XAll1" GroupName="XAll1" runat="server" Text="Chọn tất cả"></SweetSoft:ExtraCheckBox>
                                                </span>
                                                <span class="control-red">
                                                    <SweetSoft:ExtraButton ID="btnGoKetQua" Visible="false" runat="server" EIcon="A_Retweet" Transparent="true" Text="Bỏ kết quả các dòng đang chọn" OnClick="btnGoKetQua_Click"></SweetSoft:ExtraButton>
                                                </span>
                                            </div>
                                            <div style="float: left; width: 50%; text-align: right;">
                                                <asp:Literal ID="ltrChiTieuUpdateCount" runat="server"></asp:Literal>
                                            </div>
                                            <SweetSoft:ExtraComboBox ID="cbPhepSoSanhCompare" Visible="false" runat="server" ComboStyle="Dropdown_White">
                                                <asp:ListItem Value="LHGH" Text="Lớn hơn các giới hạn"></asp:ListItem>
                                                <asp:ListItem Value="LHBGH" Text="Lớn hơn bằng các giới hạn"></asp:ListItem>
                                                <asp:ListItem Value="BHGH" Text="Bé hơn các giới hạn"></asp:ListItem>
                                                <asp:ListItem Value="BHBGH" Text="Bé hơn bằng các giới hạn"></asp:ListItem>
                                                <asp:ListItem Value="TKGH" Text="Trong khoảng các giới hạn"></asp:ListItem>
                                                <asp:ListItem Value="TKHBGH" Text="Trong khoảng bằng các giới hạn"></asp:ListItem>
                                                <asp:ListItem Value="BGH" Text="Bằng các giới hạn"></asp:ListItem>
                                            </SweetSoft:ExtraComboBox>
                                            <asp:HiddenField ID="hiddenColumnCount" runat="server" Value="0" />
                                            <table class="tab-chiTieu">
                                                <tbody>
                                                    <tr>
                                                        <td rowspan="2" style="width: 30%; text-align: center;">Tên chỉ tiêu</td>
                                                        <td rowspan="2" style="width: 10%; text-align: center;">Đơn vị</td>

                                                        <td rowspan="2" style="width: 25%; text-align: center;">
                                                            <asp:HiddenField ID="hiddenGiaTriNhapVS" runat="server" Value="binhthuong" />
                                                            <span class="control-red">
                                                                <SweetSoft:ExtraButton ID="btnGiaTriNhapVS" runat="server" Visible="false" EIcon="A_Retweet" Transparent="true" Text="Chuyển đổi" OnClick="btnGiaTriNhapVS_Click"></SweetSoft:ExtraButton>
                                                                <SweetSoft:ExtraButton ID="btnGiaTriBe" runat="server" EIcon="A_List" Transparent="true" Text="" OnClick="btnGiaTriBe_Click"></SweetSoft:ExtraButton>
                                                            </span>
                                                            <asp:Literal ID="ltrGiaTriNhap" runat="server" Text="Nhập giá trị"></asp:Literal>
                                                            <span class="control-green">
                                                                <SweetSoft:ExtraButton ID="btnUpdateGiaTriBe" runat="server" EIcon="A_Save" Transparent="true" Text="" OnClick="btnUpdateGiaTriBe_Click"></SweetSoft:ExtraButton>
                                                            </span>
                                                        </td>
                                                        <asp:Literal ID="ltrColumnHeaderGroup" runat="server"></asp:Literal>
                                                        <td rowspan="2" style="width: 10%; text-align: center;">Đánh giá tự động</td>
                                                        <td rowspan="2" style="width: 10%; text-align: center;">Kết quả đánh giá</td>
                                                    </tr>
                                                    <tr>
                                                        <asp:Literal ID="ltrColumnHeader" runat="server"></asp:Literal>
                                                    </tr>
                                                    <%--<asp:Literal ID="ltrColumnBind" runat="server"></asp:Literal>--%>
                                                    <asp:Repeater ID="repUpdateChiTieu" runat="server" OnItemDataBound="repUpdateChiTieu_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td style="text-align: left; width: 45%;">
                                                                    <SweetSoft:ExtraCheckBox ID="chkItem" Visible="false" ValidationGroup="xAll" CssClass="XAll1" GroupName="XAll1" runat="server"></SweetSoft:ExtraCheckBox>
                                                                    <asp:Literal ID="ltrTenChiTieu" runat="server"></asp:Literal>
                                                                     <asp:HiddenField ID="hiddenChiTieuID" runat="server" />
                                                                </td>
                                                                <td style="text-align: center; width: 10%;">
                                                                    <asp:Literal ID="ltrDonViTinh" runat="server"></asp:Literal>
                                                                    <asp:HiddenField ID="hiddenDonViTinh" runat="server" />
                                                                     <asp:HiddenField ID="hiddenDonViTinhID" runat="server" />
                                                                    <SweetSoft:ExtraComboBox ID="cbDonViTinh" runat="server" Visible="false" EmptyMessage="Chọn đơn vị tính">
                                                                    </SweetSoft:ExtraComboBox>
                                                                </td>
                                                                <%--<asp:Literal ID="ltrContentGioiHanChoPhep" runat="server"></asp:Literal>--%>
                                                                <td style="text-align: center; width: 25%;">

                                                                    <asp:Panel ID="panelBinhThuong" runat="server">
                                                                        <div style="width: 20%; padding-right: 5%; display: inline-table;">
                                                                            <asp:Literal ID="ltrKetQuaHienThi" runat="server"></asp:Literal>
                                                                            <SweetSoft:ExtraComboBox ID="cbPhepSoSanh" EnableLiveSearch="false" CssClass="drop-empty" runat="server" ComboStyle="Warning_Bold" Visible="false">
                                                                                <asp:ListItem Value="<" Text="<"></asp:ListItem>
                                                                                <asp:ListItem Value="=" Text="=" Selected="True"></asp:ListItem>
                                                                                <asp:ListItem Value=">" Text=">"></asp:ListItem>
                                                                            </SweetSoft:ExtraComboBox>
                                                                        </div>
                                                                        <div style="width: 75%; display: inline-table;">
                                                                            <SweetSoft:ExtraTextBox ID="txtInput" Text="0" runat="server" CssClass="mask-float" Visible="false"></SweetSoft:ExtraTextBox>
                                                                            <%-- Code: COMBO-POPUP-LAYER-20260918 - CSS class định danh popup giới hạn chỉ tiêu cần ưu tiên trên header. --%>
                                                                            <SweetSoft:ExtraComboBox ID="cbInput" CssClass="criteria-limit-popup" runat="server" ComboStyle="Dropdown_White" Visible="false"></SweetSoft:ExtraComboBox>
                                                                            <asp:Literal ID="ltrNA" runat="server"></asp:Literal>

                                                                            <asp:HiddenField ID="hiddenCTQCID" runat="server" />
                                                                            <asp:HiddenField ID="hiddenMCTID" runat="server" />
                                                                            
                                                                        </div>
                                                                        <div style="clear: both"></div>
                                                                    </asp:Panel>
                                                                    <asp:Panel ID="panelNhapkhoang" runat="server" Visible="false">
                                                                        <div class="nhapkhoang-item">
                                                                            <span class="title-small">Vị trí 1</span>
                                                                            <asp:Literal ID="ltrViTri1" runat="server"></asp:Literal>
                                                                            <SweetSoft:ExtraTextBox ID="txtViTri1" CssClass="txt-small mask-float" Visible="false" runat="server" PlaceHolder="0"></SweetSoft:ExtraTextBox>
                                                                            <SweetSoft:ExtraComboBox ID="cbViTri1" CssClass="cb-small" Visible="false" runat="server" ShowArrowPopup="false"></SweetSoft:ExtraComboBox>
                                                                        </div>
                                                                        <div class="nhapkhoang-item">
                                                                            <span class="title-small">Vị trí 2</span>
                                                                            <asp:Literal ID="ltrViTri2" runat="server"></asp:Literal>
                                                                            <SweetSoft:ExtraTextBox ID="txtViTri2" CssClass="txt-small mask-float" Visible="false" runat="server" PlaceHolder="0"></SweetSoft:ExtraTextBox>
                                                                            <SweetSoft:ExtraComboBox ID="cbViTri2" CssClass="cb-small" Visible="false" runat="server" ShowArrowPopup="false"></SweetSoft:ExtraComboBox>
                                                                        </div>
                                                                        <div class="nhapkhoang-item">
                                                                            <span class="title-small">Vị trí 3</span>
                                                                            <asp:Literal ID="ltrViTri3" runat="server"></asp:Literal>
                                                                            <SweetSoft:ExtraTextBox ID="txtViTri3" CssClass="txt-small mask-float" Visible="false" runat="server" PlaceHolder="0"></SweetSoft:ExtraTextBox>
                                                                            <SweetSoft:ExtraComboBox ID="cbViTri3" CssClass="cb-small" Visible="false" runat="server" ShowArrowPopup="false"></SweetSoft:ExtraComboBox>
                                                                        </div>
                                                                        <div class="nhapkhoang-item">
                                                                            <span class="title-small">Vị trí 4</span>
                                                                            <asp:Literal ID="ltrViTri4" runat="server"></asp:Literal>
                                                                            <SweetSoft:ExtraTextBox ID="txtViTri4" CssClass="txt-small mask-float" Visible="false" runat="server" PlaceHolder="0"></SweetSoft:ExtraTextBox>
                                                                            <SweetSoft:ExtraComboBox ID="cbViTri4" CssClass="cb-small" Visible="false" runat="server" ShowArrowPopup="false"></SweetSoft:ExtraComboBox>
                                                                        </div>

                                                                        <div class="nhapkhoang-item">
                                                                            <span class="title-small">Vị trí 5</span>
                                                                            <asp:Literal ID="ltrViTri5" runat="server"></asp:Literal>
                                                                            <SweetSoft:ExtraTextBox ID="txtViTri5" CssClass="txt-small mask-float" Visible="false" runat="server" PlaceHolder="0"></SweetSoft:ExtraTextBox>
                                                                            <SweetSoft:ExtraComboBox ID="cbViTri5" CssClass="cb-small" Visible="false" runat="server" ShowArrowPopup="false"></SweetSoft:ExtraComboBox>
                                                                        </div>
                                                                    </asp:Panel>
                                                                </td>
                                                                <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                                                <td style="text-align: center; width: 10%;">
                                                                    <asp:Literal ID="ltrKetQua" runat="server"></asp:Literal>
                                                                </td>
                                                                <td style="text-align: center; width: 10%;">
                                                                    <asp:Literal ID="ltrKetQuaTuDanhGia" runat="server"></asp:Literal>
                                                                    <SweetSoft:ExtraCheckBox ID="chkTuDanhGia" runat="server" DataOn="Đạt" DataOff="Không đạt" Width="120px" StyleOn="Success" CheckboxType="Toggle" Text="Kết quả đạt"></SweetSoft:ExtraCheckBox>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
    <SweetSoft:ExtraDialog ID="dialogDoiMaMau" runat="server" RestrictionZoneID="body" Size="Normal" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            Thay đổi mã mẫu
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-6">

                    <div class="form-group">
                        <span class="extra-label">Điều chỉnh theo tháng</span>
                        <SweetSoft:ExtraComboBox ID="cbThang" runat="server" EmptyMessage="Chọn tháng" Width="120px"></SweetSoft:ExtraComboBox>
                    </div>

                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <span class="extra-label" style="display: block;">/ năm</span>
                        <SweetSoft:ExtraComboBox ID="cbNam" runat="server" EmptyMessage="Chọn năm" Width="120px"></SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-group">
                        <span class="extra-label">Điều chỉnh với mã nhập</span>
                        <SweetSoft:ExtraTextBox ID="txtMaDieuChinh" runat="server" PlaceHolder="Nhập mã điều chỉnh"></SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnDoiMaMau" runat="server" OnClick="btnDoiMaMau_Click" EStyle="Warning_Bold" EIcon="A_Retweet" Text="Xác nhận đổi"></SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogTraMau" runat="server" RestrictionZoneID="body" Size="Normal" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            Thông tin phiếu đề nghị thu tiền
        </HeaderTemplate>
        <ContentTemplate>
            <SweetSoft1:TraMau ID="traMauControl" runat="server" />
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnInPhieuTraMau" runat="server" OnClick="btnInPhieuTraMau_Click" EStyle="Warning_Bold" EIcon="A_Print" Text="In phiếu"></SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogConfig" runat="server" RestrictionZoneID="body" Size="FullScreen" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Literal ID="ltrDialogSearchTitle" runat="server" Text="Danh sách chỉ tiêu ">
            </asp:Literal>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 div-data">
                    <span style="margin-left: 6px; font-weight: bold;">
                        <SweetSoft:ExtraCheckBox ID="chkAllConfig" EnableClickGroup="true" CssClass="XAllConfig" GroupName="XAllConfig" runat="server"></SweetSoft:ExtraCheckBox>
                        Chọn tất cả</span>
                    <asp:Repeater ID="repDataConfig" runat="server">
                        <ItemTemplate>
                            <div class="search-item">
                                <SweetSoft:ExtraCheckBox ID="chkitem" ValidationGroup="XAllConfig" CssClass="XAllConfig" GroupName="XAllConfig" runat="server"></SweetSoft:ExtraCheckBox>
                                <span style="font-weight: bold; color: #712d94; display: inline-block;">
                                    <%# Eval("TenTaiCacKhoa") %>
                                </span>
                                <span style="margin-left: 10px; display: inline-block;">[ <%# Eval("TenPhuongPhap") %>]</span>


                                <%--<span class="span-item"><%# Eval("TenThongTu") %> </span><span class="span-item">&raquo;</span>--%>
                                <span class="span-item"><%# Eval("TenTrongTTQD") %> </span>
                                <span style="clear: both"></span>
                                <asp:HiddenField ID="hiddenItemID" runat="server" Value='<%# Eval("ID") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnAddConfig" runat="server" EIcon="A_Add_Square" EStyle="Success_Bold" Text="Thêm chỉ tiêu" OnClick="btnAddConfig_Click"></SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogCopy" runat="server" RestrictionZoneID="body" Size="Normal" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            Nhập mã mẫu cần copy
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-group">
                        <label class="extra-label">Mã mẫu</label>
                        <SweetSoft:ExtraTextBox ID="txtCopyMaMau" runat="server" PlaceHolder="Nhập mã mẫu copy"></SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnCopy" runat="server" EStyle="Success_Bold" EIcon="A_Clone" Text="Đồng ý" OnClick="btnCopy_Click"></SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogChiTieuQuyChuan" runat="server" RestrictionZoneID="body" Size="Large" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            Thay đổi mã mẫu
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <asp:Repeater ID="repChiTieuQuyChuan" runat="server">
                        <HeaderTemplate>
                            <table class="tab-chiTieu">
                                <thead>
                                    <td style="width: 30px; text-align: center">
                                        <SweetSoft:ExtraCheckBox EnableClickGroup="true" ID="chkAllItem" ValidationGroup="xAll1" CssClass="xAll1" GroupName="xAll1" runat="server"></SweetSoft:ExtraCheckBox>
                                    </td>
                                    <td style="width: 80%;">Tên chỉ tiêu</td>
                                    <td style="text-align: center;">Đơn giá</td>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center">
                                    <SweetSoft:ExtraCheckBox ID="chkItem" ValidationGroup="xAll1" CssClass="xAll1" GroupName="xAll1" runat="server"></SweetSoft:ExtraCheckBox></td>
                                <td>
                                    <span class='title'><%# Eval("TenTaiCacKhoa") %></span><span class='sub-title'>[ <%# Eval("TenPhuongPhap") %></span> ]</span>
                                  <asp:HiddenField ID="hiddenItemID" Value='<%# Eval("ID") %>' runat="server" />
                                    <asp:HiddenField ID="hiddenChiTieuPP" Value='<%# Eval("IDCTPP") %>' runat="server" />
                                    <asp:HiddenField ID="hiddenPhuongPhapID" Value='<%# Eval("IDPhuongPhap") %>' runat="server" />
                                </td>
                                <td class='price'><%# Eval("DonGiaToString") %></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody> </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnThemChiTieuQuyChuan" runat="server" Text="Thêm chỉ tiêu đang chọn" EIcon="A_Check" EStyle="Success_Bold" OnClick="btnThemChiTieuQuyChuan_Click"></SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
    <script src="/scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            SearchFullText();
            addRequestHanlde(SearchFullText);
        });

        function SetTab(tabName) {
            $("input[type='hidden'][id$='hiddenTab']").val(tabName);
        }

        function SearchFullText(s, a) {
            if ($("input[type='text'][id$='txtAddTenChiTieu']").length > 0) {
                $("input[type='text'][id$='txtAddTenChiTieu']").on("focus", function () {
                    $(this).select();
                });
                var txtAddTenChiTieu = $("input[id$='txtAddTenChiTieu']");
                if (txtAddTenChiTieu.length > 0) {
                    txtAddTenChiTieu.autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "/Default.aspx/SearchChiTieuPhuongPhapByKeyWord",
                                data: "{'key':'" + $("input[type='text'][id$='txtAddTenChiTieu']").val() + "', 'type':'" + $("input[type='hidden'][id$='hiddenCheckSearch']").val() + "'}",
                                dataType: "json",
                                async: true,
                                cache: false,
                                success: function (result) {
                                    console.log("result: ", result);
                                    response($.map(result.d.List, function (item) {
                                        var type = $("input[type='hidden'][id$='hiddenCheckSearch']").val();
                                        if (type === "ChiTieu") {
                                            return {
                                                TenTaiCacKhoa: item.TenTaiCacKhoa,
                                                TenTrongTTQD: item.TenTrongTTQD,
                                                TenDonVi: item.TenDonVi,
                                                TenPhuongPhap: item.TenPhuongPhap,
                                                TenThongTu: item.TenThongTu,
                                                DisplayText: item.DisplayText,
                                                DonGiaToString: item.DonGiaToString,
                                                ID: item.Id,
                                                IDCTPP: item.IDCTPP
                                            };
                                        }
                                        return {
                                            Ten: item.Ten,
                                            ID: item.Id
                                        };
                                    }));
                                },
                                error: function (msg) {

                                }
                            });
                        },
                        messages: {
                            noResults: '',
                            results: function () { }
                        },
                        focus: function (event, ui) {
                            return false;
                        },
                        select: function (event, ui) {
                            //$("input[type='text'][id$='txtAddTenChiTieu']").val(ui.item.DisplayText);
                            //$("input[type='hidden'][id$='hiddenChiTieuAddID']").val(ui.item.ID);
                            var type = $("input[type='hidden'][id$='hiddenCheckSearch']").val();
                            if (type === "ChiTieu") {
                                if (ui.item.IDCTPP != null && ui.item.IDCTPP > 0) {
                                    $("input[type='hidden'][id$='hiddenChiTieuPhuongPhapAddID']").val(ui.item.IDCTPP);
                                    $("[id$='btnAddChiTieuSingle']").click();
                                    $("input[type='text'][id$='txtAddTenChiTieu']").val("");
                                    $("input[type='text'][id$='txtAddTenChiTieu']").focus();
                                }
                            }
                            else {
                                $("input[type='hidden'][id$='hiddenChiTieuPhuongPhapAddID']").val(ui.item.ID);
                                $("[id$='btnAddChiTieuSingle']").click();
                                $("input[type='text'][id$='txtAddTenChiTieu']").val("");
                                $("input[type='text'][id$='txtAddTenChiTieu']").focus();
                            }
                            return false;
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {
                        var type = $("input[type='hidden'][id$='hiddenCheckSearch']").val();
                        if (type === "ChiTieu") {
                            var li = $("<li>")
                                .data("ui-autocomplete-item", item)
                                .append("<div class='search-item'><div class='displaytext'><span class='tenkhoa' style='font-weight:bold;color:#712d94;display:inline-block;'>" + '' + '</span>' +
                                    "<span style='margin-left:10px;display:inline-block;'> [ " + item.TenPhuongPhap + "] </div>" +
                                    "<span class='span-item'> " + item.TenTrongTTQD + "</span>&raquo;" +
                                    //"<span class='span-item'> " + item.TenThongTu + "</span>&raquo;" +
                                    "<span class='span-item'> " + item.DonGiaToString + " đ" +
                                    "</div>");
                            var tenkhoa = $(".tenkhoa", li);

                            tenkhoa.html(item.TenTaiCacKhoa);
                            return li.appendTo(ul);
                        } else {
                            return $("<li>")
                                .data("ui-autocomplete-item", item)
                                .append("<span> " + item.Ten + "</span>")
                                .appendTo(ul);
                        }
                    };
                }
            }
            var txtCopyMaMau = $("input[id$='txtCopyMaMau']");
            if (txtCopyMaMau.length > 0) {
                txtCopyMaMau.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWord",
                            data: "{'key':'" + $("input[type='text'][id$='txtCopyMaMau']").val() + "','column':'MaMau'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                console.log("result: ", result);
                                response($.map(result.d.List, function (item) {
                                    return { MaMau: item.MaMau, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtCopyMaMau']").val(ui.item.MaMau);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.MaMau + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtYeuCauKhac = $("input[id$='txtYeuCauKhac']");
            if (txtYeuCauKhac.length > 0) {
                txtYeuCauKhac.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtYeuCauKhac']").val() + "','column':'YeuCauKhac'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                console.log("result: ", result);
                                response($.map(result.d.List, function (item) {
                                    return { YeuCauKhac: item.YeuCauKhac, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtYeuCauKhac']").val(ui.item.YeuCauKhac);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.YeuCauKhac + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtDieuKienBaoQuan = $("input[id$='txtDieuKienBaoQuan']");
            if (txtDieuKienBaoQuan.length > 0) {
                txtDieuKienBaoQuan.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtDieuKienBaoQuan']").val() + "','column':'DieuKienBaoQuan'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { DieuKienBaoQuan: item.DieuKienBaoQuan, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtDieuKienBaoQuan']").val(ui.item.DieuKienBaoQuan);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.DieuKienBaoQuan + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtMoTaMau = $("input[id$='txtMoTaMau']");
            if (txtMoTaMau.length > 0) {
                txtMoTaMau.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtMoTaMau']").val() + "','column':'MoTaMau'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { MoTaMau: item.MoTaMau, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtMoTaMau']").val(ui.item.MoTaMau);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.MoTaMau + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtNoiLayMau = $("input[id$='txtNoiLayMau']");
            if (txtNoiLayMau.length > 0) {
                txtNoiLayMau.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtNoiLayMau']").val() + "','column':'NoiLayMau'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { NoiLayMau: item.NoiLayMau, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtNoiLayMau']").val(ui.item.NoiLayMau);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.NoiLayMau + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtTenNguoiGui = $("input[id$='txtTenNguoiGui']");
            if (txtTenNguoiGui.length > 0) {
                txtTenNguoiGui.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtTenNguoiGui']").val() + "','column':'TenNguoiGuiMau'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { TenNguoiGuiMau: item.TenNguoiGuiMau, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtTenNguoiGui']").val(ui.item.TenNguoiGuiMau);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.TenNguoiGuiMau + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtTenNguoiLayMau = $("input[id$='txtTenNguoiLayMau']");
            if (txtTenNguoiLayMau.length > 0) {
                txtTenNguoiLayMau.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtTenNguoiLayMau']").val() + "','column':'TenNguoiLayMau'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { TenNguoiLayMau: item.TenNguoiLayMau, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtTenNguoiLayMau']").val(ui.item.TenNguoiLayMau);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.TenNguoiLayMau + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }
            var txtTenNguoiNhanMau = $("input[id$='txtTenNguoiNhanMau']");
            if (txtTenNguoiNhanMau.length > 0) {
                txtTenNguoiNhanMau.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtTenNguoiNhanMau']").val() + "','column':'TenNguoiNhanMau'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { TenNguoiNhanMau: item.TenNguoiNhanMau, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtTenNguoiNhanMau']").val(ui.item.TenNguoiNhanMau);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.TenNguoiNhanMau + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtTenKhachHang = $("input[id$='txtTenKhachHang']");
            if (txtTenKhachHang.length > 0) {
                txtTenKhachHang.change(function () {
                    if (txtTenKhachHang.val().length === 0)
                        $("input[type='hidden'][id$='hiddenKhachHangID']").val("");
                });
                txtTenKhachHang.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchKhachHangByKeyWordDistinct",
                            data: "{'key':'" + $("input[type='text'][id$='txtTenKhachHang']").val() + "','column':'TenKhachHang'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { TenKhachHang: item.TenKhachHang, ID: item.Id, DiaChi: item.DiaChi, DienThoai: item.DienThoai, Email: item.Email };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtTenKhachHang']").val(ui.item.TenKhachHang);
                        $("input[type='text'][id$='txtDiaChiKhach']").val(ui.item.DiaChi);
                        $("input[type='text'][id$='txtDienThoaiKhach']").val(ui.item.DienThoai);
                        $("input[type='text'][id$='txtEmailKhach']").val(ui.item.Email);
                        $("input[type='hidden'][id$='hiddenKhachHangID']").val(ui.item.ID);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.TenKhachHang + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtNhanXetCuoiCung = $("[id$='txtNhanXetCuoiCung']");
            if (txtNhanXetCuoiCung.length > 0) {
                txtNhanXetCuoiCung.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("[id$='txtNhanXetCuoiCung']").val() + "','column':'NhanXetCuoiCung'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { NhanXetCuoiCung: item.NhanXetCuoiCung, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("[id$='txtNhanXetCuoiCung']").val(ui.item.NhanXetCuoiCung);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.NhanXetCuoiCung + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtNhanXetLaboHoa = $("[id$='txtNhanXetLaboHoa']");
            if (txtNhanXetLaboHoa.length > 0) {
                txtNhanXetLaboHoa.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("[id$='txtNhanXetLaboHoa']").val() + "','column':'LaboHoaNhanXet'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { LaboHoaNhanXet: item.LaboHoaNhanXet, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("[id$='txtNhanXetLaboHoa']").val(ui.item.LaboHoaNhanXet);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.LaboHoaNhanXet + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

            var txtNhanXetLaboVS = $("[id$='txtNhanXetLaboVS']");
            if (txtNhanXetLaboVS.length > 0) {
                txtNhanXetLaboVS.autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchMauXetNghiemByKeyWordDistinct",
                            data: "{'key':'" + $("[id$='txtNhanXetLaboVS']").val() + "','column':'LaboSinhNhanXet'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                response($.map(result.d.List, function (item) {
                                    return { LaboHoaNhanXet: item.LaboSinhNhanXet, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("[id$='txtNhanXetLaboVS']").val(ui.item.LaboSinhNhanXet);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.LaboSinhNhanXet + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnSubmitTrans" Visible="false" runat="server" Text="Đồng ý" OnClick="btnChuyenLabo_Click" CommandName="trans_confirm" EStyle="Success_Bold"></SweetSoft:ExtraButton>
    <SweetSoft:ExtraButton ID="btnSubmitDuyetLabo" Visible="false" runat="server" Text="Đồng ý" OnClick="btnDuyetLabo_Click" CommandName="duyet_labo" EStyle="Success_Bold"></SweetSoft:ExtraButton>
    <SweetSoft:ExtraButton ID="btnSubmitThayDoiLoaiHinhYeuCau" Visible="false" runat="server" Text="Đồng ý" OnClick="btnSubmitThayDoiLoaiHinhYeuCau_Click" EStyle="Success_Bold"></SweetSoft:ExtraButton>
</asp:Content>
