using SweetSoft.ExtraControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using SweetSoft.ExtraControls.icons;
using Pasteur.Core;
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;



namespace Pasteur.Laboratory.P2.Common
{
    public class BasePage : Page
    {
        #region Properties

        protected const string ApplicationID = "QLKQXN";
        public BaseMasterPage CURRENT_MASTERPAGE
        {
            get
            {
                if (this.Master != null && this.Master is BaseMasterPage)
                    return this.Master as BaseMasterPage;
                return null;
            }
        }

        public virtual bool IsLoginPage
        {
            get { return false; }
        }

        public virtual bool LoginRequired { get { return true; } }

        public ExtraScriptRegister CURRENT_SCRIPTMANAGER
        {
            get
            {
                if (this.Master != null && this.Master is BaseMasterPage)
                {
                    BaseMasterPage m = this.Master as BaseMasterPage;
                    return m.CURRENT_SCRIPT_MANAGER;
                }
                return null;
            }
        }
        /// <summary>
        /// Mã ngôn ngữ mặc định
        /// </summary>
        public string DefaultLanguage
        {
            get
            {
                string lang = CultureHelper.DEFAULT_LANGUAGE_CODE;
                if (CURRENT_SCRIPTMANAGER != null) lang = CURRENT_SCRIPTMANAGER.DefaultLanguage;
                return lang;
            }
        }

        public bool AllowEmbedGridStyle
        {
            get
            {
                bool allow = false;
                if (CURRENT_SCRIPTMANAGER != null) allow = CURRENT_SCRIPTMANAGER.AllowEmbedGridStyle;
                return allow;
            }
            set
            {
                if (CURRENT_SCRIPTMANAGER != null) CURRENT_SCRIPTMANAGER.AllowEmbedGridStyle = value;
            }
        }

        protected bool USE_DEFAULT_LANGUAGE
        {
            get
            {
                return ApplicationContext.Current.CurrentLanguageCode == DefaultLanguage;
            }
        }

        protected virtual string URL_REWRITE
        {
            get
            {
                return string.Empty;
            }
        }

        public string CurrentReturnURL
        {
            get
            {
                return CommonHelper.QueryString("ReturnURL");
            }
        }

