<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ThayMatKhau.aspx.cs" Inherits="Pasteur.Laboratory.P2.ThayMatKhau" %>

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
                    <h5 class="widget-title bigger lighter">Thông tin mật khẩu người dùng <b>

                        <div class="widget-toolbar no-border">
                            <SweetSoft:ExtraButton ID="btnSave" Transparent="true" runat="server" EIcon="A_Save" Text="Lưu dữ liệu" OnClick="btnSave_Click">
                            </SweetSoft:ExtraButton>
                        </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main ">
                        <div class="row">
                            <label class="col-lg-3 col-md-3 col-sm-12 control-label no-padding-right"
                                for="form-field-pass1">
                                Mật khẩu cũ
                            </label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <span class="input-icon input-icon-right">
                                    <SweetSoft:ExtraTextBox
                                        runat="server"
                                        ID="txtOldPass"
                                        ValidationGroup="info"
                                        TextMode="Password"
                                        CssClass="col-xs-12 col-sm-10  "
                                        PlaceHolder="Nhập mật khẩu cũ">
                                    </SweetSoft:ExtraTextBox>
                                    <i class="ace-icon fa fa-key fa-flip-horizontal"></i>
                                </span>
                            </div>
                        </div>


                        <div class="space-4"></div>
                        <div class="row">
                            <label class="col-md-3 col-lg-3 col-sm-12 control-label no-padding-right"
                                for="form-field-pass1">
                                Mật khẩu mới
                            </label>
                            <div class="col-md-9 col-lg-9 col-sm-12">
                                <span class="input-icon input-icon-right">
                                    <SweetSoft:ExtraTextBox
                                        runat="server"
                                        ID="txtNewPass"
                                        ValidationGroup="info"
                                        TextMode="Password"
                                        CssClass="col-xs-12 col-sm-10  "
                                        PlaceHolder="Nhập mật khẩu mới">
                                    </SweetSoft:ExtraTextBox>
                                    <i class="ace-icon fa fa-key fa-flip-horizontal"></i>
                                </span>
                            </div>
                        </div>


                        <div class="space-4"></div>

                        <div class="row">
                            <label class="col-lg-3 col-md-3 col-sm-12 control-label no-padding-right"
                                for="form-field-pass2">
                                Nhập lại mật khẩu
                            </label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <span class="input-icon input-icon-right">
                                    <SweetSoft:ExtraTextBox
                                        runat="server"
                                        ID="txtConfirmPass"
                                        ValidationGroup="info"
                                        TextMode="Password"
                                        CssClass="col-xs-12 col-sm-10  "
                                        PlaceHolder="Nhập lại mật khẩu mới">
                                    </SweetSoft:ExtraTextBox>
                                    <i class="ace-icon fa fa-key fa-flip-horizontal"></i>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
