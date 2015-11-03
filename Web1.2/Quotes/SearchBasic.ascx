<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchBasic.ascx.cs" Inherits="SplendidCRM.Quotes.SearchBasic" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
<div id="divSearch">
	<table border="0" cellpadding="0" cellspacing="0" width="100%" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td class="dataLabel" noWrap><%= L10n.Term("Quotes.LBL_QUOTE_NUM"   ) %>&nbsp;&nbsp;<asp:TextBox      ID="txtQUOTE_NUM"    CssClass="dataField" size="10" Runat="server" /></td>
						<td class="dataLabel" noWrap><%= L10n.Term("Quotes.LBL_NAME"        ) %>&nbsp;&nbsp;<asp:TextBox      ID="txtNAME"         CssClass="dataField" size="15" Runat="server" /></td>
						<td class="dataLabel" noWrap><%= L10n.Term("Quotes.LBL_ACCOUNT_NAME") %>&nbsp;&nbsp;<asp:TextBox      ID="txtACCOUNT_NAME" CssClass="dataField" size="15" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term(".LBL_CURRENT_USER_FILTER") %>&nbsp;&nbsp;<asp:CheckBox ID="chkCURRENT_USER_ONLY" CssClass="checkbox" Runat="server" /></td>
						<td align="right">
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
							<asp:Button ID="btnClear"  CommandName="Clear"  OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) %>' title='<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>' AccessKey='<%# L10n.Term(".LBL_CLEAR_BUTTON_KEY" ) %>' Runat="server" />
							<br>
							<asp:ImageButton CommandName="AdvancedSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" ImageUrl='<%# Session["themeURL"] + "images/advanced_search.gif" %>' AlternateText='<%# L10n.Term(".LNK_ADVANCED_SEARCH") %>' BorderWidth="0" Runat="server" />
							&nbsp;
							<asp:LinkButton CommandName="AdvancedSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" Runat="server"><%# L10n.Term(".LNK_ADVANCED_SEARCH") %></asp:LinkButton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<%= Utils.RegisterEnterKeyPress(txtQUOTE_NUM.ClientID   , btnSearch.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtNAME.ClientID        , btnSearch.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtACCOUNT_NAME.ClientID, btnSearch.ClientID) %>
</div>
