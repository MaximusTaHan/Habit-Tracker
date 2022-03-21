// See https://aka.ms/new-console-template for more information
using Microsoft.Data.Sqlite;

string connestionString = "Data Source=habitTracker.db";

using (var connection = new SqliteConnection(connestionString))
{
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText =
        @"CREATE TABLE IF NOT EXISTS breaks_table (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Date TIME,
            Minutes INTEGER
            )";
    command.ExecuteNonQuery();
}

UserInput();

void UserInput()
{
    Console.Clear();

    bool closeApp = false;
    int command;
    while (closeApp == false)
    {
        Console.WriteLine("\n\nREMEMBER TO TAKE BREAKS");
        Console.WriteLine("\nWhat would you like to do?");
        Console.WriteLine("\nType 0 to Close Application.");
        Console.WriteLine("Type 1 to View All Records.");
        Console.WriteLine("Type 2 to Insert Record.");
        Console.WriteLine("Type 3 to Delete Records.");
        Console.WriteLine("Type 4 to Update Record.");

        do
        {
            TryInput(out command);
        } while (command > 4 || command < 0);

        switch (command)
        {
            case 0:
                Console.WriteLine("\nGoodbye!");
                closeApp = true;
                break;
            case 1:
                ExecuteRead();
                break;
            case 2:
                ExecuteWrite();
                break;
            case 3:
                ExecuteDelete();
                break;
            case 4:
                ExecuteUpdate();
                break;
            default:
                Console.WriteLine("\nPlease enter a valid number");
                break;
        }

        // Try user input for validity (not scaleable with switch/case current implementation)

    }
}

void ExecuteWrite()
{
    int minutes;

    do
    {
        Console.WriteLine("How long was your leg-stretcher? (Time in minutes)");
    } while (!int.TryParse(Console.ReadLine(), out minutes));

    var connection = new SqliteConnection(connestionString);
    connection.Open();

    var command = connection.CreateCommand();

    // Timestamp for when the entry was made and how long the break was
    command.CommandText = $"INSERT INTO breaks_table(Date, Minutes) VALUES(DATETIME('NOW'), {minutes})";
    command.ExecuteNonQuery();
}

void ExecuteRead()
{
    var connection = new SqliteConnection(connestionString);
    connection.Open();

    var command = connection.CreateCommand();

    // Gets everything in the table
    command.CommandText = "SELECT * FROM breaks_table";
    SqliteDataReader reader = command.ExecuteReader();

    // Not quite sure how data stream reading works but the code works
    if(reader.HasRows)
    {
        while(reader.Read())
        {
            Console.WriteLine($"Entry number: {reader.GetString(0)}, Timestamp: {reader.GetString(1)}, Break time: {reader.GetInt32(2)} minutes");
        }
    }
}

void ExecuteUpdate()
{
    int id;
    int minutes;

    do
    {
        Console.WriteLine("Enter the entry number to change: ");
    } while (!int.TryParse(Console.ReadLine(), out id));
    do
    {
        Console.WriteLine("How long was your leg-stretcher? (Time in minutes)");
    } while (!int.TryParse(Console.ReadLine(), out minutes));

    var connection = new SqliteConnection(connestionString);
    connection.Open();

    var command = connection.CreateCommand();

    // Could implement check for if the Id number exists in DB or not
    // Timestamp for when the entry was made and how long the break was
    command.CommandText = $"UPDATE breaks_table SET Minutes={minutes} WHERE Id={id}";
    command.ExecuteNonQuery();
}
void ExecuteDelete()
{
    int id;

    do
    {
        Console.WriteLine("Which entry do you want to delete?");
    } while (!int.TryParse(Console.ReadLine(), out id));

    var connection = new SqliteConnection(connestionString);
    connection.Open();

    var command = connection.CreateCommand();

    command.CommandText = $"DELETE FROM breaks_table  WHERE Id = { id }";
    command.ExecuteNonQuery();
}
static int TryInput(out int num)
{
    string input = Console.ReadLine();
    if (int.TryParse(input, out num))
        return num;
    else
    {
        Console.WriteLine("\nPlease enter a valid number");
        return num = -1;
    }
}