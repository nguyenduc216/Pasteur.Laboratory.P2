<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Common_Paging.ascx.cs" Inherits="Pasteur.Laboratory.P2.Controls.Common_Paging" %>

<div class="col-md-8 col-sm-8 text-left" style="padding-right: 5px;" id="divPaging" runat="server">
    <div style="float: left; margin-right: 10px;">
        <ul class="pagination" style="margin: 0">
            <li id="liFirstPage" runat="server">
                <asp:LinkButton ID="btnFirst" runat="server" OnClick="lnkPage_Click" CommandArgument="first" ToolTip="First" data-toggle="tooltip" data-placement="top">
                    <%# GetButtonText("first") %>
                </asp:LinkButton>
            </li>
            <li id="liPreviousPage" runat="server">
                <asp:LinkButton ID="btnPrevious" runat="server" OnClick="lnkPage_Click" CommandArgument="previous" ToolTip="Previous" data-toggle="tooltip" data-placement="top">
                    <%# GetButtonText("previous") %>
                </asp:LinkButton>
            </li>
            <asp:Repeater ID="rptPager" runat="server">
                <ItemTemplate>
                    <li<%# Eval("CssClass") %>>
                       <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>'
                            CommandArgument='<%# Eval("Value") %>'
                            OnClick="lnkPage_Click"> </asp:LinkButton>
                    </li>
               
                </ItemTemplate>
            </asp:Repeater>
            <li id="liNextPage" runat="server">
                <asp:LinkButton ID="btnNext" runat="server" OnClick="lnkPage_Click" CommandArgument="next" ToolTip="Next" data-toggle="tooltip" data-placement="top">
                     <%# GetButtonText("next") %>
                </asp:LinkButton>
            </li>
            <li id="liLastPage" runat="server">
                <asp:LinkButton ID="btnLast" runat="server" OnClick="lnkPage_Click" CommandArgument="last" ToolTip="Last" data-toggle="tooltip" data-placement="top">
                     <%# GetButtonText("last") %>
                </asp:LinkButton>
            </li>
        </ul>
    </div>
    <div class="pull-left">
        <SweetSoft:ExtraComboBox ID="ddlPageSize" ComboStyle="Info" CssClass="form-control grid-paging" data-plugin-select2="1" data-search="-1"
            OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="70px">
            <asp:ListItem Value="10" Text="10" />
            <asp:ListItem Value="15" Text="15" />
            <asp:ListItem Value="30" Text="30" />
            <asp:ListItem Value="50" Text="50" />
            <asp:ListItem Value="100" Text="100" />
            <asp:ListItem Value="200" Text="200" />
        </SweetSoft:ExtraComboBox>
        <span>
            <asp:Literal ID="ltrPage" runat="server"></asp:Literal>
        </span>
    </div>
</div>
<div class="col-md-4 col-sm-4 text-right" style="padding-left: 0px; padding-top: 10px;">
    <asp:Literal ID="ltrMessage" runat="server"></asp:Literal>
</div>


