using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demo020925.Models;
using Microsoft.EntityFrameworkCore;

namespace demo020925;

public partial class ProfileWindow : Window
{
    public ProfileWindow()
    {
        InitializeComponent();
    }
    
    public ProfileWindow(User user)
    {
        InitializeComponent();

        using var ctx = new DatabaseContext();
        var localUser = ctx.Users.FirstOrDefault(x => x.Id == user.Id);

        if (localUser == null)
        {
            NameName.Text = "Пользователь не найден";
            return;
        }
        RegularInfo(localUser);
        if (localUser.RoleId == 2)
        {
            TrainerInfo(localUser);
        }
    }

    public void TrainerInfo(User localUser)
    {
        using var ctx = new DatabaseContext();
        CustomerListBox.IsVisible = true;
        
        var subOrder = ctx.SubOrders.
            Include(x => x.Client).
            Include(x => x.Sub).Where(x => x.TrainerId == localUser.Id).ToList();

        CustomerListBox.ItemsSource = subOrder;

        foreach (var x in subOrder)
        {
            Console.WriteLine(x.Client.Name);
            Console.WriteLine(x.Client.Phone);
            Console.WriteLine(x.Sub.Name);
        }
    }

    public void RegularInfo(User localUser)
    {
        using var ctx = new DatabaseContext();
        NameName.Text = localUser.Name;

        var subOrder = ctx.SubOrders.FirstOrDefault(x => x.ClientId == localUser.Id);
        if (subOrder == null)
        {
            SubscriptionName.Text = "Нет подписки";
            SubscriptionTimeName.Text = "Нет подписки";
            TrainerNameName.Text = "Нет тренера";
            TrainerPhoneName.Text = "Нет тренера";
            return;
        }

        var order = ctx.Orders.FirstOrDefault(x => x.Id == subOrder.OrderId);
        var subscription = ctx.Subscriptions.FirstOrDefault(x => x.Id == subOrder.SubId);

        SubscriptionName.Text = $"Подписка: {subscription?.Name}" ?? "Нет подписки";

        if (order != null)
        {
            var endTime = order.OrderTime.AddMonths(localUser.VisitDurationPerMonts);
            SubscriptionTimeName.Text = endTime > DateTime.Now
                ? $"До {endTime:dd.MM.yyyy}"
                : "Ваша подписка истекла";
        }

        var trainer = ctx.Users.FirstOrDefault(x => x.Id == subOrder.TrainerId);
        TrainerNameName.Text = $"Тренер: {trainer?.Name}";
        TrainerPhoneName.Text = $"Телефон тренера: {trainer?.Phone.ToString()}";
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}