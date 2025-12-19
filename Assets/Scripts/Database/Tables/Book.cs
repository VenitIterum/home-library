using SQLite;

namespace Database.Tables
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public int Author { get; set; }
        public int YearWriting { get; set; }
        public int YearPublication { get; set; }
        public bool IsRead { get; set; }
        public bool IsWantRead { get; set; }
        public bool IsReadingNow { get; set; }
        public bool IsForAdult { get; set; }
        public int Rating { get; set; } // 0-5
        public int PageCount { get; set; }
        public int BookHight { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string FilePath { get; set; }
        public string CoverPath { get; set; }
        public string SpinePath { get; set; }
        public int Storage { get; set; }

        public Book() { } 

        public Book(string title, int author, int yearWriting = 0)
        {
            Title = title;
            Author = author;
            YearWriting = yearWriting;
            IsRead = false;
            Rating = 0;
        }
    }
}
