using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMiner
{
    public class Course
    {
        public string CourseName { get; set; }
        public string CourseNumber { get; set; }
        public string CatalogDescription { get; set; }
        public string CreditHours { get; set; }
        public Offering OfferingFlags { get; set; }
        public int LastTaughtId { get; set; }
        public string Prerequisite { get; set; }
    }
}
