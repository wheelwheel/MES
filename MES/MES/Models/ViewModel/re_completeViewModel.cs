using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MES.Models
{
    public class re_completeViewModel
    {

        public IPagedList<order> ordersList { get; set; }
        public IPagedList<re_complete> re_completesList { get; set; }
        public IPagedList<re_complete_detail> re_complete_detailsList { get; set; }
        
    }
}