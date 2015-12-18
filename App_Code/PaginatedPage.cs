using System;
using System.Collections.Generic;
using System.Web.UI;
using OpenBookAllegheny;

namespace OpenBookAllegheny
{
    /// <summary>
    /// Base class for all pages in the OpenBook system that are paginated (e.g., 10, 25, 50, 100 per page)
    /// This includes common properties for current page, size and total count, which will be manipulated through a contained PagingControls instance
    /// </summary>
    public class PaginatedPage : Page
    {
        public PaginatedPage()
        {
        }

        //Paging properties
        public int PageIndex
        {
            get
            {
                return getPageIndex();
            }

            set
            {
                setPageIndex(value);
            }
        }

        public int PageSize
        {
            get
            {
                object o = ViewState["_PageSize"];
                int numResults = null == o ? 0 : (int)o;

                if (0 == numResults)
                {
                    numResults = Utils.GetIntFromQueryString(Request.QueryString["num"]);

                    if (0 == numResults) {
                        numResults = PagingControls.DEFAULT_PAGE_SIZE;
                    }
                }
                updatePageSize(numResults);

                return numResults;
            }

            set
            {
                setPageSize(value);
            }
        }

        public int PageCount
        {
            get
            {
                object o = ViewState["_PageCount"];
                if (null == o)
                {
                    return 0; // default no pages found
                }
                else
                {
                    return (int)o;
                }
            }

            set
            {
                ViewState["_PageCount"] = value;
            }
        }

        public string PageCategory
        {
            get
            {
                return getPageCategory();
            }
        }

        // Must override in subclass; get text for banner (e.g., "Contributions", "Expenditures")
        protected virtual string getPageCategory()
        {
            throw new Exception("Subclass responsibility");
        }

        // Must override in subclasses; set page size text in graphics
        protected virtual void updatePageSize(int numResults)
        {
            throw new Exception("Subclass responsibility");
        }

        // Can override with a blank method to disallow (e.g., admin pages)
        protected virtual void setPageIndex(int pageIndex)
        {
            ViewState["_PageIndex"] = pageIndex;
        }

        // Can override with a blank method to disallow (e.g., admin pages)
        protected virtual void setPageSize(int pageSize)
        {
            ViewState["_PageSize"] = pageSize;
        }

        // Override this in subclasses if it is different
        protected virtual int getPageIndex()
        {
            object o = ViewState["_PageIndex"];
            if (null == o)
            {
                return 0; // default no pages found
            }
            else
            {
                return (int)o;
            }
        }
    }
}
