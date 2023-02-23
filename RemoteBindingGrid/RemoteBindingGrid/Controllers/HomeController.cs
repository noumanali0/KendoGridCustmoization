using DocumentFormat.OpenXml.Drawing;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using KendoUIWebApp.Core;
using Newtonsoft.Json.Linq;
using RemoteBindingGrid.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Xml;

namespace RemoteBindingGrid.Controllers
{
    public class HomeController : Controller
    {
        GanericRepo repo = new GanericRepo();
        StudentCrudDbContext _context;

        public HomeController()
        {
            _context= new StudentCrudDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Read([DataSourceRequest] DataSourceRequest request)
        //{
        //    return Json(repo.Get().ToDataSourceResult(request));
        //}



        public ActionResult GetData([DataSourceRequest] DataSourceRequest request)
        {    
            // Build the initial queryable   
             var query = _context.Students.AsQueryable();     
            // Extract all distinct keys in the JSON column   
             var allKeys = query.SelectMany(e => JObject.Parse(e.StudentName).Properties()).Select(p => p.Name).Distinct();     
            // Add a column for each key to the grid   
             var columns = allKeys.Select(key => new GridColumn{Field = $"JsonColumn['{key}']",Title = key });     
            // Build the dynamic filtering expression   
             var filterExpression = request.Filters.ToExpression<XmlEntity>();     
            // Apply sorting based on the client-side request   
             var sortedColumns = request.Sorts.Select(s => new GridColumn { Field = s.Member }).ToList();   
             //var sortExpression = sortedColumns.ToExpression<_context.Students>();  \
            
            var sortExpression = sortedColumns.ToList().ToExpression<Student>();
            // Apply the filtering and sorting expressions to the queryable   
            query = query.Where(filterExpression).OrderBy(sortExpression);     
            // Apply server-side paging   
             query = query.Skip(request.Skip).Take(request.Take);     
            // Convert the JSON strings to dynamic objects   
             var filteredData = query.ToList().Select(e => new {Id = e.Id,JsonColumn = JObject.Parse(e.JsonColumn)    });     
            // Return the filtered and sorted data as a JsonResult   
             return Json(filteredData.ToDataSourceResult(request));
         }


            public void Called()
        {
            Debug.WriteLine("Called");
        }



        //public JsonResult Students_Read()
        //{
        //    var result = Enumerable.Range(0, 5).Select(i => new Student
        //    {
        //        StudentID = i,
        //        StudentName = "Student " + i
        //    });

        //    return Json(result);
        //}


        public JsonResult Students_Read()
        {

            var students = repo.Get().AsQueryable();

            return Json(students.Select(p => new { StudentID = p.StudentID, StudentName = p.StudentName, StudentCity = p.StudentCity }), JsonRequestBehavior.AllowGet);
        }


        /////////////Single Record CRUD

        //[AcceptVerbs("Post")]
        //public ActionResult Create([DataSourceRequest] DataSourceRequest request, Student std)
        //{
        //    repo.Add(std);

        //    //return Json(repo.Get().ToDataSourceResult(request, ModelState));
        //    return Json(new[] { std }.ToDataSourceResult(request, ModelState));
        //}


        //[AcceptVerbs("Post")]
        //public ActionResult Update([DataSourceRequest] DataSourceRequest request, Student std)
        //{
        //    repo.Update(std);
        //    return Json(new[] { std }.ToDataSourceResult(request, ModelState));
        //}


        //public ActionResult Delete([DataSourceRequest] DataSourceRequest request, Student student)
        //{
        //    repo.Delete(student);
        //    return Json(new[] { student }.ToDataSourceResult(request,ModelState));
        //}




        ////////////////// Batch CRUD


        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Student> stds)
        //{
        //    var results = new List<Student>();

        //    if (stds != null && ModelState.IsValid)
        //    {
        //        foreach (var std in stds)
        //        {
        //            repo.Add(std);
        //            results.Add(std);
        //        }
        //    }

        //    return Json(results.ToDataSourceResult(request, ModelState));
        //}


        [HttpGet]
        public ActionResult Create()
        {

            return View("_RadDrawerPartialView");
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Student> stds)
        {
            if (stds != null && ModelState.IsValid)
            {
                foreach (var std in stds)
                {
                    repo.Update(std);
                }
            }

            return Json(stds.ToDataSourceResult(request, ModelState));
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<Student> stds)
        {
            if (stds.Any())
            {
                foreach (var std in stds)
                {
                    repo.Delete(std);
                }
            }

            return Json(stds.ToDataSourceResult(request, ModelState));
        }



        public ActionResult RadDrawer()
        {
            return View("_RadDrawerPartialView");
        }


       
    }
}