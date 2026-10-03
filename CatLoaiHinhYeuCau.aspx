<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="CatLoaiHinhYeuCau.aspx.cs" Inherits="Pasteur.Laboratory.P2.LoaiHinhYeuCau" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
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
                            runat="server" AllowPaging="true" ShowHeaderWhenEmpty="true" AllowSorting="true" OnRowCancelingEdit="grvData_RowCancelingEdit" OnRowUpdating="grvData_RowUpdating" OnRowCreated="grvData_RowCreated"
                            OnNeedDataSource="grvData_NeedDataSource" PagingControlName="paging">
                            <emptydatatemplate>
                                Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                            </emptydatatemplate>
                            <columns>
                                <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%# GetIndex(grvData, Container.DataItemIndex)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Mã " AccessibleHeaderText="Tên " HeaderStyle-Width="200px" ItemStyle-Width="200px" SortExpression="Ten">
                                    <ItemTemplate>
                                        <%# Eval("Code")  %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <SweetSoft:ExtraTextBox ID="txtEditCode"  style="width:100% !important" EnterSubmitClientID='ctl00_ContentPlaceHolder1_grvData_ctl02_btnSubmit' Required="true" CssClass="textbox-edit-width" PlaceHolder="Nhập code" ValidRequiredMessage="Nhập code" runat="server">
                                        </SweetSoft:ExtraTextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tên " AccessibleHeaderText="Tên " HeaderStyle-Width="200px" ItemStyle-Width="200px" SortExpression="Ten">
                                    <ItemTemplate>
                                        <%# Eval("Ten")  %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hdfEditID" runat="server" />
                                        <SweetSoft:ExtraTextBox ID="txtEditTen"  style="width:100% !important" EnterSubmitClientID='ctl00_ContentPlaceHolder1_grvData_ctl02_btnSubmit' Required="true" CssClass="textbox-edit-width" PlaceHolder="Nhập tên loại hình yêu cầu" ValidRequiredMessage="Nhập tên loại hình yêu cầu" runat="server">
                                        </SweetSoft:ExtraTextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="TrangThaiSuDung">
                                    <ItemTemplate>
                                        <%# GetStatusStyle(Eval("TrangThaiSuDung")) %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <SweetSoft:ExtraCheckBox ID="chkEditStatus" runat="server" CheckboxType="Switch7" DataOn="Bật" DataOff="Tắt" Checked="true">
                                        </SweetSoft:ExtraCheckBox>
                                    </EditItemTemplate>
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
                                            <SweetSoft:ExtraButton ID="btnEdit" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" CommandName="Edit">
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
                            </columns>
                            <pagertemplate>
                                <SweetSoft:Paging ID="paging" runat="server" />
                            </pagertemplate>
                        </SweetSoft:ExtraGrid>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandName="exec_delete" Text="Xóa" EIcon="A_Delete" EStyle="Danger_Bold"> </SweetSoft:ExtraButton>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
    <SweetSoft:ExtraDialog ID="dialogSearch" runat="server" RestrictionZoneID="body" Size="Small" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            <asp:Literal ID="ltrDialogSearchTitle" runat="server" Text="Tìm kiếm ">
            </asp:Literal>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12">
                    <div class="form-group">
                        <label class="extra-label">
                            <asp:Literal ID="ltrSearchTenLoaiHinhYeuCau" runat="server"></asp:Literal>
                        </label>
                        <SweetSoft:ExtraTextBox ID="txtSearchTenLoaiHinhYeuCau" runat="server">  </SweetSoft:ExtraTextBox>
                    </div>
                   
                    <div class="form-group">
                        <label class="extra-label">Trạng thái  </label>
                        <SweetSoft:ExtraComboBox ID="cbbSearchStatus" EnableLiveSearch="true" ComboStyle="Dropdown_White" runat="server">
                            <asp:ListItem Value="" Text="Tất cả">  </asp:ListItem>
                            <asp:ListItem Value="1" Text="Kích hoạt">  </asp:ListItem>
                            <asp:ListItem Value="0" Text="Không kích hoạt"> </asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnSearchNow" EnableRound="false" EStyle="Primary_Bold" runat="server" Text="Lọc dữ liệu" EIcon="A_Filter" OnClick="btnSearchNow_Click" CommandName="Search">  </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
</asp:Content>
