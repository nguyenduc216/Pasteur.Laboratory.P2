using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2
{
    public partial class BangTongKetXetNghiem : BasePage
    {
        public override bool IsUserRole => false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadChiTieu();
            }
        }

        private void LoadChiTieu()
        {
            List<TblChiTieuXetNghiem> chiTieuCol = ChiTieuManager.GetChiTieuXetNghiemByAll(true);
            ddlChiTieu.DataSource = chiTieuCol;
            ddlChiTieu.DataValueField = TblChiTieuXetNghiem.Columns.Id;
            ddlChiTieu.DataTextField = TblChiTieuXetNghiem.Columns.TenTaiCacKhoa;
            ddlChiTieu.DataBind();
        }
    }
}