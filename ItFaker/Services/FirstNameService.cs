using System.Text.Json;
using ItFaker.Common;
using ItFaker.Models;

namespace ItFaker.Services;

/// <summary>
/// Options for generating first names
/// </summary>
public class FirstNameOptions
{
    /// <summary>
    /// Specifies the gender for the name generation
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// Indicates whether to include a professional title prefix
    /// </summary>
    public bool Prefix { get; set; }
}

public interface IFirstNameService 
{
    /// <summary>
    /// Asynchronously generates an Italian first name
    /// </summary>
    /// <param name="options">Options for name generation</param>
    /// <returns>A Task containing the generated name</returns>
    Task<string> GenerateAsync(FirstNameOptions? options = null);

    /// <summary>
    /// Synchronously generates an Italian first name
    /// </summary>
    /// <param name="options">Options for name generation</param>
    /// <returns>The generated name</returns>
    string Generate(FirstNameOptions? options = null);

    /// <summary>
    /// Gets an Italian professional title (prefix)
    /// </summary>
    /// <param name="gender">Optional gender for gender-specific titles</param>
    /// <returns>A professional title</returns>
    string GetPrefix(Gender? gender = null);

    /// <summary>
    /// Asynchronously gets an Italian professional title (prefix)
    /// </summary>
    /// <param name="gender">Optional gender for gender-specific titles</param>
    /// <returns>A Task containing the professional title</returns>
    Task<string> GetPrefixAsync(Gender? gender = null);

    /// <summary>
    /// Preloads name data for better performance
    /// </summary>
    /// <returns>A Task representing the asynchronous operation</returns>
    Task PreloadDataAsync();

    /// <summary>
    /// Synchronously preloads name data for better performance
    /// </summary>
    void PreloadData();

    /// <summary>
    /// Clears cached name data
    /// </summary>
    void ClearCache();
}

/// <summary>
/// Service for generating authentic Italian first names
/// </summary>
public class FirstNameService: IFirstNameService
{
    private readonly Dictionary<string, string[]> _commonTitles = new Dictionary<string, string[]>
        {
            { "male", new[] { "Dott.", "Ing.", "Avv.", "Prof.", "Arch.", "Rag." } },
            { "female", new[] { "Dott.ssa", "Ing.", "Avv.", "Prof.ssa", "Arch.", "Rag." } },
            { "neutral", new[] { "Ing.", "Avv.", "Arch.", "Rag.", "Geom." } }
        };

    private WeightedRandomSelector<string>? _femaleNamesSelector;
    private WeightedRandomSelector<string>? _maleNamesSelector;
    private bool _dataLoaded = false;
    private readonly IRandomizer _randomizer;

    /// <summary>
    /// Initializes a new instance of the FirstNameService
    /// </summary>
    /// <param name="randomizer">Randomizer instance for random generation</param>
    public FirstNameService(IRandomizer randomizer)
    {
        _randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));
    }

    private async Task LoadNameDataAsync()
    {
        if (_dataLoaded)
        {
            return;
        }

        try
        {
            var assembly = typeof(FirstNameService).Assembly;

            // Get the assembly name to construct the resource path
            string? assemblyName = assembly.GetName().Name;

            // Load female names json
            string femaleResourceName = $"{assemblyName}.data.femaleFirstNames.json";
            using var femaleStream = assembly.GetManifestResourceStream(femaleResourceName);
            if (femaleStream == null)
            {
                throw new FileNotFoundException($"Embedded resource not found: {femaleResourceName}");
            }

            using var femaleReader = new StreamReader(femaleStream);
            var femaleNamesJson = await femaleReader.ReadToEndAsync();

            // Load male names json
            string maleResourceName = $"{assemblyName}.data.maleFirstNames.json";
            using var maleStream = assembly.GetManifestResourceStream(maleResourceName);
            if (maleStream == null)
            {
                throw new FileNotFoundException($"Embedded resource not found: {maleResourceName}");
            }

            using var maleReader = new StreamReader(maleStream);
            var maleNamesJson = await maleReader.ReadToEndAsync();

            // Deserialize the JSON data
            var femaleFirstNames = JsonSerializer.Deserialize<WeightedData>(femaleNamesJson);
            var maleFirstNames = JsonSerializer.Deserialize<WeightedData>(maleNamesJson);

            _femaleNamesSelector = new WeightedRandomSelector<string>(femaleFirstNames!.Items);
            _maleNamesSelector = new WeightedRandomSelector<string>(maleFirstNames!.Items);

            _dataLoaded = true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error loading name data: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Asynchronously generates an Italian first name
    /// </summary>
    /// <param name="options">Options for name generation</param>
    /// <returns>A Task containing the generated name</returns>
    public async Task<string> GenerateAsync(FirstNameOptions? options = null)
    {
        await LoadNameDataAsync();

        options ??= new FirstNameOptions();
        var gender = options.Gender ?? (Gender)_randomizer.Number(0, 1);

        string? name = gender == Gender.Male
            ? _maleNamesSelector!.Select()
            : _femaleNamesSelector!.Select();

        name = StringFormatting.FormatItalianName(name);

        if (options.Prefix)
        {
            return GetNameWithPrefix(name, gender);
        }

        return name;
    }

    /// <summary>
    /// Synchronously generates an Italian first name
    /// </summary>
    /// <param name="options">Options for name generation</param>
    /// <returns>The generated name</returns>
    public string Generate(FirstNameOptions? options = null)
    {
        return GenerateAsync(options).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Gets an Italian professional title (prefix)
    /// </summary>
    /// <param name="gender">Optional gender for gender-specific titles</param>
    /// <returns>A professional title</returns>
    public string GetPrefix(Gender? gender = null)
    {
        if (!gender.HasValue)
        {
            return _randomizer.ArrayElement(_commonTitles["neutral"]);
        }

        string genderKey = gender.Value == Gender.Male ? "male" : "female";
        return _randomizer.ArrayElement(_commonTitles[genderKey]);
    }

    /// <summary>
    /// Asynchronously gets an Italian professional title (prefix)
    /// </summary>
    /// <param name="gender">Optional gender for gender-specific titles</param>
    /// <returns>A Task containing the professional title</returns>
    public Task<string> GetPrefixAsync(Gender? gender = null)
    {
        return Task.FromResult(GetPrefix(gender));
    }

    /// <summary>
    /// Preloads name data for better performance
    /// </summary>
    /// <returns>A Task representing the asynchronous operation</returns>
    public async Task PreloadDataAsync()
    {
        await LoadNameDataAsync();
    }

    /// <summary>
    /// Synchronously preloads name data for better performance
    /// </summary>
    public void PreloadData()
    {
        PreloadDataAsync().GetAwaiter().GetResult();
    }

    private string GetNameWithPrefix(string name, Gender gender)
    {
        return $"{GetPrefix(gender)} {name}";
    }

    /// <summary>
    /// Clears cached name data
    /// </summary>
    public void ClearCache()
    {
        _dataLoaded = false;
        _femaleNamesSelector = null;
        _maleNamesSelector = null;
    }
}