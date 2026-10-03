<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuTiepNhanYeuCau.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.PhieuTiepNhanYeuCau" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In phiếu tiếp nhận yêu cầu</title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/Printing.css" rel="stylesheet" />
    <script src="/scripts/jQuery-2.1.4.min.js"></script>
    <style type="text/css">
        .paper {
            padding-top: 0cm;
        }

        .table.black {
            border: solid 1px black !important;
        }

            .table.black td, .table.black th {
                border: solid 1px black !important;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paper" style="position: relative;">
            <div class="">
                <div class="row">
                    <div class="header" style="font-size: 18px;">
                        <p>Viện pasteur nha trang</p>
                        <p>Khoa sức khỏe môi trường - bệnh nghề nghiệp</p>
                    </div>
                </div>
                <div class="row">
                    <div class="title">
                        <span style="text-transform: uppercase; font-weight: bold; display: block; font-size: 18px;">phiếu tiếp nhận yêu cầu
                        </span>
                        <span style="font-style: italic; font-weight: bold; font-size: 18px;">Số:
                            <asp:Literal ID="ltrMaMau" runat="server"></asp:Literal></span>
                    </div>
                </div>
                <div class="row info">
                    <div class="col-lg-6 col-md-6 col-xs-6 col-sm-6 no-padding-left">
                        <div>
                            1.<span class="p1">Khách Hàng:</span>
                            <p>
                                <span class="label-n">Tên:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrTenKhachHang" runat="server"></asp:Literal></span>
                            </p>
                            <p>
                                <span class="label-n">Địa chỉ:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDiaChiKhachHang" runat="server"></asp:Literal></span>
                            </p>
                            <p>
                                <span class="label-n">Điện thoại / Email:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDienThoaiKhachHang" runat="server"></asp:Literal></span>
                            </p>
                        </div>
                        <div>
                            2.<span class="p1">Người lấy mẫu:</span>
                            <p>
                                <span class="label-n">Tên:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrTenNguoiLayMau" runat="server"></asp:Literal></span>
                            </p>
                            <%--   <p>
                                <span class="label-n">Địa chỉ:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDiaChiNguoiLayMau" runat="server"></asp:Literal></span>
                            </p>--%>
                            <p>
                                <span class="label-n">Điện thoại / Email:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDienThoaiNguoiLayMau" runat="server"></asp:Literal></span>
                            </p>
                        </div>
                        <div>
                            3.<span class="p1">Nơi lấy mẫu:</span>
                            <span class="content" id="spanNoiGui">
                                <asp:Literal ID="ltrNoiLayMau" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            4.<span class="p1">Loại mẫu:</span>
                            <span class="content">
                                <asp:Literal ID="ltrLoaiMau" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            5.<span class="p1">Ngày lấy mẫu:</span>
                            <span class="content">
                                <asp:Literal ID="ltrNgayLayMau" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            6.<span class="p1">Ngày trả kết quả:</span>
                            <span class="content">
                                <asp:Literal ID="ltrNgayTraKetQua" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            7.<span class="p1">Quy chuẩn đánh giá:</span>
                            <span class="content">
                                <asp:Literal ID="ltrQuyChuan" runat="server"></asp:Literal></span>
                        </div>

                    </div>
                    <div class="col-lg-6 col-md-6 col-xs-6 col-sm-6 no-padding">
                        <div>
                            8.<span class="p1">Người gửi mẫu:</span>
                            <p>
                                <span class="label-n">Tên:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrTenNguoiGuiMau" runat="server"></asp:Literal></span>
                            </p>
                            <p>
                                <span class="label-n">Địa chỉ:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDiaChiNguoiGuiMau" runat="server"></asp:Literal></span>
                            </p>
                            <p>
                                <span class="label-n">Điện thoại / Email:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDienThoaiNguoiGuiMau" runat="server"></asp:Literal></span>
                            </p>
                        </div>
                        <div>
                            9.<span class="p1">Người nhận mẫu:</span>
                            <p>
                                <span class="label-n">Tên:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrTenNguoiNhanMau" runat="server"></asp:Literal></span>
                            </p>
                            <%--<p>
                                <span class="label-n">Địa chỉ:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDiaChiNguoiNhanMau" runat="server"></asp:Literal></span>
                            </p>--%>
                            <p>
                                <span class="label-n">Điện thoại / Email:</span>
                                <span class="content">
                                    <asp:Literal ID="ltrDienThoaiNguoiNhanMau" runat="server"></asp:Literal></span>
                            </p>
                        </div>
                        <div>
                            10.<span class="p1">Điều kiện bảo quản:</span>
                            <span class="content">
                                <asp:Literal ID="ltrDieuKienBaoQuan" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            11.<span class="p1">Ngày nhận mẫu:</span>
                            <span class="content">
                                <asp:Literal ID="ltrNgayGioNhanMau" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            12.<span class="p1">Mô tả mẫu:</span>
                            <span class="content">
                                <asp:Literal ID="ltrMoTaMau" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            13.<span class="p1">Yêu cầu khác:</span>
                            <span class="content">
                                <asp:Literal ID="ltrYeuCauKhac" runat="server"></asp:Literal></span>
                        </div>
                        <div>
                            14.<span class="p1">Ghi chú:</span>
                            <span class="content">
                                <asp:Literal ID="ltrGhiChu" runat="server"></asp:Literal></span>
                        </div>
                    </div>
                </div>
                <%-- <div class="tbl-border header-fake;display: none;">
                    <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                    <div class="column phuongphap-name text-center header">Phương pháp</div>
                    <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                    <div class="column phuongphap-name text-center header">Phương pháp</div>
                    <div style="clear: both;"></div>
                </div>--%>
                <div class="row" style="margin-top: 20px; display: none;">
                    <div class="tbl-border">
                        <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                        <div class="column phuongphap-name text-center header">Phương pháp</div>
                        <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                        <div class="column phuongphap-name text-center header">Phương pháp</div>
                        <asp:Repeater ID="repData" runat="server">
                            <ItemTemplate>
                                <div class="column chitieu-name text-left"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></div>
                                <div class="column phuongphap-name text-left"><%#GetTen(Eval("TenPhuongPhap"),Eval("Italic")) %></div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div style="clear: both;"></div>
                    </div>
                </div>
                <div class="row info">
                    <table class="table table-bordered table-sm black">
                        <thead>
                            <tr>

                                <td style="width: 170px; text-align: center; font-weight: bold; text-transform: uppercase;">
                                    <div class="rezise">Chỉ tiêu</div>
                                </td>
                                <td style="text-align: center; font-weight: bold; text-transform: uppercase;">
                                    <div class="rezise">Phương pháp</div>
                                </td>
                                <td style="text-align: center; width: 80px; font-weight: bold; text-transform: uppercase;">Đơn giá</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repChiTieu" runat="server" OnItemDataBound="repChiTieu_ItemDataBound">
                                <ItemTemplate>
                                    <tr>

                                        <td style="text-align: left;"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></td>
                                        <td style="text-align: left;"><%#GetTen(Eval("TenPhuongPhap"),Eval("Italic")) %></td>
                                        <%--<td style="text-align: right;"><%# Eval("DonGiaToString") %></td>--%>
                                        <asp:Literal ID="ltrLastColumnThanhTien" runat="server"></asp:Literal>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr>
                                <td colspan="2" style="text-align: right; padding-right: 3px; font-weight: bold; text-transform: uppercase;">tổng cộng</td>
                                <td style="text-align: right; font-weight: bold;">
                                    <asp:Literal ID="ltrTongCong" runat="server"></asp:Literal></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="divVimcert" runat="server" visible="false" style="margin-top: 40px;">
                    <div style="font-weight: bold; text-transform: uppercase; margin: 5px 0px 10px 0px; text-align: center;">
                        Chỉ tiêu xét nghiệm theo Vimcert
                    </div>
                    <div class="tbl-border header-fake">
                        <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                        <div class="column phuongphap-name text-center header">Phương pháp</div>
                        <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                        <div class="column phuongphap-name text-center header">Phương pháp</div>
                        <div style="clear: both;"></div>
                    </div>
                    <div class="row" style="margin-top: 20px; display: none;">
                        <div class="tbl-border">
                            <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                            <div class="column phuongphap-name text-center header">Phương pháp</div>
                            <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                            <div class="column phuongphap-name text-center header">Phương pháp</div>
                            <asp:Repeater ID="repDataVimcert" runat="server">
                                <ItemTemplate>
                                    <div class="column chitieu-name text-left"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></div>
                                    <div class="column phuongphap-name text-left"><%#GetTen(Eval("TenPhuongPhapHienThi"),Eval("Italic")) %></div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div style="clear: both;"></div>
                        </div>
                    </div>
                    <div class="row info">
                        <asp:Repeater ID="repVimcert" runat="server">
                            <HeaderTemplate>
                                <table class="table table-bordered table-sm">
                                    <thead>
                                        <tr>

                                            <td style="width: 170px; text-align: left;">Chỉ tiêu</td>
                                            <td style="text-align: left;">Phương pháp</td>
                                            <td style="text-align: center; width: 80px;">Đơn giá</td>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>

                                    <td style="text-align: left;"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></td>
                                    <td style="text-align: left;"><%#GetTen(Eval("TenPhuongPhap"),Eval("Italic")) %></td>
                                    <td style="text-align: right;"><%# Eval("DonGiaToString") %></td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
                        </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12">
                        <span>Tổng số chỉ tiêu: <b>
                            <asp:Literal ID="ltrTongSoChiTieu" runat="server"></asp:Literal></b>
                        </span>
                        <span style="margin-left: 10px;">Trong đó Vi sinh: <b>
                            <asp:Literal ID="ltrTongSoViSinh" runat="server"></asp:Literal></b>
                        </span>
                        <span style="margin-left: 10px;">Hóa lý: <b>
                            <asp:Literal ID="ltrTongSoHoaLy" runat="server"></asp:Literal></b>
                        </span>
                    </div>
                </div>
                <div class="row" style="margin-top: 10px">
                    <div class="col-lg-6 col-md-6 col-xs-6" style="text-align: left; padding-left: 60px;">
                        <p></p>
                        <p>Khách hàng</p>

                    </div>
                    <div class="col-lg-6 col-md-6 col-xs-6" style="text-align: center;">
                        <p>
                            <asp:Literal ID="ltrKhanhHoa" runat="server"></asp:Literal>
                        </p>
                        <p>Người nhận</p>
                    </div>
                </div>
                <div class="row" style="margin-top: 80px">
                    <div class="col-lg-6 col-xs-6" style="text-align: center;">
                    </div>

                    <div class="col-lg-6 col-xs-6" style="text-align: center;">
                        <p>
                            <asp:Literal ID="ltrNguoiNhanMau" runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>

                <div class=" paper-footer" contenteditable="true">

                    <div style="padding: 4px 0px;">
                        * Phiếu tiếp nhận này được lập thành 2 bản, mỗi bên giữ 1 bản
                    </div>
                    <div style="padding: 4px; border-top: solid 1px #000000;">
                        <div class="col-lg-4 col-xs-4" style="text-align: center;">BM-TT-7.1-01</div>
                        <div class="col-lg-4 col-xs-4" style="text-align: center;">Lần ban hành: 01</div>
                        <div class="col-lg-4 col-xs-4" style="text-align: center;">Ngày ban hành: 10/02/2020</div>
                    </div>
                </div>

            </div>
        </div>
        <script type="text/javascript">
            window.onload = function () {
                var content = $("#spanNoiGui").text();

                var content2 = $("#spanNoiGui").text().replace(/\n/g, "<br />");
                $("#spanNoiGui").html(content2);
                window.print();

            }
        </script>
    </form>
</body>
</html>
