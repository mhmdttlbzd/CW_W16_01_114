using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookRepository
    {
        private string tableName = "Book";
        private string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Book> GetAll()
        {
            var db = new DataBase(_connectionString);
            var draftUsers = db.Read(tableName);
            var books = new List<Book>();
            foreach (var item in draftUsers)
            {
                books.Add(FillToBook(item));
            }
            return books;
        }
        private Book FillToBook(Dictionary<string, object> data)
        {
            var user = new Book();
            foreach (var item in data)
            {
                if (item.Key == "Id") user.Id = int.Parse(item.Value.ToString());
                else if (item.Key == "Name") user.Name = item.Value.ToString();
                else if (item.Key == "OwnerId") user.OwnerId =Guid.Parse( item.Value.ToString());
            }
            return user;
        }

        public Book GetByIdWithUser(int id)
        {
            var all= GetAll();
            var book = all.FirstOrDefault(x => x.Id == id);
            if (book != null)
            {
                var userRepo = new UserRepository(_connectionString);
                var allUsers = userRepo.GetAll();
                book.Owner = allUsers.FirstOrDefault(x => x.Id == book.OwnerId);
            }
            return book ?? new Book();
        }
    }
}
