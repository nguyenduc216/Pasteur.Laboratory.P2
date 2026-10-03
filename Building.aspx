<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Building.aspx.cs" Inherits="Pasteur.Laboratory.P2.Building" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="error-container">
        <div class="well">
            <h1 class="grey lighter smaller">
                <span class="blue bigger-125">
                    <i class="ace-icon fa fa-sitemap"></i>
                    404
                </span>
                Chức năng đang được xây dựng <b>
                    <asp:Literal ID="ltrPage" runat="server"></asp:Literal></b>
            </h1>

            <h3 class="lighter smaller">Xin lỗi, chúng tôi đang xây dựng chức năng này!</h3>

            <div>

                <div class="space"></div>
                <h4 class="smaller">Trong lúc này, xin vui lòng :</h4>

                <ul class="list-unstyled spaced inline bigger-110 margin-15">
                    <li>
                        <i class="ace-icon fa fa-hand-o-right blue"></i>
                        Kiểm tra lại chính xác đường link của bạn
                    </li>

                
                </ul>
            </div>

            <hr>
            <div class="space"></div>

            <div class="center">
                <a class="btn btn-grey" href="javascript:history.back()">
                    <i class="ace-icon fa fa-arrow-left"></i>
                    Quay trở lại
                </a>

                <a class="btn btn-info" href="#">
                    <i class="ace-icon fa fa-tachometer"></i>
                    Trang chủ
                </a>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
</asp:Content>
