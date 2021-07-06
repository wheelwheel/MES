using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace MES.Models
{
    public class RouteViewModel
    {
        public IPagedList<route> routesList { get; set; }

        public IPagedList<route_detail> route_detailsList { get; set; }
    }
}