using RemoteBindingGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KendoUIWebApp.Core
{
    internal interface IGaneric
    {
        void Add(Student student);

        void Update(Student student);

        void Delete(Student student);

        List<Student> Get();
    }
}
