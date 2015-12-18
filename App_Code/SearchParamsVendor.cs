using System;

namespace OpenBookAllegheny
{
	/// <summary>
	/// Summary description for SearchParamsVendor
	/// </summary>
	public class SearchParamsVendor
	{
		public SearchParamsVendor(){}
		private SearchParamsVendor(int vendorID)
		{
			this.vendorID = vendorID;
		}
		private int _vendorID = 0;
		public int vendorID
		{
			get { return _vendorID; }
			set { _vendorID = value; }
		}
		
	}
}
