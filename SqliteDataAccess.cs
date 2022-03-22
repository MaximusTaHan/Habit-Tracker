using Microsoft.Data.Sqlite;

namespace Habit_Tracker
{
    class DataAccesser
    {
        readonly string connestionString = "Data Source=habitTracker.db";

        public virtual SqliteCommand QueryInput(string CommandString =
            @"CREATE TABLE IF NOT EXISTS breaks_table (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TIME,
                Minutes INTEGER
                )")
        {
            var connection = new SqliteConnection(connestionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = CommandString;
               
            return command;
        }
    }
    class CreateDB : DataAccesser
    {
        public CreateDB()
        {
            base.QueryInput();
        }
    }
    class WriteAccesser : DataAccesser
    {
        private int minutes;
        private string writeString;

        public void ExecuteWrite()
        {
            do
            {
                Console.WriteLine("How long was your leg-stretcher? (Time in minutes)");
            } while (!int.TryParse(Console.ReadLine(), out minutes));
            writeString = $"INSERT INTO breaks_table(Date, Minutes) VALUES(DATETIME('NOW'), {minutes})";
            QueryInput(writeString);
            var commandHolder = QueryInput(writeString);
            commandHolder.ExecuteNonQuery();
        }
    }

    class ReadAccesser : DataAccesser
    {
        readonly string readString = "SELECT * FROM breaks_table";
        public void ExecuteRead()
        {
            var commandHolder = QueryInput(readString);
            SqliteDataReader reader = commandHolder.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Entry number: {reader.GetString(0)}, Timestamp: {reader.GetString(1)}, Break time: {reader.GetInt32(2)} minutes");
                }
            }
        }
    }

    class DeleteAccesser : DataAccesser
    {
        string deleteString;
        int id;

        public void ExecuteDelete()
        {
            do
            {
                Console.WriteLine("Which entry do you want to delete?");
            } while (!int.TryParse(Console.ReadLine(), out id));
            deleteString = $"DELETE FROM breaks_table  WHERE Id = { id }";

            var commandHolder = QueryInput(deleteString);
            commandHolder.ExecuteNonQuery();
        }
    }
    class UpdateAccesser : DataAccesser
    {
        int id;
        int minutes;
        string updateString;

        public void ExecuteUpdate()
        {
            do
            {
                Console.WriteLine("Enter the entry number to change: ");
            } while (!int.TryParse(Console.ReadLine(), out id));
            do
            {
                Console.WriteLine("How long was your leg-stretcher? (Time in minutes)");
            } while (!int.TryParse(Console.ReadLine(), out minutes));
            updateString = $"UPDATE breaks_table SET Minutes={minutes} WHERE Id={id}";

            QueryInput(updateString);
            var commandHolder = QueryInput(updateString);
            commandHolder.ExecuteNonQuery();
        }
    }
}
