namespace LibraryManagementSystem.Core.Models;

/// <summary>
/// Represents a book in the library.
/// </summary>
public class Book
{
    /// <summary>
    /// Unique identifier for the book.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 13-digit International Standard Book Number (ISBN-13).
    /// </summary>
    public string Isbn { get; set; } = string.Empty;

    /// <summary>
    /// Title of the book.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Author(s) of the book.
    /// </summary>
    public string Author { get; set; } = string.Empty;

    /// <summary>
    /// Year of publication.
    /// </summary>
    public int? YearPublished { get; set; }
}
