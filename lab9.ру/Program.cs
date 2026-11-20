using System.Text;

namespace Lab09;

// ЛР №9, вариант 3 «Копирование»:
// копируем текстовый файл и заодно считаем в нём строки, слова и символы.
// Ошибки доступа (нет файла, нет прав и т.п.) обрабатываем через try/catch.
internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.Write("Откуда копировать (путь к файлу): ");
        string sourcePath = (Console.ReadLine() ?? "").Trim().Trim('"');

        Console.Write("Куда копировать (путь к файлу): ");
        string targetPath = (Console.ReadLine() ?? "").Trim().Trim('"');

        try
        {
            if (sourcePath.Length == 0 || targetPath.Length == 0)
            {
                throw new ArgumentException("Путь не может быть пустым.");
            }

            // Если указать один и тот же файл, он обнулится при открытии на запись
            if (Path.GetFullPath(sourcePath) == Path.GetFullPath(targetPath))
            {
                throw new ArgumentException("Исходный и конечный файлы совпадают.");
            }

            var stats = CopyAndCount(sourcePath, targetPath);

            Console.WriteLine("Готово! Файл скопирован.");
            Console.WriteLine($"Строк: {stats.Lines}");
            Console.WriteLine($"Слов: {stats.Words}");
            Console.WriteLine($"Символов (без переводов строки): {stats.Chars}");
        }
        // Порядок важен: FileNotFoundException и DirectoryNotFoundException
        // являются частными случаями IOException, поэтому идут раньше него
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Ошибка: файл не найден — {sourcePath}");
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine("Ошибка: такой папки не существует. Проверьте путь.");
        }
        catch (UnauthorizedAccessException)
        {
            Console.WriteLine("Ошибка: нет доступа к файлу или папке (возможно, нет прав на запись).");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Ошибка ввода-вывода: " + ex.Message);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
        catch (NotSupportedException)
        {
            Console.WriteLine("Ошибка: путь записан в неподдерживаемом формате.");
        }
    }

    /// <summary>
    /// Читает исходный файл построчно, сразу пишет строки в новый файл и считает статистику.
    /// </summary>
    static (int Lines, int Words, int Chars) CopyAndCount(string sourcePath, string targetPath)
    {
        int lines = 0, words = 0, chars = 0;

        // using сам закроет файлы, даже если посреди работы случится ошибка
        using (var reader = new StreamReader(sourcePath, Encoding.UTF8))
        using (var writer = new StreamWriter(targetPath, false, Encoding.UTF8))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                writer.WriteLine(line);

                lines++;
                chars += line.Length;

                // Слова - это всё, что разделено пробелами или табуляцией
                words += line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        return (lines, words, chars);
    }
}
