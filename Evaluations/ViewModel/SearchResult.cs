using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using MongoDB.Results;
using Evaluations.Models;

namespace Evaluations.ViewModel
{
    public class SearchResult
    {
        public int? Page { get; set; }
        public string CourseTitle { get; set; }
        public string Barcode { get; set; }
        public string SearchButton { get; set; }
        public string CurrentFilter { get; set; }
        public string DisplayFilter { get; set; }
        public string SearchType { get; set; }
        public IPagedList<Courses> SearchResults{ get; set; }
    }
}