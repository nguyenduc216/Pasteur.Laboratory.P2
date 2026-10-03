<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HorizotalMenu.ascx.cs" Inherits="Pasteur.Laboratory.P2.Controls.HorizotalMenu" %>


<asp:Literal ID="ltrHTML2" runat="server" Visible="false"></asp:Literal>

<nav class="navbar navbar-inverse">
    <div class="navbar-header">
        <button class="navbar-toggle" type="button" data-toggle="collapse" data-target=".js-navbar-collapse">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
        </button>
        <a class="navbar-brand" href="/trang-chu" style="cursor:pointer;">
            <img src="../images/logo.png" alt="Pasteur Nha Trang" title="Pasteur Nha Trang" height="40" width="40" />
        </a>
    </div>

    <div class="collapse navbar-collapse js-navbar-collapse">
        <ul class="nav navbar-nav">
            <asp:Literal ID="ltrHtml" runat="server"></asp:Literal>
        </ul>
        <ul class="nav navbar-nav navbar-right">
            <li class="dropdown">
                <a href="#" style="background-color: #49046d; width: 50px; cursor: pointer; padding-top: 15px;"
                    class="dropdown-toggle user-info-2" data-toggle="dropdown" role="button" aria-expanded="false">
                    <asp:Literal ID="ltrAcount" runat="server" Text="Xin chào," Visible="false"></asp:Literal>

                    <i style="display: block; font-size: 22px;" class="fa fa-cogs"></i>
                </a>
                <ul class="dropdown-menu" role="menu">
                     <li><a href="javascript:(0)"><i class="fa fa-user" style="margin-right: 10px;"></i><asp:Literal ID="ltrUser" runat="server"></asp:Literal></a></li>
                    <li><a href="/thay-mat-khau"><i class="fa fa-key" style="margin-right: 10px;"></i>Đổi mật khẩu</a></li>
                    <li class="divider"></li>
                    <li><a onclick="OpenDialogClient('ctl00_dialogLogout');return false;"><i class="fa fa-sign-out" style="margin-right: 10px;"></i>Đăng xuất</a></li>
                </ul>
            </li>
        </ul>
    </div>
    <!-- /.nav-collapse -->
</nav>


