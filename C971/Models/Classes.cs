using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using C971.Services;
using C971.Views;

namespace C971.Models
{
    public class Classes
    {
        [PrimaryKey, AutoIncrement]
        public int ClassId { get; set; }
        public int TermId { get; set; } //foreign key from the Terms table
        public string ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CourseStatus { get; set; }
        public string InstructorName { get; set; }
        public string InstructorPhone { get; set; }
        public string InstructorEmail { get; set; }
        public string Notes { get; set; }
        public bool StartNotification { get; set; }

    }
}
