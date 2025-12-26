using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Database.Tables;
using System.Data.Common;

namespace Database
{
    public class LibraryManager
    {
        public SQLite.SQLiteConnection Init()
        {
            string dbPath = Path.Combine(Application.persistentDataPath, "library.db");
            var db = new SQLite.SQLiteConnection(dbPath);

            CreateTables(db);

            return db;
        }

        public void CreateTables(SQLite.SQLiteConnection db)
        {
            db.CreateTable<Book>();
            db.CreateTable<Author>();
            db.CreateTable<Genre>();
            db.CreateTable<BookGenre>();
            db.CreateTable<StoragePlace>();

            //Можно удалить после создания полностью рабочей БД
            int bookCount = db.Table<Book>().Count();
            int authorCount = db.Table<Author>().Count();
            int genreCount = db.Table<Genre>().Count();
            Debug.Log($"Книг в базе: {bookCount}; Авторов в базе: {authorCount}; Жанров в базе: {genreCount}");
            //^DELETE^
        }

        public void AddBook(SQLite.SQLiteConnection db, string title, int author, int year, bool isRead, int rating, int pageCount = 500, int bookHight = 20)
        {
            Book sampleBook = new Book
            { 
                Title = title, 
                Author = author, 
                YearWriting = year, 
                IsRead = isRead,
                Rating = rating 
            };

            sampleBook.PageCount = pageCount;
            sampleBook.BookHight = bookHight;
            
            db.Insert(sampleBook);
        }

        public void AddAuthor(SQLite.SQLiteConnection db, string firstName, string lastName)
        {
            Author sampleAuthor = new Author
            { 
                FirstName = firstName,
                LastName = lastName
            };
            
            db.Insert(sampleAuthor);
        }

        public void AddGenre(SQLite.SQLiteConnection db, string name)
        {
            Genre sampleGenre = new Genre
            { 
                Name = name
            };
            
            db.Insert(sampleGenre);
        }

        public void AddStoragePlace(SQLite.SQLiteConnection db, string name)
        {
            StoragePlace sampleStoragePlace = new StoragePlace
            { 
                Name = name
            };
            
            db.Insert(sampleStoragePlace);
        }

        //DELETE method
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
                Debug.Log($"{book.Id}. '{book.Title}' - {book.Author} ({book.YearWriting}) {status} {rating}");
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
        //^DELETE^

        public List<Book> GetBooksList(SQLite.SQLiteConnection db)
        {
            List<string> bookTitle = new List<string>();
            var allBooks = db.Table<Book>().ToList();

            return allBooks;
        }

        public void RequestToDatabase(string request)
        {
            
        }

        public Book FindBookByExactTitle(SQLite.SQLiteConnection db, string title)
        {
            return db.Table<Book>().FirstOrDefault(b => b.Title == title);
        }

        public Author FindAuthorById(SQLite.SQLiteConnection db, int idAuthor)
        {
            return db.Table<Author>().FirstOrDefault(b => b.Id == idAuthor);
        }
    }
}