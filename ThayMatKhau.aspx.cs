
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;
using System.Runtime.CompilerServices;

namespace Pasteur.Laboratory.P2
{
    public partial class ThayMatKhau : BasePage
    {
        #region Properties
        public override bool IsUserRole
        {
            get
            {
                return false;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            txtNewPass.ExtenRegex = @"^([a-z0-9A-Z\@\.]){6,15}$";
            txtNewPass.ValidRegexMessage = "Mật khẩu chỉ gồm số, chữ và ký tự @";
            txtConfirmPass.ExtenRegex = @"^([a-z0-9A-Z\@\.]){6,15}$";
            txtConfirmPass.ValidRegexMessage = "Mật khẩu chỉ gồm số, chữ và ký tự @";
            if (!IsPostBack) this.Title = "Thay đổi mật khẩu";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region Validation
            if (string.IsNullOrEmpty(txtConfirmPass.Text.Trim()))
            {
                AddValidPromt(txtConfirmPass.ClientID, "Nhập xác nhận mật khẩu mới");
                txtConfirmPass.Focus();
            }
            else if (!string.IsNullOrEmpty(txtNewPass.Text.Trim()) && txtNewPass.Text.Trim() != txtConfirmPass.Text.Trim())
            {
                AddValidPromt(txtConfirmPass.ClientID, "Mật khẩu xác nhận không đúng");
                txtConfirmPass.Focus();
            }
            else if (!Regex.Match(txtConfirmPass.Text.Trim(), @"^([a-z0-9A-Z\@\.]){6,15}$").Success)
            {
                AddValidPromt(txtConfirmPass.ClientID, "Mật khẩu dài ít nhất 6 ký tự, chỉ gồm số, chữ và ký tự @");
                txtConfirmPass.Focus();
            }

            if (string.IsNullOrEmpty(txtNewPass.Text.Trim()))
            {
                AddValidPromt(txtNewPass.ClientID, "Nhập mật khẩu mới");
                txtNewPass.Focus();
            }
            else if (!Regex.Match(txtNewPass.Text.Trim(), @"^([a-z0-9A-Z\@\.]){6,15}$").Success)
            {
                AddValidPromt(txtNewPass.ClientID, "Mật khẩu dài ít nhất 6 ký tự, chỉ gồm số, chữ và ký tự @");
                txtNewPass.Focus();
            }

            if (string.IsNullOrEmpty(txtOldPass.Text.Trim()))
            {
                AddValidPromt(txtOldPass.ClientID, "Nhập mật khẩu hiện tại");
                txtOldPass.Focus();
            }
            else
            {
                string currentPass = ApplicationContext.Current.CommonUser != null ? ApplicationContext.Current.CommonUser.MatKhau.Trim() : "";
                if (!string.IsNullOrEmpty(currentPass))
                {
                    currentPass = SecurityHelper.Decrypt128(currentPass);
                    if (currentPass != txtOldPass.Text.Trim())
                    {
                        AddValidPromt(txtOldPass.ClientID, "Mật khẩu không đúng");
                        txtOldPass.Focus();
                    }
                }
            }
            #endregion

            if (PageIsValid)
            {
                ApplicationContext.Current.CommonUser.MatKhau = SecurityHelper.Encrypt128(txtNewPass.Text.Trim());
                ApplicationContext.Current.CommonUser = UserManager.UpdateCommonUser(ApplicationContext.Current.CommonUser);
                ShowNotify("Thông báo", "Thay đổi mật khẩu thành công", SweetSoft.ExtraControls.NotifyType.Success);
                txtNewPass.Text = txtConfirmPass.Text = txtOldPass.Text = string.Empty;
            }
            else ShowValidPromt();
        }
    }
}