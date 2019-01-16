using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Services.Interface;
namespace CBUSA.Services.Model
{
    public class RandomNo : IRandom
    {

        public string StringRandom(string Prefix)
        {

            // _ObjPH.Encrypt(UName + "~iNeedItWeb~" + Convert.ToString(DateTime.Now), true, KeyString.Trim());
            string userTokenVal = Encrypt.EncryptValue(Prefix + Guid.NewGuid().ToString() + Convert.ToString(DateTime.Now), true);
            userTokenVal = userTokenVal.Replace("~", "sc");
            userTokenVal = userTokenVal.Replace("`", "sc");
            userTokenVal = userTokenVal.Replace("!", "sc");
            userTokenVal = userTokenVal.Replace("@", "sc");
            userTokenVal = userTokenVal.Replace("#", "sc");
            userTokenVal = userTokenVal.Replace("$", "sc");
            userTokenVal = userTokenVal.Replace("%", "sc");
            userTokenVal = userTokenVal.Replace("^", "sc");
            userTokenVal = userTokenVal.Replace("&", "sc");
            userTokenVal = userTokenVal.Replace("*", "sc");
            userTokenVal = userTokenVal.Replace("(", "sc");
            userTokenVal = userTokenVal.Replace(")", "sc");
            userTokenVal = userTokenVal.Replace("-", "sc");
            userTokenVal = userTokenVal.Replace("_", "sc");
            userTokenVal = userTokenVal.Replace("+", "sc");
            userTokenVal = userTokenVal.Replace("=", "sc");
            userTokenVal = userTokenVal.Replace("{", "sc");
            userTokenVal = userTokenVal.Replace("}", "sc");
            userTokenVal = userTokenVal.Replace("[", "sc");
            userTokenVal = userTokenVal.Replace("]", "sc");
            userTokenVal = userTokenVal.Replace("|", "sc");
            userTokenVal = userTokenVal.Replace("\\", "sc");
            userTokenVal = userTokenVal.Replace(":", "sc");
            userTokenVal = userTokenVal.Replace(";", "sc");
            userTokenVal = userTokenVal.Replace("'", "sc");
            userTokenVal = userTokenVal.Replace("\"", "sc");
            userTokenVal = userTokenVal.Replace(",", "sc");
            userTokenVal = userTokenVal.Replace("<", "sc");
            userTokenVal = userTokenVal.Replace(".", "sc");
            userTokenVal = userTokenVal.Replace(">", "sc");
            userTokenVal = userTokenVal.Replace("/", "sc");
            userTokenVal = userTokenVal.Replace("?", "sc");
            return userTokenVal;
        }
    }
}
