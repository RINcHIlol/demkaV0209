using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo020925.Models;

namespace demo020925;

public partial class TrainerWindow : Window
{
    public TrainerWindow(User user)
    {
        InitializeComponent();
    }
}