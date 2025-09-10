using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demo020925.Models;

namespace demo020925;
public partial class CartWindow : Window
{
    private ObservableCollection<MainWindow.SubscriptionPresenter> cart;
    public int count = 0;
    
    public CartWindow()
    {
        InitializeComponent();
    }
    public CartWindow(ObservableCollection<MainWindow.SubscriptionPresenter> subs)
    {
        InitializeComponent();
        using var ctx = new DatabaseContext();
        DataContext = this;
        var trainers = ctx.Users
            .Where(u => u.RoleId == 2)
            .ToList();
        foreach (var sub in subs)
        {
            sub.Trainers = trainers;
        }
        
        cart = subs;
        SubsListBox.ItemsSource = cart;
    }
    
    private void DeleteFromCart_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is MainWindow.SubscriptionPresenter sub)
        {
            sub.IsInCart = false;
            cart.Remove(sub);
            count++;
        }
    }
    
    private void AddToCart_OnClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.DataContext is MainWindow.SubscriptionPresenter sub)
        {
            sub.IsInCart = true;
            cart.Add(sub);
            count--;
        }
    }

    private void MainWindowButton_OnClick_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(count);
    }
}