<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchPopup.ascx.cs" Inherits="SplendidCRM.Leads.SearchPopup" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
						<td width="20%" class="dataLabel" noWrap><%= L10n.Term("Leads.LBL_FIRST_NAME"  ) %></td>
						<td width="25%" class="dataLabel" noWrap><asp:TextBox ID="txtFIRST_NAME"   CssClass="dataField" size="10" Runat="server" /></td>
						<td width="20%" class="dataLabel" noWrap><%= L10n.Term("Leads.LBL_LAST_NAME"   ) %></td>
						<td width="25%" class="dataLabel" noWrap><asp:TextBox ID="txtLAST_NAME"    CssClass="dataField" size="10" Runat="server" /></td>
						<td align="right">
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Leads.LBL_LEAD_SOURCE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstLEAD_SOURCE" DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Leads.LBL_STATUS") %></td>
						<td class="dataField"><asp:DropDownList ID="lstSTATUS"      DataValueField="NAME" DataTextField="DISPLAY_NAME" Runat="server" /></td>
						<td>&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<%= Utils.RegisterEnterKeyPress(txtFIRST_NAME.ClientID, btnSearch.ClientID) %>
	<%= Utils.RegisterEnterKeyPress(txtLAST_NAME.ClientID , btnSearch.ClientID) %>
	<%= Utils.RegisterSetFocus(txtFIRST_NAME.ClientID) %>
</div>
