<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickReview.ascx.cs" Inherits="Pasteur.Laboratory.P2.Controls.QuickReview" %>
<style type="text/css">
    .infobox-data-number {
        letter-spacing: -2px !important;
    }
</style>
<div class="widget-box">
    <div class="widget-header">
        <h4 class="widget-title lighter smaller">
            <i class='ace-icon fa fa-bar-chart'></i>
            Thống kê chung
                    </h4>
        <div class="widget-toolbar no-border">
            <a href="#" data-action="collapse">
                <i class="ace-icon fa fa-chevron-down"></i>
            </a>
        </div>
    </div>
    <div class="widget-body">
        <div class="widget-main">
            <div class="row infobox-container">
                <div class="col-lg-12 col-xs-12">
                    <asp:Literal ID="ltrHtml" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
</div>
<script src="/themes/ace/script/jquery.sparkline.min.js"></script>
<script src="/themes/ace/script/jquery.easypiechart.min.js"></script>
<script type="text/javascript">
    $(function () {
        DashBoardInit();
        addRequestHanlde(DashBoardInit);
    });
    function DashBoardInit() {
        function drawPieChart(placeholder, data, position) {
            $.plot(placeholder, data, {
                series: {
                    pie: {
                        show: true,
                        tilt: 0.8,
                        highlight: {
                            opacity: 0.25
                        },
                        stroke: {
                            color: '#fff',
                            width: 2
                        },
                        startAngle: 2
                    }
                },
                legend: {
                    show: true,
                    position: position || "ne",
                    labelBoxBorderColor: null,
                    margin: [-30, 15]
                }
                ,
                grid: {
                    hoverable: true,
                    clickable: true
                }
            })
        };
        var placeholder = $('#piechart-placeholder').css({ 'width': '90%', 'min-height': '150px' });
        var data = [
            { label: "Trong nước", data: parseFloat($("[id$='hiddenTN']").val()), color: "#68BC31" },
            { label: "Quốc tế", data: parseFloat($("[id$='hiddenQT']").val()), color: "#2091CF" },
            { label: "Việt Kiều", data: parseFloat($("[id$='hiddenVK']").val()), color: "#AF4E96" }
        ];
        drawPieChart(placeholder, data);
    };
</script>

