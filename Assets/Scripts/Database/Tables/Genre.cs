using SQLite;

namespace Database.Tables
{
    public class Genre
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        public Genre() {}

        public Genre(string name)
        {
            Name = name;
        }
    }
}
