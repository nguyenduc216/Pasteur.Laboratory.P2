<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="MauXetNghiem.aspx.cs" Inherits="Pasteur.Laboratory.P2.MauXetNghiem" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<%@ Register Src="~/Controls/TraMau.ascx" TagName="TraMau" TagPrefix="SweetSoft1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Danh sách
                        <asp:Literal ID="headerTitle" runat="server"></asp:Literal></h5>
                    <div class=" widget-toolbar filter no-border">
                        <SweetSoft:ExtraComboBox ID="cbbFilterColumns" runat="server" DisplayItemCount="7" CssClass="filter-columns-right grid-columns-list" SelectionMode="Multiple"
                            EnableDropUp="false" ItemSelectedChangeTextCount="1" Width="170px">
                        </SweetSoft:ExtraComboBox>
                        <a class="white" data-action="fullscreen" href="#"><i class="ace-icon fa fa-expand"></i></a>
                    </div>
                    <div class="widget-toolbar no-border">
                        <SweetSoft:ExtraButton ID="btnAddNew" Visible="false" Transparent="true" runat="server" EIcon="A_Add" Text="Thêm mới" OnClick="btnAddNew_Click">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton ID="btnShowAll" Visible="false" Transparent="true" runat="server" EIcon="A_Eye" Text="Xem toàn bộ danh sách" OnClick="btnShowAll_Click">
                        </SweetSoft:ExtraButton>
                        <asp:HiddenField ID="hiddenShowAll" Value="0" runat="server" />
                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main ">
                        <SweetSoft:ExtraGrid Width="100%" ID="grvData" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                            runat="server" AllowPaging="true" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowDataBound="grvData_RowCreated"
                            OnNeedDataSource="grvData_NeedDataSource" PagingControlName="paging">
                            <EmptyDataTemplate>
                                Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%# GetIndex(grvData, Container.DataItemIndex)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã mẫu " AccessibleHeaderText="Tên " HeaderStyle-Width="70px" ItemStyle-Width="70px" SortExpression="mauXN.MaMau">
                                    <ItemTemplate>
                                        <SweetSoft:ExtraButton Transparent="true" CssClass="text-black" Style="color: #000 !important;" ID="lnkEdit" Visible="false" runat="server" CommandName="Update"></SweetSoft:ExtraButton>
                                        <asp:Label ID="lbName" Visible="false" runat="server"></asp:Label>
                                        <asp:Literal ID="ltrMaNoiBo" runat="server"></asp:Literal>
                                        <asp:Literal ID="ltrMaTraKetQua" runat="server"></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tên khách hàng " AccessibleHeaderText="Tên " HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="khachHang.TenKhachHang">
                                    <ItemTemplate>
                                        <%# Eval("TenKhachHang")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Địa chỉ " AccessibleHeaderText="Tên " HeaderStyle-Width="250px" ItemStyle-Width="250px" SortExpression="khachHang.DiaChi">
                                    <ItemTemplate>
                                        <%# Eval("DiaChiKhachHang")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Điện thoại " AccessibleHeaderText="Tên " HeaderStyle-Width="70px" ItemStyle-Width="70px" SortExpression="khachHang.DienThoai">
                                    <ItemTemplate>
                                        <%# Eval("DienThoaiKhachHang")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Người gửi mẫu" AccessibleHeaderText="Tên " HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="mauXN.TenNguoiGuiMau">
                                    <ItemTemplate>
                                        <%# Eval("TenNguoiGuiMau")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Người lấy mẫu" AccessibleHeaderText="Tên " HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="mauXN.TenNguoiLayMau">
                                    <ItemTemplate>
                                        <%# Eval("TenNguoiLayMau")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Người nhận mẫu" AccessibleHeaderText="Tên " HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="mauXN.TenNguoiNhanMau">
                                    <ItemTemplate>
                                        <%# Eval("TenNguoiNhanMau")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loại mẫu" AccessibleHeaderText="Tên " HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="loaimau.Ten">
                                    <ItemTemplate>
                                        <%# Eval("TenLoaiMau")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quy chuẩn" AccessibleHeaderText="Tên " HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="quyChuan.Ten">
                                    <ItemTemplate>
                                        <%# Eval("TenQuyChuan")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loại hình yêu cầu" AccessibleHeaderText="Tên " HeaderStyle-Width="120px" ItemStyle-Width="120px" SortExpression="quyChuan.Ten">
                                    <ItemTemplate>
                                        <%# Eval("TenLoaiHinhYeuCau")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày lấy mẫu" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayLayMau">
                                    <ItemTemplate>
                                        <%# Eval("NgayLayMauToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày nhận mẫu" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayNhanMau">
                                    <ItemTemplate>
                                        <%# Eval("NgayNhanMauToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày trả KQ" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayTraKetQua">
                                    <ItemTemplate>
                                        <%# Eval("NgayTraKetQuaToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Ngày thanh lý" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayTraKetQua">
                                    <ItemTemplate>
                                        <%# GetNgayThanhLy(Container.DataItem)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Người thanh lý" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayTraKetQua">
                                    <ItemTemplate>
                                        <%# Eval("NguoiThanhLy")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tổng / Hóa / Vi sinh" AccessibleHeaderText="Tên " HeaderStyle-Width="130px" ItemStyle-Width="130px">
                                    <ItemTemplate>
                                        <%# GetTongSo(Container.DataItem)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="TrangThaiSuDung">
                                    <ItemTemplate>
                                        <%# GetTrangThai(Container.DataItem) %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày duyệt Hóa" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayTraKetQua">
                                    <ItemTemplate>
                                        <%# Eval("NgayHoaLyToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày duyệt VS" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayTraKetQua">
                                    <ItemTemplate>
                                        <%# Eval("NgayViSinhToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Kết quả" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-Width="100px" SortExpression="mauXN.NgayTraKetQua">
                                    <ItemTemplate>
                                        <%# Eval("KetQuaMauToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tổng tiền" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="TrangThaiSuDung">
                                    <ItemTemplate>
                                        <%# GetDecimalToString(Eval("TongTien")) %> đ
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã nội bộ " AccessibleHeaderText="Tên " HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="khachHang.TenKhachHang">
                                    <ItemTemplate>
                                        <%# GetMaNoiBo(Container.DataItem)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Thao tác" AccessibleHeaderText="" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="">
                                    <HeaderTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnSearch" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Search" ToolTip="Tìm kiếm" OnClientClick='<%# string.Format("{0}return false;", dialogSearch.GetScriptOpen()) %>'>
                                            </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnCancel" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy tìm kiếm" CommandName="Cancel_Search" OnClick="btnCancel_Click" Visible='<%# grvData.BindingWithFiltering %>'>
                                            </SweetSoft:ExtraButton>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnEdit" Visible="false" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" CommandArgument='<%# Eval("ID") %>'>
                                            </SweetSoft:ExtraButton>
                                            <div class="btn-group alright" id="btnPrintGroup" runat="server">
                                                <button data-toggle="dropdown" class="btn btn-warning  btn-xs dropdown-toggle" style="font-size: 15px;" aria-expanded="false">
                                                    <span class="ace-icon fa fa-print"></span>
                                                    <span class="ace-icon fa fa-caret-down icon-on-right"></span>
                                                </button>

                                                <ul class="dropdown-menu dropdown-inverse">
                                                    <li>
                                                        <asp:LinkButton ID="lnkInPhieuTiepNhanYC" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandName="1" CommandArgument='<%# Eval("ID") %>' OnClientClick="document.forms[0].target = '_blank';">
                                        <span class="ace-icon fa fa-print"></span> Phiếu tiếp nhận yêu cầu
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPhieuDeNghiThuTien" Visible="false" runat="server" OnClick="btnInPhieuDeNghiThanhToan_Click" ToolTip="In phiếu đề nghị thu tiền" CommandArgument='<%# Eval("ID") %>'>
                                        <span class="ace-icon fa fa-print"></span> Phiếu đề nghị thu tiền
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkInPhieuChuyenMau" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandName="2" CommandArgument='<%# Eval("ID") %>'>
                                        <span class="ace-icon fa fa-print"></span> Phiếu chuyển mẫu
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPhieuTraKetQua" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandName="4" CommandArgument='<%# Eval("ID") %>'>
                                        <span class="ace-icon fa fa-print"></span> Phiếu trả kết quả
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkPhieuTraKetQuaInPhoi" Visible="true" runat="server" OnClick="lnkPhieuTraKetQuaInPhoi_Click" CommandName="4" CommandArgument='<%# Eval("ID") %>'>
                                        <span class="ace-icon fa fa-print"></span> Phiếu trả kết quả (in phôi)
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkInPhieuKetQuaNoiNo" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandName="3" CommandArgument='<%# Eval("ID") %>'>
                                                         <span class="ace-icon fa fa-print"></span> Phiếu kết quả kiểm nghiệm nội bộ
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="ltrInBienBanGhiChepThuNghiem" Visible="false" runat="server" OnClick="lnkPrint_Onclick" CommandName="5" CommandArgument='<%# Eval("ID") %>'>
                                                         <span class="ace-icon fa fa-print"></span> Biên bản ghi chép thử nghiệm
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>

                                            <SweetSoft:ExtraButton ID="btnInPhieuDeNghiThanhToan" Visible="false" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Print" OnClick="btnInPhieuDeNghiThanhToan_Click" ToolTip="In phiếu đề nghị thu tiền" CommandArgument='<%# Eval("ID") %>'>
                                            </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnDelete" Visible="false" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="confirm">
                                            </SweetSoft:ExtraButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnSubmit" runat="server" CssClass="btn-xs" EStyle="Success" EnableSmallSize="true" EIcon="A_Submit" ToolTip="Cập nhật" CommandName="Update">
                                            </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="ExtraButton1" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy" CommandName="Cancel">
                                            </SweetSoft:ExtraButton>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <SweetSoft:Paging ID="paging" runat="server" />
                            </PagerTemplate>
                        </SweetSoft:ExtraGrid>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
    <SweetSoft:ExtraDialog ID="dialogTraMau" runat="server" RestrictionZoneID="body" Size="Normal" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            Thông tin phiếu thu tiền
        </HeaderTemplate>
        <ContentTemplate>
            <SweetSoft1:TraMau ID="traMauControl" runat="server" />
            <asp:HiddenField ID="hiddenTraMauID" runat="server" />
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnInPhieuTraMau" runat="server" OnClick="btnInPhieuTraMau_Click" EStyle="Warning_Bold" EIcon="A_Print" Text="In phiếu"></SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogSearch" runat="server" RestrictionZoneID="body" Size="Large" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Literal ID="ltrDialogSearchTitle" runat="server" Text="Tìm kiếm ">
            </asp:Literal>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12 col-lg-2">
                    <div class="form-group">
                        <label class="extra-label">
                            <asp:Literal ID="ltrMaMau" Text="Mã mẫu" runat="server"></asp:Literal>

                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchMaMau" PlaceHolder="Nhập mã mẫu cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-2 no-padding-left">
                    <div class="form-group">
                        <label class="extra-label">
                            <asp:Literal ID="ltrTimKiemMaMau" Text="Mã mẫu" runat="server"></asp:Literal>

                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchMaNoiBo" PlaceHolder="Nhập mã  cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Tên khách hàng
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchTenKhachHang" PlaceHolder="Nhập tên khách hàng cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Địa chỉ khách hàng
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchDiaChiKhach" PlaceHolder="Nhập địa chỉ khách cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Tên người gửi mẫu
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchTenNguoiGuiMau" PlaceHolder="Nhập tên người gửi mẫu cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Tên người lấy mẫu
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchTenNguoiLayMau" PlaceHolder="Nhập tên người lấy mẫu cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Tên người nhận mẫu
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchTenNguoiNhanMau" PlaceHolder="Nhập tên người nhận cần tìm" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Loại mẫu
                        </label>
                        <SweetSoft:ExtraComboBox ID="cbSearchLoaiMau" runat="server" EmptyMessage="Chọn loại mẫu tìm kiếm"></SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Quy chuẩn
                        </label>
                        <SweetSoft:ExtraComboBox ID="cbSearchQuyChuan" runat="server" ComboStyle="Warning" EmptyMessage="Chọn quy chuẩn tìm kiếm"></SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-xs-12 col-lg-4">
                    <div class="form-group">
                        <label class="extra-label">
                            Loại hình yêu cầu mẫu
                        </label>
                        <SweetSoft:ExtraComboBox ID="cbSearchLoaiHinh" runat="server" ComboStyle="Success" EmptyMessage="Chọn loại hình yêu cầu mẫu tìm kiếm"></SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày lấy mẫu từ</span>
                        <SweetSoft:ExtraDateTimePicker ID="dtSearchNgayLayMauTu" runat="server" DateTimeMask="Date" PlaceHolder="Ngày lấy mẫu từ" Width="100%" />
                    </div>
                </div>
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày nhận mẫu từ</span>
                        <SweetSoft:ExtraDateTimePicker ID="dtSearchNgayNhanMauTu" runat="server" DateTimeMask="Date" PlaceHolder="Ngày nhận mẫu từ" Width="100%" />
                    </div>
                </div>
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày trả kết quả từ</span>
                        <SweetSoft:ExtraDateTimePicker ID="dtSearchNgayTraKQTu" runat="server" DateTimeMask="Date" PlaceHolder="Ngày trả kết quả từ" Width="100%" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày lấy mẫu tới</span>
                        <SweetSoft:ExtraDateTimePicker ID="dtSearchNgayLayMauToi" runat="server" DateTimeMask="Date" PlaceHolder="Ngày lấy mẫu " Width="100%" />
                    </div>
                </div>
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày nhận mẫu tới</span>
                        <SweetSoft:ExtraDateTimePicker ID="dtSearchNgayNhanMauToi" runat="server" DateTimeMask="Date" PlaceHolder="Ngày nhận mẫu " Width="100%" />
                    </div>
                </div>
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày trả kết quả tới</span>
                        <SweetSoft:ExtraDateTimePicker ID="dtSearchNgayTraKQToi" runat="server" DateTimeMask="Date" PlaceHolder="Ngày trả kết quả" Width="100%" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Trạng thái mẫu</span>
                        <SweetSoft:ExtraComboBox ID="cbSearchTrangThaiMau" runat="server" ComboStyle="Success">
                            <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                            <asp:ListItem Value="New" Text="Tạo mới"></asp:ListItem>
                            <asp:ListItem Value="Labo" Text="Đã chuyển Labo"></asp:ListItem>
                            <asp:ListItem Value="DangThucHien" Text="Đang thực hiện Labo"></asp:ListItem>
                            <asp:ListItem Value="ChoTraKetQua" Text="Chờ trả kết quả"></asp:ListItem>
                            <asp:ListItem Value="DaTraKetQua" Text="Đã trả kết quả"></asp:ListItem>
                            <asp:ListItem Value="Huy" Text="Hủy"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày duyệt Hóa</span>
                        <SweetSoft:ExtraDateTimePicker ID="dateSearchHoaLyFrom" runat="server" DateTimeMask="Date" PlaceHolder="Ngày duyệt Hóa lý " Width="100%" />
                    </div>
                </div>
                <div class="col-lg-4 col-xs-12">
                    <div class="form-group">
                        <span class="extra-label">Ngày duyệt Vi Sinh</span>
                        <SweetSoft:ExtraDateTimePicker ID="dateSearchViSinhFrom" runat="server" DateTimeMask="Date" PlaceHolder="Ngày duyệt vi sinh" Width="100%" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnSearchNow" EnableRound="false" EStyle="Primary_Bold" runat="server" Text="Lọc dữ liệu" EIcon="A_Filter" OnClick="btnSearchNow_Click" CommandName="Search">  </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnDelete" Visible="false" runat="server" OnClick="btnDelete_Click" CommandName="exec_delete" Text="Xóa" EIcon="A_Delete" EStyle="Danger_Bold"></SweetSoft:ExtraButton>
    <%--<SweetSoft:ExtraButton ID="btnTraKetQuaSubmit" Visible="false" runat="server" OnClick="btnTraKetQua_Click" CommandName="TraKetQua" Text="Đồng ý" EIcon="A_Check" EStyle="Success_Bold"></SweetSoft:ExtraButton>--%>
</asp:Content>
