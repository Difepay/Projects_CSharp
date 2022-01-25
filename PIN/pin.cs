using System;

namespace Lessons
{
    class Pin
    {
        // Enums
        public enum Options
        {
            EASY = 1,
            MODERATE,
            HARD,
            CRAZY,
        }

        // Defines
        public const byte PIN_LEN = 4;
        public const byte GUESS_TYPE_LEN = 2;
        public const byte MIN_PIN_DIGIT = 1;
        public const byte MAX_PIN_DIGIT = 6;

        public const byte EASY_ATTEMPTS = 20;
        public const byte MODERATE_ATTEMPTS = 15;
        public const byte HARD_ATTEMPTS = 10;
        public const byte CRAZY_OPTION_MIN = 5;
        public const byte CRAZY_OPTION_MAX = 25;

        public const char PLAY_GAME = 'y';
        public const char END_GAME = 'n';


        static void Main(string[] args)
        {
            // Programm data
            byte[] pin = new byte[PIN_LEN];
            byte attemptCount = 0, attemptsUsed = 0;
            bool crazyMod = false;

            // User data
            byte[] userPin = new byte[PIN_LEN];
            byte[] guessed = new byte[GUESS_TYPE_LEN];
            bool anotherPlay = false;

            do
            {
                printWelcomeMessage();

                Options gameDifficulty = getOption();
                pin = getPin();
                attemptCount = getAttempts(gameDifficulty);
                attemptsUsed = attemptCount;
                crazyMod = isCrazy(gameDifficulty);

                while (attemptsUsed != 0 && guessed[0] != PIN_LEN)
                {
                    printAttempt(crazyMod, attemptsUsed);
                    userPin = getPinFromUser();

                    if (!isPinCorrect(userPin))
                    {
                        printErrorMsg();
                        continue;
                    }

                    guessed = getGuess(userPin, pin);
                    --attemptsUsed;
                    printStageMsg(attemptCount, attemptsUsed, guessed, pin);
                }
                anotherPlay = askForNext();
            }
            while (anotherPlay);

            printEndMessage();
            Console.Read();
        }

        public static void printWelcomeMessage()
        {
            Console.Write("\nWelcome to \"MAGSHIMIM CODE-BREAKER\"!!!\n\n");

            Console.Write("A secret password was chosen to protect the credit card of Pancratius,\n");
            Console.Write("the descedant of Antiochus.\n");
            Console.Write("Your mission is to stop Pancratius by revealing his secret password.\n\n");

            Console.Write("The rules are as follows:\n");
            Console.Write("1. In each round you try to guess the secret password (4 distinct digits)\n");

            Console.Write("2. After every guess you'll receive two hints about the password\n");
            Console.Write("   HITS:\tThe number of digits in your guess which were exactly right.\n");
            Console.Write("   MISSES:\tThe number of digits in your guess which belongs to\n\t\tthe password but were miss-placed.\n");

            Console.Write("3. If you'll fail to guess the password after a certain number of rounds\n");
            Console.Write("   Pancratius will buy all the gifts for Hanukkah!!!\n\n");
        }

        public static Options getOption()
        {
            int userChoice = 0;
            Options retOption = Options.EASY;

            Console.WriteLine("Please choose the game level:");
            Console.WriteLine($"{Options.EASY} - Easy (20 rounds)");
            Console.WriteLine($"{Options.MODERATE} - Moderate (15 rounds)");
            Console.WriteLine($"{Options.HARD} - Hard (10 rounds)");
            Console.WriteLine($"{Options.CRAZY} - Crazy (random number of rounds 5-25)");

            do
            {
                Console.Write("Make a choice: ");
                userChoice = int.Parse(Console.ReadLine());
                retOption = (Options)userChoice;

            }
            while (retOption < Options.EASY || retOption > Options.CRAZY);

            return retOption;
        }

