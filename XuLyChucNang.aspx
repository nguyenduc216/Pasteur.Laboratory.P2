<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="XuLyChucNang.aspx.cs" Inherits="Pasteur.Laboratory.P2.XuLyChucNang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <SweetSoft:ExtraButton ID="btnXuLyMaNoiBo" runat="server" Text="Xử lý mã nội bộ" EIcon="A_Check" OnClick="btnXuLyMaNoiBo_Click"></SweetSoft:ExtraButton><br />
    <SweetSoft:ExtraButton ID="btnXuLyDonViTinh" runat="server" Text="Xử lý đơn vị tính chỉ tiêu" EIcon="A_Check" OnClick="btnXuLyDonViTinh_Click"></SweetSoft:ExtraButton>
    <span style="color:red;">
    <asp:Literal ID="ltrMes" runat="server"></asp:Literal></span>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
</asp:Content>
