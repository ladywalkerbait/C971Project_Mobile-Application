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
    public class Assessments
    {
        [PrimaryKey, AutoIncrement]
        public int AssessmentId { get; set; }
        public int ClassId { get; set; } //foreign key from the Classes table
        public string AssessmentName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string AssessmentType { get; set; }
        public bool StartNotification { get; set; }

    }
}
