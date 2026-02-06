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

    public IEnumerable<Book> GetAll() => _books.Values.OrderBy(b => b.Id).ToList();

    public Book? GetById(int id) => _books.TryGetValue(id, out var book) ? book : null;

    public Book? GetByIsbn(string isbn) =>
        _books.Values.FirstOrDefault(b => string.Equals(b.Isbn, isbn, StringComparison.OrdinalIgnoreCase));

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

    public bool Update(Book book)
    {
        if (!_books.ContainsKey(book.Id))
            return false;

        _books[book.Id] = book;
        return true;
    }

    public bool Delete(int id) => _books.TryRemove(id, out _);
}
