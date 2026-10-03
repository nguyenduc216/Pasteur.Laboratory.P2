<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BaoCaoTongHopKetQua.aspx.cs" Inherits="Pasteur.Laboratory.P2.Reports.BaoCaoTongHopKetQua" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Printing.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Báo cáo tổng hợp kết quả</h5>
                    <div class=" widget-toolbar filter no-border">
                        <a class="white" data-action="fullscreen" href="#"><i class="ace-icon fa fa-expand"></i></a>
                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main">
                        <div class="row">
                           <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Lấy mẫu từ ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateFrom" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn từ ngày" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">tới ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateTo" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn tới ngày" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Labo Hóa duyệt nội bộ từ ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateDuyetNoiBoTu" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn từ ngày" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">tới ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateDuyetNoiBoToi" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn tới ngày" />
                                </div>
                            </div>
                             <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Labo VS duyệt nội bộ từ ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateVSDuyetNoiBoTu" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn từ ngày" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">tới ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateVSDuyetNoiBoToi" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn tới ngày" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Mã mẫu</span>
                                    <SweetSoft:ExtraTextBox ID="txtMamMau" runat="server" PlaceHolder="Nhập mã mẫu"></SweetSoft:ExtraTextBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3">
                                <div class="form-group">
                                    <span class="extra-label">
                                        <SweetSoft:ExtraCheckBox ID="chkCheckAllThongTin" AutoPostBack="true" runat="server" OnCheckedChanged="chkCheckAllThongTin_CheckedChanged" DataOn="Tất cả" DataOff="Xóa"></SweetSoft:ExtraCheckBox>
                                        <asp:Literal ID="ltrThongTinHienThi" Text="Thông tin hiển thị" runat="server"></asp:Literal>
                                    </span>
                                    <SweetSoft:ExtraComboBox ID="cbThongTinHienThi" EmptyMessage="Chọn thông tin hiển thị" DataTextFormatString="" runat="server" SelectionMode="Multiple" ShowTick="true"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3">
                                <div class="form-group">
                                    <span class="extra-label">
                                        <SweetSoft:ExtraCheckBox ID="chkCheckAllXetNghiem" AutoPostBack="true" runat="server" OnCheckedChanged="chkCheckAllXetNghiem_CheckedChanged" DataOn="Tất cả" DataOff="Xóa"></SweetSoft:ExtraCheckBox>
                                        <asp:Literal ID="ltrXetNghiem" Text="Danh sách xét nghiệm" runat="server"></asp:Literal>
                                        <asp:LinkButton ID="lnkGetDSXetNghiem" runat="server" Text=" [ Lấy danh sách ]" OnClick="lnkGetDSXetNghiem_Click"></asp:LinkButton>
                                    </span>
                                    <SweetSoft:ExtraComboBox ID="cbDanhSachXetNghiem" runat="server" EmptyMessage="[ Lấy danh sách] để nhận dữ liệu &#9757;" SelectionMode="Multiple" ShowTick="true"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row" >
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Tình trạng trả kết quả
                                    </span>
                                    <SweetSoft:ExtraComboBox ID="cbTraKQ" EmptyMessage="Tất cả" DataTextFormatString="" runat="server">
                                        <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đã trả kết quả"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Chưa trả kết quả"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-10 col-xs-10" style="margin-top: 25px;">
                                <SweetSoft:ExtraButton ID="btnExport" runat="server" Text="Xem báo cáo" OnClick="btnExport_Click" EStyle="Success_Bold" EIcon="A_Eye"></SweetSoft:ExtraButton>
                                <SweetSoft:ExtraButton ID="btnExcel" runat="server" Text="Xuất báo cáo" OnClick="btnExcel_Click" EStyle="Warning_Bold" EIcon="A_Excel"></SweetSoft:ExtraButton>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-12">
                                <asp:Literal ID="ltrData" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ScriptInlinePage" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
</asp:Content>
