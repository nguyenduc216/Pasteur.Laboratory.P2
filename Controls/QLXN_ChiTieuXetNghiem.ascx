<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QLXN_ChiTieuXetNghiem.ascx.cs" Inherits="Pasteur.Laboratory.Controls.QLXN_ChiTieuXetNghiem" %>
<div class="row">
    <div class="col-lg-12">
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Tên thông thường (*)</span>
                <SweetSoft:ExtraTextBox ID="txtTenThongThuong" runat="server" PlaceHolder="Nhập tên thông thường" Required="true"></SweetSoft:ExtraTextBox>
            </div>
        </div>
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Tên tại khoa (*)</span>
                <SweetSoft:ExtraTextBox ID="txtTenTaiKhoa" runat="server" PlaceHolder="Nhập tên tại khoa" Required="true"></SweetSoft:ExtraTextBox>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Tên trong TTQĐ</span>
                <SweetSoft:ExtraTextBox ID="txtTenTrongThongTu" runat="server" PlaceHolder="Nhập tên trong thông tư" Required="true"></SweetSoft:ExtraTextBox>
            </div>
        </div>
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">STT</span>
                <SweetSoft:ExtraTextBox ID="txtSTT" runat="server" PlaceHolder="Nhập STT" Required="false"></SweetSoft:ExtraTextBox>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Đơn vị tính (*)</span>
                <SweetSoft:ExtraComboBox ID="cbDonViTinh" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Chọn đơn vị tính"></SweetSoft:ExtraComboBox>
            </div>
        </div>
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Quy trình (*)</span>
                <SweetSoft:ExtraComboBox ID="cbQuyChuan" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Chọn quy chuẩn"></SweetSoft:ExtraComboBox>
            </div>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-lg-12">
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Phương pháp (*)</span>
                <SweetSoft:ExtraComboBox ID="cbPhuongPhap" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Chọn phương pháp"></SweetSoft:ExtraComboBox>
                <asp:HiddenField ID="hiddenEditID" runat="server" />
            </div>
        </div>
        <div class="col-lg-6 col-xs-12">
            <div class="form-group">
                <span class="label">Trạng thái sử dụng</span>
                <SweetSoft:ExtraCheckBox ID="chkTrangThaiSuDung" runat="server" DataOn="Có" DataOff="Không" Checked="true" CheckboxType="Switch7"></SweetSoft:ExtraCheckBox>
            </div>
        </div>
    </div>
</div>
