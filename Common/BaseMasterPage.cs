
using SweetSoft.ExtraControls;
using SweetSoft.ExtraControls.icons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Pasteur.Core;

namespace Pasteur.Laboratory.P2.Common
{
    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        #region Properties
        public BasePage CURRENT_PAGE
        {
            get
            {
                if (this.Page != null && this.Page is BasePage) return this.Page as BasePage;
                return null;
            }
        }

        public MessageBoxResult CurrentConfirmResult
        {
            get
            {
                if (Session["CurrentConfirmResult"] != null)
                    return (MessageBoxResult)Session["CurrentConfirmResult"];
                return null;
            }
            set
            {
                Session["CurrentConfirmResult"] = value;
            }
        }

        public virtual ExtraScriptRegister CURRENT_SCRIPT_MANAGER { get { return null; } }

        public override bool EnableViewState
        {
            get
            {
                return true;
            }
            set
            {
                base.EnableViewState = value;
            }
        }

        public virtual string GetMessageBoxClientID() { return string.Empty; }
        #endregion

        #region Function
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ResourceText();
        }

        protected virtual void ResourceText()
        {

        }

        public virtual void UpdateContainerPanel() { }
        /// <summary>
        /// Hàm lấy resource text cho hệ thống
        /// </summary>
        /// <param name="resourceKey">Từ khóa</param>
        /// <param name="fileName">File Resource</param>
        /// <returns></returns>
        public string GetResourceText(string resourceKey, string fileName)
        {
            if (CURRENT_PAGE != null) return CURRENT_PAGE.GetResourceText(resourceKey, fileName);
            return resourceKey;
        }
        /// <summary>
        /// Hàm lấy resource text cho hệ thống
        /// </summary>
        /// <param name="resourceKey">từ khóa</param>
        /// <returns></returns>
        public string GetResourceText(string resourceKey)
        {
            if (CURRENT_PAGE != null) return CURRENT_PAGE.GetResourceText(resourceKey);
            return resourceKey;
        }

        private void RunScriptC(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, typeof(string), "runScript",
                                       script, true);
        }

        private void RunScript(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", script, true);

        }

        protected void PerformanceButtonClick(string buttonid, bool clientid)
        {
            RunScriptC("PerformanceButtonClick", string.Format("'{0}',{1}", buttonid, clientid.ToString().ToLower()));
        }
        #endregion

        #region Message + Confirm
        public virtual void CloseMessageBox()
        {
        }
        public virtual void OpenMessageBox(string title, string message) { }

        public virtual void OpenMessageBox(string title, string message, ButtonStyle submitStyle, ExtraIcon submitIcon, string submitText) { }

        public virtual void OpenMessageBox(string title, string message, ButtonStyle submitStyle, ExtraIcon submitIcon, string submitText,
                                                                         ButtonStyle cancelStyle, ExtraIcon cancelIcon, string cancelText) { }
        #endregion
    }

    public enum ConfirmType
    {
        Cancel,
        YesCancel,
        YesNoCancel,
        YesNo
    }
}