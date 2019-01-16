
using CBUSA.Areas.CbusaBuilder.Models;
using CBUSA.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;

namespace CBUSA.Models
{
    public static class CustomHelper
    {
        public static MvcHtmlString FileIcon(this HtmlHelper helper, String FilePath,
                                                object htmlAttributes = null)
        {

            string StringBuilder = string.Empty;

            string Extension = System.IO.Path.GetExtension(FilePath);

            if (Extension == "doc" || Extension == "docx" || Extension == ".doc" || Extension == ".docx")
            {
                StringBuilder = "<i class='fa fa-file-word-o blue' aria-hidden='true'></i>";
            }
            else if (Extension == "xls" || Extension == "xlsx" || Extension == ".xls" || Extension == ".xlsx")
            {
                StringBuilder = "<i class='fa fa-file-excel-o green' aria-hidden='true'></i>";
            }
            else if (Extension == "ppt" || Extension == "pptx" || Extension == ".ppt" || Extension == ".ppx")
            {
                StringBuilder = "<i class='fa fa-file-powerpoint-o' aria-hidden='true'></i>";
            }
            else if (Extension == "pdf" || Extension == ".pdf")
            {
                StringBuilder = "<i class='fa fa-file-pdf-o red' aria-hidden='true'></i>";
            }
            else if (Extension == "png" || Extension == "jpg" || Extension == "jpeg" || Extension == ".png" || Extension == ".jpg" || Extension == ".jpeg")
            {
                StringBuilder = "<i class='fa fa-file-image-o' aria-hidden='true'></i>";
            }
            else
            {
                StringBuilder = "<i class=' fa fa-file' aria-hidden='true'></i>";
            }



            return new MvcHtmlString(StringBuilder.ToString());

        }
        public static MvcHtmlString PreviewTextBoxTypeQuestion(this HtmlHelper helper, string Question, bool IsFileUploadAvail,
                                                object htmlAttributes = null)
        {

            StringBuilder Sb = new StringBuilder();
            Sb.Append("<div class='form-group'><label class='control-label col-md-12 col-sm-12'>" + Question + "</label><div class='clearfix'></div></div>");
            Sb.Append("<div class='form-group'><div class='col-md-12  col-sm-12'><input class='form-control' type='text' /><div class='clearfix'></div></div></div>");
            if (IsFileUploadAvail)
            {
                Sb.Append("<div class='form-group'><div class='col-md-12  col-sm-12'><input  type='checkbox' checked  disabled='disabled' /> Enable Builder to upload files <div class='clearfix'></div></div></div>");
                //Sb.Append("<div class='clearfix'></div></div>");
            }
            return new MvcHtmlString(Sb.ToString());
        }

        public static MvcHtmlString PreviewDropListTypeQuestion(this HtmlHelper helper, string Question, ICollection<QuestionDropdownSetting> ObjList, bool IsFileUploadAvail,
                                               object htmlAttributes = null)
        {

            StringBuilder Sb = new StringBuilder();
            Sb.Append("<div class='form-group'><label class='control-label col-md-12 col-sm-12'>" + Question + "</label><div class='clearfix'></div> </div>");
            Sb.Append("<div class='col-md-12  col-sm-12'><div class='form-group selectRightIcon'><select class='form-control'>");

            foreach (QuestionDropdownSetting Item in ObjList)
            {
                Sb.Append("<option>" + Item.Value + "</option>");
            }
            Sb.Append("</select><div class='clearfix'></div></div> </div>");

            if (IsFileUploadAvail)
            {
                Sb.Append("<div class='form-group'><div class='col-md-12  col-sm-12'><input  type='checkbox' checked  disabled='disabled' /> Enable Builder to upload files <div class='clearfix'></div></div></div>");
            }
            return new MvcHtmlString(Sb.ToString());
        }

