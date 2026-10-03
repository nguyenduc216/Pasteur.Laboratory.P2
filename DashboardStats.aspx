<%@ Page Title="Thống kê chung" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="DashboardStats.aspx.cs" Inherits="Pasteur.Laboratory.P2.DashboardStats" EnableSessionState="ReadOnly" %>
<%@ Register Src="~/Controls/QuickReview.ascx" TagPrefix="SweetSoft" TagName="QuickReview" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    <%-- Code: DASHBOARD-PARALLEL-LOAD-20260918 - Endpoint chỉ đọc Session cho phép chạy song song với log. --%>
    <style type="text/css">
        .horizontal-menu { display: none !important; }
        .page-body-wrapper { margin-top: 0 !important; }
        .content-wrapper { padding: 0 !important; }
        body { background: #fff !important; }
    </style>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <SweetSoft:QuickReview ID="quickReview" runat="server" />
</asp:Content>
