using Avalonia.ReactiveUI;
using ContactBook.ViewModels;
using ReactiveUI;

namespace ContactBook.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(ViewModel, vm => vm.FirstName, view => view.txtFirstName.Text));
                d(this.Bind(ViewModel, vm => vm.LastName, view => view.txtLastName.Text));
                d(this.Bind(ViewModel,vm => vm.Address,view => view.txtAddress.Text));
                d(this.Bind(ViewModel,vm => vm.Email,view => view.txtEmail.Text));  
                d(this.Bind(ViewModel,vm => vm.Phone,view => view.txtPhone.Text));
                d(this.Bind(ViewModel, vm => vm.Age, view => view.txtAge.Text));

                d(this.OneWayBind(ViewModel, vm => vm.ErrorMessage, view => view.txtError.Text));

                d(this.OneWayBind(ViewModel, vm => vm.Contacts, view => view.listBoxContacts.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.SelectedContact, view => view.listBoxContacts.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.NewContactCmd, view => view.btnNew));
                d(this.BindCommand(ViewModel, vm => vm.InsertContactCmd, view => view.btnInsert));
                d(this.BindCommand(ViewModel,vm => vm.UpdateContactCmd, view => view.btnUpdate));
                d(this.BindCommand(ViewModel,vm => vm.DeleteContactCmd, view => view.btnDelete));
                d(this.BindCommand(ViewModel, vm => vm.ClearContactsCmd, view => view.btnClear));
            });
        }
    }
}