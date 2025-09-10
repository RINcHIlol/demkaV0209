using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demo020925.Models;

namespace demo020925;

public partial class RegWindow : Window
{
    private bool _isPasswordVisible = false;
    public RegWindow()
    {
        InitializeComponent();
        PasswordName.PasswordChar = '•';
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var login = LoginName.Text;
        var password = PasswordName.Text;
        var name = NameName.Text;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(name))
        {
            ErrorMessage.Text = "Поля не должны быть пустыми!";
            return;
        }
        
        if (name.Any(char.IsDigit))
        {
            ErrorMessage.Text = "Имя не должно содержать цифры!";
            return;
        }

        var ctx = new DatabaseContext();

        var user = new User()
        {
            Login = login,
            Password = password,
            Name = name,
            RoleId = 3,
            VisitDurationPerMonts = 0,
            Discount = 0,
            CurrentSubsriptionId = null
        };

        var oldUser = ctx.Users.FirstOrDefault(u => u.Login == login);
        if (oldUser != null)
        {
            ErrorMessage.Text = "Такой логин уже существует!";
            return;
        }
        
        ctx.Users.Add(user);
        ctx.SaveChanges();
        ErrorMessage.Text = "";
        Close(user);
    }

    private void TogglePasswordVisibilityClick(object? sender, RoutedEventArgs e)
    {
        _isPasswordVisible = !_isPasswordVisible;

        if (_isPasswordVisible)
        {
            PasswordName.PasswordChar = '\0';
            TogglePasswordVisibility.Content = "Hide";
        }
        else
        {
            PasswordName.PasswordChar = '•';
            TogglePasswordVisibility.Content = "Show";
        }
    }
}