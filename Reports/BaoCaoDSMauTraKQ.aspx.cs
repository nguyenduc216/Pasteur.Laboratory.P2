using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.Core;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;

namespace Pasteur.Laboratory.P2.Reports
{
    public partial class BaoCaoDSMauTraKQ : BasePage
    {
        #region Properties
        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_ChiTieu;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_ChiTieu;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Báo cáo chỉ tiêu";

                return rs;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}