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
 * Portions created by SplendidCRM Software are Copyright (C) 2005 SplendidCRM Software, Inc. All Rights Reserved.
 * Contributor(s): ______________________________________.
 *********************************************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace SplendidCRM.Bugs
{
	/// <summary>
	///		Summary description for New.
	/// </summary>
	public class NewRecord : SplendidControl
	{
		protected Label                      lblError       ;
		protected TextBox                    txtNAME        ;
		protected DropDownList               lstTYPE        ;
		protected DropDownList               lstRELEASE     ;
		protected DropDownList               lstPRIORITY    ;
		protected RequiredFieldValidator     reqNAME        ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "NewRecord" )
			{
				reqNAME.Enabled = true;
				reqNAME.Validate();
				if ( Page.IsValid )
				{
					Guid gID = Guid.Empty;
					try
					{
						SqlProcs.spBUGS_New(ref gID, txtNAME.Text, lstPRIORITY.SelectedValue, lstRELEASE.SelectedValue, lstTYPE.SelectedValue);
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
						lblError.Text = ex.Message;
					}
					if ( !Sql.IsEmptyGuid(gID) )
						Response.Redirect("~/Bugs/view.aspx?ID=" + gID.ToString());
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			// 06/04/2006 Paul.  NewRecord should not be displayed if the user does not have edit rights. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
			//this.DataBind();  // Need to bind so that Text of the Button gets updated. 
			reqNAME.ErrorMessage = L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Bugs.LBL_LIST_SUBJECT") + "<br>";
			if ( !IsPostBack )
			{
				lstPRIORITY.DataSource = SplendidCache.List("bug_priority_dom");
				// 03/30/2006 Paul.  IBM DB2 is having a problem returning lists. 
				// This is causing a Page.DataBind() error, so only bind if the list is not null. 
				if ( lstPRIORITY.DataSource != null )
				{
					lstPRIORITY.DataBind();
					try
					{
						// Set default value for priority. 
						// 07/16/2005 Paul.  Setting the default value is not working. 
						lstPRIORITY.SelectedValue = "Medium";
					}
					catch(Exception ex)
					{
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
					}
				}
				lstTYPE.DataSource = SplendidCache.List("bug_type_dom");
				lstTYPE.DataBind();
				// 07/16/2005 Paul.  Items.Insert is not working (but not failing).  Use manually if table is empty. 
				DataTable dtRelease = SplendidCache.Release();
				if ( dtRelease.Rows.Count > 0 )
				{
					lstRELEASE.DataSource = dtRelease;
					lstRELEASE.DataBind();
				}
				else
				{
					lstRELEASE.Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
				}
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
			m_sMODULE = "Bugs";
		}
		#endregion
	}
}
