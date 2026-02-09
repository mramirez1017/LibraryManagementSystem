using LibraryManagementSystem.Core.Models;

namespace LibraryManagementSystem.Core.Repositories;

/// Repository interface for book data access. Abstracts data persistence and follows the Repository Pattern.
public interface IBookRepository
{
    /// Gets all books.
    IEnumerable<Book> GetAll();

    /// Gets a book by its unique identifier.
    Book? GetById(int id);

    /// Gets a book by its ISBN.
    Book? GetByIsbn(string isbn);

    /// Adds a new book.
    Book Add(Book book);

    /// Updates an existing book.
    bool Update(Book book);

    /// Deletes a book by its ID.
    bool Delete(int id);
}
