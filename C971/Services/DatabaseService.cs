using SQLite;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971.Views;
using C971.Models;

namespace C971.Services
{
    public static class DatabaseService
    {

        private static SQLiteAsyncConnection _db;
        //private static object term1;

        //private static string GetDatabase()
        //{
        //    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "TermData.db";
        //}
        static async Task GetDatabase() //took out public 4/3
        {
            if (_db != null) //Don't create database if it already exists
            {
                return;
            }
            //Get absolute path to database file
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "TermData.db");

            _db = new SQLiteAsyncConnection(databasePath);

            await _db.CreateTableAsync<Terms>();
            await _db.CreateTableAsync<Classes>();
            await _db.CreateTableAsync<Assessments>();

            //adding for future work
            //await _db.CreateTableAsync<Models.User>();

            // await AddInitialTerms(); //testing

        }
        #region Terms methods
        //Below are methods for the Terms
        public static async Task AddTerm(string termName, DateTime startDate, DateTime endDate)
        {
            await GetDatabase();

            var terms = new Terms()
            {
                TermName = termName,
                StartDate = startDate,
                EndDate = endDate
            };
            await _db.InsertAsync(terms);

            var termId = terms.TermId;
        }
        //method to add intial terms
        private static async Task AddInitialTerms()
        {
            await GetDatabase();
            var existingTerms = await _db.Table<Terms>().ToListAsync();
            if (existingTerms.Count == 0)
            {
                var term1 = new Terms { TermName = "Term 1", StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(6) };
                var term2 = new Terms { TermName = "Term 2", StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(5) };
                await _db.InsertAsync(term1);
                await _db.InsertAsync(term2);

            }

        }
        public static async Task DeleteTerm(int termId)
        {
            await GetDatabase();
            ////added to delete to ensure correct termId
            //var termsList = await _db.Table<Terms>().ToListAsync();
            //foreach (var term in termsList)
            //{
            //    if (term.TermId == termId)
            //    { 
            //        await _db.DeleteAsync(term); 
            //        break;
            //    }
            //}

            await _db.DeleteAsync<Terms>(termId);
        }
        public static async Task<IEnumerable<Terms>> GetTerm()
        {
            await GetDatabase();
            var terms = await _db.Table<Terms>().ToListAsync();
            return terms;
        }
        public static async Task UpdateTerm(int termId, string termName, DateTime startDate, DateTime endDate)
        {
            await GetDatabase();

            var termQuery = await _db.Table<Terms>().Where(i => i.TermId == termId).FirstOrDefaultAsync();
            if (termQuery != null)
            {
                termQuery.TermName = termName;
                termQuery.StartDate = startDate;
                termQuery.EndDate = endDate;

                await _db.UpdateAsync(termQuery);

            }
        }
        #endregion

        #region Classes methods
        //Below are methods for the Classes
        public static async Task AddClass(int termId, string className, DateTime startDate, DateTime endDate,
                                            string courseStatus, string instructorName, string instructorPhone,
                                            string instructorEmail, string notes)
        {
            await GetDatabase();

            //check if Term already has 6 classes before allowing to add a class
            var existingClassesCount = await _db.Table<Classes>().Where(i => i.TermId == termId).CountAsync();
            if (existingClassesCount >= 6)
            {
                throw new InvalidOperationException("You cannot add more than 6 classes for this Term.");
            }

            var classes = new Classes()
            {
                TermId = termId,
                ClassName = className,
                StartDate = startDate,
                EndDate = endDate,
                CourseStatus = courseStatus,
                InstructorName = instructorName,
                InstructorPhone = instructorPhone,
                InstructorEmail = instructorEmail,
                Notes = notes
            };
            await _db.InsertAsync(classes);
            var classId = classes.ClassId;

        }
        //tesing below method
        public static async Task AddInitialClasses(int termId)
        {
            await GetDatabase();
            var existingTerms = await _db.Table<Terms>().ToListAsync();
            var term1 = existingTerms.FirstOrDefault(t => t.TermName == "Term 1");

            var term1Classes = await _db.Table<Classes>().Where(c => c.TermId == term1.TermId).ToListAsync();

            //await GetDatabase();

            //var existingClasses = await _db.Table<Classes>().Where(c => c.TermId == termId).ToListAsync();
            ////if no classes exist, add them
            //if (existingClasses.Count == 0)
            if (term1Classes.Count == 0)
            {

                var class1 = new Classes
                {
                    TermId = termId,
                    ClassName = "Course 1", //};
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    CourseStatus = "In Progress",
                    InstructorName = "John Doe",
                    InstructorPhone = "555-5555",
                    InstructorEmail = "johndoe@wgu.edu",
                    Notes = "Notes for class go here."
                };

                await _db.InsertAsync(class1);
            }

        }
        public static async Task DeleteClass(int classId)
        {
            await GetDatabase();

            //var classList = await _db.Table<Classes>().ToListAsync();
            //foreach (var class in classList)
            //{
            //    if (class.ClassId == classId)
            //    {
            //        await _db.DeleteAsync(class);
            //        break;
            //    }
            //}

            await _db.DeleteAsync<Classes>(classId);
        }
        public static async Task<IEnumerable<Classes>> GetClass(int termId)
        {
            await GetDatabase();

            var classes = await _db.Table<Classes>().Where(i => i.TermId == termId).ToListAsync();
            return classes;
        }
        public static async Task<IEnumerable<Classes>> GetClass()
        {
            await GetDatabase();

            var classes = await _db.Table<Classes>().ToListAsync();
            return classes;
        }
        public static async Task UpdateClass(int classId, string className, DateTime startDate, DateTime endDate,
                                            string courseStatus, string instructorName, string instructorPhone,
                                            string instructorEmail, string notes)
        {
            await GetDatabase();

            var classesQuery = await _db.Table<Classes>().Where(i => i.ClassId == classId).FirstOrDefaultAsync();

            if (classesQuery != null)
            {
                classesQuery.ClassName = className;
                classesQuery.StartDate = startDate;
                classesQuery.EndDate = endDate;
                classesQuery.CourseStatus = courseStatus;
                classesQuery.InstructorEmail = instructorEmail;
                classesQuery.InstructorName = instructorName;
                classesQuery.Notes = notes;

                await _db.UpdateAsync(classesQuery);
            }
        }
        #endregion

