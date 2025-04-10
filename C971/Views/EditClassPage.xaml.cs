using C971.Models;
using C971.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace C971.Views;

public partial class EditClassPage : ContentPage
{
    private readonly int _selectedClassId;
	public EditClassPage(Classes selectedClass)
	{
		InitializeComponent();

        _selectedClassId = selectedClass.ClassId;

        ClassId.Text = selectedClass.ClassId.ToString();
        ClassName.Text = selectedClass.ClassName;
        StartDatePicker.Date = selectedClass.StartDate;
        EndDatePicker.Date = selectedClass.EndDate;
        CourseStatus.SelectedItem = selectedClass.CourseStatus;
        InstructorName.Text = selectedClass.InstructorName;
        InstructorPhone.Text = selectedClass.InstructorPhone;
        InstructorEmail.Text = selectedClass.InstructorEmail;
        NotesEditor.Text = selectedClass.Notes;

	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int countAssessments = await DatabaseService.GetAssessmentsCountAsync(_selectedClassId);
        CountLabel.Text = countAssessments.ToString();

        AssessmentsCollectionView.ItemsSource = await DatabaseService.GetAssessment(_selectedClassId);
    }

    async void SaveClass_OnClicked(object sender, EventArgs e)
    {
        DateTime startDate = StartDatePicker.Date;
        DateTime endDate = EndDatePicker.Date;

        if (string.IsNullOrWhiteSpace(ClassName.Text))
        {
            await DisplayAlert("Missing Class Name", "Please enter a name.", "OK");
            return;
        }
        if (string.IsNullOrEmpty(InstructorName.Text))
        {
            await DisplayAlert("Missing Instructor Name", "Please enter a name", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(InstructorPhone.Text))
        {
            await DisplayAlert("Missing Instructor Phone Number", "Please enter a phone number.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(InstructorEmail.Text))
        {
            await DisplayAlert("Missing Instructor Email", "Please enter an email address.", "OK");
            return;
        }

        if (startDate > endDate)
        {
            await DisplayAlert("Invalid Date", "The Start Date cannot come after the End Date.", "OK");
            return;
        }

        //added 4/7 for further validation
        if (!IsValidName(InstructorName.Text))
        {
            await DisplayAlert("Invalid Name", "Please enter a name in a valid format", "OK");
            return;
        }
        //added 4/7 for further validation
        if (!IsValidPhone(InstructorPhone.Text))
        {
            await DisplayAlert("Invalid Phone Number", "Please enter a phone number in a valid format.", "OK");
            return;
        }
        //added 4/7 for further validation
        if (!IsValidEmail(InstructorEmail.Text))
        {
            await DisplayAlert("Invalid Email Address", "Please enter an email in a valid format.", "OK");
            return;
        }

        await DatabaseService.UpdateClass(Int32.Parse(ClassId.Text), ClassName.Text, StartDatePicker.Date, EndDatePicker.Date,
            CourseStatus.SelectedItem.ToString(), InstructorName.Text, InstructorPhone.Text, InstructorEmail.Text,
            NotesEditor.Text, Notification.IsToggled);
        await Navigation.PopAsync();
    }
    async void CancelClass_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    async void DeleteClass_OnClicked(object sender, EventArgs e)
    {
        var delete = await DisplayAlert("Delete this Class?", "Delete this Class?", "Yes", "No");
        if (delete == true)
        {
            var classId = int.Parse(ClassId.Text);
            await DatabaseService.DeleteClass(classId);
            await DisplayAlert("Class Deleted", "Class Deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Canceled", "No classes were deleted", "OK");
        }
        await Navigation.PopAsync();
    }

    async void ShareButton_OnClicked(object sender, EventArgs e)
    {
        var text = NotesEditor.Text;

        if (string.IsNullOrEmpty(text))
        {
            await DisplayAlert("Error", "There are not notes to share.", "OK");
            return;
        }

        await Share.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Share Notes"
        });
    }

    //Extra validation methods for Name, Phone, and Email
    public static bool IsValidName(string InstructorName)
    {
        string[] nameParts = InstructorName.Split(' ');

        foreach (char c in InstructorName)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                return false;
            }
        }
        if (InstructorName.StartsWith(" ") || InstructorName.EndsWith(" ") || InstructorName.Contains("  "))
        {
            return false;
        }
        return true;
    }
    public static bool IsValidPhone(string InstructorPhone)
    {
        if (InstructorPhone.Length == 12 && InstructorPhone[3] == '-' && InstructorPhone[7] == '-')
        {
            string[] parts = InstructorPhone.Split('-');
            return parts.Length == 3 && parts[0].All(char.IsDigit) && parts[1].All(char.IsDigit) && parts[2].All(char.IsDigit);
        }
        if (InstructorPhone.Length == 8 && InstructorPhone[3] == '-')
        {
            string[] parts = InstructorPhone.Split('-');
            return parts.Length == 2 && parts[0].All(char.IsDigit) && parts[1].All(char.IsDigit);
        }
        return false;
    }
    public static bool IsValidEmail(string InstructorEmail)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(InstructorEmail, pattern);
        //return false;
    }

    async void AssessmentsCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedAssessment = (Assessments)e.CurrentSelection.FirstOrDefault();
        if (e.CurrentSelection != null)
        {
            //await Navigation.PushAsync((new EditAssessmentsPage(selectedAssessment)));
        }
    }
    async void AddAssessment_OnClicked(object sender, EventArgs e)
    {
        var classId = Int32.Parse(ClassId.Text);

        //var classCount = await _db.Table<Classes>().Where(i => i.TermId == termId).CountAsync();
        int countAssessments = await DatabaseService.GetAssessmentsCountAsync(_selectedClassId);
        if (countAssessments >= 2)
        {
            await DisplayAlert("Limit Reached", "You cannot add more than 2 assessments to this course.", "OK");
            return;
        }
   
        await Navigation.PushAsync(new AddAssessmentPage(classId));
    }
}