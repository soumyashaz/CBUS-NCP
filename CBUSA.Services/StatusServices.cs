using CBUSA.Domain;
using CBUSA.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBUSA.Services
{
    //public class StatusServices : CBUSA.Services.IStatusServices
    //{
    //    private readonly IUnitOfWork _ObjUnitWork;

    //    public StatusServices(IUnitOfWork ObjUnitWork)
    //    {
    //        _ObjUnitWork = ObjUnitWork;
    //    }

    //    public IEnumerable<Status> GetStatus()
    //    {
    //        var list = _ObjUnitWork.Statuses.GetAll();
    //        return list;
    //    }

    //    public void SaveStatus(Status ObjStatus)
    //    {
    //        _ObjUnitWork.Statuses.Add(ObjStatus);
    //        _ObjUnitWork.Complete();
    //        _ObjUnitWork.Dispose();
    //    }

    //    public void EditStatus(Status ObjStatus)
    //    {
    //        _ObjUnitWork.Statuses.Update(ObjStatus);
    //        _ObjUnitWork.Complete();
    //        _ObjUnitWork.Dispose();
    //    }

    //    public void DeleteStatus(Status ObjStatus)
    //    {
    //        _ObjUnitWork.Statuses.Update(ObjStatus);
    //        _ObjUnitWork.Complete();
    //        _ObjUnitWork.Dispose();
    //    }

    //}
}
