using LibraryManagementSystem.Core.Models;
using LibraryManagementSystem.Infrastructure.Repositories;
using Xunit;

namespace LibraryManagementSystem.Tests;

public class InMemoryBookRepositoryTests
{
    private readonly InMemoryBookRepository _repository = new();

    [Fact]
    public void GetAll_WhenEmpty_ReturnsEmptyList()
    {
        var result = _repository.GetAll().ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void Add_AssignsIdAndReturnsBook()
    {
        var book = new Book { Isbn = "9780132350884", Title = "Clean Code", Author = "Robert Martin", YearPublished = 2008 };
        var added = _repository.Add(book);

        Assert.Equal(1, added.Id);
        Assert.Equal(book.Isbn, added.Isbn);
        Assert.Equal(book.Title, added.Title);
    }

    [Fact]
    public void Add_MultipleBooks_IncrementsId()
    {
        var b1 = _repository.Add(new Book { Isbn = "9780132350884", Title = "A", Author = "X" });
        var b2 = _repository.Add(new Book { Isbn = "9780201633610", Title = "B", Author = "Y" });

        Assert.Equal(1, b1.Id);
        Assert.Equal(2, b2.Id);
    }

    [Fact]
    public void GetById_WhenExists_ReturnsBook()
    {
        var added = _repository.Add(new Book { Isbn = "9780132350884", Title = "Clean Code", Author = "Robert Martin" });
        var found = _repository.GetById(added.Id);

        Assert.NotNull(found);
        Assert.Equal(added.Id, found.Id);
        Assert.Equal("9780132350884", found.Isbn);
    }

    [Fact]
    public void GetById_WhenNotExists_ReturnsNull()
    {
        var found = _repository.GetById(999);
        Assert.Null(found);
    }

    [Fact]
    public void GetByIsbn_WhenExists_ReturnsBook()
    {
        _repository.Add(new Book { Isbn = "9780132350884", Title = "Clean Code", Author = "Robert Martin" });
        var found = _repository.GetByIsbn("9780132350884");

        Assert.NotNull(found);
        Assert.Equal("9780132350884", found.Isbn);
    }

    [Fact]
    public void GetByIsbn_WhenNotExists_ReturnsNull()
    {
        var found = _repository.GetByIsbn("0000000000000");
        Assert.Null(found);
    }

    [Fact]
    public void Update_WhenExists_ReturnsTrueAndUpdatesData()
    {
        var added = _repository.Add(new Book { Isbn = "9780132350884", Title = "Original", Author = "Author" });
        added.Title = "Updated Title";
        var result = _repository.Update(added);

        Assert.True(result);
        var found = _repository.GetById(added.Id);
        Assert.NotNull(found);
        Assert.Equal("Updated Title", found.Title);
    }

    [Fact]
    public void Update_WhenNotExists_ReturnsFalse()
    {
        var book = new Book { Id = 999, Isbn = "9780132350884", Title = "X", Author = "Y" };
        var result = _repository.Update(book);
        Assert.False(result);
    }

    [Fact]
    public void Delete_WhenExists_ReturnsTrueAndRemovesBook()
    {
        var added = _repository.Add(new Book { Isbn = "9780132350884", Title = "Clean Code", Author = "Robert Martin" });
        var result = _repository.Delete(added.Id);

        Assert.True(result);
        Assert.Null(_repository.GetById(added.Id));
    }

    [Fact]
    public void Delete_WhenNotExists_ReturnsFalse()
    {
        var result = _repository.Delete(999);
        Assert.False(result);
    }
}
