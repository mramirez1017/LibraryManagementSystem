# Library Management System

A simple console application in C# .NET 8 for managing a library of books. The project demonstrates clean code, separation of concerns, and the **Repository Pattern**.

## Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)

## Building and Running

```bash
# Restore and build the solution
dotnet restore
dotnet build

# Run the console application
dotnet run --project src/LibraryManagementSystem/LibraryManagementSystem.csproj

# Run unit tests
dotnet test tests/LibraryManagementSystem.Tests/LibraryManagementSystem.Tests.csproj
```

## Project Structure

```
LibraryManagementSystem/
├── src/
│   ├── LibraryManagementSystem/           # Console application (entry point)
│   │   ├── Program.cs                     # DI setup, launches ConsoleUI
│   │   └── ConsoleUI.cs                   # Menu and user interaction
│   ├── LibraryManagementSystem.Core/      # Domain and contracts
│   │   ├── Models/
│   │   │   └── Book.cs                    # Book entity
│   │   └── Repositories/
│   │       └── IBookRepository.cs         # Repository interface
│   ├── LibraryManagementSystem.Infrastructure/  # Data access
│   │   └── Repositories/
│   │       └── InMemoryBookRepository.cs  # In-memory implementation
│   └── LibraryManagementSystem.Services/ # Business logic
│       ├── IBookService.cs
│       ├── BookService.cs                 # Uses repository, validates ISBN
│       └── IsbnValidator.cs               # 13-digit ISBN validation
├── tests/
│   └── LibraryManagementSystem.Tests/     # Unit tests (xUnit)
│       ├── InMemoryBookRepositoryTests.cs
│       ├── BookServiceTests.cs
│       └── IsbnValidatorTests.cs
├── LibraryManagementSystem.sln
└── README.md
```

## Design Decisions

### Layering and SOLID

- **Core** holds the domain model (`Book`) and the repository **interface** (`IBookRepository`). No implementation details; the console app and services depend on abstractions (Dependency Inversion).
- **Infrastructure** implements `IBookRepository` with an in-memory store (`ConcurrentDictionary` + auto-increment ID). Data is lost when the app exits; this keeps the assignment simple and avoids external dependencies.
- **Services** contain business logic and validation. `BookService` depends on `IBookRepository` and performs:
  - **ISBN-13 validation**: exactly 13 digits (hyphens allowed) and correct check digit.
  - Duplicate ISBN check on add/update.
  - Required title and author.
- **Console app** uses Microsoft.Extensions.DependencyInjection: `Program.cs` registers services (no direct instantiation) and launches `ConsoleUI`, which handles all menu I/O.

### Repository Pattern

- `IBookRepository` defines the data contract: GetById, GetByIsbn, GetAll, Add, Update, Delete.
- `InMemoryBookRepository` is the only implementation. Swapping to a database or file-based store would require a new class implementing the same interface and no change to the service or console layer.

### ISBN Validation

- **Format**: 13 digits, with optional hyphens or spaces (e.g. `978-0-13-235088-4` or `9780132350884`).
- **Check digit**: Validated using the standard ISBN-13 weighting (1, 3, 1, 3, …). Invalid or non-13-digit input is rejected by the service.

### Testing

- **Repository**: `InMemoryBookRepositoryTests` cover CRUD, ID assignment, and GetByIsbn.
- **Service**: `BookServiceTests` cover add/update/delete, valid/invalid ISBN, duplicate ISBN, and required fields.
- **Validator**: `IsbnValidatorTests` cover valid/invalid formats and normalization.

Tests use the real `InMemoryBookRepository`; the service is not mocked so that repository–service interaction is exercised.

## Console Menu

1. **Add a new book** – ISBN (13 digits), title, author, optional year.
2. **Update an existing book** – By ID; then ISBN, title, author, optional year.
3. **Delete a book** – By ID.
4. **List all books** – ID, ISBN, title, author.
5. **View details of a specific book** – By ID; full details.
6. **Exit**

## License

This project is provided as-is for assignment/assessment purposes.
