<%@ Page language="c#" Codebehind="PopupMultiSelect.aspx.cs" AutoEventWireup="false" Inherits="SplendidCRM.Opportunities.PopupMultiSelect" %>
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
	<SplendidCRM:ListHeader Title="Opportunities.LBL_SEARCH_FORM_TITLE" Runat="Server" />
	<%@ Register TagPrefix="SplendidCRM" Tagname="SearchPopup" Src="SearchPopup.ascx" %>
	<SplendidCRM:SearchPopup ID="ctlSearch" Runat="Server" />
	<br>

<script type="text/javascript">
function SelectOpportunity(sPARENT_ID, sPARENT_NAME)
{
	if ( window.opener != null && window.opener.ChangeOpportunity != null )
	{
		window.opener.ChangeOpportunity(sPARENT_ID, sPARENT_NAME);
		window.close();
	}
	else
	{
		alert('Original window has closed.  Opportunity cannot be assigned.');
	}
}
function SelectOpportunitys()
{
	if ( window.opener != null && window.opener.ChangeOpportunity != null )
	{
		var sOpportunitys = '';
		for ( var i = 0 ; i < document.all.length ; i++ )
		{
			if ( document.all[i].name == 'chkMain' )
			{
				if ( document.all[i].checked )
				{
					if ( sOpportunitys.length > 0 )
						sOpportunitys += ',';
					sOpportunitys += document.all[i].value;
				}
			}
		}
		window.opener.ChangeOpportunity(sOpportunitys, '');
		window.close();
	}
	else
	{
		alert('Original window has closed.  Opportunity cannot be assigned.');
	}
}
function CancelOpportunity()
{
	window.close();
}
</script>
	<SplendidCRM:ListHeader Title="Opportunitys.LBL_LIST_FORM_TITLE" Runat="Server" />
	<table width="100%" border="0" cellpadding="0" cellspacing="0">
		<tr>
			<td style="padding-bottom: 2px;">
				<input id="btnSelect" type="button" class="button" onclick="SelectOpportunitys();" value='<%= "  " + L10n.Term("Opportunitys.LBL_SELECT_CHECKED_BUTTON_LABEL") + "  " %>' title='<%= L10n.Term("Opportunitys.LBL_SELECT_CHECKED_BUTTON_TITLE") %>' />
				<input id="btnCancel" type="button" class="button" onclick="CancelOpportunitys();" value='<%= "  " + L10n.Term(".LBL_DONE_BUTTON_LABEL"               ) + "  " %>' title='<%= L10n.Term(".LBL_DONE_BUTTON_TITLE"               ) %>' AccessKey='<%= L10n.Term(".LBL_DONE_BUTTON_KEY") %>' />
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
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="2%">
				<ItemTemplate>
					<input name="chkMain" id="OPPORTUNITY_ID_<%# DataBinder.Eval(Container.DataItem, "ID") %>" class="checkbox" type="checkbox" value="<%# DataBinder.Eval(Container.DataItem, "ID") %>" />
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn     HeaderText="Opportunities.LBL_LIST_OPPORTUNITY_NAME" DataField="NAME"          SortExpression="NAME"         ItemStyle-Width="38%" />
			<asp:BoundColumn     HeaderText="Opportunities.LBL_LIST_ACCOUNT_NAME"     DataField="ACCOUNT_NAME"  SortExpression="ACCOUNT_NAME" ItemStyle-Width="40%" />
			<asp:TemplateColumn  HeaderText="Opportunities.LBL_LIST_DATE_CLOSED"                                SortExpression="DATE_CLOSED"  ItemStyle-Width="10%">
				<ItemTemplate>
					<%# Sql.ToDateString(T10n.FromServerTime(DataBinder.Eval(Container.DataItem, "DATE_CLOSED"))) %>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</SplendidCRM:SplendidGrid>

<%@ Register TagPrefix="SplendidCRM" Tagname="DumpSQL" Src="~/_controls/DumpSQL.ascx" %>
<SplendidCRM:DumpSQL ID="ctlDumpSQL" Visible="<%# !PrintView %>" Runat="Server" />
</form>
</body>
</html>