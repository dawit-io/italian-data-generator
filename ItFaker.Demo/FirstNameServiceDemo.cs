using ItFaker.Models;
using ItFaker.Services;

namespace ItFaker.Demo;

public class FirstNameServiceDemo
{
    private readonly IFirstNameService _firstNameService;

    public FirstNameServiceDemo(IFirstNameService firstNameService)
    {
        _firstNameService = firstNameService ?? throw new ArgumentNullException(nameof(firstNameService));
    }

    public async Task RunAsync()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("=== FirstNameService Demo ===");
        Console.WriteLine("==================================\n");

        // Preload data to improve performance
        Console.WriteLine("Preloading data...");
        await _firstNameService.PreloadDataAsync();
        Console.WriteLine("Data preloaded successfully!\n");

        // Generate a random Italian name
        Console.WriteLine("1) Generate a random name:");
        string randomName = await _firstNameService.GenerateAsync();
        Console.WriteLine($"   Random Italian name: {randomName}\n");

        // Generate male names
        Console.WriteLine("2) Generate male names:");
        for (int i = 0; i < 5; i++)
        {
            string maleName = await _firstNameService.GenerateAsync(new FirstNameOptions
            {
                Gender = Gender.Male
            });
            Console.WriteLine($"   Male name {i + 1}: {maleName}");
        }
        Console.WriteLine();

        // Generate female names
        Console.WriteLine("3) Generate female names:");
        for (int i = 0; i < 5; i++)
        {
            string femaleName = await _firstNameService.GenerateAsync(new FirstNameOptions
            {
                Gender = Gender.Female
            });
            Console.WriteLine($"   Female name {i + 1}: {femaleName}");
        }
        Console.WriteLine();

        // Generate names with professional prefix
        Console.WriteLine("4) Generate names with professional titles:");
        for (int i = 0; i < 3; i++)
        {
            string maleProfessionalName = await _firstNameService.GenerateAsync(new FirstNameOptions
            {
                Gender = Gender.Male,
                Prefix = true
            });
            Console.WriteLine($"   Male name with title {i + 1}: {maleProfessionalName}");
        }

        for (int i = 0; i < 3; i++)
        {
            string femaleProfessionalName = await _firstNameService.GenerateAsync(new FirstNameOptions
            {
                Gender = Gender.Female,
                Prefix = true
            });
            Console.WriteLine($"   Female name with title {i + 1}: {femaleProfessionalName}");
        }
        Console.WriteLine();

        // Get only professional prefixes
        Console.WriteLine("5) Professional prefixes only:");
        string malePrefix = await _firstNameService.GetPrefixAsync(Gender.Male);
        string femalePrefix = await _firstNameService.GetPrefixAsync(Gender.Female);
        string neutralPrefix = await _firstNameService.GetPrefixAsync();
        Console.WriteLine($"   Male prefix: {malePrefix}");
        Console.WriteLine($"   Female prefix: {femalePrefix}");
        Console.WriteLine($"   Neutral prefix: {neutralPrefix}\n");

        // Use the synchronous version
        Console.WriteLine("6) Using synchronous methods:");
        string syncName = _firstNameService.Generate(new FirstNameOptions { Prefix = true });
        Console.WriteLine($"   Name generated synchronously: {syncName}\n");

        // Reset cache and regenerate
        Console.WriteLine("7) Reset cache and regenerate:");
        _firstNameService.ClearCache();
        Console.WriteLine("   Cache cleared");
        string nameAfterCacheClear = await _firstNameService.GenerateAsync();
        Console.WriteLine($"   Name after cache clear: {nameAfterCacheClear}\n");

        Console.WriteLine("FirstNameService demo completed!");
        Console.WriteLine("==================================\n");
    }
}