using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1000
{
    internal class Game
    {
        public Player[] players = new Player[8];
        public byte playersCount = 0, currentMove = 0;

        // Functions
        public Game(byte len, string[] str)
        {
            playersCount = len;
            for (byte i = 0; i < len; i++)
                players[i] = new Player(str[i]);
        }

        public void printStats()
        {
            byte lineLen = calculateLine();

            // Print stats
            Console.ForegroundColor = ConsoleColor.Blue;
            printLine(lineLen);
            Console.WriteLine($"{getSpaces(lineLen)}Table");
            Console.ForegroundColor = ConsoleColor.White;
            for (byte i = 0; i < playersCount; i++)
                Console.Write($"{players[i].name}\t");
            
            Console.WriteLine();
            for (byte i = 0; i < playersCount; i++)
                Console.Write($"{players[i].points}{getTabs(i)}");

            Program.nextLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            printLine(lineLen);
            Console.ResetColor();
        }

        byte calculateLine()
        {
            byte len = 0;
            for (int i = 0; i < playersCount; i++)
                len += Convert.ToByte(8 * (players[i].name.Length / 8 + 1));
            return len;
        }

        void printLine(byte len)
        {
            Console.WriteLine($"{new String('-', len)}");
        }

        string getSpaces(byte len)
        {
            return new String(' ', Convert.ToByte((len - 5) / 2));
        }

        string getTabs(byte i)
        {
            return new String('\t', Convert.ToByte(players[i].name.Length / 8 + 1));
        }

        public void doTurn()
        {
            players[currentMove].throwDice();
        }
        
        public void switchTurn()
        {
            if (currentMove == playersCount - 1)
                currentMove = 0;
            else
                currentMove++;
        }
    }
}