        public static MvcHtmlString PreviewGridTypeQuestion(this HtmlHelper helper, string Question, ICollection<QuestionGridSettingHeader> ObjList,
                                              object htmlAttributes = null)
        {
            // int Count = 1;
            StringBuilder Sb = new StringBuilder();
            Sb.Append("<div class='form-group'><label class='control-label col-md-12 col-sm-12'>" + Question + "</label></div>");
            if (ObjList.Count > 0)
            {
                var RowList = ObjList.Where(x => x.ColoumnHeaderValue == null);
                var ColList = ObjList.Where(x => x.RowHeaderValue == null);
                Sb.Append("<div class='form-group'><div class='col-md-12  col-sm-12'><table class='table table-sm'>");

                if (ColList.Count() > 0)
                {
                    Sb.Append("<thead><tr><th class='text-center'></th>");
                    foreach (QuestionGridSettingHeader Item in ColList)
                    {
                        Sb.Append("<th class='text-center'>" + Item.ColoumnHeaderValue + "</th>");
                    }
                    Sb.Append("</tr></thead>");
                }
                if (RowList.Count() > 0)
                {
                    Sb.Append("<tbody>");
                    foreach (QuestionGridSettingHeader Item in RowList)
                    {
                        Sb.Append("<tr><th class='text-left' scope='row'>" + Item.RowHeaderValue + "</th>");

                        foreach (var Itemchild in ColList)
                        {
                            if (Itemchild.ControlType == "1") //text box type
                            {
                                Sb.Append("<td class='text-center'><input class='form-control' type='text'></td>");
                            }
                            else   //drop down type
                            {
                                Sb.Append("<td class='text-center'>");
                                Sb.Append("<div class='selectRightIcon'><select class='form-control'>");
                                string[] DropDownValue = Itemchild.DropdownTypeOptionValue.Split(',');
                                foreach (var ItemOption in DropDownValue)
                                {
                                    Sb.Append("<option>" + ItemOption + "</option>");
                                }
                                Sb.Append("</select></div>");
                                Sb.Append("</td>");
                            }
                        }
                        Sb.Append("</tr>");
                    }
                    Sb.Append("</tbody>");
                }
                Sb.Append("</table> <div class='clearfix'></div></div></div>");
            }
            return new MvcHtmlString(Sb.ToString());
        }

        public static MvcHtmlString ContractLogo(this HtmlHelper helper, byte[] image,
                                               object htmlAttributes = null)
        {

            var builder = new TagBuilder("img");

            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var imageString = "";
            if (image != null)
            {
                imageString = Convert.ToBase64String(image);
            }
            else
            {
                imageString = "";
            }

