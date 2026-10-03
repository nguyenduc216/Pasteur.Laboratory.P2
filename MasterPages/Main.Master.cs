using Pasteur.Core.Helper;
using Pasteur.Laboratory.P2.Common;
using SweetSoft.ExtraControls.icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.MasterPages
{
    public partial class Main : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ApplicationContext.Current.CommonUser != null)
                    hiddenUserLoginId.Value = ApplicationContext.Current.CommonUser.IDTaiKhoan.ToString();
            }
        }

        public override void OpenMessageBox(string title, string message)
        {
            ltrDialogConfirmTitle.Text = title;
            ltrDialogConfirmContent.Text = message;
            btnConfirmCancel.Visible = btnConfirmSubmit.Visible = false;
            dialogConfirm.ShowDialog();
        }

        public override void OpenMessageBox(string title, string message, ButtonStyle submitStyle, ExtraIcon submitIcon, string submitText)
        {
            ltrDialogConfirmTitle.Text = title;
            ltrDialogConfirmContent.Text = message;
            btnConfirmSubmit.Visible = submitStyle != ButtonStyle.None;
            btnConfirmSubmit.EStyle = submitStyle;
            btnConfirmSubmit.EIcon = submitIcon;
            btnConfirmSubmit.Text = submitText;
            btnConfirmCancel.Visible = false;
            dialogConfirm.ShowDialog();
        }

        public override void OpenMessageBox(string title, string message, ButtonStyle submitStyle, ExtraIcon submitIcon, string submitText,
                                                                         ButtonStyle cancelStyle, ExtraIcon cancelIcon, string cancelText)
        {
            ltrDialogConfirmTitle.Text = title;
            ltrDialogConfirmContent.Text = message;
            btnConfirmSubmit.Visible = submitStyle != ButtonStyle.None;
            btnConfirmSubmit.EStyle = submitStyle;
            btnConfirmSubmit.EIcon = submitIcon;
            btnConfirmSubmit.Text = submitText;
            btnConfirmCancel.Visible = cancelStyle != ButtonStyle.None;
            btnConfirmCancel.EStyle = cancelStyle;
            btnConfirmCancel.EIcon = cancelIcon;
            btnConfirmCancel.Text = cancelText;
            dialogConfirm.ShowDialog();
        }

        public override void CloseMessageBox()
        {
            dialogConfirm.CloseDialog();
        }

        public override string GetMessageBoxClientID()
        {
            return dialogConfirm.ClientID;
        }

        protected void btnConfirmSubmit_Click(object sender, EventArgs e)
        {
            if (this.Page is BasePage)
                ((this.Page) as BasePage).MessageBoxSubmitHanlder();
        }

        protected void btnConfirmCancel_Click(object sender, EventArgs e)
        {
            if (this.Page is BasePage)
                ((this.Page) as BasePage).MessageBoxCancelHanlder();
        }

        //update by vi.ho 14-06-16
        protected void btnSubmitLogout_Click(object sender, EventArgs e)
        {
            // Code: CTGH-SESSION-20260918 - Kết thúc Session để cache giới hạn chỉ tiêu không tồn tại qua lần đăng nhập mới.
            ApplicationContext.Current.ClearUser();
            ApplicationContext.Current.ClearUserFlag();
            Session.Clear();
            Session.Abandon();
            HttpCookie ck = Request.Cookies["common-user"];
            if (ck != null)
            {
                ck.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(ck);
            }
            Response.Redirect(UrlRewriteHelper.GetURLPoliceLogin());
        }
    }
}
