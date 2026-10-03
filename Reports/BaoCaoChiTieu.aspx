<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BaoCaoChiTieu.aspx.cs" Inherits="Pasteur.Laboratory.P2.Reports.BaoCaoChiTieu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Printing.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Báo cáo theo chỉ tiêu</h5>
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
                                    <span class="extra-label">Hóa lý / vi sinh</span>
                                    <SweetSoft:ExtraComboBox ID="cbHoaSinh" runat="server" EmptyMessage="Tất cả">
                                        <asp:ListItem Value="" Text="Tất cả" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="Hoa" Text="Hóa lý"></asp:ListItem>
                                        <asp:ListItem Value="VS" Text="Vi Sinh"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Kết quả đánh giá</span>
                                    <SweetSoft:ExtraComboBox ID="cbTinhTrangDuyet" runat="server" EmptyMessage="Tất cả">
                                        <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Đã đánh giá" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Labo Hóa đã duyệt"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="labo Vi sinh đã duyệt"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Chấp nhận chất lượng"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Mẫu kiểm tra lại"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Chưa đánh giá"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-4 col-xs-12" style="padding-top: 20px;">
                                <SweetSoft:ExtraButton ID="btnSearch" runat="server" Text="Báo cáo" EStyle="Primary_Bold" EIcon="A_Filter" OnClick="btnSearch_Click"></SweetSoft:ExtraButton>
                                <SweetSoft:ExtraButton ID="btnExport" runat="server" Text="Xuất excel" EStyle="Warning_Bold" EIcon="A_Excel" OnClick="btnExport_Click"></SweetSoft:ExtraButton>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-12">
                                <table id="tblData" style="width: 100%; border-collapse: collapse;">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px;">Bộ phận</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px;">Phân loại</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px;">Tổng số mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px;">Chỉ tiêu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px;">Tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Literal ID="ltrBody" runat="server"></asp:Literal>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td colspan="4" style="text-align: right; font-weight: bold; padding-right: 10px; text-transform: uppercase; border: solid 1px #000000;">Tổng cộng tiền</td>
                                            <td style="text-align: right; font-weight: bold; border: solid 1px #000000; padding-right: 10px;">
                                                <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
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
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
</asp:Content>
