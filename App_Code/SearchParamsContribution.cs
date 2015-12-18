using System;

namespace OpenBookAllegheny
{
	/// <summary>
	/// Summary description for SearchParamsContribution
	/// </summary>
	public class SearchParamsContribution
	{
		public SearchParamsContribution(){}

		private SearchParamsContribution(int candidateID, string office, int dateContribution, string contributorKeywords, string contributorSearchOptions, 
										string employerKeywords, string zip, double radius)
		{
			this.candidateID = candidateID;
			this.office = office;
			this.dateContribution = dateContribution;
			this.contributorKeywords = contributorKeywords;
			this.contributorSearchOptions = contributorSearchOptions;
			this.employerKeywords = employerKeywords;
			this.zip = zip;
			this.radius = radius;
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
		private int _dateContribution = 0;
		public int dateContribution
		{
			get { return _dateContribution; }
			set { _dateContribution = value; }
		}
		private string _contributorKeywords;
		public string contributorKeywords
		{
			get { return _contributorKeywords; }
			set { _contributorKeywords = value; }
		}
		private string _contributorSearchOptions;
		public string contributorSearchOptions
		{
			get { return _contributorSearchOptions; }
			set { _contributorSearchOptions = value; }
		}
		private string _employerKeywords;
		public string employerKeywords
		{
			get { return _employerKeywords; }
			set { _employerKeywords = value; }
		}
		private string _zip;
		public string zip
		{
			get { return _zip; }
			set { _zip = value; }
		}
		private double _radius = 0;
		public double radius
		{
			get { return _radius; }
			set { _radius = value; }
		}

	}
}