        protected string CurrentUserFullname
        {
            get
            {
                string result = string.Empty;
                if (ApplicationContext.Current.CommonUser != null)
                {
                    result = ApplicationContext.Current.CommonUser.HoVaTen;
                    if (!string.IsNullOrEmpty(ApplicationContext.Current.CommonUser.HoVaTen))
                        result = ApplicationContext.Current.CommonUser.Email;
                }
                return result;
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

        #endregion

        #region Function
        protected string ConvertStatusToText(object value, string defaultValue)
        {
            if (value != null)
            {
                if (value.GetType().Equals(typeof(System.Boolean)))
                {
                    bool b = false;
                    bool.TryParse(value.ToString(), out b);
                    if (b) return GetResourceText(ResourceHelper.BE.Default_Status_Active);
                }
                else
                    if (value.GetType().Equals(typeof(System.String)) && value.ToString().Equals(ResourceHelper.BE.Default_Status_Active))
                    return GetResourceText(ResourceHelper.BE.Default_Status_Active);
                return GetResourceText(ResourceHelper.BE.Default_Status_InActive);
            }
            return defaultValue;
        }

        protected string GetStatusStyle(object value)
        {
            if (value != null)
            {
                if (value.GetType().Equals(typeof(System.Boolean)))
                {
                    bool b = false;
                    bool.TryParse(value.ToString(), out b);
                    if (b)
                        return "<span class='label label-success'>Kích hoạt</span>";
                }
                else
                    if (value.GetType().Equals(typeof(System.String)) &&
                        value.ToString().Equals(ResourceHelper.BE.Default_Status_Active))
                    return "<span class='label'>Khóa</span>";
                return "<span class='label'>Khóa</span>";
            }
            return "<span class='label'>Khóa</span>";
        }

        public string GetYesNoStyle(object value)
        {
            if (value != null)
            {
                if (value.GetType().Equals(typeof(System.Boolean)))
                {
                    bool b = false;
                    bool.TryParse(value.ToString(), out b);
                    if (b)
                        return "<span class='label label-info'>Duyệt</span>";
                }
                else
                    if (value.GetType().Equals(typeof(System.String)) &&
                        value.ToString().Equals(ResourceHelper.BE.Default_Status_Active))
                    return "<span class='label'>Chưa duyệt</span>";
                return "<span class='label'>Chưa duyệt</span>";
            }
            return "<span class='label'>Chưa duyệt</span>";
        }

        protected string GetStatusStyle2(object stt)
        {
            string span = "<big><i class='ace-icon fa fa-times-circle red'></i></big>";
            if (stt != null)
            {
                if (stt.GetType().Equals(typeof(System.Boolean)))
                {
                    bool b = false;
                    bool.TryParse(stt.ToString(), out b);
                    if (b)
                        span = "<big><i class='ace-icon fa fa-check-circle green'></i></big>";
                }
                else if (stt.GetType().Equals(typeof(System.String)) &&
                         stt.ToString().Equals(ResourceHelper.BE.Default_Status_Active))
                    span = "<big><i class='ace-icon fa fa-times-circle red'></i></big>";
            }
            return span;
        }



        /// <summary>
        /// Hàm lấy cờ cho dữ liệu theo người dùng công an
        /// </summary>
        /// <param name="entId">id doanh nghiệp</param>
        /// <param name="hotelId">id khách sạn</param>
        protected string GetHotelFlag(long? entId, long? hotelId)
        {
            string result = string.Empty;
            if (entId.HasValue && hotelId.HasValue)
                result = string.Format("{0}.K{1}.", entId.Value, hotelId.Value);
            if (entId.HasValue && !hotelId.HasValue)
                result = string.Format("{0}.", entId.Value);
            return result;
        }




        public bool ButtonState(string rightName)
        {
            bool a = ApplicationContext.Current.CommonUser != null &&
                     ApplicationContext.Current.CommonUser.TenDangNhap == "Administrator";
            bool b = UserHasRight(rightName) || a;
            return b;
        }

        protected string ConvertStatusToText(object value)
        {
            return ConvertStatusToText(value, string.Empty);
        }

        protected string ConvertCurrencyToText(object value, string defaultValue)
        {
            decimal d = 0;
            if (value != null)
            {
                decimal.TryParse(value.ToString(), out d);
                return d.ToStringWithLanguage();
            }
            return defaultValue;
        }

        protected string ConvertCurrencyToText(object value)
        {
            return ConvertCurrencyToText(value, string.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            ResourceText();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ResourceText();
            if (ApplicationContext.Current.CommonUser == null)
            {
                HttpCookie ck = Request.Cookies["common-user"];
                if (ck != null)
                {
                    TblTaiKhoanNguoiDung user = UserManager.GetCommonUserByEmail(SecurityHelper.Decrypt128(ck.Value));
                    if (user != null)
                    {
                        if (user.TrangThaiKichHoat.HasValue && user.TrangThaiKichHoat.Value)
                        {
                            ApplicationContext.Current.AddCommonUser(user);//lưu user vào session
                            CurrentUserRight = RoleManager.GetRightByCommonUserID(user.IDTaiKhoan);
                        }
                    }
                }
                else

                    if ((!IsLoginPage && LoginRequired))
                {
                    if (!string.IsNullOrEmpty(Request.RawUrl) &&
                        !Request.RawUrl.ToLower().Contains("default.aspx"))
                    {
                        string url = string.Format("/loginURL{0}",
                                                    HttpUtility.UrlEncode(Request.RawUrl.Replace("/", "_")));
                        Response.Redirect(url);
                    }
                    else
                        Response.Redirect(UrlRewriteHelper.GetURLPoliceErrorLogin());
                }
            }
            else
            {
                if (!UserHasFunctionPage(this.FUNCTION_CODE) && !IsErrorpage && IsUserRole && !IsLoginPage)
                {
                    Session["403Error"] = FUNCTION_NAME;
                    Session["403Function"] = FUNCTION_CODE;
                    Response.Redirect(UrlRewriteHelper.GetURLSystemError(403));
                }
            }
            this.Page.Title = FUNCTION_NAME;
        }

        protected virtual void ResourceText()
        {

        }
        /// <summary>
        /// Refresh panel giữa
        /// </summary>
        protected void UpdateContainerPanel()
        {
            if (CURRENT_MASTERPAGE != null)
                CURRENT_MASTERPAGE.UpdateContainerPanel();
        }
        /// <summary>
        /// Hàm lấy resource text cho hệ thống
        /// </summary>
        /// <param name="resourceKey">Từ khóa</param>
        /// <param name="fileName">File Resource</param>
        /// <returns></returns>
        public string GetResourceText(string resourceKey, string fileName)
        {
            object value = GetGlobalResourceObject(fileName, resourceKey);
            if (value != null)
                return value.ToString();
            return resourceKey;
        }
        /// <summary>
        /// Hàm lấy resource text cho hệ thống
        /// </summary>
        /// <param name="resourceKey">từ khóa</param>
        /// <returns></returns>
        public string GetResourceText(string resourceKey)
        {
            return GetResourceText(ResourceType.Default, resourceKey);
        }

        public string GetResourceText(ResourceType type, string resourceKey)
        {
            return GetResourceText(resourceKey,
                                   string.Format("{0}_{1}", type.EnumToString(),
                                   ApplicationContext.Current.CurrentLanguageCode.Replace("-", "_")));
        }

        public void RunScript(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", script, true);
        }

        public void RunScriptC(string scriptName, string param)
        {
            string script = string.Format("{0}({1});", scriptName, param);
            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, typeof(string), "runScript",
                                       script, true);
        }

        public void RunScriptWithString(string script)
        {
            AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, typeof(string), "runScript",
                                       script, true);
        }
        #endregion

        #region Right
        public virtual string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Base_Code;
            }
        }

