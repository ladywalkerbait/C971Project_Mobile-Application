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
    public class Terms
    {
        [PrimaryKey, AutoIncrement]
        public int TermId { get; set; } //Auto-incrementing primary key
        public string TermName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
