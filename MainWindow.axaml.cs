using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using demo020925.Models;

namespace demo020925;

public partial class MainWindow : Window
{
    ObservableCollection<SubscriptionPresenter> subscriptions = new ObservableCollection<SubscriptionPresenter>();
    List<SubscriptionPresenter> dataSourceSubscriptions;
    private int currentPage = 1;
    private int itemsPerPage = 6;
    public User user;
    public ObservableCollection<SubscriptionPresenter> SubsInCart { get; set; } = new();
    public List<string> MonthsList { get; } = new List<string>{"Все", "1", "3", "6", "12"};
    public List<string> TypeSubList { get; } = new List<string>();
    public List<string> CostList { get; } = new List<string>{"Сброс", "Цена убывание", "Цена возрастание"};
    public MainWindow()
    {
        InitializeComponent();
        using var ctx = new DatabaseContext();
        dataSourceSubscriptions = ctx.Subscriptions.Select(sub => new SubscriptionPresenter()
        {
            Id = sub.Id,
            Name = sub.Name,
            Description = sub.Description,
            Price = sub.Price,
            DurationPerMonths = sub.DurationPerMonths,
            IsInCart = false,
            CategoryId = sub.CategoryId,
        }).ToList();
        SubsListBox.ItemsSource = subscriptions;
        
        MonthsComboBox.ItemsSource = MonthsList;
        TypeSubList = ctx.SubscriptionsCategories.Select(it => it.Name).ToList();
        TypeSubList.Insert(0, "Все");
        TypeSubComboBox.ItemsSource = TypeSubList;
        CostComboBox.ItemsSource = CostList;
        DisplaySubscriptions();
    }

    public class SubscriptionPresenter() : Subscription
    {
        public bool IsInCart { get; set; }

        public string IsInCartStr
        {
            get => IsInCart ? "В корзине" : "Купить";
        }
        
        public List<User> Trainers { get; set; } = new List<User>();
        public bool HasTrainers => CategoryId == 6 || CategoryId == 7 || CategoryId == 8;
    }
    
    public void DisplaySubscriptions()
    {
        using var ctx = new DatabaseContext();
        var temp = dataSourceSubscriptions;
        subscriptions.Clear();
        
        if (MonthsComboBox.SelectedItem != null && int.TryParse(MonthsComboBox.SelectedItem.ToString(), out var month))
        {
            temp = temp.Where(x => x.DurationPerMonths == month).ToList();
        }

        if (TypeSubComboBox.SelectedItem != null && TypeSubComboBox.SelectedItem.ToString() != "Все")
        {
            var category = ctx.SubscriptionsCategories.FirstOrDefault(x => x.Name == TypeSubComboBox.SelectedItem.ToString());
            temp = temp.Where(x => x.CategoryId == category.Id).ToList();
        }
        
        switch (CostComboBox.SelectedIndex)
        {
            case 2: temp = temp.OrderBy(it => it.Price).ToList(); break;
            case 1: temp = temp.OrderByDescending(it => it.Price).ToList(); break;
            case 0: temp = dataSourceSubscriptions; break;
            default: break;
        }
        
        if (!string.IsNullOrEmpty(SearchTextBox.Text))
        {
            var search = SearchTextBox.Text;
            temp = temp.Where(it => IsContains(it.Name, it.Description, search)).ToList();
        }
        
        int totalItems = temp.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

        if (currentPage > totalPages)
        {
            currentPage = totalPages;
        }
        if (currentPage < 1)
        {
            currentPage = 1;
        }

        var paginatedList = temp.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();

        foreach (var item in paginatedList)
        {
            subscriptions.Add(item);
        }
    }
    
    public bool IsContains(string title, string? description, string search)
    {
        string desc = string.Empty;
        if(description != null) desc = description;
        string message = (title + desc).ToLower();
        search = search.ToLower();
        return message.Contains(search);
    }
    
