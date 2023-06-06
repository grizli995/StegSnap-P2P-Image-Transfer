internal class Program
{
    static Random rnd = new Random();
    private static Timer timer;
    private static int? PreviousValue;

    private static void Main(string[] args)
    {
        Console.WriteLine("Please specify the path to the .txt file.");
        var path = Console.ReadLine();
        StartWritingRandomNumberToFile(path, TimeSpan.FromSeconds(2));
        while (true)
        {
            var input = Console.ReadLine();
            if(!String.IsNullOrEmpty(input)) break;
        }
    }

    private static void WriteRandomDataToFile(string path)
    {
        int valueNumber;
        if(PreviousValue.HasValue)
        {
            valueNumber = rnd.Next(PreviousValue.Value - 5, PreviousValue.Value + 5);
            PreviousValue = valueNumber;
        }
        else
        {
            valueNumber = rnd.Next(-100, 100);
            PreviousValue = valueNumber;
        }
        var fahrenheitValue = (valueNumber  * 9 / 5) + 32;
        var value = $"{valueNumber}°C / {fahrenheitValue}°F";
        try
        {
            File.WriteAllText(path, value);
            Console.WriteLine($"'{value}'  written to file successfully.");
        }
        catch (Exception e)
        {
            // Output the exception message to the console, if any.
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    public static void StartWritingRandomNumberToFile(string path, TimeSpan period)
    {
        timer = new Timer(_ => WriteRandomDataToFile(path), null, TimeSpan.Zero, period);
    }

    public static void StopWriting()
    {
        timer?.Dispose();
    }



}