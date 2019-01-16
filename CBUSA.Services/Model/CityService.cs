using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;
using System.Collections.Generic;

namespace CBUSA.Services.Model
{
    public class CityService : ICityService
    {
          private readonly IUnitOfWork _ObjUnitWork;
          public CityService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
          public IEnumerable<City> GetCity()
          {
              return _ObjUnitWork.City.GetAll();
          }
          public IEnumerable<City> FindCityByState(int StateId)
          {
              return _ObjUnitWork.City.Search(x => x.StateId == StateId && x.IsActive == (int)RowActiveStatus.Active);

          }
          public IEnumerable<City> GetCityByName(string City)
          {
              return _ObjUnitWork.City.Search(x => x.CityName == City && x.IsActive == (int)RowActiveStatus.Active);
          }
    }
}
