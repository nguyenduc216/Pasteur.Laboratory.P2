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
    public partial class QLTTLT_UCUserInRole : Pasteur.Laboratory.P2.Common.BaseUserControl
    {
        #region Properties
        private bool cr_ishotel = false;

        public bool IsHotelRole
        {
            get
            {
                return cr_ishotel;
            }
            set
            {
                cr_ishotel = value;
            }
        }

        public Guid CurrentUserID
        {
            get
            {
                Guid g = Guid.Empty;
                if (ViewState["CurrentUserID"] != null)
                {
                    g = new Guid(ViewState["CurrentUserID"].ToString());
                }
                return g;
            }
            set
            {
                ViewState["CurrentUserID"] = value.ToString();
            }
        }

        private bool cr_isadmin = true;

        public bool IsAdmin
        {
            get
            {
                return cr_isadmin;
            }
            set
            {
                cr_isadmin = value;
            }
        }

        private List<ApplicationUserRight> CurrentRoleList
        {
            get
            {
                if (Session["CurrentRole"] != null)
                    return Session["CurrentRole"] as List<ApplicationUserRight>;
                return new List<ApplicationUserRight>();
            }
            set
            {
                Session["CurrentRole"] = value;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void LoadRole(TblTaiKhoanNguoiDung user)
        {
            LoadRole(user.IDTaiKhoan, user.QuanTriVien);
        }

        public void LoadRole(Guid id, bool? isAdmin)
        {
            CurrentUserID = id;
            CurrentRoleList = RoleManager.GetRoleOfUser(id, true);

            List<ApplicationUserRight> lstEmpty = new List<ApplicationUserRight>();
            lstEmpty.Add(new ApplicationUserRight());

            grvRole.DataSource = CurrentRoleList;
            grvRole.DataSourceEmpty = lstEmpty;
            grvRole.Rebind();
        }

        public bool Save()
        {
            bool save = false;
            foreach (GridViewRow row in grvRole.Rows)
            {
                CheckBox chkItem = row.FindControl("chkItem") as CheckBox;
                HiddenField hdfRoleID = row.FindControl("hdfRoleID") as HiddenField;
                HiddenField hdfUserInRoleID = row.FindControl("hdfUserInRoleID") as HiddenField;

                if (chkItem != null && hdfUserInRoleID != null && hdfRoleID != null)
                {
                    if (chkItem.Checked)
                    {
                        int roleId = -1;
                        if (int.TryParse(hdfRoleID.Value, out roleId) )
                        {
                            if (string.IsNullOrEmpty(hdfUserInRoleID.Value))
                            {
                                TblVaiTroVaNguoiDung uRole = new TblVaiTroVaNguoiDung();
                                uRole.IDTaiKhoanNguoiDung = CurrentUserID;
                                uRole.IDVaiTro = roleId;
                                save |= RoleManager.InsertUserInRole(uRole) != null;
                            }
                            else save |= true;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(hdfUserInRoleID.Value))
                        {
                            save |= RoleManager.DeleteUserInRole(hdfUserInRoleID.Value);
                        }
                    }
                }
            }
            return save;
        }
    }
}