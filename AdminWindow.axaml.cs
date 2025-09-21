using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demo020925.Models;

namespace demo020925;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
    }

    private void Add_OnClick(object? sender, RoutedEventArgs e)
    {
        using var ctx = new DatabaseContext();
        
        var name = NameTextBox.Text;
        var login = LoginTextBox.Text;
        var password = PasswordTextBox.Text;
        var phone = PhoneTextBox.Text;
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(phone))
        {
            ErrorTextBlock.IsVisible = true;
            ErrorTextBlock.Text = "Заполните все поля";
            return;
        }

        if (phone.Length != 11 || !long.TryParse(phone, out long phoneInt))
        {
            ErrorTextBlock.IsVisible = true;
            ErrorTextBlock.Text = "Телефон должен состоять из 11 цифр";
            return;
        }
        
        User newTrainer = new User()
        {
            Name = name,
            Login = login,
            Password = password,
            Phone = phoneInt,
            RoleId = 2,
            CurrentSubsriptionId = null,
            VisitDurationPerMonts = 0,
            Discount = 0
        };

        ctx.Users.Add(newTrainer);
        ctx.SaveChanges();
        Close();
    }
    
    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}