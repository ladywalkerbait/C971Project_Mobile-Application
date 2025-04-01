using C971.Models;
using C971.Services;

namespace C971.Views;

public partial class EditTermPage : ContentPage
{
	public Terms CurrentTerm { get; set; }
	public EditTermPage(Terms term)
	{
		InitializeComponent();
		CurrentTerm = term;
		BindingContext = CurrentTerm;
	}
	private async void OnEditSaveClicked(object sender, EventArgs e)
	{
		string updatedTermName = TermNameEntry.Text;
		DateTime updatedStartDate = StartDatePicker.Date;
		DateTime updatedEndDate = EndDatePicker.Date;

		await DatabaseService.UpdateTerm(CurrentTerm.TermId, updatedTermName, updatedStartDate, updatedEndDate);	

		await Navigation.PopAsync();
	}
}