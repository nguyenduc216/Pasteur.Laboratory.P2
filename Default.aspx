<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Pasteur.Laboratory.P2.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%-- Code: DASHBOARD-PARALLEL-LOAD-20260918 - Hai endpoint tải độc lập; phần hoàn thành trước hiển thị trước. --%>
    <style type="text/css">
        .dashboard-section-frame { width: 100%; min-height: 120px; border: 0; display: none; }
        .dashboard-section-status { margin-bottom: 14px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="alert alert-block alert-warning">
        <i class="ace-icon fa fa-check green"></i>
        Chào mừng anh/chị đã đăng nhập vào hệ thống quản lý kết quả xét nghiệm của viện Pasteur Nha Trang.
    </div>
    <%-- Code: DASHBOARD-PARALLEL-LOAD-20260918 - Trạng thái riêng của thống kê. --%>
    <div id="statsStatus" class="alert alert-info dashboard-section-status">
        <i class="ace-icon fa fa-spinner fa-spin"></i> Thống kê sẽ bắt đầu tải sau 5 giây...
    </div>
    <iframe id="statsFrame" class="dashboard-section-frame" title="Thống kê chung"></iframe>

    <%-- Code: DASHBOARD-PARALLEL-LOAD-20260918 - Trạng thái riêng của nhật ký. --%>
    <div id="logStatus" class="alert alert-info dashboard-section-status">
        <i class="ace-icon fa fa-spinner fa-spin"></i> Nhật ký sẽ bắt đầu tải sau 5 giây...
    </div>
    <iframe id="logFrame" class="dashboard-section-frame" title="Lịch sử thao tác"></iframe>

    <script type="text/javascript">
        // Người sửa: Codex - 21/09/2026 - Mã: DASHBOARD-QUERY-TIMEOUT-20260921 - Tăng thời gian chờ tải thống kê và nhật ký từ 60 giây lên 5 phút.
        // Code: DASHBOARD-PARALLEL-LOAD-20260918 - Chạy song song và báo timeout riêng cho từng phần.
        (function () {
            function startSection(frameId, statusId, url, title) {
                var frame = document.getElementById(frameId);
                var status = document.getElementById(statusId);
                var completed = false;

                status.innerHTML = '<i class="ace-icon fa fa-spinner fa-spin"></i> Đang tải ' + title + '...';
                frame.onload = function () {
                    completed = true;
                    status.style.display = 'none';
                    frame.style.display = 'block';
                    try {
                        frame.style.height = Math.max(120, frame.contentWindow.document.documentElement.scrollHeight) + 'px';
                    } catch (e) { }
                };
                frame.src = url + '?v=' + new Date().getTime();

                window.setTimeout(function () {
                    if (!completed) {
                        status.className = 'alert alert-danger dashboard-section-status';
                        status.innerHTML = '<i class="ace-icon fa fa-exclamation-triangle"></i> Không thể tải ' + title +
                            ' sau 5 phút. Vui lòng kiểm tra kết nối database hoặc nhật ký lỗi trên server.';
                    }
                // Mã: DASHBOARD-QUERY-TIMEOUT-20260921 - Cho phép truy vấn chạy tối đa 5 phút trước khi thông báo quá thời gian chờ.
                }, 300000);
            }

            window.setTimeout(function () {
                startSection('statsFrame', 'statsStatus', '/DashboardStats.aspx', 'thống kê');
                startSection('logFrame', 'logStatus', '/DashboardLog.aspx', 'nhật ký');
            }, 5000);
        })();
    </script>
</asp:Content>
