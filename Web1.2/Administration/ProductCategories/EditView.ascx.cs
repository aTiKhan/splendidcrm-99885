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
using System.IO;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Diagnostics;
using System.Globalization;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Administration.ProductCategories
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected Label           lblError                     ;
		protected Guid            gID                          ;
		protected TextBox         txtNAME                      ;
		protected HtmlInputHidden txtPARENT_ID                 ;
		protected TextBox         txtPARENT_NAME               ;
		protected TextBox         txtDESCRIPTION               ;
		protected TextBox         txtLIST_ORDER                ;
		protected RequiredFieldValidator reqNAME      ;
		protected RequiredFieldValidator reqLIST_ORDER;

		protected _controls.ListHeader ctlListHeader ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			if ( e.CommandName == "Save" || e.CommandName == "SaveNew" )
			{
				if ( Page.IsValid )
				{
					try
					{
						// 06/02/2006 Paul.  Can't be your own parent.  Just clear parent value. 
						Guid gPARENT_ID = Sql.ToGuid(txtPARENT_ID.Value);
						if ( gPARENT_ID == gID )
							gPARENT_ID = Guid.Empty;
						SqlProcs.spPRODUCT_CATEGORIES_Update(
							ref gID
							, gPARENT_ID
							, txtNAME.Text
							, txtDESCRIPTION.Text
							, Sql.ToInteger(txtLIST_ORDER.Text)
							);
						Cache.Remove("vwPRODUCT_CATEGORIES_LISTBOX");
					}
					catch(Exception ex)
					{
						SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
						lblError.Text = ex.Message;
						return;
					}
					if ( e.CommandName == "SaveNew" )
						Response.Redirect("edit.aspx");
					else
						Response.Redirect("default.aspx");
				}
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term("ProductCategories.LBL_NAME"));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = SplendidCRM.Security.IS_ADMIN;
			if ( !this.Visible )
				return;

			try
			{
				// 06/09/2006 Paul.  Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
				//Page.DataBind();  // 09/03/2005 Paul. DataBind is required in order for the RequiredFieldValidators to work. 
				// 07/02/2006 Paul.  The required fields need to be bound manually. 
				reqNAME.DataBind();
				reqLIST_ORDER.DataBind();
				gID = Sql.ToGuid(Request["ID"]);
				reqLIST_ORDER.DataBind();
				if ( !IsPostBack )
				{
					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *                   " + ControlChars.CrLf
							     + "  from vwPRODUCT_CATEGORIES" + ControlChars.CrLf
							     + " where ID = @ID            " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@ID", gID);
								con.Open();
#if DEBUG
								Page.RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
#endif
								using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
								{
									if ( rdr.Read() )
									{
										txtNAME       .Text  = Sql.ToString(rdr["NAME"       ]);
										ctlListHeader .Title = L10n.Term("ProductCategories.LBL_NAME") + " " + txtNAME.Text;
										txtPARENT_ID  .Value = Sql.ToString(rdr["PARENT_ID"  ]);
										txtPARENT_NAME.Text  = Sql.ToString(rdr["PARENT_NAME"]);
										txtDESCRIPTION.Text  = Sql.ToString(rdr["DESCRIPTION"]);
										txtLIST_ORDER .Text  = Sql.ToString(rdr["LIST_ORDER" ]);
									}
								}
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				lblError.Text = ex.Message;
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
		}
		#endregion
	}
}
