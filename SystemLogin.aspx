<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemLogin.aspx.cs" Inherits="Pasteur.Laboratory.P2.SystemLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    <title>Đăng nhập phần mềm quản lý kết quả xét nghiệm</title>
    <link rel="icon" href="images/logo_icon.ico" type="image/gif" sizes="16x16" />
    <meta name="description" content="User login page" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />

    <!-- bootstrap & fontawesome -->
    <link href="/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/icons/AweIcon/aweicon.css" rel="stylesheet" />
    <link href="/icons/FlatIcon/flaticon.css" rel="stylesheet" />

    <!-- text fonts -->
    <link href="https://fonts.googleapis.com/css2?family=Questrial&display=swap" rel="stylesheet">
    <style type="text/css">
        body, html {
            font-family: 'Questrial' !important;
            font-size: 15px !important;
        }
    </style>

    <!-- ace styles -->
    <link href="/themes/ace/style/ace.min.css" rel="stylesheet" />
    <!-- controls -->
    <link href="/css/textbox/ExtraTextbox.css" rel="stylesheet" />
    <link href="/css/checkbox/checkbox.css" rel="stylesheet" />
    <link href="/css/nprogress/nprogress.css" rel="stylesheet" />
    <link href="/css/pnotify/animate.css" rel="stylesheet" />
    <link href="/css/pnotify/pnotify.core.css" rel="stylesheet" />
    <link href="/css/pnotify/pnotify.custom.min.css" rel="stylesheet" />

    <link href="/css/validation/validationEngine.jquery.css" rel="stylesheet" />
    <link href="/css/ControlStyle.css" rel="stylesheet" />
    <script src="/scripts/jQuery-2.1.4.min.js"></script>

    <script src="/scripts/pnotify/pnotify.custom.min.js"></script>
    <script src="/scripts/pnotify/pnotify.animate.js"></script>
    <script src="/scripts/nprogress/nprogress.js"></script>

    <script src="/scripts/validation/languages/jquery.validationEngine-vi-VN.js"></script>
    <script src="/scripts/validation/jquery.validationEngine.js"></script>
    <script src="/scripts/validation/validationScript.js"></script>
    <style type="text/css" media="screen">
        body {
            font-family: "Roboto",sans-serif;
        }

        .top-login {
            background: rgba(140, 15, 15, 0.2);
            padding: 20px;
        }

        .wrap-content-login {
            padding: 20px;
            min-height: 500px;
        }

        .login-form .position-relative {
            max-width: 400px;
            margin: auto;
            margin-top: 50px;
        }

        body.login-layout {
            background: transparent;
            background-image: url('/images/login-background.jpg');
            background-repeat: no-repeat;
            background-size: cover;
        }

        .footer-auth {
            background: rgba(14, 17, 19, 0.2);
            position: absolute;
            bottom: 0px;
            left: 0px;
            right: 0px;
            padding: 20px;
        }

        .helper {
            z-index: 999;
            text-align: center;
            margin-top: 20px;
            padding-bottom: 100px;
        }

        .login-processing {
            margin-top: 12px;
            margin-bottom: 0;
            text-align: center;
        }
    </style>
    <script src="/scripts/Config.js"></script>
    <script src="/scripts/button/ExtraButton.js"></script>
