using CBUSA.Areas.CbusaBuilder.Models;
using CBUSA.Domain;
using CBUSA.Services;
using CBUSA.Services.Interface;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Claims;
using Spire.Xls;

namespace CBUSA.Areas.CbusaBuilder.Controllers
{
    public class OfflineReportingController : Controller
    {
        private enum _EXCEL_COLUMN_INDEX
        {
            _COL_INDEX_SL_NO = 1,
            _COL_INDEX_PROJECT_STATUS = 2,
            _COL_INDEX_PROJECT_ID = 3,
            _COL_INDEX_PROJECT_NAME = 4,
            _COL_INDEX_LOT_NO = 5,
            _COL_INDEX_ADDRESS = 6,
            _COL_INDEX_CITY = 7,
            _COL_INDEX_STATE = 8,
            _COL_INDEX_ZIP = 9
        }

        readonly IStateService _ObjStateService;
        readonly IQuaterService _ObjQuaterService;
        readonly IContractServices _ObjContractService;
        readonly ISurveyService _ObjSurveyService;
        readonly IProjectService _ObjProjectService;
        readonly IContractBuilderService _ObjContractBuilderService;
        readonly IQuestionService _ObjQuestionService;
        readonly IProjectStatusService _ObjProjectStatusService;
        readonly IBuilderQuaterAdminReportService _ObjQuaterAdminReportService;
        readonly IBuilderQuarterContractStatusService _ObjBuilderQuaterContractStatusService;
        readonly IBuilderQuaterContractProjectReportService _ObjQuaterContractProjectReport;
        readonly IBuilderQuaterContractProjectDetailsService _ObjQuaterContractProjectDetails;

        public OfflineReportingController(IProjectService ObjProjectService, IQuaterService ObjQuaterService, IStateService ObjStateService, 
                                            IContractServices ObjContractService, ISurveyService ObjSurveyService,
                                            IContractBuilderService ObjContractBuilderService, IQuestionService ObjQuestionService,
                                            IProjectStatusService ObjProjectStatusService, IBuilderQuaterAdminReportService ObjQuaterAdminReportService,
                                            IBuilderQuarterContractStatusService ObjBuilderQuaterContractStatusService, IBuilderQuaterContractProjectReportService ObjQuaterContractProjectReport,
                                            IBuilderQuaterContractProjectDetailsService ObjQuaterContractProjectDetails
                                         )
        {
            _ObjProjectService = ObjProjectService;
            _ObjQuaterService = ObjQuaterService;
            _ObjStateService = ObjStateService;
            _ObjContractService = ObjContractService;
            _ObjSurveyService = ObjSurveyService;
            _ObjContractBuilderService = ObjContractBuilderService;
            _ObjQuestionService = ObjQuestionService;            
            _ObjProjectStatusService = ObjProjectStatusService;
            _ObjQuaterAdminReportService = ObjQuaterAdminReportService;
            _ObjQuaterContractProjectReport = ObjQuaterContractProjectReport;
            _ObjBuilderQuaterContractStatusService = ObjBuilderQuaterContractStatusService;
            _ObjQuaterContractProjectDetails = ObjQuaterContractProjectDetails;
        }

        // GET: CbusaBuilder/OfflineReporting
        public ActionResult Index()
        {
            return View();
        }

        public FileResult ProjectList()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            var AllProjectList = _ObjProjectService.BuilderProjectList(BuilderId)
                                 .Where(x => x.RowStatusId == (int)RowActiveStatus.Active || x.RowStatusId == (int)RowActiveStatus.Deleted).
                                    Select(x => new ProjectViewModel
                                    {
                                        //ContractList = GetContractListForBuilderProject(x.BuilderId, x.ProjectId),
                                        ProjectId = x.ProjectId,
                                        BuilderId = x.BuilderId,
                                        ProjectName = x.ProjectName,
                                        LotNo = x.LotNo,
                                        Address = x.Address,
                                        City = x.City,
                                        State = x.State,
                                        Zip = x.Zip,
                                        RowStatusId = x.RowStatusId
                                    }).OrderBy(x => x.RowStatusId).ThenBy(x => x.ProjectId);

            //************************ POPULATE LIST OF STATES ***************************
            var AllStates = _ObjStateService.GetState();

            //*****************************************************************************************************************
            //************************************************** GENERATE EXCEL ***********************************************
            Workbook wb = new Workbook();

            object missing = Type.Missing;

            Worksheet sheet = wb.Worksheets[0];
            sheet.Name = "Projects";

            //--------------- HEADER COLUMNS ---------------
            sheet.Range["A1"].Value = "#";
            sheet.Range["B1"].Value = "Status";
            sheet.Range["C1"].Value = "Project Id";
            sheet.Range["D1"].Value = "Project Name";
            sheet.Range["E1"].Value = "Lot No.";
            sheet.Range["F1"].Value = "Address";
            sheet.Range["G1"].Value = "City";
            sheet.Range["H1"].Value = "State";
            sheet.Range["I1"].Value = "Zip";

            for (int i = 1; i <= 9; i++)
            {
                string colName = String.Concat(GetColumnAlphabetFromIndex(i), "1");
                sheet.Range[colName].Style.Font.IsBold = true;
                sheet.Range[colName].Style.Font.Color = System.Drawing.Color.DarkSlateBlue;
            }

            //************************ POPULATE PROJECT STATUS LIST (OPEN/CLOSED) ***************************
            sheet.Range[1, 25].Value = "OPEN";
            sheet.Range[2, 25].Value = "CLOSED";

            //------------- POPULATE STATE LIST IN 'Z' COLUMN --------------
            int StateListRowIndex = 1;
            foreach (State State in AllStates)
            {
                sheet.Range[StateListRowIndex, 26].Value = State.StateName;
                StateListRowIndex++;
            }
            sheet.HideColumn(26);

            //------------------ ADD PROJECT ROWS -----------------
            int RowIndex = 2;

