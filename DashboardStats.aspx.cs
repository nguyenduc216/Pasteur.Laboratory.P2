using Pasteur.Laboratory.P2.Common;
using System;

namespace Pasteur.Laboratory.P2
{
    // Code: DASHBOARD-PARALLEL-LOAD-20260918 - Endpoint độc lập tải thống kê dashboard.
    public partial class DashboardStats : BasePage
    {
        public override bool IsUserRole { get { return false; } }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            // Code: DASHBOARD-PARALLEL-LOAD-20260918 - Không khởi tạo menu và truy vấn menu trong endpoint iframe.
            System.Web.UI.Control menu = Master.FindControl("horizotalMenu");
            if (menu != null) menu.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e) { }
    }
}
