<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuKetQuaNoiBo.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.PhieuKetQuaNoiBo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In phiếu kết quả kiểm nghiệm nội bộ</title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/Printing.css" rel="stylesheet" />
    <script src="/scripts/jQuery-2.1.4.min.js"></script>
    <style type="text/css">
        .paper {
            padding: 1cm;
        }

        table.tbl-data td, table.tbl-data th {
            border: solid 1px black;
            padding: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paper" style="position: relative;">
            <div class="row" style="padding: 2px 0px 10px 0px; border-bottom: solid 2px black;">
                <div class="col-lg-6 col-xs-6">
                    <img src="../images/logo.png" width="95" height="95" style="padding: 5px; display: block; float: left;" />
                    <div style="float: left; width: 70%;">
                        <span style="display: block; text-align: center; text-transform: uppercase; font-size: 14px; font-weight: bold;">bộ y tế<br />
                            viện pasteur nha trang
                        </span>
                        <span style="border-bottom: solid 2px black; display: block; width: 120px; height: 2px; margin: 5px 55px;"></span>
                        <span style="display: block; text-align: center; text-transform: uppercase; font-size: 14px; font-weight: bold;">khoa sức khỏe môi trường
                            <br />
                            - bệnh nghề nghiệp
                        </span>
                    </div>
                </div>
                <div class="col-lg-6 col-xs-6" style="text-align: center;">
                    <span style="display: block; text-align: center; text-transform: uppercase; font-size: 14px; font-weight: bold;">cộng hòa xã hội chủ nghĩa việt nam
                    </span>
                    <span style="display: inline-block; text-align: center; font-size: 13px; font-weight: bold; padding-bottom: 5px; border-bottom: solid 1px black;">Độc lập - Tự do - Hạnh phúc
                    </span>
                    <span style="display: block; text-align: right; margin-top: 40px;">
                        <asp:Literal ID="ltrNgayThang" runat="server"></asp:Literal>
                    </span>
                </div>
            </div>
            <div class="row" style="padding: 10px 0px; border-bottom: solid 2px black;">
                <span style="text-align: center; text-transform: uppercase; font-size: 20px; font-weight: bold; display: block;">phiếu kết quả kiểm nghiệm nội bộ
                </span>
                <span style="text-align: center; font-size: 18px; font-weight: bold; display: block;">Số:
                    <asp:Literal ID="ltrMaMau" runat="server"></asp:Literal>
                </span>
            </div>
            <div class="row" style="padding-top: 20px">
                <div>
                    <span style="display: inline-block; width: 200px">1.Mã số mẫu:</span>
                    <b>
                        <asp:Literal ID="ltrMaSoMau" runat="server"></asp:Literal></b>
                </div>
                <div style="padding: 5px 0px;">
                    <span style="display: inline-block; width: 200px">2.Loại mẫu:</span>
                    <b>
                        <asp:Literal ID="ltrLoaiMau" runat="server"></asp:Literal></b>
                </div>
                <div style="padding: 5px 0px;">
                    <span style="display: inline-block; width: 200px">3.Điều kiện bảo quản:</span>
                    <b>
                        <asp:Literal ID="ltrDieuKienBaoQuan" runat="server"></asp:Literal></b>
                </div>
                <div style="padding: 5px 0px;">
                    <span style="display: inline-block; width: 200px">4.Ngày nhận mẫu:</span>
                    <b>
                        <asp:Literal ID="ltrNgayNhanMau" runat="server"></asp:Literal></b>
                </div>
                <div style="padding: 5px 0px;">
                    <span style="display: inline-block; width: 200px">5.Thời gian kiểm nghiệm:</span>
                    <b>
                        <asp:Literal ID="ltrThoiGianKiemNghiem" runat="server" Text="Từ ngày...........đến ngày..........."></asp:Literal></b>
                </div>
                <div style="padding: 5px 0px;">
                    <span style="display: inline-block; width: 200px">6.Thời gian lưu mẫu:</span>
                    <b>
                        <asp:Literal ID="ltrThoiGianLuuMau" runat="server"></asp:Literal></b>
                </div>
                <div style="padding: 5px 0px;">
                    <span style="display: inline-block; width: 200px">7.Kết quả kiểm nghiệm:</span>
                    Xem từ trang sau
                </div>
            </div>
            <div class="row" style="padding: 10px;">
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    <span style="display: block; text-transform: uppercase; text-align: center; font-weight: bold; font-size: 15px">tổ trưởng</span>
                    <span style="display: block; margin-top: 90px">
                        <asp:Literal ID="ltrToTruong" runat="server"></asp:Literal>
                    </span>
                </div>
                <div class="col-lg-4 col-xs-4"></div>
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    <span style="display: block; text-transform: uppercase; text-align: center; font-weight: bold; font-size: 15px">TRƯỞNG KHOA</span>
                    <span style="display: block; margin-top: 90px;font-family:'Times New Roman'">Hồ Văn Quốc</span>
                </div>
            </div>
        </div>

        <div class="paper" style="position: relative; page-break-after: always; margin-top: 20px;">
            <div style="text-transform: uppercase; font-size: 20px; font-weight: bold; text-align: center; margin: 10px;" contenteditable="true">
                kết quả kiểm nghiệm
            </div>
            <div id="divKetQuaThuong" runat="server" visible="false">
                <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                    <thead>
                        <tr>
                            <th rowspan="2" style="width: 30px; text-align: center;"><div class="rezise">STT</div></th>
                            <th rowspan="2" style="width: 150px; text-align: center;"><div class="rezise">Tên chỉ tiêu</div></th>
                            <th rowspan="2" style="text-align: center;"><div class="rezise">Phương pháp kiểm nghiệm</div></th>
                            <th rowspan="2" style="width: 100px; text-align: center;"><div class="rezise">Kết quả kiểm nghiệm</div></th>
                            <asp:Literal ID="ltrColumnHeaderGroup" runat="server"></asp:Literal>
                            <%--<th style="width: 150px; text-align: center;">Giới hạn cho phép <sup>(*)</sup></th>--%>
                        </tr>
                        <tr>
                            <asp:Literal ID="ltrColumnHeader" runat="server"></asp:Literal>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="repData" runat="server" OnItemDataBound="repData_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;"><%# ( Container.ItemIndex +1) %></td>
                                    <td style="text-align: left;"><%# GetTen(Eval("TenXuatKetQuaAB"),Eval("Italic")) %></td>
                                    <td style="text-align: center;"><%# Eval("TenPhuongPhapHienThi") %></td>
                                    <td style="text-align: center;"><%# GetKetQuaHienThi(Container.DataItem) %></td>
                                    <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                    <%--<td style="text-align: center;"><%#  GetGioiHanChoPhep(Container.DataItem) %></td>--%>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div id="divKetQuaCB" runat="server" visible="false">

                <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                    <thead>
                        <tr>
                            <th style="width: 50px; text-align: center;"><div class="rezise">STT / No</div></th>
                            <th style="width: 220px; text-align: center;"><div class="rezise">Tên chỉ tiêu / Parameter</div></th>
                            <th style="text-align: center;"><div class="rezise">Phương pháp kiểm nghiệm / Test method</div></th>
                            <th colspan="2" style="width: 100px; text-align: center;"><div class="rezise">Kết quả / Result</div></th>
                            <th style="width: 100px; text-align: center;"><div class="rezise">Giới hạn tối đa cho phép / Maximum limit (*)</div></th>
                        </tr>

                    </thead>
                    <tbody>
                        <asp:Repeater ID="repDataCB" runat="server" OnItemDataBound="repDataCB_ItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="ltrData" runat="server"></asp:Literal>

                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div id="divKetQuaM2" runat="server" visible="false">
                <asp:Literal ID="ltrDataM2" runat="server"></asp:Literal>
            </div>
            <div style="padding-top: 10px;">
                <span style="vertical-align: top; font-weight: bold; display: inline-table; width: 65px;">Ghi chú:</span>
                <span style="display: inline-table; width: 85%; padding-top: 20px;">
                    <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>


                </span>
            </div>
            <div>
                <span style="vertical-align: top; font-weight: bold; display: inline-table; width: 65px;">Nhận xét:</span>
                <span style="display: inline-table; width: 85%;">

                    <asp:Literal ID="ltrNhanXet" runat="server"></asp:Literal>

                    <%--Về phương diện hóa học, chỉ tiêu được phân tích trên đây của mẫu nước đạt giới hạn B chất lượng nước sinh hoạt theo QCVN 02:2009/BYT--%>
                </span>
            </div>
        </div>
        <script type="text/javascript">
            window.onload = function () { window.print(); }
        </script>
    </form>
</body>
</html>
