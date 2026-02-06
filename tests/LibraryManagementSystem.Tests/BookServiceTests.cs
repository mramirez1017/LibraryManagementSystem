using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Core.Repositories;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.Services;
using Xunit;

namespace LibraryManagementSystem.Tests;

public class BookServiceTests
{
    private readonly IBookRepository _repository = new InMemoryBookRepository();
    private readonly IBookService _service;

    public BookServiceTests()
    {
        _service = new BookService(_repository);
    }

    [Fact]
    public void AddBook_WithValidIsbn13_Succeeds()
    {
        // Valid ISBN-13: 978-0-13-235088-4
        var book = _service.AddBook("9780132350884", "Clean Code", "Robert Martin", 2008, out var error);

        Assert.NotNull(book);
        Assert.Null(error);
        Assert.Equal("9780132350884", book.Isbn);
        Assert.Equal("Clean Code", book.Title);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("12345678901234")]
    [InlineData("abcdefghijklm")]
    [InlineData("9780132350885")] // wrong check digit
    public void AddBook_WithInvalidIsbn_ReturnsError(string isbn)
    {
        var book = _service.AddBook(isbn, "Title", "Author", null, out var error);

        Assert.Null(book);
        Assert.NotNull(error);
        Assert.Contains("13-digit ISBN", error);
    }

    [Fact]
    public void AddBook_WithValidIsbnWithHyphens_Succeeds()
    {
        var book = _service.AddBook("978-0-13-235088-4", "Clean Code", "Robert Martin", null, out var error);

        Assert.NotNull(book);
        Assert.Null(error);
        Assert.Equal("9780132350884", book.Isbn);
    }

    [Fact]
    public void AddBook_DuplicateIsbn_ReturnsError()
    {
        _service.AddBook("9780132350884", "First", "Author", null, out _);
        var book = _service.AddBook("9780132350884", "Second", "Author", null, out var error);

        Assert.Null(book);
        Assert.NotNull(error);
        Assert.Contains("already exists", error);
    }

    [Fact]
    public void AddBook_EmptyTitle_ReturnsError()
    {
        var book = _service.AddBook("9780132350884", "  ", "Author", null, out var error);

        Assert.Null(book);
        Assert.NotNull(error);
        Assert.Contains("Title", error);
    }

    [Fact]
    public void AddBook_EmptyAuthor_ReturnsError()
    {
        var book = _service.AddBook("9780132350884", "Title", "", null, out var error);

        Assert.Null(book);
        Assert.NotNull(error);
        Assert.Contains("Author", error);
    }

    [Fact]
    public void UpdateBook_WhenExists_WithValidData_Succeeds()
    {
        var added = _service.AddBook("9780132350884", "Original", "Author", null, out _);
        Assert.NotNull(added);

        var updated = _service.UpdateBook(added.Id, "9780132350884", "Updated Title", "New Author", 2020, out var error);

        Assert.True(updated);
        Assert.Null(error);
        var book = _service.GetBookById(added.Id);
        Assert.NotNull(book);
        Assert.Equal("Updated Title", book.Title);
        Assert.Equal("New Author", book.Author);
        Assert.Equal(2020, book.YearPublished);
    }

    [Fact]
    public void UpdateBook_InvalidIsbn_ReturnsError()
    {
        var added = _service.AddBook("9780132350884", "Original", "Author", null, out _);
        Assert.NotNull(added);

        var updated = _service.UpdateBook(added.Id, "invalid", "Title", "Author", null, out var error);

        Assert.False(updated);
        Assert.NotNull(error);
    }

    [Fact]
    public void UpdateBook_NonExistentId_ReturnsError()
    {
        var updated = _service.UpdateBook(999, "9780132350884", "Title", "Author", null, out var error);

        Assert.False(updated);
        Assert.NotNull(error);
        Assert.Contains("not found", error);
    }

    [Fact]
    public void DeleteBook_WhenExists_Succeeds()
    {
        var added = _service.AddBook("9780132350884", "To Delete", "Author", null, out _);
        Assert.NotNull(added);

        var deleted = _service.DeleteBook(added.Id, out var error);

        Assert.True(deleted);
        Assert.Null(error);
        Assert.Null(_service.GetBookById(added.Id));
    }

    [Fact]
    public void DeleteBook_WhenNotExists_ReturnsError()
    {
        var deleted = _service.DeleteBook(999, out var error);

        Assert.False(deleted);
        Assert.NotNull(error);
    }

    [Fact]
    public void GetBookById_WhenExists_ReturnsBook()
    {
        var added = _service.AddBook("9780132350884", "Clean Code", "Robert Martin", 2008, out _);
        Assert.NotNull(added);

        var book = _service.GetBookById(added.Id);

        Assert.NotNull(book);
        Assert.Equal(added.Id, book.Id);
        Assert.Equal("Clean Code", book.Title);
    }

    [Fact]
    public void GetAllBooks_ReturnsAllAddedBooks()
    {
        _service.AddBook("9780132350884", "Book 1", "Author 1", null, out _);
        _service.AddBook("9780201633610", "Book 2", "Author 2", null, out _);

        var books = _service.GetAllBooks().ToList();

        Assert.Equal(2, books.Count);
    }
}
