//using Android.App;
using System.Text.RegularExpressions;
using C971;
using C971.Services;
using C971.Models;
using SQLite;
using Microsoft.Maui.Controls;
using System.Linq; //am i allowed to use this?
using System.Runtime.CompilerServices;

namespace C971.Views;


public partial class AddAssessmentPage : ContentPage
{
	private readonly int _selectedClassId;
	public AddAssessmentPage(int classId)
	{
		InitializeComponent();
        _selectedClassId = classId;
	}
	async void SaveAssessment_OnClicked(object sender, EventArgs e)
	{
        DateTime startDate = StartDatePicker.Date;
        DateTime endDate = EndDatePicker.Date;
        int classId = _selectedClassId;
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
        bool assessmentExists = await DoesAssessmentExist(classId, assessmentType);
        if (assessmentExists)
        {
            await DisplayAlert("Error", $"An assessment of type {assessmentType} already exists for this class.", "Ok");
            return;
        }
       
        //var existingAssessment = DatabaseService.GetAssessment(_selectedClassId); //added to try to check for same existing assessment
        //if (existingAssessment != null)
        //{
        //    await DisplayAlert("Limit Reached", "You cannot add more than one of each assessment type to this course.", "OK");
        //    return;
        //}

        await DatabaseService.AddAssessment(_selectedClassId, AssessmentName.Text, StartDatePicker.Date, EndDatePicker.Date,
            AssessmentTypePicker.SelectedItem.ToString());
        await Navigation.PopAsync();
    }
	async void CancelAssessment_OnClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
	async void Home_OnClicked(object sender, EventArgs e)
	{
        await Navigation.PopToRootAsync();
    }

    public async Task<bool> DoesAssessmentExist(int classId, string assessmentType)
    {
        var existingAssessments = await DatabaseService.GetAssessment(classId);
        foreach (var assessment in existingAssessments)
        {
            if (assessment.AssessmentType.Equals(assessmentType, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}