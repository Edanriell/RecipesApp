using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RecipesApp.Client.Core.Validation;

public class ValidationErrorExposer : INotifyPropertyChanged, IDisposable
{
    private readonly ObservableValidator _validator;

    public ValidationErrorExposer(ObservableValidator observableValidator)
    {
        _validator = observableValidator;
        _validator.ErrorsChanged += ObservableValidator_ErrorsChanged;
    }

    public List<ValidationResult> this[string property]
        => _validator.GetErrors(property).ToList();

    public void Dispose() { _validator.ErrorsChanged -= ObservableValidator_ErrorsChanged; }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void ObservableValidator_ErrorsChanged(object? sender, DataErrorsChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs($"Item[{e.PropertyName}]"));
    }
}