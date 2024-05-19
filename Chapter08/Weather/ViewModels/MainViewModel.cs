﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Weather.Models;
using Weather.Services;

namespace Weather.ViewModels;

public partial class MainViewModel(IWeatherService weatherService) : ViewModel
{
    private readonly IWeatherService weatherService = weatherService;

    [ObservableProperty]
    private string city;

    [ObservableProperty]
    private ObservableCollection<ForecastGroup> days;

    [ObservableProperty]
    private bool isRefreshing;

    [RelayCommand]
    public async Task RefreshAsync()
    {
        await LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        IsRefreshing = true;

        var status = await AppPermissions.CheckAndRequestRequiredPermissionAsync();
        if (status == PermissionStatus.Granted)
        {
            var location = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();
            var forecast = await weatherService.GetForecastAsync(location.Latitude, location.Longitude);
            var itemGroups = new List<ForecastGroup>();

            foreach (var item in forecast.Items)
            {
                if (itemGroups.Count == 0)
                {
                    itemGroups.Add(new ForecastGroup(new List<ForecastItem>() { item })
                    {
                        Date = item.DateTime.Date
                    });
                    continue;
                }

                var group = itemGroups.SingleOrDefault(x => x.Date == item.DateTime.Date);

                if (group == null)
                {
                    itemGroups.Add(new ForecastGroup(new List<ForecastItem>() { item })
                    {
                        Date = item.DateTime.Date
                    });

                    continue;
                }

                group.Items.Add(item);
            }

            Days = new ObservableCollection<ForecastGroup>(itemGroups);
            City = forecast.City;
        }

        IsRefreshing = false;
    }
}
