using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MauiMiner
{
    public static class Miner
    {
        public static List<Course> MineCourses(string subject)
        {
            List<JToken> courseTokens = MauiAPI.GetCourse(subject);
            List<Course> courses = new List<Course>();
            Dictionary<string, int> offeringFlags = new Dictionary<string, int>();

            int preqCount = 0;
            int noPreqCount = 0;
            int numErrors = 0;
            foreach (JToken tok in courseTokens)
            {
                int lastTaughtId = 0;
                if(tok["lastTaught"] != null)
                {
                    lastTaughtId = (int)tok["lastTaughtId"];
                }
                string courseNumber = (string)tok["courseNumber"];
                int poundIndex = courseNumber.IndexOf('#');
                if (poundIndex != -1)
                {
                    string old = courseNumber;
                    courseNumber = courseNumber.Substring(0, poundIndex) + courseNumber.Substring(poundIndex + 1);
                    //Console.WriteLine("Changed bad course number {0} -> {1}", old, courseNumber);
                }

                if(!offeringFlags.ContainsKey(courseNumber))
                {
                    offeringFlags.Add(courseNumber, lastTaughtId);
                    Course c = new Course()
                    {
                        CourseName = (string)tok["title"],
                        CourseNumber = courseNumber,
                        CreditHours = (string)tok["creditHours"],
                        CatalogDescription = (string)tok["catalogDescription"],
                        LastTaughtId = lastTaughtId,
                        OfferingFlags = Offering.NONE
                    };
                    courses.Add(c);

                    List<JToken> courseSections = MauiAPI.GetCourseSections(c.LastTaughtId, c.CourseNumber);
                    if (courseSections.Count > 0)
                    {
                        JToken section = courseSections[0];
                        if (section["prerequisite"] != null)
                        {
                            Console.WriteLine(c.CourseNumber + ": " + section["prerequisite"]);
                            preqCount++;
                        }
                        else
                        {
                            //Console.WriteLine(c.CourseName + ": NONE");
                            noPreqCount++;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("No courses -> course: {0} session: {1}", c.CourseNumber, c.LastTaughtId);
                        //List<JToken> tokens = MauiAPI.GetCourse(c.CourseNumber);
                        //foreach (JToken t in tokens)
                        //{
                        //    Console.WriteLine('\t' + t.ToString());
                        //}
                        numErrors++;
                    }
                }
            }
            Console.WriteLine("Course overview:\n\tTotal: {0}\n\tNum with preq: {1}\n\tNum with no preq: {2}\n\tNum errors: {3}", courses.Count, preqCount, noPreqCount, numErrors);

            return courses;
        }
    }
}
