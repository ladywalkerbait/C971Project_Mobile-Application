using C971.Services;
using C971.Models;
using C971;
using SQLite;
using System;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace C971.Views;

public partial class TermDetailedView : ContentPage
{
	
 //   private ObservableCollection<Classes> _class; //added to try to make it dynamic 3/31
 //   //public Terms selectedTerm { get; set; }
	//private int TermId { get; set; }
	//public TermDetailedView(int termId)
	//{
	//	InitializeComponent();
	//	TermId = termId;
 //       _class = new ObservableCollection<Classes>(); //added to try to make it dynamic 3/31
 //       BindingContext = this;
 //   }
	//protected override async void OnAppearing()
	//{
	//	//base.OnAppearing();
	//	//await LoadTermDetails();
 //       base.OnAppearing();

	//	await DatabaseService.AddInitialClasses(TermId);

	//	var classesList = await DatabaseService.GetClass(TermId);

	//	_class.Clear();
	//	foreach (var classItem in classesList)
	//	{
	//		_class.Add(classItem);
	//	}
	//	//CreateClassButtons(_class);
 //   }
 //   public ObservableCollection<Classes> Classes => _class;
 //   private async Task LoadTermDetails()
	//{
	//	//Get all terms and then filter for the correct term
	//	var terms = await DatabaseService.GetTerm();
	//	var term = terms.FirstOrDefault(t => t.TermId == TermId);

	//	if (term != null)
	//	{
	//		TermNameLabel.Text = term.TermName;
	//		//StartDateLabel.Text = $"Start Date: {term.StartDate.ToShortDateString()}";
	//		//EndDateLabel.Text = $"End Date: {term.EndDate.ToShortDateString()}";

	//		var classes = await DatabaseService.GetClass(TermId);
	//		if (classes.Any())
	//		{
 //               NoCoursesLabel.IsVisible = false;
	//			//CreateCourseButtons(classes.ToList());
	//		}
	//		else
	//		{
	//			NoCoursesLabel.IsVisible = true;
	//		}
	//	}
	//	else
	//	{
	//		await DisplayAlert("Error", "Term not found.", "OK");
	//	}
	//}
	//private void CreateClassButtons(ObservableCollection<Classes> classes)
	//{
	//	//ClassButtonLayout.Children.Clear();
	//	if (classes.Any())
	//	{
	//		//CoursesButtonLayout.Children.Clear();
	//		foreach (var classItem in classes)
	//		{
	//			var classButton = new Button
	//			{
	//				Text = classItem.ClassName,
	//				//Command = new Command(async () => await OnCourseButtonClicked(classItem.ClassId))
	//			};
				
	//		}
	//	}
	//}
 //   private async void OnCourseButtonClicked(object sender, EventArgs e)
	//{
 //       var button = sender as Button;
 //       var selectedClass = button?.BindingContext as Classes;
 //       if (selectedClass != null)
 //       {
	//		//added this to show button works, but it is not connected yet. 
	//		await DisplayAlert("Term Selected", $"You clicked on: {selectedClass.ClassName}", "OK");
	//		Console.WriteLine($"Term selected: {selectedClass.ClassName}");

	//		// await Navigation.PushAsync(new ClassDetailedView(selectedClass.ClassId));
	//	}
 //   }
 //   private async void OnAddCourseClicked(object sender, EventArgs e)
	//{
 //       Console.WriteLine("Button Clicked");
 //       await Navigation.PushAsync(new AddClassPage());

 //       //adding to try to refresh page
 //       var classes = await DatabaseService.GetClass();
 //   }
	//private async void OnDeleteCourseClicked(object sender, EventArgs e)
	//{

	//}
}