namespace ATMMachine;

public class ATMCLI
{
    public static event Action? WithdrawSelected;
    public static event Action? ViewCurrentStateSelected;

    public static void Main(string[] args)
    {
        InputHandler input = new();

        Console.WriteLine("Welcome to the ATM");
        Console.WriteLine();

        bool repeat = true;

        while (repeat)
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1) View the current state of the ATM");
            Console.WriteLine("2) Make a withdrawl");
            Console.WriteLine("3) Exit");
            Console.Write(">  ");

            switch (Console.ReadLine())
            {
                case "1" or "1)" or "1.":
                    ViewCurrentStateSelected?.Invoke();
                    break;
                case "2" or "2)" or "2.":
                    WithdrawSelected?.Invoke();
                    break;
                case "3" or "3)" or "3." or "Q" or "Q." or "q" or "q."
                or "end" or "End" or "exit" or "Exit" or "quit" or "Quit":
                    repeat = false;
                    break;
                default:
                    Console.WriteLine("InvalidInput\r\n>  ");
                    break;
            }
        }
    }
}
