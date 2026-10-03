using SweetSoft.ExtraControls.icons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SweetSoft.ExtraControls;
using SweetSoft.ExtraControls.html;
using System.Web.UI.HtmlControls;

namespace Pasteur.Laboratory.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:MathTextbox runat=\"server\"></{0}:MathTextbox>")]
    public class MathTextBox : HiddenField
    {
        protected override void OnInit(EventArgs e)
        {

        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            SweetSoft.ExtraControls.html. divOuter = new HtmlElement("div");
            divOuter.Attributes.Add("class", "math-outer");
            divOuter.Attributes.Add("id", string.Format("math_out_{0}", this.ClientID));
            divOuter.Attributes.Add("data-hid", string.Format("{0}", this.ClientID));

            HtmlGenericControl divContent = new HtmlGenericControl("div");
            divContent.Attributes.Add("class", "math-content");
            divContent.Attributes.Add("id", string.Format("math_content_{0}", this.ClientID));
            divContent.Attributes.Add("contenteditable", "true");
            divOuter.Controls.Add(divContent);

            this.Page.Controls.Add(divOuter);

            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
          
        }
    }
}