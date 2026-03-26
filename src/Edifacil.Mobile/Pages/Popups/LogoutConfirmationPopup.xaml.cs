using CommunityToolkit.Maui.Views;
using Edifacil.Mobile.ViewModels;

namespace Edifacil.Mobile.Pages.Popups;

public partial class LogoutConfirmationPopup : Popup
{
	public LogoutConfirmationPopup(LogoutConfirmationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
