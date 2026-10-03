<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="QLNguoiDung.aspx.cs" Inherits="Pasteur.Laboratory.P2.QLNguoiDung" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<%@ Register Src="~/Controls/QLTTLT_UCUserInRole.ascx" TagName="UCUserInRole" TagPrefix="SweetSoft1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="udpContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Danh sách tài khoản</h5>
                    <div class=" widget-toolbar filter no-border">
                        <SweetSoft:ExtraComboBox
                            ID="cbbFilterColumns"
                            runat="server"
                            DisplayItemCount="9"
                            CssClass="filter-columns-right grid-columns-list"
                            SelectionMode="Multiple"
                            EnableDropUp="false"
                            ItemSelectedChangeTextCount="1"
                            Width="170px">
                        </SweetSoft:ExtraComboBox>
                        <a class="white"
                            data-action="fullscreen"
                            href="#">
                            <i class="ace-icon fa fa-expand"></i>
                        </a>
                    </div>
                    <div class="widget-toolbar no-border">
                        <SweetSoft:ExtraButton
                            ID="btnAddNew"
                            Transparent="true"
                            runat="server"
                            EIcon="A_Add"
                            Text="Thêm mới"
                            OnClick="btnAddNew_Click">
                        </SweetSoft:ExtraButton>
                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main">
                        <SweetSoft:ExtraGrid
                            ID="grvUser"
                            AutoResponsive="true"
                            EnableFixHeader="true"
                            AutoGenerateColumns="false"
                            runat="server"
                            Width="100%"
                            AllowPaging="true"
                            AllowSorting="true"
                            OnNeedDataSource="grvUser_NeedDataSource"
                            PagingControlName="paging" ShowHeaderWhenEmpty="true">
                            <EmptyDataTemplate>
                                Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField
                                    HeaderText="STT"
                                    AccessibleHeaderText="STT2"
                                    ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="5%"
                                    ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%# GetIndex(grvUser, Container.DataItemIndex)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Tên nhân viên" AccessibleHeaderText="Tên nhân viên" HeaderStyle-Width="200px" SortExpression="nv.Ten">
                                    <ItemTemplate>
                                        <%# Eval("HoVaTen")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email" AccessibleHeaderText="Email" HeaderStyle-Width="200px" SortExpression="nd.Email">
                                    <ItemTemplate>
                                        <%# Eval("Email")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Giới tính" AccessibleHeaderText="Giới tính" HeaderStyle-Width="100px" SortExpression="nv.GioiTinh">
                                    <ItemTemplate>
                                        <%# Eval("GioiTinh")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Phòng ban" AccessibleHeaderText="Phòng ban" HeaderStyle-Width="200px" SortExpression="pb.TenPhongBan">
                                    <ItemTemplate>
                                        <%# Eval("TenPhongBan")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chức vụ" AccessibleHeaderText="Phòng ban" HeaderStyle-Width="200px" SortExpression="pb.NhomNhanVien">
                                    <ItemTemplate>
                                        <%# Eval("NhomNhanVien")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Có tài khoản" AccessibleHeaderText="Có tài khoản" HeaderStyle-Width="100px" SortExpression="nd.Email">
                                    <ItemTemplate>
                                        <%# CheckAccount(Container.DataItem)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" SortExpression="nv.TrangThaiSuDung" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <%# GetStatusStyle(Eval("TrangThaiSuDung")) %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Thao tác" AccessibleHeaderText="" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnSearch" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Search" ToolTip="Tìm kiếm" OnClientClick='<%# string.Format("{0}return false;",    dialogSearch.GetScriptOpen()) %>'>
                                            </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnCancel" runat="server" EStyle="Danger" CssClass="btn-xs" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy tìm kiếm" CommandName="Cancel_Search" OnClick="btnCancel_Click" Visible='<%# grvUser.BindingWithFiltering %>'>
                                            </SweetSoft:ExtraButton>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnEdit" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" OnClick="btnEdit_Click" CommandArgument='<%# Eval("ID") %>'> </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnAuthorization" runat="server" CssClass="btn-xs" EStyle="Violet" EnableSmallSize="true" EIcon="A_Config" ToolTip="Phân quyền chức năng" CommandArgument='<%# Eval("IDNguoiDung") %>' OnClick="btnAuthorization_Click">  </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnResetPass" runat="server" CssClass="btn-xs" EStyle="Success" EnableSmallSize="true" EIcon="A_Refresh" ToolTip="Cấp lại mật khẩu" OnClick="btnResetPass_Click" CommandArgument='<%# Eval("IDNguoiDung") %>' CommandName="confirm"> </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnDelete" runat="server" EStyle="Danger" CssClass="btn-xs" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="confirm">  </SweetSoft:ExtraButton>
                                        </div>
                                    </ItemTemplate>
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
<asp:Content ID="Content5" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
    <SweetSoft:ExtraDialog runat="server" ID="dialogSearch" EnableDraggable="true" HeadButtons="Close" Effect="FadeIn" Size="Normal" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Label runat="server" ID="Label1"> </asp:Label>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Họ và tên </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchHoVaTen" runat="server"> </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Chức vụ</label>
                        <SweetSoft:ExtraComboBox ID="cbSearchChucVu" runat="server" ComboStyle="Dropdown_White">
                            <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                            <asp:ListItem Value="Nhân viên" Text="Nhân viên"></asp:ListItem>
                            <asp:ListItem Value="Thủ kho" Text="Thủ kho"></asp:ListItem>
                            <asp:ListItem Value="Quản lý" Text="Quản lý"></asp:ListItem>
                            <asp:ListItem Value="Ban giám đốc" Text="Ban giám đốc"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Giới tính </label>
                        <SweetSoft:ExtraComboBox ID="cbSearchGioiTinh" runat="server">
                            <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                            <asp:ListItem Value="Nam" Text="Nam"></asp:ListItem>
                            <asp:ListItem Value="Nữ" Text="Nữ"></asp:ListItem>
                            <asp:ListItem Value="Không xác định" Text="Không xác định"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Trạng thái sử dụng</label>
                        <SweetSoft:ExtraComboBox ID="cbSearchTrangThai" runat="server">
                            <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Kích hoạt"></asp:ListItem>
                            <asp:ListItem Value="0" Text="Khóa"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Phòng ban</label>
                        <SweetSoft:ExtraComboBox ID="cbSearchPhongBan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbSearchPhongBan_SelectedIndexChanged">
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                 <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12" style="display:none;">
                    <div class="form-group">
                        <label class="extra-label">Tổ nhân viên</label>
                        <SweetSoft:ExtraComboBox ID="cbSearchToNhanVien" runat="server">
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Tài khoản đăng nhâp</label>
                        <SweetSoft:ExtraComboBox ID="cbSearchTaiKhoanDangNhap" runat="server">
                            <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                            <asp:ListItem Value=" is not null " Text="Có tài khoản"></asp:ListItem>
                            <asp:ListItem Value=" is null " Text="Không có tài khoản"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">

                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Email</label>
                        <SweetSoft:ExtraTextBox ID="txtSearchEmail" runat="server"></SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnSearchNow" runat="server" EIcon="A_Filter" EStyle="Default_Bold" ESubmitAndCheckValid="true" EnableSnipButton="true" ValidationGroup="Detail" OnClick="btnSearchNow_Click" Text="Tìm kiếm">  </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogRole" runat="server" EnableDraggable="true" HeadButtons="Close" Effect="FadeIn" Size="Large" CloseText="Đóng">
        <HeaderTemplate>
            Phân vai trò cho người dùng
            <asp:Label runat="server" ID="lblRoleTitle"> </asp:Label>
        </HeaderTemplate>
        <ContentTemplate>
            <SweetSoft1:UCUserInRole runat="server" ID="ucUserInRole" />
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnRoleForUser" runat="server" EIcon="A_Save" EStyle="Success_Bold" EnableSnipButton="true" Text="Lưu" OnClick="btnRoleForUser_Click"> </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog runat="server" ID="dialogDetail" EnableDraggable="true" HeadButtons="Close" Effect="FadeIn" Size="Normal" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Label runat="server" ID="lblDialogDetailTitle"> </asp:Label>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Họ * </label>
                        <SweetSoft:ExtraTextBox ID="txtDetailHo" runat="server"> </SweetSoft:ExtraTextBox>
                        <asp:HiddenField ID="hdfDetailUserID" runat="server" />
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Tên * </label>
                        <SweetSoft:ExtraTextBox ID="txtDetailTen" runat="server"> </SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Giới tính </label>
                        <SweetSoft:ExtraComboBox ID="cbEditGioiTinh" runat="server" ComboStyle="Dropdown_White">
                            <asp:ListItem Value="Nam" Text="Nam"></asp:ListItem>
                            <asp:ListItem Value="Nữ" Text="Nữ"></asp:ListItem>
                            <asp:ListItem Value="Không xác định" Text="Không xác định"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Ngày sinh </label>
                        <SweetSoft:ExtraDateTimePicker ID="dateEditNgaySinh" Width="100%" runat="server" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Phòng ban</label>
                        <SweetSoft:ExtraComboBox ID="cbEditPhongBan" runat="server" ComboStyle="Dropdown_White" AutoPostBack="true" OnSelectedIndexChanged="cbEditPhongBan_SelectedIndexChanged">
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                 <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12"  style="display:none;">
                    <div class="form-group">
                        <label class="extra-label">Tổ nhân viên</label>
                        <SweetSoft:ExtraComboBox ID="cbEditToNhanVien" runat="server" ComboStyle="Dropdown_White">
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Chức vụ</label>
                        <SweetSoft:ExtraComboBox ID="cbEditChucVu" runat="server" ComboStyle="Dropdown_White">
                            <asp:ListItem Value="Nhân viên" Text="Nhân viên"></asp:ListItem>
                            <asp:ListItem Value="Thủ kho" Text="Thủ kho"></asp:ListItem>
                            <asp:ListItem Value="Quản lý" Text="Quản lý"></asp:ListItem>
                            <asp:ListItem Value="Ban giám đốc" Text="Ban giám đốc"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Có tài khoản người dùng</label><br />
                        <SweetSoft:ExtraCheckBox ID="chkEditCoTaiKhoanNguoiDung" OnCheckedChanged="chkEditCoTaiKhoanNguoiDung_CheckedChanged" AutoPostBack="true" runat="server" CheckboxType="Switch7" DataOff="Không" DataOn="Có"></SweetSoft:ExtraCheckBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Email</label>
                        <SweetSoft:ExtraTextBox ID="txtEditEmail" Enabled="false" runat="server"></SweetSoft:ExtraTextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">Trạng thái sử dụng </label>
                        <br />
                        <SweetSoft:ExtraCheckBox ID="chkDetailTrangThaiSuDung" runat="server" CheckboxType="Switch7" DataOn="Bật" DataOff="Khóa" Width="100%"></SweetSoft:ExtraCheckBox>
                    </div>
                </div>

            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnSave" runat="server" EIcon="A_Save" Text="Lưu dữ liệu" EStyle="Success_Bold" ESubmitAndCheckValid="true" EnableSnipButton="true" ValidationGroup="Detail" OnClick="btnSave_Click">  </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandName="exec_delete" Text="Xóa" EIcon="A_Delete" EStyle="Danger_Bold"> </SweetSoft:ExtraButton>
</asp:Content>
