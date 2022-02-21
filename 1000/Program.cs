using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1000
{
    class Program
    {
        static void Main(string[] args)
        {
            printWelcome();
            string[] names = new string[8];
            byte count = getPlayersCount();
            getStrings(count, names);
            changeName(count, names, askChange());


            // Game starts
            Game game = new Game(count, names);

            while (stopGame(game))
            {
                clearConsole();
                game.printStats();
                game.doTurn();
                game.switchTurn();
            }

            printWinner(game);
        }

        // Functions
        static void printWelcome()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\t\t\t\t\tWelcome to my 1000 game\n");
            Console.ResetColor();
        }

        static byte getPlayersCount()
        {
            while (true)
            {
                byte n = 0;
                Console.Write("Enter count of players (1-8): ");
                Byte.TryParse(Console.ReadLine(), out n);

                if (n <= 8 && n >= 1)
                    return n;
            }
        }

        static void getStrings(byte n, string[] names)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Enter {i + 1} name: ");
                names[i] = Console.ReadLine();

                if (names[i].Length == 0)
                    i--;
            }
        }
    
        static bool askChange()
        {
            Console.WriteLine();
            while (true)
            {
                char ans = '\0';
                Console.Write("Want to change some name (y/n): ");
                Char.TryParse(Console.ReadLine(), out ans);

                if (ans == 'y' || ans == 'n')
                    return ans == 'y';
            }
        }

        static void changeName(byte n, string[] names, bool wantChange)
        {
            while (wantChange)
            {
                byte index = 0;
                do
                {
                    Console.Write($"Enter index of name to change (1-{n}): ");
                    Byte.TryParse(Console.ReadLine(), out index);
                }
                while (index < 1 || index > n);

                do
                {
                    Console.Write($"Enter new {index} name: ");
                    names[index - 1] = Console.ReadLine();
                }
                while (names[index - 1].Length == 0);

                wantChange = askChange();
            }
        }
    
        static void clearConsole()
        {
            Console.Clear();
        }

        static bool stopGame(Game game)
        {
            for (int i = 0, n = game.playersCount; i < n; i++)
                if (game.players[i].points >= 1000)
                    return false;
            return true;
        }

        static void printWinner(Game game)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            for (int i = 0, n = game.playersCount; i < n; i++)
                if (game.players[i].points >= 1000)
                    Console.WriteLine($"\n\t\t\t\t\t\t{game.players[i].name} WINNER!");
            Console.ResetColor();
        }

        public static void nextLine()
        {
            Console.WriteLine();
        }

    }
}
