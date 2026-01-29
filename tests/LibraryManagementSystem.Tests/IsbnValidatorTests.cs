using LibraryManagementSystem.Services;
using Xunit;

namespace LibraryManagementSystem.Tests;

public class IsbnValidatorTests
{
    [Theory]
    [InlineData("9780132350884")]       // Clean Code
    [InlineData("978-0-13-235088-4")]
    [InlineData("978 0 13 235088 4")]
    [InlineData("978-0201633610")]      // Design Patterns (without last hyphen)
    public void IsValidIsbn13_ValidIsbn_ReturnsTrue(string isbn)
    {
        Assert.True(IsbnValidator.IsValidIsbn13(isbn));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123")]
    [InlineData("12345678901234")]
    [InlineData("abcdefghijklm")]
    [InlineData("9780132350885")]   // wrong check digit
    [InlineData("0000000000001")]   // invalid check digit (correct would be 0)
    public void IsValidIsbn13_InvalidIsbn_ReturnsFalse(string? isbn)
    {
        Assert.False(IsbnValidator.IsValidIsbn13(isbn));
    }

    [Fact]
    public void NormalizeIsbn_RemovesHyphensAndSpaces()
    {
        Assert.Equal("9780132350884", IsbnValidator.NormalizeIsbn("978-0-13-235088-4"));
        Assert.Equal("9780132350884", IsbnValidator.NormalizeIsbn("978 0 13 235088 4"));
    }
}