            foreach (ProjectViewModel Proj in AllProjectList)
            {
                sheet.Range[RowIndex, 1].NumberValue = (RowIndex - 1);

                //---------- ADD PROJECT STATUS LIST ----------
                CellRange ProjectStatusRange = sheet.Range["Y1:Y2"];
                sheet.Range[RowIndex, 2].DataValidation.DataRange = ProjectStatusRange;
                sheet.Range[RowIndex, 2].Value = (Proj.RowStatusId == (int)RowActiveStatus.Active ? "OPEN" : "CLOSED");

                sheet.Range[RowIndex, 3].NumberValue = Proj.ProjectId;
                sheet.Range[RowIndex, 3].Style.Locked = true;

                sheet.Range[RowIndex, 4].Value = Proj.ProjectName;
                sheet.Range[RowIndex, 5].Value = Proj.LotNo;
                sheet.Range[RowIndex, 6].Value = Proj.Address;
                sheet.Range[RowIndex, 7].Value = Proj.City;

                //--------- ADD STATE DROPDOWN LIST --------
                string StateColRange = String.Concat("Z1:", "Z", StateListRowIndex.ToString());
                CellRange StateRange = sheet.Range[StateColRange];
                sheet.Range[RowIndex, 8].DataValidation.DataRange = StateRange;
                sheet.Range[RowIndex, 8].Value = Proj.State;

                //sheet.Range[RowIndex, 9].Style. NumberFormat = "@";
                sheet.Range[RowIndex, 9].Value = "'" + Proj.Zip.ToString();

                RowIndex++;
            }

            for (int i = RowIndex; i <= (RowIndex + 500); i++)
            {
                sheet.Range[i, 3].NumberValue = 0;

                CellRange ProjectStatusRange = sheet.Range["Y1:Y2"];
                sheet.Range[i, 2].DataValidation.DataRange = ProjectStatusRange;

                string StateColRange = String.Concat("Z1:", "Z", StateListRowIndex.ToString());
                CellRange StateRange = sheet.Range[StateColRange];
                sheet.Range[RowIndex, 8].DataValidation.DataRange = StateRange;
                sheet.Range[i, 8].DataValidation.DataRange = StateRange;
            }

            //---------- COPY SHEET TO KEEP ORIGINAL LIST ----------
            wb.Worksheets[1].Remove();
            wb.Worksheets.AddCopyAfter(wb.Worksheets[0]);
            wb.Worksheets[2].Remove();

            Worksheet sheet_original = wb.Worksheets[1];
            sheet_original.Name = "ProjectList_Original";
            sheet_original.Visibility = WorksheetVisibility.StrongHidden;

            //---------- HIDE Project ID Column -----------
            sheet.HideColumn(3);
            sheet.HideColumn(25);
            //---------------------------------------------

            //---------- HIDE Project ID Column -----------
            sheet.SetColumnWidth(4, 35);
            sheet.SetColumnWidth(6, 50);
            sheet.SetColumnWidth(7, 20);
            sheet.SetColumnWidth(8, 20);
            //---------------------------------------------

            //--------- FREEZE FIRST ROW ------------------
            sheet.FreezePanes(2, 1);
            //---------------------------------------------

            sheet.Activate();

            //---------- SAVE FILE ------------
            string FileName = string.Concat("ProjectList_", BuilderId.ToString(), ".xlsx");
            string FilePath = Server.MapPath("~/App_Data/") + FileName;

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            int sheets = wb.Worksheets.Count();
            wb.SaveToFile(FilePath, FileFormat.Version2013);

            //---------- RETURN FILE ------------
            return File(FilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public string UpdateProjectList(IEnumerable<HttpPostedFileBase> UploadProjectFile)
        {
            if (UploadProjectFile != null)
            {
                var fileName = "";
                var fileExt = "";

                foreach (var file in UploadProjectFile)
                {
                    fileName = String.Concat(DateTime.Now.Hour.ToString(), "_", DateTime.Now.Minute.ToString(), "_", DateTime.Now.Second.ToString(), Path.GetExtension(file.FileName));
                    fileExt = Path.GetExtension(file.FileName);

                    var dir = Server.MapPath("~/App_Data/");
                    var subDir = Server.MapPath("~/App_Data/Uploads");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        Directory.CreateDirectory(subDir);
                    }

                    if (!Directory.Exists(subDir))
                    {
                        Directory.CreateDirectory(subDir);
                    }

                    var physicalPath = Path.Combine(subDir, fileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }

                    file.SaveAs(physicalPath);

                    ReadProjectListExcel(physicalPath);
                }

                return "";
            }
            else
            {
                return "";
            }
        }

        public string ReadProjectListExcel(string UploadedFilePath)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            object missing = Type.Missing;

            Workbook xlWorkBook = new Workbook();
            xlWorkBook.LoadFromFile(UploadedFilePath);

            Worksheet xlModifiedWorkSheet;
            Worksheet xlOriginalWorkSheet;

            xlModifiedWorkSheet = xlWorkBook.Worksheets[0];    //This is the sheet updated by Builder
            xlOriginalWorkSheet = xlWorkBook.Worksheets[1];    //This is the original Project File sheet (kept hidden from Builders)

            //------ UNHIDE PROJECT ID COLUMN TO READ ITS CONTENT ------
            xlModifiedWorkSheet.ShowColumn(3);
            xlOriginalWorkSheet.ShowColumn(3);

            CellRange rngColProjectName = xlModifiedWorkSheet.Range["D1:D500"];   //Get the entire Project Name column

            // Find the last row which has value entered in the Project Name column
            int lastModifiedRow = xlModifiedWorkSheet.LastRow;
            for (int i = xlModifiedWorkSheet.LastRow; i >= 0; i--)
            {
                CellRange cr = xlModifiedWorkSheet.Rows[i - 1].Columns[1];
                if (!cr.IsBlank)
                {
                    lastModifiedRow = i;
                    break;
                }
            }

