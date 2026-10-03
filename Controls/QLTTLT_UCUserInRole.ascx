<%@ Control Language="C#" AutoEventWireup="true"
    CodeBehind="QLTTLT_UCUserInRole.ascx.cs"
    Inherits="Pasteur.Laboratory.P2.Controls.QLTTLT_UCUserInRole" %>

<SweetSoft:ExtraGrid
    ID="grvRole"
    runat="server"
    AutoGenerateColumns="false"
    AllowSorting="false"
    DataKeyNames="RightID"
    AllowPaging="false"
    PagerSettings-Position="Bottom"
    CssClass="table dataTable table-bordered table-striped table-hover"
    RowStyle-Height="42px"
    RowStyle-CssClass="td-middle" ShowHeaderWhenEmpty="true"
    IsResponsive="true">
    <EmptyDataTemplate>
        Hệ thống không trích lọc được dữ liệu, vui lòng thử lại với tiêu chí tìm kiếm khác!
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField
            HeaderStyle-HorizontalAlign="Center"
            ItemStyle-HorizontalAlign="Center"
            HeaderStyle-Width="7%"
            ItemStyle-Width="7%">
            <HeaderTemplate>
                <SweetSoft:ExtraCheckBox
                    ID="chkAll"
                    runat="server">
                </SweetSoft:ExtraCheckBox>
            </HeaderTemplate>
            <ItemTemplate>
                <SweetSoft:ExtraCheckBox
                    ID="chkItem"
                    runat="server"
                    Checked='<%# Eval("RightInRoleID") != null %>'
                    ValidationGroup="cbRole">
                </SweetSoft:ExtraCheckBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField
            HeaderStyle-HorizontalAlign="Center"
            HeaderStyle-Width="30%"
            ItemStyle-Width="30%"
            HeaderText="Tên vai trò">
            <ItemTemplate>
                <%# Eval("RightName") %>
                <asp:HiddenField
                    ID="hdfRoleID"
                    runat="server"
                    Value='<%# Eval("RightID") %>' />
                <asp:HiddenField
                    ID="hdfUserInRoleID"
                    runat="server"
                    Value='<%# Eval("RightInRoleID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField
            HeaderStyle-HorizontalAlign="Center"
            HeaderText="Miêu tả vai trò">
            <ItemTemplate>
                <%# Eval("RightDescription") %>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</SweetSoft:ExtraGrid>

