using SweetSoft.ExtraControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using SweetSoft.ExtraControls.icons;
using Pasteur.Core.Manager;
using Pasteur.Core.Helper;

namespace Pasteur.Laboratory.P2.Common
{
    public class BaseUserControl : UserControl
    {
        #region Properties
        protected const string ApplicationID = "QLKQXN";
        public BasePage CURRENT_PAGE
        {
            get
            {
                if (this.Page != null && this.Page is BasePage) return this.Page as BasePage;
                return null;
            }
        }

        protected string FUNCTION_CODE
        {
            get
            {
                if (CURRENT_PAGE != null) return CURRENT_PAGE.FUNCTION_CODE;
                return FunctionSystem.Code.Base_Code;
            }
        }

        private bool cr_typeControl = true;
        public virtual bool BusinessControl
        {
            get
            {
                return cr_typeControl;
            }
            set
            {
                cr_typeControl = value;
            }
        }

        public LogPage CurrentLog
        {
            get
            {
                if (CURRENT_PAGE != null) return CURRENT_PAGE.CurrentLog;
                return null;
            }
        }

        public MessageBoxResult CurrentMessageBoxResult
        {
            set
            {
                Session["CR_MESSAGEBOXRESULT"] = value;
            }
            get
            {
                if (Session["CR_MESSAGEBOXRESULT"] != null)
                    return Session["CR_MESSAGEBOXRESULT"] as MessageBoxResult;
                return new MessageBoxResult();
            }
        }

        public List<ApplicationUserRight> CurrentUserRight
        {
            get
            {
                if (CURRENT_PAGE != null) return CURRENT_PAGE.CurrentUserRight;
                return new List<ApplicationUserRight>();
            }

        }

        public bool UserHasRight(string rightCode)
        {
            if (CURRENT_PAGE != null) return CURRENT_PAGE.UserHasRight(rightCode);
            return false;

        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ResourceText();
        }

        #region BasePage
        public void RunScript(string scriptName, string scriptParam)
        {
            if (this.Page is BasePage)
            {
                ((BasePage)this.Page).RunScript(scriptName, scriptParam);
            }
        }

        protected string GetYesNoStyle(object stt)
        {
            if (this.Page is BasePage)
            {
                ((BasePage)this.Page).GetYesNoStyle(stt);
            }
            return "";
        }
        #endregion

        #region Function
        protected virtual void ResourceText()
        {

        }

        /// <summary>
        /// Hàm lấy resource text cho hệ thống
        /// </summary>
        /// <param name="resourceKey">Từ khóa</param>
        /// <param name="fileName">File Resource</param>
        /// <returns></returns>
        public string GetResourceText(string resourceKey, string fileName)
        {
            if (CURRENT_PAGE != null)
                return CURRENT_PAGE.GetResourceText(resourceKey, fileName);
            return resourceKey;
        }
        /// <summary>
        /// Hàm lấy resource text cho hệ thống
        /// </summary>
        /// <param name="resourceKey">từ khóa</param>
        /// <returns></returns>
        public string GetResourceText(string resourceKey)
        {
            if (CURRENT_PAGE != null)
                return CURRENT_PAGE.GetResourceText(resourceKey);
            return resourceKey;
        }

        protected bool ButtonState(string rightName)
        {
            bool result = false;
            if (CURRENT_PAGE != null)
                result = CURRENT_PAGE.ButtonState(rightName);
            return result;
        }

        protected string CurrentUserFullname
        {
            get
            {
                string result = string.Empty;
                if (ApplicationContext.Current.CommonUser != null)
                {
                    result = ApplicationContext.Current.CommonUser.HoVaTen;

                }
                return result;
            }
        }
        #endregion

        #region Validation
        public void AddValidPromt(string validClientID, string message)
        {
            if (CURRENT_PAGE != null)
                CURRENT_PAGE.AddValidPromt(validClientID, message);
        }
        public void ShowValidPromt()
        {
            if (CURRENT_PAGE != null) CURRENT_PAGE.ShowValidPromt();
        }
        /// <summary>
        /// Kiếm tra 
        /// </summary>
        public bool ControlIsValid
        {
            get
            {
                if (CURRENT_PAGE != null)
                    return CURRENT_PAGE.PageIsValid;
                return true;
            }
        }
        #endregion

        #region Notification
        /// <summary>
        /// Hàm hiển thị thông báo notification
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="message">Tin nhắn</param>
        /// <param name="Type">Loại tin nhắn</param>
        protected void ShowNotify(string title, string message, NotifyType Type)
        {
            if (CURRENT_PAGE != null)
                CURRENT_PAGE.ShowNotify(title, message, Type);
        }
        /// <summary>
        /// Hàm hiển thị thông báo notification
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="message">Nội dung</param>
        /// <param name="image">Hình ảnh</param>
        /// <param name="Type">Loại notification</param>
        /// <param name="sticky">Đính lại trang</param>
        /// <param name="light">Opacity màu nền</param>
        /// <param name="center">Xuất hiện ở giữa màn hình</param>
        /// <param name="runStartup">Bật khi load trang</param>
        public void ShowNotify(string title, string message, string image, NotifyType Type, bool sticky, bool light, bool center, bool runStartup)
        {
            if (CURRENT_PAGE != null)
                CURRENT_PAGE.ShowNotify(title, message, image, Type, sticky, light, center, runStartup);
        }
        #endregion

        #region Message + Confirm
        public void CloseMessageBox()
        {
            if (CURRENT_PAGE != null)
                CURRENT_PAGE.CloseMessageBox();
        }


        public virtual void OpenMessageBox(string title, string message)
        {
            if (CURRENT_PAGE != null)
                CURRENT_PAGE.OpenMessageBox(title, message);
        }
        #endregion
    }
}