        public virtual string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Base_XN_Name;
            }
        }

        public virtual bool IsUserRole
        {
            get
            {
                return true; // Thay đổi lại khi đã phân quyền
            }
        }

        public virtual bool IsErrorpage
        {
            get { return false; }
        }

        public List<ApplicationUserRight> CurrentUserRight
        {
            get
            {
                List<ApplicationUserRight> list = new List<ApplicationUserRight>();
                if (Session[SessionName.Right_CurrentUserRight] != null)
                    list = Session[SessionName.Right_CurrentUserRight] as List<ApplicationUserRight>;

                if (list.Count == 0)
                {
                    if (ApplicationContext.Current.CommonUser != null)
                    {
                        Session[SessionName.Right_CurrentUserRight] = RoleManager.GetRightByCommonUserID(ApplicationContext.Current.CommonUser.IDTaiKhoan);
                        if (Session[SessionName.Right_CurrentUserRight] != null)
                            list = Session[SessionName.Right_CurrentUserRight] as List<ApplicationUserRight>;
                    }
                }
                if (list == null) list = new List<ApplicationUserRight>();
                return list;
            }
            set
            {
                Session[SessionName.Right_CurrentUserRight] = value;
            }
        }

        public bool UserHasFunctionPage(string functionCode)
        {
            bool hasRight = ApplicationContext.Current.CommonUser != null && ApplicationContext.Current.CommonUser.TenDangNhap == "Administrator";
            if (CurrentUserRight.Count > 0)
            {
                ApplicationUserRight right = CurrentUserRight.FirstOrDefault(p => functionCode.Trim().StartsWith(p.FunctionCode.Trim()));
                hasRight |= right != null;
            }
            return hasRight;
        }

        public void AddRightFunction()
        {
            //ApplicationUserRight right = CurrentUserRight.FirstOrDefault(p => p.FunctionCode.Trim() == f.Code.Trim());
            //if (right == null)
            //{
            //    right = new ApplicationUserRight();
            //    right.FunctionCode = f.Code;
            //    right.FunctionID = f.Id;
            //    right.FunctionName = !string.IsNullOrEmpty(f.TitleLang) ? f.TitleLang : f.Title;
            //    CurrentUserRight.Add(right);
            //}
        }

        public bool UserHasRight(string rightCode)
        {
            if (CurrentUserRight.Count > 0 || (ApplicationContext.Current.CommonUser != null
                 && ApplicationContext.Current.CommonUser.TenDangNhap == "Administrator"))
            {
                ApplicationUserRight right = CurrentUserRight.FirstOrDefault(p => p.RightCode.Trim().Equals(rightCode));
                return right != null || (ApplicationContext.Current.CommonUser.TenDangNhap == "Administrator");
            }
            return false;
        }
        #endregion

        #region Notification


        /// <summary>
        /// Hàm hiển thị thông báo notification
        /// </summary>
        /// <param name="title">Tiêu đề</param>
        /// <param name="message">Tin nhắn</param>
        /// <param name="Type">Loại tin nhắn</param>
        public void ShowNotify(string title, string message, NotifyType Type)
        {
            ShowNotify(title, message, string.Empty, Type, false, false, false, false);

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
            string script = string.Empty;
            if (ExtraSkinManager.DEFAULT_THEME == ExtraTheme.ACE)
            {
                script = string.Format("ACEnotify.Show('{0}','{1}','{2}',{3},'{4}',{5},{6});", title, message, image, sticky.ToString().ToLower(), "gritter-" + Type.EnumToString(), center.ToString().ToLower(), light.ToString().ToLower());
            }
            else
                script = string.Format(" SPnotify.Show('{0}','{1}','{2}','{3}','{4}');", Type.EnumToString(), title, message, AnimationType.SlideInRight.EnumToString(), AnimationType.SlideInRight.EnumToString());

            if (!string.IsNullOrEmpty(script))
            {
                if (!runStartup)
                    AjaxControlToolkit.ToolkitScriptManager.RegisterClientScriptBlock(this, typeof(string), "runNotifyScript",
                                         script, true);
                else
                    ExtraScriptRegister.ScriptRunStartUp += script;
            }
        }

        protected void ShowNotifyStartUp(string title, string message, bool isSticky, NotifyType Type)
        {
            ShowNotifyStartUp(title, message, string.Empty, Type, isSticky, false, false, true);
        }

        public void ShowNotifyStartUp(string title, string message, string image, NotifyType Type, bool sticky, bool light, bool center, bool runStartup)
        {
            string script = string.Format("ACEnotify.Show('{0}','{1}','{2}',{3},'{4}',{5},{6});", title, message, string.Empty, sticky.ToString().ToLower(), "gritter-" + Type.EnumToString(), center.ToString().ToLower(), light.ToString().ToLower());
            ExtraScriptRegister.ScriptRunStartUp += script;
        }
        #endregion

        #region Logging
        public LogPage CurrentLog
        {
            get
            {
                if (Session["Log_" + FUNCTION_CODE] == null)
                {
                    LogPage lg = new LogPage();
                    lg.FunctionCode = FUNCTION_CODE;
                    Session["Log_" + FUNCTION_CODE] = lg;
                }
                return Session["Log_" + FUNCTION_CODE] as LogPage;
            }
        }
        #endregion

        #region Validation
        private StringBuilder cr_listvalidationpromt = new StringBuilder();

        public void AddValidPromt(string validClientID, string message)
        {
            cr_listvalidationpromt.AppendFormat("$('#{0}').validationEngine('showPrompt', '{1}', 'error', true);", validClientID, message);
        }



        public void ShowValidPromt()
        {
            if (cr_listvalidationpromt.Length > 0)
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RunScript", cr_listvalidationpromt.ToString(), true);
        }
        /// <summary>
        /// Kiếm tra 
        /// </summary>
        public bool PageIsValid { get { return cr_listvalidationpromt.Length == 0; } }
        #endregion

        #region Send Mail

        protected void ProcessException(Exception ex)
        {
            CommonHelper.SendEmailError(ex.Message, ex);
        }

        #endregion

        #region Message + Confirm
        public virtual void CloseMessageBox()
        {
            if (CURRENT_MASTERPAGE != null)
            {
                Session["CR_MESSAGEBOXRESULT"] = null;
                CURRENT_MASTERPAGE.CloseMessageBox();
            }
        }

        public void OpenMessageBox(string title, string message)
        {
            if (CURRENT_MASTERPAGE != null) CURRENT_MASTERPAGE.OpenMessageBox(title, message);
        }

        public void OpenMessageBox(string title, string message, ButtonStyle submitStyle, ExtraIcon submitIcon, string submitText,
                                                                         ButtonStyle cancelStyle, ExtraIcon cancelIcon, string cancelText, MessageBoxResult result)
        {
            if (CURRENT_MASTERPAGE != null)
            {
                result.Submit = true;
                CurrentMessageBoxResult = result;
                CURRENT_MASTERPAGE.OpenMessageBox(title, message, submitStyle, submitIcon, submitText, cancelStyle, cancelIcon, cancelText);
            }
        }

        public void OpenMessageBox(string title, string message, ButtonStyle submitStyle, ExtraIcon submitIcon, string submitText)
        {
            if (CURRENT_MASTERPAGE != null)
            {
                CurrentMessageBoxResult.Submit = true;
                CURRENT_MASTERPAGE.OpenMessageBox(title, message, submitStyle, submitIcon, submitText);
            }
        }

        public virtual void MessageBoxSubmitHanlder()
        {

        }

        public virtual void MessageBoxCancelHanlder()
        {

        }
        #endregion

        #region Send via Amazon
        public static bool SendEmailViaAmazon(string accessKey, string secretAccessKey, string emailSender, string emailSenderAccount, string subject, string strMessage, List<string> emailToList)
        {
            foreach (string email in emailToList)
            {

            }
            return true;
        }
        #endregion

        protected string GetDecimalToString(object price)
        {
            return GetDecimalToString(price, "");
        }


        protected string GetDecimalToString(object price, string N0)
        {
            if (price != null)
            {
                decimal d = -1;
                if (decimal.TryParse(price.ToString(), out d)) return d.ToStringWithLanguage(N0);
            }
            return "";
        }
    }


    #region Control Event
    public delegate void ControlDialogEvent(object sender, ControlEventArgs e);

    public delegate void MenuClickEvent(object sender, ControlEventArgs e);


    public class ControlEventArgs
    {
        private string message = string.Empty;
        private object data = null;
        private DateTime date = DateTime.Now;
        public ControlEventArgs() { }
        public string Message { get { return message; } set { message = value; } }
        public object Data { get { return data; } set { data = value; } }
        public DateTime Date { get { return date; } set { date = value; } }
    }

    public class MessageBoxResult
    {
        private bool submit = false;
        private bool fireInControl = false;
        private object value = null;
        private string controlName = string.Empty;
        private string commandName = string.Empty;
        public MessageBoxResult() { this.Submit = true; }
        public bool Submit { get; set; }
        public object Value { get; set; }
        public bool FireInControl { get; set; }
        public string ControlName { get; set; }
        public string CommandName { get; set; }
    }
    #endregion

    #region Right
    public class RightPageName
    {

        #region Home
        public static string Home_Mau_New = "Home_Mau_New";
        public static string Home_Cho_Tra_Ket_Qua = "Home_Cho_Tra_Ket_Qua";
        public static string Home_Cho_Tra_Ket_Qua_Hoa = "Home_Cho_Tra_Ket_Qua_Hoa";
        public static string Home_Cho_Tra_Ket_Qua_Sinh = "Home_Cho_Tra_Ket_Qua_Sinh";
        public static string Home_Chua_Thanh_Toan = "Home_Chua_Thanh_Toan";
        public static string Home_Thanh_Ly = "Home_Thanh_Ly";
        
        #endregion

        #region Loại mẫu
        public static string Cat_LM_Insert = "Cat_LoaiMau_Insert";
        public static string Cat_LM_Update = "Cat_LoaiMau_Update";
        public static string Cat_LM_Delete = "Cat_LoaiMau_Delete";
        #endregion

        #region Phương pháp
        public static string Cat_PhuongPhap_Insert = "Cat_PhuongPhap_Insert";
        public static string Cat_PhuongPhap_Update = "Cat_PhuongPhap_Update";
        public static string Cat_PhuongPhap_Delete = "Cat_PhuongPhap_Delete";
        #endregion

        #region Quy chuẩn
        public static string Cat_QuyChuan_Insert = "Cat_QuyChuan_Insert";
        public static string Cat_QuyChuan_Update = "Cat_QuyChuan_Update";
        public static string Cat_QuyChuan_Delete = "Cat_QuyChuan_Delete";
        #endregion

        #region Chứng chỉ
        public static string Cat_ChungChi_Insert = "Cat_ChungChi_Insert";
        public static string Cat_ChungChi_Update = "Cat_ChungChi_Update";
        public static string Cat_ChungChi_Delete = "Cat_ChungChi_Delete";
        #endregion

        #region Thông tư
        public static string Cat_ThongTu_Insert = "ThongTu_Insert";
        public static string Cat_ThongTu_Update = "ThongTu_Update";
        public static string Cat_ThongTu_Delete = "ThongTu_Delete";
        #endregion
        #region Phiếu mẫu xét nghiệm
        public static string PMXN_Insert = "PMXN_Insert";
        public static string PMXN_Update = "PMXN_Update";
        public static string PMXN_Delete = "PMXN_Delete";
        public static string PMXN_ChuyenMau = "PMXN_ChuyenMau";
        #endregion

        #region Labo Hóa
        public static string Hoa_TNMSH_Update = "TNMHS_Update";
        public static string Hoa_TNMSH_Chot = "TNMHS_Chot";
        #endregion
        #region Labo Vi sinh
        public static string VS_TNMSH_Update = "TNMSH_Update";
        public static string VS_TNMSH_Chot = "TNMSH_Chot";
        #endregion

        #region Trả kết quả
        public static string PMXN_TraKetQua = "PMXN_TraKetQua";
        public static string PMXN_CapNhatIn = "PMXN_CapNhatIn";
        public static string PMXN_CapNhatThongTin = "PMXN_CapNhatThongTin";
        public static string PMXN_TLMXN = "PMXN_TLMXN";
        #endregion

        #region Kế toán
        public static string KeToa_TNMKT_Print = "TNMKT_Print";
        #endregion
        #region Chỉ tiêu
        public static string ChiTieu_Insert = "ChiTieu_Insert";
        public static string ChiTieu_Update = "ChiTieu_Update";
        public static string ChiTieu_Delete = "ChiTieu_Delete";
        public static string ChiTieu_Setting = "ChiTieu_Setting";
        #endregion 

        #region Loại yêu cầu
        public static string Cat_LoaiYeuCau_Insert = "Cat_LoaiYeuCau_Insert";
        public static string Cat_LoaiYeuCau_Update = "Cat_LoaiYeuCau_Update";
        public static string Cat_LoaiYeuCau_Delete = "Cat_LoaiYeuCau_Delete";
        #endregion
        #region Nhóm XN
        public static string Cat_NhomXN_Insert = "Cat_NhomXN_Insert";
        public static string Cat_NhomXN_Update = "Cat_NhomXN_Update";
        public static string Cat_NhomXN_Delete = "Cat_NhomXN_Delete";
        #endregion
        #region Giới hạn chỉ tiêu
        public static string GHCT_Insert = "GHCT_Insert";
        public static string GHCT_Update = "GHCT_Update";
        public static string GHCT_Delete = "GHCT_Delete";
        #endregion
        #region Hóa chất
        public static string HoaChat_Insert = "HoaChat_Insert";
        public static string HoaChat_Update = "HoaChat_Update";
        public static string HoaChat_Delete = "HoaChat_Delete";
        public static string HoaChat_FullDept = "HoaChat_FullDept";

        public static string Cat_HoaChat_Insert = "Cat_HoaChat_Insert";
        public static string Cat_HoaChat_Update = "Cat_HoaChat_Update";
        public static string Cat_HoaChat_Delete = "Cat_HoaChat_Delete";
        #endregion
        #region Vật tư tiêu hao
        public static string VTTH_Insert = "VTTH_Insert";
        public static string VTTH_Update = "VTTH_Update";
        public static string VTTH_Delete = "VTTH_Delete";
        public static string VTTH_FullDept = "VTTH_FullDept";

        public static string Cat_VTTH_Insert = "Cat_VTTH_Insert";
        public static string Cat_VTTH_Update = "Cat_VTTH_Update";
        public static string Cat_VTTH_Delete = "Cat_VTTH_Delete";
        #endregion

        #region Đơn vị tính
        public static string DVT_Insert = "DVT_Insert";
        public static string DVT_Update = "DVT_Update";
        public static string DVT_Delete = "DVT_Delete";
        #endregion

        #region Quy cách
        public static string QC_Insert = "QC_Insert";
        public static string QC_Update = "QC_Update";
        public static string QC_Delete = "QC_Delete";
        #endregion

        #region Nguồn kinh phí
        public static string NKP_Insert = "NKP_Insert";
        public static string NKP_Update = "NKP_Update";
        public static string NKP_Delete = "NKP_Delete";
        #endregion

        #region Phòng ban
        public static string PB_Insert = "PB_Insert";
        public static string PB_Update = "PB_Update";
        public static string PB_Delete = "PB_Delete";
        #endregion

        #region Kho
        public static string Kho_Insert = "Kho_Insert";
        public static string Kho_Update = "Kho_Update";
        public static string Kho_Delete = "Kho_Delete";
        #endregion

        #region Tổ nhân viên
        public static string TNV_Insert = "TNV_Insert";
        public static string TNV_Update = "TNV_Update";
        public static string TNV_Delete = "TNV_Delete";
        #endregion

        #region Nhóm hàng hóa
        public static string NHH_Insert = "NHH_Insert";
        public static string NHH_Update = "NHH_Update";
        public static string NHH_Delete = "NHH_Delete";
        #endregion

        #region Nhà cung cấp
        public static string NCC_Insert = "NCC_Insert";
        public static string NCC_Update = "NCC_Update";
        public static string NCC_Delete = "NCC_Delete";
        #endregion

        #region Quyền
        public static string Role_Insert = "Role_Insert";
        public static string Role_Update = "Role_Update";
        public static string Role_Delete = "Role_Delete";
        public static string Role_Setting = "Role_Setting";
        #endregion

        #region Người dùng
        public static string User_Admin_Insert = "Them_Moi_Nguoi_Dung";
        public static string User_Admin_Update = "Cap_Nhat_Nguoi_Dung";
        public static string User_Admin_Delete = "Xoa_Nguoi_Dung";
        public static string User_Admin_Reset = "User_Admin_Reset";
        #endregion

        #region Nhân viên
        public static string NV_Insert = "NV_Insert";
        public static string NV_Update = "NV_Update";
        public static string NV_Delete = "NV_Delete";
        public static string NV_Role = "NV_Role";
        #endregion

        #region Bảo trì bảo dưỡng
        public static string Bao_Tri_Bao_Duong_Insert = "Bao_Tri_Bao_Duong_Insert";
        public static string Bao_Tri_Bao_Duong_Update = "Bao_Tri_Bao_Duong_Update";
        public static string Bao_Tri_Bao_Duong_Delete = "Bao_Tri_Bao_Duong_Delete";
        public static string Bao_Tri_Bao_Duong_FullDept = "Bao_Tri_Bao_Duong_FullDept";
        #endregion

        #region Sử dụng hàng hóa
        public static string HHTH_Insert = "HHTH_Insert";
        public static string HHTH_Update = "HHTH_Update";
        public static string HHTH_Delete = "HHTH_Delete";
        public static string HHTH_FullDept = "HHTH_FullDept";
        #endregion

        #region Phiếu nhập kho  
        public static string Phieu_Nhap_Kho_Insert = "Phieu_Nhap_Kho_Insert";
        public static string Phieu_Nhap_Kho_Update = "Phieu_Nhap_Kho_Update";
        public static string Phieu_Nhap_Kho_Delete = "Phieu_Nhap_Kho_Delete";
        public static string Phieu_Nhap_Kho_Chot_Phieu = "Phieu_Nhap_Kho_Chot_Phieu";
        #endregion

        #region Phiếu xuất kho
        public static string Phieu_Xuat_Kho_Insert = "Phieu_Xuat_Kho_Insert";
        public static string Phieu_Xuat_Kho_Update = "Phieu_Xuat_Kho_Update";
        public static string Phieu_Xuat_Kho_Delete = "Phieu_Xuat_Kho_Delete";
        public static string Phieu_Xuat_Kho_Chot_Phieu = "Phieu_Xuat_Kho_Chot_Phieu";
        #endregion

        #region Thanh ly
        public static string TLHL_Insert = "TLHL_Insert";
        public static string TLHL_Delete = "TLHL_Delete";
        public static string TLVS_Insert = "TLVS_Insert";
        public static string TLVS_Delete = "TLHL_TLVS_DeleteInsert";
        #endregion
    }
    #endregion
}