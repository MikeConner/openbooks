using System;
using System.Collections.Generic;
using System.Web;

namespace OpenBookPgh
{
    /// <summary>
    /// Common code to keep track of first/last/prev/next for all pages
    /// Operates on a PaginatedPage (base class for all pages with page controls)
    /// </summary>
    public class PagingControls
    {
        public const int DEFAULT_PAGE_SIZE = 10;

        public PagingControls(PaginatedPage page)
        {
            mPage = page;
        }

        // Calculate total number of pages available for display, given the total number of rows
        public void setPageCount(int totalRows)
        {
            // Set the local TotalRows property to cache it for future calls
            TotalRows = totalRows;

            // Unless the pageSize divides exactly evenly, there will be a "leftovers" page, so add 1 in that case
            int extraPageOffset = (0 == (totalRows % mPage.PageSize)) ? 0 : 1;

            // Set the host page's PageCount property
            mPage.PageCount = (totalRows / mPage.PageSize) + extraPageOffset;
        }

        // If calling this after setPageCount (which caches the rows in the local TotalRows property), can just use the property
        public string getPagingBanner()
        {
            return getPagingBanner(TotalRows);
        }

        // If calling this before setPageCount, or any time the property might not be defined, supply an explicit totalRows value
        public string getPagingBanner(int totalRows)
        {
            // Calculate Results & Update Pager
            int startResults = (mPage.PageIndex * mPage.PageSize) + 1;
            int endResults = (mPage.PageIndex * mPage.PageSize) + mPage.PageSize;

            if (endResults > totalRows)
            {
                endResults = totalRows;
            }

            return 0 == startResults ? "We couldn't find any " + mPage.PageCategory + " that matched your criteria." : "Results: " + startResults.ToString() + " - " + endResults.ToString() + " of " + totalRows.ToString();
        }

        public int TotalRows {
            get {
                return (-1 == mRows) ? 0 : mRows;
            }

            set
            {
                mRows = value;
            }
        }

        private PaginatedPage mPage = null;
        private int mRows = -1;
    }
}

