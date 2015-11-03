<%@ Control Language="c#" AutoEventWireup="false" Codebehind="SearchAdvanced.ascx.cs" Inherits="SplendidCRM.Opportunities.SearchAdvanced" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
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
	<table width="100%" border="0" cellspacing="0" cellpadding="0" class="tabForm">
		<tr>
			<td>
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
					<tr>
						<td width="20%" class="dataLabel"><%= L10n.Term("Opportunities.LBL_OPPORTUNITY_NAME") %></td>
						<td width="25%" class="dataField"><asp:TextBox ID="txtNAME" TabIndex="1" Size="25" MaxLength="50" Runat="server" /></td>
						<td width="20%" class="dataLabel"><%= L10n.Term("Opportunities.LBL_AMOUNT") %></td>
						<td width="25%" class="dataField"><asp:TextBox ID="txtAMOUNT" TabIndex="2" Size="15" MaxLength="25" Runat="server" /></td>
						<td width="10%" class="dataLabel" rowspan="5" align="right" nowrap>
							<asp:Button ID="btnSearch" CommandName="Search" OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_SEARCH_BUTTON_LABEL") %>' title='<%# L10n.Term(".LBL_SEARCH_BUTTON_TITLE") %>' AccessKey='<%# L10n.Term(".LBL_SEARCH_BUTTON_KEY") %>' Runat="server" />
							<asp:Button ID="btnClear"  CommandName="Clear"  OnCommand="Page_Command" CssClass="button" Text='<%# L10n.Term(".LBL_CLEAR_BUTTON_LABEL" ) %>' title='<%# L10n.Term(".LBL_CLEAR_BUTTON_TITLE" ) %>' AccessKey='<%# L10n.Term(".LBL_CLEAR_BUTTON_KEY" ) %>' Runat="server" />
							<br>
							<asp:ImageButton CommandName="BasicSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" ImageUrl='<%# Session["themeURL"] + "images/basic_search.gif" %>' AlternateText='<%# L10n.Term(".LNK_BASIC_SEARCH") %>' BorderWidth="0" Runat="server" />
							&nbsp;
							<asp:LinkButton CommandName="BasicSearch" OnCommand="Page_Command" CssClass="tabFormAdvLink" Runat="server"><%# L10n.Term(".LNK_BASIC_SEARCH") %></asp:LinkButton>
						</td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_ACCOUNT_NAME") %></td>
						<td class="dataField"><asp:TextBox ID="txtACCOUNT_NAME" TabIndex="1" Size="25" MaxLength="150" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_DATE_CLOSED") %></td>
						<td class="dataField"><asp:TextBox ID="txtDATE_CLOSED" TabIndex="2" Size="10" MaxLength="10" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_TYPE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstOPPORTUNITY_TYPE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_NEXT_STEP") %></td>
						<td class="dataField"><asp:TextBox ID="txtNEXT_STEP" TabIndex="2" Size="15" MaxLength="25" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_LEAD_SOURCE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstLEAD_SOURCE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="1" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_SALES_STAGE") %></td>
						<td class="dataField"><asp:DropDownList ID="lstSALES_STAGE" DataValueField="NAME" DataTextField="DISPLAY_NAME" TabIndex="2" Runat="server" /></td>
					</tr>
					<tr>
						<td class="dataLabel"><%= L10n.Term(".LBL_ASSIGNED_TO") %></td>
						<td class="dataField"><asp:ListBox ID="lstASSIGNED_USER_ID" DataValueField="ID" DataTextField="USER_NAME" TabIndex="1" Rows="3" SelectionMode="Multiple" Runat="server" /></td>
						<td class="dataLabel"><%= L10n.Term("Opportunities.LBL_PROBABILITY") %></td>
						<td class="dataField"><asp:TextBox ID="txtPROBABILITY" TabIndex="2" Size="4" MaxLength="3" Runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
