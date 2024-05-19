using News.ViewModels;

namespace News.Views;

public partial class HeadlinesView : ContentPage
{
    private readonly HeadlinesViewModel _viewModel;

	public HeadlinesView(HeadlinesViewModel viewModel)
	{
        _viewModel = viewModel;
		InitializeComponent();
        Task.Run(async () => await Initialize(HeadlinesView.GetScopeFromRoute()));
    }

    private async Task Initialize(string scope)
    {
        BindingContext = _viewModel;
        await _viewModel.Initialize(scope);
    }

    private static string GetScopeFromRoute()
    {
        // Hack: As the shell can't define query parameters in XAML, we have to parse the route. 
        // as a convention the last route section defines the category.
        var route = Shell.Current.CurrentState.Location.OriginalString.Split("/").LastOrDefault();
        return route;
    }
}