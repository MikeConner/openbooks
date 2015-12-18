using System;

namespace OpenBookAllegheny
{
	/// <summary>
	/// Summary description for SearchParamsExpenditures
	/// </summary>
	public class SearchParamsExpenditures
	{
		public SearchParamsExpenditures(){}

		private SearchParamsExpenditures(int candidateID, string office, int datePaid, string vendorKeywords, string vendorSearchOptions, string keywords)
		{
			this.candidateID = candidateID;
			this.office = office;
            this.datePaid = datePaid;
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
        private int _datePaid = 0;
        public int datePaid
        {
            get { return _datePaid; }
            set { _datePaid = value; }
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
