using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Controls
{
    public partial class HorizotalMenu : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltrAcount.Text = "AA";
                TblTaiKhoanNguoiDung u = Session[SessionName.User_CurrentUser] as TblTaiKhoanNguoiDung;
                if (u != null)
                {
                   
                    ltrUser.Text = string.Format("Xin chào {0}", u.Email);
                    //LoadMenu();
                }
                LoadMenu();
            }
        }

        private void LoadMenu()
        {
            bool b = false;
            string html = string.Empty;
            string html2 = string.Empty;
            List<TblChucNang> userFunction
                = FunctionManager.GetAllFunction(1, ApplicationID);
            List<TblChucNang> headFunction
                = userFunction.FindAll(p => string.IsNullOrEmpty(p.MaChucNangCha));
            if (headFunction != null)
            {
                foreach (TblChucNang F in headFunction)
                {
                    if (F.MaChucNang.ToUpper() == "HOME") continue;
                    bool isActive = false,b2 = false;
                    ApplicationUserRight right = CurrentUserRight.FirstOrDefault(p => p.FunctionCode.ToLower() == F.MaChucNang.ToLower());

                    string subHtml = LoadSubMenu(F, userFunction, ref isActive);
                    b |= (right != null || !string.IsNullOrEmpty(subHtml));

                    if (!string.IsNullOrEmpty(subHtml))
                        html += subHtml;
                    else if (b)
                    {
                        html += string.Format(@" <li class='{3}'>
                                                <a  href='{0}'><i class='{1}' style='font-size:20px;display:block;text-align:center;'></i>{2}
                                                 </a>
                                               </li>",
                                              F.Url, F.CssClass, string.Format("<span style='margin-left:5px;display:block;text-align:center;' class='menu-title'>{0}</span>", F.TenChucNang),
                                              FUNCTION_CODE.Equals(F.MaChucNang) ? "nav-item active" : "nav-item");
                        html2 += string.Format(@"
                             <li class='{2}'>
                                  < a class='nav-link' href='{0}'>{1}</a>
                                </li>
                        ", F.Url, F.TenChucNang, FUNCTION_CODE.Equals(F.MaChucNang) ? "nav-item active" : "nav-item");
                    }
                }
            }
            //html = "<nav class='navbar navbar-expand-sm bg-light navbar-light'>" + html + "</nav>";
            ltrHtml.Text = html;
        }

        private string LoadSubMenu(TblChucNang F, List<TblChucNang> lstFunction, ref bool active)
        {
            bool b = false;
            string result = string.Empty;
            if (CurrentUserRight.Count > 0)
            {
                List<TblChucNang> subFunction
                   = lstFunction.FindAll(p => !string.IsNullOrEmpty(p.Url) && p.MaChucNangCha == F.MaChucNang);
                if (subFunction.Count > 0)
                {
                    bool isActive = false;
                    foreach (TblChucNang SF in subFunction)
                    {
                        ApplicationUserRight right = CurrentUserRight.FirstOrDefault(p => p.FunctionCode.ToLower() == SF.MaChucNang.ToLower());

                        bool ac = false, b2 = false;
                        string subSubHtml = LoadSubMenu(SF, lstFunction, ref ac);
                        b2 |= (right != null || !string.IsNullOrEmpty(subSubHtml));
                        b |= b2;

                        isActive |= ac || FUNCTION_CODE.Equals(SF.MaChucNang.Trim());
                        if (b2) {
                            if (!string.IsNullOrEmpty(subSubHtml))
                                result += subSubHtml;
                            else
                                result += string.Format(@" <li >
                                                <a class='{3}' style='display:block !important;' href='{0}'>{2}
                                                 </a>
                                               </li>",
                                                        SF.Url, SF.CssClass, SF.TenChucNang,
                                                        FUNCTION_CODE.Equals(SF.MaChucNang.Trim()) ? "active" : string.Empty);
                        }
                    }
                    ApplicationUserRight right2 = CurrentUserRight.FirstOrDefault(p => p.FunctionCode.ToLower() == F.MaChucNang.ToLower());
                    b |= right2 != null;
                    if (b)
                    {
                        result = string.Format(@"<li class='dropdown mega-dropdown {2}'>
                                            <a href='#' class='dropdown-toggle' data-toggle='dropdown' ><i class='{0}'  style='font-size:20px;display:block;text-align:center;'></i>
                                                {1} 
                                               <span class='caret'></span>
                                            </a>
                                            <ul class='dropdown-menu mega-dropdown-menu {2}' >",
                                            F.CssClass, F.TenChucNang, isActive ? " active open" : "")
                           + result;
                        result += " </ul></li>";
                        active = isActive;
                    }
                }
            }

            lstFunction.RemoveAll(p => !string.IsNullOrEmpty(p.MaChucNang) &&
                                       p.MaChucNangCha == F.MaChucNang);

            return result;
        }

    }
}