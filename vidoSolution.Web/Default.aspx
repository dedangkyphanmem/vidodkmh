<%@ Page Language="C#" AutoEventWireup="true" Inherits="DefaultVertical" EnableViewState="false" validateRequest="false" Codebehind="Default.aspx.cs" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v10.1" Namespace="DevExpress.ExpressApp.Web.Templates.ActionContainers" TagPrefix="cc2" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v10.1" Namespace="DevExpress.ExpressApp.Web.Controls" TagPrefix="cc4" %>
<%@ Register Assembly="DevExpress.ExpressApp.Web.v10.1" Namespace="DevExpress.ExpressApp.Web.Templates.Controls" TagPrefix="tc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Trang chủ</title>
	<meta http-equiv="content-type" content="text/html; charset=UTF-8" />
</head>
<body onload="OnLoad()" class="VerticalTemplate">
    <div class="bar"></div>
 <div style="width:100%; margin:0 auto; padding:0; height:120px;">
     <table class='tblHeader fullWidth' width='100%' cellpadding='0' cellspacing='0'>
                                    <tbody>
                                        <tr class='trHeaderBar'>
                                            <td>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table class='tblLogo fullWidth' cellpadding='0' cellspacing='0'>

                                                    <tbody>
                                                        <tr>
                                                            <td class='tdLogo'>
                                                                <embed width="156" height="120" src="../Images/Logo.swf" wmode="transparent" quality="high"
                                                                    type="application/x-shockwave-flash" />
                                                            </td>
                                                            <td class='tdBanner'>
                                                                <embed width="800" height="120" src="../Images/banner.swf" wmode="transparent"
                                                                    quality="high" type="application/x-shockwave-flash" />
                                                            </td>
                                                        </tr>

                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>

    </div>
    <div class="bar"></div>
    <div id="PageContent" class="PageContent">
    <script src="MoveFooter.js" type="text/javascript"></script>
	<cc4:ProgressControl ID="ProgressControl" runat="server" ImageName="~/Images/Progress.gif" CssClass="Progress" Text="" />
	<form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" border="0" class="Top" width="100%">
            <tr>
                <td class="Logo">
                <h1>WEB quản lý học vụ và đăng ký môn học </h1>
                    <%--<asp:HyperLink runat="server" NavigateUrl="~/default.aspx" ID="LogoLink">
                        <asp:Image ID="LogoImage" BorderWidth="0px" runat="server" Visible="false"  />
                    </asp:HyperLink>--%>
                </td>                
                <td class="Security">
                <table cellpadding="2" cellspacing="2" border="0" class="Top" width="100%">
                    <tr>
                    <td>
                        <cc2:ActionContainerHolder runat="server" ID="SecurityActionContainer" CssClass="Security" Categories="Security" ContainerStyle="Links" />
                        </td>
                    </tr>
                    <tr>
                    <td>   
                    <asp:Label ID="currentSemester" CssClass="Security" runat="server"></asp:Label>
                    </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-bottom: 5px;">
            <tr class="Content">
	            <td class="Content WithPaddings">
		            <table border="0" cellpadding="0" cellspacing="0" class="Search" width="100%">
			            <tr>
			                <td class="left" align="left"><cc2:ActionContainerHolder ID="VerticalNewActionContainer" runat="server" Categories="RootObjectsCreation" ContainerStyle="Links" /></td>
				          <%--  <td class="right" align="right"><cc2:ActionContainerHolder  ID="SearchActionContainer" runat="server" Categories="Search;FullTextSearch" ContainerStyle="Buttons" /></td>--%>
			            </tr>
		            </table>
                </td>
            </tr>
        </table>
		<table border="0" cellpadding="0" cellspacing="0" class="Main" width="100%">
			<tr>
			    <td class="Left" id="left_panel">
			        <cc2:NavigationActionContainer ID="NavigationBarActionContainer" runat="server" CssClass="xafNavigationBarActionContainer" ContainerId="ViewsNavigation" AutoCollapse="True"/>
                    <dxrp:ASPxRoundPanel ID="ToolsRoundPanel" runat="server" Width="100%" CssClass="ToolsActionContainerPanel" HeaderText="Tools">
                     <PanelCollection>
                         <dxrp:PanelContent ID="PanelContent1" runat="server">
                               <cc2:ActionContainerHolder id="VerticalToolsActionContainer" runat="server" Categories="Tools" Orientation="Vertical" ContainerStyle="Links" ShowSeparators="False"/>
                         </dxrp:PanelContent>
                     </PanelCollection>
                    </dxrp:ASPxRoundPanel>
                </td>
				<td class="Right">
				    <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100%" HeaderText="">
				        <HeaderTemplate>
                            <cc2:NavigationHistoryActionContainer ID="ViewsHistoryNavigationContainer" runat="server" CssClass="NavigationHistoryLinks" ContainerId="ViewsHistoryNavigation" Delimiter=" / "/>				    
				        </HeaderTemplate>
                    <PanelCollection>
                        <dxrp:PanelContent ID="PanelContent2" runat="server">
		            <table border="0" cellpadding="0" cellspacing="0" class="MainContent" width="100%">
			            <tr class="Content">
				            <td class="Content">
		                        <table cellpadding="0" cellspacing="0" border="0">
			                        <tr><td class="ViewImage">
					                        <asp:Image ID="ViewImage" runat="server" /></td>
				                        <td class="ViewCaption">
					                        <h1><asp:Label ID="ViewCaptionLabel" runat="server" Text="Contact list"></asp:Label></h1>
				                        </td>
			                        </tr>
		                        </table>
		                        <cc2:ActionContainerHolder runat="server" ID="ToolBar" CssClass="ToolBar" ContainerStyle="ToolBar" Orientation="Horizontal"  Categories="ObjectsCreation;Save;Edit;UndoRedo;RecordEdit;View;Reports;RecordsNavigation;Filters" >
	                            <%--    <cc2:WebActionContainer ID="ContextObjectsCreationActionContainer" runat="server" ContainerId="ObjectsCreation" />
			                        <cc2:WebActionContainer ID="TopRecordEditActionContainer" runat="server" ContainerId="RecordEdit" />
			                        <cc2:WebActionContainer ID="RecordsNavigationContainer" runat="server" ContainerId="RecordsNavigation" />
			                        <cc2:WebActionContainer ID="ViewPresentationActionContainer" runat="server" ContainerId="View" />
			                        <cc2:WebActionContainer ID="ListViewDataManagementActionContainer" runat="server" ContainerId="Filters" />--%>
		                        </cc2:ActionContainerHolder>
					            <tc:ErrorInfoControl ID="ErrorInfo" style="margin: 10px 0px 10px 0px" runat="server"></tc:ErrorInfoControl>
                                <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0" >
                                    <asp:TableRow ID="TableRow2" runat="server">
                                        <asp:TableCell runat="server" ID="ViewSite">views content here</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <cc2:ActionContainerHolder runat="server" ID="ActionContainerHolder1" CssClass="ToolBar" ContainerStyle="ToolBar" Orientation="Horizontal"  Categories="ObjectsCreation;Save;Edit;UndoRedo;RecordEdit;View;Reports;RecordsNavigation;Filters" >
                                </cc2:ActionContainerHolder>
            	                <div id="Spacer" class="Spacer"></div>
		                    </td>
	                    </tr>
			            <tr class="Content">
				            <td class="Content Links" align="center"><cc2:QuickAccessNavigationActionContainer CssClass="NavigationLinks" ID="QuickAccessNavigationActionContainer" runat="server" ContainerId="ViewsNavigation" ImageTextStyle="Caption" ShowSeparators ="True"/></td>
	                    </tr>
                    </table>
                    </dxrp:PanelContent>
                    </PanelCollection>
                    </dxrp:ASPxRoundPanel>
                </td>
			</tr>
			<tr class="Footer">
			    <td class="Left">
			    </td>
			    <td class="Right">                   			    
			    </td>
			</tr>
		</table>
	</form>
	<script type="text/javascript">
	<!--
	    function OnLoad() {
	        DXMoveFooter();
            DXattachEventToElement(window, "resize", DXWindowOnResize);
        }
    //-->	  
     
        window.__OriginalDXUpdateRowCellsHandler = ASPxClientTableFixedColumnsHelper.prototype.UpdateRowCells;
        ASPxClientTableFixedColumnsHelper.prototype.UpdateRowCells = function(row, startIndex, endIndex, display) {
            if ((row.cells.length == 0) || (row.cells[0].getAttribute("ci") == null))
                window.__OriginalDXUpdateRowCellsHandler(row, startIndex, endIndex, display); // base call
            else
            {
                //custom processing
                for(var i = startIndex; i < endIndex; i ++) {
                    var cell = FindCellWithColumnIndex(row, i);
                    if(cell != null)
                        cell.style.display = display;
                }
            }
        };
        function FindCellWithColumnIndex(row, colIndex)
        {
            for(var i = 0; i < row.cells.length; i ++) 
            {
                if (row.cells[i].getAttribute("ci") == colIndex)
                    return row.cells[i];
            }
            
            return null;
        }

      
	</script>
	</div>
    <div class="bar"></div>
     <div style="width:100%; height:30px; margin:0 auto;" >
        <table class='tblFooter fullWidth' cellpadding='0' cellspacing='0'>
                                    <tbody>
                                        <tr>
                                            <td align="center" class="tdLinks">
                                                <span id="dnn_dnnLINKS_lblLinks"><a class="dnnLinks" href="http://www.vido.edu.vn/Trangchủ/tabid/36/Default.aspx">Trang chủ</a><span class="dnnLinks"> | </span><a class="dnnLinks" href="http://www.vido.edu.vn/Hợptácquốctế/tabid/82/Default.aspx">Hợp tác quốc tế</a><span class="dnnLinks"> | </span><a class="dnnLinks" href="http://www.vido.edu.vn/Côngtácchínhtrịquảnlýsinhviên/tabid/250/Default.aspx">Công tác chính trị quản lý sinh viên</a><span class="dnnLinks"> | </span><a class="dnnLinks" href="http://www.vido.edu.vn/Diễnđàn/tabid/63/Default.aspx">Diễn đàn</a><span class="dnnLinks"> | </span><a class="dnnLinks" href="http://www.vido.edu.vn/Tuyểndụng/tabid/132/Default.aspx">Tuyển dụng</a><span class="dnnLinks"> | </span><a class="dnnLinks" href="http://www.vido.edu.vn/Liênhệ/tabid/108/Default.aspx">Liên hệ</a></span>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="dnn_FooterPane" valign="top" class="tdFooterPane">
                                            <a name="427"></a>
