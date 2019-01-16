using System;
using System.Collections.Generic;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class NonResponderReportService : INonResponderReportService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public NonResponderReportService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<dynamic> GetNonResponderBuilderList(Int64 QuarterId)
        {
            return _ObjUnitWork.NonResponderReport.GetNonResponderBuilderList(QuarterId);
        }
    }
}
