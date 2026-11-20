using System.Text;
using DateCalc.Core;

namespace DateCalc.App;

// Консольная оболочка для модуля DateCalc.Core.
// Здесь только ввод-вывод, вся логика лежит в DateCalculator.
internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            ShowMenu();
            Console.Write("Ваш выбор: ");
            string choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1":
                    ShowDifference();
                    break;
                case "2":
                    ShowAddDays();
                    break;
                case "3":
                    ShowDayOfWeek();
                    break;
                case "0":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Нет такого пункта, выберите из меню.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("=== Калькулятор дат ===");
        Console.WriteLine("1 - Разница между датами");
        Console.WriteLine("2 - Прибавить (или отнять) дни");
        Console.WriteLine("3 - Узнать день недели");
        Console.WriteLine("0 - Выход");
    }

    // Спрашивает дату, пока пользователь не введёт нормальную
    static DateTime ReadDate(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (дд.мм.гггг): ");

            if (DateCalculator.TryParseDate(Console.ReadLine(), out DateTime date))
            {
                return date;
            }

            Console.WriteLine("Такой даты не существует или формат неверный, попробуйте ещё раз.");
        }
    }

    static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt + ": ");

            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }

            Console.WriteLine("Нужно целое число, попробуйте ещё раз.");
        }
    }

    static void ShowDifference()
    {
        DateTime first = ReadDate("Первая дата");
        DateTime second = ReadDate("Вторая дата");

        int days = DateCalculator.DaysBetween(first, second);

        if (days < 0)
        {
            Console.WriteLine($"Разница: {days} дн. (вторая дата раньше первой)");
        }
        else
        {
            Console.WriteLine($"Разница: {days} дн.");
        }
    }

    static void ShowAddDays()
    {
        DateTime date = ReadDate("Исходная дата");
        int days = ReadInt("Сколько дней прибавить (отрицательное — отнять)");

        try
        {
            DateTime result = DateCalculator.AddDays(date, days);
            Console.WriteLine($"Получилась дата: {result:dd.MM.yyyy} ({DateCalculator.GetDayOfWeekName(result)})");
        }
        catch (ArgumentOutOfRangeException)
        {
            Console.WriteLine("Слишком далеко: получившаяся дата выходит за допустимые пределы.");
        }
    }

    static void ShowDayOfWeek()
    {
        DateTime date = ReadDate("Введите дату");
        Console.WriteLine($"{date:dd.MM.yyyy} — {DateCalculator.GetDayOfWeekName(date)}");
    }
}
