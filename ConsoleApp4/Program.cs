using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
                var library = new LibraryService(db);

                while (true)
                {
                    Console.WriteLine("\nВведите команду (add/show/remove/find/count/stats/borrow/return/exit):");
                    string result = Console.ReadLine();

                    switch (result)
                    {
                        case "add":
                            Console.WriteLine("Введите тип книги (paperbook/ebook):");
                            string type = Console.ReadLine();

                            Console.WriteLine("Введите название книги:");
                            string title = Console.ReadLine();

                            Console.WriteLine("Введите автора книги:");
                            string author = Console.ReadLine();

                            if (type == "paperbook")
                            {
                                int pageCount = GetValidInt("Введите количество страниц:");
                                library.AddPaperBook(title, author, pageCount);
                                Console.WriteLine("Книга добавлена!");
                            }
                            else if (type == "ebook")
                            {
                                double fileSize = GetValidDouble("Введите размер файла (МБ):");
                                library.AddEBook(title, author, fileSize);
                                Console.WriteLine("Книга добавлена!");
                            }
                            else
                            {
                                Console.WriteLine("Такого типа книги нет.");
                            }
                            break;

                        case "show":

                            Console.WriteLine(library.GetAllBookFormatted());
                            break;

                        case "remove":
                            var allBooks = library.GetAllBooks();
                            if (allBooks.Count == 0)
                            {
                                Console.WriteLine("Список книг пуст. Нечего удалять.");
                                break;
                            }

                            Console.WriteLine("\n=== Список книг ===");
                            for (int i = 0; i < allBooks.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {allBooks[i].GetInfo()}");
                            }

                            int bookNumber = GetValidInt("Введите номер книги для удаления:");
                            if (bookNumber >= 1 && bookNumber <= allBooks.Count)
                            {
                                library.RemoveBook(allBooks[bookNumber - 1].Id);
                                Console.WriteLine("Книга удалена.");
                            }
                            else
                            {
                                Console.WriteLine("Неверный номер.");
                            }
                            break;

                        case "find":
                            Console.WriteLine("Введите автора для поиска его книг:");
                            string findAuthor = Console.ReadLine();

                            var foundBooks = library.FindBooksByAuthor(findAuthor);
                            if (foundBooks.Count == 0)
                            {
                                Console.WriteLine("Книг с таким автором не найдено.");
                            }
                            else
                            {
                                Console.WriteLine($"Найдено книг с таким автором: {foundBooks.Count}");
                                for (int i = 0; i < foundBooks.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1}. {foundBooks[i].GetInfo()}");
                                }
                            }
                            break;

                        case "count":
                            Console.WriteLine($"Всего книг в библиотеке: {library.BookCount()}");
                            break;
                        case "stats":
                            Console.WriteLine(library.GetAuthorStats());
                            break;
                        case "borrow":
                            Console.WriteLine($"{library.GetAllBookFormatted()}");
                            int borrowNumber = GetValidInt("Введите номер книги для выдачи");
                            var allbooks = library.GetAllBooks();
                            if (borrowNumber >= 1 && borrowNumber <= allbooks.Count)
                            {
                                allbooks[borrowNumber - 1].Borrow();
                                db.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("Такого номера книги не существует");
                            }
                            break;
                        case "return":
                            Console.WriteLine(library.GetAllBookFormatted());
                            int returnNumber = GetValidInt("Введите номер книги для возврата");
                            var allbook = library.GetAllBooks();
                            if (returnNumber >= 1 && returnNumber <= allbook.Count)
                            {
                                allbook[returnNumber - 1].Return();
                                db.SaveChanges();
                            }
                            else
                            {
                                Console.WriteLine("Такой книги не существует");
                            }

                                break;
                        case "exit":
                            Console.WriteLine("Программа завершена.");
                            return;

                        default:
                            Console.WriteLine("Неизвестная команда.");
                            break;
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
    }
}