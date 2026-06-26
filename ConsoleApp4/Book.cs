using System;

namespace ConsoleApp4
{
    public abstract class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public abstract string GetInfo();

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
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
            return $"[PaperBook] {Title} ({Author}) - Страниц: {PageCount}";
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
            return $"[EBook] {Title} ({Author}) - Размер: {FileSize} МБ";
        }
    }
}