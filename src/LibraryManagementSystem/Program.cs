using LibraryManagementSystem;
using LibraryManagementSystem.Core.Repositories;
using LibraryManagementSystem.Infrastructure.Repositories;
using LibraryManagementSystem.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<IBookRepository, InMemoryBookRepository>();
services.AddSingleton<IBookService, BookService>();
services.AddSingleton<ConsoleUI>();

var provider = services.BuildServiceProvider();
var ui = provider.GetRequiredService<ConsoleUI>();
ui.Run();
