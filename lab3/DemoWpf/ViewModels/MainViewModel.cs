using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace DemoWpf.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string firstName = string.Empty;

    partial void OnFirstNameChanged(string value)
    {
        OnPropertyChanged(nameof(FullName));

        // Validation: FirstName is required
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            ValidationMessage = "Imię jest wymagane";
        }
        else
        {
            ValidationMessage = string.Empty;
        }
    }

    [ObservableProperty]
    private string lastName = string.Empty;

    partial void OnLastNameChanged(string value)
    {
        OnPropertyChanged(nameof(FullName));
    }
    [ObservableProperty]
    private bool isBusy;

    partial void OnIsBusyChanged(bool value)
    {
        // Notify that IsNotBusy changed as well
        OnPropertyChanged(nameof(IsNotBusy));
        // If the command has been generated, notify it to requery can-execute
        SaveCommand?.NotifyCanExecuteChanged();
    }

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    private string validationMessage = string.Empty;

    [ObservableProperty]
    private string displayName = string.Empty;

    public string FullName => $"{FirstName} {LastName}".Trim();

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    public async Task SaveAsync()
    {
        // Validate FirstName before saving
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            ValidationMessage = "Imię jest wymagane";
            // Notify command state in case UI relies on it
            SaveCommand?.NotifyCanExecuteChanged();
            return;
        }

        // Clear validation message when valid
        ValidationMessage = string.Empty;

        try
        {
            IsBusy = true;
            // Simulate async save operation
            await Task.Delay(500);
            // In a real app you'd persist data here

            // Update DisplayName only after successful save/click
            DisplayName = FullName;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
