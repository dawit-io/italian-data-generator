using ItFaker.Demo;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("ItFaker Library Demo");
        Console.WriteLine("===================\n");

        var faker = new ItFaker.ItFaker();

        var firstNameDemo = new FirstNameServiceDemo(faker.FirstNameService.Value);
        await firstNameDemo.RunAsync();

        Console.WriteLine("\nPress any key to end...");
        Console.ReadKey();
    }
}