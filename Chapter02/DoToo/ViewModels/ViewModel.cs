namespace DoToo.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public abstract partial class ViewModel : ObservableObject
{
    public INavigation Navigation { get; set; }
}
