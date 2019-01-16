using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CBUSA.Models
{
    public class GridRequestParameters
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string FilterLogic { get; set; }
        public IEnumerable<KendoGroupInfo> Groups { get; set; }
        public IEnumerable<SortingInfo> Sortings { get; set; }
        public IEnumerable<KendoFilterInfo> Filters { get; set; }

        public static GridRequestParameters Current
        {
            get
            {
                var p = new GridRequestParameters();
                p.Populate();
                return p;
            }
        }

        //TODO: pull default values from config
        internal void Populate()
        {
            if (HttpContext.Current != null)
            {
                HttpRequest curRequest = HttpContext.Current.Request;
                this.Page = curRequest["page"] != null ? Convert.ToInt16(curRequest["page"]) : 0;
                this.PageSize = curRequest["pageSize"] != null ? Convert.ToInt16(curRequest["pageSize"]) : 0;
                this.Skip = curRequest["skip"] != null ? Convert.ToInt16(curRequest["skip"]) : 0;
                this.Take = curRequest["take"] != null ? Convert.ToInt16(curRequest["take"]) : 0;
                this.FilterLogic = curRequest["filter[logic]"] != null ? curRequest["filter[logic]"].ToUpper() : "";
                var groups = new List<KendoGroupInfo>();
                var g = 0;
                while (g < 20)
                {
                    var GroupField = curRequest["group[" + g + "][field]"];
                    if (GroupField != null)
                    {
                        groups.Add(new KendoGroupInfo { GroupBy = GroupField });
                    }
                    else
                    {
                        break;
                    }
                    g++;
                }
                Groups = groups;
                //build sorting objects
                var sorts = new List<SortingInfo>();
                var x = 0;
                while (x < 20)
                {
                    var sortDirection = curRequest["sort[" + x + "][dir]"];
                    if (sortDirection == null)
                    {
                        break;
                    }
                    var sortOn = curRequest["sort[" + x + "][field]"];
                    if (sortOn != null)
                    {
                        sorts.Add(new SortingInfo { SortOn = sortOn, SortOrder = sortDirection });
                    }
                    x++;
                }
                Sortings = sorts;

                //build filter objects
                var filters = new List<KendoFilterInfo>();
                x = 0;
                var i = 0;
                if (!string.IsNullOrWhiteSpace(this.FilterLogic))
                {
                    while (x < 20)
                    {
                        var curTest = curRequest["filter[filters][" + x + "][logic]"] != null ? curRequest["filter[filters][" + x + "][logic]"].ToUpper() : "";
                        if (!string.IsNullOrWhiteSpace(curTest))
                        {
                            while (i < 20)
                            {
                                var field = curRequest["filter[filters][" + x + "][filters][" + i + "][field]"];
                                if (field == null)
                                {
                                    break;
                                }

                                var val = curRequest["filter[filters][" + x + "][filters][" + i + "][value]"] ?? string.Empty;

                                var strop = curRequest["filter[filters][" + x + "][filters][" + i + "][operator]"];
                                if (strop != null)
                                {
                                    filters.Add(new KendoFilterInfo
                                    {
                                        Operator = KendoFilterInfo.ParseOperator(strop),
                                        Field = field,
                                        Value = val
                                    });
                                }
                                i++;
                            }
                        }
                        else
                        {
                            var field = curRequest["filter[filters][" + x + "][field]"];
                            if (field == null)
                            {
                                break;
                            }

                            var val = curRequest["filter[filters][" + x + "][value]"] ?? string.Empty;

                            var strop = curRequest["filter[filters][" + x + "][operator]"];
                            if (strop != null)
                            {
                                filters.Add(new KendoFilterInfo
                                {
                                    Operator = KendoFilterInfo.ParseOperator(strop),
                                    Field = field,
                                    Value = val
                                });
                            }
                        }

                        x++;
                    }
                }
                //Filters = filters.Select(y=> new KendoFilterInfo {Field= y.Field, Operator= y.Operator, Value= y.Value }).Distinct().Select(z => new KendoFilterInfo
                //{
                //    Field = z.Field,
                //    Operator = z.Operator,
                //    Value = z.Value
                //});
                Filters = filters.GroupBy(y => new { y.Field, y.Operator, y.Value }, (key, group) => new KendoFilterInfo { Field = key.Field, Operator = key.Operator, Value = key.Value });
            }
        }
        public void KendoGridFilterGeneric(KendoFilterInfo Filter, string ColumnName, ref IEnumerable<dynamic> VarList, string FilterDataType, Boolean IsDropDownList = false)
        {

            switch (FilterDataType)
            {
                case "string":
                    var FilterableValue = Convert.ToString(Filter.Value).Trim();
                    // if(FilterableValue!="A")
                    switch (Filter.Operator.ToString())
                    {
                        case "Contains":
                            if (IsDropDownList == false)
                            {
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Contains(FilterableValue.ToLower().Trim()));
                            }
                            else
                            {
                                var x = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c));
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Contains(FilterableValue.ToLower()));
                            }
                            break;
                        case "NotContains":
                            //VarList = VarList.Where(c => !c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Contains(Filter.Value.ToLower()));
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim() != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim().Contains(FilterableValue.ToLower()));

                            break;
                        case "Equals":
                            // VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Equals(Filter.Value.ToLower()));
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim() != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim().Equals(FilterableValue.ToLower()));
                            break;
                        case "NotEquals":
                            // VarList = VarList.Where(c => !c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Equals(Filter.Value.ToLower()));
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim() != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim().Equals(FilterableValue.ToLower()));
                            break;
                        case "StartsWith":
                            var a = VarList;
                            //VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().StartsWith(Filter.Value.ToLower()));
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim() != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim().StartsWith(FilterableValue.ToLower()));
                            break;
                        case "EndsWith":
                            // VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).Trim().ToLower().EndsWith(Filter.Value.ToLower()));
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim() != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToLower().Trim().EndsWith(FilterableValue.ToLower()));
                            break;
                            //case "Greater":
                            //     VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) > FilterableValue.ToLower());
                            //   // VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim() != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Equals(Filter.Value.ToLower()));
                            //    break;
                            //case "GreaterOrEquals":
                            //     VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) >= FilterableValue.ToLower());
                            //   // VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim() != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Equals(Filter.Value.ToLower()));
                            //    break;
                            //case "LessThan":
                            //   VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) < FilterableValue.ToLower());
                            //   // VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim() != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Equals(Filter.Value.ToLower()));
                            //    break;
                            //case "LessThanOrEquals":
                            //   VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) <= FilterableValue.ToLower());
                            //    //VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim() != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToLower().Trim().Equals(Filter.Value.ToLower()));
                            //    break;
                    }
                    break;
                case "number":
                    //  if(Filter.Value)
                    decimal FilterNumberValue = Convert.ToDecimal(Filter.Value.Trim());
                    switch (Filter.Operator.ToString())
                    {
                        case "Contains":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()).Contains(FilterNumberValue));
                            break;
                        case "NotContains":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && !Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()).Contains(FilterNumberValue));
                            break;
                        case "Equals":

                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()) == (FilterNumberValue));
                            break;
                        case "NotEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()) != FilterNumberValue);
                            break;
                        case "StartsWith":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).StartsWith(FilterNumberValue));
                            break;
                        case "EndsWith":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).EndsWith(FilterNumberValue));
                            break;
                        case "Greater":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()) > FilterNumberValue);
                            break;
                        case "GreaterOrEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()) >= FilterNumberValue);
                            break;
                        case "LessThan":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()) < FilterNumberValue);
                            break;
                        case "LessThanOrEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Length > 0 && Convert.ToDecimal(c.GetType().GetProperty(ColumnName).GetValue(c).ToString()) <= FilterNumberValue);
                            break;
                    }
                    break;
                case "double":
                    //  if(Filter.Value)
                    double FilterDoubleValue = Convert.ToDouble(Filter.Value.Trim());
                    switch (Filter.Operator.ToString())
                    {
                        case "Contains":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Contains(FilterDoubleValue.ToString()));
                            break;
                        case "NotContains":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToString().Contains(FilterDoubleValue.ToString()));
                            break;
                        case "Equals":

                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) == FilterDoubleValue);
                            break;
                        case "NotEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) != FilterDoubleValue);
                            break;
                        case "StartsWith":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().StartsWith(FilterDoubleValue.ToString()));
                            break;
                        case "EndsWith":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().EndsWith(FilterDoubleValue.ToString()));
                            break;
                        case "Greater":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) > FilterDoubleValue);
                            break;
                        case "GreaterOrEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) >= FilterDoubleValue);
                            break;
                        case "LessThan":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) < FilterDoubleValue);
                            break;
                        case "LessThanOrEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c) <= FilterDoubleValue);
                            break;
                    }
                    break;
                case "integer":
                    //  if(Filter.Value)
                    int FilterIntegerValue = Convert.ToInt32(Filter.Value.Trim());
                    switch (Filter.Operator.ToString())
                    {
                        case "Contains":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).Contains(FilterIntegerValue));
                            break;
                        case "NotContains":
                            VarList = VarList.Where(c => !c.GetType().GetProperty(ColumnName).GetValue(c).Contains(FilterIntegerValue));
                            break;
                        case "Equals":

                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) == FilterIntegerValue);
                            break;
                        case "NotEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != FilterIntegerValue);
                            break;
                        case "StartsWith":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).StartsWith(FilterIntegerValue));
                            break;
                        case "EndsWith":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c).EndsWith(FilterIntegerValue));
                            break;
                        case "Greater":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) > FilterIntegerValue);
                            break;
                        case "GreaterOrEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) >= FilterIntegerValue);
                            break;
                        case "LessThan":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) < FilterIntegerValue);
                            break;
                        case "LessThanOrEquals":
                            VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) <= FilterIntegerValue);
                            break;
                    }
                    break;
                case "date":
                    if (!string.IsNullOrWhiteSpace(Filter.Value.Trim()))
                    {
                        string FilterValue;
                        if (Filter.Value.Contains("GMT"))
                        {
                            FilterValue = Filter.Value.Substring(0, Filter.Value.Length - 30);
                        }
                        else
                        {
                            FilterValue = Filter.Value;
                        }
                        // FilterValue = Filter.Value;
                        switch (Filter.Operator.ToString())
                        {
                            case "Contains":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date.Contains(Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "NotContains":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && !Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date.Contains(Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "Equals":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date.Equals(Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "NotEquals":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && !Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date.Equals(Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "StartsWith":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date().StartsWith(Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "EndsWith":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date.EndsWith(Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "Greater":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date > (Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "GreaterOrEquals":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date >= (Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "LessThan":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date < (Convert.ToDateTime(FilterValue).Date));
                                break;
                            case "LessThanOrEquals":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && Convert.ToDateTime(c.GetType().GetProperty(ColumnName).GetValue(c)).Date <= (Convert.ToDateTime(FilterValue).Date));
                                break;
                        }
                    }
                    break;
                case "boolean":
                    if (!string.IsNullOrWhiteSpace(Filter.Value))
                    {
                        Boolean FilterBooleanValue = Filter.Value == "true" ? true : false;
                        switch (Filter.Operator.ToString())
                        {

                            case "Equals":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToUpper().Equals(FilterBooleanValue.ToString().ToUpper()));
                                break;
                            case "NotEquals":
                                VarList = VarList.Where(c => c.GetType().GetProperty(ColumnName).GetValue(c) != null && !c.GetType().GetProperty(ColumnName).GetValue(c).ToString().ToUpper().Equals(FilterBooleanValue.ToString().ToUpper()));
                                break;

                        }
                    }
                    break;
            }
        }
        public void KendoGridSortGeneric(string SortBy, string ColumnName, ref IEnumerable<dynamic> VarList, bool ConvertToNumber = false)
        {
            if (SortBy == "asc")
            {
                if (ConvertToNumber)
                {
                    VarList = VarList.OrderBy(c => (c.GetType().GetProperty(ColumnName).GetValue(c)).Length).ThenBy(c => (c.GetType().GetProperty(ColumnName).GetValue(c)));
                }
                else
                {
                    VarList = VarList.OrderBy(c => c.GetType().GetProperty(ColumnName).GetValue(c));
                }
            }
            else
            {
                if (ConvertToNumber)
                {
                    VarList = VarList.OrderByDescending(c => (c.GetType().GetProperty(ColumnName).GetValue(c)).Length).ThenByDescending(c => (c.GetType().GetProperty(ColumnName).GetValue(c)));
                }
                else
                {
                    VarList = VarList.OrderByDescending(c => c.GetType().GetProperty(ColumnName).GetValue(c));
                }
            }
        }
        public static IEnumerable<T> MultipleSort<T>(IEnumerable<T> data, List<SortingInfo> GridSort)
        {
            var sortExpressions = new List<Tuple<string,
            string>>();
            for (int j = 0; j < GridSort.Count(); j++)
            {
                var fieldName = GridSort[j].SortOn.Trim();
                var sortOrder = GridSort[j].SortOrder.Trim().ToLower();
                sortExpressions.Add(new Tuple<string, string>(fieldName, sortOrder));
            }
            // No sorting needed
            if ((sortExpressions == null) || (sortExpressions.Count() <= 0))
            {
                return data;
            }

            // Let us sort it
            IEnumerable<T> query = from item in data select item;
            IOrderedEnumerable<T> orderedQuery = null;
            int i = 0;
            foreach (var SortItem in sortExpressions)
            {
                // We need to keep the loop index, not sure why it is altered by the Linq.
                var index = i;
                Func<T, object> expression = item => item.GetType()
                                .GetProperty(SortItem.Item1)
                                .GetValue(item, null);

                if (SortItem.Item2 == "asc")
                {
                    orderedQuery = (index == 0) ? query.OrderBy(expression)
                      : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = (index == 0) ? query.OrderByDescending(expression)
                             : orderedQuery.ThenByDescending(expression);
                }
                i++;
            }

            query = orderedQuery;

            return query;
        }


    }

    public class KendoFilterInfo
    {
        public string Field { get; set; }
        public FilterOperations Operator { get; set; }
        public string Value { get; set; }

        public static FilterOperations ParseOperator(string theOperator)
        {
            switch (theOperator)
            {
                //equal ==
                case "eq":
                case "==":
                case "isequalto":
                case "equals":
                case "equalto":
                case "equal":
                    return FilterOperations.Equals;
                //not equal !=
                case "neq":
                case "!=":
                case "isnotequalto":
                case "notequals":
                case "notequalto":
                case "notequal":
                case "ne":
                    return FilterOperations.NotEquals;
                // Greater
                case "gt":
                case ">":
                case "isgreaterthan":
                case "greaterthan":
                case "greater":
                    return FilterOperations.Greater;
                // Greater or equal
                case "gte":
                case ">=":
                case "isgreaterthanorequalto":
                case "greaterthanequal":
                case "ge":
                    return FilterOperations.GreaterOrEquals;
                // Less
                case "lt":
                case "<":
                case "islessthan":
                case "lessthan":
                case "less":
                    return FilterOperations.LessThan;
                // Less or equal
                case "lte":
                case "<=":
                case "islessthanorequalto":
                case "lessthanequal":
                case "le":
                    return FilterOperations.LessThanOrEquals;
                case "startswith":
                    return FilterOperations.StartsWith;

                case "endswith":
                    return FilterOperations.EndsWith;
                //string.Contains()
                case "contains":
                    return FilterOperations.Contains;
                case "doesnotcontain":
                    return FilterOperations.NotContains;
                default:
                    return FilterOperations.Contains;
            }
        }
    }
    public class KendoGroupInfo
    {
        public string GroupBy { get; set; }
    }
    public class SortingInfo
    {
        public string SortOrder { get; set; }
        public string SortOn { get; set; }
    }

    public enum FilterOperations
    {
        Equals,
        NotEquals,
        Greater,
        GreaterOrEquals,
        LessThan,
        LessThanOrEquals,
        StartsWith,
        EndsWith,
        Contains,
        NotContains,
    }
}