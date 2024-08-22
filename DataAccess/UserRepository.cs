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
            var draftUsers =  db.GetAllData(tableName);
            var users = new List<User>();
            foreach (var item in draftUsers) { 
                users.Add(FillToUser(item));
            }
            return users;
        }
        public User GetById(Guid id) {
            var db = new DataBase(_connectionString);
            return FillToUser(db.GetById(tableName, id));
        }
        private User FillToUser(Dictionary<string, object> data) {
            var user = new User();
            foreach (var item in data) { 
                if (item.Key == "BookId") user.Id = Guid.Parse(item.Value.ToString());
                else if (item.Key == "Name") user.Name = item.Value.ToString();
                else if (item.Key == "LastName") user.LastName = item.Value.ToString();
            }
            return user;
        }
        private Dictionary<string, object> FillToData(User user) {
            var data = new Dictionary<string, object>();
            data.Add(nameof(user.Id), user.Id);
            data.Add(nameof(user.Name), user.Name);
            data.Add(nameof(user.LastName), user.LastName);
            return data;
        }
        public User GetByIdWhitBooks(Guid id) { 
            
            var user = GetById(id);
            if (user != null) {
                var bookRepo = new BookRepository(_connectionString);
                
                user.Books = bookRepo.GetBy("OwnerId",user.Id);
            }
            return user ?? new User();
        }
        public void Write(User user)
        {
            var data = FillToData(user);
            var db = new DataBase(_connectionString);
            db.WriteOne(data,tableName);
        }
        public void WriteAll(List<User> users)
        {
            var data = new List<Dictionary<string, object>>();
            foreach (var user in users) { 
                data.Add(FillToData(user));
            }
            var db = new DataBase(_connectionString);
            db.WriteAll(data, tableName);
        }
    }
}