            string ColumnHeader1 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_STATUS].Value;
            string ColumnHeader2 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_NAME].Value;
            string ColumnHeader3 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_LOT_NO].Value;
            string ColumnHeader4 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_ADDRESS].Value;
            string ColumnHeader5 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_CITY].Value;
            string ColumnHeader6 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_STATE].Value;
            string ColumnHeader7 = xlModifiedWorkSheet.Range[1, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_ZIP].Value;

            if (ColumnHeader1 != "Status" || ColumnHeader2 != "Project Name" || ColumnHeader3 != "Lot No." || ColumnHeader4 != "Address"
                || ColumnHeader5 != "City" || ColumnHeader6 != "State" || ColumnHeader7 != "Zip")
            {
                return "Invalid Excel file format :: Missing Headers";
            }

            for (int rowIndex = 2; rowIndex <= lastModifiedRow; rowIndex++)
            {
                var OriginalProjStatus = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_STATUS].Value;
                var ModifiedProjStatus = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_STATUS].Value;

                var OriginalProjId = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_ID].NumberText;
                var ModifiedProjId = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_ID].NumberText;

                var OriginalProjName = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_NAME].Value;
                var ModifiedProjName = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_PROJECT_NAME].Value;

                var OriginalLotNo = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_LOT_NO].Value;
                var ModifiedLotNo = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_LOT_NO].Value;

                var OriginalAddress = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_ADDRESS].Value;
                var ModifiedAddress = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_ADDRESS].Value;

                var OriginalCity = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_CITY].Value;
                var ModifiedCity = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_CITY].Value;

                var OriginalState = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_STATE].Value;
                var ModifiedState = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_STATE].Value;

                var OriginalZip = xlOriginalWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_ZIP].Value;
                var ModifiedZip = xlModifiedWorkSheet.Range[rowIndex, (int)_EXCEL_COLUMN_INDEX._COL_INDEX_ZIP].Value;

                bool DisposeDbContext = (rowIndex == lastModifiedRow ? true : false);

                Project _objProject = new Project();

                _objProject.BuilderId = BuilderId;
                _objProject.ProjectName = ModifiedProjName;
                _objProject.LotNo = ModifiedLotNo;
                _objProject.Address = ModifiedAddress;
                _objProject.City = ModifiedCity;
                _objProject.State = ModifiedState;
                _objProject.Zip = ModifiedZip;

                if (OriginalProjId.ToString() == "0" && ModifiedProjId.ToString() == "0")           //New Project added - not in existing list
                {
                    _objProject.RowStatusId = (int)RowActiveStatus.Active;

                    _ObjProjectService.SaveProject(_objProject, DisposeDbContext);
                }
                else
                {
                    if (ModifiedProjId == OriginalProjId)   //Indicates the row has not been moved, deleted or tampered with
                    {
                        _objProject.ProjectId = Convert.ToInt64(OriginalProjId);
                        _objProject.RowStatusId = (ModifiedProjStatus == "OPEN" ? (int)RowActiveStatus.Active : (int)RowActiveStatus.Deleted);

                        _ObjProjectService.UpdateProject(_objProject, DisposeDbContext);
                    }
                    else
                    {
                        _objProject.ProjectId = Convert.ToInt64(ModifiedProjId);
                        _objProject.RowStatusId = (ModifiedProjStatus == "OPEN" ? (int)RowActiveStatus.Active : (int)RowActiveStatus.Deleted);

                        _ObjProjectService.UpdateProject(_objProject, DisposeDbContext);
                    }
                }
            }

            System.IO.File.Delete(UploadedFilePath);

            return "";
        }

        public string GetContractListForBuilderProject(Int64 BuilderId, Int64 ProjectId)
        {
            var AllContract = _ObjQuaterContractProjectReport.GetLatestContractAgainstBuilderProject(BuilderId, ProjectId);
            var ContractList = AllContract.Select(x => x.Contract.ContractName).Distinct().ToList().ToString();

            return ContractList;
        }

        public FileResult QuarterReport()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            DateTime currentdate = DateTime.Now.Date;
            Quater Q = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            Int64 QuaterId = Q.QuaterId;

            var EnrolledContractList = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId);

            //*****************************************************************************************************************
            //************************************************** GENERATE EXCEL ***********************************************
            Workbook wb = new Workbook();
            wb.Worksheets[2].Remove();
            wb.Worksheets[1].Remove();

            //---------- CREATE A SHEET FOR EACH CONTRACT ------------
            int iContractCounter = 1;
            foreach (Contract objContract in EnrolledContractList)
            {
                int DropDownColumnIndex = 26;

                int TotalSheetCount = wb.Worksheets.Count;
                wb.Worksheets.AddCopy(wb.Worksheets[0]);

                Worksheet sheet = wb.Worksheets[(wb.Worksheets.Count - 1)];
                sheet.Name = objContract.ContractName;

                long ContractId = objContract.ContractId;

                //--------------- ADD EACH REPORTABLE PROJECT IN FIRST COLUMN ------------
                var ProjectPrevReportedOrNeverReportMarked = _ObjQuaterContractProjectReport.GetRepotDetails(BuilderId, ContractId)
                                                              .Where(bqcpr => bqcpr.QuaterId != QuaterId
                                                              && (bqcpr.ProjectStatusId == (int)EnumProjectStatus.ReportUnit || bqcpr.ProjectStatusId == (int)EnumProjectStatus.NeverReport))
                                                              .Select(bqcpr => bqcpr.ProjectId).Distinct().ToList();

                var AllProjectList = _ObjProjectService.BuilderProjectList(BuilderId).Where(x => x.RowStatusId == (int)RowActiveStatus.Active
                                        && !ProjectPrevReportedOrNeverReportMarked.Contains(x.ProjectId)).
                                        Select(x => new ProjectViewModel
                                        {
                                            ProjectId = x.ProjectId,
                                            BuilderId = x.BuilderId,
                                            ProjectName = x.ProjectName,
                                            LotNo = x.LotNo,
                                            Address = x.Address,
                                            City = x.City,
                                            State = x.State,
                                            Zip = x.Zip,
                                            RowStatusId = x.RowStatusId
                                        }).OrderBy(x => x.RowStatusId).ThenBy(x => x.ProjectId);


                //----------- POPULATE PROJECT REPORTING STATUS LIST -------------
                sheet.Range[1, DropDownColumnIndex].Value = "Report Units";
                sheet.Range[2, DropDownColumnIndex].Value = "Nothing to Report";
                sheet.Range[3, DropDownColumnIndex].Value = "Never Report";

                sheet.HideColumn(DropDownColumnIndex);
                DropDownColumnIndex++;
                //-----------------------------------------------------------------

                //--------------- HEADER COLUMNS ---------------
                sheet.Range[1, 1].Value = "#";
                sheet.Range[1, 2].Value = "Project Id";
                sheet.Range[1, 3].Value = "Project";
                sheet.Range[1, 4].Value = "Reporting Status";

                sheet.SetColumnWidth(3, 40);     //Project
                sheet.SetColumnWidth(4, 20);     //Project Reporting Status

                for (int i = 1; i <= 50; i++)
                {
                    sheet.Range[1, i].Style.Font.IsBold = true;
                    sheet.Range[1, i].Style.Font.Color = System.Drawing.Color.DarkSlateBlue;

                    if (i >= 5)         //Question columns
                    {
                        sheet.Range[1, i].Style.WrapText = true;
                        sheet.SetColumnWidth(i, 40);
                    }
                }

                int RowIndex = 3;
                foreach (ProjectViewModel Proj in AllProjectList)
                {
                    //-------- SERIAL NO. COLUMN (A) ------------
                    sheet.Range[RowIndex, 1].NumberValue = (RowIndex - 2);

                    //-------- PROJECT ID COLUMN (B) ------------
                    sheet.Range[RowIndex, 2].NumberValue = Proj.ProjectId;

                    //---------- PROJECT DETAILS COLUMN (C) ----------
                    string ProjectDetails = String.Concat(Proj.ProjectName, ",", Proj.LotNo, ",", Proj.Address, ",", Proj.City);
                    sheet.Range[RowIndex, 3].Value = ProjectDetails;

                    //--------- ADD PROJECT REPORTING STATUS DROPDOWN LIST (D) --------
                    CellRange ProjectReportingStatus = sheet.Range["Z1:Z3"];

                    sheet.Range[RowIndex, 4].DataValidation.DataRange = ProjectReportingStatus;

                    RowIndex++;
                }

                //--------------- ADD QUESTIONS OF NCP SURVEY IN EACH COLUMN ------------
                Survey objContractCurrentNCPSurvey = _ObjSurveyService.FindContractNcpSurveysAllByQuarter(ContractId, Q.QuaterName, Q.Year.ToString()).Where(s => s.IsPublished == true).FirstOrDefault();

                if (objContractCurrentNCPSurvey != null)
                {
                    IEnumerable<Question> SurveyQuestionList = _ObjQuestionService.GetSurveyQuestionAll(objContractCurrentNCPSurvey.SurveyId);

                    bool IncrementColIndex = true;

                    int ColIndex = 5;
                    foreach (Question objQuestion in SurveyQuestionList)
                    {
                        sheet.Range[1, ColIndex].Style.Font.IsBold = true;
                        sheet.Range[1, ColIndex].Style.Font.Color = System.Drawing.Color.DarkSlateBlue;

                        //-------- QUESTION ID ------------
                        sheet.Range[1, ColIndex].NumberValue = objQuestion.QuestionId;
                        sheet.HideColumn(ColIndex);

                        ColIndex++;

                        //-------- QUESTION COLUMN ------------
                        if (objQuestion.QuestionTypeId == 1)            //-------------- TEXT TYPE QUESTION ---------------
                        {
                            sheet.Range[2, (ColIndex - 1)].Value = "1";       //-------------- TO INDICATE TEXT-TYPE QUESTION ----------
                            sheet.Range[1, ColIndex].Value = objQuestion.QuestionValue;
                        }
                        else if (objQuestion.QuestionTypeId == 2)       //-------------- DROP-DOWN TYPE QUESTION ---------------
                        {
                            sheet.Range[2, (ColIndex - 1)].Value = "2";       //-------------- TO INDICATE DROPDOWN-TYPE QUESTION ----------
                            sheet.Range[1, ColIndex].Value = objQuestion.QuestionValue;

                            var objQuesDropDownList = objQuestion.QuestionDropdownSetting;
                            int drpDownRowIndex = 1;
                            foreach (QuestionDropdownSetting objDrpDnSetting in objQuesDropDownList)
                            {
                                if (objDrpDnSetting.Value.ToString() != "SELECT")
                                {
                                    sheet.Range[drpDownRowIndex, DropDownColumnIndex].Value = objDrpDnSetting.Value;
                                    drpDownRowIndex++;
                                }
                            }
                            sheet.HideColumn(DropDownColumnIndex);

                            string ValidationRule = String.Concat(GetColumnAlphabetFromIndex(DropDownColumnIndex), "1:", GetColumnAlphabetFromIndex(DropDownColumnIndex), (drpDownRowIndex - 1).ToString());
                            CellRange DrpDownQuestionRange = sheet.Range[ValidationRule];

                            for (int i = 3; i <= RowIndex; i++)
                            {
                                sheet.Range[i, ColIndex].DataValidation.DataRange = DrpDownQuestionRange;
                            }

                            DropDownColumnIndex++;
                        }
                        else        //-------------- GRID TYPE QUESTION ---------------
                        {
                            IncrementColIndex = false;

                            int GridColumns = objQuestion.QuestionGridSetting.FirstOrDefault().Column;
                            int GridRows = objQuestion.QuestionGridSetting.FirstOrDefault().Row;

                            sheet.Range[2, (ColIndex - 1)].Value = String.Concat(GridRows.ToString(), "|", GridColumns.ToString());

                            int RowCounter = 1;
                            int ColumnCounter = 1;

                            if (GridColumns > 1 && GridRows == 1)
                            {
                                IEnumerable<QuestionGridSettingHeader> GridColHeaders = objQuestion.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.Where(qgsh => !String.IsNullOrEmpty(qgsh.ColoumnHeaderValue));

                                int GridColCounter = 0;
                                foreach (QuestionGridSettingHeader objQGSH in GridColHeaders)
                                {
                                    string QuestionText = String.Concat(objQGSH.ColoumnHeaderValue);

                                    sheet.Range[1, ColIndex].Value = QuestionText;
                                    sheet.Range[2, ColIndex].Value = String.Concat("0", "|", GridColCounter.ToString());

                                    GridColCounter++;

                                    if (objQGSH.ControlType == "2")
                                    {
                                        var DropDownOptions = objQGSH.DropdownTypeOptionValue.Split(',').ToList();

                                        int drpDownRowIndex = 1;
                                        foreach (string strDrpDownOption in DropDownOptions)
                                        {
                                            if (strDrpDownOption != "SELECT")
                                            {
                                                sheet.Range[drpDownRowIndex, DropDownColumnIndex].Value = strDrpDownOption;
                                                drpDownRowIndex++;
                                            }
                                        }
                                        sheet.HideColumn(DropDownColumnIndex);

                                        string ValidationRule = String.Concat(GetColumnAlphabetFromIndex(DropDownColumnIndex), "1:", GetColumnAlphabetFromIndex(DropDownColumnIndex), (drpDownRowIndex - 1).ToString());
                                        CellRange DrpDownQuestionRange = sheet.Range[ValidationRule];

                                        for (int i = 3; i <= RowIndex; i++)
                                        {
                                            sheet.Range[i, ColIndex].DataValidation.DataRange = DrpDownQuestionRange;
                                        }

                                        DropDownColumnIndex++;
                                    }

                                    ColumnCounter++;
                                    ColIndex++;
                                }
                            }
                            else if (GridColumns == 1 && GridRows > 1)
                            {
                                IEnumerable<QuestionGridSettingHeader> GridRowHeaders = objQuestion.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.Where(qgsh => !String.IsNullOrEmpty(qgsh.RowHeaderValue));

                                int GridRowCounter = 0;

                                foreach (QuestionGridSettingHeader objQGSH in GridRowHeaders)
                                {
                                    string QuestionText = String.Concat(objQGSH.RowHeaderValue);

                                    sheet.Range[1, ColIndex].Value = QuestionText;
                                    sheet.Range[2, ColIndex].Value = String.Concat(GridRowCounter.ToString(), "|", "0");

                                    GridRowCounter++;

                                    if (objQGSH.ControlType == "2")
                                    {
                                        var DropDownOptions = objQGSH.DropdownTypeOptionValue.Split(',').ToList();

                                        int drpDownRowIndex = 1;
                                        foreach (string strDrpDownOption in DropDownOptions)
                                        {
                                            if (strDrpDownOption != "SELECT")
                                            {
                                                sheet.Range[drpDownRowIndex, DropDownColumnIndex].Value = strDrpDownOption;
                                                drpDownRowIndex++;
                                            }
                                        }
                                        sheet.HideColumn(DropDownColumnIndex);

                                        string ValidationRule = String.Concat(GetColumnAlphabetFromIndex(DropDownColumnIndex), "1:", GetColumnAlphabetFromIndex(DropDownColumnIndex), (drpDownRowIndex - 1).ToString());
                                        CellRange DrpDownQuestionRange = sheet.Range[ValidationRule];

                                        for (int i = 3; i <= RowIndex; i++)
                                        {
                                            sheet.Range[i, ColIndex].DataValidation.DataRange = DrpDownQuestionRange;
                                        }

                                        DropDownColumnIndex++;
                                    }

                                    RowCounter++;
                                    ColIndex++;
                                }
                            }
                            else
                            {
                                IEnumerable<QuestionGridSettingHeader> GridRowHeaders = objQuestion.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.Where(qgsh => !String.IsNullOrEmpty(qgsh.RowHeaderValue));

                                int GridRowCounter = 0;

                                foreach (QuestionGridSettingHeader objQGSHRow in GridRowHeaders)
                                {
                                    string QuestionRowHeader = objQGSHRow.RowHeaderValue;

                                    IEnumerable<QuestionGridSettingHeader> GridColHeaders = objQuestion.QuestionGridSetting.FirstOrDefault().QuestionGridSettingHeader.Where(qgsh => !String.IsNullOrEmpty(qgsh.ColoumnHeaderValue));

                                    int GridColumnCounter = 0;
                                    foreach (QuestionGridSettingHeader objQGSHCol in GridColHeaders)
                                    {
                                        string QuestionText = String.Concat(QuestionRowHeader, " : ", objQGSHCol.ColoumnHeaderValue);

                                        sheet.Range[1, ColIndex].Value = QuestionText;
                                        sheet.Range[2, ColIndex].Value = String.Concat(GridRowCounter.ToString(), "|", GridColumnCounter.ToString());

                                        GridColumnCounter++;

                                        if (objQGSHRow.ControlType == "2")
                                        {
                                            var DropDownOptions = objQGSHRow.DropdownTypeOptionValue.Split(',').ToList();

                                            int drpDownRowIndex = 1;
                                            foreach (string strDrpDownOption in DropDownOptions)
                                            {
                                                if (strDrpDownOption != "SELECT")
                                                {
                                                    sheet.Range[drpDownRowIndex, DropDownColumnIndex].Value = strDrpDownOption;
                                                    drpDownRowIndex++;
                                                }
                                            }
                                            sheet.HideColumn(DropDownColumnIndex);

                                            string ValidationRule = String.Concat(GetColumnAlphabetFromIndex(DropDownColumnIndex), "1:", GetColumnAlphabetFromIndex(DropDownColumnIndex), (drpDownRowIndex - 1).ToString());
                                            CellRange DrpDownQuestionRange = sheet.Range[ValidationRule];

                                            for (int i = 3; i <= RowIndex; i++)
                                            {
                                                sheet.Range[i, ColIndex].DataValidation.DataRange = DrpDownQuestionRange;
                                            }

                                            DropDownColumnIndex++;
                                        }

                                        ColumnCounter++;
                                        ColIndex++;
                                    }

                                    GridRowCounter++;

                                    RowCounter++;
                                }
                            }
                        }

                        if (IncrementColIndex)
                        {
                            ColIndex++;
                        }
                    }
                }

                //---------- HIDE Project ID Column -----------
                sheet.HideColumn(2);
                sheet.HideRow(2);
                //---------------------------------------------

                //--------- FREEZE FIRST ROW ------------------
                sheet.FreezePanes(2, 1);
                //---------------------------------------------

                sheet.Activate();

                iContractCounter++;
            }

            wb.Worksheets[0].Remove();

            //---------- SAVE FILE ------------
            string FileName = string.Concat("QuarterlyReporting_", BuilderId.ToString(), ".xlsx");
            string FilePath = Server.MapPath("~/App_Data/") + FileName;

            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
            wb.SaveToFile(FilePath, FileFormat.Version2013);

            //---------- RETURN FILE ------------
            return File(FilePath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public string UploadQuarterReport(IEnumerable<HttpPostedFileBase> UploadQuarterReportFile)
        {
            if (UploadQuarterReportFile != null)
            {
                var fileName = "";
                var fileExt = "";

                foreach (var file in UploadQuarterReportFile)
                {
                    fileName = String.Concat(DateTime.Now.Hour.ToString(), "_", DateTime.Now.Minute.ToString(), "_", DateTime.Now.Second.ToString(), Path.GetExtension(file.FileName));
                    fileExt = Path.GetExtension(file.FileName);

                    var dir = Server.MapPath("~/App_Data/");
                    var subDir = Server.MapPath("~/App_Data/Uploads");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                        Directory.CreateDirectory(subDir);
                    }

                    if (!Directory.Exists(subDir))
                    {
                        Directory.CreateDirectory(subDir);
                    }

                    var physicalPath = Path.Combine(subDir, fileName);

                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }

                    file.SaveAs(physicalPath);

                    ReadQuarterReportExcel(physicalPath);
                }

                return "";
            }
            else
            {
                return "";
            }
        }

        public string ReadQuarterReportExcel(string UploadedFilePath)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string bldid = claims.FirstOrDefault(p => p.Type == ClaimTypes.Sid).Value;
            Int64 BuilderId = Convert.ToInt64(bldid);

            //-------------------- GET CURRENT QUARTER ID ---------------------
            DateTime currentdate = DateTime.Now.Date;
            Quater Q = _ObjQuaterService.GetQuaterByDate(currentdate).FirstOrDefault();
            Int64 QuaterId = Q.QuaterId;
            //------------------------------------------------------------------

            //------------------- GET BUILDER QUARTER ADMIN REPORT ID ----------
            Int64 QuarterAdminReportId = 0;
            InsertQuaterAdminReportIfNotExists(QuaterId, BuilderId, ref QuarterAdminReportId);
            //---------------------------------------------------------------------------------------------------

            int TotalContractsToReport = _ObjContractBuilderService.GetActiveOnlyContractsRegularReportingBybuilderJoining(BuilderId, QuaterId).Count();

            Workbook xlWorkBook = new Workbook();
            xlWorkBook.LoadFromFile(UploadedFilePath);

            int TotalWorksheets = xlWorkBook.Worksheets.Count();

            bool SubmitQuarterlyReport = true;
            int TotalContractsReported = 0;
            int TotalContractSheets = 0;

            for (int ContractCounter = 0; ContractCounter < TotalWorksheets; ContractCounter++)
            {
                bool AllProjectNTRTQ = true;
                bool AllProjectReported = true;

                Worksheet xlContractWorkSheet = xlWorkBook.Worksheets[ContractCounter];

                //------------------- GET BUILDER QUARTER ADMIN REPORT ID ----------
                Contract C = _ObjContractService.GetActiveContract().Where(c => c.ContractName == xlContractWorkSheet.Name).FirstOrDefault();

                Int64 BuilderQuarterContractStatusId = 0;
                if (C != null)
                {
                    TotalContractSheets++;                    
                    InsertBuilderQuarterContractStatus(BuilderId, C.ContractId, QuaterId, QuarterAdminReportId, ref BuilderQuarterContractStatusId);
                }
                //---------------------------------------------------------------------------------------------------

                if (C != null)
                {
                    int lastModifiedRow = xlContractWorkSheet.LastRow;
                    for (int j = xlContractWorkSheet.LastRow; j >= 0; j--)
                    {
                        CellRange cr = xlContractWorkSheet.Rows[j - 1].Columns[1];
                        if (!cr.IsBlank)
                        {
                            lastModifiedRow = j;
                            break;
                        }
                    }

                    int UnReportedProjectCount = 0;
                    int ContractProjectCounter = 0;

                    for (int ProjectCounter = 3; ProjectCounter <= lastModifiedRow; ProjectCounter++)
                    {
                        ContractProjectCounter++;

                        int ProjectId = (int)xlContractWorkSheet.Range[ProjectCounter, 2].NumberValue;
                        string ProjectReportStatus = xlContractWorkSheet.Range[ProjectCounter, 4].Value;

                        if (ProjectReportStatus == "")
                        {
                            AllProjectReported = false;
                            SubmitQuarterlyReport = false;
                            UnReportedProjectCount++;
                        }
                        else
                        {
                            BuilderQuaterContractProjectReport BQCPR = _ObjQuaterContractProjectReport.CheckExistProjectAgainstBuilderQuaterContract(BuilderId, QuaterId, C.ContractId, ProjectId).FirstOrDefault();

                            EnumProjectStatus CurrProjectStatusId;
                            if (ProjectReportStatus == "Nothing to Report" || ProjectReportStatus == "Never Report")
                            {
                                CurrProjectStatusId = (ProjectReportStatus == "Nothing to Report" ? EnumProjectStatus.NothingtoReport : EnumProjectStatus.NeverReport);
                            }
                            else
                            {
                                AllProjectNTRTQ = false;
                                CurrProjectStatusId = EnumProjectStatus.ReportUnit;
                            }

                            if (BQCPR == null)
                            {
                                BQCPR = new BuilderQuaterContractProjectReport();
                                BQCPR.ProjectId = ProjectId;
                                BQCPR.BuilderId = BuilderId;
                                BQCPR.QuaterId = QuaterId;
                                BQCPR.ContractId = C.ContractId;
                                BQCPR.ProjectStatusId = (long)CurrProjectStatusId;
                                BQCPR.IsComplete = true;
                                BQCPR.RowStatusId = (int)RowActiveStatus.Active;
                                BQCPR.CreatedOn = DateTime.Now;
                                BQCPR.ModifiedOn = DateTime.Now;
                                BQCPR.CreatedBy = 1;
                                BQCPR.ModifiedBy = 1;
                                BQCPR.CompleteDate = DateTime.Now;
                                BQCPR.RowGUID = Guid.NewGuid();
                                BQCPR.BuilderQuarterContractStatusId = BuilderQuarterContractStatusId;
                                BQCPR.BuilderQuaterAdminReportId = QuarterAdminReportId;

                                _ObjQuaterContractProjectReport.SaveProjectReport(BQCPR, false);
                            }
                            else
                            {
                                BQCPR.ProjectStatusId = (long)CurrProjectStatusId;
                                BQCPR.ModifiedOn = DateTime.Now;
                                BQCPR.CompleteDate = DateTime.Now;
                                _ObjQuaterContractProjectReport.UpdateProjectReport(BQCPR);
                            }

                            //----------------------- INSERT REBATE REPORT ANSWERS ------------------------
                            int LastQuestionColumn = 25;    //From 26th column the options for drop-down questions are listed

                            for (int QuestionCounter = 5; QuestionCounter <= LastQuestionColumn; QuestionCounter++)
                            {
                                bool IsColumnVisible = xlContractWorkSheet.IsColumnVisible(QuestionCounter);

                                if (!IsColumnVisible)               //-------- COLUMN CONTAINS QUESTION ID
                                {
                                    bool ProjectReportDetailsExist = false;

                                    int QuestionId = (int)xlContractWorkSheet.Range[1, QuestionCounter].NumberValue;
                                    string QuestionTypeGridCol = xlContractWorkSheet.Range[2, QuestionCounter].Value;

                                    if (QuestionTypeGridCol == "1" || QuestionTypeGridCol == "2")
                                    {
                                        BuilderQuaterContractProjectDetails BQCPD = _ObjQuaterContractProjectDetails.GetProjectDetailsForBuilderQuaterContractProjectReport(BQCPR.BuilderQuaterContractProjectReportId).Where(bqcpr => bqcpr.QuestionId == QuestionId).FirstOrDefault();

                                        if (BQCPD == null)
                                        {
                                            BQCPD = new BuilderQuaterContractProjectDetails();
                                            BQCPD.BuilderQuaterContractProjectReportId = BQCPR.BuilderQuaterContractProjectReportId;
                                            BQCPD.QuestionId = QuestionId;
                                            BQCPD.RowStatusId = (int)RowActiveStatus.Active;
                                            BQCPD.RowNumber = 0;
                                            BQCPD.ColumnNumber = 0;
                                            BQCPD.CreatedOn = DateTime.Now;
                                            BQCPD.CreatedBy = 1;
                                            BQCPD.ModifiedOn = DateTime.Now;
                                            BQCPD.ModifiedBy = 1;
                                            BQCPD.RowGUID = Guid.NewGuid();
                                        }
                                        else
                                        {
                                            ProjectReportDetailsExist = true;
                                        }

                                        string Answer = xlContractWorkSheet.Range[ProjectCounter, (QuestionCounter + 1)].Value;
                                        BQCPD.Answer = Answer;

                                        if (ProjectReportDetailsExist)
                                        {
                                            _ObjQuaterContractProjectDetails.UpdateProjectReportDetails(BQCPD, false);
                                        }
                                        else
                                        {
                                            _ObjQuaterContractProjectDetails.AddProjectReportDetails(BQCPD, false);
                                        }

                                        QuestionCounter++;
                                    }
                                    else
                                    {
                                        string[] arrGridQuestionRowCol = QuestionTypeGridCol.Split('|');

                                        int GridQuestionRow = Convert.ToInt32(arrGridQuestionRowCol[0]);
                                        int GridQuestionCol = Convert.ToInt32(arrGridQuestionRowCol[1]);

                                        int TotalColumnsToRead = (GridQuestionRow * GridQuestionCol);
                                        int GridQuestionCounter = 0;

                                        for (GridQuestionCounter = 1; GridQuestionCounter <= TotalColumnsToRead; GridQuestionCounter++)
                                        {
                                            ProjectReportDetailsExist = false;

                                            string RowColNumber = xlContractWorkSheet.Range[2, (QuestionCounter + GridQuestionCounter)].Value;
                                            string[] arrRowColNumber = RowColNumber.Split('|');

                                            int RowNumber = Convert.ToInt16(arrRowColNumber[0]);
                                            int ColNumber = Convert.ToInt16(arrRowColNumber[1]);

                                            BuilderQuaterContractProjectDetails BQCPD = _ObjQuaterContractProjectDetails.GetProjectDetailsForBuilderQuaterContractProjectReport(BQCPR.BuilderQuaterContractProjectReportId)
                                                                                        .Where(bqcpr => bqcpr.QuestionId == QuestionId && bqcpr.RowNumber == RowNumber && bqcpr.ColumnNumber == ColNumber).FirstOrDefault();

                                            if (BQCPD == null)
                                            {
                                                BQCPD = new BuilderQuaterContractProjectDetails();
                                                BQCPD.BuilderQuaterContractProjectReportId = BQCPR.BuilderQuaterContractProjectReportId;
                                                BQCPD.QuestionId = QuestionId;
                                                BQCPD.RowStatusId = (int)RowActiveStatus.Active;
                                                BQCPD.RowNumber = RowNumber;
                                                BQCPD.ColumnNumber = ColNumber;
                                                BQCPD.CreatedOn = DateTime.Now;
                                                BQCPD.CreatedBy = 1;
                                                BQCPD.ModifiedOn = DateTime.Now;
                                                BQCPD.ModifiedBy = 1;
                                                BQCPD.RowGUID = Guid.NewGuid();
                                            }
                                            else
                                            {
                                                BQCPD.RowNumber = RowNumber;
                                                BQCPD.ColumnNumber = ColNumber;
                                                BQCPD.ModifiedOn = DateTime.Now;
                                                ProjectReportDetailsExist = true;
                                            }

                                            string Answer = xlContractWorkSheet.Range[ProjectCounter, (QuestionCounter + GridQuestionCounter)].Value;
                                            BQCPD.Answer = Answer;

                                            if (ProjectReportDetailsExist)
                                            {
                                                _ObjQuaterContractProjectDetails.UpdateProjectReportDetails(BQCPD, false);
                                            }
                                            else
                                            {
                                                _ObjQuaterContractProjectDetails.AddProjectReportDetails(BQCPD, false);
                                            }
                                        }

                                        QuestionCounter = (QuestionCounter + GridQuestionCounter) - 1;
                                    }
                                }
                            }
                        }

                        //------------------------------------------------------------------------------------------------
                    }

                    //------------------- UPDATE BUILDER QUARTER CONTRACT STATUS -----------------
                    ProjectReportStatusEnum ContractProjectReportStatus = ProjectReportStatusEnum.Completed;

                    if (UnReportedProjectCount == ContractProjectCounter)
                    {
                        ContractProjectReportStatus = ProjectReportStatusEnum.NotStarted;
                    }

                    UpdateBuilderQuarterContractStatus(BuilderId, BuilderQuarterContractStatusId, (AllProjectNTRTQ == true ? QuarterContractStatusEnum.NothingToReportThisQuarter : QuarterContractStatusEnum.ReportForThisQuarter), (AllProjectReported == true ? ProjectReportStatusEnum.Completed : ContractProjectReportStatus));
                    //-----------------------------------------------------------------------------

                    if (AllProjectNTRTQ || AllProjectReported)
                    {
                        TotalContractsReported++;
                    }
                }
            }

            System.IO.File.Delete(UploadedFilePath);

            //------------------- MARK BUILDER QUARTER REPORT SUBMITTED ----------
            if (SubmitQuarterlyReport && (TotalContractsReported == TotalContractsToReport))
            {
                UpdateQuaterAdminReportStatus(QuaterId, BuilderId);
            }
            //--------------------------------------------------------------------

            return "";
        }

        public string SendErrorMessage()
        {
            return "Invalid Excel file format :: Missing Headers";
        }

        public string GetColumnAlphabetFromIndex(int ColumnIndex)
        {
            string[] Alphabets = new string[] {
                                                "", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                                                "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
                                              };

            return Alphabets[ColumnIndex];
        }

        private void InsertQuaterAdminReportIfNotExists(Int64 qauterid, Int64 BuilderId, ref Int64 BuilderQuaterAdminReportId)
        {
            var resultset = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, qauterid);
            if (resultset.Count() == 0)
            {
                BuilderQuaterAdminReport objReport = new BuilderQuaterAdminReport();
                objReport.BuilderId = BuilderId;
                objReport.QuaterId = qauterid;
                objReport.SubmitDate = DateTime.Now.Date;
                objReport.IsSubmit = false;
                objReport.RowGUID = Guid.NewGuid();

                _ObjQuaterAdminReportService.SaveBuilderQaterReport(objReport, false);
                BuilderQuaterAdminReportId = objReport.BuilderQuaterAdminReportId;
            }
            else
            {
                BuilderQuaterAdminReportId = resultset.FirstOrDefault().BuilderQuaterAdminReportId;
            }
        }

        private void UpdateQuaterAdminReportStatus(Int64 qauterid, Int64 BuilderId)
        {
            BuilderQuaterAdminReport BQAR = _ObjQuaterAdminReportService.CheckBuilderQuaterReport(BuilderId, qauterid).FirstOrDefault();
            BQAR.IsSubmit = true;
            _ObjQuaterAdminReportService.SaveBuilderQaterReport(BQAR, false);
        }

        private void InsertBuilderQuarterContractStatus(Int64 BuilderId, Int64 ContractId, Int64 QuarterId, Int64 QuarterAdminReportId, ref Int64 BuilderQuaterContractStatusId)
        {
            var BuilderQuaterContractStatus = _ObjBuilderQuaterContractStatusService.CheckExistContractAgainstBuilderQuater(Convert.ToInt64(BuilderId), Convert.ToInt64(QuarterId), Convert.ToInt64(ContractId)).FirstOrDefault();
            if (BuilderQuaterContractStatus == null)
            {
                BuilderQuaterContractStatus = new BuilderQuarterContractStatus()
                {
                    BuilderId = BuilderId,
                    ContractId = ContractId,
                    QuaterId = QuarterId,
                    QuarterContractStatusId = (Int64)QuarterContractStatusEnum.ReportForThisQuarter,
                    ProjectReportStatusId = (Int64)ProjectReportStatusEnum.NotStarted,
                    BuilderQuaterAdminReportId = QuarterAdminReportId,
                    SubmitDate = DateTime.Now.Date
                };
                _ObjBuilderQuaterContractStatusService.AddBuilderQuarterContractStatus(BuilderQuaterContractStatus);
            }

            BuilderQuaterContractStatusId = BuilderQuaterContractStatus.BuilderQuarterContractStatusId;
        }

        private void UpdateBuilderQuarterContractStatus(Int64 BuilderId, Int64 BuilderQuaterContractStatusId, QuarterContractStatusEnum ContractStatus, ProjectReportStatusEnum ProjectReportStatus)
        {
            BuilderQuarterContractStatus BQCS = _ObjBuilderQuaterContractStatusService.CheckExistingBuilderRecord(BuilderId).Where(bqcs => bqcs.BuilderQuarterContractStatusId == BuilderQuaterContractStatusId).FirstOrDefault();

            if (BQCS != null)
            {
                BQCS.QuarterContractStatusId = (Int64)ContractStatus;
                BQCS.ProjectReportStatusId = (Int64)ProjectReportStatus;

                _ObjBuilderQuaterContractStatusService.UpdateBuilderQuarterContractStatus(BQCS);
            }
        }
    }
}