<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="TKLuongMauTheoChiTieuQuyChuan.aspx.cs" Inherits="Pasteur.Laboratory.P2.Reports.TKLuongMauTheoChiTieuQuyChuan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/css/Printing.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Báo cáo theo chỉ tiêu và quy chuẩn</h5>
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
                            <div class="col-lg-5 col-xs-5">
                                <div class="form-group">
                                    <span class="extra-label">Chỉ tiêu</span>

                                    <SweetSoft:ExtraMathTextbox ID="txtMTenChiTieu" ShowKeyboard="false" runat="server" Visible="true" PlaceHolder="Nhập chỉn tiêu cần tìm 2" />
                                    <SweetSoft:ExtraTextBox ID="txtTenChiTieu" Visible="false" runat="server" PlaceHolder="Nhập chỉ tiêu cần tìm"></SweetSoft:ExtraTextBox>
                                    <asp:HiddenField ID="hiddenID" runat="server" />
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3">
                                <div class="form-group">
                                    <span class="extra-label">Quy chuẩn</span>
                                    <SweetSoft:ExtraComboBox ID="cbQuyChuan" ComboStyle="Warning" runat="server" EmptyMessage="Chọn quy chuẩn"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>

                        </div>
                        <div class="row">
                            <div class="col-lg-2 col-xs-2">
                                <div class="form-group">
                                    <span class="extra-label">Loại mẫu</span>
                                    <SweetSoft:ExtraComboBox ID="cbLoaiMau" runat="server" EmptyMessage="Chọn loại mẫu"></SweetSoft:ExtraComboBox>
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
                            <div class="col-lg-3 col-xs-3" style="display: none;">
                                <div class="form-group">
                                    <span class="extra-label">KQ Đánh giá</span>
                                    <SweetSoft:ExtraComboBox ID="cbKQDG" ComboStyle="Success" runat="server" EmptyMessage="Chọn kết quả đánh giá">
                                        <asp:ListItem Value="" Text="Tất cả"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Đã đánh giá"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="Chưa đánh giá"></asp:ListItem>
                                    </SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-xs-3" style="display: none;">
                                <div class="form-group">
                                    <span class="extra-label">Nơi nhận mẫu</span>
                                    <SweetSoft:ExtraTextBox ID="txtSearchNoiNhanMau" runat="server" PlaceHolder="Nhập nơi nhận mẫu"></SweetSoft:ExtraTextBox>
                                </div>
                            </div>
                            <div class="col-lg-2 col-xs-12">
                                <div class="form-group">
                                    <span class="extra-label">Phạm vi lấy mẫu</span>
                                    <SweetSoft:ExtraComboBox ID="cbNoiLayMauGoiY" runat="server">
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
                            <div class="col-lg-3 col-xs-3" style="margin-top: 22px;">
                                <SweetSoft:ExtraButton ID="btnSearch" runat="server" Text="Báo cáo" EStyle="Primary_Bold" EIcon="A_Filter" OnClick="btnSearch_Click"></SweetSoft:ExtraButton>
                                <SweetSoft:ExtraButton ID="btnExport" runat="server" Text="Xuất excel" EStyle="Warning_Bold" EIcon="A_Excel" OnClick="btnExport_Click"></SweetSoft:ExtraButton>
                            </div>
                        </div>
                        <div class="row" style="text-align: center;">
                        </div>
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-lg-12">
                                <table id="tblData" style="width: 100%; border-collapse: collapse;">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 30px;">STT</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 150px;">Tên chỉ tiêu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 120px;">Quy chuẩn đánh giá</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 120px;">Loại mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 220px;">Phạm vi lấy mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 50px;">Tổng mẫu</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 50px;">Đạt</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 50px;">Không đạt</th>
                                            <th style="text-align: center; font-weight: bold; text-transform: uppercase; border: solid 1px #000000; padding: 5px; width: 50px;">Không đánh giá</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="repData" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# Container.ItemIndex + 1 %></td>
                                                    <td style="text-align: left; padding: 5px; border: solid 1px #000000;"><%# Eval("TenTaiCacKhoa") %></td>
                                                    <td style="text-align: left; padding: 5px; border: solid 1px #000000;"><%# Eval("TenQuyChuan") %></td>
                                                    <td style="text-align: left; padding: 5px; border: solid 1px #000000;"><%# Eval("TenLoaiMau") %></td>
                                                    <td style="text-align: left; padding: 5px; border: solid 1px #000000;"><%# GeDiaChi2(Eval("IDChiTieu") ) %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# TinhTong(Container.DataItem,"tm") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# TinhTong(Container.DataItem,"1") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# TinhTong(Container.DataItem,"2") %></td>
                                                    <td style="text-align: center; padding: 5px; border: solid 1px #000000;"><%# TinhTong(Container.DataItem,"3") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                    <tfoot>
                                        <tr>
                                            <td style="border: solid 1px #000000; text-align: right; padding: 5px; font-weight: bold;" colspan="5">Tổng:</td>
                                            <td style="border: solid 1px #000000; text-align: center; font-weight: bold; padding: 5px;">
                                                <asp:Literal ID="ltrTongMau" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: solid 1px #000000; text-align: center; font-weight: bold; padding: 5px;">
                                                <asp:Literal ID="ltrTongMauDat" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: solid 1px #000000; text-align: center; font-weight: bold; padding: 5px;">
                                                <asp:Literal ID="ltrTongMauKhongDat" runat="server"></asp:Literal>
                                            </td>
                                            <td style="border: solid 1px #000000; text-align: center; font-weight: bold; padding: 5px;">
                                                <asp:Literal ID="ltrTongKhongDanhGia" runat="server"></asp:Literal>
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
    <script src="/scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(function () {
            SearchFullText();
            addRequestHanlde(SearchFullText);

        });
        function SearchFullText(s, a) {

            if ($("#math_content_ctl00_ContentPlaceHolder1_txtMTenChiTieu").length > 0) {
                $("#math_content_ctl00_ContentPlaceHolder1_txtMTenChiTieu").on("focus", function () {
                    $(this).select();
                });
                var txtAddTenChiTieu = $("#math_content_ctl00_ContentPlaceHolder1_txtMTenChiTieu");
                if (txtAddTenChiTieu.length > 0) {
                    txtAddTenChiTieu.change(function () {
                        var l = $(this).val().length;
                        if (l === 0) $("input[type='hidden'][id$='hiddenID']").val("");
                    });
                    txtAddTenChiTieu.autocomplete({
                        source: function (request, response) {
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "/Default.aspx/SearchChiTieuByKeyWord",
                                data: "{'key':'" + $("#math_content_ctl00_ContentPlaceHolder1_txtMTenChiTieu").html() + "'}",
                                dataType: "json",
                                async: true,
                                cache: false,
                                success: function (result) {
                                    console.log("result: ", result);
                                    response($.map(result.d.List, function (item) {
                                        return {
                                            TenTaiCacKhoa: item.TenTaiCacKhoa,
                                            ID: item.Id
                                        };
                                    }));
                                },
                                error: function (msg) {

                                }
                            });
                        },
                        messages: {
                            noResults: '',
                            results: function () { }
                        },
                        focus: function (event, ui) {
                            return false;
                        },
                        select: function (event, ui) {
                            $("#math_content_ctl00_ContentPlaceHolder1_txtMTenChiTieu").html(ui.item.TenTaiCacKhoa);
                            $("input[type='hidden'][id$='hiddenID']").val(ui.item.ID);

                            return false;
                        }
                    }).data("ui-autocomplete")._renderItem = function (ul, item) {

                        return $("<li>")
                            .data("ui-autocomplete-item", item)
                            .append("<span> " + item.TenTaiCacKhoa + "</span>")
                            .appendTo(ul);

                    };
                }
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
</asp:Content>