            //var imageString = image != null ? Convert.ToBase64String(image) : "";
            var img = string.Format("data:image/png;base64,{0}", imageString);
            builder.MergeAttribute("src", img);
           // builder.Attributes["style"] = "width:50%";
           // builder.Attributes["style"] = "height:100px";
            builder.Attributes["id"] = "ContractLogoImage";
            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));

        }

        public static MvcHtmlString ShowLogo(this HtmlHelper helper, byte[] image,
                                               object htmlAttributes = null)
        {

            var builder = new TagBuilder("img");

            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var imageString = "";
            if (image != null)
            {
                imageString = Convert.ToBase64String(image);
            }
            else
            {
                imageString = "";
            }

            //var imageString = image != null ? Convert.ToBase64String(image) : "";
            var img = string.Format("data:image/png;base64,{0}", imageString);
            builder.MergeAttribute("src", img);

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));

        }


        public static MvcHtmlString TakeSurveyTextBoxTypeQuestion(this HtmlHelper helper, SurveyResult ObjSurveyResult, ICollection<QuestionTextBoxSetting> ObjList, int QuestionOrder
                                               , object htmlAttributes = null)
        {
            StringBuilder Sb = new StringBuilder();
            if (ObjSurveyResult != null)
            {
                switch (ObjList.FirstOrDefault().TextBoxTypeId)
                {
                    case 1: //Text
                        Sb.Append("<input data-check='true' name='txt_" + QuestionOrder + "' class='form-control' value='" + ObjSurveyResult.Answer + "' type='text'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                    case 2: //Number
                        Sb.Append("<input data-check='true' onkeyup='numericFilter(this)' name='txt_" + QuestionOrder + "' class='form-control' value='" + ObjSurveyResult.Answer + "' type='text'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                    case 3: //Email
                        Sb.Append("<input data-check='true' name='txt_" + QuestionOrder + "' class='form-control' value='" + ObjSurveyResult.Answer + "' type='email'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                    case 4: //Phone
                        Sb.Append("<input data-check='true' data-phonenumber='true' name='txt_" + QuestionOrder + "' class='form-control' value='" + ObjSurveyResult.Answer + "' type='text'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                }
            }
            else
            {
                switch (ObjList.FirstOrDefault().TextBoxTypeId)
                {
                    case 1:
                        Sb.Append("<input data-check='true' name='txt_" + QuestionOrder + "' class='form-control' type='text'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                    case 2:
                        Sb.Append("<input data-check='true' onkeyup='numericFilter(this)' name='txt_" + QuestionOrder + "' class='form-control' type='text'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                    case 3:
                        Sb.Append("<input data-check='true' name='txt_" + QuestionOrder + "' class='form-control' type='email'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                    case 4:
                        Sb.Append("<input data-check='true' data-phonenumber='true' name='txt_" + QuestionOrder + "' class='form-control' type='text'>");
                        Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + "'></span>");
                        break;
                }
            }
            Sb.Append("<input type='hidden' name='HdnTextBoxTypeId' value='" + ObjList.FirstOrDefault().TextBoxTypeId + "' />");
            switch (ObjList.FirstOrDefault().TextBoxTypeId)
            {
                case 1: //Text
                    Sb.Append("<input type='hidden' name='HdnAllowOnly' value='" + ObjList.FirstOrDefault().IsAlphabets + "," + ObjList.FirstOrDefault().IsNumber + "," + ObjList.FirstOrDefault().IsSpecialCharecter + "' />");
                    break;
                case 2: //Number
                    Sb.Append("<input type='hidden' name='HdnIsNumberLimit' value='" + ObjList.FirstOrDefault().LowerLimit + "," + ObjList.FirstOrDefault().UpperLimit + "' />");
                    break;
            }
            Sb.Append("<div class='clearfix'></div>");
            return new MvcHtmlString(Sb.ToString());
        }


        public static MvcHtmlString TakeDropListTypeQuestion(this HtmlHelper helper, SurveyResult ObjSurveyResult, ICollection<QuestionDropdownSetting> ObjList,
                                               object htmlAttributes = null)
        {
            if (ObjSurveyResult != null)
            {
                StringBuilder Sb = new StringBuilder();
                Sb.Append("<div class='form-group'>");
                Sb.Append("<select class='form-control' name='ddlquestion'>");
                foreach (QuestionDropdownSetting Item in ObjList)
                {
                    if (ObjSurveyResult.Answer == Item.Value)
                    {
                        Sb.Append("<option selected='selected'>" + Item.Value + "</option>");
                    }
                    else
                    {
                        Sb.Append("<option>" + Item.Value + "</option>");
                    }

                }
                Sb.Append("</select><div class='clearfix'></div></div>");
                return new MvcHtmlString(Sb.ToString());
            }
            else
            {
                StringBuilder Sb = new StringBuilder();
                Sb.Append("<div class='form-group'>");
                Sb.Append("<select class='form-control'  name='ddlquestion'>");
                foreach (QuestionDropdownSetting Item in ObjList)
                {
                    Sb.Append("<option>" + Item.Value + "</option>");
                }
                Sb.Append("</select><div class='clearfix'></div></div>");
                return new MvcHtmlString(Sb.ToString());
            }






        }



        public static MvcHtmlString TakeGridTypeQuestion(this HtmlHelper helper, List<SurveyResult> ObjSurveyResult, ICollection<QuestionGridSettingHeader> ObjList,
                                            object htmlAttributes = null)
        {
            // int Count = 1;
            StringBuilder Sb = new StringBuilder();

            if (ObjList.Count > 0)
            {
                var RowList = ObjList.Where(x => x.ColoumnHeaderValue == null);
                var ColList = ObjList.Where(x => x.RowHeaderValue == null);
                Sb.Append("<table class='table' data-table='true' >");
                if (ColList.Count() > 0)
                {
                    Sb.Append("<tr><th  width='15%'></th>");
                    foreach (QuestionGridSettingHeader Item in ColList)
                    {
                        Sb.Append("<th class='text-center'>" + Item.ColoumnHeaderValue + "</th>");
                    }
                    Sb.Append("</tr>");
                }
                if (RowList.Count() > 0)
                {
                    int Row = 0;
                    int Col = 0;
                    foreach (QuestionGridSettingHeader Item in RowList)
                    {
                        Sb.Append("<tr data-tr='true' ><th valign='middle'>" + Item.RowHeaderValue + "</th>");
                        Col = 0;
                        foreach (var Itemchild in ColList)
                        {
                            SurveyResult Result = ObjSurveyResult.Where(x => x.RowNumber == Row && x.ColumnNumber == Col).FirstOrDefault();
                            if (Itemchild.ControlType == "1") //text box type
                            {
                                if (Result != null)
                                {
                                    Sb.Append("<td data-td='true' class='text-center'><input class='form-control' value='" + Result.Answer + "' type='text'></td>");
                                }
                                else
                                {
                                    Sb.Append("<td data-td='true' class='text-center'><input class='form-control' type='text'></td>");
                                }

                            }
                            else   //drop down type
                            {
                                Sb.Append("<td  data-td='true' class='text-center'>");
                                Sb.Append("<select class='form-control'>");
                                string[] DropDownValue = Itemchild.DropdownTypeOptionValue.Split(',');

                                if (Result != null)
                                {
                                    foreach (var ItemOption in DropDownValue)
                                    {
                                        if (Result.Answer == ItemOption)
                                        {
                                            Sb.Append("<option selected='selected'>" + ItemOption + "</option>");
                                        }
                                        else
                                        {
                                            Sb.Append("<option>" + ItemOption + "</option>");
                                        }

                                    }
                                }
                                else
                                {
                                    foreach (var ItemOption in DropDownValue)
                                    {
                                        Sb.Append("<option>" + ItemOption + "</option>");
                                    }
                                }

                                Sb.Append("</select>");
                                Sb.Append("</td>");
                            }
                            Col = Col + 1;
                        }
                        Sb.Append("</tr>");
                        Row = Row + 1;
                    }

                }
                Sb.Append("</table>");
            }
            return new MvcHtmlString(Sb.ToString());
        }




        public static MvcHtmlString AdminReportTextBoxTypeQuestion(this HtmlHelper helper, ICollection<QuestionTextBoxSetting> ObjList, int QuestionOrder, Int64 ProjectId,
                                                string Value,
                                                object htmlAttributes = null)
        {
            StringBuilder Sb = new StringBuilder();

            switch (ObjList.FirstOrDefault().TextBoxTypeId)
            {
                case 1:
                    Sb.Append("<input data-check='true' name='txt_" + QuestionOrder + ProjectId + "' class='form-control' style='width:440px;' type='text' value='" + Value + "' >");
                    Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + ProjectId + "'></span>");
                    break;
                case 2:
                    Sb.Append("<input data-check='true' onkeyup='numericFilter(this)' name='txt_" + QuestionOrder + ProjectId + "' class='form-control' type='text' value='" + Value + "' >");
                    Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + ProjectId + "'></span>");
                    break;
                case 3:
                    Sb.Append("<input data-check='true' name='txt_" + QuestionOrder + ProjectId + "' class='form-control' type='email'value='" + Value + "' >");
                    Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + ProjectId + "'></span>");
                    break;
                case 4:
                    Sb.Append("<input data-check='true' data-phonenumber='true' name='txt_" + QuestionOrder + ProjectId + "' class='form-control' type='text' value='" + Value + "' >");
                    Sb.Append("<span class='k-invalid-msg' data-for='txt_" + QuestionOrder + ProjectId + "'></span>");
                    break;
            }

            Sb.Append("<input type='hidden' name='HdnTextBoxTypeId' value='" + ObjList.FirstOrDefault().TextBoxTypeId + "' />");
            switch (ObjList.FirstOrDefault().TextBoxTypeId)
            {
                case 1: //Text
                    Sb.Append("<input type='hidden' name='HdnAllowOnly' value='" + ObjList.FirstOrDefault().IsAlphabets + "," + ObjList.FirstOrDefault().IsNumber + "," + ObjList.FirstOrDefault().IsSpecialCharecter + "' />");
                    break;
                case 2: //Number
                    Sb.Append("<input type='hidden' name='HdnIsNumberLimit' value='" + ObjList.FirstOrDefault().LowerLimit + "," + ObjList.FirstOrDefault().UpperLimit + "' />");
                    break;
            }

            return new MvcHtmlString(Sb.ToString());
        }


        public static MvcHtmlString AdminReportDropListTypeQuestion(this HtmlHelper helper, ICollection<QuestionDropdownSetting> ObjList, string Value,
                                              object htmlAttributes = null)
        {
            StringBuilder Sb = new StringBuilder();

            Sb.Append("<select class='form-control'  name='ddlquestion'>");

            foreach (QuestionDropdownSetting Item in ObjList)
            {
                if (Value != null && Item.Value.Trim().ToLower() == Value.Trim().ToLower())
                {
                    Sb.Append("<option selected='selected'>" + Item.Value + "</option>");
                }
                else
                {
                    Sb.Append("<option>" + Item.Value + "</option>");
                }




            }

            Sb.Append("</select>");
            return new MvcHtmlString(Sb.ToString());
        }



        public static MvcHtmlString AdminReportGridTypeQuestion(this HtmlHelper helper, ICollection<QuestionGridSettingHeader> ObjList, List<BuilderReportSubmitViewModel> AdminReportResult,
                                            object htmlAttributes = null)
        {
            // int Count = 1;
            StringBuilder Sb = new StringBuilder();

            if (ObjList.Count > 0)
            {
                var RowList = ObjList.Where(x => x.ColoumnHeaderValue == null);
                var ColList = ObjList.Where(x => x.RowHeaderValue == null);
                Sb.Append("<table class='table' data-table='true' >");
                if (ColList.Count() > 0)
                {
                    Sb.Append("<tr><th  width='15%'></th>");
                    foreach (QuestionGridSettingHeader Item in ColList)
                    {
                        Sb.Append("<th class='text-center'>" + Item.ColoumnHeaderValue + "</th>");
                    }
                    Sb.Append("</tr>");
                }
                if (RowList.Count() > 0)
                {
                    int Row = 0;
                    int Col = 0;
                    foreach (QuestionGridSettingHeader Item in RowList)
                    {
                        Sb.Append("<tr data-tr='true' ><th class='text-left'>" + Item.RowHeaderValue + "</th>");
                        Col = 0;
                        foreach (var Itemchild in ColList)
                        {
                            BuilderReportSubmitViewModel Result = AdminReportResult.Where(x => x.RowNumber == Row && x.ColumnNumber == Col).FirstOrDefault();
                            string Value = Result != null ? Result.Answer : "";
                            if (Itemchild.ControlType == "1") //text box type
                            {
                                Sb.Append("<td data-td='true' class='text-center'><input class='form-control' type='text' value='" + Value + "'></td>");
                            }
                            else   //drop down type
                            {
                                Sb.Append("<td  data-td='true' class='text-center'>");
                                Sb.Append("<select class='form-control'>");
                                string[] DropDownValue = Itemchild.DropdownTypeOptionValue.Split(',');
                                foreach (var ItemOption in DropDownValue)
                                {
                                    if (ItemOption.Trim() == Value.Trim())
                                    {
                                        Sb.Append("<option selected='selected'>" + ItemOption + "</option>");
                                    }
                                    else
                                    {
                                        Sb.Append("<option>" + ItemOption + "</option>");
                                    }

                                }
                                Sb.Append("</select>");
                                Sb.Append("</td>");
                            }
                            Col = Col + 1;
                        }
                        Sb.Append("</tr>");
                        Row = Row + 1;
                    }
                }
                Sb.Append("</table>");
            }
            return new MvcHtmlString(Sb.ToString());
        }

        public static IHtmlString reCaptcha(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            string publickey = WebConfigurationManager.AppSettings["RecaptchaPublicKey"];
            sb.AppendLine("<script type=\"text/javascript\"src='https://www.google.com/recaptcha/api.js'></script>");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"g-recaptcha\" data-sitekey=\"" + publickey + "\"></div>");
            return MvcHtmlString.Create(sb.ToString());
        }

    }
}