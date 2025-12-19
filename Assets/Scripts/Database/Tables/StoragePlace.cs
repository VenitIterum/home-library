using SQLite;

namespace Database.Tables
{
    public class StoragePlace
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        public StoragePlace() {}

        public StoragePlace(string name)
        {
            Name = name;
        }
    }
}