<table class="fullWidth" cellpadding="0" cellspacing="0">
    <tbody>
        <tr>
            <td class="tdContainerSolPartActions">

                
            </td>
        </tr>
        <tr>
            <td id="dnn_ctr427_ContentPane" valign="top" class="tdContainerContentPane" align="left">
            <!-- Start_Module_427 --><div id="dnn_ctr427_ModuleContent">
	<div id="dnn_ctr427_HtmlModule_HtmlModule_lblContent" class="Normal">
	<table cellspacing="1" cellpadding="1" border="0" width="100%" style="color: rgb(85, 85, 85);">
    <tbody>
        <tr>

            <td width="28%"><strong>TRƯỜNG CAO ĐẲNG VIỄN Đ&Ocirc;NG</strong><br />
            207/20/1 Nguyễn Văn Đậu, P.11, Q. B&igrave;nh Thạnh, Tp.HCM<br />
            <strong>Điện thoại:</strong> (08) 22.459.333 <br />
            <strong>Fax: </strong>(08) 35.501.272<br />

            <strong>Email: </strong>vido@vido.edu.vn</td>
            <td width="25%" valign="top">
            <p><strong>Cơ sở Quận 9</strong><br />
            118 Nam H&ograve;a , P.Phước Long A, Q.9, Tp.HCM<br />
            <strong>Điện thoại: </strong>(08) 22.459.222<strong><br />

            Fax: </strong>(08) 62.839.798&nbsp;</p>
            </td>
            <td width="25%" valign="top"><strong>Cơ sở Ph&uacute; Nhuận</strong><br />
            164 Nguyễn Đ&igrave;nh Ch&iacute;nh, P.11, Q.PN, Tp.HCM<br />
            <strong>Điện thoại:</strong> (08) 39.971.416<br />

            <strong>Fax: </strong>(08) 38.442.959</td>
            <td width="22%" valign="top"><strong>Cơ sở Quận 10</strong><br />
            357 L&ecirc; Hồng Phong, P.2, Q.10, Tp.HCM<br />
            <strong>Điện thoại:</strong> (08) 38.337.982<br />

            <strong>             Fax: </strong>(08) 38.337.982</td>
        </tr>
    </tbody>
</table>

<div align="center" style="color: rgb(85, 85, 85);"></div>

</div>






<div align="center">
<table>
<tbody>
                                        <tr>

                                            <td valign="top" class="tdCopyRight">
                                                &nbsp; <span id="dnn_dnnCOPYRIGHT_lblCopyright" class="dnnCopyright">
                                                Bản quyền thuộc Trường Cao Đẳng Viễn Đông</span>
 &nbsp; <a id="dnn_dnnTERMS_hypTerms" class="dnnTerms" href="http://www.vido.edu.vn/Trangch%e1%bb%a7/tabid/36/ctl/Terms/Default.aspx">Thỏa Thuận về Dịch Vụ</a>&nbsp; <a id="dnn_dnnPRIVACY_hypPrivacy" class="dnnPrivacy" href="http://www.vido.edu.vn/Trangch%e1%bb%a7/tabid/36/ctl/Privacy/Default.aspx">Về Bảo Vệ Thông Tin Người Dùng</a>&nbsp; <a id="dnn_dnnHELP_hypHelp" class="SkinObject"></a>&nbsp;
                                            </td>
                                        </tr>

                                    </tbody>
                                </table>

    </div>
</body>
</html>
