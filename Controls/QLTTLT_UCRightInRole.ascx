<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QLTTLT_UCRightInRole.ascx.cs"
    Inherits="Pasteur.Laboratory.P2.Controls.QLTTLT_UCRightInRole" %>

<asp:UpdatePanel ID="udpRights" runat="server">
    <ContentTemplate>
        <asp:Repeater
            ID="rptFunction"
            runat="server"
            OnItemCreated="rptFunction_ItemCreated">
            <HeaderTemplate>
                <table id="tableRole"
                    class="table dataTable table-bordered table-hover">
                    <thead>
                        <th style="width: 30px;">
                            <SweetSoft:ExtraCheckBox
                                ID="chkAll"
                                runat="server"
                                role="check-all" EnableClickGroup="true" GroupName="All_Role">
                            </SweetSoft:ExtraCheckBox>
                        </th>
                        <th style="width: 30px"></th>
                        <th style="width: 300px">Quyền</th>
                        <th>Miêu tả quyền</th>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Literal
                    ID="ltrRowOpen"
                    runat="server">
                </asp:Literal>
                <td style="text-align: center;">
                    <SweetSoft:ExtraCheckBox
                        ID="chkFunction"
                        runat="server" CssClass="All_Role">
                    </SweetSoft:ExtraCheckBox>
                </td>
                <td colspan="3">
                    <i class="fa fa-plus"></i>
                    <asp:Label
                        ID="lbFunctionName"
                        runat="server">
                    </asp:Label>
                </td>
                </tr>
                <asp:Repeater
                    ID="rptRight"
                    runat="server"
                    OnItemCreated="rptRight_ItemCreated">
                    <ItemTemplate>
                        <asp:Literal
                            ID="ltrRowOpen"
                            runat="server">
                </asp:Literal>
                        <td></td>
                        <td style="text-align: center;">
                            <SweetSoft:ExtraCheckBox
                                ID="chkItem"
                                runat="server" CssClass="All_Role">
                            </SweetSoft:ExtraCheckBox>
                        </td>
                        <td>
                            <asp:Label
                                ID="lbRightName"
                                runat="server">
                    </asp:Label>
                            <asp:HiddenField
                                ID="hdfRightInRoleID"
                                runat="server" />
                            <asp:HiddenField
                                ID="hdfRightID"
                                runat="server" />
                        </td>
                        <td>
                            <asp:Label
                                ID="lbDescription"
                                runat="server">
                    </asp:Label>
                        </td>
                        </tr>
           
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
                    </table>
           
            </FooterTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
