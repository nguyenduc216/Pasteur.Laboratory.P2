<%@ Page Title="Lịch sử thao tác" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DashboardLog.aspx.cs" Inherits="Pasteur.Laboratory.P2.DashboardLog" EnableSessionState="ReadOnly" %>
<%@ Register Src="~/Controls/Admin_UCLog.ascx" TagPrefix="SweetSoft" TagName="Log" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <%-- Code: DASHBOARD-PARALLEL-LOAD-20260918 - Endpoint chỉ đọc Session cho phép chạy song song với thống kê. --%>
    <style type="text/css">
        .horizontal-menu { display: none !important; }
        .page-body-wrapper { margin-top: 0 !important; }
        .content-wrapper { padding: 0 !important; }
        body { background: #fff !important; }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <SweetSoft:Log ID="ucLog" runat="server" />
</asp:Content>
