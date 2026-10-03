<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="403.aspx.cs" Inherits="Pasteur.Laboratory.P2.Page403" %>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row">
        <div class="col-xs-12">
            <!-- PAGE CONTENT BEGINS -->

            <div class="error-container">
                <div class="well">
                    <h1 class="grey lighter smaller">
                        <span class="red bigger-125">
                            <i class="ace-icon fa fa-times-circle"></i>
                            403
                        </span>
                        Không có quyền truy cập chức năng <b>
                            <asp:Literal ID="ltrFunction" runat="server"></asp:Literal></b>
                    </h1>

                    <hr>
                    <h3 class="lighter smaller">Xin lỗi, bạn không có quyền truy cập chức năng này!
                    </h3>

                    <div class="space"></div>

                    <div>
                        <h4 class="lighter smaller">Trong lúc này, vui lòng làm theo  bước sau:</h4>

                        <ul class="list-unstyled spaced inline bigger-110 margin-15">
                           
                            <li>
                                <i class="ace-icon fa fa-hand-o-right blue"></i>
                                Liên hệ với quản trị viên để được cấp quyền truy cập chức năng.
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

                        <a class="btn btn-primary" href="/dashboard">
                            <i class="ace-icon fa fa-tachometer"></i>
                            Trang điều khiển
                        </a>
                    </div>
                </div>
            </div>

            <!-- PAGE CONTENT ENDS -->
        </div>
        <!-- /.col -->
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
