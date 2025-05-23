//adding this to see
using Plugin.LocalNotification;
using SQLite;
using System;
using Microsoft.Maui.Controls;
using C971.Models;
using C971.Services;
using C971.Views;
using C971;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace C971.Views;

public partial class TermList : ContentPage
{
	//private ObservableCollection<Terms> _terms; 
	public TermList()
	{
		InitializeComponent();
		//_terms = new ObservableCollection<Terms>(); 
		//BindingContext = this; 
	}

	//private async void OnAddTermClicked(object sender, EventArgs e)
	//{
	//	Console.WriteLine("Button Clicked");
	//	await Navigation.PushAsync(new AddTermPage());

	//	//adding to try to refresh page
	//	var terms = await DatabaseService.GetTerm();
		
	//}
	////checking to try to make it dynamic 3/31
	//protected override async void OnAppearing()
	//{
	//	base.OnAppearing();
	//	var termsList = await DatabaseService.GetTerm();
	//	_terms.Clear();
	//	foreach (var term in termsList)
	//	{
	//		_terms.Add(term);
	//	}
	//}
	//public ObservableCollection<Terms> Terms => _terms; //not needed yet

	////adding for now
	//private async void OnTermButtonClicked(object sender, EventArgs e)
	//{
	//	var button = sender as Button;
	//	var selectedTerm = button?.BindingContext as Terms;
	//	if (selectedTerm != null)
 //       {
	//		//added this to show button works, but it is not connected yet. I will connect in part C2
	//		//await DisplayAlert("Term Selected", $"You clicked on: {selectedTerm.TermName}", "OK");
	//		//Console.WriteLine($"Term selected: {selectedTerm.TermName}");
			
	//		await Navigation.PushAsync(new TermDetailedView(selectedTerm.TermId));
	//	}
 //   }
	//private async void OnEditTermClicked(object sender, EventArgs e)
	//{
	//	Button button = (Button) sender;
		
	//	int termId = (int)button.CommandParameter;
	//	var selectedTerm = Terms.FirstOrDefault(t => t.TermId == termId);
 //       if (selectedTerm != null)
 //       {
	//		await Navigation.PushAsync(new EditTermPage(selectedTerm));
 //       }
 //      // await DatabaseService.UpdateTerm(termId, termName, startDate, endDate);
		
	//}
	//private async void OnDeleteTermClicked(object sender, EventArgs e)
	//{
	//	Button button = (Button) sender;
	//	if (button != null)
	//	{
	//		int termId = (int)button.CommandParameter;
	//		await DatabaseService.DeleteTerm(termId);
	//		//await DatabaseService.GetTerm();
	//		var termsList = await DatabaseService.GetTerm();
	//		_terms.Clear();
	//		foreach( var term in termsList)
	//		{
	//			_terms.Add(term);
	//		}
	//	}
	//}

	//Modified code 4/3
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (Services.Settings.FirstRun)
		{
			await DatabaseService.LoadSampleData();
			Services.Settings.FirstRun = false;
			await RefreshTermsCollectionView();
		}
		await RefreshTermsCollectionView();
		ShowClassesNotifications();
		ShowAssessmentsNotifications();
	}
	async void AddTerm_OnClicked(object? sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddTermPage());
	}

	//private async void ClearDatabase_OnClicked(object sender, EventArgs e) //Delete before submitting project
	//{
	//	await DatabaseService.ClearSampleData();
	//	await RefreshTermsCollectionView();
	//}
	//private async void LoadSampleData_OnClicked(object sender, EventArgs e) //Delete before submitting project
	//{
	//	if (Settings.FirstRun)
	//	{
	//		await DatabaseService.LoadSampleData();
	//		await RefreshTermsCollectionView();
	//	}
	//}
	private async void TermsCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection != null)
		{
			Terms term = (Terms)e.CurrentSelection.FirstOrDefault();

			await Navigation.PushAsync(new EditTermPage(term));
		}
    }
	private async Task RefreshTermsCollectionView()

	{
		TermsCollectionView.ItemsSource = await DatabaseService.GetTerm();
	}

	//Notifications
	private async void ShowClassesNotifications()
	{
		if (await LocalNotificationCenter.Current.AreNotificationsEnabled()==false)
		{
			await LocalNotificationCenter.Current.RequestNotificationPermission();
		}
		var ClassesList = await DatabaseService.GetClass();
		var notifyRandom = new Random();

		foreach (Classes classRecord  in ClassesList)
		{
			if (classRecord.StartNotification == true)
			{
				if (classRecord.StartDate.Date == DateTime.Today)
				{
					var notifyId = notifyRandom.Next(1000);

					var notification = new NotificationRequest
					{
						NotificationId = notifyId,
						Title = "Class Notification",
						Description = $"{classRecord.ClassName} begins today!",
						ReturningData = $"ClassId={classRecord.ClassId}",
						BadgeNumber = 1,
						Schedule = new NotificationRequestSchedule()
						{
							NotifyTime = DateTime.Now.AddSeconds(5),
						}
					};
					await LocalNotificationCenter.Current.Show(notification);
				}
			}
			//added to check for end dates
			if (classRecord.EndDate.Date == DateTime.Today)
			{
				var endNotification = new NotificationRequest
				{
					NotificationId = notifyRandom.Next(1000),
					Title = "Class Notification",
					Description = $"{classRecord.ClassName} ends today!",
					ReturningData = $"ClassId={classRecord.ClassId}",
					BadgeNumber = 1,
					Schedule = new NotificationRequestSchedule()
					{
						NotifyTime = DateTime.Now.AddSeconds(5),
					}
				};
				await LocalNotificationCenter.Current.Show(endNotification);
            }
		}
	}

    private async void ShowAssessmentsNotifications()
    {
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }
        var AssessmentList = await DatabaseService.GetAssessment();
        var notifyRandom = new Random();

        foreach (Assessments assessmentRecord in AssessmentList)
        {
            if (assessmentRecord.StartNotification == true)
            {
                if (assessmentRecord.StartDate.Date == DateTime.Today)
                {
                    var notifyId = notifyRandom.Next(1000);

                    var notification = new NotificationRequest
                    {
                        NotificationId = notifyId,
                        Title = "Assessment Notification",
                        Description = $"{assessmentRecord.AssessmentName} begins today!",
                        ReturningData = $"AssessmentId={assessmentRecord.AssessmentId}",
                        BadgeNumber = 1,
                        Schedule = new NotificationRequestSchedule()
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5),
                        }
                    };
                    await LocalNotificationCenter.Current.Show(notification);
                }
            }
            //added to check for end dates
            if (assessmentRecord.EndDate.Date == DateTime.Today)
            {
                var endNotification = new NotificationRequest
                {
                    NotificationId = notifyRandom.Next(1000),
                    Title = "Assessment Notification",
                    Description = $"{assessmentRecord.AssessmentName} ends today!",
                    ReturningData = $"AssessmentId={assessmentRecord.AssessmentId}",
                    BadgeNumber = 1,
                    Schedule = new NotificationRequestSchedule()
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                    }
                };
                await LocalNotificationCenter.Current.Show(endNotification);
            }
        }
    }
}