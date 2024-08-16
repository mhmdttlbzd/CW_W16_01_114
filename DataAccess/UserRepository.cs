using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserRepository
    {
        private string tableName = "Users1";
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetAll()
        {
            var db = new DataBase(_connectionString);
            var draftUsers =  db.Read(tableName);
            var users = new List<User>();
            foreach (var item in draftUsers) { 
                users.Add(FillToUser(item));
            }
            return users;
        }
        private User FillToUser(Dictionary<string, object> data) {
            var user = new User();
            foreach (var item in data) { 
                if (item.Key == "Id") user.Id = Guid.Parse(item.Value.ToString());
                else if (item.Key == "Name") user.Name = item.Value.ToString();
                else if (item.Key == "LastName") user.LastName = item.Value.ToString();
            }
            return user;
        }
        public User GetByIdWhitBooks(Guid id) { 
            var all = GetAll();
            var user = all.FirstOrDefault(x => x.Id == id);
            if (user != null) {
                var bookRepo = new BookRepository(_connectionString);
                var allBooks = bookRepo.GetAll();
                user.Books = allBooks.Where(x => x.OwnerId == user.Id).ToList();
            }
            return user ?? new User();
        }
    }
}
