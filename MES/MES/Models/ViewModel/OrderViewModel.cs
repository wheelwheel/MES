using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MES.Models
{
    public class OrderViewModel
    {
        public IPagedList<order> ordersList { get; set; }

        public IPagedList<order_detail> order_detailsList { get; set; }
    }
}