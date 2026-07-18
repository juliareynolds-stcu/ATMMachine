namespace ATMMachine;

public class ATMCLI
{
    public static event Action? WithdrawSelected;
    public static event Action? DepositSelected;
    public static event Action? ViewCurrentStateSelected;
    public static event Action? ViewAccountBalanceSelected;
    public static event Action? ViewPocketContentsSelected;

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
            Console.WriteLine("3) Make a deposit");
            Console.WriteLine("4) View my account balance");
            Console.WriteLine("5) Check my pocket");
            Console.WriteLine("Q) Quit");
            Console.Write(">  ");

            switch (Console.ReadLine())
            {
                case "1" or "1)" or "1.":
                    ViewCurrentStateSelected?.Invoke();
                    break;
                case "2" or "2)" or "2.":
                    WithdrawSelected?.Invoke();
                    break;
                case "3" or "3)" or "3.":
                    DepositSelected?.Invoke();
                    break;
                case "4" or "4)" or "4.":
                    ViewAccountBalanceSelected?.Invoke();
                    break;
                case "5" or "5)" or "5.":
                    ViewPocketContentsSelected?.Invoke();
                    break;
                case "Q)" or "Q" or "Q." or "q" or "q." or "q)"
                or "end" or "End" or "END"
                or "exit" or "Exit" or "EXIT"
                or "quit" or "Quit" or "QUIT":
                    repeat = false;
                    break;
                default:
                    Console.WriteLine("InvalidInput\r\n");
                    break;
            }
        }
    }
}
