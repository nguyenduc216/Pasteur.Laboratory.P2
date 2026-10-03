<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="QLVaiTro.aspx.cs" Inherits="Pasteur.Laboratory.P2.QLVaiTro" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<%@ Register Src="~/Controls/QLTTLT_UCRightInRole.ascx" TagPrefix="SweetSoft1" TagName="UCRightInRole" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .textbox-edit-width {
            width: 100% !important;
        }
    </style>
</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Danh sách vai trò</h5>
                    <div class=" widget-toolbar filter no-border">
                        <SweetSoft:ExtraComboBox
                            ID="cbbFilterColumns"
                            runat="server"
                            DisplayItemCount="7"
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
                        <SweetSoft:ExtraButton ID="btnAddNew" Transparent="true" runat="server" EIcon="A_Add" Text="Thêm mới" OnClick="btnAddNew_Click">
                        </SweetSoft:ExtraButton>
                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main">
                        <SweetSoft:ExtraGrid ID="grvRole" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false" runat="server" Width="100%" AllowPaging="true" AllowSorting="true" OnRowCancelingEdit="grvRole_RowCancelingEdit"
                            OnRowUpdating="grvRole_RowUpdating" OnRowCreated="grvRole_RowCreated" OnNeedDataSource="grvRole_NeedDataSource" ColumnVisibleDefault="0,1,2,3,4,5" ShowHeaderWhenEmpty="true" PagingControlName="paging">
                            <EmptyDataTemplate>
                                Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%" ItemStyle-Width="5%">
                                    <ItemTemplate>
                                        <%# GetIndex(grvRole, Container.DataItemIndex)  %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tên vai trò" AccessibleHeaderText="Tên vai trò" SortExpression="TenVaiTro">
                                    <ItemTemplate>
                                        <%# Eval("TenVaiTro")  %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="hdfEditID" runat="server" />
                                        <SweetSoft:ExtraTextBox ID="txtEditTitle" Required="true" CssClass="textbox-edit-width" PlaceHolder="Nhập tên vai trò" ValidRequiredMessage="Nhập tên vai trò" runat="server">
                                        </SweetSoft:ExtraTextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Miêu tả" AccessibleHeaderText="Miêu tả" SortExpression="MieuTa" HeaderStyle-Width="30%" ItemStyle-Width="30%">
                                    <ItemTemplate>
                                        <%# Eval("MieuTa")  %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <SweetSoft:ExtraTextBox ID="txtEditDescription" Required="true" CssClass="textbox-edit-width" PlaceHolder="Nhập miêu tả" ValidRequiredMessage="Nhập miêu tả" TextMode="MultiLine" Height="50px" runat="server"> </SweetSoft:ExtraTextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="12%" ItemStyle-Width="12%" SortExpression="TrangThaiSuDung">
                                    <ItemTemplate>
                                        <%# GetStatusStyle(Eval("TrangThaiSuDung")) %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <SweetSoft:ExtraCheckBox ID="chkEditStatus" runat="server" CheckboxType="Switch7" DataOn="Bật" DataOff="Tắt" Checked="true">    </SweetSoft:ExtraCheckBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Thao tác" AccessibleHeaderText="" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnSearch" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Search" ToolTip="Tìm kiếm" OnClientClick='<%# string.Format("{0}return false;",   dialogSearch.GetScriptOpen()) %>'>  </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnCancel" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy tìm kiếm" CommandName="Cancel_Search" OnClick="btnCancel_Click" Visible='<%# grvRole.BindingWithFiltering %>'>  </SweetSoft:ExtraButton>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnEdit" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" CommandName="Edit"> </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton runat="server" ID="btnAuthorization" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Config" ToolTip="Phân quyền chức năng" CommandArgument='<%# Eval("ID") %>' OnClick="btnAuthorization_Click"> </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="btnDelete" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="confirm">  </SweetSoft:ExtraButton>
                                        </div>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <SweetSoft:ExtraButton ID="btnSubmit" runat="server" CssClass="btn-xs" EStyle="Success" EnableSmallSize="true" EIcon="A_Submit" ToolTip="Cập nhật" CommandName="Update">  </SweetSoft:ExtraButton>
                                            <SweetSoft:ExtraButton ID="ExtraButton1" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy" CommandName="Cancel"> </SweetSoft:ExtraButton>
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
<asp:Content ID="Content5" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
    <SweetSoft:ExtraDialog
        ID="dialogSearch"
        runat="server"
        RestrictionZoneID="body"
        HeadButtons="Close"
        CloseText="Đóng">
        <HeaderTemplate>
            <asp:Literal
                ID="ltrDialogSearchTitle"
                runat="server"
                Text="Tìm kiếm vai trò">
            </asp:Literal>
        </HeaderTemplate>
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Tên vai trò
                        </label>
                        <SweetSoft:ExtraTextBox
                            ID="txtSearchTitle"
                            runat="server">
                        </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Miêu tả
                        </label>
                        <SweetSoft:ExtraTextBox
                            ID="txtSearchDescription"
                            runat="server">
                        </SweetSoft:ExtraTextBox>
                    </div>
                </div>
                <div class="col-lg-12">
                    <div class="form-group">
                        <label class="extra-label">
                            Trạng thái
                        </label>
                        <SweetSoft:ExtraComboBox
                            ID="cbbSearchStatus"
                            EnableLiveSearch="true"
                            ComboStyle="Dropdown_White"
                            runat="server">
                            <asp:ListItem
                                Value=""
                                Text="Tất cả">
                            </asp:ListItem>
                            <asp:ListItem
                                Value="1"
                                Text="Kích hoạt">
                            </asp:ListItem>
                            <asp:ListItem
                                Value="0"
                                Text="Không kích hoạt">
                            </asp:ListItem>
                        </SweetSoft:ExtraComboBox>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton
                ID="btnSearchNow"
                EnableRound="false"
                EStyle="Primary_Bold"
                runat="server"
                Text="Lọc dữ liệu"
                EIcon="A_Filter"
                OnClick="btnSearchNow_Click"
                CommandName="Search">
            </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
    <SweetSoft:ExtraDialog ID="dialogFunction" runat="server" EnableDraggable="true" Effect="FadeIn" Width="900px" HeadButtons="Close" CloseText="Đóng">
        <HeaderTemplate>
            Phân quyền cho vai trò 
            <asp:Label runat="server" ID="lblFunctionTitle">  </asp:Label>
        </HeaderTemplate>
        <ContentTemplate>
            <SweetSoft1:UCRightInRole runat="server" ID="ucRightInRole" />
        </ContentTemplate>
        <FooterTemplate>
            <SweetSoft:ExtraButton ID="btnRightForRole" runat="server" EIcon="A_Save" EStyle="Success_Bold" EnableSnipButton="true" Text="Lưu" OnClick="btnRightForRole_Click"> </SweetSoft:ExtraButton>
        </FooterTemplate>
    </SweetSoft:ExtraDialog>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
    <script type="text/javascript">
        function BuildTableRole() {
            var table = $("table#tableRole");
            if (table.length > 0) {
                var checkbox = $("input[type='checkbox']", table);
                if (checkbox.length > 0) {
                    checkbox.click(function () {
                        var item = $(this);
                        var role = item.attr("role");
                        if (typeof (role) !== 'undefined' && role.length > 0) {
                            var checked = item.is(":checked");
                            var subs = $("." + role, table);
                            if (subs.length > 0) {
                                if (checked)
                                    subs.attr("checked", "checked").prop("checked", true);
                                else
                                    subs.removeAttr("checked").prop("checked", false);
                            }
                        }
                    });
                }

                var section = $("tr.row-section", table);
                if (section.length > 0) {
                    section.each(function () {
                        var item = $(this);
                        var role = item.attr("role");
                        if (typeof (role) !== 'undefined' && role.length > 0) {
                            var i = $("i", item);
                            var subs = $("tr.role-" + role, table);
                            i.click(function () {
                                if (i.length > 0 && subs.length > 0) {
                                    if (i.hasClass("fa-plus")) {
                                        subs.removeClass("hide");
                                        i.removeClass("fa-plus").addClass("fa-minus");
                                    }
                                    else {
                                        subs.addClass("hide");
                                        i.removeClass("fa-minus").addClass("fa-plus");
                                    }
                                }
                            });
                        }
                    });
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandName="exec_delete" Text="Xóa" EIcon="A_Delete" EStyle="Danger_Bold">
    </SweetSoft:ExtraButton>
</asp:Content>
