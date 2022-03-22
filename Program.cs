// See https://aka.ms/new-console-template for more information
using Habit_Tracker;

CreateDB createDB = new CreateDB();
WriteAccesser writeAccesser = new WriteAccesser();
ReadAccesser readAccesser = new ReadAccesser();
UpdateAccesser updateAccesser = new UpdateAccesser();
DeleteAccesser deleteAccesser = new DeleteAccesser();

createDB.QueryInput();
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
                readAccesser.ExecuteRead();
                break;
            case 2:
                writeAccesser.ExecuteWrite();
                break;
            case 3:
                deleteAccesser.ExecuteDelete();
                break;
            case 4:
                updateAccesser.ExecuteUpdate();
                break;
            default:
                Console.WriteLine("\nPlease enter a valid number");
                break;
        }

    }
}

// Try user input for validity (not scaleable with switch/case current implementation)
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