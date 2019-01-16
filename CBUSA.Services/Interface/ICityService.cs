using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;

namespace CBUSA.Services.Interface
{
   public interface ICityService
    {
       IEnumerable<City> GetCity();
       IEnumerable<City> FindCityByState(int StateId);
       IEnumerable<City> GetCityByName(string City);
    }
}
