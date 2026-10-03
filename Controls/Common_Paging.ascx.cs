
using SweetSoft.ExtraControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Pasteur.Laboratory.P2.Controls
{
    public partial class Common_Paging : ExtraGridPaging
    {
        string defaultMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rptPager.Visible = ddlPageSize.Visible = divPaging.Visible = this.Visible;
                ddlPageSize.ClearSelection();
                ddlPageSize.SelectedValue = "15";
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            int vl = 10;
            if (!int.TryParse(ddlPageSize.SelectedValue, out vl)) vl = 10;
            ExtraPagingEventArg ev = new ExtraPagingEventArg();
            ev.PageSize = vl;
            PageSizeChangeBegin(this, ev);
        }

        protected void lnkPage_Click(object sender, EventArgs e)
        {
            LinkButton btn = sender as LinkButton;
            if (btn != null)
            {
                int c_Page = 1;
                string cmdArg = btn.CommandArgument;
                switch (cmdArg)
                {
                    case "first":
                        PageCurrentIndex = 1;
                        break;
                    case "previous":
                        PageCurrentIndex -= 1;
                        break;
                    case "next":
                        PageCurrentIndex += 1;
                        break;
                    case "last":
                        PageCurrentIndex = PageTotal;
                        break;
                    default:
                        int vl = 1;
                        if (!int.TryParse(cmdArg, out vl))
                            vl = 1;
                        PageCurrentIndex = vl;
                        break;
                }

                ExtraPagingEventArg ev = new ExtraPagingEventArg();
                ev.PageIndex = PageCurrentIndex;
                PageChangeBegin(this, ev);
            }
        }



        #region Override Function
        public override void RenderButtonControl(int pageStart, int pageSize, int pageCount, int VirtualCount)
        {
            if (!this.Visible) return;
            PageTotal = VirtualCount / pageSize;
            if (VirtualCount % pageSize > 0) PageTotal += 1;
            if (!AlwayShowPaging)
                this.Visible = PageTotal > 0;
            liFirstPage.Visible = liPreviousPage.Visible = pageStart > 1;
            liLastPage.Visible = liNextPage.Visible = pageStart < PageTotal;

            if (pageStart + 2 > PageEndIndex && pageStart + 2 < PageTotal) PageBeginIndex += 1;

            if (pageStart + pageCount - 1 > PageTotal)
            {
                PageEndIndex = PageTotal;
                PageBeginIndex = PageEndIndex - pageCount;
            }
            else if (pageStart - (pageCount / 2) < PageTotal)
            {
                PageBeginIndex = pageStart - (pageCount / 2);
                if (PageBeginIndex < 1) PageBeginIndex = 1;
                PageEndIndex = PageBeginIndex + pageCount;
            }


            if (PageEndIndex > PageTotal) PageEndIndex = PageTotal;
            if (pageStart > PageTotal)
            {
                if (PageTotal > 0)
                {
                    ExtraPagingEventArg ev = new ExtraPagingEventArg();
                    ev.PageIndex = 1;
                    PageChangeBegin(this, ev);
                }
                return;
            }

            if (PageBeginIndex <= 0)
            {
                if (VirtualCount > 0) PageBeginIndex = 1;
                else PageBeginIndex = 0;
            }

            int begin = PageBeginIndex;
            List<PagingItem> lst = new List<PagingItem>();
            ltrPage.Text = string.Format(" Trang <b>1</b> / tổng số <b>{0}</b> trang.", PageTotal);
            for (; begin <= PageEndIndex; begin++)
            {
                PagingItem item = new PagingItem();
                item.Value = begin;
                item.Text = begin.ToString();
                item.CssClass = begin == pageStart ? " class='active'" : string.Empty;
                if (begin == pageStart)
                    ltrPage.Text = string.Format(" Trang <b>{0}</b> / tổng số <b>{1}</b> trang.", begin, PageTotal);
                lst.Add(item);
            }
            PageCurrentIndex = pageStart;
            rptPager.DataSource = lst;
            rptPager.DataBind();

            ListItem item1 = ddlPageSize.Items.FindByValue(pageSize.ToString());
            if (item1 != null) item1.Selected = true;
            else
            {
                int idx = 0;
                for (int i = 0; i < ddlPageSize.Items.Count; i++)
                {
                    if (int.Parse(ddlPageSize.Items[i].Value) > pageSize) { idx = i; break; }
                }
                item1 = new ListItem();
                item1.Text = pageSize.ToString();
                item1.Value = pageSize.ToString();
                ddlPageSize.Items.Insert(idx, item1);
            }

            //ltrMessage.Text = string.Format("Trang <b>{0}</b> in <b>{1}</b> {2}, {3} <b>{4}</b> {5}.", pageStart, PageTotal, PageTotal > 1 ? "pages" : "page", BindingWithFiltering ? "found" : "total is", VirtualCount, VirtualCount > 1 ? "records" : "record");
        }

        public override void SetMessage(string message)
        {
            ltrMessage.Text = message;
        }
        #endregion
    }
}