using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBUSA.Domain;
using CBUSA.Services.Interface;
using CBUSA.Repository;

namespace CBUSA.Services.Model
{
    public class BuilderQuaterAdminReportService : IBuilderQuaterAdminReportService
    {
        private readonly IUnitOfWork _ObjUnitWork;
        public BuilderQuaterAdminReportService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }
        public void SaveBuilderQaterReport(BuilderQuaterAdminReport ObjReport, bool disposeDbContext = true)
        {
            _ObjUnitWork.BuilderQuaterAdminReport.Add(ObjReport);
            _ObjUnitWork.Complete();
            // --------- Neyaz On 18-Oct-2017 ---- #8447------ Sart
            //_ObjUnitWork.Dispose();
            if (disposeDbContext)
            {
                _ObjUnitWork.Dispose();
            }
            // --------- Neyaz On 18-Oct-2017 ---- #8447------ End
        }
        public IEnumerable<BuilderQuaterAdminReport> CheckBuilderQuaterReport(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId);
        }
        public IEnumerable<BuilderQuaterAdminReport> GetId(Int64 AdminReportId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderQuaterAdminReportId == AdminReportId);
        }
        public void UpdateBuilderAdminQuaterReport(BuilderQuaterAdminReport ObjReport)
        {
            _ObjUnitWork.BuilderQuaterAdminReport.Update(ObjReport);
            _ObjUnitWork.Complete();
            _ObjUnitWork.Dispose();
        }
        public IEnumerable<BuilderQuaterAdminReport> GetBuilderReport(Int64 BuilderId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.IsSubmit == false && x.RowStatusId == (int)RowActiveStatus.Active);
        }

        #region Add project status

        public IEnumerable<BuilderQuaterAdminReport> GetBuilderQuaterReport(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId);
        }

        #endregion

        #region Rabi

        public bool IsReportAllreadySubmited(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId
                && x.IsSubmit == true && x.RowStatusId == (int)RowActiveStatus.Active).Any();
        }


        public bool IsReportInitiated(Int64 BuilderId, Int64 QuaterId)
        {
            return _ObjUnitWork.BuilderQuaterAdminReport.Search(x => x.BuilderId == BuilderId && x.QuaterId == QuaterId
                && x.RowStatusId == (int)RowActiveStatus.Active).Any();
        }

        #endregion        
    }
}
