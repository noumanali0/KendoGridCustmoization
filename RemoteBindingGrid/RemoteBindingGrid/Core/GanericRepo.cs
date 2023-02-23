using Kendo.Mvc.Extensions;
using RemoteBindingGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KendoUIWebApp.Core
{
    
    public class GanericRepo : IGaneric
    {
        StudentCrudDbContext _context;
        public GanericRepo() {
            _context = new StudentCrudDbContext();
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public void Delete(Student student)
        {
            var exists = _context.Students.Where(item => item.StudentID == student.StudentID).FirstOrDefault();
            if(exists != null)
            {
                _context.Students.Remove(exists);
                _context.SaveChanges();
            }
            
        }

        public List<Student> Get()
        {
            return _context.Students.ToList();
        }

        public void Update(Student student)
        {
            var exists = _context.Students.Where(item => item.StudentID == student.StudentID).FirstOrDefault();
            if (exists != null)
            {
                exists.StudentName = student.StudentName;
                exists.StudentCity = student.StudentCity;
                _context.SaveChanges();
            }
        }
    }
}