using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PagedList;

namespace MES.Models
{
    public class ProcessViewModel
    {
        public IPagedList<process> processList { get; set; }
    }
}