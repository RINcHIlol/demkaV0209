using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo020925.Models;

namespace demo020925;

public partial class AdminWindow : Window
{
    public AdminWindow(User user)
    {
        InitializeComponent();
    }
}