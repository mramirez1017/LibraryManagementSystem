using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Services;

/// <summary>
/// Service interface for book operations.
/// Encapsulates business logic and validation.
/// </summary>
public interface IBookService
{
    /// <summary>
    /// Gets all books.
    /// </summary>
    IEnumerable<Book> GetAllBooks();

    /// <summary>
    /// Gets a book by ID.
    /// </summary>
    Book? GetBookById(int id);

    /// <summary>
    /// Adds a new book after validating ISBN and ensuring no duplicate ISBN.
    /// </summary>
    /// <param name="isbn">13-digit ISBN.</param>
    /// <param name="title">Book title.</param>
    /// <param name="author">Author name.</param>
    /// <param name="yearPublished">Optional year of publication.</param>
    /// <param name="errorMessage">Error message if the operation fails.</param>
    /// <returns>The added book, or null on validation/duplicate error.</returns>
    Book? AddBook(string isbn, string title, string author, int? yearPublished, out string? errorMessage);

    /// <summary>
    /// Updates an existing book. Validates ISBN if changed and ensures no duplicate.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <param name="isbn">13-digit ISBN.</param>
    /// <param name="title">Book title.</param>
    /// <param name="author">Author name.</param>
    /// <param name="yearPublished">Optional year of publication.</param>
    /// <param name="errorMessage">Error message if the operation fails.</param>
    /// <returns>True if updated; false on validation error or not found.</returns>
    bool UpdateBook(int id, string isbn, string title, string author, int? yearPublished, out string? errorMessage);

    /// <summary>
    /// Deletes a book by ID.
    /// </summary>
    /// <param name="id">Book ID.</param>
    /// <param name="errorMessage">Error message if the operation fails.</param>
    /// <returns>True if deleted; false if not found.</returns>
    bool DeleteBook(int id, out string? errorMessage);
}
