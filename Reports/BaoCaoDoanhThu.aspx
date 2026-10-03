<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BaoCaoDoanhThu.aspx.cs" Inherits="Pasteur.Laboratory.P2.Reports.BaoCaoDoanhThu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Printing.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Báo cáo doanh thu</h5>
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
                            <div class="col-lg-3 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Loại mẫu</span>
                                    <SweetSoft:ExtraComboBox ID="cbLoaiMau" runat="server" EmptyMessage="Chọn loại mẫu"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3">
                                <div class="form-group">
                                    <span class="extra-label">Quy chuẩn</span>
                                    <SweetSoft:ExtraComboBox ID="cbQuyChuan" ComboStyle="Warning" runat="server" EmptyMessage="Chọn quy chuẩn"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-3">
                                <div class="form-group">
                                    <span class="extra-label">Tình trạng thanh toán</span>
                                    <SweetSoft:ExtraComboBox ID="cbTinhTrangThanhToan" ComboStyle="Success" runat="server" EmptyMessage="Chọn tình trạng thanh toán">
                                        <asp:ListItem Value="" Text="Tất cả" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="ChuaLapPhieu" Text="Chưa in phiếu"></asp:ListItem>
                                        <asp:ListItem Value="DaLapPhieu" Text="Đã in phiếu"></asp:ListItem>
                                        <asp:ListItem Value="DTT" Text="Đã thanh toán"></asp:ListItem>
                                        <asp:ListItem Value="GhiNo" Text="Ghi nợ"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>

                        </div>
                        <div class="row" style="text-align: center;">
                            <SweetSoft:ExtraButton ID="btnSearch" runat="server" Text="Báo cáo" EStyle="Primary_Bold" EIcon="A_Filter" OnClick="btnSearch_Click"></SweetSoft:ExtraButton>
                            <SweetSoft:ExtraButton ID="btnSearchNgayThu" runat="server" Text="Báo cáo theo ngày thu" EStyle="Success_Bold" EIcon="A_Filter" CommandArgument="NgayThu" OnClick="btnSearch_Click"></SweetSoft:ExtraButton>
                            <SweetSoft:ExtraButton ID="btnExport" runat="server" Text="Xuất excel" EStyle="Warning_Bold" EIcon="A_Excel" OnClick="btnExport_Click"></SweetSoft:ExtraButton>
                        </div>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-12">
                                <table id="tblData" style="width: 100%; border-collapse: collapse;">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 30px;">STT</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Mã mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Ngày thu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Loại mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Quy chuẩn đánh giá</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Tình trạng</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 120px;">Tổng tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="repData" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Container.ItemIndex + 1 %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Eval("Mamau") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Eval("ThuTienNgayThuToString") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Eval("TenLoaiMau") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Eval("TenQuyChuan") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# GetTrangThai(Container.DataItem) %></td>
                                                    <td style="text-align: right; padding: 5px; border: solid 1px #000000;"><%# GetDecimalToString(Eval("TongTien") )%></td>
                                                    
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td style="border: solid 1px #000000; text-align: right; padding: 5px; font-weight: bold;" colspan="6">Tổng:</td>
                                            <td style="border: solid 1px #000000; text-align: right; font-weight: bold; padding: 5px;">
                                                <asp:Literal ID="ltrTongTien" runat="server"></asp:Literal>
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
