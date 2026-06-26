using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2
{
    internal class Program
    {
        static string filePath = "books.txt";

        static void Main(string[] args)
        {
            List<Book> books = LoadBooks();

            while (true)
            {
                Console.WriteLine("\nВведите команду (add/show/file/remove/exit):");
                string result = Console.ReadLine();

                if (result == "add")
                {
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
                        books.Add(paper);
                        SaveBooks(books);
                        Console.WriteLine("Книга добавлена!");
                    }
                    else if (type == "ebook")
                    {
                        double fileSize = GetValidDouble("Введите размер файла (МБ):");
                        EBook ebook = new EBook(fileSize, title, author);
                        books.Add(ebook);
                        SaveBooks(books);
                        Console.WriteLine("Книга добавлена!");
                    }
                    else
                    {
                        Console.WriteLine("Такого типа книги нет.");
                    }
                }
                else if (result == "file")
                {
                    if (File.Exists(filePath))
                    {
                        Console.WriteLine("Содержимое фалйа");
                        using (StreamReader streamReader = new StreamReader(filePath))
                        {
                            string line;
                            int lineNumber = 1;
                            while ((line = streamReader.ReadLine()) != null)

                            {
                                Console.WriteLine($"строка: {lineNumber} : {line}");
                                lineNumber++;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Такогго файла нет");
                    }

                }
                else if (result == "show")
                {
                    // === ПОКАЗ ВСЕХ КНИГ ===
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
                    Console.WriteLine("Список книг");
                    for (int i = 0; i < books.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} {books[i].GetInfo()}");
                    }
                    int booknumber = GetValidInt("Введите номер киниг для удаления");

                    if (booknumber >= 1 && booknumber <= books.Count)
                    {
                        int index = booknumber - 1;
                        string removetitle = books[index].Title;
                        books.RemoveAt(index);
                        SaveBooks(books);
                        Console.WriteLine($"Книга '{removetitle}' удалена");
                    }
                    else
                    {
                        Console.WriteLine("Неверный номер книги");
                    }
                }
                else if (result == "exit")
                {
                    // === ПЕРЕД ВЫХОДОМ — СОХРАНЯЕМ КНИГИ ===
                    SaveBooks(books);
                    Console.WriteLine("Данные сохранены. Программа завершена.");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда.");
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