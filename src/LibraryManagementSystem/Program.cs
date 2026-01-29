using LibraryManagementSystem.Core.Repositories;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.Services;

// Composition root: wire up dependencies
IBookRepository bookRepository = new InMemoryBookRepository();
IBookService bookService = new BookService(bookRepository);

Console.WriteLine("=== Library Management System ===");
Console.WriteLine();

while (true)
{
    ShowMenu();
    var choice = Console.ReadLine()?.Trim();

    switch (choice)
    {
        case "1":
            AddBook(bookService);
            break;
        case "2":
            UpdateBook(bookService);
            break;
        case "3":
            DeleteBook(bookService);
            break;
        case "4":
            ListAllBooks(bookService);
            break;
        case "5":
            ViewBookDetails(bookService);
            break;
        case "6":
            Console.WriteLine("Goodbye.");
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }

    Console.WriteLine();
}

static void ShowMenu()
{
    Console.WriteLine("1. Add a new book");
    Console.WriteLine("2. Update an existing book");
    Console.WriteLine("3. Delete a book");
    Console.WriteLine("4. List all books");
    Console.WriteLine("5. View details of a specific book");
    Console.WriteLine("6. Exit");
    Console.Write("Enter your choice (1-6): ");
}

static void AddBook(IBookService bookService)
{
    Console.Write("ISBN (13 digits): ");
    var isbn = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Title: ");
    var title = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Author: ");
    var author = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Year published (optional, press Enter to skip): ");
    var yearInput = Console.ReadLine()?.Trim();
    int? year = null;
    if (!string.IsNullOrEmpty(yearInput) && int.TryParse(yearInput, out var y))
        year = y;

    var book = bookService.AddBook(isbn, title, author, year, out var error);
    if (book != null)
        Console.WriteLine($"Book added successfully. ID: {book.Id}");
    else
        Console.WriteLine($"Error: {error}");
}

static void UpdateBook(IBookService bookService)
{
    Console.Write("Enter book ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    Console.Write("ISBN (13 digits): ");
    var isbn = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Title: ");
    var title = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Author: ");
    var author = Console.ReadLine()?.Trim() ?? "";
    Console.Write("Year published (optional, press Enter to skip): ");
    var yearInput = Console.ReadLine()?.Trim();
    int? year = null;
    if (!string.IsNullOrEmpty(yearInput) && int.TryParse(yearInput, out var y))
        year = y;

    var updated = bookService.UpdateBook(id, isbn, title, author, year, out var error);
    if (updated)
        Console.WriteLine("Book updated successfully.");
    else
        Console.WriteLine($"Error: {error}");
}

static void DeleteBook(IBookService bookService)
{
    Console.Write("Enter book ID to delete: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var deleted = bookService.DeleteBook(id, out var error);
    if (deleted)
        Console.WriteLine("Book deleted successfully.");
    else
        Console.WriteLine($"Error: {error}");
}

static void ListAllBooks(IBookService bookService)
{
    var books = bookService.GetAllBooks().ToList();
    if (books.Count == 0)
    {
        Console.WriteLine("No books in the library.");
        return;
    }

    Console.WriteLine($"Total books: {books.Count}");
    Console.WriteLine(new string('-', 60));
    foreach (var b in books)
    {
        Console.WriteLine($"ID: {b.Id} | ISBN: {b.Isbn} | {b.Title} by {b.Author}");
    }
}

static void ViewBookDetails(IBookService bookService)
{
    Console.Write("Enter book ID: ");
    if (!int.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Invalid ID.");
        return;
    }

    var book = bookService.GetBookById(id);
    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.WriteLine($"ID:           {book.Id}");
    Console.WriteLine($"ISBN:         {book.Isbn}");
    Console.WriteLine($"Title:        {book.Title}");
    Console.WriteLine($"Author:       {book.Author}");
    Console.WriteLine($"Year:         {book.YearPublished?.ToString() ?? "-"}");
}
