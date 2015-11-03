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
using System.Data.Common;
using System.Configuration;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
//using Microsoft.VisualBasic;

namespace SplendidCRM.Tasks
{
	/// <summary>
	///		Summary description for EditView.
	/// </summary>
	public class EditView : SplendidControl
	{
		protected _controls.ModuleHeader ctlModuleHeader;
		protected _controls.EditButtons  ctlEditButtons ;

		protected Guid            gID                          ;
		protected HtmlTable       tblMain                      ;

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			// 08/21/2005 Paul.  Redirect to parent if that is where the note was originated. 
			Guid   gPARENT_ID   = Sql.ToGuid(Request["PARENT_ID" ]);
			Guid   gCONTACT_ID  = Sql.ToGuid(Request["CONTACT_ID"]);
			string sMODULE      = String.Empty;
			string sPARENT_TYPE = String.Empty;
			string sPARENT_NAME = String.Empty;
			try
			{
				SqlProcs.spPARENT_Get(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				// The only possible error is a connection failure, so just ignore all errors. 
				gPARENT_ID = Guid.Empty;
			}
			if ( e.CommandName == "Save" )
			{
				// 01/16/2006 Paul.  Enable validator before validating page. 
				SplendidDynamic.ValidateEditViewFields(m_sMODULE + ".EditView", this);
				if ( Page.IsValid )
				{
					string sCUSTOM_MODULE = "TASKS";
					DataTable dtCustomFields = SplendidCache.FieldsMetaData_Validated(sCUSTOM_MODULE);
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						using ( IDbTransaction trn = con.BeginTransaction() )
						{
							try
							{
								SqlProcs.spTASKS_Update
									( ref gID
									, new DynamicControl(this, "ASSIGNED_USER_ID").ID
									, new DynamicControl(this, "NAME"            ).Text
									, new DynamicControl(this, "STATUS"          ).SelectedValue
									, new DynamicControl(this, "DATE_DUE"        ).DateValue
									, new DynamicControl(this, "DATE_START"      ).DateValue
									, new DynamicControl(this, "PARENT_TYPE"     ).SelectedValue
									, new DynamicControl(this, "PARENT_ID"       ).ID
									, new DynamicControl(this, "CONTACT_ID"      ).ID
									, new DynamicControl(this, "PRIORITY"        ).SelectedValue
									, new DynamicControl(this, "DESCRIPTION"     ).Text
									, trn
									);
								SplendidDynamic.UpdateCustomFields(this, trn, gID, sCUSTOM_MODULE, dtCustomFields);
								trn.Commit();
							}
							catch(Exception ex)
							{
								trn.Rollback();
								SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
								ctlEditButtons.ErrorText = ex.Message;
								return;
							}
						}
					}
					if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						Response.Redirect("~/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID.ToString());
					else if ( !Sql.IsEmptyGuid(gCONTACT_ID) )
						Response.Redirect("~/Contacts/view.aspx?ID=" + gCONTACT_ID.ToString());
					else
						Response.Redirect("view.aspx?ID=" + gID.ToString());
				}
			}
			else if ( e.CommandName == "Cancel" )
			{
				if ( !Sql.IsEmptyGuid(gPARENT_ID) )
					Response.Redirect("~/" + sMODULE + "/view.aspx?ID=" + gPARENT_ID.ToString());
				else if ( !Sql.IsEmptyGuid(gCONTACT_ID) )
					Response.Redirect("~/Contacts/view.aspx?ID=" + gCONTACT_ID.ToString());
				else if ( Sql.IsEmptyGuid(gID) )
					Response.Redirect("default.aspx");
				else
					Response.Redirect("view.aspx?ID=" + gID.ToString());
			}
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE));
			// 06/04/2006 Paul.  Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			this.Visible = (SplendidCRM.Security.GetUserAccess(m_sMODULE, "edit") >= 0);
			if ( !this.Visible )
				return;