    private void ComboBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        DisplaySubscriptions();
    }
    
    private void SearchTextBox_OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        DisplaySubscriptions();
    }

    
    private void NextPage(object? sender, RoutedEventArgs e)
    {
        currentPage++;
        DisplaySubscriptions();
    }

    private void PreviousPage(object? sender, RoutedEventArgs e)
    {
        currentPage--;
        DisplaySubscriptions();
    }
    
    private async void Auth_OnClick(object? sender, RoutedEventArgs e)
    {
        AuthWindow authWindow = new AuthWindow();
        user = await authWindow.ShowDialog<User>(this);
        Console.WriteLine($"idUser: {user.Id}");

        UserNameTextBlock.IsVisible = true;
        UserNameTextBlock.Text = user.Name;
        RegButton.IsVisible = false;
        AuthButton.IsVisible = false;
        LogoutButton.IsVisible = true;
        CartButton.IsVisible = true;
        switch (user.RoleId)
        {
            case 1: UserPostTextBlock.Text = "Admin"; break;
            case 2: UserPostTextBlock.Text = "Trainer"; break;
            case 3: UserPostTextBlock.Text = "Customer"; break;
        }
        UserPostTextBlock.IsVisible = true;
    }
    
    private async void Reg_OnClick(object? sender, RoutedEventArgs e)
    {
        RegWindow regWindow = new RegWindow();
        user = await regWindow.ShowDialog<User>(this);
        Console.WriteLine($"idUser: {user.Id}");

        UserNameTextBlock.IsVisible = true;
        UserNameTextBlock.Text = user.Name;
        RegButton.IsVisible = false;
        AuthButton.IsVisible = false;
        LogoutButton.IsVisible = true;
        CartButton.IsVisible = true;
        switch (user.RoleId)
        {
            case 1: UserPostTextBlock.Text = "Admin"; break;
            case 2: UserPostTextBlock.Text = "Trainer"; break;
            case 3: UserPostTextBlock.Text = "Customer"; break;
        }
        UserPostTextBlock.IsVisible = true;
    }
    
    private void Logout_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new DatabaseContext();
        user = null;
        UserNameTextBlock.Text = string.Empty;
        UserPostTextBlock.Text = "Guest";
        CartButton.Content = "Корзина";

        RegButton.IsVisible = true;
        AuthButton.IsVisible = true;

        CartButton.IsVisible = false;
        LogoutButton.IsVisible = false;
        UserNameTextBlock.IsVisible = false;
        
        SubsInCart.Clear();

        dataSourceSubscriptions = ctx.Subscriptions.Select(sub => new SubscriptionPresenter()
        {
            Id = sub.Id,
            Name = sub.Name,
            Description = sub.Description,
            Price = sub.Price,
            DurationPerMonths = sub.DurationPerMonths,
            IsInCart = false
        }).ToList();
        DisplaySubscriptions();
    }
    
    private void AddToCartButton_OnClick(object? sender, RoutedEventArgs e)
    {
        if (user != null)
        {
            if (sender is Button btn && btn.DataContext is SubscriptionPresenter selectedSub)
            {
                if (user.RoleId == 3 && selectedSub.CategoryId == 8)
                {
                    return;
                }
                if (selectedSub.IsInCart)
                {
                    selectedSub.IsInCart = false;
                    SubsInCart.Remove(selectedSub);
                }
                else
                {
                    selectedSub.IsInCart = true;
                    SubsInCart.Add(selectedSub);
                }

                CartButton.Content = SubsInCart.Count > 0
                    ? $"Корзина ({SubsInCart.Count})"
                    : "Корзина";
            }
        }
        DisplaySubscriptions();
    }
    
    private async void CartButton_OnClick(object? sender, RoutedEventArgs e)
    {
        CartWindow cartWindow = new CartWindow(SubsInCart);
        await cartWindow.ShowDialog<int>(this);
        DisplaySubscriptions();
        CartButton.Content = SubsInCart.Count > 0
            ? $"Корзина ({SubsInCart.Count})"
            : "Корзина";
    }
}