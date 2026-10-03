<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BienBanThanhLy.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.BienBanThanhLy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Biên bản thanh lý</title>
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
        <div>
            <div class="paper" style="position: relative;">
                <div class="row">
                    <div style="text-align: center; text-transform: uppercase; border-bottom: double">
                        viện pasteur nha trang<br />
                        khoa sức khỏe môi trường - bệnh nghề nghiệp
                    </div>
                </div>
                <div class="row" style="margin-top: 10px;">
                    <div class="title">
                        <span style="text-transform: uppercase; font-weight: bold; display: block; font-size: 18px;">biên bản thanh lý - hủy mẫu
                        </span>
                    </div>
                </div>
                <div class="row" style="margin-top: 10px;">

                    <asp:Literal ID="ltrHomNay" runat="server"></asp:Literal>
                    tại <span contenteditable="true">
                        <asp:Literal ID="ltrTai" runat="server"></asp:Literal>
                    </span>chúng tôi gồm:<br />
                    <div contenteditable="true" style="cursor: text;">
                        <asp:Literal ID="ltrChungToGom" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="row" style="margin-top: 10px;">
                    Tiến hành hủy các mẫu sau:
                </div>
                <div class="row">
                    <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                        <thead>
                            <tr>
                                <th style="width: 30px; text-align: center;">TT </th>
                                <th style="width: 100px; text-align: center;">Mã số mẫu </th>
                                <th style="width: 100px; text-align: center;">Ngày nhận mẫu </th>
                                <th style="width: 100px; text-align: center;">Ngày trả kết quả </th>

                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;"><%# Container.ItemIndex +1 %></td>
                                        <td style="text-align: center;"><%# Eval("MaMau") %></td>
                                        <td style="text-align: center;"><%# Eval("NgayNhanMauToString") %></td>
                                        <td style="text-align: center;"><%# Eval("NgayTraKetQuaToString") %></td>

                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>

                    <asp:Literal ID="ltrKetThuc" runat="server"></asp:Literal>
                    <br />
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        <span style="display: block; text-transform: uppercase; text-align: center; font-weight: bold; font-size: 15px">Đại diện người hủy mẫu</span>
                        <span style="display: block; margin-top: 90px">
                            <asp:Literal ID="ltrToTruong" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <div class="col-lg-4 col-xs-4"></div>
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        <span style="display: block; text-transform: uppercase; text-align: center; font-weight: bold; font-size: 15px">Trưởng khoa</span>
                        <span style="display: block; margin-top: 90px">
                            <asp:Literal ID="ltrTruongKhoa" runat="server"></asp:Literal></span>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
