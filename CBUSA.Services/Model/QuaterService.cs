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
    public class QuaterService : IQuaterService
    {
        private readonly IUnitOfWork _ObjUnitWork;

        public QuaterService(IUnitOfWork ObjUnitWork)
        {
            _ObjUnitWork = ObjUnitWork;
        }

        public IEnumerable<Quater> GetQuater()
        {
            return _ObjUnitWork.Quater.GetAll();
        }

        public IEnumerable<Quater> GetQuaterByDate(DateTime currentdate)
        {
            IEnumerable<Quater> Q = _ObjUnitWork.Quater.Search(x => (currentdate.Date >= x.ReportingStartDate && currentdate.Date <= x.ReportingEndDate));

            if (Q.Count() <= 0)
            {
                Q = _ObjUnitWork.Quater.Search(x => (currentdate.Date >= x.StartDate && currentdate.Date <= x.EndDate));
            }
            return Q;
        }

        public void UpdateQuarterReportingWindow(Int64 QuarterID, DateTime StartDate, DateTime EndDate)
        {
            Quater Q = _ObjUnitWork.Quater.Get(QuarterID);
            Q.ReportingStartDate = StartDate;
            Q.ReportingEndDate = EndDate;

            _ObjUnitWork.Quater.Update(Q);
        }

        public bool IsReportingPeriodOn(DateTime currentdate)
        {
            var GetQuarterForReportingPeriod = _ObjUnitWork.Quater.Search(x => (currentdate.Date >= x.ReportingStartDate && currentdate.Date <= x.ReportingEndDate)).Count();

            if (GetQuarterForReportingPeriod > 0)
                return true;
            else
                return false;
        }
        
        public Quater GetQuaterById(long QuaterId)
        {
            return _ObjUnitWork.Quater.Get(QuaterId);
        }

        public IEnumerable<Quater> GetQuatersUptoCurrentQuater(Int64 QuaterId, Int64 Year)
        {
            return _ObjUnitWork.Quater.Search(x => x.QuaterId <= QuaterId && x.Year == Year);
        }
        
        #region Add ProjectStatus
        public IEnumerable<Quater> GetAllPreviousQuater(Int64 QuaterId)
        {
            var CurrentQuater = _ObjUnitWork.Quater.Get(QuaterId);

            if (CurrentQuater != null)
            {
                return _ObjUnitWork.Quater.Search(x => x.EndDate < CurrentQuater.StartDate);
            }

            return null;
        }

        #endregion
        
        public Quater GetQuaterWithYearQuaterName(Int64 Year,string QuaterName)
        {
            return _ObjUnitWork.Quater.Search(x => x.Year == Year && x.QuaterName == QuaterName).FirstOrDefault();
        }
        
        public IEnumerable<Quater> GetNCPReportingQuarterByCurrentQuarter(Int64 CurrentQuarterId)
        {
            var CurrentQuarter = _ObjUnitWork.Quater.Search(x => x.QuaterId == CurrentQuarterId).Select(y => new { y.QuaterId, y.StartDate, y.EndDate}).FirstOrDefault();
            var ReportingQuarterId = _ObjUnitWork.Quater.Search(x => (x.StartDate < CurrentQuarter.StartDate && x.EndDate < CurrentQuarter.EndDate)).Max(y => y.QuaterId);
            return _ObjUnitWork.Quater.Search(x => x.QuaterId == ReportingQuarterId);
        }

        public Quater GetLastReportingQuarter()
        {
            DateTime CurrentDate = DateTime.Now.Date;
            DateTime CurrentStartDate = _ObjUnitWork.Quater.Search(x => CurrentDate >= x.StartDate  && CurrentDate <= x.EndDate).Select(y => y.StartDate).FirstOrDefault();
            var ReportingQuarterId = _ObjUnitWork.Quater.Search(x => x.EndDate <= CurrentStartDate).OrderByDescending(z => z.StartDate).Select(c=> c.QuaterId).FirstOrDefault();
            return _ObjUnitWork.Quater.Search(x => x.QuaterId == ReportingQuarterId).FirstOrDefault();
        }
    }
}
