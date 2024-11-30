namespace BankApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bank<int> bank = new Bank<int>();
            var menu = new Menu<int>(bank);
            menu.Show();



        }
    }
}
