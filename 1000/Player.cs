using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1000
{
    internal class Player
    {
        public string name;
        public ushort points;

        public byte[] currentThrow = new byte[5];

        // Functions
        public Player(string nm)
        {
            name = nm;
            points = 0;
        }

        public void throwDice()
        {
            byte[] currThrow = new byte[5];
            byte[] currValues = new byte[6];

            ushort currentPoints = 0;
            bool mustRethrow = false, oneTurn = false;
            byte option = 0;

            printTurns();

            if (!askTurn())
                return;

            fillThrow(currThrow);
            do
            {
                if (mustRethrow)
                    fillThrow(currThrow);
                if (oneTurn)
                    completeThrow(currThrow);

                printThrow(currThrow);
                resetValues(currValues);
                updateValues(currValues, currThrow);

                ushort dicePoints = 0;
                if (!oneTurn)
                {
                    dicePoints = Convert.ToUInt16(checkForLadder(currValues) + checkForUpLadder(currValues) + checkMul(currValues));
                    Console.WriteLine("NOT one throw");
                }

                mustRethrow = getRethrow(currValues, dicePoints);
                dicePoints += checkElse(currValues[0], currValues[4], oneTurn);
                currentPoints = oneTurn ? dicePoints : Convert.ToUInt16(currentPoints + dicePoints);
                Console.WriteLine(currentPoints);

                if (dicePoints == 0)
                {
                    currentPoints = 0;
                    break;
                }

                option = askForRethrow(mustRethrow, calcFree(currValues[0], currValues[4]));
                oneThrow = option == 1 && !mustRethrow;
            }
            while (option == 1);

            writeScore(currentPoints);
            waitEnter();
        }

        void printTurns()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.nextLine();
            Console.WriteLine($"{name}'s turn\n");
            Console.ResetColor();
        }

        bool askTurn()
        {
            Console.WriteLine("1) Throw dice");
            Console.WriteLine("2) Skip turn");
            while (true)
            {
                Console.Write("Your option: ");
                char res = '\0';
                Char.TryParse(Console.ReadLine(), out res);

                if (res == '1' || res == '2')
                    return res == '1';
            }
        }

        ushort checkForLadder(byte[] values)
        {
            for (int i = 0, n = values.Length - 1; i < n; i++)
                if (values[i] == 0 || values[i] > 1)
                    return 0;
            return 125;
        }

        ushort checkForUpLadder(byte[] values)
        {
            for (int i = 1, n = values.Length; i < n; i++)
                if (values[i] == 0 || values[i] > 1)
                    return 0;
            return 250;
        }

        ushort checkMul(byte[] values)
        {
            for (int i = 0, n = values.Length; i < n; i++)
            {
                if (values[i] == 3)
                    return Convert.ToUInt16((i + 1) * 10);
                if (values[i] == 4)
                    return Convert.ToUInt16((i + 1) * 20);
                if (values[i] == 5)
                    return Convert.ToUInt16((i + 1) * 100);
            }
            return 0;
        }

        ushort checkElse(byte ones, byte fives, bool oneThrow)
        {
            if (oneThrow)
                return Convert.ToUInt16(ones * 10 + fives * 5);

            ushort sum = 0;
            if (ones < 3)
                sum += Convert.ToUInt16(ones * 10);
            if (fives < 3)
                sum += Convert.ToUInt16(fives * 5);
            return sum;
        }

        ushort calcFree(byte ones, byte fives)
        {
            return Convert.ToUInt16(5 - (ones + fives));
        }

        byte askForRethrow(bool must, ushort free)
        {
            byte max = Convert.ToByte(must ? 1 : 2), option = 0;
            Console.WriteLine("\nOptions:");
            if (must)
            {
                Console.WriteLine("1) Throw all 5 again");
            }
            else
            {
                Console.WriteLine($"1) Throw {free} again");
                Console.WriteLine("2) Save result");
            }
            while (true)
            {
                Console.Write("Your option: ");
                Byte.TryParse(Console.ReadLine(), out option);

                if (option <= max && option >= 1)
                    return option;
            }
        }

        bool getRethrow(byte[] values, ushort dice)
        {
            return dice > 0 || 5 == (values[0] + values[4]);
        }

        void resetValues(byte[] values)
        {
            for (int i = 0, n = values.Length; i < n; i++)
                values[i] = 0;
        }

        void updateValues(byte[] values, byte[] currThrow)
        {
            foreach (int i in currThrow)
                values[i - 1]++;
        }
    
        void printThrow(byte[] currThrow)
        {
            Console.WriteLine("\nCurrent throw:");
            foreach (int i in currThrow)
                Console.Write($"|{i}|\t");
            Program.nextLine();
        }
    
        void fillThrow(byte[] currThrow)
        {
            Random rand = new Random();
            for (int i = 0, n = currThrow.Length; i < n; i++)
                currThrow[i] = getRandom(rand);
        }

        void completeThrow(byte[] currThrow)
        {
            Random rand = new Random();
            for (int i = 0, n = currThrow.Length; i < n; i++)
                if (currThrow[i] != 1 && currThrow[i] != 5)
                    currThrow[i] = getRandom(rand);
        }

        byte getRandom(Random rnd)
        {
            return Convert.ToByte(rnd.Next(6) + 1);
        }
    
        void writeScore(ushort currPoints)
        {
            if (currPoints == 0)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"\nAdded {currPoints} to {name}'s score!");
            Console.ResetColor();
            points += currPoints;
        }

        void waitEnter()
        {
            if (points < 1000)
            {
                Console.Write("\nTo do next turn click any keyboard button. . . ");
                Console.Read();
            }
        }
    }
}
