using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;
using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.Core;
using Pasteur.DataAccess;
using SweetSoft.ExtraControls;


namespace Pasteur.Laboratory.P2.Controls
{
    public partial class QLTTLT_UCRightInRole : Pasteur.Laboratory.P2.Common.BaseUserControl
    {
        public int? CurrentRoleID
        {
            set
            {
                ViewState[string.Format("{0}_ID", FUNCTION_CODE)] = value;
            }
            get
            {
                int? b = null;
                int bi = 0;
                if (ViewState[string.Format("{0}_ID", FUNCTION_CODE)] != null)
                    if (int.TryParse(ViewState[string.Format("{0}_ID", FUNCTION_CODE)].ToString(), out bi))
                        b = bi;
                return b;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadRightFunction(TblVaiTro role)
        {
            CurrentRoleID = role.Id;
            ListRight = RoleManager.GetRightsOfRole(role.Id, true);
            rptFunction.DataSource = GetFunctionList(1);
            rptFunction.DataBind();
        }   

        private List<TblChucNang> GetFunctionList(int roleType)
        {
            List<TblChucNang> funcs = FunctionManager.GetFunctionByRoleType(roleType,ApplicationID);
            Session["RoleFunction"] = funcs;
            return Session["RoleFunction"] as List<TblChucNang>;
        }

        private List<ApplicationUserRight> ListRight
        {
            get
            {
                if (Session["RightInRole"] != null)
                    return Session["RightInRole"] as List<ApplicationUserRight>;
                return new List<ApplicationUserRight>();
            }
            set
            {
                Session["RightInRole"] = value;
            }
        } 

        public bool Save()
        {
            bool save = false;
            foreach (RepeaterItem item in rptFunction.Items)
            {
                Repeater rptRight = item.FindControl("rptRight") as Repeater;
                if (rptRight != null)
                {
                    foreach (RepeaterItem item2 in rptRight.Items)
                    {
                        HiddenField hdfRightInRoleID
                            = item2.FindControl("hdfRightInRoleID") as HiddenField;
                        HiddenField hdfRightID
                            = item2.FindControl("hdfRightID") as HiddenField;
                        CheckBox chkItem
                            = item2.FindControl("chkItem") as CheckBox;

                        if (chkItem != null)
                        {
                            int r = -1;
                            if (hdfRightID != null)
                            {
                                if (!int.TryParse(hdfRightID.Value, out r))
                                    continue;
                            }
                            else
                                continue;

                            if (chkItem.Checked)
                            {
                                if (hdfRightInRoleID != null)
                                {
                                    if (string.IsNullOrEmpty(hdfRightInRoleID.Value))
                                    {
                                        TblVaiTroVaQuyen rightInRole = new TblVaiTroVaQuyen();
                                        rightInRole.IDQuyen = r;
                                        rightInRole.IDVaiTro = CurrentRoleID;
                                        save |= RoleManager.InsertRightInRole(rightInRole) != null;
                                    }
                                }
                            }
                            else
                            {
                                if (hdfRightInRoleID != null)
                                {
                                    if (!string.IsNullOrEmpty(hdfRightInRoleID.Value))
                                    {
                                        save |= RoleManager.DeleteRightInRole(hdfRightInRoleID.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (ApplicationContext.Current.CommonUser != null)
            {
                TblTaiKhoanNguoiDung user = ApplicationContext.Current.CommonUser;
                Session[SessionName.Right_CurrentUserRight] = RoleManager.GetRightByCommonUserID(user.IDTaiKhoan);
            }
            return save;
        }

        protected void rptFunction_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null &&
                (e.Item.ItemType == ListItemType.Item ||
                 e.Item.ItemType == ListItemType.AlternatingItem))
            {
                TblChucNang function = e.Item.DataItem as TblChucNang;
                if (function != null)
                {
                    bool b = !string.IsNullOrEmpty(function.Url) && function.Url.Trim() != "#"; // Có page chức năng
                    e.Item.Visible = b;

                    //e.Item.Visible = !string.IsNullOrEmpty(function.ParentCode) &&
                    //                 !function.DisplayOrder.Contains("2.1.") &&
                    //                 !function.FunctionCode.Contains("LocalHouse_Management");
                    Label lbFunctionName
                        = e.Item.FindControl("lbFunctionName") as Label;
                    ExtraCheckBox chkFunction
                        = e.Item.FindControl("chkFunction") as ExtraCheckBox;
                    Repeater rptRight
                        = e.Item.FindControl("rptRight") as Repeater;
                    Literal ltrRowOpen
                        = e.Item.FindControl("ltrRowOpen") as Literal;

                    if (chkFunction != null)
                    {
                        chkFunction.EnableClickGroup = true;
                        chkFunction.GroupName = "check-" + function.Id;
                    }
                    if (lbFunctionName != null)
                        lbFunctionName.Text = function.TenChucNang;
                    if (rptRight != null && ListRight != null && ListRight.Count > 0)
                    {
                        List<ApplicationUserRight> currentRight
                            = ListRight.FindAll(p => p.FunctionCode.Trim() == function.MaChucNang.Trim());
                        rptRight.DataSource = currentRight;
                        rptRight.DataBind();
                        chkFunction.Checked
                            = currentRight.FirstOrDefault(p => p.RightInRoleID.HasValue) != null;
                    }

                    if (ltrRowOpen != null)
                        ltrRowOpen.Text = string.Format("<tr class='row-section' role='{0}'>", function.Id);

                }
            }
        }

        protected void rptRight_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.DataItem != null &&
                (e.Item.ItemType == ListItemType.Item ||
                 e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ApplicationUserRight right = e.Item.DataItem as ApplicationUserRight;
                if (right != null)
                {
                    Label lbRightName
                        = e.Item.FindControl("lbRightName") as Label;
                    Label lbDescription
                        = e.Item.FindControl("lbDescription") as Label;
                    HiddenField hdfRightInRoleID
                        = e.Item.FindControl("hdfRightInRoleID") as HiddenField;
                    HiddenField hdfRightID
                        = e.Item.FindControl("hdfRightID") as HiddenField;
                    CheckBox chkItem
                        = e.Item.FindControl("chkItem") as CheckBox;
                    Literal ltrRowOpen
                        = e.Item.FindControl("ltrRowOpen") as Literal;

                    if (lbRightName != null)
                        lbRightName.Text = right.RightName;
                    if (lbDescription != null)
                        lbDescription.Text = right.RightDescription;
                    if (hdfRightID != null && right.RightID.HasValue)
                        hdfRightID.Value = right.RightID.Value.ToString();
                    if (hdfRightInRoleID != null && right.RightInRoleID.HasValue)
                        hdfRightInRoleID.Value = right.RightInRoleID.Value.ToString();
                    if (chkItem != null)
                    {
                        chkItem.Checked = right.RightInRoleID.HasValue;
                        chkItem.CssClass += " check-" + right.FunctionID.Value;
                    }
                    if (ltrRowOpen != null)
                        ltrRowOpen.Text = string.Format("<tr class='hide role-{0}'>", right.FunctionID);
                }
            }
        }
    }
}