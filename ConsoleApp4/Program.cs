using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleApp4;

namespace ConsoleApp4
{
    internal class Program
    {
        static string filePath = "books.txt";

        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                while (true)
                {
                    Console.WriteLine("\nВведите команду (add/show/remove/exit):");
                    string result = Console.ReadLine();

                    if (result == "add")
                    {
                        // Логика добавления книги в базу данных
                        Console.WriteLine("Введите тип книги (paperbook/ebook):");
                        string type = Console.ReadLine();

                        Console.WriteLine("Введите название книги:");
                        string title = Console.ReadLine();

                        Console.WriteLine("Введите автора книги:");
                        string author = Console.ReadLine();

                        if (type == "paperbook")
                        {
                            int pageCount = GetValidInt("Введите количество страниц:");
                            PaperBook paper = new PaperBook(pageCount, title, author);
                            db.Books.Add(paper);
                            db.SaveChanges();
                            Console.WriteLine("Книга добавлена!");
                        }
                        else if (type == "ebook")
                        {
                            double fileSize = GetValidDouble("Введите размер файла (МБ):");
                            EBook ebook = new EBook(fileSize, title, author);
                            db.Books.Add(ebook);
                            db.SaveChanges();
                            Console.WriteLine("Книга добавлена!");
                        }
                        else
                        {
                            Console.WriteLine("Такого типа книги нет.");
                        }
                    }
                    else if (result == "show")
                    {
                        // Чтение всех книг из базы данных
                        var books = db.Books.ToList();

                        if (books.Count == 0)
                        {
                            Console.WriteLine("Список книг пуст.");
                        }
                        else
                        {
                            Console.WriteLine("\n=== Список книг ===");
                            for (int i = 0; i < books.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {books[i].GetInfo()}");
                            }
                        }
                    }
                    else if (result == "remove")
                    {
                        // Логика удаления книги
                        var books = db.Books.ToList();

                        if (books.Count == 0)
                        {
                            Console.WriteLine("Список книг пуст. Нечего удалять.");
                            continue;
                        }

                        Console.WriteLine("\n=== Список книг ===");
                        for (int i = 0; i < books.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {books[i].GetInfo()}");
                        }

                        int booknumber = GetValidInt("Введите номер книги для удаления:");

                        if (booknumber >= 1 && booknumber <= books.Count)
                        {
                            var bookToRemove = books[booknumber - 1];
                            db.Books.Remove(bookToRemove);
                            db.SaveChanges();
                            Console.WriteLine($"Книга удалена.");
                        }
                        else
                        {
                            Console.WriteLine("Неверный номер.");
                        }
                    }
                    else if (result == "exit")
                    {
                        Console.WriteLine("Программа завершена.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неизвестная команда.");
                    }
                }
            }
        }
        static int GetValidInt(string message)
        {
            int result = 0;
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                try
                {
                    result = int.Parse(input);
                    if (result <= 0)
                    {
                        Console.WriteLine("Число должно быть больше 0.");
                        continue;
                    }
                    isValid = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка! Введите целое число.");
                }
            }
            return result;
        }

        static double GetValidDouble(string message)
        {
            double result = 0;
            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                try
                {
                    result = double.Parse(input);
                    if (result <= 0)
                    {
                        Console.WriteLine("Число должно быть больше 0.");
                        continue;
                    }
                    isValid = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка! Введите число (например, 2.5).");
                }
            }
            return result;
        }
        static void SaveBooks(List<Book> books)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Book book in books)
                {
                    if (book is PaperBook paper)
                    {
                        writer.WriteLine($"PaperBook|{paper.Title}|{paper.Author}|{paper.PageCount}");
                    }
                    else if (book is EBook ebook)
                    {
                        writer.WriteLine($"EBook|{ebook.Title}|{ebook.Author}|{ebook.FileSize}");
                    }
                }
            }
        }

        static List<Book> LoadBooks()
        {
            List<Book> books = new List<Book>();

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length < 4) continue;

                        string type = parts[0];
                        string title = parts[1];
                        string author = parts[2];

                        if (type == "PaperBook")
                        {
                            int pageCount = int.Parse(parts[3]);
                            books.Add(new PaperBook(pageCount, title, author));
                        }
                        else if (type == "EBook")
                        {
                            double fileSize = double.Parse(parts[3]);
                            books.Add(new EBook(fileSize, title, author));
                        }
                    }
                }
            }
            return books;
        }
    }
}