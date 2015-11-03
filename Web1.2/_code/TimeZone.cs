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
using System.Web;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SplendidCRM
{
	/// <summary>
	/// Summary description for TimeZone.
	/// </summary>
	public class TimeZone
	{
		[StructLayout( LayoutKind.Sequential )]
		private struct SYSTEMTIME
		{
			public UInt16 wYear        ;
			public UInt16 wMonth       ;
			public UInt16 wDayOfWeek   ;
			public UInt16 wDay         ;
			public UInt16 wHour        ;
			public UInt16 wMinute      ;
			public UInt16 wSecond      ;
			public UInt16 wMilliseconds;
		}

		[StructLayout( LayoutKind.Sequential )]
		private struct TZI
		{
			public int        nBias         ;
			public int        nStandardBias ;
			public int        nDaylightBias ;
			public SYSTEMTIME dtStandardDate;
			public SYSTEMTIME dtDaylightDate;
		}
		
		protected Guid   m_gID                    ;
		protected string m_sNAME                  ;
		protected string m_sSTANDARD_NAME         ;
		protected string m_sSTANDARD_ABBREVIATION ;
		protected string m_sDAYLIGHT_NAME         ;
		protected string m_sDAYLIGHT_ABBREVIATION ;
		protected int    m_nBIAS                  ;
		protected int    m_nSTANDARD_BIAS         ;
		protected int    m_nDAYLIGHT_BIAS         ;
		protected int    m_nSTANDARD_YEAR         ;
		protected int    m_nSTANDARD_MONTH        ;
		protected int    m_nSTANDARD_WEEK         ;
		protected int    m_nSTANDARD_DAYOFWEEK    ;
		protected int    m_nSTANDARD_HOUR         ;
		protected int    m_nSTANDARD_MINUTE       ;
		protected int    m_nDAYLIGHT_YEAR         ;
		protected int    m_nDAYLIGHT_MONTH        ;
		protected int    m_nDAYLIGHT_WEEK         ;
		protected int    m_nDAYLIGHT_DAYOFWEEK    ;
		protected int    m_nDAYLIGHT_HOUR         ;
		protected int    m_nDAYLIGHT_MINUTE       ;
		protected bool   m_bGMTStorage            ;
		
		public Guid ID
		{
			get
			{
				return m_gID;
			}
		}

		public static TimeZone CreateTimeZone(Guid gTIMEZONE)
		{
			HttpApplicationState Application = HttpContext.Current.Application;
			TimeZone T10z = Application["TIMEZONE." + gTIMEZONE.ToString()] as SplendidCRM.TimeZone;
			if ( T10z == null )
			{
				// 08/29/2005 Paul. First try and use the default from CONFIG. 
				gTIMEZONE = Sql.ToGuid(Application["CONFIG.default_timezone"]);
				T10z = Application["TIMEZONE." + gTIMEZONE.ToString()] as SplendidCRM.TimeZone;
				if ( T10z == null )
				{
					// Default to EST if default not specified. 
					gTIMEZONE = new Guid("BFA61AF7-26ED-4020-A0C1-39A15E4E9E0A");
					T10z = Application["TIMEZONE." + gTIMEZONE.ToString()] as SplendidCRM.TimeZone;
				}
				// If timezone is still null, then create a blank zone. 
				if ( T10z == null )
				{
					string sMessage = "Could not load default timezone " + Sql.ToString(Application["CONFIG.default_timezone"]) + " nor EST timezone BFA61AF7-26ED-4020-A0C1-39A15E4E9E0A. "
					                + "Eastern Standard Time will be extracted from the Windows Registry and used as the default.";
					SplendidError.SystemError(new StackTrace(true).GetFrame(0), sMessage);
					// T10z = new TimeZone();
					// 07/25/2006  Paul.  Still having a problem with the hosting company.  
					// Try and skip the entire registry code. 
					T10z = new TimeZone
						( new Guid("BFA61AF7-26ED-4020-A0C1-39A15E4E9E0A")
						, "(GMT-05:00) Eastern Time (US & Canada)"
						, "EST"
						, "Eastern Standard Time"
						, "Eastern Daylight Time"
						, "EDT"
						, 300
						,   0
						, -60
						,   0
						,  10
						,   5
						,   0
						,   2
						,   0
						,   0
						,   4
						,   1
						,   0
						,   2
						,   0
						, false
						);
					Application["TIMEZONE." + gTIMEZONE.ToString()] = T10z;
				}
			}
			return T10z;
		}
		
		public TimeZone()
		{
			m_gID                    = Guid.Empty  ;
			m_sNAME                  = String.Empty;
			m_sSTANDARD_NAME         = String.Empty;
			m_sSTANDARD_ABBREVIATION = String.Empty;
			m_sDAYLIGHT_NAME         = String.Empty;
			m_sDAYLIGHT_ABBREVIATION = String.Empty;
			try
			{
				RegistryKey keyEST = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Time Zones\Eastern Standard Time");
				if ( keyEST != null )
				{
					m_sSTANDARD_NAME = keyEST.GetValue("Std"    ).ToString();
					m_sNAME          = keyEST.GetValue("Display").ToString();
					m_sDAYLIGHT_NAME = keyEST.GetValue("Dlt"    ).ToString();
					byte[] byTZI         = (byte[]) keyEST.GetValue("TZI");

					TZI tzi ;
					GCHandle h = GCHandle.Alloc(byTZI, GCHandleType.Pinned);
					try
					{
						tzi = (TZI) Marshal.PtrToStructure( h.AddrOfPinnedObject(), typeof(TZI) );
						m_nBIAS                = tzi.nBias                    ;
						m_nSTANDARD_BIAS       = tzi.nStandardBias            ;
						m_nDAYLIGHT_BIAS       = tzi.nDaylightBias            ;
						m_nSTANDARD_YEAR       = tzi.dtStandardDate.wYear     ;
						m_nSTANDARD_MONTH      = tzi.dtStandardDate.wMonth    ;
						m_nSTANDARD_WEEK       = tzi.dtStandardDate.wDay      ;
						m_nSTANDARD_DAYOFWEEK  = tzi.dtStandardDate.wDayOfWeek;
						m_nSTANDARD_HOUR       = tzi.dtStandardDate.wHour     ;
						m_nSTANDARD_MINUTE     = tzi.dtStandardDate.wMinute   ;
						m_nDAYLIGHT_YEAR       = tzi.dtDaylightDate.wYear     ;
						m_nDAYLIGHT_MONTH      = tzi.dtDaylightDate.wMonth    ;
						m_nDAYLIGHT_WEEK       = tzi.dtDaylightDate.wDay      ;
						m_nDAYLIGHT_DAYOFWEEK  = tzi.dtDaylightDate.wDayOfWeek;
						m_nDAYLIGHT_HOUR       = tzi.dtDaylightDate.wHour     ;
						m_nDAYLIGHT_MINUTE     = tzi.dtDaylightDate.wMinute   ;
					}
					finally
					{
						h.Free();
					}
				}
			}
			catch
			{
				// 07/25/2006 Paul.  Some web hosting companies have tight security and block all access to the registry. 
				// In those cases, just assume EST. 
				m_sNAME                  = "(GMT-05:00) Eastern Time (US & Canada)";
				m_sSTANDARD_ABBREVIATION = "EST";
				m_sSTANDARD_NAME         = "Eastern Standard Time";
				m_sDAYLIGHT_NAME         = "Eastern Daylight Time";
				m_sDAYLIGHT_ABBREVIATION = "EDT";
				m_nBIAS                  = 300;
				m_nSTANDARD_BIAS         =   0;
				m_nDAYLIGHT_BIAS         = -60;
				m_nSTANDARD_YEAR         =   0;
				m_nSTANDARD_MONTH        =  10;
				m_nSTANDARD_WEEK         =   5;
				m_nSTANDARD_DAYOFWEEK    =   0;
				m_nSTANDARD_HOUR         =   2;
				m_nSTANDARD_MINUTE       =   0;
				m_nDAYLIGHT_YEAR         =   0;
				m_nDAYLIGHT_MONTH        =   4;
				m_nDAYLIGHT_WEEK         =   1;
				m_nDAYLIGHT_DAYOFWEEK    =   0;
				m_nDAYLIGHT_HOUR         =   2;
				m_nDAYLIGHT_MINUTE       =   0;
			}
		}
		
		public TimeZone
			( Guid   gID                   
			, string sNAME                 
			, string sSTANDARD_NAME        
			, string sSTANDARD_ABBREVIATION
			, string sDAYLIGHT_NAME        
			, string sDAYLIGHT_ABBREVIATION
			, int    nBIAS                 
			, int    nSTANDARD_BIAS        
			, int    nDAYLIGHT_BIAS        
			, int    nSTANDARD_YEAR        
			, int    nSTANDARD_MONTH       
			, int    nSTANDARD_WEEK        
			, int    nSTANDARD_DAYOFWEEK   
			, int    nSTANDARD_HOUR        
			, int    nSTANDARD_MINUTE      
			, int    nDAYLIGHT_YEAR        
			, int    nDAYLIGHT_MONTH       
			, int    nDAYLIGHT_WEEK        
			, int    nDAYLIGHT_DAYOFWEEK   
			, int    nDAYLIGHT_HOUR        
			, int    nDAYLIGHT_MINUTE      
			, bool   bGMTStorage           
			)
		{
			m_gID                    = gID                    ;
			m_sNAME                  = sNAME                  ;
			m_sSTANDARD_NAME         = sSTANDARD_NAME         ;
			m_sSTANDARD_ABBREVIATION = sSTANDARD_ABBREVIATION ;
			m_sDAYLIGHT_NAME         = sDAYLIGHT_NAME         ;
			m_sDAYLIGHT_ABBREVIATION = sDAYLIGHT_ABBREVIATION ;
			m_nBIAS                  = nBIAS                  ;
			m_nSTANDARD_BIAS         = nSTANDARD_BIAS         ;
			m_nDAYLIGHT_BIAS         = nDAYLIGHT_BIAS         ;
			m_nSTANDARD_YEAR         = nSTANDARD_YEAR         ;
			m_nSTANDARD_MONTH        = nSTANDARD_MONTH        ;
			m_nSTANDARD_WEEK         = nSTANDARD_WEEK         ;
			m_nSTANDARD_DAYOFWEEK    = nSTANDARD_DAYOFWEEK    ;
			m_nSTANDARD_HOUR         = nSTANDARD_HOUR         ;
			m_nSTANDARD_MINUTE       = nSTANDARD_MINUTE       ;
			m_nDAYLIGHT_YEAR         = nDAYLIGHT_YEAR         ;
			m_nDAYLIGHT_MONTH        = nDAYLIGHT_MONTH        ;
			m_nDAYLIGHT_WEEK         = nDAYLIGHT_WEEK         ;
			m_nDAYLIGHT_DAYOFWEEK    = nDAYLIGHT_DAYOFWEEK    ;
			m_nDAYLIGHT_HOUR         = nDAYLIGHT_HOUR         ;
			m_nDAYLIGHT_MINUTE       = nDAYLIGHT_MINUTE       ;
			m_bGMTStorage            = bGMTStorage            ;
		}

		private static DateTime TransitionDate(int nYEAR, int nMONTH, int nWEEK, int nDAYOFWEEK, int nHOUR, int nMINUTE)
		{
			DateTime dtTransitionDate = new DateTime(nYEAR, nMONTH, 1, nHOUR, nMINUTE, 0);
			// First DAYOFWEEK (typically Sunday) in the month. 
			int nFirstDayOfWeek = nDAYOFWEEK + (DayOfWeek.Sunday - dtTransitionDate.DayOfWeek);
			if ( nFirstDayOfWeek < 0 )
				nFirstDayOfWeek += 7;
			dtTransitionDate = dtTransitionDate.AddDays(nFirstDayOfWeek);
			// Now add the weeks, but watch for overflow to next month.  
			dtTransitionDate = dtTransitionDate.AddDays(7 * (nWEEK - 1));
			// In case of overflow, subtract a week until the month matches. 
			while ( dtTransitionDate.Month != nMONTH )
				dtTransitionDate = dtTransitionDate.AddDays(-7);
			return dtTransitionDate;
		}

		public DateTime FromServerTime(object objServerTime)
		{
			DateTime dtServerTime = Sql.ToDateTime(objServerTime);
			if ( dtServerTime == DateTime.MinValue )
				return dtServerTime;
			// 11/07/2005 Paul.  SugarCRM 3.5 now stores time in GMT. 
			if ( m_bGMTStorage )
				return FromUniversalTime(dtServerTime);
			else
				return FromUniversalTime(dtServerTime.ToUniversalTime());
		}

		public DateTime FromServerTime(DateTime dtServerTime)
		{
			// 11/07/2005 Paul.  SugarCRM 3.5 now stores time in GMT. 
			if ( m_bGMTStorage )
				return FromUniversalTime(dtServerTime);
			else
				return FromUniversalTime(dtServerTime.ToUniversalTime());
		}

		public DateTime FromUniversalTime(DateTime dtUniversalTime)
		{
			// 11/07/2005 Paul.  Don't modify if value is MinValue.
			if ( dtUniversalTime == DateTime.MinValue )
				return dtUniversalTime;
			DateTime dtZoneTime = dtUniversalTime.AddMinutes(-m_nBIAS);
			int nLocalMonth = dtZoneTime.Month;
			// This date/time conversion function will be called with a very high frequency.  It is therefore important to optimize as much as possible. 
			// For example, we only have to worry about complicated daylight savings calculations during the transition months.  
			// Otherwise, we are either in daylight savings or not in daylight savings. 
			// If a timezone does not observer daylight savings, then the months will be 0 and no calculations will be performed. 
			if ( nLocalMonth == m_nDAYLIGHT_MONTH )
			{
				// The transition date needs to be calculated every time because the Local year may change, and the date changes every year. 
				DateTime dtTransitionDate = TransitionDate(dtZoneTime.Year, m_nDAYLIGHT_MONTH, m_nDAYLIGHT_WEEK, m_nDAYLIGHT_DAYOFWEEK, m_nDAYLIGHT_HOUR, m_nDAYLIGHT_MINUTE);
				if ( dtZoneTime > dtTransitionDate )
					dtZoneTime = dtZoneTime.AddMinutes(-m_nDAYLIGHT_BIAS);
			}
			else if ( nLocalMonth == m_nSTANDARD_MONTH )
			{
				// The transition date needs to be calculated every time because the Local year may change, and the date changes every year. 
				DateTime dtTransitionDate = TransitionDate(dtZoneTime.Year, m_nSTANDARD_MONTH, m_nSTANDARD_WEEK, m_nSTANDARD_DAYOFWEEK, m_nSTANDARD_HOUR, m_nSTANDARD_MINUTE);
				// For the transition at the end, we compare to an already adjusted Local date.
				if ( dtZoneTime.AddMinutes(-m_nDAYLIGHT_BIAS) < dtTransitionDate )
					dtZoneTime = dtZoneTime.AddMinutes(-m_nDAYLIGHT_BIAS);
			}
			else if ( nLocalMonth > m_nDAYLIGHT_MONTH && nLocalMonth < m_nSTANDARD_MONTH )
			{
				// If we are solidly in the daylight savings months, then the calculation is simple. 
				dtZoneTime = dtZoneTime.AddMinutes(-m_nDAYLIGHT_BIAS);
			}
			return dtZoneTime;
		}

		public bool IsDaylightSavings(DateTime dtZoneTime)
		{
			bool bDaylightSavings = false;
			int nLocalMonth = dtZoneTime.Month;
			// This date/time conversion function will be called with a very high frequency.  It is therefore important to optimize as much as possible. 
			// For example, we only have to worry about complicated daylight savings calculations during the transition months.  
			// Otherwise, we are either in daylight savings or not in daylight savings. 
			// If a timezone does not observer daylight savings, then the months will be 0 and no calculations will be performed. 
			if ( nLocalMonth == m_nDAYLIGHT_MONTH )
			{
				// The transition date needs to be calculated every time because the Local year may change, and the date changes every year. 
				DateTime dtTransitionDate = TransitionDate(dtZoneTime.Year, m_nDAYLIGHT_MONTH, m_nDAYLIGHT_WEEK, m_nDAYLIGHT_DAYOFWEEK, m_nDAYLIGHT_HOUR, m_nDAYLIGHT_MINUTE);
				if ( dtZoneTime > dtTransitionDate )
					bDaylightSavings = true;
			}
			else if ( nLocalMonth == m_nSTANDARD_MONTH )
			{
				// The transition date needs to be calculated every time because the Local year may change, and the date changes every year. 
				DateTime dtTransitionDate = TransitionDate(dtZoneTime.Year, m_nSTANDARD_MONTH, m_nSTANDARD_WEEK, m_nSTANDARD_DAYOFWEEK, m_nSTANDARD_HOUR, m_nSTANDARD_MINUTE);
				// Don't add the bias here because it is already part of the zone time. 
				// Since there is an overlap due to the drop back in time, we cannot fully be sure that the 
				// supplied time is before or after the daylight transition.  We will always assume that it is before. 
				if ( dtZoneTime < dtTransitionDate )
					bDaylightSavings = true;
			}
			else if ( nLocalMonth > m_nDAYLIGHT_MONTH && nLocalMonth < m_nSTANDARD_MONTH )
			{
				// If we are solidly in the daylight savings months, then the calculation is simple. 
				bDaylightSavings = true;
			}
			return bDaylightSavings;
		}

		// 04/04/2006 Paul.  SOAP needs a quick way to convert from UniversalTime to ServerTime. 
		public DateTime ToServerTimeFromUniversalTime(DateTime dtUniversalTime)
		{
			if ( dtUniversalTime == DateTime.MinValue )
				return dtUniversalTime;
			// 11/07/2005 Paul.  SugarCRM 3.5 now stores time in GMT. 
			if ( m_bGMTStorage )
				return dtUniversalTime;
			else
				return dtUniversalTime.ToLocalTime();
		}

		// 08/17/2006 Paul.  SOAP needs a quick way to convert from ServerTime to UniversalTime. 
		public DateTime ToUniversalTimeFromServerTime(DateTime dtServerTime)
		{
			if ( dtServerTime == DateTime.MinValue )
				return dtServerTime;
			// 11/07/2005 Paul.  SugarCRM 3.5 now stores time in GMT. 
			if ( m_bGMTStorage )
				return dtServerTime;
			else
				return dtServerTime.ToUniversalTime();
		}

		public DateTime ToServerTimeFromUniversalTime(string sUniversalTime)
		{
			DateTime dtUniversalTime = DateTime.Parse(sUniversalTime);
			return ToServerTimeFromUniversalTime(dtUniversalTime);
		}

		public DateTime ToServerTime(DateTime dtZoneTime)
		{
			DateTime dtUniversalTime = ToUniversalTime(dtZoneTime);
			if ( dtUniversalTime == DateTime.MinValue )
				return dtUniversalTime;
			// 11/07/2005 Paul.  SugarCRM 3.5 now stores time in GMT. 
			if ( m_bGMTStorage )
				return dtUniversalTime;
			else
				return dtUniversalTime.ToLocalTime();
		}

		public DateTime ToServerTime(string sZoneTime)
		{
			if ( sZoneTime == String.Empty )
				return DateTime.MinValue;
			DateTime dtZoneTime = Sql.ToDateTime(sZoneTime);
			if ( dtZoneTime == DateTime.MinValue )
				return dtZoneTime ;
			DateTime dtUniversalTime = ToUniversalTime(dtZoneTime);
			if ( dtUniversalTime == DateTime.MinValue )
				return dtUniversalTime;
			// 11/07/2005 Paul.  SugarCRM 3.5 now stores time in GMT. 
			if ( m_bGMTStorage )
				return dtUniversalTime;
			else
				return dtUniversalTime.ToLocalTime();
		}

		public DateTime ToUniversalTime(DateTime dtZoneTime)
		{
			// 11/07/2005 Paul.  Don't modify if value is MinValue.
			if ( dtZoneTime == DateTime.MinValue )
				return dtZoneTime;
			DateTime dtUniversalTime = dtZoneTime;
			if ( IsDaylightSavings(dtZoneTime) )
			{
				dtUniversalTime = dtUniversalTime.AddMinutes(m_nDAYLIGHT_BIAS);
			}
			// When converting to Universal Time, the bias is removed after any daylight calculations. 
			dtUniversalTime = dtUniversalTime.AddMinutes(m_nBIAS);
			return dtUniversalTime;
		}

		public string Abbreviation(DateTime dtZoneTime)
		{
			string sZone = String.Empty;
			if ( IsDaylightSavings(dtZoneTime) )
			{
				sZone = m_sDAYLIGHT_ABBREVIATION;
			}
			else
			{
				sZone = m_sSTANDARD_ABBREVIATION;
			}
			return sZone;
		}
	}
}
