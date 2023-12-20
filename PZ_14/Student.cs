using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ_14
{
    public class Student
    {
        public string Name { get; set; }
        public string Grade1 { get; set; }
        public string Grade2 { get; set; }
        public string Grade3 { get; set; }
        public string Grade4 { get; set; }
        public string Grade5 { get; set; }
        public string Grade6 { get; set; }
        public string Grade7 { get; set; }

        public double AverageGrade
        {
            get
            {
                var grades = new List<double>
                {
                    Convert.ToDouble(Grade1),
                    Convert.ToDouble(Grade2),
                    Convert.ToDouble(Grade3),
                    Convert.ToDouble(Grade4),
                    Convert.ToDouble(Grade5),
                    Convert.ToDouble(Grade6),
                    Convert.ToDouble(Grade7)
                };
                return grades.Average();
            }
        }
    }
}
