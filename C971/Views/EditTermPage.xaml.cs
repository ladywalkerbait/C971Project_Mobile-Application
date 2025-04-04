using C971.Models;
using C971.Services;
using System.Threading.Tasks;
using Plugin.LocalNotification;

namespace C971.Views;

public partial class EditTermPage : ContentPage
{
	private readonly int _selectedTermId;
	//public Terms CurrentTerm { get; set; }
	public EditTermPage(Terms term)
	{
		InitializeComponent();
		//CurrentTerm = term;
		//BindingContext = CurrentTerm;
		_selectedTermId = term.TermId;

		TermId.Text = term.TermId.ToString();
		TermName.Text = term.TermName;
		StartDatePicker.Date = term.StartDate;
		EndDatePicker.Date = term.EndDate;
	}
    //private async void OnEditSaveClicked(object sender, EventArgs e)
    //{
    //	string updatedTermName = TermNameEntry.Text;
    //	DateTime updatedStartDate = StartDatePicker.Date;
    //	DateTime updatedEndDate = EndDatePicker.Date;

    //	await DatabaseService.UpdateTerm(CurrentTerm.TermId, updatedTermName, updatedStartDate, updatedEndDate);	

    //	await Navigation.PopAsync();
    //}


    //Methods added 4/3
    protected override async void OnAppearing()
	{
		base.OnAppearing();

		ClassesCollectionView.ItemsSource = await DatabaseService.GetClass(_selectedTermId);
	}
	
	 async void SaveTerm_OnClicked(object sender, EventArgs e)
	{
		if (string.IsNullOrWhiteSpace(TermName.Text))
		{
			await DisplayAlert("Missing Term Name", "Please enter a Term Name", "OK");
			return;
		}
		await DatabaseService.UpdateTerm(Int32.Parse(TermId.Text), TermName.Text, DateTime.Parse(StartDatePicker.Date.ToString()),
			  DateTime.Parse(EndDatePicker.Date.ToString()));
		//await DatabaseService.UpdateTerm(Int32.Parse(TermId.Text), TermName.Text, StartDatePicker.Date, EndDatePicker.Date);
		await Navigation.PopAsync();
	}
	async void CancelTerm_OnClicked(object sender, EventArgs e)
	{
		await Navigation.PopAsync();
	}
	async void DeleteTerm_OnClicked(object sender, EventArgs e) 
	{
		var answer = await DisplayAlert("Delete Term and related Classes?", "Delete this Term?", "Yes", "No");
		if (answer == true)
		{
			var termId = int.Parse(TermId.Text);
			await DatabaseService.DeleteTerm(termId);
			await DisplayAlert("Term Deleted", "Term Deleted", "OK");
		}
		else
		{
			await DisplayAlert("Delete Canceled", "Nothing was deleted", "OK");
		}
		await Navigation.PopAsync();
	}
	async void ClassesCollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var classes = (Classes)e.CurrentSelection.FirstOrDefault();
		if (e.CurrentSelection != null)
		{
			//await Navigation.PushAsync((new ClassEdit(classes)));
		}
	}
	async void AddClass_OnClicked(object sender, EventArgs e)
	{
		var termId = Int32.Parse(TermId.Text);
		//await Navigation.PushAsync(new AddClassPage(termId));
	}
}