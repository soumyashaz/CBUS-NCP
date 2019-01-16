using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CBUSA.Models
{
    public static class BuildModelError
    {

        public static string GetModelError(string[] ModelError)
        {
            StringBuilder Sb = new StringBuilder();
            Sb.Append("<ul>");

            foreach (string Error in ModelError)
            {
                Sb.Append("<li>&nbsp");
                Sb.Append(Error);
                Sb.Append("&nbsp</li>");
            }

            Sb.Append("</ul>");
            return Sb.ToString();
        }

    }
}