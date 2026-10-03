<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TraMau.ascx.cs" Inherits="Pasteur.Laboratory.P2.Controls.TraMau" %>
<asp:UpdatePanel ID="panelUpdate" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Nhận của ông / bà</span>
                    <SweetSoft:ExtraTextBox ID="txtThuTienHoVaTen" runat="server" PlaceHolder="Tên người đóng tiền"></SweetSoft:ExtraTextBox>
                </div>
            </div>
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Số điện thoại</span>
                    <SweetSoft:ExtraTextBox ID="txtThuTienSoDienThoai" runat="server" PlaceHolder="Số điện thoại người đóng tiền"></SweetSoft:ExtraTextBox>
                </div>
            </div>
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Người thu tiền</span>
                    <SweetSoft:ExtraTextBox ID="txtNguoiThuTien" runat="server" PlaceHolder="Tên người thu tiền"></SweetSoft:ExtraTextBox>
                </div>
            </div>
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Ngày thu tiền</span>
                    <SweetSoft:ExtraDateTimePicker ID="dateNgayThuTien" Width="100%" DateTimeMask="Date" runat="server" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Đơn vị:</span>
                    <SweetSoft:ExtraTextBox ID="txtThuTienDonVi" runat="server" PlaceHolder="Tên đơn vị"></SweetSoft:ExtraTextBox>
                    <asp:HiddenField ID="hiddenMauID" runat="server" />
                </div>
            </div>
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Mã số thuế:</span>
                    <SweetSoft:ExtraTextBox ID="txtMaSoThue" runat="server" PlaceHolder="Mã số thuế"></SweetSoft:ExtraTextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                    <span class="extra-label">Địa chỉ</span>
                    <SweetSoft:ExtraTextBox ID="txtDiaChi" runat="server" PlaceHolder="Địa chỉ" TextMode="MultiLine" Height="70px"></SweetSoft:ExtraTextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-6 col-xs-6">
                <div class="form-group">
                    <span class="extra-label">Số hóa đơn:</span>
                    <SweetSoft:ExtraTextBox ID="txtSoHoaDon" runat="server" PlaceHolder="Số hóa đơn"></SweetSoft:ExtraTextBox>
                </div>
            </div>
            <div class="col-lg-6 col-xs-6">
                <div class="form-group" style="padding-top:35px;">
                    <SweetSoft:ExtraCheckBox ID="chkGhiNo" runat="server"></SweetSoft:ExtraCheckBox>
                    <span style="margin-left:5px;">Khách hàng ghi nợ hóa đơn</span>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

