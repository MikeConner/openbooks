using System;

namespace OpenBookPgh
{
    /// <summary>
    /// Class Object for search parameters used in search
    /// </summary>
    /// 
    public class SearchRangeParamsContract
    {
        public static DateTime DEFAULT_START_DATE = Convert.ToDateTime("1/1/1990");

        public SearchRangeParamsContract() { }

        private SearchRangeParamsContract(int vendorID, int contractID, string vendorKeywords, string vendorSearchOptions, int cityDept, int contractType,
        string keywords, DateTime beginDate, DateTime endDate, int minContractAmt, int maxContractAmt)
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
            this.minContractAmt = minContractAmt;
            this.maxContractAmt = maxContractAmt;
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
        private int _contractID = 0;
        public int contractID
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
        private DateTime _beginDate = DEFAULT_START_DATE;
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
        private int _minContractAmt = 0;
        public int minContractAmt
        {
            get { return _minContractAmt; }
            set { _minContractAmt = value; }
        }
        private int _maxContractAmt = 0;
        public int maxContractAmt
        {
            get { return _maxContractAmt; }
            set { _maxContractAmt = value; }
        }
    }
}
