<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuKetQua.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.PhieuKetQua" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In phiếu kết quả </title>
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
            /*position: fixed;*/
        }

        .row {
            /* padding: 0;
            margin:0;*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paper" style="position: relative;">
            <div class="mask"></div>
            <div class="row" style="padding: 2px 0px 10px 0px;">
                <div class="col-lg-12 col-xs-12">
                    <img src="../images/logo.png" width="90" height="90" style="margin-top: 32px; padding: 5px; display: block; float: left;" />
                    <div style="float: left; width: 64%; text-align: center;">
                        <div style="display: block; padding: 10px 0px 0px 5px; text-align: left; text-transform: uppercase; font-size: 16px;">
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; text-align: center;">Bộ y tế</p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; text-align: center;">viện pasteur nha trang</p>
                            <p style="line-height: 15px; width: 135px; border-bottom: solid 1px black; margin: 0 38%;"></p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; font-weight: bold; text-align: center; margin-top: 15px;">khoa sức khỏe môi trường - bệnh nghề nghiệp</p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; font-style: italic; text-align: center;">
                                DEPARTMENT OF ENVIRONMENT AND ACCUPATION HEALTH
                            </p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; font-style: italic; text-align: center; text-transform: none; font-size: 13px;">
                                Địa chỉ/ Address: 6-8-10 Trần Phú, Nha Trang, Khánh Hòa
                            </p>
                        </div>

                    </div>
                    <div style="margin-top: 5px; text-align: right; float: left; width: 23%;">
                        <img src="../images/print.result.centificate.PNG" width="200" height="125" />
                        <asp:Repeater ID="repImage" runat="server" OnItemDataBound="repImage_ItemDataBound" Visible="false">
                            <ItemTemplate>
                                <asp:Literal ID="ltrImage" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            <div class="row">
                <asp:Literal ID="ltrChungNhan" runat="server"></asp:Literal>
            </div>
            <div class="row">
                <div class="col-lg-3 col-xs-3" style="border: solid 1px black; text-align: center; height: 60px; padding: 7px 0px;">
                    Mã mẫu / Sample Code:<br />
                    <b>
                        <asp:Literal ID="ltrMaMau" runat="server"></asp:Literal></b>
                </div>
                <div class="col-lg-6 col-xs-6 no-padding" style="height: 60px; border: solid 1px black; border-left: 0px; padding: 7px 0px; border-right: 0px; text-transform: uppercase; text-align: center; font-size: 17px;">
                    <p style="line-height: 15px; margin: 0px; padding: 2px 0px; text-align: center; font-weight: bold;">phiếu kết quả kiểm nghiệm</p>
                    <p style="line-height: 15px; margin: 0px; padding: 2px 0px; text-align: center;">test report</p>
                    <p style="line-height: 15px; font-size: 14px; text-transform: none;">
                        Số/No:
                        <asp:Literal ID="ltrSo" runat="server"></asp:Literal>
                    </p>
                </div>
                <div class="col-lg-3 col-xs-3" style="border: solid 1px black; text-align: center; height: 60px; padding: 7px 0px;">
                    Ngày / Date:<br />
                    <asp:Literal ID="ltrNgay" runat="server"></asp:Literal>
                </div>
            </div>
            <div class="row" style="padding-top: 20px">
                <div class="col-lg-12">
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">1.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Khách hàng <span class="sub-title">/ Client</span></span>

                        :
                        <asp:Literal ID="ltrKhachHang" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px; display: none;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">2.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Mã số mẫu <span class="sub-title">(Sample Code)</span></span>
                        <b>:
                            <asp:Literal ID="ltrMaSoMau" runat="server"></asp:Literal></b>
                    </div>
                    <div style="padding: 5px 0px;">

                        <span style="display: inline-block; width: 270px; vertical-align: top;">Địa chỉ <span class="sub-title">/ Address </span></span>
                        :
                            <asp:Literal ID="ltrDiaChi" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">3.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Loại mẫu <span class="sub-title">/ Type of sample </span></span>

                        :
                        <asp:Literal ID="ltrLoaiMau" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">4.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Nơi lấy mẫu <span class="sub-title">/ Place of sampling</span></span>

                        :
                        <asp:Literal ID="ltrNoiLayMau" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">5.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Mô tả mẫu <span class="sub-title">/ Sample description </span></span>

                        :
                        <asp:Literal ID="ltrMieuTaMau" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px; display: none;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top;">6.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Điều kiện bảo quản mẫu 
                            <span class="sub-title">( Sample storage conditions)</span>
                        </span>
                        :
                        <asp:Literal ID="ltrDieuKienBaoQuan" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px; display: none;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top;">7.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Người lấy mẫu <span class="sub-title">( Person recevinging )</span>  </span>
                        :
                        <asp:Literal ID="ltrNguoiLayMau" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px; display: none;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top;">8.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Người gửi mẫu <span class="sub-title">( Person submission )</span>   </span>
                        :
                        <asp:Literal ID="ltrNguoiGui" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">9.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Ngày lấy mẫu <span class="sub-title">/ Date of sampling </span></span>
                        :
                        <asp:Literal ID="ltrNgayLayMau" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">10.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Ngày nhận mẫu <span class="sub-title">/ Sample received on</span>  </span>
                        :
                        <asp:Literal ID="ltrNgayNhanMau" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">11.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Ngày kiểm nghiệm  <span class="sub-title">/ Sample tested on</span>  </span>
                        :
                        <asp:Literal ID="ltrThoiGianKiemNghiem" runat="server" Text="Từ ngày...........đến ngày..........."></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">12.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Thời gian lưu mẫu  <span class="sub-title">/ Sample storage period</span> </span>
                        :
                        <asp:Literal ID="ltrThoiGianLuuTru" runat="server"></asp:Literal>
                    </div>
                    <div style="padding: 5px 0px; display: none;">
                        <span style="display: inline-block; margin-right: 2px; vertical-align: top;">13.</span>
                        <span style="display: inline-block; width: 270px; vertical-align: top;">Kết quả kiểm nghiệm<span class="sub-title"> (  Testing Results )</span></span>
                        :
                        <asp:Literal ID="ltrKetQuaThuNghiem" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>

            <div>
                <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                    <thead>
                        <tr>
                            <th rowspan="2" style="width: 50px; text-align: center;">STT / No</th>
                            <th rowspan="2" style="width: 150px; text-align: center;">Tên chỉ tiêu / Parameter</th>
                            <th rowspan="2" style="text-align: center;">Phương pháp kiểm nghiệm / Test method</th>
                            <th rowspan="2" style="width: 100px; text-align: center;">Kết quả / Result</th>
                            <%--<th style="width: 150px; text-align: center;">Giới hạn cho phép (*)</th>--%>
                            <asp:Literal ID="ltrColumnHeaderGroup" runat="server"></asp:Literal>
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
                                    <td style="text-align: center;"><%# Eval("TenPhuongPhapTLGOC") %></td>
                                    <td style="text-align: center;"><%# Eval("KetQuaHienThiToString") %></td>
                                    <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
            <div style="margin: 14px 0px 0px 10px;">
                <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px;">
                    (a): Chỉ tiêu được công nhận phù hợp với ISO/IEC 17025:2017/ Parameter is accredited ISO/IEC 17025:2017
                </p>
                <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px;">
                    (b): Chỉ tiêu được chứng nhận VIMCERT/ VIMCERT test certified by MONRE
                </p>
                <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px;">
                    <asp:Literal ID="ltrGhiChu" runat="server"></asp:Literal>
                    <asp:Literal ID="ltrNhanXet" runat="server"></asp:Literal>
                </p>
            </div>
            <div contenteditable="true" style="min-height: 20px;"></div>
            <div contenteditable="true" class="row" style="margin-top: 20px;">
                <div class="col-lg-5 col-xs-5" style="text-align: center;">
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">TRƯỞNG KHOA</span>
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">HEAD OF DEPARTMENT</span>
                    <span style="display: block; margin-top: 100px; font-weight: bold;"></span>

                </div>
                <div class="col-lg-2 col-xs-2" style="text-align: center;"></div>
                <div class="col-lg-5 col-xs-5" style="text-align: center;">
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">VIỆN TRƯỞNG </span>
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">DIRECTOR </span>

                </div>
            </div>
            <div style="clear: both;"></div>
            <div class="paper-footer">
                <div style="padding: 4px 0px; border-top: solid 1px #000000; font-size: 11px;">
                    <ol>
                        <li>Kết quả này chỉ có giá trị đối với mẫu được kiểm nghiệm./ This result is only valid on tested sample.</li>
                        <li>Không được trích sao một phần hay toàn bộ kết quả này nếu không có sự đồng ý bằng văn bản của Viện Pasteur Nha Trang./ This test report must not be reproduced, partially or fully, without the written approval of Pasteur Institute in Nha Trang.</li>
                        <li>Thông tin của khách hàng và mẫu do khách hàng cung cấp./ The sample information are written as client’s request.</li>
                    </ol>
                </div>
            </div>
        </div>

        <div class="paper hide" style="position: relative; page-break-after: always; display: none;">
            <div style="text-transform: uppercase; font-size: 20px; font-weight: bold; text-align: center; margin: 10px;">
                kết quả kiểm nghiệm
            </div>
            <div>
                <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                    <thead>
                        <tr>
                            <th rowspan="2" style="width: 30px; text-align: center;">STT</th>
                            <th rowspan="2" style="width: 150px; text-align: center;">Tên chỉ tiêu / Parameter</th>
                            <th rowspan="2" style="text-align: center;">Phương pháp kiểm nghiệm / Test method</th>
                            <th rowspan="2" style="width: 100px; text-align: center;">KQ kiểm nghiệm / Result</th>
                            <%--<th style="width: 150px; text-align: center;">Giới hạn cho phép (*)</th>--%>
                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        </tr>
                        <tr>

                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>

                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="repData_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td style="text-align: center;"><%# ( Container.ItemIndex +1) %></td>
                                    <td style="text-align: left;"><%# GetTen(Eval("TenXuatKetQua"),Eval("Italic")) %></td>
                                    <td style="text-align: center;"><%# Eval("TenPhuongPhap") %></td>
                                    <td style="text-align: center;"><%# Eval("KetQuaHienThiToString") %></td>
                                    <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>

            <div id="divVimcert" runat="server" visible="false" style="margin-top: 40px;">
                <div style="font-weight: bold; text-transform: uppercase; margin: 5px 0px 10px 0px; text-align: center;">
                    Chỉ tiêu xét nghiệm theo Vimcert
                </div>
                <div>
                    <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                        <thead>
                            <tr>
                                <th rowspan="2" style="width: 30px; text-align: center;">STT</th>
                                <th rowspan="2" style="width: 150px; text-align: center;">Tên chỉ tiêu</th>
                                <th rowspan="2" style="text-align: center;">Phương pháp kiểm nghiệm</th>
                                <th rowspan="2" style="width: 100px; text-align: center;">KQ kiểm nghiệm</th>
                                <%--<th style="width: 150px; text-align: center;">Giới hạn cho phép (*)</th>--%>
                                <asp:Literal ID="ltrColumnHeaderGroup1" runat="server"></asp:Literal>
                            </tr>
                            <tr>

                                <asp:Literal ID="ltrColumnHeader1" runat="server"></asp:Literal>

                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="repDataVimcert" runat="server" OnItemDataBound="repData_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;"><%# ( Container.ItemIndex +1) %></td>
                                        <td style="text-align: left;"><%# GetTen(Eval("TenXuatKetQua"),Eval("Italic")) %></td>
                                        <td style="text-align: center;"><%# Eval("TenPhuongPhapTLGOC") %></td>
                                        <td style="text-align: center;"><%# Eval("KetQuaHienThiToString") %></td>
                                        <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </div>
            <div style="padding-top: 10px;">
                <span style="vertical-align: top; font-weight: bold; display: inline-table; width: 65px;">Ghi chú</span>
                <span style="display: inline-table; width: 85%; padding-top: 20px;">
                    <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>

                </span>
            </div>
            <div>
                <span style="vertical-align: top; font-weight: bold; display: inline-table; width: 65px;">Nhận xét:</span>
                <span style="display: inline-table; width: 85%;"></span>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">TRƯỞNG KHOA</span>
                    <span style="display: block; margin-top: 100px; font-weight: bold;font-family:'Times New Roman'">Hồ Văn Quốc</span>

                </div>
                <div class="col-lg-4 col-xs-4" style="text-align: center;"></div>
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">Viện trưởng</span>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            window.onload = function () { setTimeout(window.print(), 1000); }
        </script>
    </form>
</body>
</html>

