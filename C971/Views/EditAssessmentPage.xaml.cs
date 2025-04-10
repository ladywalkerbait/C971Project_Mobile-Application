//using Android.App;
using C971.Models;
using C971.Services;
//using Java.Lang;
using System;

namespace C971.Views;

public partial class EditAssessmentPage : ContentPage
{
	private readonly int _selectedAssessmentId;
    private readonly int _classId;
	public EditAssessmentPage(Assessments selectedAssessment)
	{
		InitializeComponent();

		_selectedAssessmentId = selectedAssessment.AssessmentId;

		AssessmentId.Text = selectedAssessment.AssessmentId.ToString();
		AssessmentName.Text = selectedAssessment.AssessmentName;
		StartDatePicker.Date = selectedAssessment.StartDate;
		EndDatePicker.Date = selectedAssessment.EndDate;
		AssessmentTypePicker.SelectedItem = selectedAssessment.AssessmentType;
        Notification.IsToggled = selectedAssessment.StartNotification;

	}

	async void SaveAssessment_OnClicked(object sender, EventArgs e)
	{
        DateTime startDate = StartDatePicker.Date;
        DateTime endDate = EndDatePicker.Date;

        int assessmentId = _selectedAssessmentId;
        int classId = _classId;
        string assessmentType = AssessmentTypePicker.SelectedItem.ToString();

        if (string.IsNullOrWhiteSpace(AssessmentName.Text))
        {
            await DisplayAlert("Missing Assessment Name", "Please enter a name.", "OK");
            return;
        }
        if (startDate > endDate)
        {
            await DisplayAlert("Invalid Date", "The Start Date cannot come after the End Date.", "OK");
            return;
        }
        bool assessmentExists = await DoesAssessmentExist(classId, assessmentType, assessmentId);
        if (assessmentExists)
        {
            await DisplayAlert("Error", $"An assessment of type {assessmentType} already exists for this class.", "Ok");
            return;
        }

        await DatabaseService.UpdateAssessment(Int32.Parse(AssessmentId.Text), AssessmentName.Text, StartDatePicker.Date, EndDatePicker.Date,
            AssessmentTypePicker.SelectedItem.ToString());
        await Navigation.PopAsync();
    }
    async void CancelAssessment_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    async void DeleteAssessment_OnClicked(object sender, EventArgs e)
    {
        var delete = await DisplayAlert("Delete this Assessment?", "Delete this Assessment?", "Yes", "No");
        if (delete == true)
        {
            var assessmentId = int.Parse(AssessmentId.Text);
            await DatabaseService.DeleteAssessment(assessmentId);
            await DisplayAlert("Assessment Deleted", "Assessment Deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Canceled", "No assessments were deleted", "OK");
        }
        await Navigation.PopAsync();
    }

    public async Task<bool> DoesAssessmentExist(int classId, string assessmentType, int assessmentId)
    {
        var existingAssessments = await DatabaseService.GetAssessment(classId);
        foreach (var assessment in existingAssessments)
        {
            //if (assessment.AssessmentType.Equals(assessmentType, StringComparison.OrdinalIgnoreCase))
            if (assessment.AssessmentId != _selectedAssessmentId && assessment.AssessmentType == assessmentType)
            {
                return true;
            }
        }
        return false;
    }
}