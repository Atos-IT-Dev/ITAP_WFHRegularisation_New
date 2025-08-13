using System;
using System.Configuration;

namespace WFHRegularisationBLL
{
	public class WFHRegularisation
	{
		#region Variables.
		private int m_WFMEmployeeTimeInTimeOutID;
		private string m_Name;
		private string m_Company;
		private string m_DASID;
		private string m_SAPNumber;
		private string m_Location;
		private string m_OrgUnit;
		private string m_RegularisationDate;
		//private DateTime m_TimeIn;
		//private DateTime m_TimeOut;
		private TimeSpan m_TimeIn;
		private TimeSpan m_TimeOut;
		private string m_ServiceLine;
		private string m_TimeInTimeOutType;
		private bool m_isInShift;
		private bool m_isTimeEntryComplete;
		#endregion

		#region Properties.
		public int WFMEmployeeTimeInTimeOutID
		{
			get
			{
				return m_WFMEmployeeTimeInTimeOutID;
			}

			set
			{
				m_WFMEmployeeTimeInTimeOutID = value;
			}
		}
		public string Name
		{
			get
			{
				return m_Name;
			}

			set
			{
				m_Name = value;
			}
		}

		public string Company
		{
			get
			{
				return m_Company;
			}

			set
			{
				m_Company = value;
			}
		}

		public string DASID
		{
			get
			{
				return m_DASID;
			}

			set
			{
				m_DASID = value;
			}
		}

		public string SAPNumber
		{
			get
			{
				return m_SAPNumber;
			}

			set
			{
				m_SAPNumber = value;
			}
		}

		public string Location
		{
			get
			{
				return m_Location;
			}

			set
			{
				m_Location = value;
			}
		}

		public string OrgUnit
		{
			get
			{
				return m_OrgUnit;
			}

			set
			{
				m_OrgUnit = value;
			}
		}

		public string RegularisationDate
		{
			get
			{
				return m_RegularisationDate;
			}

			set
			{
				m_RegularisationDate = value;
			}
		}

		public TimeSpan TimeIn
		{
			get
			{
				return m_TimeIn;
			}

			set
			{
				m_TimeIn = value;
			}
		}

		public TimeSpan TimeOut
		{
			get
			{
				return m_TimeOut;
			}

			set
			{
				m_TimeOut = value;
			}
		}

		public string ServiceLine
		{
			get
			{
				return m_ServiceLine;
			}

			set
			{
				m_ServiceLine = value;
			}
		}

		public string TimeInTimeOutType
		{
			get
			{
				return m_TimeInTimeOutType;
			}

			set
			{
				m_TimeInTimeOutType = value;
			}
		}

		public bool IsInShift
		{
			get
			{
				return m_isInShift;
			}

			set
			{
				m_isInShift = value;
			}
		}
		public bool IsTimeEntryComplete
		{
			get
			{
				return m_isTimeEntryComplete;
			}

			set
			{
				m_isTimeEntryComplete = value;
			}
		}
		#endregion

		#region Methods.
		public bool CanAccessTimeInTimeOut(string CompanyCode)
		{
			bool blnCanAccess = false;

			string[] strEntityCodes = ConfigurationManager.AppSettings["AllowedEntities"].ToString().Split(',');

			for (int i = 0; i < strEntityCodes.Length; i++)
			{
				if (CompanyCode.ToUpper() == strEntityCodes[i].ToString().ToUpper())
				{
					blnCanAccess = true;
					break;
				}
			}
			return blnCanAccess;
		}

		public string GetMonthNamefromMonthNumber(int MonthNumber)
		{
			string strMonthName = "";

			switch(MonthNumber)
			{
				case 1:
					strMonthName = "JAN";
					break;
				case 2:
					strMonthName = "FEB";
					break;
				case 3:
					strMonthName = "MAR";
					break;
				case 4:
					strMonthName = "APR";
					break;
				case 5:
					strMonthName = "MAY";
					break;
				case 6:
					strMonthName = "JUN";
					break;
				case 7:
					strMonthName = "JUL";
					break;
				case 8:
					strMonthName = "AUG";
					break;
				case 9:
					strMonthName = "SEP";
					break;
				case 10:
					strMonthName = "OCT";
					break;
				case 11:
					strMonthName = "NOV";
					break;
				case 12:
					strMonthName = "DEC";
					break;
			}
			return strMonthName;
		}
		#endregion
	}
}
