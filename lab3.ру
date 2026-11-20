using System.Text;

namespace Lab03;

// ЛР №3, вариант 3 «Простые числа»:
// выводим все простые числа от 1 до N.
internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string answer;
        do
        {
            Console.Write("Введите N (целое число, не меньше 2): ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n < 2)
            {
                Console.WriteLine("Нужно целое число, которое не меньше 2.");
            }
            else
            {
                PrintPrimes(n);
            }

            Console.Write("Повторить с другим N? (д/н): ");
            answer = (Console.ReadLine() ?? "").Trim().ToLower();
        }
        while (answer == "д");
    }

    static void PrintPrimes(int n)
    {
        Console.WriteLine($"Простые числа от 1 до {n}:");

        int count = 0;

        // 1 простым не считается, поэтому начинаем с двойки
        for (int number = 2; number <= n; number++)
        {
            bool isPrime = true;

            // Достаточно искать делители до корня из числа.
            // Умножаем в long, чтобы не переполнить int на больших N.
            for (int d = 2; (long)d * d <= number; d++)
            {
                if (number % d == 0)
                {
                    isPrime = false;
                    break; // нашли делитель - дальше проверять нет смысла
                }
            }

            if (!isPrime)
            {
                continue;
            }

            Console.Write(number + " ");
            count++;

            // Чтобы длинный список не уезжал за край экрана,
            // переносим строку после каждого десятого числа
            if (count % 10 == 0)
            {
                Console.WriteLine();
            }
        }

        if (count % 10 != 0)
        {
            Console.WriteLine();
        }

        Console.WriteLine($"Всего найдено: {count}");
    }
}
