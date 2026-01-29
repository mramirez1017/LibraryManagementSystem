using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Core.Repositories;

namespace LibraryManagementSystem.Services;

/// <summary>
/// Service layer for book operations. Handles business logic and validation,
/// delegating data access to the repository.
/// </summary>
public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
    }

    /// <inheritdoc />
    public IEnumerable<Book> GetAllBooks() => _bookRepository.GetAll();

    /// <inheritdoc />
    public Book? GetBookById(int id) => _bookRepository.GetById(id);

    /// <inheritdoc />
    public Book? AddBook(string isbn, string title, string author, int? yearPublished, out string? errorMessage)
    {
        errorMessage = null;

        if (!IsbnValidator.IsValidIsbn13(isbn))
        {
            errorMessage = "Invalid ISBN. Must be a valid 13-digit ISBN (e.g. 978-0-13-235088-4).";
            return null;
        }

        var normalizedIsbn = IsbnValidator.NormalizeIsbn(isbn);
        if (_bookRepository.GetByIsbn(normalizedIsbn) != null)
        {
            errorMessage = "A book with this ISBN already exists.";
            return null;
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            errorMessage = "Title is required.";
            return null;
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            errorMessage = "Author is required.";
            return null;
        }

        var book = new Book
        {
            Isbn = normalizedIsbn,
            Title = title.Trim(),
            Author = author.Trim(),
            YearPublished = yearPublished
        };

        return _bookRepository.Add(book);
    }

    /// <inheritdoc />
    public bool UpdateBook(int id, string isbn, string title, string author, int? yearPublished, out string? errorMessage)
    {
        errorMessage = null;

        var existing = _bookRepository.GetById(id);
        if (existing == null)
        {
            errorMessage = "Book not found.";
            return false;
        }

        if (!IsbnValidator.IsValidIsbn13(isbn))
        {
            errorMessage = "Invalid ISBN. Must be a valid 13-digit ISBN (e.g. 978-0-13-235088-4).";
            return false;
        }

        var normalizedIsbn = IsbnValidator.NormalizeIsbn(isbn);
        var bookWithSameIsbn = _bookRepository.GetByIsbn(normalizedIsbn);
        if (bookWithSameIsbn != null && bookWithSameIsbn.Id != id)
        {
            errorMessage = "Another book with this ISBN already exists.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            errorMessage = "Title is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(author))
        {
            errorMessage = "Author is required.";
            return false;
        }

        existing.Isbn = normalizedIsbn;
        existing.Title = title.Trim();
        existing.Author = author.Trim();
        existing.YearPublished = yearPublished;

        return _bookRepository.Update(existing);
    }

    /// <inheritdoc />
    public bool DeleteBook(int id, out string? errorMessage)
    {
        errorMessage = null;
        if (_bookRepository.GetById(id) == null)
        {
            errorMessage = "Book not found.";
            return false;
        }

        return _bookRepository.Delete(id);
    }
}
