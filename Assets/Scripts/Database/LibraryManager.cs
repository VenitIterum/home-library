using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LibraryManager
{
    public SQLite.SQLiteConnection Init()
    {
        string dbPath = Path.Combine(Application.persistentDataPath, "library.db");
        var db = new SQLite.SQLiteConnection(dbPath);

        return db;
    }

    public void CreateTables(SQLite.SQLiteConnection db)
    {
        db.CreateTable<Book>();

        int bookCount = db.Table<Book>().Count();
        Debug.Log($"📚 Книг в базе: {bookCount}");
    }

    public void AddBook(SQLite.SQLiteConnection db, string title, string author, int year, bool isRead, int rating)
    {
        Debug.Log("Добавляем тестовую книгу...");
        
        Book sampleBook = new Book
        { 
            Title = title, 
            Author = author, 
            Year = year, 
            IsRead = isRead,
            Rating = rating 
        };
        
        db.Insert(sampleBook);
        
        Debug.Log($"✅ Добавлена {sampleBook.Title} тестовая книга");
    }

    public void TestDatabase(SQLite.SQLiteConnection db)
    {
        // Получаем все книги из БД
        var allBooks = db.Table<Book>().ToList();
        
        Debug.Log("=== СОДЕРЖИМОЕ БАЗЫ ДАННЫХ ===");
        Debug.Log($"Всего книг: {allBooks.Count}");
        
        foreach (var book in allBooks)
        {
            string status = book.IsRead ? "✓ Прочитана" : "✗ Не прочитана";
            string rating = book.Rating > 0 ? $"★ {book.Rating}/5" : "Без оценки";
            Debug.Log($"{book.Id}. '{book.Title}' - {book.Author} ({book.Year}) {status} {rating}");
        }
        
        // Показываем путь к файлу БД
        string dbPath = Application.persistentDataPath + "/library.db";
        Debug.Log($"\n📁 Файл базы данных: {dbPath}");
        
        // Проверяем размер файла
        if (File.Exists(dbPath))
        {
            long fileSize = new FileInfo(dbPath).Length;
            Debug.Log($"Размер файла БД: {fileSize} байт");
        }
    }

    public List<Book> GetBooksTitles(SQLite.SQLiteConnection db)
    {
        List<string> bookTitle = new List<string>();

        var allBooks = db.Table<Book>().ToList();

        // foreach (var book in allBooks)
        // {
        //     bookTitle.Add(book.Title);
        // }

        return allBooks;
    }

    public void RequestToDatabase(string request)
    {
        
    }
}
