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
    private User localUser;
    public int count = 0;
    
    public CartWindow()
    {
        InitializeComponent();
    }
    public CartWindow(ObservableCollection<MainWindow.SubscriptionPresenter> subs, User user)
    {
        InitializeComponent();
        localUser = user;
        using var ctx = new DatabaseContext();
        DataContext = this;
        var trainers = ctx.Users.Where(u => u.RoleId == 2).ToList();
        var clients = ctx.Users.Where(u => u.RoleId == 3).ToList();
        foreach (var sub in subs)
        {
            sub.Trainers = trainers;
            sub.Clients = clients;
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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new DatabaseContext();

        var price = cart.Sum(sub => sub.Price);

        var order = new Order
        {
            Price = price,
            OrderTime = DateTime.Now,
            CustomerId = localUser.Id
        };

        ctx.Orders.Add(order);
        ctx.SaveChanges();

        foreach (var sub in cart)
        {
            ctx.SubOrders.Add(new SubOrder
            {
                OrderId = order.Id,
                SubId = sub.Id,
                TrainerId = sub.SelectedTrainer?.Id,
                ClientId = sub.SelectedClient?.Id,
            });
            ctx.Users.Update(new User()
            {
                Id = sub.SelectedClient.Id,
                Name = sub.SelectedClient.Name,
                RoleId = sub.SelectedClient.RoleId,
                CurrentSubsriptionId = sub.Id,
                VisitDurationPerMonts = sub.DurationPerMonths,
                Discount = sub.SelectedClient.Discount,
                Login = sub.SelectedClient.Login,
                Password = sub.SelectedClient.Password,
                Phone = sub.SelectedClient.Phone,
            });
        }

        ctx.SaveChanges();
        cart.Clear();
        count = 0;

        Close(count);
    }
}