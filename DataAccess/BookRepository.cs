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
            var draftUsers = db.GetAllData(tableName);
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
                if (item.Key == nameof(Book.Id)) user.Id = int.Parse(item.Value.ToString());
                else if (item.Key == nameof(Book.Name)) user.Name = item.Value.ToString();
                else if (item.Key == nameof(Book.Owner)) user.OwnerId =Guid.Parse( item.Value.ToString());
            }
            return user;
        }
        public List<Book> GetBy(string propName,object valueToSearch) { 
            var db = new DataBase(_connectionString);
            var res = new List<Book>();
            foreach(var item in db.GetBy(tableName, propName, valueToSearch))
            {
                res.Add(FillToBook(item));
            }
            return res;
        }



        public Book GetById(int id) { 
            var db = new DataBase(_connectionString);
            return FillToBook( db.GetById(tableName, id));
        }
        public Book GetByIdWithUser(int id)
        {
            var book = GetById(id);
            if (book != null)
            {
                var userRepo = new UserRepository(_connectionString);
                book.Owner = userRepo.GetById(book.OwnerId);
            }
            return book ?? new Book();
        }
    }
}
