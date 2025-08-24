using ContactBook.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactBook.DataAccess
{
    public class ContactRepository
    {
        private readonly string connStr;

        public ContactRepository()
        {
            connStr = "Insert your connection string";
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            using var con = new SqlConnection(connStr);
            var cmd = "SELECT * FROM dbo.Contacts;";
            var contacts = await con.QueryAsync<Contact>(cmd);
            return contacts;
        }

        public async Task<int> InsertAsync(Contact? contact)
        {
            using var con = new SqlConnection(connStr);
            var cmd = "INSERT INTO dbo.Contacts(FirstName,LastName,Address,Email,Phone,Age) VALUES(@FirstName,@LastName,@Address,@Email,@Phone,@Age);";
            int rowsAffected = await con.ExecuteAsync(cmd, contact);
            return rowsAffected; 
        }

        public async Task<int> UpdateAsync(Contact? contact)
        {
            using var con = new SqlConnection(connStr);
            var cmd = "UPDATE dbo.Contacts SET FirstName=@FirstName,LastName=@LastName,Address=@Address,Email=@Email,Phone=@Phone,Age=@Age WHERE ID=@ID;";
            int rowsAffected = await con.ExecuteAsync(cmd, contact);
            return rowsAffected;
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var con = new SqlConnection(connStr);
            var cmd = "DELETE FROM dbo.Contacts WHERE ID=@ID;";
            int rowsAffected = await con.ExecuteAsync(cmd,new { id });
            return rowsAffected; 
        }

        public async Task<int> ClearAsync()
        {
            using var con = new SqlConnection(connStr);
            var cmd = "DELETE FROM dbo.Contacts;";
            int rowsAffected = await con.ExecuteAsync(cmd);
            return rowsAffected;
        }
    }
}
