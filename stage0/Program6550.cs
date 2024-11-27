    partial class Program
{
    private static void Main(string[] args)
    {
        Welcom6550();
        Welcom4998();
                Console.ReadKey();
    }

    private static void Welcom6550()
    {
        Console.Write("Enter your name ");
        string name = Console.ReadLine()!;
        Console.WriteLine("{0}, welcome to my first console application ", name);
    }
    static partial void Welcom4998();
    }
