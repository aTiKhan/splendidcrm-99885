<%@ Control CodeBehind="EmailMarketing.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="SplendidCRM.Campaigns.EmailMarketing" %>
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
 * The Original Code is: SplendidCRM Open Source
 * The Initial Developer of the Original Code is SplendidCRM Software, Inc.
 * Portions created by SplendidCRM Software are Copyright (C) 2007 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
%>
<script type="text/javascript">
function CampaignPreview()
{
	return window.open('Preview.aspx?ID=<%= gID %>', 'CampaignPreviewPopup','width=600,height=400,resizable=1,scrollbars=1');
}
</script>
<div id="divCampaignsEmailMarketing">
	<br />
	<%@ Register TagPrefix="SplendidCRM" Tagname="ListHeader" Src="~/_controls/ListHeader.ascx" %>
	<SplendidCRM:ListHeader Title="EmailMarketing.LBL_MODULE_NAME" Runat="Server" />
	
	<asp:Panel CssClass="button-panel" Visible="<%# !PrintView %>" runat="server">
		<asp:Button Visible='<%# SplendidCRM.Security.GetUserAccess("EmailMarketing", "edit") >= 0 %>' CommandName="EmailMarketing.Create" OnCommand="Page_Command" CssClass="button" Text='<%# "  " + L10n.Term(".LBL_NEW_BUTTON_LABEL"             ) + "  " %>' ToolTip='<%# L10n.Term(".LBL_NEW_BUTTON_TITLE"             ) %>' AccessKey='<%# L10n.AccessKey(".LBL_NEW_BUTTON_KEY"             ) %>' Runat="server" />
		<asp:Button                                                                                    OnClientClick="CampaignPreview(); return false;"             CssClass="button" Text='<%# "  " + L10n.Term("Campaigns.LBL_PREVIEW_BUTTON_LABEL") + "  " %>' ToolTip='<%# L10n.Term("Campaigns.LBL_PREVIEW_BUTTON_TITLE") %>' AccessKey='<%# L10n.AccessKey("Campaigns.LBL_PREVIEW_BUTTON_KEY") %>' Runat="server" />
		<asp:Label ID="lblError" ForeColor="Red" EnableViewState="false" Runat="server" />
	</asp:Panel>

	<SplendidCRM:SplendidGrid id="grdMain" Width="100%" CssClass="listView"
		CellPadding="3" CellSpacing="0" border="0"
		AllowPaging="true" PageSize="20" AllowSorting="false" 
		AutoGenerateColumns="false" 
		EnableViewState="true" runat="server">
		<ItemStyle            CssClass="oddListRowS1"  />
		<AlternatingItemStyle CssClass="evenListRowS1" />
		<HeaderStyle          CssClass="listViewThS1"  />
		<PagerStyle HorizontalAlign="Right" Mode="NextPrev" PageButtonCount="6" Position="Top" CssClass="listViewPaginationTdS1" PrevPageText=".LNK_LIST_PREVIOUS" NextPageText=".LNK_LIST_NEXT" />
		<Columns>
			<asp:TemplateColumn HeaderText="" ItemStyle-Width="1%" ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="false">
				<ItemTemplate>
					<asp:ImageButton Visible='<%# SplendidCRM.Security.GetUserAccess("EmailMarketing", "edit") >= 0 %>' CommandName="EmailMarketing.Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_EDIT") %>' ImageUrl='<%# Session["themeURL"] + "images/edit_inline.gif" %>' BorderWidth="0" Width="12" Height="12" ImageAlign="AbsMiddle" Runat="server" />
					<asp:LinkButton  Visible='<%# SplendidCRM.Security.GetUserAccess("EmailMarketing", "edit") >= 0 %>' CommandName="EmailMarketing.Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_EDIT") %>' Runat="server" />
					&nbsp;
					<span onclick="return confirm('<%= L10n.TermJavaScript(".NTC_DELETE_CONFIRMATION") %>')">
						<asp:ImageButton Visible='<%# SplendidCRM.Security.GetUserAccess("Campaigns", "edit", Sql.ToGuid(DataBinder.Eval(Container.DataItem, "CAMPAIGN_ASSIGNED_USER_ID"))) >= 0 %>' CommandName="EmailMarketing.Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" AlternateText='<%# L10n.Term(".LNK_REMOVE") %>' ImageUrl='<%# Session["themeURL"] + "images/delete_inline.gif" %>' BorderWidth="0" Width="12" Height="12" ImageAlign="AbsMiddle" Runat="server" />
						<asp:LinkButton  Visible='<%# SplendidCRM.Security.GetUserAccess("Campaigns", "edit", Sql.ToGuid(DataBinder.Eval(Container.DataItem, "CAMPAIGN_ASSIGNED_USER_ID"))) >= 0 %>' CommandName="EmailMarketing.Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID") %>' OnCommand="Page_Command" CssClass="listViewTdToolsS1" Text='<%# L10n.Term(".LNK_REMOVE") %>' Runat="server" />
					</span>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</SplendidCRM:SplendidGrid>
</div>
