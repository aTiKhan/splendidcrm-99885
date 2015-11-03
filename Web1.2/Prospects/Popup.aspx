<%@ Page language="c#" Codebehind="Popup.aspx.cs" AutoEventWireup="false" Inherits="SplendidCRM.Prospects.Popup" %>
<%@ Register TagPrefix="SplendidCRM" Namespace="SplendidCRM" Assembly="SplendidCRM" %>
<%@ Import Namespace="SplendidCRM" %>
<%
/**********************************************************************************************************************
 * The contents of this file are subject to the SugarCRM Public License Version 1.1.3 ("License"); You may not use this
 * file except in compliance with the License. You may obtain a copy of the License at http://www.sugarcrm.com/SPL
 * Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
 * express or implied.  See the License for the specific language governing rights and limitations under the License.
 *
 * All copies of the Covered Code must include on each user interface screen:
 *    (i) the "Powered by SugarCRM" logo and
 *    (ii) the SugarCRM copyright notice
 *    (iii) the SplendidCRM copyright notice
 * in the same form as they appear in the distribution.  See full license for requirements.
 *
 * The Original Code is: SugarCRM Open Source
 * The Initial Developer of the Original Code is SugarCRM, Inc.
 * Portions created by SugarCRM are Copyright (C) 2004-2005 SugarCRM, Inc. All Rights Reserved.
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" >
<html>
<head>
	<title><asp:Literal id="litPageTitle" runat="server" /></title>
	<%@ Register TagPrefix="SplendidCRM" Tagname="MetaHeader" Src="~/_controls/MetaHeader.ascx" %>
	<SplendidCRM:MetaHeader ID="ctlMetaHeader" Runat="Server" />
</head>
<body style="margin: 10px">
<form id="frmMain" method="post" runat="server">
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader Title="Prospects.LBL_SEARCH_FORM_TITLE" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="SearchPopup" Src="SearchPopup.ascx" %>
	<SplendidCRM:SearchPopup ID="ctlSearch" Runat="Server" />
	<br>

<script type="text/javascript">
function SelectProspect(sPARENT_ID, sPARENT_NAME)
{
	if ( window.opener != null && window.opener.ChangeProspect != null )
	{
		window.opener.ChangeProspect(sPARENT_ID, sPARENT_NAME);
		window.close();
	}
	else
	{
		alert('Original window has closed.  Prospect cannot be assigned.');
	}
}
function ClearProspect()
{
	if ( window.opener != null && window.opener.ChangeProspect != null )
	{
		window.opener.ChangeProspect('', '');
		window.close();
	}
	else
	{
		alert('Original window has closed.  Prospect cannot be assigned.');
	}
}
function CancelProspect()
{
	window.close();
}
</script>
	<SplendidCRM:ListHeader Title="Prospects.LBL_LIST_FORM_TITLE" Runat="Server" />
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="padding-bottom: 2px;">
				<div style="DISPLAY: <%= (Request["ClearDisabled"] == "1") ? "none" : "inline" %>" >
				<input id="btnClear" type="button" class="button" onclick="ClearProspect();"  value='<%= "  " + L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) + "  " %>' title='<%= L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>' AccessKey='<%= L10n.Term(".LBL_CLEAR_BUTTON_KEY" ) %>' />
				</div>
				<input id="btnCancel" type="button" class="button" onclick="CancelProspect();" value='<%= "  " + L10n.Term(".LBL_CANCEL_BUTTON_LABEL") + "  " %>' title='<%= L10n.Term(".LBL_CANCEL_BUTTON_TITLE") %>' AccessKey='<%= L10n.Term(".LBL_CANCEL_BUTTON_KEY") %>' />
				<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
			</td>
		</tr>
	</table>
	<SplendidCRM:SplendidGrid id="grdMain" Width="100%" CssClass="listView"
		CellPadding="3" CellSpacing="0" border="0"
		AllowPaging="true" PageSize="20" AllowSorting="true" 
		AutoGenerateColumns="false" 
		EnableViewState="true" runat="server">
		<ItemStyle            CssClass="oddListRowS1"  />
		<AlternatingItemStyle CssClass="evenListRowS1" />
		<HeaderStyle          CssClass="listViewThS1"  />
		<PagerStyle HorizontalAlign="Right" Mode="NextPrev" PageButtonCount="6" Position="Top" CssClass="listViewPaginationTdS1" PrevPageText=".LNK_LIST_PREVIOUS" NextPageText=".LNK_LIST_NEXT" />
		<Columns>
			<asp:TemplateColumn HeaderText="Prospects.LBL_LIST_NAME" SortExpression="NAME" ItemStyle-Width="35%">
				<ItemTemplate>
					<a name="PROSPECT_ID" id="PROSPECT_ID_<%# DataBinder.Eval(Container.DataItem, "ID") %>" class="listViewTdLinkS1" href="#" onclick="SelectProspect('<%# Sql.EscapeJavaScript(Sql.ToString(DataBinder.Eval(Container.DataItem, "ID"))) %>', '<%# Sql.EscapeJavaScript(Sql.ToString(DataBinder.Eval(Container.DataItem, "NAME"))) %>');"><%# DataBinder.Eval(Container.DataItem, "NAME") %></a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn     HeaderText="Prospects.LBL_LIST_TITLE"         DataField="TITLE"            SortExpression="TITLE"        ItemStyle-Width="15%" />
			<asp:BoundColumn     HeaderText="Prospects.LBL_LIST_EMAIL_ADDRESS" DataField="EMAIL1"           SortExpression="EMAIL1"       ItemStyle-Width="35%" />
			<asp:BoundColumn     HeaderText="Prospects.LBL_LIST_PHONE"         DataField="PHONE_WORK"       SortExpression="PHONE_WORK"   ItemStyle-Width="15%" />
		</Columns>
	</SplendidCRM:SplendidGrid>

<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</form>
</body>
</html>
