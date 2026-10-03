<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="BangTongKetXetNghiem.aspx.cs" Inherits="Pasteur.Laboratory.P2.BangTongKetXetNghiem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .select2-search:after {
            content: "" !important;
        }

        .select2-item:focus {
            background-color: #bababa;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>
            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Tìm kiếm
                        <asp:Literal ID="headerTitle" runat="server"></asp:Literal></h5>

                </div>
                <div class="widget-body">
                    <div class="widget-main ">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-xs-12">
                                <div class="form-group">
                                    <label class="extra-label">
                                        Từ ngày
                                    </label>
                                    <SweetSoft:ExtraDateTimePicker ID="dateFrom" runat="server" Width="100%" PlaceHolder="Chọn ngày bắt đầu"></SweetSoft:ExtraDateTimePicker>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-12">
                                <div class="form-group">
                                    <label class="extra-label">
                                        tới ngày
                                    </label>
                                    <SweetSoft:ExtraDateTimePicker ID="dateTo" runat="server" Width="100%" PlaceHolder="Chọn ngày kết thúc"></SweetSoft:ExtraDateTimePicker>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-12">
                                <div class="form-group">
                                    <label class="extra-label">
                                        Loại mẫu
                                    </label>
                                    <SweetSoft:ExtraComboBox ID="cbLoaiMau" runat="server" EmptyMessage="Chọn loại mẫu" ComboStyle="Success"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                            <div class="col-lg-3 col-md-3 col-xs-12">
                                <div class="form-group">
                                    <label class="extra-label">
                                        Quy chuẩn
                                    </label>
                                    <SweetSoft:ExtraComboBox ID="cbQuyChuan" runat="server" EmptyMessage="Chọn quy chuẩn" ComboStyle="Warning"></SweetSoft:ExtraComboBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label class="extra-label">
                                        Thông tin mẫu
                                    </label>
                                    <asp:ListBox SelectionMode="Multiple" ID="ddlThongTinMau" name="states[]" runat="server" Width="100%" CssClass="select2">
                                        <asp:ListItem Value="HoTenMaMMau">Họ tên / mã số mẫu</asp:ListItem>
                                        <asp:ListItem Value="DiaChi">Địa chỉ</asp:ListItem>
                                        <asp:ListItem Value="NgayLayMau">Ngày lấy mẫu</asp:ListItem>
                                        <asp:ListItem Value="NgayNhanMau">Ngày nhận mẫu</asp:ListItem>
                                        <asp:ListItem Value="NgayThucHien">Ngày thực hiện</asp:ListItem>
                                    </asp:ListBox>
                                </div>
                            </div>
                            <div class="col-lg-6 col-xs-12">
                                <div class="form-group">
                                    <label class="extra-label">
                                        Chỉ tiêu xét nghiệm
                                    </label>
                                    <asp:ListBox ID="ddlChiTieu" SelectionMode="Multiple" Width="100%" runat="server" CssClass="select2"></asp:ListBox>
                                </div>
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
<asp:Content ID="Content8" ContentPlaceHolderID="ScriptInlinePage" runat="server">

    <link href="/css/combobox/select2.min.css" rel="stylesheet" />
    <script src="/scripts/combobox/select2.min.js"></script>
    <script type="text/javascript">
        function InitSelect2() {
            var select_2 = $(".select2");
            select_2.select2();
            //var select_3 = $(".select2");
            //select_3.select2({

            //    ajax: {
            //        url: "/Default.aspx/SearchChiTieuByKeyWord",
            //        type: "POST",
            //        dataType: "json",
            //        contentType: "application/json; charset=utf-8",
            //        delay: 250,
            //        data: function (params) {

            //            var search = '';

            //            if (params.term !== undefined) {
            //                search = params.term;
            //            }

            //            return "{'key': '" + search + "'}";
            //        },
            //        processResults: function (data) {

            //            var items = data.d.List;

            //            if (items == null) {
            //                return null;
            //            }

            //            return {
            //                results: items
            //            };
            //        },

            //    },
            //    minimumInputLength: 2,
            //    templateResult: formatRepo,
            //    templateSelection: formatRepoSelection

            //});
        }

        function formatRepo(repo) {

            var value = repo.TenTaiCacKhoa;
            if (typeof (value) != 'undefined') {
                var $container = $(
                    "<div class='select2-item'>" + repo.TenTaiCacKhoa + "</div>"
                );
                return $container;
            }

            //var $container = $(
            //    "<div class='select2-result-repository clearfix'>" +
            //    "<div class='select2-result-repository__avatar'></div>" +
            //    "<div class='select2-result-repository__meta'>" +
            //    "<div class='select2-result-repository__title'></div>" +
            //    "<div class='select2-result-repository__description'>" + repo.text + "</div>" +
            //    "<div class='select2-result-repository__statistics'>" +
            //    "<div class='select2-result-repository__forks'><i class='fa fa-flash'></i> </div>" +
            //    "<div class='select2-result-repository__stargazers'><i class='fa fa-star'></i> </div>" +
            //    "<div class='select2-result-repository__watchers'><i class='fa fa-eye'></i> </div>" +
            //    "</div>" +
            //    "</div>" +
            //    "</div>"
            //);
            //return repo.TenTaiCacKhoa;


            //$container.find(".select2-result-repository__title").text(repo.full_name);
            //$container.find(".select2-result-repository__description").text(repo.description);
            //$container.find(".select2-result-repository__forks").append(repo.forks_count + " Forks");
            //$container.find(".select2-result-repository__stargazers").append(repo.stargazers_count + " Stars");
            //$container.find(".select2-result-repository__watchers").append(repo.watchers_count + " Watchers");

            //return $container;
        }

        function formatRepoSelection(repo) {

            var value = repo.TenTaiCacKhoa;
            if (typeof (value) != 'undefined') {
                var $container = $(
                    "<div style='background-color:gray;color:white;'>" + repo.TenTaiCacKhoa + "</div>"
                );
                return $container;
            }
        }

        addRequestHanlde(InitSelect2);
        InitSelect2();

    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
</asp:Content>
