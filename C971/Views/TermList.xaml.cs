//adding this to see
using SQLite;
using System;
using Microsoft.Maui.Controls;
using C971.Models;
using C971.Services;
using C971.Views;
using C971;
using System.Collections.ObjectModel;

namespace C971.Views;

public partial class TermList : ContentPage
{
	private ObservableCollection<Terms> _terms; //added to try to make it dynamic 3/31
	public TermList()
	{
		InitializeComponent();
		_terms = new ObservableCollection<Terms>(); //added to try to make it dynamic 3/31
		BindingContext = this; //added to try to make it dynamic 3/31
	}

	private async void OnAddTermClicked(object sender, EventArgs e)
	{
		Console.WriteLine("Button Clicked");
		await Navigation.PushAsync(new AddTermPage());

		//adding to try to refresh page
		var terms = await DatabaseService.GetTerm();
		
	}
	//checking to try to make it dynamic 3/31
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var termsList = await DatabaseService.GetTerm();
		_terms.Clear();
		foreach (var term in termsList)
		{
			_terms.Add(term);
		}
	}
	public ObservableCollection<Terms> Terms => _terms; //not needed yet

	//adding for now
	private async void OnTermButtonClicked(object sender, EventArgs e)
	{
		var button = sender as Button;
		var selectedTerm = button?.BindingContext as Terms;
        if (selectedTerm != null)
        {
			//added this to show button works, but it is not connected yet. I will connect in part C2
			await DisplayAlert("Term Selected", $"You clicked on: {selectedTerm.TermName}", "OK");
			Console.WriteLine($"Term selected: {selectedTerm.TermName}");
        }
    }
	private async void OnEditTermClicked(object sender, EventArgs e)
	{
		Button button = (Button) sender;
		
		int termId = (int)button.CommandParameter;
		var selectedTerm = Terms.FirstOrDefault(t => t.TermId == termId);
        if (selectedTerm != null)
        {
			await Navigation.PushAsync(new EditTermPage(selectedTerm));
        }
       // await DatabaseService.UpdateTerm(termId, termName, startDate, endDate);
		
	}
	private async void OnDeleteTermClicked(object sender, EventArgs e)
	{
		Button button = (Button) sender;
		if (button != null)
		{
			int termId = (int)button.CommandParameter;
			await DatabaseService.DeleteTerm(termId);
			//await DatabaseService.GetTerm();
			var termsList = await DatabaseService.GetTerm();
			_terms.Clear();
			foreach( var term in termsList)
			{
				_terms.Add(term);
			}
		}
	}

}