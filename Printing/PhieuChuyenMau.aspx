<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuChuyenMau.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.PhieuChuyenMau" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In phiếu chuyển mẫu</title>
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
        <div class="paper" style="position: relative;" runat="server" id="divHL" visible="false">
            <div class="">
                <div class="row">
                    <div class="header" style="font-size: 18px;">
                        <p>Viện pasteur nha trang</p>
                        <p>Khoa sức khỏe môi trường - bệnh nghề nghiệp</p>
                    </div>
                </div>
                <div class="row">
                    <div class="title">
                        <span style="font-weight: bold; font-size: 20px; text-transform: uppercase;">phiếu chuyển mẫu</span>
                    </div>
                </div>
                <div class="row">
                    <table style="border-spacing: 0; border: solid 1px black; width: 100%;">
                        <tr>
                            <td style="width: 35%; border: solid 1px black; padding: 5px;">1. Loại mẫu:
                                <asp:Literal ID="ltrLoaiMau" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 30%; border: solid 1px black; padding: 5px;">2. Ngày nhận:
                                <asp:Literal ID="ltrNgayNhanMau" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 35%; border: solid 1px black; padding: 5px;">3. Mã số mãu: <b>
                                <asp:Literal ID="ltrMaSoMau" runat="server"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 35%; border: solid 1px black; padding: 5px;">4. Lượng mẫu:
                                <asp:Literal ID="ltrLuongMau" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 30%; border: solid 1px black; padding: 5px;">Hóa lý:
                                <asp:Literal ID="ltrLuongHoaLy" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 35%; border: solid 1px black; padding: 5px;">Vi sinh: 
                                <asp:Literal ID="ltrLuongViSinh" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="border: solid 1px black; padding: 5px;">5.Điều kiện bảo quản:<asp:Literal ID="ltrDieuKienBaoQuan" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 35%; border: solid 1px black; padding: 5px;">6. Ngày chuyển mẫu:<br />
                                <asp:Literal ID="ltrNgayChuyenMau" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 30%; border: solid 1px black; padding: 5px;">7.Quy chuẩn đánh giá:<br />
                                <asp:Literal ID="ltrQuyChuanDanhGia" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 35%; border: solid 1px black; padding: 5px;">8.Ngày trả kết qủa:
                                <br />
                                <asp:Literal ID="ltrNgayTraKetQua" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row" style="margin-top: 20px; display: none;">
                    <div class="tbl-border">
                        <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                        <div class="column phuongphap-name text-center header">Phương pháp</div>
                        <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                        <div class="column phuongphap-name text-center header">Phương pháp</div>
                        <asp:Repeater ID="repHoaLy" runat="server">
                            <ItemTemplate>
                                <div class="column chitieu-name text-left"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></div>
                                <div class="column phuongphap-name text-left"><%#Eval("TenPhuongPhap") %></div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div style="clear: both;"></div>
                    </div>
                </div>
                <div class="row info" style="margin-top: 20px;">
                    <table class="table table-bordered table-sm black">
                        <thead>
                            <tr>

                                <td style="text-align: center; font-weight: bold; text-transform: uppercase;">
                                    <div class="rezise">Chỉ tiêu</div>
                                </td>
                                <td style="text-align: center; font-weight: bold; text-transform: uppercase;">
                                    <div class="rezise">Phương pháp</div>
                                </td>
                                <td style="text-align: center; width: 80px; font-weight: bold; text-transform: uppercase; display: none;">Đơn giá</td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repChiTieuHoaLy" runat="server" OnItemDataBound="repChiTieuHoaLy_ItemDataBound">
                                <ItemTemplate>
                                    <tr>

                                        <td style="text-align: left;"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></td>
                                        <td style="text-align: left;">
                                            <div contenteditable="true"><%#GetTen(Eval("TenPhuongPhap"),Eval("Italic")) %></div>
                                        </td>
                                        <%--<td style="text-align: right;"><%# Eval("DonGiaToString") %></td>--%>
                                        <asp:Literal ID="ltrLastColumnThanhTien" Visible="false" runat="server"></asp:Literal>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                            <tr style="display:none;">
                                <td  style="text-align: right; padding-right: 3px; font-weight: bold; text-transform: uppercase;">tổng cộng</td>
                                <td style="text-align: right; font-weight: bold;">
                                    <asp:Literal ID="ltrTongCong" runat="server"></asp:Literal></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="row" style="margin-top: 20px;">
                    <table style="border-spacing: 0; border: solid 1px black; width: 100%;">
                        <tr>
                            <td style="width: 38%; border: solid 1px black; padding: 5px;">Tổng số chỉ tiêu:
                                <asp:Literal ID="ltrTongSoChiTieu" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 31%; border: solid 1px black; padding: 5px;">Vi sinh: 
                                <asp:Literal ID="ltrTongSoViSinh" runat="server"></asp:Literal>
                            </td>
                            <td style="width: 31%; border: solid 1px black; padding: 5px;">Hóa lý:
                                <asp:Literal ID="ltrTongSoHoaLy" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row" style="margin-top: 10px;">
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        Người chuyển mẫu
                    </div>
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        Labo Hóa
                    </div>
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        Labo Vi sinh
                    </div>
                </div>
                <div class="row" style="margin-top: 130px;">
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">
                        <asp:Literal ID="ltrNguoiChuyenMau" runat="server"></asp:Literal>
                    </div>
                </div>

                <div class=" paper-footer">
                    <div style="padding: 4px 0px; border-bottom: solid 1px #000000;">
                        * Phiếu chuyển mẫu này được lập thành
                            <asp:Literal ID="ltrBan" runat="server"></asp:Literal>
                        bản, mỗi bộ phận lưu 1 bản
                    </div>
                    <div style="padding: 4px">
                        <div class="col-lg-4 col-xs-4" style="text-align: center;">BM-TT-7.3&7.4-02</div>
                        <div class="col-lg-4 col-xs-4" style="text-align: center;">Lần ban hành: 01</div>
                        <div class="col-lg-4 col-xs-4" style="text-align: center;">Ngày ban hành: 10/02/2020</div>
                    </div>
                </div>

            </div>
        </div>

        <div class="paper" style="page-break-before: always" runat="server" id="divVS" visible="false">
            <div class="row">
                <div class="header" style="font-size: 18px;">
                    <p>Viện pasteur nha trang</p>
                    <p>Khoa sức khỏe môi trường - bệnh nghề nghiệp</p>
                </div>
            </div>
            <div class="row">
                <div class="title">
                    <span style="font-weight: bold; font-size: 20px; text-transform: uppercase;">phiếu chuyển mẫu</span>
                </div>
            </div>
            <div class="row">
                <table style="border-spacing: 0; border: solid 1px black; width: 100%;">
                    <tr>
                        <td style="width: 35%; border: solid 1px black; padding: 5px;">1. Loại mẫu:
                                <asp:Literal ID="ltrVSLoaiMau" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 30%; border: solid 1px black; padding: 5px;">2. Ngày nhận:
                                <asp:Literal ID="ltrVSNgayNhanMau" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 35%; border: solid 1px black; padding: 5px;">3. Mã số mãu: <b>
                            <asp:Literal ID="ltrVSMaSoMau" runat="server"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 35%; border: solid 1px black; padding: 5px;">4. Lượng mẫu:
                                <asp:Literal ID="ltrVSLuongMau" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 30%; border: solid 1px black; padding: 5px;">Hóa lý:
                                <asp:Literal ID="ltrVSLuongHoaLy" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 35%; border: solid 1px black; padding: 5px;">Vi sinh:
                                <asp:Literal ID="ltrVSLuongViSinh" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="border: solid 1px black; padding: 5px;">5.Điều kiện bảo quản:<asp:Literal ID="ltrVSDieuKienBaoQuan" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 35%; border: solid 1px black; padding: 5px;">6. Ngày chuyển mẫu:<br />
                            <asp:Literal ID="ltrVSNgayChuyenMau" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 30%; border: solid 1px black; padding: 5px;">7.Quy chuẩn đánh giá:<br />
                            <asp:Literal ID="ltrVSQuyChuanDanhGia" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 35%; border: solid 1px black; padding: 5px;">8.Ngày trả kết qủa:
                                <br />
                            <asp:Literal ID="ltrVSNgayTraKetQua" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="row" style="margin-top: 20px; display: none;">
                <div class="tbl-border">
                    <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                    <div class="column phuongphap-name text-center header">Phương pháp</div>
                    <div class="column chitieu-name text-center header">Chỉ tiêu</div>
                    <div class="column phuongphap-name text-center header">Phương pháp</div>
                    <asp:Repeater ID="repViSinh" runat="server">
                        <ItemTemplate>
                            <div class="column chitieu-name text-left"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></div>
                            <div class="column phuongphap-name text-left"><%#Eval("TenPhuongPhap") %></div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div style="clear: both;"></div>
                </div>
            </div>
            <div class="row info" style="margin-top: 20px;">
                <table class="table table-bordered table-sm black">
                    <thead>
                        <tr>

                            <td style=" text-align: center; font-weight: bold; text-transform: uppercase;">
                                <div class="rezise">Chỉ tiêu</div>
                            </td>
                            <td style="text-align: center; font-weight: bold; text-transform: uppercase;">
                                <div class="rezise">Phương pháp</div>
                            </td>
                            <td style="text-align: center; width: 80px; font-weight: bold; text-transform: uppercase;display:none;">Đơn giá</td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repChiTieuViSinh" runat="server" OnItemDataBound="repChiTieuHoaLy_ItemDataBound">
                            <ItemTemplate>
                                <tr>

                                    <td style="text-align: left;"><%# GetTen(Eval("TenTaiCacKhoa"),Eval("Italic")) %></td>
                                    <td style="text-align: left;">
                                        <div contenteditable="true"><%#GetTen(Eval("TenPhuongPhap"),Eval("Italic")) %></div>
                                    </td>
                                    <%--<td style="text-align: right;"><%# Eval("DonGiaToString") %></td>--%>
                                    <asp:Literal ID="ltrLastColumnThanhTien" Visible="false" runat="server"></asp:Literal>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr style="display:none;">
                            <td  style="text-align: right; padding-right: 3px; font-weight: bold; text-transform: uppercase;">tổng cộng</td>
                            <td style="text-align: right; font-weight: bold;">
                                <asp:Literal ID="ltrTongCongViSinh" runat="server"></asp:Literal></td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="row" style="margin-top: 20px;">
                <table style="border-spacing: 0; border: solid 1px black; width: 100%;">
                    <tr>
                        <td style="width: 38%; border: solid 1px black; padding: 5px;">Tổng số chỉ tiêu:
                                <asp:Literal ID="ltrVSTongSoChiTieu" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 31%; border: solid 1px black; padding: 5px;">Vi sinh: 
                                <asp:Literal ID="ltrVSTongSoViSinh" runat="server"></asp:Literal>
                        </td>
                        <td style="width: 31%; border: solid 1px black; padding: 5px;">Hóa lý:
                                <asp:Literal ID="ltrVSTongSoHoaLy" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="row" style="margin-top: 10px;">
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    Người chuyển mẫu
                </div>
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    Labo Hóa
                </div>
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    Labo Vi sinh
                </div>
            </div>
            <div class="row" style="margin-top: 130px;">
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    <asp:Literal ID="ltrVSNguoiChuyenMau" runat="server"></asp:Literal>
                </div>
            </div>

            <div class=" paper-footer">
                <div style="padding: 4px 0px; border-bottom: solid 1px #000000;">
                    * Phiếu chuyển mẫu này được lập thành
                            <asp:Literal ID="LltrBanVS" runat="server"></asp:Literal>
                    bản, mỗi bộ phận lưu 1 bản
                </div>
                <div style="padding: 4px">
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">BM-TT-7.3&7.4-02</div>
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">Lần ban hành: 01</div>
                    <div class="col-lg-4 col-xs-4" style="text-align: center;">Ngày ban hành: 10/02/2020</div>
                </div>
            </div>

        </div>
        <script type="text/javascript">
            window.onload = function () { window.print(); }
        </script>
    </form>
</body>
</html>
