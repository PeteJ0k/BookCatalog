using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp4;

namespace ConsoleApp4
{
    public class LibraryService
    {
        private readonly AppDbContext _db;

        public LibraryService(AppDbContext db)
        {
            _db = db;
        }

        public void AddPaperBook(string title, string author, int pageCount)
        {
            var book = new PaperBook(pageCount, title, author);
            _db.Books.Add(book);
            _db.SaveChanges();
        }

        public void AddEBook(string title, string author, double fileSize)
        {
            var book = new EBook(fileSize, title, author);
            _db.Books.Add(book);
            _db.SaveChanges();
        }

        public List<Book> GetAllBooks()
        {
            return _db.Books.ToList();
        }

        public void RemoveBook(int id)
        {
            var book = _db.Books.Find(id);
            if (book != null)
            {
                _db.Books.Remove(book);
                _db.SaveChanges();
            }
        }

        public string GetAllBookFormatted()
        {
            var books = _db.Books.ToList();

            if (books.Count == 0)
            {
               return "Список книг пуст";
            }

            var result = $"\nСписок книг\n";
            for (int i = 0; i < books.Count; i++) 
            {
                result += $"{i + 1}, {books[i].GetInfo()}\n";
            }
            return result;
        }
        public int BookCount()
        {
            return _db.Books.Count();
        }

        public List<Book> FindBooksByAuthor(string author)
        {
            return _db.Books
                .Where(b => b.Author.ToLower().Contains(author.ToLower()))
                .ToList();
        }
        public string GetAuthorStats()
        {
            var allBooks = _db.Books.ToList();
            if (allBooks.Count == 0)
                return "Библиотека пуста.";

            var authorGroups = allBooks.GroupBy(b => b.Author);
            var result = "Статистика по авторам\n";
            foreach (var group in authorGroups)
            {
                var titles = group.Select(b => b.Title).ToList();
                result += $"{group.Key}: {group.Count()} книг ({string.Join(", ", titles)})\n";
            }
            return result;
        }
    }
}