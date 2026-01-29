using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Repositories;

/// <summary>
/// Repository interface for book data access.
/// Abstracts data persistence and follows the Repository Pattern.
/// </summary>
public interface IBookRepository
{
    /// <summary>
    /// Gets all books.
    /// </summary>
    IEnumerable<Book> GetAll();

    /// <summary>
    /// Gets a book by its unique identifier.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>The book if found; otherwise null.</returns>
    Book? GetById(int id);

    /// <summary>
    /// Gets a book by its ISBN.
    /// </summary>
    /// <param name="isbn">The 13-digit ISBN.</param>
    /// <returns>The book if found; otherwise null.</returns>
    Book? GetByIsbn(string isbn);

    /// <summary>
    /// Adds a new book.
    /// </summary>
    /// <param name="book">The book to add.</param>
    /// <returns>The added book with assigned Id.</returns>
    Book Add(Book book);

    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="book">The book with updated data.</param>
    /// <returns>True if the book was found and updated; otherwise false.</returns>
    bool Update(Book book);

    /// <summary>
    /// Deletes a book by its ID.
    /// </summary>
    /// <param name="id">The book ID.</param>
    /// <returns>True if the book was found and deleted; otherwise false.</returns>
    bool Delete(int id);
}