			try
			{
				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					// 07/29/2005 Paul.  SugarCRM 3.0 does not allow the NONE option. 
					//lstPARENT_TYPE     .Items.Insert(0, new ListItem(L10n.Term(".LBL_NONE"), ""));
					Guid gDuplicateID = Sql.ToGuid(Request["DuplicateID"]);
					if ( !Sql.IsEmptyGuid(gID) || !Sql.IsEmptyGuid(gDuplicateID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL ;
							sSQL = "select *           " + ControlChars.CrLf
							     + "  from vwTASKS_Edit" + ControlChars.CrLf
							     + " where ID = @ID    " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								if ( !Sql.IsEmptyGuid(gDuplicateID) )
								{
									Sql.AddParameter(cmd, "@ID", gDuplicateID);
									gID = Guid.Empty;
								}
								else
								{
									Sql.AddParameter(cmd, "@ID", gID);
								}
								con.Open();
#if DEBUG
								Page.RegisterClientScriptBlock("SQLCode", Sql.ClientScriptBlock(cmd));
#endif
								using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
								{
									if ( rdr.Read() )
									{
										ctlModuleHeader.Title = Sql.ToString(rdr["NAME"]);
										Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
										Utils.UpdateTracker(Page, m_sMODULE, gID, ctlModuleHeader.Title);
										ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;

										this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, rdr);
										// 07/15/2006 Paul.  Contacts are not valid parents for a Task.  
										// Manually remove them as the list record_type_display is common across all other parents. 
										DropDownList lstPARENT_TYPE = FindControl("PARENT_TYPE") as DropDownList;
										if ( lstPARENT_TYPE != null )
											lstPARENT_TYPE.Items.Remove("Contacts");

										// 03/04/2006 Paul.  The close button on the Tasks List is used to edit and set STATUS to Completed. 
										// 06/21/2006 Paul.  Change parameter to Close so that the same parameter can be used for Calls, Meetings and Tasks. 
										// 08/08/2006 Paul.  SugarCRM uses Completed in its URL, so we will do the same. 
										if ( Sql.ToString(Request["Status"]) == "Completed" )
										{
											new DynamicControl(this, "STATUS").SelectedValue = "Completed";
										}
									}
								}
							}
						}
					}
					else
					{
						this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
						// 07/15/2006 Paul.  Contacts are not valid parents for a Task.  
						// Manually remove them as the list record_type_display is common across all other parents. 
						DropDownList lstPARENT_TYPE = FindControl("PARENT_TYPE") as DropDownList;
						if ( lstPARENT_TYPE != null )
							lstPARENT_TYPE.Items.Remove("Contacts");

						Guid gPARENT_ID  = Sql.ToGuid(Request["PARENT_ID" ]);
						Guid gCONTACT_ID = Sql.ToGuid(Request["CONTACT_ID"]);
						if ( !Sql.IsEmptyGuid(gPARENT_ID) )
						{
							string sMODULE      = String.Empty;
							string sPARENT_TYPE = String.Empty;
							string sPARENT_NAME = String.Empty;
							SqlProcs.spPARENT_Get(ref gPARENT_ID, ref sMODULE, ref sPARENT_TYPE, ref sPARENT_NAME);
							if ( !Sql.IsEmptyGuid(gPARENT_ID) )
							{
								// 07/15/2006 Paul.  If the parent is a contact, then convert to a contact. 
								if ( sPARENT_TYPE == "Contacts" )
								{
									gCONTACT_ID = gPARENT_ID;
								}
								else
								{
									new DynamicControl(this, "PARENT_ID"  ).ID   = gPARENT_ID;
									new DynamicControl(this, "PARENT_NAME").Text = sPARENT_NAME;
									new DynamicControl(this, "PARENT_TYPE").SelectedValue = sPARENT_TYPE;
								}
							}
						}
						if ( !Sql.IsEmptyGuid(gCONTACT_ID) )
						{
							string sMODULE       = String.Empty;
							string sCONTACT_TYPE = String.Empty;
							string sCONTACT_NAME = String.Empty;
							SqlProcs.spPARENT_Get(ref gCONTACT_ID, ref sMODULE, ref sCONTACT_TYPE, ref sCONTACT_NAME);
							if ( !Sql.IsEmptyGuid(gCONTACT_ID) )
							{
								new DynamicControl(this, "CONTACT_ID"  ).ID   = gCONTACT_ID  ;
								new DynamicControl(this, "CONTACT_NAME").Text = sCONTACT_NAME;
							}
						}
						try
						{
							// 12/04/2005 Paul.  Default value is Medium. 
							new DynamicControl(this, "PRIORITY").SelectedValue = "Medium";
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
						try
						{
							// 12/04/2005 Paul.  Default value is Not Started. 
							new DynamicControl(this, "STATUS").SelectedValue = "Not Started";
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex.Message);
						}
					}
				}
				else
				{
					// 12/02/2005 Paul.  When validation fails, the header title does not retain its value.  Update manually. 
					ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
					Utils.SetPageTitle(Page, L10n.Term(".moduleList." + m_sMODULE) + " - " + ctlModuleHeader.Title);
				}
			}
			catch(Exception ex)
			{
				SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex.Message);
				ctlEditButtons.ErrorText = ex.Message;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This Task is required by the ASP.NET Web Form Designer.
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
			ctlEditButtons.Command = new CommandEventHandler(Page_Command);
			m_sMODULE = "Tasks";
			if ( IsPostBack )
			{
				// 12/02/2005 Paul.  Need to add the edit fields in order for events to fire. 
				this.AppendEditViewFields(m_sMODULE + ".EditView", tblMain, null);
			}
		}
		#endregion
	}
}
