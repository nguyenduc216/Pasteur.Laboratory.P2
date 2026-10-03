<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PhieuKetQuaNew2P.aspx.cs" Inherits="Pasteur.Laboratory.P2.Printing.PhieuKetQuaNew2P" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>In phiếu kết quả </title>
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/css/Printing.css" rel="stylesheet" />
    <script src="/scripts/jQuery-2.1.4.min.js"></script>
    <link href="../icons/AweIcon/aweicon.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Questrial&display=swap" rel="stylesheet" />
    <style type="text/css">
        .t-content {
            width: 390px !important;
        }

        /* @page {
            @bottom-center {
                content: 'Trang ' counter(page);
            }
        }*/

        @media print {
            .ftt {
                max-width: 100% !important;
            }

            .t-content {
                width: 480px !important;
            }

            .paper .mask {
                top: 20% !important;
                left: 17% !important;
                width: 600px;
                height: 600px;
                z-index: -1;
                background: #fff url(../images/logo.png) center center repeat;
                opacity: 0.1;
                position: fixed;
                background-size: auto 100%;
            }

            body {
                padding-bottom: 10mm !important;
                display: flex;
                flex-direction: column;
                min-height: 100vh;
            }

            thead {
                /*display: table-header-group*/
            }

            tfoot {
                display: table-row-group
            }

            tr {
                page-break-inside: avoid;
            }

            .print-phoi {
                display: none;
            }

            .tbl-data {
                /*  max-height: 400px;
                display: table-row;*/
            }

            .paper {
                padding: 0px !important;
                margin: 0cm 1cm 0px 1.5cm !important;
            }



            .paper-footer {
                width: 98% !important;
                background-color: #efefef;
                flex: 0 0 50px; /*or just height:50px;*/
                margin-top: auto;
            }

            .a-span {
                display: inline-block;
            }

            .a-link[href]:after {
                content: "";
            }

            body {
                margin-bottom: 20px;
            }

            table.tbl-data {
                page-break-inside: auto
            }

                /* div {
                page-break-inside: avoid;
            }*/
                /* This is the key */
                table.tbl-data thead {
                    display: table-header-group
                }

                table.tbl-data tfoot {
                    display: table-footer-group
                }
        }

        .bottom {
            position: fixed !important;
            bottom: 20px;
        }

        .a-span {
            display: none;
        }

        .paper {
            padding: 1cm;
        }

        body, html {
            font-family: 'Questrial' !important;
            font-size: 15px !important;
            display: flex;
            flex-direction: column;
            /*  align-items: center;*/
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
            width: 18cm;
            position: fixed;
            bottom: 0px;
        }

        .row {
            /* padding: 0;
            margin:0;*/
        }

        .print-phoi {
            position: fixed;
            left: 10px;
            top: 10px;
            width: 100px;
            height: 100px;
            border: solid 5px #cecece;
            border-radius: 50%;
            cursor: pointer;
            background: #0f4293;
        }

            .print-phoi i {
                font-size: 33px;
                display: block;
                text-align: center;
                color: #ffffff;
                padding-top: 10px;
            }

            .print-phoi span {
                display: block;
                text-align: center;
                color: white;
                font-size: 18px;
                font-weight: bold;
            }

        .resize {
            /*border:solid 2px #ff0000;*/
        }

        #pageHeader {
            display: none;
        }

        .visibilityd {
            visibility: hidden;
        }

        /*.main-table > thead {
            display: table-header-group;
        }

        .main-table > tfoot {
            display: table-footer-group;
        }

        table.report-container {
            page-break-after: always;
        }

        thead.report-header {
            display: table-header-group;
        }

        tfoot.report-footer {
            display: table-footer-group;
        }*/
        body {
            counter-reset: page;
        }

        @media print {
            /*#pageHeader {
                display: block;*/
            /*counter-increment: page;*/
            /*}*/

            /*#pageHeader:after {
                    content: counter(page);
                }*/
        }
    </style>
    <script type="text/javascript">
        function InPhoi() {
            $(".item-visiblity").addClass("visibilityd");

        }

        (function ($) {

            var parentOffset,
                item,
                overRight = false,
                newWidth,
                dragged = false;

            $.fn.dlResizeable = function (options) {

                /* var settings = $.extend({
                    
                     color: "#556b2f",
                     backgroundColor: "white"
                 }, options );*/

                item = this;

                $(document).mousedown(function (e) {
                    newWidth = parentOffset.relX - item.outerWidth();
                    if (overRight) {
                        dragged = true;
                    }
                });

                $(document).mouseup(function (e) {
                    dragged = false;
                });

                $(document).mousemove(function (e) {
                    parentOffset = item.offset();
                    var relX = e.pageX - parentOffset.left;
                    var relY = e.pageY - parentOffset.top;

                    var widthToAdd = 0;

                    /* check if mouse is above right border */
                    if (relX >= item.outerWidth() - 4 && relX <= $("#item").outerWidth()) {
                        item.css("cursor", "col-resize");
                        overRight = true;
                    } else {
                        item.css("cursor", "default");
                        overRight = false;
                    }
                    if (dragged) {
                        newWidth = relX - item.outerWidth();
                        item.width(item.outerWidth() + newWidth);
                    }

                });

                return this;
            }

        }(jQuery));

        $(document).ready(function (e) {
            //$(".resize").dlResizeable();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="paper" style="position: relative;">
            <%--<div style="position: fixed; top: 0px; right: 20px; border: solid 2px #0f4293; width: 230px; height: 20px;background-color:#0f4293;">Số ở đây</div>--%>
            <div class="mask item-visiblity"></div>
            <div class="print-phoi" onclick="InPhoi();"><i class="fa fa-print"></i><span>In Phôi</span></div>
            <%--  <table class=".report-container">

                <tbody>--%>
            <%-- <tr>
                        <td>--%>
            <div class="row" style="height: 5px; margin: 2px 0px; text-align: right; top: -5px; font-size: 10px; position: fixed; right: 0px; z-index: 1000;">
                <span id="pageHeader">KQ<asp:Literal ID="ltrPageHeader" runat="server"></asp:Literal>
                </span>
            </div>
            <div class="row " style="padding: 2px 0px 10px 0px; font-family: 'Times New Roman';">
                <div class="col-lg-12 col-xs-12 item-visiblity" style="padding: 0px;">
                    <img src="../images/logo.png" width="70" height="70" style="margin-top: 22px; padding: 5px; display: block; float: left; border-bottom: solid 3px #0f4293;" />
                    <div style="float: left; width: 73%; text-align: center; border-bottom: solid 2px #e0dddd; padding-bottom: 3px;">
                        <div style="display: block; padding: 10px 0px 0px 5px; text-align: left; text-transform: uppercase; font-size: 15px;">
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; text-align: center;">Bộ y tế</p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; text-align: center;"><b>viện pasteur nha trang</b></p>
                            <p style="line-height: 15px; width: 135px; border-bottom: solid 1px black; margin: 0 37%;"></p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; font-weight: bold; text-align: center; margin-top: 0px;">khoa sức khỏe môi trường - bệnh nghề nghiệp</p>
                            <p style="line-height: 15px; margin: 0px; padding: 2px 0px; font-style: italic; text-align: center;">
                                DEPARTMENT OF ENVIRONMENT AND ACCUPATION HEALTH
                            </p>

                        </div>

                    </div>
                    <div style="margin-top: 5px; text-align: right; float: left; width: 17%; border-bottom: solid 2px #e0dddd;">
                        <img src="../images/print.result.centificate.PNG" width="125" height="85" />
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
            <div class="resize">
                <div class="row" style="padding: 0px 0px 0px 15px;">

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
                        <div style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">1.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Khách hàng <span class="sub-title">/ Client</span></span>

                            <asp:Literal ID="ltrKhachHang" runat="server"></asp:Literal>
                        </div>
                        <div style="padding: 5px 0px; display: none;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">2.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Mã số mẫu <span class="sub-title">(Sample Code)</span></span>
                            <b>:
                            <asp:Literal ID="ltrMaSoMau" runat="server"></asp:Literal></b>
                        </div>
                        <div style="padding: 2px 0px;">

                            <span style="display: inline-block; width: 270px; vertical-align: top;">Địa chỉ <span class="sub-title">/ Address </span></span>

                            <asp:Literal ID="ltrDiaChi" runat="server"></asp:Literal>
                        </div>
                        <div style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">3.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Loại mẫu <span class="sub-title">/ Type of sample </span></span>

                            :
                        <asp:Literal ID="ltrLoaiMau" runat="server"></asp:Literal>
                        </div>
                        <div style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">4.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Nơi lấy mẫu <span class="sub-title">/ Place of sampling</span></span>


                            <asp:Literal ID="ltrNoiLayMau" runat="server"></asp:Literal>
                        </div>
                        <div style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">5.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Mô tả mẫu <span class="sub-title">/ Sample description </span></span>


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
                        <div style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">9.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Ngày lấy mẫu <span class="sub-title">/ Date of sampling </span></span>
                            :
                        <asp:Literal ID="ltrNgayLayMau" runat="server"></asp:Literal>
                        </div>
                        <div style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">10.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Ngày nhận mẫu <span class="sub-title">/ Sample received on</span>  </span>
                            :
                        <asp:Literal ID="ltrNgayNhanMau" runat="server"></asp:Literal>
                        </div>
                        <div contenteditable="true" style="padding: 2px 0px;">
                            <span style="display: inline-block; margin-right: 2px; vertical-align: top; display: none;">11.</span>
                            <span style="display: inline-block; width: 270px; vertical-align: top;">Ngày kiểm nghiệm  <span class="sub-title">/ Sample tested on</span>  </span>
                            :
                        <asp:Literal ID="ltrThoiGianKiemNghiem" runat="server" Text="Từ ngày...........đến ngày..........."></asp:Literal>
                        </div>
                        <div style="padding: 2px 0px;">
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

                <div id="divKetQuaThuong" runat="server" visible="false">
                    <div>
                        <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                            <thead>
                                <asp:Literal ID="ltrThongTin" runat="server" Visible="false"></asp:Literal>

                                <tr>
                                    <th rowspan="2" style="width: 50px; text-align: center;">STT / No</th>
                                    <th rowspan="2" style="width: 220px; text-align: center;">Tên chỉ tiêu / Parameter</th>
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
                                            <td contenteditable="true" style="text-align: center;"><%# GetPageIndex() %></td>
                                            <td style="text-align: left;"><%# GetTen(Eval("TenXuatKetQuaAB"),Eval("Italic")) %></td>
                                            <td style="text-align: center;"><%# Eval("TenPhuongPhap") %></td>
                                            <td style="text-align: center;"><%# GetKetQuaHienThi(Container.DataItem)%></td>
                                            <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div runat="server" id="divBM" class="item-visiblity" style="position: relative; margin-top: 30px;">
                        <div style="position: absolute; border-bottom: solid 3px #0f4293; background-color: white !important; top: -22px;">BM-TT-7.8</div>
                        <div contenteditable="true" style="padding: 4px 0px; border-top: solid 2px #e0dddd; font-size: 11px; background-color: white;">
                            <ol style="padding-left: 10px;">
                                <li>Kết quả này chỉ có giá trị đối với mẫu được kiểm nghiệm./ This result is only valid on tested sample.</li>
                                <li>Không được trích sao một phần hay toàn bộ kết quả này nếu không có sự đồng ý bằng văn bản của Viện Pasteur Nha Trang./ This test report must not be reproduced, partially or fully, without the written approval of Pasteur Institute in Nha Trang.</li>
                                <li>Thông tin của khách hàng và mẫu do khách hàng cung cấp./ The sample information are written as client’s request.</li>
                            </ol>
                        </div>
                    </div>
                    <div>
                        <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                            <tbody>
                                <asp:Repeater ID="repData2" runat="server" OnItemDataBound="repData_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td contenteditable="true" style="width: 50px; text-align: center;"><%# GetPageIndex() %></td>
                                            <td style="vertical-align: text-bottom; width: 220px; text-align: left;"><%# GetTen(Eval("TenXuatKetQuaAB"),Eval("Italic")) %></td>
                                            <td style="vertical-align: text-bottom; text-align: center;"><%# Eval("TenPhuongPhapHienThi") %></td>
                                            <td style="vertical-align: text-bottom; width: 100px; text-align: center;"><%# GetKetQuaHienThi(Container.DataItem) %></td>
                                            <asp:Literal ID="ltrGioiHanChoPhep" runat="server"></asp:Literal>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <div contenteditable="true" style="margin: 14px 0px 0px 10px; text-align: justify;">
                        <p style="font-weight: bold; margin-bottom: 0px;">Ghi chú:</p>
                        <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px; text-align: justify;">
                            <asp:Literal ID="ltrA" runat="server" Text="(a): Chỉ tiêu được công nhận phù hợp với ISO/IEC 17025:2017/ Parameter is accredited ISO/IEC 17025:2017" Visible="false"></asp:Literal>

                        </p>
                        <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px; text-align: justify;">
                            <%--(b): Chỉ tiêu được chứng nhận VIMCERTS/ VIMCERTS test certified by MONRE--%>
                            <asp:Literal ID="ltr1" runat="server" Text="(1): Chỉ tiêu được thực hiện bởi nhà thầu phụ/ Parameter is tested by subcontractor" Visible="false"></asp:Literal>

                        </p>
                        <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px; text-align: justify;">
                            <asp:Literal ID="ltrFooter" runat="server"></asp:Literal>
                        </p>
                        <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px; text-align: justify;">
                            <asp:Literal ID="ltrGhiChu" runat="server"></asp:Literal>
                            <asp:Literal ID="ltrNhanXet" runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>
                <div id="divKetQuaCB" runat="server" visible="false">
                    <table class="tbl-data" style="border: solid 1px black; width: 100%;">
                        <thead>
                            <tr>
                                <th style="width: 50px; text-align: center;">STT / No</th>
                                <th style="width: 220px; text-align: center;">Tên chỉ tiêu / Parameter</th>
                                <th style="text-align: center;">Phương pháp kiểm nghiệm / Test method</th>
                                <th colspan="2" style="width: 100px; text-align: center;">Kết quả / Result</th>
                                <th style="width: 100px; text-align: center;">Giới hạn tối đa cho phép / Maximum limit (*)</th>
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
                    <div contenteditable="true" style="margin: 14px 0px 0px 10px; text-align: justify;">

                        <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px; text-align: justify;">
                            <asp:Literal ID="ltrGhiChuM1" runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>
                <div id="divKetQuaM2" runat="server" visible="false">
                    <asp:Literal ID="ltrDataM2" runat="server"></asp:Literal>
                    <div contenteditable="true" style="margin: 14px 0px 0px 10px; text-align: justify;">

                        <p style="font-size: 13px; line-height: 15px; font-style: italic; margin: 0px; padding: 2px 0px; text-align: justify;">
                            <asp:Literal ID="ltrGhiChuM2" runat="server"></asp:Literal>
                        </p>
                    </div>
                </div>

                <div contenteditable="true" class="row" style="margin-top: 10px;">
                    <div class="col-lg-5 col-xs-5" style="text-align: center;">
                        <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">TRƯỞNG KHOA</span>
                        <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">HEAD OF DEPARTMENT</span>

                        <br />
                        <br />
                        <br />
                        <br />
                        <span style="font-weight: bold; font-size: 18px; font-family: 'Times New Roman'">Hồ Văn Quốc </span>
                    </div>
                    <div class="col-lg-2 col-xs-2" style="text-align: center;"></div>
                    <div class="col-lg-5 col-xs-5" style="text-align: center;">
                        <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">VIỆN TRƯỞNG </span>
                        <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">DIRECTOR </span>
                        <br />
                        <br />
                        <br />
                        <br />
                        <span style="font-weight: bold; font-size: 18px; font-family: 'Times New Roman'">Đỗ Thái Hùng </span>
                    </div>
                </div>
                <div style="clear: both;"></div>
            </div>
            <%--  </td>
                    </tr>--%>
            <%--</tbody>--%>
            <%-- <tfoot class="report-footer">
                    <tr>
                        <td>--%>

            <%-- </td>
                    </tr>

                </tfoot>
            </table>--%>
        </div>
        <div class="paper-footer " contenteditable="true" style="width: 100%; z-index: 1000; background-color: white !important;">
            <div class=" item-visiblity" style="background-color: white;">

                <div class="ftt" style="height: 35px; max-width: 755px; margin: 0px auto; background-color: #0f4293; text-align: center">
                    <img src="../images/print.result.footer.PNG" style="width: 100%;" />
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
                                    <td style="text-align: center;"><%# GetKetQuaHienThi(Container.DataItem) %></td>
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
                                        <td style="text-align: center;"><%# GetKetQuaHienThi(Container.DataItem) %></td>
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
                <span style="display: inline-table; width: 85%; padding-top: 20px;"></span>
            </div>
            <div>
                <span style="vertical-align: top; font-weight: bold; display: inline-table; width: 65px;">Nhận xét:</span>
                <span style="display: inline-table; width: 85%;"></span>
            </div>
            <div class="row" style="margin-top: 10px;">
                <div class="col-lg-4 col-xs-4" style="text-align: center;">
                    <span style="display: block; width: 100%; text-transform: uppercase; font-weight: bold;">TRƯỞNG KHOA</span>
                    <span style="display: block; margin-top: 100px; font-weight: bold; font-family: 'Times New Roman'">Hồ Văn Quốc</span>

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

