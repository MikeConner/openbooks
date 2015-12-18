using System;

namespace OpenBookAllegheny
{
	/// <summary>
	/// Class Object for search parameters used in search
	/// </summary>
	public class SearchParamsContract
	{
		public SearchParamsContract() { }

		private SearchParamsContract(int vendorID, string contractID, string vendorKeywords, string vendorSearchOptions, int cityDept, int contractType, 
		string keywords, DateTime beginDate, DateTime endDate, int contractAmt)
		{
			this.vendorID = vendorID;
			this.contractID = contractID;
			this.vendorKeywords = vendorKeywords;
			this.vendorSearchOptions = vendorSearchOptions;
			this.cityDept = cityDept;
			this.contractType = contractType;
			this.keywords = keywords;
			this.beginDate = beginDate;
			this.endDate = endDate;
			this.contractAmt = contractAmt;
			//this.pageIndex = pageIndex;
		}
		//private int _pageIndex = 0;
		//public int pageIndex
		//{
		//    get { return _pageIndex; }
		//    set { _pageIndex = value; }
		//}
		private int _vendorID = 0;
		public int vendorID
		{
			get { return _vendorID; }
			set { _vendorID = value; }
		}
		private string _contractID = null;
		public string contractID
		{
			get { return _contractID; }
			set { _contractID = value; }
		}
		
		private string _vendorKeywords;
		public string vendorKeywords
		{
			get { return _vendorKeywords; }
			set { _vendorKeywords = value; }
		}
		private string _vendorSearchOptions;
		public string vendorSearchOptions
		{
			get { return _vendorSearchOptions; }
			set { _vendorSearchOptions = value; }
		}
		
		private int _cityDept = 0;
		public int cityDept
		{
			get { return _cityDept; }
			set { _cityDept = value; }
		}
		private int _contractType = 0;
		public int contractType
		{
			get { return _contractType; }
			set { _contractType = value; }
		}
		private string _keywords;
		public string keywords
		{
			get { return _keywords; }
			set { _keywords = value; }
		}
        private DateTime _beginDate = SearchRangeParamsContract.DEFAULT_START_DATE;
		public DateTime beginDate
		{
			get { return _beginDate; }
			set { _beginDate = value; }
		}
		private DateTime _endDate = DateTime.Now;
		public DateTime endDate
		{
			get { return _endDate; }
			set { _endDate = value; }
		}
		private int _contractAmt = 0;
		public int contractAmt
		{
			get { return _contractAmt; }
			set { _contractAmt = value; }
		}
	}
}