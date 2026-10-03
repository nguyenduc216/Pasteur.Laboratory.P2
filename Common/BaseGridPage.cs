
using SweetSoft.ExtraControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Common
{
    public class BaseGridPage : BasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AllowEmbedGridStyle = true;
        }

        protected virtual int GetRowIndex(SweetSoft.ExtraControls.ExtraGrid grData, int containIndex)
        {
            return (grData.CurrentPageIndex - 1) * grData.PageSize + containIndex + 1;
        }

        protected int GetIndex(ExtraGrid gridData, int idx)
        {
            return (gridData.CurrentPageSize * (gridData.CurrentPageIndex - 1)) + idx + 1;
        }

        #region Grid
        protected virtual void BaseGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                Literal ltrNoDataFound = e.Row.FindControl("ltrNoDataFound") as Literal;
                //if (ltrNoDataFound != null)
                //    ltrNoDataFound.Text = GetResourceText(ResourceHelper.BE.Grid_NodataFound);
            }
        }

        protected virtual void BaseGrid_NeedDataSource(object sender, ExtraGridEventArg e)
        {

        }
        #endregion
    }
}