using Common;
using System.Collections.Generic;
using System.Web.Mvc;
using TelerikMovies.Web.Areas.Admin.Models.Contracts;

namespace TelerikMovies.Web.Areas.Admin.Controllers
{ 
    public abstract class DataTableController:Controller
    {
        protected virtual void FillDataTable<T>(IDataTableViewModel<T> dataTableData,List<T> allData, int draw, int totalCount, int start = 0, int length = 10 )
        {
            string search = Request.Params["search[value]"];
            int sortColumn = -1;
            string sortDirection = "asc";

            // note: we only sort one column at a time
            if (Request.Params["order[0][column]"] != null)
            {
                sortColumn = int.Parse(Request.Params["order[0][column]"]);
            }
            if (Request.Params["order[0][dir]"] != null)
            {
                sortDirection = Request.Params["order[0][dir]"];
            }

            dataTableData.draw = draw;
            dataTableData.recordsTotal = totalCount;
            int recordsFiltered = 0;
            dataTableData.data = DataTableOrderer.FilterData<T>(ref recordsFiltered, allData, start, length, search, sortColumn, sortDirection);
            dataTableData.recordsFiltered = recordsFiltered;
        }

       

       
    }
}