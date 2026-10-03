<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ThanhLy.aspx.cs" Inherits="Pasteur.Laboratory.P2.ThanhLy" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<%@ Register Src="~/Controls/ThanhLy.ascx" TagName="ThanhLy" TagPrefix="MLT" %>
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

                        <SweetSoft:ExtraButton ID="btnAddNew" Transparent="true" runat="server" EIcon="A_Add" Text="Thêm mới" OnClick="btnAddNew_Click">
                        </SweetSoft:ExtraButton>


                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main ">
                        <SweetSoft:ExtraGrid Width="100%" ID="grvData" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                            runat="server" AllowPaging="true" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCreated="grvData_RowCreated" OnNeedDataSource="grvData_NeedDataSource"
                            PagingControlName="paging">
                            <EmptyDataTemplate>
                                Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%" SortExpression="Ten">
                                    <ItemTemplate>
                                        <%# GetIndex(grvData, Container.DataItemIndex)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày thực hiện" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" SortExpression="Ten">
                                    <ItemTemplate>
                                        <%# Eval("NgayThucHienToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày kết thúc" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" SortExpression="Ten">
                                    <ItemTemplate>
                                        <%# Eval("ThoiGianKetThucToString")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Người thực hiện" AccessibleHeaderText="Tên " HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200px" SortExpression="Ten">
                                    <ItemTemplate>
                                        <%# Eval("NguoiThucHien")  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mãu thanh lý" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <%# Eval("MaMauList") %>
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
                                            <SweetSoft:ExtraButton ID="btnEdit" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" OnClick="btnEdit_Click" CommandArgument='<%# Eval("ID") %>'>
                                            </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnPrint" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Print" ToolTip="In biển bản" NavigateUrl='<%# string.Format("/bien-ban-thanh-ly/{0}",Eval("ID") ) %>' Target="_blank"> 
                                            </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnDelete" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="confirm">
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
    <SweetSoft:ExtraDialog ID="dialogSearch" runat="server" RestrictionZoneID="body" Size="Normal" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Literal ID="ltrDialogSearchTitle" runat="server" Text="Tìm kiếm ">
            </asp:Literal>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Mã số mẫu
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchMamau" runat="server"></SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Tình trạng trước khi thanh lý
                        </label>
                        <SweetSoft:ExtraComboBox ID="cbSearchTinhTrang" Enabled="false" runat="server" EmptyMessage="Chọn tình trạng trước khi thanh lý">
                            <asp:ListItem Value="" Text="Tất cả" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Bình thường" Text="Bình thường"></asp:ListItem>
                            <asp:ListItem Value="Quá hạn" Text="Quá hạn"></asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Thực hiện từ ngày
                        </label>
                        <SweetSoft:ExtraDateTimePicker ID="dateSearchThucHienFrom" runat="server" PlaceHolder="Chọn ngày" Width="100%" DateTimeMask="Date" />
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            tới ngày
                        </label>
                        <SweetSoft:ExtraDateTimePicker ID="dateSearchThucHienTo" runat="server" PlaceHolder="Chọn ngày" Width="100%" DateTimeMask="Date" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Kết thúc từ ngày
                        </label>
                        <SweetSoft:ExtraDateTimePicker ID="dateSearchKetThucFrom" runat="server" PlaceHolder="Chọn ngày" Width="100%" DateTimeMask="Date" />
                    </div>
                </div>
                <div class="col-lg-6 col-md-6 col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            tới ngày
                        </label>
                        <SweetSoft:ExtraDateTimePicker ID="dateSearchKetThucTo" runat="server" PlaceHolder="Chọn ngày" Width="100%" DateTimeMask="Date" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnSearchNow" EnableRound="false" EStyle="Primary_Bold" runat="server" Text="Lọc dữ liệu" EIcon="A_Filter" OnClick="btnSearchNow_Click" CommandName="Search">  </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogDetail" runat="server" RestrictionZoneID="body" Size="Large" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Literal ID="ltrDetailTitle" runat="server" Text="Thêm mới">
            </asp:Literal>
        </HeaderTemplate>
        <ContentTemplate>
            <MLT:ThanhLy ID="thanhLy" runat="server" />
        </ContentTemplate>
        <FooterTemplate>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
    <script src="/scripts/jquery-ui.min.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandName="exec_delete" Text="Xóa" EIcon="A_Delete" EStyle="Danger_Bold"> </SweetSoft:ExtraButton>
</asp:Content>
