using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Services;

/// Service interface for book operations. Encapsulates business logic and validation.
public interface IBookService
{
    /// Gets all books.
    IEnumerable<Book> GetAllBooks();

    /// Gets a book by ID.
    Book? GetBookById(int id);

    /// Adds a new book after validating ISBN and ensuring no duplicate ISBN.
    Book? AddBook(string isbn, string title, string author, int? yearPublished, out string? errorMessage);

    /// Updates an existing book. Validates ISBN if changed and ensures no duplicate.
    bool UpdateBook(int id, string isbn, string title, string author, int? yearPublished, out string? errorMessage);

    /// Deletes a book by ID.
    bool DeleteBook(int id, out string? errorMessage);
}
