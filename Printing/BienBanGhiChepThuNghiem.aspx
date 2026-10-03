<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BienBanGhiChepThuNghiem.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.BienBanGhiChepThuNghiem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Biên bản ghi chép thử nghiệm</title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/Printing.css" rel="stylesheet" />
    <script src="/scripts/jQuery-2.1.4.min.js"></script>
    <style type="text/css">
        .paper {
            padding: 1cm;
        }

        body, html {
            font-family: 'Questrial' !important;
            font-size: 15px !important;
        }

        .label-dot {
            display: inline-table;
            min-width: 50px;
            /*border-bottom: 1px dashed;*/
        }

        .p1 {
            display: inline-block;
            text-decoration: none;
        }

        table.tbl-data td, table.tbl-data th {
            border: solid 1px black;
            padding: 2px;
        }



        .sub-title {
            display: inline-block;
            font-size: 12px;
            font-style: italic
        }

        .paper-footer {
            width: 19cm;
            /*position: fixed;*/
        }

        @media print {
            table.tbl-data {
                page-break-after: auto
            }

            .tbl-data tr {
                page-break-inside: avoid;
                page-break-after: auto
            }

                .tbl-data tr td {
                    page-break-inside: avoid;
                    page-break-after: auto
                }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paper" style="position: relative;">
            <div class="row">
                <div class="title">
                    <span style="text-transform: uppercase; font-weight: bold; display: block; font-size: 18px;">biên bản ghi chép thử nghiệm - kết quả quan trắc gốc
                    </span>
                </div>
            </div>
            <div class="row">
                <div style="float: left; width: 25%;">
                    <span class="p1">TT:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrTT" runat="server"></asp:Literal>
                    </span>
                </div>
                <div style="float: left; width: 30%; padding-left: 10px;">
                    <span class="p1">Mã số mẫu:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrMaMau" runat="server"></asp:Literal></span>
                </div>
                <div style="float: left; width: 43%; padding-left: 10px;">
                    <span class="p1">Loại mẫu:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrLoaiMau" runat="server"></asp:Literal></span>
                </div>
            </div>
            <div class="row" style="margin-top: 5px;">
                <div style="float: left; width: 25%;">
                    <span class="p1">Ngày nhận:</span>
                    <span class="label-dot" style="min-width: 71px;">
                        <asp:Literal ID="ltrNgayNhan" runat="server"></asp:Literal>
                    </span>
                </div>
                <div style="float: left; width: 30%; padding-left: 10px;">
                    <span class="p1">Ngày trả:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrNgayTra" runat="server"></asp:Literal></span>
                </div>
                <div style="float: left; width: 43%; padding-left: 10px;">
                    <span class="p1">Quy chuẩn:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrQuyChan" runat="server"></asp:Literal></span>
                </div>
            </div>
            <div class="row" style="margin-top: 5px;">
                <div style="float: left; width: 25%;">
                    <span class="p1">Ngày thực hiện:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrNgayThucHien" runat="server"></asp:Literal>
                    </span>
                </div>
                <div style="float: left; width: 30%; padding-left: 10px;">
                    <span class="p1">Ngày kết thúc:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrNgayKetThuc" runat="server"></asp:Literal></span>
                </div>
                <div style="float: left; width: 43%; padding-left: 10px;">
                    <span class="p1"><b>SNB:</b></span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrSNB" runat="server"></asp:Literal></span>
                </div>
            </div>
            <div class="row " style="margin-top: 5px;">
                <div style="float: left; width: 35%;">
                    <span class="p1">Người kiểm tra:</span>
                    <span class="label-dot">
                        <asp:Literal ID="ltrNgayKiemTra" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
            <div class="row" style="margin-top: 10px">
                <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                    <thead>
                    </thead>
                    <tbody>
                        <tr>
                            <td rowspan="2" style="width: 50px; text-align: center;">TT</td>
                            <td rowspan="2" style="width: 150px; text-align: center;">Tên chỉ tiêu / phương pháp</td>
                            <td colspan="3" style="text-align: center;">Quá trình nuôi cấy</td>
                            <td rowspan="2" style="width: 100px; text-align: center;">Kết quả</td>
                        </tr>
                        <tr>
                            <td style="text-align: center; width: 130px">Ngày làm ....../....../20......</td>
                            <td style="text-align: center; width: 130px">Ngày làm ....../....../20......</td>
                            <td style="text-align: center; width: 130px">Ngày làm ....../....../20......</td>
                        </tr>
                        <asp:Repeater ID="repData" runat="server" OnItemDataBound="repData_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;"><%# Container.ItemIndex + 1 %></td>
                                    <td style="vertical-align: text-top">
                                        <span style="font-size: 12px; min-height: 70px; display: block;">
                                            <b>Chỉ tiêu:</b><br />
                                            <asp:Literal ID="tenChiTieu" runat="server" Text='<%# Eval("TenTaiCacKhoa") %>'></asp:Literal>
                                        </span>
                                        <span style="font-size: 12px; min-height: 130px; display: block;">
                                            <b>Phương pháp:</b><br />
                                            <%# Eval("TenPhuongPhap") %>
                                        </span>
                                        <span style="font-size: 12px;"><b>Người thực hiện:</b></span>
                                    </td>
                                    <td style="vertical-align: text-top">
                                        <span style="min-height: 70px; display: block;">MT:
                                            <asp:Literal ID="ltrMT" runat="server"></asp:Literal>
                                        </span>
                                        <span style="min-height: 137px; display: block;">V:<br />
                                            <span style="font-size: 12px; display: inline-block">.Chứng âm </br>☐ NaCl 0,9%</span><br />

                                            <span style="font-size: 12px; display: inline-block;">☐ Khác..................</span><br />

                                            <span style="font-size: 12px; display: inline-block">.Chứng dương
                                                <asp:Literal ID="ltrChungDuong1" runat="server"></asp:Literal>
                                                <br />☐ Khác..................
                                            </span>
                                            <br />
                                            <span style="font-size: 12px; display: inline-block;">Nhiệt độ NC: 
                                                <br />
                                                ☐ (36&#177;1)℃(INB 700)
                                                <br />
                                                ☐ (44&#177;0.5)℃(INB 400)
                                            </span>
                                            <%--<span style="font-size: 12px; display: inline-block; padding-left: 68px;">☐ 44.5℃</span>--%>
                                        </span>
                                        <span style="font-size: 12px; display: block;">☐ Thắm ☐ Vinh
                                        </span>
                                    </td>
                                    <td style="vertical-align: text-top">
                                        <span style="min-height: 70px; display: block;">MT:
                                             
                                        </span>
                                        <span style="min-height: 137px; display: block;">V:<br />
                                            <span style="font-size: 12px; display: inline-block">.Chứng âm</br> ☐ NaCl 0,9%</span><br />

                                            <span style="font-size: 12px; display: inline-block;">☐ Khác..................</span><br />

                                            <span style="font-size: 12px; display: inline-block">.Chứng dương
                                                <asp:Literal ID="ltrChungDuong2" runat="server"></asp:Literal>
                                                <br />☐ Khác..................
                                            </span><br />
                                            <span style="font-size: 12px; display: inline-block;">Nhiệt độ NC: 
                                                <br />
                                                ☐ (36&#177;1)℃(INB 700)
                                                <br />
                                                ☐ (44&#177;0.5)℃(INB 400)
                                            </span>
                                            <%--<span style="font-size: 12px; display: inline-block; padding-left: 68px;">☐ 44.5℃</span>--%>
                                        </span>
                                        <span style="font-size: 12px; display: block;">☐ Thắm ☐ Vinh
                                        </span>
                                    </td>
                                    <td>
                                        <span style="min-height: 70px; display: block;">MT:<br />
                                        </span>
                                        <span style="min-height: 137px; display: block;">V:<br />
                                            <span style="font-size: 12px; display: inline-block">.Chứng âm</br> ☐ NaCl 0,9%</span><br />

                                            <span style="font-size: 12px; display: inline-block;">☐ Khác..................</span><br />

                                            <span style="font-size: 12px; display: inline-block">.Chứng dương
                                                <asp:Literal ID="ltrChungDuong3" runat="server"></asp:Literal>
                                                <br />☐ Khác..................
                                            </span><br />
                                            <span style="font-size: 12px; display: inline-block;">Nhiệt độ NC: 
                                                <br />
                                                ☐ (36&#177;1)℃(INB 700)
                                                <br />
                                                ☐ (44&#177;0.5)℃(INB 400)
                                            </span>
                                            <%--<span style="font-size: 12px; display: inline-block; padding-left: 68px;">☐ 44.5℃</span>--%>
                                        </span>
                                        <span style="font-size: 12px; display: block;">☐ Thắm ☐ Vinh
                                        </span>
                                    </td>
                                    <td></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
        <script type="text/javascript">
            window.onload = function () { window.print(); }
        </script>
    </form>
</body>
</html>
