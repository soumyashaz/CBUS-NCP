using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CBUSA.Domain;
namespace CBUSA.Models
{
    public class CustomProjectControlVm
    {
        public List<Project> ProjectList { get; set; }
        public List<Int64> SelectedProject { get; set; }

    }
}