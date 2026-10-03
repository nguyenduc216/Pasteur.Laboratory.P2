<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Admin_UCLog.ascx.cs"
    Inherits="Pasteur.Laboratory.P2.Controls.Admin_UCLog" %>
<style>
    .control-padding-bottom {
        padding-bottom: 10px;
    }

    .control-padding-top {
        padding-top: 10px;
    }

    .control-padding-left {
        padding-left: 10px;
    }

    .control-padding-right {
        padding-right: 10px;
    }

    .transparent-button {
        background-color: transparent !important;
        border: transparent;
    }
</style>

<div class="row">
    <div class="col-lg-12">
        <div class="widget-box">
            <div class="widget-header">
                <h4 class="widget-title lighter smaller">
                    <i class='ace-icon fa fa-history'></i>
                    Lịch sử thao tác
                </h4>
                <div class="widget-toolbar no-border">
                    <a href="#"
                        data-action="collapse">
                        <i class="ace-icon fa fa-chevron-down"></i>
                    </a>
                </div>
            </div>
            <div class="widget-body">
                <div class="widget-main">
                    <%-- Code: DASHBOARD-PARALLEL-LOAD-20260918 - Vùng hiển thị lỗi truy vấn log. --%>
                    <asp:Literal ID="ltrLoadError" runat="server"></asp:Literal>
                    <div id="divSearchLog"
                        class="control-padding-bottom control-padding-left control-padding-right control-padding-top">
                        <asp:Panel runat="server"
                            ID="pnlSearchLog"
                            DefaultButton="btnSearchLog">
                            <div class="input-group">
                                <SweetSoft:ExtraTextBox
                                    runat="server"
                                    ID="txtSearchLog"
                                    CssClass="form-control"
                                    PlaceHolder="Nhập từ khóa tìm kiếm log">
                                </SweetSoft:ExtraTextBox>
                                <span class="input-group-btn">
                                    <SweetSoft:ExtraButton
                                        runat="server"
                                        ID="btnSearchLog"
                                        OnClick="btnSearchLog_Click"
                                        EStyle="Success"
                                        Width="35"
                                        EIcon="A_Search">
                                    </SweetSoft:ExtraButton>
                                    <SweetSoft:ExtraButton
                                        runat="server"
                                        ID="btnCancelSearch"
                                        OnClick="btnCancelSearch_Click"
                                        EStyle="Danger"
                                        Visible="false"
                                        Width="35"
                                        EIcon="A_Close">
                                    </SweetSoft:ExtraButton>
                                </span>
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="timeline-container control-padding-right">
                        <div class="timeline-items">
                            <asp:Repeater ID="rptLog"
                                runat="server">
                                <ItemTemplate>
                                    <div class="timeline-item clearfix">
                                        <div class="timeline-info">
                                            <img src="../images/logo.png" width="30" />
                                        </div>
                                        <div class="widget-box transparent collapsed">
                                            <div class="widget-header widget-header-small">
                                                <h5 class="widget-title smaller">
                                                    <b>
                                                        <a href="#"><%# Eval("UserName") %></a>
                                                    </b>
                                                    <span>
                                                        <%#Eval("LogTitle") %>
                                                    </span>
                                                </h5>
                                                <span class="widget-toolbar no-border">
                                                    <i class="ace-icon fa fa-clock-o bigger-110"></i>
                                                    <span>
                                                        <%#Eval("CreatedDateToString") %>
                                                    </span>
                                                </span>
                                                <span class="widget-toolbar">
                                                    <a href="#" data-action="collapse">
                                                        <i class="ace-icon fa fa-chevron-down"></i>
                                                    </a>
                                                </span>
                                            </div>
                                            <div class="widget-body">
                                                <div class="widget-main">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <%#GetLogContent(Eval("LogContent")) %>
                                                            </td>
                                                            <td>
                                                                <div class="pull-right action-buttons">
                                                                    <SweetSoft:ExtraButton
                                                                        runat="server"
                                                                        ID="btnDeleteLog"
                                                                        EIcon="A_Close"
                                                                        OnClick="btnDeleteLog_Click"
                                                                        CommandArgument='<%#Eval("ID") %>'
                                                                        Visible="false"
                                                                        CssClass="btn-xs">
                                                                    </SweetSoft:ExtraButton>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class="text-right control-padding-bottom control-padding-right">
                        <SweetSoft:ExtraButton
                            runat="server"
                            ID="btnViewPrevious"
                            EStyle="Success"
                            OnClick="btnViewPrevious_Click">
                            <i class="icon-on-right ace-icon fa fa-arrow-left"></i>
                            <span>Trước</span>
                        </SweetSoft:ExtraButton>

                        <SweetSoft:ExtraButton
                            runat="server"
                            ID="btnViewMore"
                            EStyle="Success"
                            OnClick="btnViewMore_Click">
                            <span>Sau</span>
                            <i class="icon-on-right ace-icon fa fa-arrow-right"></i>
                        </SweetSoft:ExtraButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

