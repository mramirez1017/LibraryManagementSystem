using System.Collections.Concurrent;
using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Core.Repositories;

namespace LibraryManagementSystem.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of <see cref="IBookRepository"/>.
/// Uses a thread-safe dictionary keyed by book ID for storage.
/// </summary>
public class InMemoryBookRepository : IBookRepository
{
    private readonly ConcurrentDictionary<int, Book> _books = new();
    private int _nextId = 1;
    private readonly object _idLock = new();

    /// <inheritdoc />
    public IEnumerable<Book> GetAll() => _books.Values.OrderBy(b => b.Id).ToList();

    /// <inheritdoc />
    public Book? GetById(int id) => _books.TryGetValue(id, out var book) ? book : null;

    /// <inheritdoc />
    public Book? GetByIsbn(string isbn) =>
        _books.Values.FirstOrDefault(b => string.Equals(b.Isbn, isbn, StringComparison.OrdinalIgnoreCase));

    /// <inheritdoc />
    public Book Add(Book book)
    {
        int id;
        lock (_idLock)
        {
            id = _nextId++;
        }

        var newBook = new Book
        {
            Id = id,
            Isbn = book.Isbn,
            Title = book.Title,
            Author = book.Author,
            YearPublished = book.YearPublished
        };
        _books[id] = newBook;
        return newBook;
    }

    /// <inheritdoc />
    public bool Update(Book book)
    {
        if (!_books.ContainsKey(book.Id))
            return false;

        _books[book.Id] = book;
        return true;
    }

    /// <inheritdoc />
    public bool Delete(int id) => _books.TryRemove(id, out _);
}
