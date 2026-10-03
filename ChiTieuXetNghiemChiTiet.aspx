<%@ Page Title="" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="ChiTieuXetNghiemChiTiet.aspx.cs" Inherits="Pasteur.Laboratory.P2.ChiTieuXetNghiemChiTiet" %>

<%@ Register Src="~/Controls/Common_Paging.ascx" TagName="Paging" TagPrefix="SweetSoft" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        tr.edit {
            background-color: #e2baf7 !important;
        }

            tr.edit td {
            }

        .w81 {
            width: 81px !important;
            display: inline-table !important;
        }

        .extra-label {
            display: block;
        }

        .keyboard {
            display: inline-block;
            padding: 2px 3px;
            border-radius: 2px;
            border: solid 1px #a0a4ab;
            background-color: #e4e6e9;
            font-size: 8px;
            color: #610a8e;
            min-width: 30px;
            font-style: italic;
            text-align: center;
        }

            .keyboard.arrow {
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updateContent" runat="server">
        <ContentTemplate>

            <pre style="display: none; padding: 14px 0px 0px 0px; vertical-align: text-bottom; line-height: unset; height: 54px; overflow: hidden;">
                    <span class="keyboard">ctrl</span> + <span class="keyboard arrow">&#8593;</span> : Số trên  <span class="keyboard">ctrl</span> + <span class="keyboard arrow">&#8595;</span> : Số dưới <span class="keyboard">esc</span>: Về trạng thái cũ
                </pre>

            <div class="widget-box">
                <div class="widget-header">
                    <h5 class="widget-title bigger lighter">Thông tin chỉ tiêu xét nghiệm
                            
                        <asp:Literal ID="headerTitle" runat="server"></asp:Literal></h5>

                    <div class="widget-toolbar no-border">
                        <SweetSoft:ExtraButton ID="btnSave" Transparent="true" runat="server" EIcon="A_Save" Text="Lưu thông tin" OnClick="btnSave_Click">
                        </SweetSoft:ExtraButton>
                        <SweetSoft:ExtraButton ID="btnAddNew" Transparent="true" runat="server" EIcon="A_Add" Text="Thêm mới" OnClick="btnAddNew_Click">
                        </SweetSoft:ExtraButton>
                        <asp:HiddenField ID="hiddenEditID" runat="server" />

                    </div>
                </div>
                <div class="widget-body">
                    <div class="widget-main ">

                        <div class="row">
                            <div class="col-lg-12">
                                <div class="col-lg-5 col-xs-12">
                                    <div class="form-group">

                                        <span class="extra-label">Tên tại khoa (*)</span>
                                        <SweetSoft:ExtraTextBox ID="txtTenTaiKhoa" runat="server" Visible="false" PlaceHolder="Nhập tên tại khoa" Required="true"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraMathTextbox ID="txtMTenTaiKhoa" runat="server" PlaceHolder="Nhập tên tại khoa" Visible="true" />
                                    </div>
                                </div>
                                <div class="col-lg-5 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Tên trong TTQĐ (*)</span>
                                        <SweetSoft:ExtraMathTextbox ID="txtMTenTrongThongTu" runat="server" PlaceHolder="Nhập tên trong thông tư" Visible="true" />
                                        <SweetSoft:ExtraTextBox ID="txtTenTrongThongTu" Visible="false" runat="server" PlaceHolder="Nhập tên trong thông tư" Required="true"></SweetSoft:ExtraTextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Hóa / Vi sinh</span>
                                        <SweetSoft:ExtraComboBox ID="cbHoaSinh" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Chọn hóa/ sinh">
                                            <asp:ListItem Value="HoaLy" Text="Hóa sinh" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="ViSinh" Text="Vi Sinh"></asp:ListItem>
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>

                                <div class="col-lg-2 col-xs-12" style="display: none;">
                                    <div class="form-group">
                                        <span class="extra-label">STT</span>
                                        <SweetSoft:ExtraTextBox ID="txtSTT" runat="server" PlaceHolder="Nhập STT" Required="false"></SweetSoft:ExtraTextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12" style="display: none;">
                                    <div class="form-group">
                                        <span class="extra-label">Đơn giá</span>
                                        <SweetSoft:ExtraTextBox ID="txtDonGia" Text="0" runat="server" PlaceHolder="Đơn giá" Required="false" ValidationType="Currency"></SweetSoft:ExtraTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">

                                <div class="col-lg-3 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Nhóm xét nghiệm</span>
                                        <SweetSoft:ExtraComboBox ID="cbNhomXetNghiem" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Không chọn xét nghiệm">
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Hình thức giới hạn</span>
                                        <SweetSoft:ExtraComboBox ID="cbHinhThucGioiHanChuan" runat="server" AutoPostBack="true" ComboStyle="Dropdown_White" OnSelectedIndexChanged="cbHinhThucGioiHan_SelectedIndexChanged">

                                            <asp:ListItem Value="NhapSo" Text="Nhập số" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="ChonGiaTri" Text="Chọn giá trị"></asp:ListItem>
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12 no-padding-left">
                                    <div class="form-group">
                                        <span class="extra-label">Đơn vị tính</span>
                                        <SweetSoft:ExtraComboBox ID="cbDonViTinh" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Không chọn đơn vin tính"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12 no-padding-left">
                                    <div class="form-group">
                                        <span class="extra-label">Đơn vị tính tiền (*)</span>
                                        <SweetSoft:ExtraComboBox ID="cbDonViTinhTien" runat="server" ComboStyle="Dropdown_White" EmptyMessage="Chọn đơn vị tính tiền"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-1 col-xs-12 no-padding-left">
                                    <div class="form-group">
                                        <span class="extra-label">TT sử dụng</span>
                                        <SweetSoft:ExtraCheckBox ID="chkTrangThaiSuDung" runat="server" DataOn="Có" DataOff="Không" Checked="true" CheckboxType="Switch7"></SweetSoft:ExtraCheckBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12 no-padding-left">
                                    <div class="form-group">
                                        <span class="extra-label">In nghiêng tên</span>
                                        <SweetSoft:ExtraCheckBox ID="chkInNghieng" runat="server" DataOn="Có" DataOff="Không" CheckboxType="Switch7"></SweetSoft:ExtraCheckBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12" style="display: none;">
                                    <div class="form-group">
                                        <span class="extra-label">Giá trị bé</span>
                                        <%--ád--%>
                                        <SweetSoft:ExtraTextBox ID="txtGiaTriBe" Text="0" runat="server" PlaceHolder="Đơn giá" Required="false"></SweetSoft:ExtraTextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="display: none;">
                            <div class="col-lg-12">

                                <div class="col-lg-2 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Số thông tư</span>
                                        <SweetSoft:ExtraTextBox ID="txtSoThongTu" runat="server" PlaceHolder="Số thông tư " Required="false"></SweetSoft:ExtraTextBox>
                                    </div>
                                </div>
                                <div class="col-lg-2 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Ngày thông tư</span>
                                        <SweetSoft:ExtraDateTimePicker ID="dateNgayThongTu" DateTimeMask="Date" runat="server" Width="100%" PlaceHolder="Ngày thông tư" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divDetail" runat="server" style="margin-top: 20px">
                <asp:HiddenField ID="hiddenTab" runat="server" Value="Nhom" />
                <ul class="nav nav-tabs" id="myTab">
                    <li class='<%= GetTabVisible("Nhom","") %>'>
                        <a data-toggle="tab" onclick="SetTab('Nhom')" href="#nhom" aria-expanded="false">Chứng chỉ xét nghiệm / Phương pháp</a>
                    </li>
                    <li class='<%= GetTabVisible("QuyChuan","") %>'>
                        <a data-toggle="tab" onclick="SetTab('QuyChuan')" href="#quyChuan" aria-expanded="true">Quy chuẩn XN</a>
                    </li>
                </ul>
                <div class="tab-content">
                    <div id="quyChuan" class='<%= GetTabVisible("QuyChuan","tab-pane") %>'>
                        <div class="row">
                            <div class="col-lg-4 col-xs-12 no-padding-right">
                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Hình thức giới hạn</span>
                                        <SweetSoft:ExtraComboBox ID="cbHinhThucGioiHan" runat="server" AutoPostBack="true" ComboStyle="Dropdown_White" OnSelectedIndexChanged="cbHinhThucGioiHan_SelectedIndexChanged">
                                            <asp:ListItem Value="NhapSo" Text="Nhập số" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="ChonGiaTri" Text="Chọn giá trị"></asp:ListItem>
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>

                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Giới hạn 1</span>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan1" runat="server" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>

                                        <SweetSoft:ExtraTextBox ID="txtGioiHan1P" runat="server" Style="padding-left: 5px;" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraComboBox ID="cbGioiHan1" ComboStyle="Dropdown_White" Visible="false" runat="server" EmptyMessage="Chọn giới hạn"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Quy chuẩn</span>
                                        <SweetSoft:ExtraComboBox ID="cbQuyChuan" runat="server" ComboStyle="Dropdown_White"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Giới hạn 2</span>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan2" runat="server" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan2P" runat="server" Style="padding-left: 5px;" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraComboBox ID="cbGioiHan2" ComboStyle="Dropdown_White" Visible="false" runat="server" EmptyMessage="Chọn giới hạn"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Đơn vị tính</span>
                                        <SweetSoft:ExtraComboBox ID="cbDonViTinhQuyChuan" runat="server" EmptyMessage="Chọn đơn vị tính" ComboStyle="Dropdown_White">
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Giới hạn 3</span>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan3" runat="server" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan3P" runat="server" Style="padding-left: 5px;" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraComboBox ID="cbGioiHan3" ComboStyle="Dropdown_White" Visible="false" runat="server" EmptyMessage="Chọn giới hạn"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">KQ đạt/không đạt</span>
                                        <SweetSoft:ExtraComboBox ID="cbGiaTriDat" runat="server" ComboStyle="Dropdown_White">
                                            <asp:ListItem Value="1" Text="Đạt"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Không đạt"></asp:ListItem>
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-xs-12 ">
                                    <div class="form-group">
                                        <span class="extra-label">Giới hạn 4</span>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan4" runat="server" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraTextBox ID="txtGioiHan4P" runat="server" Style="padding-left: 5px;" PlaceHolder="Nhập số" CssClass="mask-float w81"></SweetSoft:ExtraTextBox>
                                        <SweetSoft:ExtraComboBox ID="cbGioiHan4" ComboStyle="Dropdown_White" Visible="false" runat="server" EmptyMessage="Chọn giới hạn"></SweetSoft:ExtraComboBox>
                                    </div>
                                </div>

                                <div class="col-lg-8 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Phép so sánh</span>
                                        <SweetSoft:ExtraComboBox ID="cbPhepSoSanh" runat="server" ComboStyle="Dropdown_White">
                                            <asp:ListItem Value="LHGH" Text="Lớn hơn các giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="LHBGH" Text="Lớn hơn bằng các giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="BHGH" Text="Bé hơn các giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="BHBGH" Text="Bé hơn bằng các giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="TKGH" Text="Trong khoảng các giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="TKHBGH" Text="Trong khoảng bằng các giới hạn"></asp:ListItem>
                                            <asp:ListItem Value="BGH" Text="Bằng các giới hạn"></asp:ListItem>
                                        </SweetSoft:ExtraComboBox>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Hiển thị dấu</span>
                                        <SweetSoft:ExtraCheckBox ID="chkHienThiDau" runat="server"></SweetSoft:ExtraCheckBox>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-xs-12">
                                    <div class="form-group">
                                        <span class="extra-label">Giá trị dấu bé</span>
                                        <SweetSoft:ExtraTextBox ID="txtGiaTriDauBe" Text="0" runat="server" PlaceHolder="Đơn giá" Required="false"></SweetSoft:ExtraTextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-1 col-xs-12" style="padding-top: 80px;">

                                <asp:LinkButton ID="lnkChiTieuQuyChuan" CssClass="btn  btn-success" runat="server" OnClick="btnSaveChiTieuQuyChuan_Click">
                                    <i class="fa fa-arrow-right"></i>
                                </asp:LinkButton><br />
                                <br />
                                <asp:LinkButton ID="lnkCanCelChiTieuQuyChuan" Visible="false" CssClass="btn  btn-danger" runat="server" OnClick="lnkCanCelChiTieuQuyChuan_Click">
                                    <i class="fa fa-close"></i>
                                </asp:LinkButton>
                                <asp:HiddenField ID="hiddenChiTieuQuyChuanID" runat="server" />
                            </div>
                            <div class="col-lg-7 col-xs-12 no-sort">
                                <div id="divGridEmpty" visible="false" runat="server">
                                    <table class="table dataTable  table-bordered   dtr-inline" cellspacing="0" rules="all" showheaderwhenempty="true" data-fix-header="true" data-sorting="false" border="1" style="width: 100%; border-collapse: collapse; display: table;" role="grid">
                                        <thead>
                                            <tr role="row">
                                                <th scope="col" abbr="STT2" style="width: 20px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="STT: activate to sort column ascending">STT</th>
                                                <th scope="col" abbr="STT2" style="width: 150px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Tên quy chuẩn: activate to sort column ascending">Tên quy chuẩn</th>
                                                <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Giới hạn 1 : activate to sort column ascending">Giới hạn 1 </th>
                                                <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Giới hạn 2 : activate to sort column ascending">Giới hạn 2 </th>
                                                <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Giới hạn 3 : activate to sort column ascending">Giới hạn 3 </th>
                                                <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Giới hạn 4 : activate to sort column ascending">Giới hạn 4 </th>
                                                <th scope="col" abbr="STT2" style="width: 150px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Đạt/không đạt : activate to sort column ascending">Đạt/không đạt </th>
                                                <th scope="col" abbr="STT2" style="width: 150px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Đơn vị tính : activate to sort column ascending">Đơn vị tính </th>
                                                <th align="center" scope="col" style="width: 100px;" class="sorting" tabindex="0" aria-controls="ctl00_ContentPlaceHolder1_grvChiTieuQuyChuan" rowspan="1" colspan="1" aria-label="Thao tác: activate to sort column ascending">Thao tác</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr role="row" class="odd">
                                                <td colspan="9">
                                                    <center>Chưa có dữ liệu quy chuẩn</center>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <SweetSoft:ExtraGrid Width="100%" ID="grvChiTieuQuyChuan" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                                    runat="server" AllowPaging="false" ShowHeaderWhenEmpty="true" AllowSorting="false" OnRowCreated="grvChiTieuQuyChuan_RowCreated" PagingControlName="paging">
                                    <EmptyDataTemplate>
                                        Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%# GetIndex(grvPhuongPhap, Container.DataItemIndex)  %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tên quy chuẩn" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="20px">
                                            <ItemTemplate>
                                                <%# Eval("TenQuyChuan") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Giới hạn 1 " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# GetGioiHan(Container.DataItem,1) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Giới hạn 2 " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# GetGioiHan(Container.DataItem,2) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Giới hạn 3 " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# GetGioiHan(Container.DataItem,3) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Giới hạn 4 " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <%# GetGioiHan(Container.DataItem,4) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Đạt/không đạt " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <%# GetTrangThaiDat(Container.DataItem) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ĐVT " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <%# Eval("TenDonViQuyChuan") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Giá trị bé " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px">
                                            <ItemTemplate>
                                                <%# GetDecimalToString( Eval("GiaTriBe")) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Thao tác" AccessibleHeaderText="" HeaderStyle-Width="100px" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="">
                                            <ItemTemplate>
                                                <div class="hidden-sm hidden-xs btn-group">
                                                    <SweetSoft:ExtraButton ID="btnEditChiTieuQuyChuan" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" OnClick="btnEditChiTieuQuyChuan_Click" CommandArgument='<%# Eval("ID") %>'>
                                                    </SweetSoft:ExtraButton>
                                                    <SweetSoft:ExtraButton ID="btnDelete" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="tieuChi_confirm">
                                                    </SweetSoft:ExtraButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </SweetSoft:ExtraGrid>
                            </div>
                        </div>
                    </div>
                    <div id="nhom" class='<%= GetTabVisible("Nhom","tab-pane") %>'>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="widget-box">
                                    <div class="widget-header">
                                        <h4 class="widget-title">Chứng chỉ / chứng nhận</h4>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main no-sort">
                                            <SweetSoft:ExtraGrid Width="100%" ID="grvQuyChuan" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                                                runat="server" AllowPaging="false" ShowHeaderWhenEmpty="true" AllowSorting="false" OnRowDataBound="grvQuyChuan_RowDataBound" PagingControlName="paging">
                                                <EmptyDataTemplate>
                                                    Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Tên quy chuẩn" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <%# Eval("Ten") %>
                                                            <asp:HiddenField ID="hiddenQuyChuanID" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Chứng chỉ / chứng nhận" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                                        <ItemTemplate>
                                                            <asp:Repeater ID="repDataChungChi" runat="server" OnItemDataBound="repDataChungChi_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkItem" runat="server" />
                                                                    <asp:HiddenField ID="hiddenChungChiEditID" runat="server" />
                                                                    <asp:HiddenField ID="hiddenChiTietID" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tần suất" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="400px" ItemStyle-Width="400px">
                                                        <ItemTemplate>
                                                            <asp:Repeater ID="repDataTanSuat" runat="server" OnItemDataBound="repDataTanSuat_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkItem" runat="server" />
                                                                    <asp:HiddenField ID="hiddenTanSuatEditID" runat="server" />
                                                                    <asp:HiddenField ID="hiddenChiTietID" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </SweetSoft:ExtraGrid>
                                            <SweetSoft:ExtraGrid Width="100%" ID="grvChungNhan" Visible="false" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                                                runat="server" AllowPaging="false" ShowHeaderWhenEmpty="true" AllowSorting="false" OnRowCreated="grvChungNhan_RowCreated" PagingControlName="paging">
                                                <EmptyDataTemplate>
                                                    Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                                                </EmptyDataTemplate>
                                                <Columns>

                                                    <asp:TemplateField HeaderText="" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <SweetSoft:ExtraCheckBox ID="chkAll" runat="server" ValidationGroup="chk-all-chungchi" GroupName="chk-all-chungchi" EnableClickGroup="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <SweetSoft:ExtraCheckBox ID="chkItem" runat="server" CssClass="chk-all-chungchi" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tên chứng nhận" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hiddenID" runat="server" />
                                                            <asp:HiddenField ID="hiddenChungNhanID" runat="server" />
                                                            <asp:HiddenField ID="hiddenName" runat="server" />
                                                            <%# Eval("Ten") %><br />
                                                            <br />
                                                            <img src='<%# Eval("ImagePath") %>' style="width: 120px; height: 90px;" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </SweetSoft:ExtraGrid>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-6 col-md-6">

                                <div class="widget-box">
                                    <div class="widget-header">
                                        <h4 class="widget-title">Phương pháp xét nghiệm</h4>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main no-sort">
                                            <div id="divPhuongPhapFake" visible="false" runat="server">
                                                <table class="table dataTable  table-bordered   dtr-inline" cellspacing="0" rules="all" showheaderwhenempty="true" data-fix-header="true" data-sorting="false" border="1" style="width: 100%; border-collapse: collapse; display: table;" role="grid">
                                                    <thead>
                                                        <tr role="row">
                                                            <th scope="col" abbr="STT2" style="width: 20px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label=" column ascending">STT</th>
                                                            <th scope="col" abbr="STT2" style="width: 150px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label=" column ascending">Tên Thông tư</th>
                                                            <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label="column ascending">Tên phương pháp </th>
                                                            <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label="column ascending">Mục </th>
                                                            <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label="column ascending">Đơn giá </th>
                                                            <th align="center" scope="col" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label=" column ascending">
                                                                <div class="hidden-sm hidden-xs btn-group">
                                                                    <SweetSoft:ExtraButton ID="ExtraButton1" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Add" OnClick="btnAddNewPhuongPhap_Click">
                                                                    </SweetSoft:ExtraButton>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr role="row" class="odd">
                                                            <td colspan="6">
                                                                <center>Chưa có dữ liệu phương pháp xét nghiệm</center>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <SweetSoft:ExtraGrid Width="100%" ID="grvPhuongPhap" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                                                runat="server" AllowPaging="false" ShowHeaderWhenEmpty="true" AllowSorting="false"
                                                OnRowCancelingEdit="grvPhuongPhap_RowCancelingEdit"
                                                OnRowUpdating="grvPhuongPhap_RowUpdating"
                                                OnRowDataBound="grvPhuongPhap_RowDataBound"
                                                OnRowCreated="grvPhuongPhap_RowCreated"
                                                PagingControlName="paging">
                                                <EmptyDataTemplate>
                                                    Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%# GetIndex(grvPhuongPhap, Container.DataItemIndex)  %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tên thông tư" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <%# Eval("TenThongTu") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="hdfEditID" runat="server" />
                                                            <SweetSoft:ExtraComboBox Width="150px" ID="cbEditThongTu" runat="server" EmptyMessage="Chọn thông tư"></SweetSoft:ExtraComboBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tên phương pháp" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <%# Eval("TenPhuongPhapToString") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <SweetSoft:ExtraComboBox Width="300px" CssClass="phuong-phap-dropdown" ID="cbEditPhuongPhap" runat="server" EmptyMessage="Chọn phương pháp"></SweetSoft:ExtraComboBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mục" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <%# Eval("STT") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <SweetSoft:ExtraTextBox ID="txtEditSTT" runat="server" PlaceHolder="Nhập STT" Style="width: 120px !important;"></SweetSoft:ExtraTextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Đơn giá" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="70px" ItemStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <%# GetDecimalToString( Eval("DonGia")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <SweetSoft:ExtraTextBox ID="txtEditDonGia" runat="server" PlaceHolder="Nhập đơn giá" Text="0" ValidationType="Currency" Style="width: 100px !important;"></SweetSoft:ExtraTextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Thao tác" AccessibleHeaderText="" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="">
                                                        <HeaderTemplate>
                                                            <div class="hidden-sm hidden-xs btn-group">
                                                                <SweetSoft:ExtraButton ID="btnAddNewPhuongPhap" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Add" OnClick="btnAddNewPhuongPhap_Click">
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div class="hidden-sm hidden-xs btn-group">
                                                                <SweetSoft:ExtraButton ID="btnEdit" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" OnClick="btnEdit_Click" CommandArgument='<%# Container.DataItemIndex %>'>
                                                                </SweetSoft:ExtraButton>
                                                                <SweetSoft:ExtraButton ID="btnDelete" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="phuongPhap_confirm">
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="hidden-sm hidden-xs btn-group">
                                                                <SweetSoft:ExtraButton ID="btnSubmit" runat="server" CssClass="btn-xs" EStyle="Success" EnableSmallSize="true" EIcon="A_Submit" ToolTip="Cập nhật" CommandName="Update">
                                                                </SweetSoft:ExtraButton>
                                                                <SweetSoft:ExtraButton ID="btnClose" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy" OnClick="btnClose_Click">
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </SweetSoft:ExtraGrid>
                                        </div>
                                    </div>
                                </div>

                                <div class="widget-box">
                                    <div class="widget-header">
                                        <h4 class="widget-title">Biên bản ghi chép</h4>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main no-sort">
                                            <div id="divBienBanPhaKe" visible="false" runat="server">
                                                <table class="table dataTable  table-bordered   dtr-inline" cellspacing="0" rules="all" showheaderwhenempty="true" data-fix-header="true" data-sorting="false" border="1" style="width: 100%; border-collapse: collapse; display: table;" role="grid">
                                                    <thead>
                                                        <tr role="row">
                                                            <th scope="col" abbr="STT2" style="width: 20px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label=" column ascending">STT</th>
                                                            <th scope="col" abbr="STT2" style="width: 150px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label=" column ascending">Miêu tả</th>
                                                            <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label="column ascending">Loại</th>
                                                            <th scope="col" abbr="STT2" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label="column ascending">Trạng thái sử dụng </th>

                                                            <th align="center" scope="col" style="width: 100px;" class="sorting" tabindex="0" rowspan="1" colspan="1" aria-label=" column ascending">
                                                                <div class="hidden-sm hidden-xs btn-group">
                                                                    <SweetSoft:ExtraButton ID="btnAddBienBanFake" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Add" OnClick="btnAddBienBanFake_Click">
                                                                    </SweetSoft:ExtraButton>
                                                                </div>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr role="row" class="odd">
                                                            <td colspan="4">
                                                                <center>Chưa có dữ liệu biên bản ghi chép</center>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <SweetSoft:ExtraGrid Width="100%" ID="grvBienBanGhiChep" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                                                runat="server" AllowPaging="false" ShowHeaderWhenEmpty="true" AllowSorting="false"
                                                OnRowCancelingEdit="grvBienBanGhiChep_RowCancelingEdit"
                                                OnRowUpdating="grvBienBanGhiChep_RowUpdating"
                                                OnRowDataBound="grvBienBanGhiChep_RowDataBound"
                                                OnRowCreated="grvBienBanGhiChep_RowCreated"
                                                PagingControlName="paging">
                                                <EmptyDataTemplate>
                                                    Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%# GetIndex(grvBienBanGhiChep, Container.DataItemIndex)  %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tên " AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <%# Eval("Ten") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="hdfEditID" runat="server" />
                                                            <SweetSoft:ExtraTextBox ID="txtEditTen" runat="server" PlaceHolder="Nhập nội dung"></SweetSoft:ExtraTextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Loại" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <%# Eval("Loai") %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <SweetSoft:ExtraComboBox Width="300px" ID="cbEditLoai" runat="server" EmptyMessage="Chọn nhóm">
                                                                <asp:ListItem Value="MT" Text="MT"></asp:ListItem>
                                                                <asp:ListItem Value="Chứng dương" Text="Chứng dương"></asp:ListItem>
                                                            </SweetSoft:ExtraComboBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Trạng thái" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" ItemStyle-Width="150px" SortExpression="TrangThaiSuDung">
                                                        <ItemTemplate>
                                                            <%# GetStatusStyle(Eval("TrangThaiSuDung")) %>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <SweetSoft:ExtraCheckBox ID="chkEditStatus" runat="server" CheckboxType="Switch7" DataOn="Bật" DataOff="Tắt" Checked="true">
                                                            </SweetSoft:ExtraCheckBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Thao tác" AccessibleHeaderText="" HeaderStyle-Width="10%" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" SortExpression="">
                                                        <HeaderTemplate>
                                                            <div class="hidden-sm hidden-xs btn-group">
                                                                <SweetSoft:ExtraButton ID="btnAddNewBienBan" runat="server" CssClass="btn-xs" EStyle="Warning" EnableSmallSize="true" EIcon="A_Add" OnClick="btnAddNewBienBan_Click">
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div class="hidden-sm hidden-xs btn-group">
                                                                <SweetSoft:ExtraButton ID="btnEdit" runat="server" CssClass="btn-xs" EStyle="Primary" EnableSmallSize="true" EIcon="A_Edit" ToolTip="Chỉnh sửa" OnClick="btnEdit_Click" CommandArgument='<%# Container.DataItemIndex %>'>
                                                                </SweetSoft:ExtraButton>
                                                                <SweetSoft:ExtraButton ID="btnDelete" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Delete" ToolTip="Xóa" OnClick="btnDelete_Click" CommandArgument='<%# Eval("ID") %>' CommandName="bienBan_confirm">
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="hidden-sm hidden-xs btn-group">
                                                                <SweetSoft:ExtraButton ID="btnSubmit" runat="server" CssClass="btn-xs" EStyle="Success" EnableSmallSize="true" EIcon="A_Submit" ToolTip="Cập nhật" CommandName="Update">
                                                                </SweetSoft:ExtraButton>
                                                                <SweetSoft:ExtraButton ID="btnCloseBienBan" runat="server" CssClass="btn-xs" EStyle="Danger" EnableSmallSize="true" EIcon="A_Close" ToolTip="Hủy" OnClick="btnCloseBienBan_Click">
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </SweetSoft:ExtraGrid>
                                        </div>
                                    </div>
                                </div>


                                <div class="widget-box" style="margin-top: 20px; display: none;">
                                    <div class="widget-header">
                                        <h4 class="widget-title">Nhóm xét nghiệm</h4>
                                    </div>
                                    <div class="widget-body">
                                        <div class="widget-main no-sort">
                                            <SweetSoft:ExtraGrid Width="100%" Visible="false" ID="grvNhom" AutoResponsive="true" EnableFixHeader="true" AutoGenerateColumns="false"
                                                runat="server" AllowPaging="false" ShowHeaderWhenEmpty="true" AllowSorting="false" OnRowCreated="grvNhom_RowCreated" PagingControlName="paging">
                                                <EmptyDataTemplate>
                                                    Hệ thống không trích xuất được dữ liệu, vui lòng lựa chọn tiêu chí tìm kiếm khác!
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="STT" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <ItemTemplate>
                                                            <%# GetIndex(grvNhom, Container.DataItemIndex)  %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="20px" ItemStyle-Width="20px">
                                                        <HeaderTemplate>
                                                            <SweetSoft:ExtraCheckBox ID="chkAll" runat="server" ValidationGroup="chk-all-nhom" GroupName="chk-all-nhom" EnableClickGroup="true" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <SweetSoft:ExtraCheckBox ID="chkItem" runat="server" CssClass="chk-all-nhom" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tên nhóm" AccessibleHeaderText="STT2" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hiddenID" runat="server" />
                                                            <asp:HiddenField ID="hiddenNhomID" runat="server" />
                                                            <asp:HiddenField ID="hiddenName" runat="server" />
                                                            <%# Eval("Ten") %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </SweetSoft:ExtraGrid>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="DialogPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="StyleFScriptPlaceHolder" runat="server">
    <script src="/scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        function SetTab(tabName) {
            $("input[type='hidden'][id$='hiddenTab']").val(tabName);
        }

        $(function () {
            SearchTenTT();
            addRequestHanlde(SearchTenTT);

        });

        function SetTab(tabName) {
            $("input[type='hidden'][id$='hiddenTab']").val(tabName);
        }



        function SearchTenTT(s, a) {
            if ($("input[type='text'][id$='txtTenTrongThongTu']").length > 0) {

                $("input[type='text'][id$='txtTenTrongThongTu']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchChiTieuTTQDTextByKeyWord",
                            data: "{'key':'" + $("input[type='text'][id$='txtTenTrongThongTu']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                console.log("result: ", result);
                                response($.map(result.d.List, function (item) {
                                    return { TenTrongTTQD: item.TenTrongTTQD, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtTenTrongThongTu']").val(ui.item.TenTrongTTQD);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.TenTrongTTQD + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }


            if ($("input[type='text'][id$='txtEditSTT']").length > 0) {

                $("input[type='text'][id$='txtEditSTT']").autocomplete({
                    source: function (request, response) {
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "/Default.aspx/SearchChiTieuSTTextByKeyWord",
                            data: "{'key':'" + $("input[type='text'][id$='txtEditSTT']").val() + "'}",
                            dataType: "json",
                            async: true,
                            cache: false,
                            success: function (result) {
                                console.log("result: ", result);
                                response($.map(result.d.List, function (item) {
                                    return { Stt: item.Stt, ID: item.Id };
                                }));
                            },
                            error: function (msg) {

                            }
                        });
                    },
                    messages: {
                        noResults: '',
                        results: function () { }
                    },
                    focus: function (event, ui) {
                        return false;
                    },
                    select: function (event, ui) {
                        $("input[type='text'][id$='txtEditSTT']").val(ui.item.Stt);
                        return false;
                    }
                }).data("ui-autocomplete")._renderItem = function (ul, item) {
                    return $("<li>")
                        .data("ui-autocomplete-item", item)
                        .append("<a><span style='width:30px;'>" + item.Stt + '</span>' + "</a>")
                        .appendTo(ul);
                };
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ConfirmHeaderPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ConfirmContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ConfirmFooterPlaceHolder" runat="server">
    <SweetSoft:ExtraButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" CommandName="exec_delete" Text="Xóa" EIcon="A_Delete" EStyle="Danger_Bold"></SweetSoft:ExtraButton>
</asp:Content>
