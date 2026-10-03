<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuDeNghiThuTien.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.PhieuDeNghiThuTien" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In phiếu đề nghị thu tiền </title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/Printing.css" rel="stylesheet" />
    <script src="/scripts/jQuery-2.1.4.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Questrial&display=swap" rel="stylesheet" />
    <style type="text/css">
        .paper {
            padding: 1cm;
        }

        body, html {
            font-family: 'Questrial' !important;
            font-size: 15px !important;
        }

        table.tbl-data td, table.tbl-data th {
            border: solid 1px black;
            padding: 2px;
        }

        .sub-title {
            display: inline-block;
            font-size: 13px;
            font-style: italic
        }

        .paper-footer {
            width: 19cm;
        }

        p {
            margin-bottom: 2px;
        }

        @media print {
            @page {
                size: landscape
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paper landscape">
            <div class="row">
                <div class="col-lg-5 col-xs-5">
                    <span style="display: block; text-transform: uppercase;">viện pasteur nha trang
                    </span>
                    <span style="display: block;">KHOA / TT: <b>Khoa sức khỏe môi trường - Bệnh nghề nghiệp</b>
                    </span>
                </div>
                <div class="col-lg-5 col-xs-5">
                    <span style="display: block; font-weight: bold; text-transform: uppercase; text-align: center; font-size: 16px;">giấy đề nghị thu tiền
                    </span>
                    <span style="font-weight: bold; display: block; text-align: center;">
                        <asp:Literal ID="ltrSo" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
            <div class="row" style="margin-top: 20px; padding-left: 40px;">
                <p>
                    Nhận của ông/bà: <b>
                        <asp:Literal ID="ltrHoTenNguoiDongTien" runat="server"></asp:Literal></b>
                </p>
                <p>
                    Đơn vị: <b>
                        <asp:Literal ID="ltrDonVi" runat="server"></asp:Literal>
                    </b>-  Mã số thuế:
                    <b>
                        <asp:Literal ID="ltrMaSoThue" runat="server"></asp:Literal>
                    </b>
                </p>
                <p>
                    Địa chỉ: <b>
                        <asp:Literal ID="ltrDiaChi" runat="server"></asp:Literal></b>    -  Điện thoại:
                    <b>
                        <asp:Literal ID="ltrDienThoai" runat="server"></asp:Literal>
                    </b>
                </p>
                <p>
                    Nội dung xét nghiệm: <b>
                        <asp:Literal ID="ltrNoiDungXetNghiem" runat="server"></asp:Literal></b>
                </p>
            </div>
            <div class="row" style="margin-top: 0px;">
                <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                    <thead>
                        <tr>
                            <th colspan="6" style="font-weight: bold; text-align: center;">Phần dành cho các khoa / phòng / Trung tâm</th>
                            <th colspan="1" rowspan="2" style="font-weight: bold; text-align: center;">Thành tiền</th>
                        </tr>
                        <tr>
                            <th style="font-weight: bold; text-align: center; width: 30px">STT</th>
                            <th style="font-weight: bold; text-align: center; width: 150px;">Tên chỉ tiêu tại các khoa/TT</th>
                            <th style="font-weight: bold; text-align: center;">Tên chỉ tiêu XN trong TT/QĐ</th>
                            <th style="font-weight: bold; text-align: center; width: 120px">TT/QĐ</th>
                            <th style="font-weight: bold; text-align: center; width: 120px">STT</th>
                            <th style="font-weight: bold; text-align: center; width: 120px">ĐVT</th>
                            <%--<th style="font-weight: bold; text-align: center; width: 100px">Số lượng</th>
                           <th style="font-weight: bold; text-align: center; width: 100px">Đơn giá</th> --%>
                            <%--<th style="font-weight: bold; text-align: center; width: 100px">Thành tiền</th>--%>
                        </tr>
                        <tr>
                            <th style="font-weight: bold; text-align: center; width: 30px">A</th>
                            <th style="font-weight: bold; text-align: center;">B</th>
                            <th style="font-weight: bold; text-align: center; width: 150px;">C</th>
                            <th style="font-weight: bold; text-align: center; width: 100px">D</th>
                            <th style="font-weight: bold; text-align: center; width: 70px">E</th>
                            <th style="font-weight: bold; text-align: center; width: 70px">F</th>
                            <th style="font-weight: bold; text-align: center; width: 70px">G</th>
                            <%--<th style="font-weight: bold; text-align: center; width: 70px">H</th>--%>
                            <%--<th style="font-weight: bold; text-align: center; width: 70px">H</th>--%>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repData" runat="server" OnItemDataBound="repData_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;"><%# (Container.ItemIndex + 1) %></td>
                                    <td style="text-align: left;"><%#GetTen( Eval("TenTaiCacKhoa"), Eval("Italic")) %></td>
                                    <td style="text-align: left;"><%# Eval("TenTrongTTQD") %></td>
                                    <td style="text-align: left;"><%# Eval("TenThongTu") %></td>
                                    <td style="text-align: center;"><%# Eval("STTPP") %></td>
                                    <td style="text-align: center;"><%# Eval("TenDonViTinhTien") %></td>
                                    <%--<td style="text-align: left;">1</td>--%>
                                    <%-- <asp:Literal ID="ltrLastColumnDonGia" runat="server"></asp:Literal>--%>
                                    <asp:Literal ID="ltrLastColumnThanhTien" runat="server"></asp:Literal>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="6" style="text-align: right; padding-right: 20px; font-weight: bold;">Tổng cộng
                            </td>
                            <td style="text-align: right; font-weight: bold;">
                                <asp:Literal ID="ltrTongTien" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div contenteditable="true" class="row" style="text-align: right; padding-right: 20px; padding-top: 5px;">
                    <span style="display: block;">
                        <asp:Literal ID="ltrTongTienBangChu" runat="server"></asp:Literal></span>

                    <span style="display: block; margin-top: 20px;">
                        <asp:Literal ID="ltrNhaTrang" runat="server"></asp:Literal></span>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        <span style="display: block; font-weight: bold;">Người lập phiếu</span>
                        <span style="display: block; margin-top: 100px;">
                            <asp:Literal ID="ltrNguoiLapPhieu" runat="server"></asp:Literal>
                        </span>
                    </div>
                    <div class="col-lg-4 col-xs-4"></div>
                    <div class="col-lg-4 col-xs-4" style="text-align: center; font-weight: bold">Phòng TCKT</div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            window.onload = function () { setTimeout(window.print(), 500); }
        </script>
    </form>
</body>
</html>
