<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ThanhLy.ascx.cs" Inherits="Pasteur.Laboratory.P2.Controls.ThanhLy" %>
<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<asp:HiddenField ID="hiddenEditID" runat="server" />
<asp:HiddenField ID="hiddenChiTietID" runat="server" />
<asp:HiddenField ID="hiddenPageType" runat="server" />
<asp:HiddenField ID="hiddenMauID" runat="server" />
<div class="row">
    <div class="col-lg-3 col-md-3 col-xs-12">
        <div class="form-group">
            <label class="extra-label">
                Ngày thực hiện
            </label>
            <SweetSoft:ExtraDateTimePicker ID="dateNgayThucHien" runat="server" Width="100%" PlaceHolder="Chọn ngày" />
        </div>
    </div>
    <div class="col-lg-3 col-md-3 col-xs-12">
        <div class="form-group">
            <label class="extra-label">
                Ngày kết thúc
            </label>
            <SweetSoft:ExtraDateTimePicker ID="dateNgayKetThuc" runat="server" Width="100%" PlaceHolder="Chọn ngày" />
        </div>
    </div>
    <div class="col-lg-2 col-md-2 col-xs-12">
        <div class="form-group">
            <label class="extra-label">
                Mã số mẫu
            </label>
            <SweetSoft:ExtraTextBox ID="txtSearchMaMau" runat="server" PlaceHolder="Nhập mã phiếu"></SweetSoft:ExtraTextBox>
        </div>
    </div>
    <div class="col-lg-4 col-md-4 col-xs-12">
        <div class="btn-group dropup" style="margin-top: 25px;">
            <SweetSoft:ExtraButton ID="btnThem" runat="server" EStyle="Primary" EIcon="A_Add" Text="Thêm mẫu vào danh sách" OnClick="btnThem_Click"></SweetSoft:ExtraButton>
            <button data-toggle="dropdown" class="btn btn-primary dropdown-toggle" aria-expanded="false">
                <span class="ace-icon fa fa-caret-down icon-only"></span>
            </button>
            <ul class="dropdown-menu dropdown-danger">
                <li>
                    <asp:LinkButton ID="lnkUpdate" runat="server" Text="Cập nhật thông tin chung" OnClick="btnThem_Click" CommandArgument="update"></asp:LinkButton>
                </li>
            </ul>
        </div>

        <div class="form-group" style="display: none;">
            <label class="extra-label">
                Tình trạng
            </label>
            <SweetSoft:ExtraComboBox ID="cbTinhTrang" runat="server" EmptyMessage="Chọn tình trạng trước khi thanh lý">
                <asp:ListItem Value="Bình thường" Text="Bình thường"></asp:ListItem>
                <asp:ListItem Value="Quá hạn" Text="Quá hạn"></asp:ListItem>
            </SweetSoft:ExtraComboBox>
        </div>
    </div>
</div>

<div class="row">
    <div class="col-lg-12">
        <div class="table-responsive no-top-bottom default">
            <table style="display: inline-table !important;" class="table dataTable table-bordered table-hover   dt-responsive responsive  table-striped dtr-inline">
                <thead>
                    <tr>
                        <th style="width: 30px">STT</th>
                        <th style="width: 70px">Mã mẫu</th>
                        <th style="width: 70px">Ngày nhận mẫu</th>
                        <th style="width: 105px">Ngày dự kiến trả mẫu</th>
                        <%--  <th style="width: 120px">Tình trạng mẫu khi thanh lý</th>--%>
                        <th style="width: 40px">Thao tác</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="repData" runat="server" OnItemDataBound="repData_ItemDataBound">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: center;"><%# Container.ItemIndex + 1 %></td>
                                <td style="text-align: center;">
                                    <%# Eval("Mamau") %>
                                </td>
                                <td style="text-align: center;"><%# Eval("NgayNhanMauToString") %></td>
                                <td style="text-align: center;"><%# Eval("NgayThanhLyDuKienToString") %></td>
                                <%--<td style="text-align: center;"><%# Eval("TinhTrangMauTruocHuy") %></td>--%>
                                <td style="text-align: center;">
                                    <div class="hidden-sm hidden-xs btn-group">
                                        <SweetSoft:ExtraButton ID="btnDelete" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>'>
                                        </SweetSoft:ExtraButton>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <label class="extra-label">
            Chúng tôi gồm:
        </label>
        <SweetSoft:ExtraTextBox ID="txtChungToiGom" runat="server" TextMode="MultiLine" Height="100px" PlaceHolder="Chúng tôi gồm"></SweetSoft:ExtraTextBox>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        SearchMau();
        addRequestHanlde(SearchMau);

    });

    function SearchMau(s, a) {
        var hiddenMauID = $("[id$='hiddenMauID']");
        var hiddenPageType = $("[id$='hiddenPageType']");
        if ($("input[type='text'][id$='txtSearchMaMau']").length > 0) {
            //$(".ui-autocomplete, .ui-dialog, .ui-helper-hidden-accessible").remove();
            //$("input[type='text'][id$='txtDetailTenChiTietHangHoa']").change(function () {
            //    var val = $("input[type='text'][id$='txtDetailTenChiTietHangHoa']").val();
            //});
            $("input[type='text'][id$='txtSearchMaMau']").on('input', function (e) {
                var vl = $("input[type='text'][id$='txtSearchMaMau']").val();
                if (vl.length === 0) {

                }

            });

            $("input[type='text'][id$='txtSearchMaMau']").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/Default.aspx/GetMauXetNghiemThanhLy",
                        data: "{'key':'" + $("input[type='text'][id$='txtSearchMaMau']").val() + "', 'Type':'" + hiddenPageType.val() + "'}",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (result) {
                            response($.map(result.d.List, function (item) {
                                return { MaMau: item.MaMau, ID: item.Id, NgayThanhLyDuKienToString: item.NgayThanhLyDuKienToString };
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
                    hiddenMauID.val("");
                    return false;
                },
                select: function (event, ui) {
                    $("input[type='text'][id$='txtSearchMaMau']").val(ui.item.MaMau);
                    hiddenMauID.val(ui.item.ID);
                    return false;
                }
            }).data("ui-autocomplete")._renderItem = function (ul, item) {
                return $("<li>")
                    .data("ui-autocomplete-item", item)
                    .append("<span class='text' style='width:30px;'>"
                        + item.MaMau
                        + "<small class='text-muted'><span>Ngày thanh lý: "
                        + item.NgayThanhLyDuKienToString
                        + "</span></small>"
                        + '</span>')
                    .appendTo(ul);
            };
        }
        else {
        }
    }
</script>
