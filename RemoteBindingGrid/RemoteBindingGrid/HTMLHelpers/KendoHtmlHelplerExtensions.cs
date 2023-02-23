using Kendo.Mvc.UI;
using Kendo.Mvc.UI.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace RemoteBindingGrid.HTMLHelpers
{
    public static class KendoHtmlHelplerExtensions
    {
        public static string grid_NoRecords = "Grid_NoRecords";

        private static GridBuilder<T> PhoenixGridBase<T>(this GridBuilder<T> helper,
                string name, UrlHelper urlHelper, bool toolbar = true, bool restrictedExport = false) where T : class
        {
            var hlp = helper.Name(name).Filterable(filter => filter.Enabled(true).Mode(GridFilterMode.Row))
                .Sortable(sort => sort.Enabled(true))
                .Pageable(page => page.Enabled(true)
                    .Input(true)
                    .Refresh(true)
                    .PreviousNext(true)
                    .PageSizes(new[] { 5, 10, 20, 50, 100 })
                    .Info(true)
                    .Numeric(true))
                .Groupable(g => g.Enabled(true))
                .Selectable(sel => sel.Enabled(true).Mode(GridSelectionMode.Single).Type(GridSelectionType.Row))
                .Resizable(r => r.Columns(true))
                .ColumnMenu(cm => cm.Enabled(true).Sortable(true).Filterable(true).Columns(true))
                .Navigatable(n => n.Enabled(true))
                .Reorderable(r => r.Columns(true))
                .Events(e =>
                {
                    e.ExcelExport("SR.KendoGridEvents.Instance.ExcelExport");
                    e.PdfExport("SR.KendoGridEvents.Instance.PDFExport");
                })
                .Scrollable(s => s.Enabled(true).Height("50vh"))
                .Pdf(e => e.AllPages().Landscape().ProxyURL(urlHelper.ResolveActionFromHtmlHelper("Export"))
                    .ForceProxy(true))
                .Excel(e => e.AllPages(true).Filterable(true).ProxyURL(urlHelper.ResolveActionFromHtmlHelper("Export"))
                    .ForceProxy(true))
                .NoRecords(grid_NoRecords);

            if (toolbar)
            {
                hlp = hlp.ToolBar(tb =>
                {
                    tb.CustomExcelExport(restrictedExport);
                    tb.CustomPdfExport(restrictedExport);
                });
            }

            return hlp;
        }
        private static GridBuilder<T> PhoenixGridBase<T>(this HtmlHelper helper,
            IEnumerable<T> dataSource, string name, bool toolbar = true, bool restrictedExport = false) where T : class
        {
            return helper.Kendo().Grid(dataSource)
                .PhoenixGridBase(name, helper.ResolveUrlHelperFromHtmlHelper(), toolbar, restrictedExport);
        }


        public static GridBuilder<T> PhoenixLocalGrid<T>(this HtmlHelper helper, IEnumerable<T> dataSource, string name, bool toolbar = true) where T : class
        {
            return PhoenixGridBase(helper, dataSource, name, toolbar);
        }


        private static string ResolveActionFromHtmlHelper(this UrlHelper helper, string action)
        {
            return helper.Action(action, helper.RequestContext.RouteData.Values["controller"]);
        }

        private static UrlHelper ResolveUrlHelperFromHtmlHelper(this HtmlHelper helper)
        {
            return new UrlHelper(helper.ViewContext.RequestContext);
        }


        public static GridToolBarCustomCommandBuilder<T> CustomExcelExport<T>(this GridToolBarCommandFactory<T> tb, bool restrictedExport) where T : class
        {
            return tb.Custom().Text(" ").Name("customExcelExport").IconClass("fas fa-file-excel fa-2x no-margin")
                .HtmlAttributes(new
                {
                    title = "Excel",
                    onclick = "SR.KendoGridEvents.Instance.onCustomExcelExportClick(this," +
                              restrictedExport.ToString().ToLower() + "); return false;",
                    @class = "sr-toolbarbutton"
                });
        }
        public static GridToolBarCustomCommandBuilder<T> CustomPdfExport<T>(this GridToolBarCommandFactory<T> tb, bool restrictedExport) where T : class
        {
            return tb.Custom().Text(" ").Name("customPdfExport").IconClass("fas fa-file-pdf fa-2x no-margin")
                .HtmlAttributes(new
                {
                    title = "PDF",
                    onclick = "SR.KendoGridEvents.Instance.onCustomPdfExportClick(this," +
                              restrictedExport.ToString().ToLower() + "); return false;",
                    @class = "sr-toolbarbutton"
                });
        }


        public static GridColumnFactory<T> ApplyDefaultColumnFilterOperators<T>(this GridColumnFactory<T> helper, string[] excludedColumnNames = null) where T : class
        {
            if (excludedColumnNames == null)
            {
                excludedColumnNames = new string[0];
            }
            var allCols = helper.ColumnsContainer.Columns.OfType<IGridBoundColumn>().Where(x => !excludedColumnNames.Contains(x.Member)).ToList();
            foreach (var colItem in allCols)
            {
                if (colItem.MemberType == typeof(string))
                {
                    // Now see if the operator exists.
                    var opExists = colItem.FilterableSettings.Operators.Strings.Operators.ContainsKey("contains");

                    // Operator is a string so it must exist before we attempt to set it.
                    if (opExists)
                    {
                        colItem.FilterableSettings.CellSettings.Operator = "contains";
                    }
                }
                else if (colItem.MemberType == typeof(DateTime) || colItem.MemberType == typeof(DateTime?))
                {
                    // We only enforce this if it is not already set.
                    if (string.IsNullOrWhiteSpace(colItem.Format))
                    {
                        colItem.Format = "{0:dd/MM/yyyy HH:mm:ss}";
                    }

                    // We only enforce this if it is not already set.
                    if (!string.IsNullOrWhiteSpace(colItem.FilterableSettings.CellSettings.Template.HandlerName))
                        continue;

                    colItem.FilterableSettings.CellSettings.Template.HandlerName = "SR.KendoGridEvents.Instance.getDateTimeFilter";
                    colItem.FilterableSettings.CellSettings.Operator = "gte";
                }
            }

            return helper;
        }
    }
}