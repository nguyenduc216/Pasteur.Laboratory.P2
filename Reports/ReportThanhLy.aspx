<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ReportThanhLy.aspx.cs" Inherits="Pasteur.Laboratory.P2.Reports.ReportThanhLy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Printing.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Báo cáo mẫu thanh lý</h5>
                    <div class=" widget-toolbar filter no-border">
                        <a class="white" data-action="fullscreen" href="#"><i class="ace-icon fa fa-expand"></i></a>
                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main">
                        <div class="row">
                            <div class="col-lg-2 col-xs-3">
                                <div class="form-group">
                                    <span class="extra-label">Ngày nhận KQ từ ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateFrom" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn từ ngày" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">tới ngày</span>
                                    <SweetSoft:ExtraDateTimePicker ID="dateTo" runat="server" DateTimeMask="Date" Width="100%" PlaceHolder="Chọn tới ngày" />
                                </div>
                            </div>
                            <div class="col-lg-4 col-xs-4">
                                <div class="form-group">
                                    <span class="extra-label">Mã mẫu</span>
                                    <SweetSoft:ExtraMathTextbox ID="txtMTenChiTieu" runat="server" PlaceHolder="Nhập mã mẫu" />
                                    <SweetSoft:ExtraTextBox ID="txtTenChiTieu" Visible="false" runat="server" PlaceHolder="Nhập chỉ tiêu cần tìm"></SweetSoft:ExtraTextBox>
                                    <asp:HiddenField ID="hiddenID" runat="server" />
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Tình trạng</span>
                                    <SweetSoft:ExtraComboBox ID="cbTinhTrang" runat="server" EmptyMessage="Chọn tình trạng">
                                        <asp:ListItem Value="" Text="Tất cả" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đã thanh lý"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Chưa thanh lý"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Hóa lý / vi sinh</span>
                                    <SweetSoft:ExtraComboBox ID="cbHoaSinh" runat="server" EmptyMessage="Tất cả">
                                        <asp:ListItem Value="Hoa" Text="Hóa lý"></asp:ListItem>
                                        <asp:ListItem Value="VS" Text="Vi Sinh"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-12" style="padding-top: 20px;">
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
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px;">Mã mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Ngày nhận mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Ngày trả kết quả</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Ngày thanh lý dự kiến</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Ngày thanh lý thực tế</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="repData" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Container.ItemIndex + 1 %></td>
                                                    <td style="text-align: left; padding: 5px; border: solid 1px #000000;"><%# Eval("MaMau") %></td>
                                                    <td style="text-align: left; padding: 5px; border: solid 1px #000000;"><%# Eval("NgayNhanMauToString") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Eval("NgayTraKetQuaThucTeToString") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Eval("NgayThanhLyDuKienToString") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# GetNgayThanhLyThucTe(Container.DataItem) %></td>
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
