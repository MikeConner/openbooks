using System;

namespace OpenBookPgh
{
	/// <summary>
	/// Summary description for SearchParamsExpenditures
	/// </summary>
	public class SearchParamsExpenditures
	{
		public SearchParamsExpenditures(){}

		private SearchParamsExpenditures(int candidateID, string office, string vendorKeywords, string vendorSearchOptions, string keywords)
		{
			this.candidateID = candidateID;
			this.office = office;
			this.vendorKeywords = vendorKeywords;
			this.vendorSearchOptions = vendorSearchOptions;
			this.keywords = keywords;
		}
		private int _candidateID;
		public int candidateID
		{
			get { return _candidateID; }
			set { _candidateID = value; }
		}
		private string _office;
		public string office
		{
			get { return _office; }
			set { _office = value; }
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
		private string _keywords;
		public string keywords
		{
			get { return _keywords; }
			set { _keywords = value; }
		}
	}
}