        public static byte[] getPin()
        {
            byte[] pin = new byte[PIN_LEN];

            Random random = new Random();

            for (int i = 0; i < PIN_LEN;)
            {
                byte currDigit = Convert.ToByte(random.Next(MIN_PIN_DIGIT, MAX_PIN_DIGIT + 1));

                if (!Array.Exists(pin, it => it == currDigit))
                {
                    pin[i] = currDigit;
                    ++i;
                }
            }

            return pin;
        }

        public static byte getAttempts(Options option)
        {
            if (Options.EASY == option)
                return EASY_ATTEMPTS;
            else if (Options.MODERATE == option)
                return MODERATE_ATTEMPTS;
            else if (Options.HARD == option)
                return HARD_ATTEMPTS;

            Random random = new Random();
            return Convert.ToByte(random.Next(CRAZY_OPTION_MIN, CRAZY_OPTION_MAX + 1));
        }

        public static bool isCrazy(Options option)
        {
            return Options.CRAZY == option;
        }

        public static void printAttempt(bool crazyFlag, byte attempts)
        {
            Console.WriteLine($"Write your guess (only {MIN_PIN_DIGIT}-{MAX_PIN_DIGIT}, no ENTER is needed)");

            if (!crazyFlag)
                Console.WriteLine($"{attempts} guesses left");
            else
                Console.WriteLine("CRAZY MODE!!!");
        }

        public static byte[] getPinFromUser()
        {
            byte[] pin = new byte[PIN_LEN];
            string str = Console.ReadLine();

            for (int i = 0; i < PIN_LEN; ++i)
                pin[i] = byte.Parse(str[i].ToString());

            return pin;
        }

        public static bool isPinCorrect(byte[] pin)
        {
            for (int i = 0; i < PIN_LEN; ++i)
                if (pin[i] < MIN_PIN_DIGIT || pin[i] > MAX_PIN_DIGIT)
                    return false;

            for (int i = 0; i < PIN_LEN; ++i)
                for (int j = 0; j < PIN_LEN; ++j)
                    if (i != j && pin[i] == pin[j])
                        return false;

            return true;
        }

        public static void printErrorMsg()
        {
            Console.WriteLine($"\nOnly {MIN_PIN_DIGIT}-{MAX_PIN_DIGIT} are allowed, try again!");
        }

        public static byte[] getGuess(byte[] userPin, byte[] pin)
        {
            byte[] guessed = new byte[GUESS_TYPE_LEN];

            for (int i = 0; i < PIN_LEN; ++i)
                if (userPin[i] == pin[i])
                    ++guessed[0];

            for (int i = 0; i < PIN_LEN; ++i)
                for (int j = 0; j < PIN_LEN; ++j)
                    if (i != j && userPin[i] == pin[j])
                        ++guessed[1];

            return guessed;
        }

        public static void printStageMsg(byte attemptsCount, byte attempts, byte[] guessed, byte[] pin)
        {
            if (attempts != 0)
            {
                if (PIN_LEN == guessed[0])
                {
                    Console.WriteLine($"\n{guessed[0]} HITS YOU WON!!!\n");
                    Console.WriteLine($"It took you only {attemptsCount - attempts} guesses, you are a professional code breaker");
                }
                else
                    Console.WriteLine($"\nYou got\t{guessed[0]} HITS\t{guessed[1]} MISSES.");
            }
            else
            {
                Console.WriteLine("\n\nOOOOHHHH!!! Pancratius won and bought all Hanukkah's gifts.");
                Console.WriteLine("Nothing left for you...");
                Console.WriteLine($"The secret password was {pin[0]}{pin[1]}{pin[2]}{pin[3]}");
            }
        }

        public static bool askForNext()
        {
            char next = '\0';

            do
            {
                Console.Write($"\nWould you like to play again? ({PLAY_GAME}/{END_GAME}): ");
                next = Console.ReadLine()[0];
            }
            while (next != PLAY_GAME && next != END_GAME);

            return PLAY_GAME == next;
        }
    
        public static void printEndMessage()
        {
            Console.WriteLine("\nBye Bye!");
            Console.WriteLine("Press any key to continue . . .");
        }
    }
}
