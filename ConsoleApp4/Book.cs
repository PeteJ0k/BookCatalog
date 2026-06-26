using System;

namespace ConsoleApp4
{
    public abstract class Book : IBorrowable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; private set; }

        public abstract string GetInfo();

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
            IsBorrowed = false;
        }
        public void Borrow()
        {
            if (IsBorrowed)
            {
                Console.WriteLine($"Книга \"{Title}\" уже выдана.");
            }
            else
            {
                IsBorrowed = true;
                Console.WriteLine($"Книга \"{Title}\" выдана читателю.");
            }
        }

        public void Return()
        {
            if (!IsBorrowed)
            {
                Console.WriteLine($"Книга \"{Title}\" не была выдана.");
            }
            else
            {
                IsBorrowed = false;
                Console.WriteLine($"Книга \"{Title}\" возвращена.");
            }
        }
    }
    

    public class PaperBook : Book 
    {
        public int PageCount { get; set; }

        public PaperBook(int pageCount, string title, string author) : base(title, author)
        {
            PageCount = pageCount;
        }

        public override string GetInfo()
        {
            string status = IsBorrowed ? "Выдана" : "В наличии";
            return $"[PaperBook] {Title} ({Author}) - Страниц: {PageCount}, Статус - {status}";
        }
    }

    public class EBook : Book
    {
        
        public double FileSize { get; set; }

        public EBook(double fileSize, string title, string author) : base(title, author)
        {
            FileSize = fileSize;
        }

        public override string GetInfo()
        {
            string status = IsBorrowed ? "Выдана" : "В наличии";
            return $"[EBook] {Title} ({Author}) - Размер: {FileSize} МБ, Статус - {status}";
        }

    }

}