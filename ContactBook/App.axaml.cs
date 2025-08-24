using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ContactBook.ViewModels;
using ContactBook.Views;

namespace ContactBook
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var vm = new MainWindowViewModel(); 

                desktop.MainWindow = new MainWindow
                {
                    DataContext = vm,
                };

                await vm.UpdateDataAsync();
            }

            base.OnFrameworkInitializationCompleted();
        }

    }
}