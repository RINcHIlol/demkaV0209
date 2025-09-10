using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using demo020925.Models;

namespace demo020925;

public partial class AuthWindow : Window
{
    private bool _isPasswordVisible = false;
    public AuthWindow()
    {
        InitializeComponent();
        PasswordName.PasswordChar = '•';
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        var login = LoginName.Text;
        var password = PasswordName.Text;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage.Text = "Поля не должны быть пустыми!";
            return;
        }

        var ctx = new DatabaseContext();

        var user = ctx.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        
        
        if (user == null)
        {
            ErrorMessage.Text = "Вы ввели неверный логин или пароль. Пожалуйста, проверьте ещё раз данные";
            return;
        }
        
        ErrorMessage.Text = "";
        
        // switch (user.RoleId)
        // {
        //     case 1:
        //         AdminWindow studentWindow = new AdminWindow();
        //         studentWindow.ShowDialog(this);
        //         break;
        //     case 2:
        //         TrainerWindow teacherWindow = new TrainerWindow();
        //         teacherWindow.ShowDialog(this);
        //         break;
        //     case 3:
        //         CustomerWindow adminWindow = new CustomerWindow();
        //         adminWindow.ShowDialog(this);
        //         break;
        // }
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