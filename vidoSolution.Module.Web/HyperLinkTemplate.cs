using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using System.Web;
using DevExpress.ExpressApp.Web;

namespace vidoSolution.Module.Web
{
    public class HyperLinkTemplate : ITemplate
    {
        void ITemplate.InstantiateIn(Control container)
        {
            GridViewDataItemTemplateContainer gridViewDataItemTemplateContainer = container as GridViewDataItemTemplateContainer;
            if (gridViewDataItemTemplateContainer != null)
            {

                ASPxHyperLink link = RenderHelper.CreateASPxHyperLink();
                link.EncodeHtml = false;
                link.Text = gridViewDataItemTemplateContainer.Text;
                link.NavigateUrl = gridViewDataItemTemplateContainer.Text.Contains("://") ? gridViewDataItemTemplateContainer.Text :
                     "http://" + HttpContext.Current.Request.Url.Authority + gridViewDataItemTemplateContainer.Text;
                link.Target = "_blank";
                //ASPxHtmlEditor htmlEditor = new ASPxHtmlEditor();
                //htmlEditor.ActiveView = HtmlEditorView.Preview;
                //htmlEditor.Settings.AllowHtmlView = false;
                //htmlEditor.Settings.AllowDesignView = false;
                //htmlEditor.Html = "<a href='" + link.NavigateUrl + "'>" + link.Text + "</a>";
                //container.Controls.Add(htmlEditor);
                container.Controls.Add(link);
            }
        }

    }

}
