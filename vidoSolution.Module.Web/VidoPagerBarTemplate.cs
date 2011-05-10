using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxPager;

namespace vidoSolution.Module.Web
{
    class VidoPagerBarTemplate : ITemplate
    {
        ASPxGridView grid;
        void ITemplate.InstantiateIn(Control container)
        {
            GridViewPagerBarTemplateContainer gridViewDataItemTemplateContainer = container as GridViewPagerBarTemplateContainer;
            if (gridViewDataItemTemplateContainer != null)
            {
                GridViewPagerBarTemplateContainer templateContainer = (GridViewPagerBarTemplateContainer)container;
                ASPxPager mypager =  new ASPxPager();
                grid = templateContainer.Grid;
                grid.SettingsPager.Summary.AllPagesText = "Trang {0}/{1} ({2} dòng dữ liệu)";
            }
        }
    }
}
