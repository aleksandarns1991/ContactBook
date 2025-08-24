using ContactBook.DataAccess;
using ContactBook.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContactBook.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive]
        public IEnumerable<Contact> Contacts { get; set; } = Enumerable.Empty<Contact>();
        [Reactive]
        public Contact? SelectedContact { get; set; }

        public int ID { get; set; }
        [Reactive]
        public string? FirstName { get; set; }
        [Reactive]
        public string? LastName { get; set; }
        [Reactive]
        public string? Address { get; set; }
        [Reactive]
        public string? Email { get; set; }
        [Reactive]
        public string? Phone { get; set; }
        [Reactive]
        public int Age { get; set; }

        [Reactive]
        public string? ErrorMessage { get; set; }

        public ReactiveCommand<Unit, Unit>? NewContactCmd { get; set; }
        public ReactiveCommand<Unit, Unit>? InsertContactCmd { get; set; }
        public ReactiveCommand<Unit, Unit>? UpdateContactCmd { get; set; }
        public ReactiveCommand<Unit, Unit>? DeleteContactCmd { get; set; }
        public ReactiveCommand<Unit, Unit>? ClearContactsCmd { get; set; }

        private readonly ContactRepository contactRepository = new();

        public MainWindowViewModel()
        {
            // Create commands and set their state
            CreateCommands();
            // Handle command exceptions
            SetCommandErrors();
            // Check selected contact when changed
            ObserveSelectedContact();
        }

        public async Task UpdateDataAsync()
        {
            try
            {
                Contacts = await contactRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message; // Or set your error message
            }
        }

        private void CreateCommands()
        {
            const string nameRegex = "^[a-zA-Z]+$";
            const string phoneRegex = "^[0-9]+$";

            var canInsert = this.WhenAnyValue(x => x.FirstName, x => x.LastName, x => x.Address, x => x.Phone, x => x.Age,x => x.SelectedContact,
                                             (firstName, lastName, address, phone, age,contact) =>
                                             Regex.IsMatch(firstName ?? string.Empty, nameRegex)
                                          && Regex.IsMatch(lastName ?? string.Empty, nameRegex)
                                          && Regex.IsMatch(phone ?? string.Empty, phoneRegex)
                                          && !string.IsNullOrEmpty(address)
                                          && age > 0
                                          && contact == null);

            var canUpdate = this.WhenAnyValue(x => x.FirstName, x => x.LastName, x => x.Address, x => x.Phone, x => x.Age, x => x.SelectedContact,
                                             (firstName, lastName, address, phone, age, contact) =>
                                             Regex.IsMatch(firstName ?? string.Empty, nameRegex)
                                          && Regex.IsMatch(lastName ?? string.Empty, nameRegex)
                                          && Regex.IsMatch(phone ?? string.Empty, phoneRegex)
                                          && !string.IsNullOrEmpty(address)
                                          && age > 0
                                          && contact != null);

            var canDelete = this.WhenAnyValue(x => x.SelectedContact).Select(x => x != null);
            var canClear = this.WhenAnyValue(x => x.Contacts).Select(x => x.Any());

            NewContactCmd = ReactiveCommand.Create(NewContact);
            InsertContactCmd = ReactiveCommand.CreateFromTask(InsertContactAsync, canInsert);
            UpdateContactCmd = ReactiveCommand.CreateFromTask(UpdateContactAsync,canUpdate);
            DeleteContactCmd = ReactiveCommand.CreateFromTask(DeleteContactAsync, canDelete);
            ClearContactsCmd = ReactiveCommand.CreateFromTask(ClearContactsAsync, canClear);
        }

        private void SetCommandErrors()
        {
            InsertContactCmd!.ThrownExceptions.Subscribe(_ => { ErrorMessage = "Insert operation has failed."; });
            UpdateContactCmd!.ThrownExceptions.Subscribe(_ => { ErrorMessage = "Update operation has failed."; });
            DeleteContactCmd!.ThrownExceptions.Subscribe(_ => { ErrorMessage = "Delete operation has failed."; });
            ClearContactsCmd!.ThrownExceptions.Subscribe(_ => { ErrorMessage = "Clear operation has failed."; });
        }

        private void ObserveSelectedContact()
        {
            this.WhenAnyValue(x => x.SelectedContact).Subscribe(_ =>
            {
                if (SelectedContact != null)
                {
                    ID = SelectedContact.ID;
                    FirstName = SelectedContact.FirstName;
                    LastName = SelectedContact.LastName;
                    Address = SelectedContact.Address;
                    Email = SelectedContact.Email;
                    Phone = SelectedContact.Phone;
                    Age = SelectedContact.Age;
                }
                else
                {
                    ClearFields();
                }
            });
        }

        private void ClearFields()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty; 
            Email = string.Empty;
            Phone = string.Empty;
            Age = 0;
        }

        private void NewContact()
        {
            if (SelectedContact != null)
            {
                SelectedContact = null;
            }
            else
            {
                ClearFields();
            }
        }

        private async Task InsertContactAsync()
        {
            int rowsAffected = await contactRepository.InsertAsync(CreateContact());

            if (rowsAffected > 0)
            {
                await UpdateDataAsync();
            }
        }

        private async Task UpdateContactAsync()
        {
            int rowsAffected = await contactRepository.UpdateAsync(CreateContact(SelectedContact!.ID));

            if (rowsAffected > 0)
            {
                await UpdateDataAsync();
            }
        }

        private async Task DeleteContactAsync()
        {
            int rowsAffected = await contactRepository.DeleteAsync(SelectedContact!.ID);

            if (rowsAffected > 0)
            {
                await UpdateDataAsync();
            }
        }

        private async Task ClearContactsAsync()
        {
            int rowsAffected = await contactRepository.ClearAsync();

            if (rowsAffected > 0)
            {
                await UpdateDataAsync();
            }
        }

        private Contact? CreateContact(int id = 0)
        {
            return new Contact
            {
                ID = id,
                FirstName = FirstName,
                LastName = LastName,
                Address = Address,
                Email = Email,
                Phone = Phone,
                Age = Age,
            };
        }
    }
}