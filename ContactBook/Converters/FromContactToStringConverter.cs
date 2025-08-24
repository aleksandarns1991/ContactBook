using Avalonia.Data.Converters;
using ContactBook.Models;
using System;
using System.Globalization;

namespace ContactBook.Converters
{
    public class FromContactToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var contact = value as Contact;

            if (contact != null)
            {
                return $"{contact.FirstName} {contact.LastName} \n"
                     + $"{contact.Address} \n"
                     + $"{contact.Age}";
            }

            return string.Empty; 
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
