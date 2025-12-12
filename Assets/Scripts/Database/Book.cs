using UnityEngine;
using SQLite;

public class Book
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
    public bool IsRead { get; set; }
    public int Rating { get; set; } // 0-5
    public string Notes { get; set; }
    public string FilePath { get; set; }
    public string CoverPath { get; set; }

    public Book() { } 

    public Book(string title, string author, int year = 0)
    {
        Title = title;
        Author = author;
        Year = year;
        IsRead = false;
        Rating = 0;
    }
}
