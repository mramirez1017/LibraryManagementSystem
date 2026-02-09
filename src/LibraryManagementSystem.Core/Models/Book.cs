namespace LibraryManagementSystem.Core.Models;

/// Represents a book in the library.
public class Book
{
    /// Unique identifier for the book.
    public int Id { get; set; }

    /// 13-digit International Standard Book Number (ISBN-13).
    public string Isbn { get; set; } = string.Empty;

    /// Title of the book.
    public string Title { get; set; } = string.Empty;

    /// Author(s) of the book.
    public string Author { get; set; } = string.Empty;

    /// Year of publication.
    public int? YearPublished { get; set; }
}