</head>
<body class="login-layout light-login">
    <form id="mainForm" runat="server" defaultbutton="btnLogin">
        <asp:ScriptManager ID="scriptManager" runat="server"></asp:ScriptManager>
        <div class="main-container">
            <div class="main-content">
                <div class="row">
                    <div class="top-login">

                        <div class="col-md-6 title-page text-center col-md-offset-3">
                            <h2 style="color: #fff; font-size: 25px; text-align: center;" class="text-shadown">Quản lý kết quả xét nghiệm <br /> Khoa Sức khỏe môi trường - Bệnh nghề nghiệp </h2>
                            <h1 style="font-size: 24px; color: #fff; font-weight: 700; text-transform: uppercase; text-align: center;" class="text-shadown">Viện Pasteur Nha Trang</h1>
                        </div>
                        <div class="clearfix"></div>
                    </div>

                    <div class="wrap-content-login">
                        <div class="col-md-12 login-form">
                            <div class="position-relative">
                                <div id="login-box" class="login-box visible widget-box no-border">
                                    <div class="widget-body">
                                        <div class="widget-main">
                                            <h4 class="header blue lighter bigger">
                                                <i class="ace-icon fa fa-key blue"></i>
                                                Thông tin đăng nhập
                                            </h4>
                                            <div class="space-6"></div>
                                            <div>
                                                <asp:UpdatePanel runat="server" ID="udpLogin">
                                                    <ContentTemplate>
                                                        <fieldset>
                                                            <label class="block clearfix">
                                                                <span class="block input-icon input-icon-right">
                                                                    <SweetSoft:ExtraTextBox runat="server"
                                                                        ID="txtLoginName" TabIndex="1" PlaceHolder="Tên đăng nhập" Required="true">
                                                                    </SweetSoft:ExtraTextBox>
                                                                    <i class="ace-icon fa fa-user"></i>
                                                                </span>
                                                            </label>
                                                            <label class="block clearfix">
                                                                <span class="block input-icon input-icon-right">
                                                                    <SweetSoft:ExtraTextBox runat="server" ID="txtLoginPassword"
                                                                        TabIndex="2" PlaceHolder="Mật khẩu" Required="true" TextMode="Password">
                                                                    </SweetSoft:ExtraTextBox>
                                                                    <i class="ace-icon fa fa-lock"></i>
                                                                </span>
                                                            </label>

                                                            <div class="space"></div>

                                                            <div class="clearfix">
                                                                <label class="inline">
                                                                    <SweetSoft:ExtraCheckBox
                                                                        runat="server"
                                                                        ID="chkLoginRemember"
                                                                        TabIndex="3" Checked="true"
                                                                        CssClass="ace">
                                                                    </SweetSoft:ExtraCheckBox>
                                                                    <span class="lbl">Duy trì đăng nhập</span>
                                                                </label>
                                                                <SweetSoft:ExtraButton
                                                                    runat="server" ID="btnLogin" EIconPosition="Left" TabIndex="4" EIcon="A_Sign_In" EStyle="Info"
                                                                    CssClass="width-40 pull-right btn btn-sm " ValidationGroup="login" ESubmitAndCheckValid="true" EnableSnipButton="true" SnipText="Đang xử lý..." OnClick="btnLogin_Click">
                                                                    <span class="bigger-110">Đăng nhập</span>
                                                                </SweetSoft:ExtraButton>
                                                            </div>
                                                            <div class="space-4"></div>
                                                        </fieldset>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%-- Code: LOGIN-PROGRESS-20260918 - Hiển thị trạng thái trong lúc SystemLogin.btnLogin_Click xử lý đăng nhập. --%>
                                                <asp:UpdateProgress ID="progressLogin" runat="server"
                                                    AssociatedUpdatePanelID="udpLogin" DisplayAfter="0">
                                                    <ProgressTemplate>
                                                        <div class="alert alert-info login-processing">
                                                            <i class="ace-icon fa fa-spinner fa-spin"></i>
                                                            Đang kiểm tra thông tin đăng nhập, vui lòng chờ...
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                            <div class="space-6"></div>
                                        </div>
                                        <!-- /.widget-main -->

                                        <div class="toolbar clearfix" style="display: none;">
                                            <div>
                                                <a href="#" data-target="#forgot-box"
                                                    class="forgot-password-link">
                                                    <i class="ace-icon fa fa-arrow-left"></i>
                                                    Quên mật khẩu
                                                </a>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.widget-body -->
                                </div>
                                <!-- /.login-box -->


                            </div>
                        </div>

                    </div>

                    <div class="footer-auth text-right">
                        <h5 class="white">
                            <%--<img width="20" alt="logo" src="/images/logo-thue.png" />--%>
                            Hệ thống quản lý kết quả xét nghiệm  - viện Pasteur Nha Trang</h5>
                    </div>
                </div>
                <!-- /.row -->
            </div>
            <!-- /.main-content -->
        </div>
        <!-- /.main-container -->
    </form>

    <!-- basic scripts -->

    <script type="text/javascript">
        window.jQuery || document.write("<script src='/scripts/jQuery-2.1.4.min.js'>" + "<" + "/script>");
    </script>

    <%--<script type="text/javascript">
        window.jQuery || document.write("<script src='assets/js/jquery1x.min.js'>" + "<" + "/script>");
    </script>--%>
    <%--<script type="text/javascript">
        if ('ontouchstart' in document.documentElement)
            document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
    </script>--%>

    <!-- inline scripts related to this page -->
    <script type="text/javascript">

        jQuery(function ($) {
            $(document).on('click', '.toolbar a[data-target]', function (e) {
                e.preventDefault();
                var target = $(this).data('target');
                $('.widget-box.visible').removeClass('visible');//hide others
                $(target).addClass('visible');//show target
            });
        });

        //used for changing background
        jQuery(function ($) {
            $('#btn-login-dark').on('click', function (e) {
                $('body').attr('class', 'login-layout');
                $('#id-text2').attr('class', 'white');
                $('#id-company-text').attr('class', 'blue');

                e.preventDefault();
            });
            $('#btn-login-light').on('click', function (e) {
                $('body').attr('class', 'login-layout light-login');
                $('#id-text2').attr('class', 'grey');
                $('#id-company-text').attr('class', 'blue');

                e.preventDefault();
            });
            $('#btn-login-blur').on('click', function (e) {
                $('body').attr('class', 'login-layout blur-login');
                $('#id-text2').attr('class', 'white');
                $('#id-company-text').attr('class', 'light-blue');

                e.preventDefault();
            });

        });
    </script>
</body>
</html>
