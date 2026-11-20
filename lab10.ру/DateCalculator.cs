using System.Globalization;

namespace DateCalc.Core;

/// <summary>
/// Калькулятор дат: разница между датами, прибавление дней и день недели.
/// Все методы статические и ничего не хранят, поэтому их легко тестировать.
/// </summary>
public static class DateCalculator
{
    // Индекс в массиве совпадает с числовым значением DayOfWeek (воскресенье = 0)
    private static readonly string[] DayNames =
    {
        "воскресенье", "понедельник", "вторник", "среда", "четверг", "пятница", "суббота"
    };

    // Принимаем и "05.03.2024", и "5.3.2024"
    private static readonly string[] DateFormats = { "dd.MM.yyyy", "d.M.yyyy" };

    /// <summary>
    /// Считает, сколько дней прошло от первой даты до второй.
    /// Если вторая дата раньше первой, результат будет отрицательным.
    /// Время суток игнорируется, сравниваются только календарные дни.
    /// </summary>
    /// <param name="from">Начальная дата.</param>
    /// <param name="to">Конечная дата.</param>
    public static int DaysBetween(DateTime from, DateTime to)
    {
        return (to.Date - from.Date).Days;
    }

    /// <summary>
    /// Прибавляет к дате указанное количество дней (отрицательное число - отнимает).
    /// </summary>
    /// <param name="date">Исходная дата.</param>
    /// <param name="days">Сколько дней прибавить.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Если результат выходит за пределы допустимых дат (год 1 - 9999).
    /// </exception>
    public static DateTime AddDays(DateTime date, int days)
    {
        return date.Date.AddDays(days);
    }

    /// <summary>Возвращает название дня недели по-русски, например "суббота".</summary>
    /// <param name="date">Дата.</param>
    public static string GetDayOfWeekName(DateTime date)
    {
        return DayNames[(int)date.DayOfWeek];
    }

    /// <summary>Проверяет, високосный ли год.</summary>
    /// <param name="year">Год от 1 до 9999.</param>
    /// <exception cref="ArgumentOutOfRangeException">Если год вне диапазона 1-9999.</exception>
    public static bool IsLeapYear(int year)
    {
        if (year < 1 || year > 9999)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "Год должен быть от 1 до 9999.");
        }

        return DateTime.IsLeapYear(year);
    }

    /// <summary>
    /// Пытается разобрать строку формата дд.мм.гггг.
    /// Возвращает false, если строка не похожа на реальную дату (например, 31.02.2024).
    /// </summary>
    /// <param name="text">Строка для разбора.</param>
    /// <param name="result">Получившаяся дата, если разбор удался.</param>
    public static bool TryParseDate(string? text, out DateTime result)
    {
        return DateTime.TryParseExact(
            text?.Trim(),
            DateFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);
    }
}
