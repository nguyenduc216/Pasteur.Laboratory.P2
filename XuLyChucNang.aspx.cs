using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.DataAccess;
using Pasteur.Laboratory.P2.Common;
using SubSonic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2
{
    public partial class XuLyChucNang : BasePage
    {
        public override bool IsUserRole => false;
        private string CurrentRole
        {
            get { return CommonHelper.QueryString("Role"); }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            btnXuLyMaNoiBo.Visible = CurrentRole.StartsWith("Admin");
        }

        protected void btnXuLyMaNoiBo_Click(object sender, EventArgs e)
        {
            List<TblMauXetNghiem> mauXetNghiem = XetNghiemManager.Temp_GetMauXetNghiemXuLyMaNoiBo();
            int i = 0;
            foreach (TblMauXetNghiem mxn in mauXetNghiem)
            {
                i++;
                if (!string.IsNullOrEmpty(mxn.MaSoTraHoaLy))
                {
                    string[] maMau = mxn.MaSoTraHoaLy.ToString().Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
                    if (maMau.Length == 3) mxn.MaSoTraHoaLyIndex = string.Format("{0}/{1}/{2}", maMau[1], maMau[2], maMau[0]);
                }
                if (!string.IsNullOrEmpty(mxn.MaSoTraViSinh))
                {
                    string[] maMau2 = mxn.MaSoTraViSinh.ToString().Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
                    if (maMau2.Length == 3) mxn.MaSoTraViSinhIndex = string.Format("{0}/{1}/{2}", maMau2[1], maMau2[2], maMau2[0]);
                }
                XetNghiemManager.UpdateMauXetNghiem(mxn);

            }
            string msg = string.Format("Đã xử lý {0} mẫu nội bộ", i);
            mauXetNghiem = XetNghiemManager.Temp_GetMauXetNghiemXuLyMaTraKQ();
            i = 0;
            string sql = "";
            foreach (TblMauXetNghiem mxn in mauXetNghiem)
            {
                i++;
                if (!string.IsNullOrEmpty(mxn.MaSoTraKQ))
                {
                    string[] maMau = mxn.MaSoTraKQ.ToString().Split(new string[] { @"/" }, StringSplitOptions.RemoveEmptyEntries);
                    if (maMau.Length == 3) mxn.MaSoTraKQIndex = string.Format("{0}/{1}/{2}", maMau[1], maMau[2], maMau[0]);
                    sql += string.Format("update TblMauXetNghiem set MaSoTraKQIndex = N'{0}' where ID = {1};", mxn.MaSoTraKQIndex, mxn.Id);
                }
               
                //XetNghiemManager.UpdateMauXetNghiem(mxn);

            }
            new InlineQuery().Execute(sql);
            msg += string.Format(" và xử lý {0} mã trả KQ", i);
            ltrMes.Text = msg;
        }

        protected void btnXuLyDonViTinh_Click(object sender, EventArgs e)
        {
            int i = 0;
            List<TblMauXetNghiemChiTieu> chiTieu = XetNghiemManager.Temp_GetMauXetNghiemChiTieu();
            foreach (TblMauXetNghiemChiTieu ct in chiTieu)
            {
                ct.TenDonViNA = ct.TempDVTQC;
                if (string.IsNullOrEmpty(ct.TenDonViNA)) ct.TenDonViNA = ct.TempDVTCT;
                XetNghiemManager.UpdateMauChiTieu(ct);
                i++;
            }
            ltrMes.Text = string.Format("Đã xử lý {0} chỉ tiêu", i);
        }
    }
}