using System;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.TestScripts;
using DevExpress.ExpressApp.Web;
using System.Web.UI;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;

public partial class LoginPage : System.Web.UI.Page, IWindowTemplate, IViewSiteTemplate, ISupportViewControlAdding
{
    protected override void InitializeCulture()
    {
        base.InitializeCulture();
        WebApplication.Instance.InitializeCulture();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        WebApplication.Instance.CreateLogonControls(this);
    }
    #region IFrameTemplate Members
    private bool isSizeable;
    public IActionContainer DefaultContainer
    {
        get { return null; }
    }
    public ICollection<IActionContainer> GetContainers()
    {
        return PopupActions.ActionContainers;
    }
    public void SetView(DevExpress.ExpressApp.View view)
    {
        this.ViewSite.Controls.Clear();
        if (view != null)
        {
            view.CreateControls();
            if (ViewControlAdding != null)
            {
                ViewControlAdding(this, EventArgs.Empty);
            }
            ViewSite.Controls.Add((Control)view.Control);
            ViewCaptionLabel.Text = view.Caption;
            Header.Title = view.Caption;
            ImageInfo imageInfo = ImageLoader.Instance.GetLargeImageInfo(ViewImageNameHelper.GetImageName(view));
            if (imageInfo.IsEmpty)
            {
                ViewImage.Visible = false;
            }
            else
            {
                ViewImage.ImageUrl = imageInfo.ImageUrl;
            }
        }
    }
    public bool IsSizeable
    {
        get { return isSizeable; }
        set { isSizeable = value; }
    }
    public object ViewSiteControl
    {
        get { return ViewSite; }
    }
    public void SetCaption(string caption) { }
    public void SetStatus(System.Collections.Generic.ICollection<string> statusMessages) { }

    public event EventHandler ViewControlAdding;
    #endregion
}
