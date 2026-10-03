<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BaoCaoDSMauTraKQ.aspx.cs" Inherits="Pasteur.Laboratory.P2.Reports.BaoCaoDSMauTraKQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Printing.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Danh sách mãu cho bộ phận trả kết quả</h5>
                    <div class=" widget-toolbar filter no-border">
                        <a class="white" data-action="fullscreen" href="#"><i class="ace-icon fa fa-expand"></i></a>
                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main">
                        <div class="row">
                            <div class="col-lg-4 col-xs-12">
                                <div class="form-group">
                                    <span class="extra-label">Từ ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateFrom" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn từ ngày" />
                                </div>
                            </div>
                            <div class="col-lg-4 col-xs-12">
                                <div class="form-group">
                                    <span class="extra-label">tới ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateTo" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn tới ngày" />
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
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 30px;">STT</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 120px;">Tên mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 70px;">Mã mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Quy chuẩn</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Khách hàng</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 70px;">Số chỉ tiêu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 100px;">Trả KQ</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 100px;">Ngày trả</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 100px;">Tổng tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="repData" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: center; border: solid 1px #000000; padding: 5px;"><%# Container.ItemIndex +1 %></td>
                                                    <td style="text-align: left; border: solid 1px #000000; padding: 5px;"><%# Eval("TenLoaiMau") %></td>
                                                    <td style="text-align: center; border: solid 1px #000000; padding: 5px;"><%# Eval("MaMau") %></td>
                                                    <td style="text-align: left; border: solid 1px #000000; padding: 5px;"><%# Eval("TenQuyChuan") %></td>
                                                    <td style="text-align: center; border: solid 1px #000000; padding: 5px;"><%# Eval("SoChiTieu") %></td>
                                                    <td style="text-align: center; border: solid 1px #000000; padding: 5px;"><%# Eval("NgayKetQua") %></td>
                                                    <td style="text-align: center; border: solid 1px #000000; padding: 5px;"><%# Eval("NgayKetQua") %></td>
                                                    <td style="text-align: center; border: solid 1px #000000; padding: 5px;"><%# Eval("TongTien") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
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