        #region Assessments methods
        //Below are methods for the Assessments
        public static async Task AddAssessment(int classId, string assessmentName)
        {
            await GetDatabase();

            var assessments = new Models.Assessments()
            {
                ClassId = classId,
                AssessmentName = assessmentName
            };
            await _db.InsertAsync(assessments);
            var assessmentId = assessments.AssessmentId;
        }
        public static async Task DeleteAssessment(int assessmentId)
        {
            await GetDatabase();

            await _db.DeleteAsync<Assessments>(assessmentId);
        }
        public static async Task<IEnumerable<Models.Assessments>> GetAssessment(int classId)
        {
            await GetDatabase();

            var assessments = await _db.Table<Models.Assessments>().Where(i => i.ClassId == classId).ToListAsync();
            return assessments;
        }
        public static async Task<IEnumerable<Models.Assessments>> GetAssessment()
        {
            await GetDatabase();

            var assessments = await _db.Table<Models.Assessments>().ToListAsync();
            return assessments;
        }
        public static async Task UpdateAssessment(int assessmentId, string assessmentName)
        {
            await GetDatabase();

            var assessmentQuery = await _db.Table<Assessments>().Where(i => i.AssessmentId == assessmentId).FirstOrDefaultAsync();

            if (assessmentQuery != null)
            {
                assessmentQuery.AssessmentName = assessmentName;
            }
        }
        #endregion

        #region DemoData
        public static async Task LoadSampleData() //Changed "void" to "Task"
        {
            //Add Term with Classes
            await GetDatabase();
            Terms term = new Terms
            {
                TermName = "Term 1",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Now.AddMonths(5),
            };
            await _db.InsertAsync(term);

            //Classes class1 = new Classes
            //{
            //    ClassName = "Course 1",
            //    StartDate = DateTime.Today.Date,
            //    EndDate = DateTime.Now.AddMonths(2),
            //    CourseStatus = "In-Progress",
            //    InstructorName = "Anika Patel",
            //    InstructorPhone = "555-123-4567",
            //    InstructorEmail = "anika.patel@strimeuniversity.edu",
            //    TermId = term.TermId
            //};
            //await _db.InsertAsync(class1);

            //Classes class2 = new Classes
            //{
            //    ClassName = "Course 2"
            //};

            //Add another Term with Classes
            Terms term2 = new Terms
            {
                TermName = "Term 2",
                StartDate = DateTime.Now.AddMonths(6),
                EndDate = DateTime.Now.AddMonths(10)
            };
            await _db.InsertAsync(term2);

            //Classes class1 = new Classes
            //{
            //    //Class info goes here
            //    TermId = term2.TermId
            //};
        }
        public static async Task ClearSampleData() //Changed "void" to "Task"
        {
            await GetDatabase();
            await _db.DropTableAsync<Terms>();
            await _db.DropTableAsync<Classes>();
            await _db.DropTableAsync<Assessments>();
            _db = null;

            Settings.ClearSettings();
        }
        public static async void LoadSampleDataSql()
        {

        }
        #endregion
    }
}
