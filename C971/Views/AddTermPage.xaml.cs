using C971.Services;
using C971.Models;
using SQLite;
using Microsoft.Maui.Controls;
using System;
using C971;

namespace C971.Views;

public partial class AddTermPage : ContentPage
{
	public AddTermPage()
	{
		InitializeComponent();
	}
	//private async void OnSaveTermClicked(object sender, EventArgs e)
	//{
	//	string termName = TermName.Text;
	//	DateTime startDate = StartDatePicker.Date;
	//	DateTime endDate = EndDatePicker.Date;

	//	if (string.IsNullOrEmpty(termName))
	//	{
	//		await DisplayAlert("Error", "Please enter a term name", "OK");
	//		return; //added to see if this works 3/28
	//	}

	//	//Testing to see if this adds
	//	var newTerm = new Terms
	//	{
	//		TermName = termName,
	//		StartDate = startDate,
	//		EndDate = endDate
	//	};
	//	//
	//	await DatabaseService.AddTerm(termName, startDate, endDate);

	//	await Navigation.PopAsync();

	//	await DisplayAlert("Success", "Term added successfully!", "OK");
	//}

	async void SaveTerm_OnClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(TermName.Text))
		{
			await DisplayAlert("Missing Name", "Please Enter Term Name", "OK");
			return;
		}
		await DatabaseService.AddTerm(TermName.Text, StartDatePicker.Date, EndDatePicker.Date);
		await Navigation.PushAsync(new TermList());
	}
	async void CancelTerm_OnClicked(Object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
}