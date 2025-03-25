using Bogus;
using ItFaker.Common;
using ItFaker.Services;

namespace ItFaker
{
    /// <summary>
    /// Main entry point for the ItFaker library which provides Italian-specific fake data generation
    /// </summary>
    public class ItFaker
    {
        private readonly IRandomizer? _randomizer;
        private IFirstNameService? _firstNameService;

        /// <summary>
        /// Initializes a new instance of the ItFaker class with optional seed
        /// </summary>
        /// <param name="seed">Optional seed for deterministic data generation</param>
        public ItFaker(int? seed = null)
        {
            // Create our randomizer with optional seed
            _randomizer = seed.HasValue
                ? new BogusRandomizer(new Randomizer(seed.Value))
                : new BogusRandomizer(new Randomizer());

            // Initialize services
            FirstNameService = new Lazy<IFirstNameService>(() =>
                _firstNameService ??= new FirstNameService(_randomizer));
        }

        /// <summary>
        /// Initializes a new instance of the ItFaker class with a custom randomizer
        /// </summary>
        /// <param name="randomizer">Custom implementation of IRandomizer</param>
        public ItFaker(IRandomizer randomizer)
        {
            _randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));

            // Initialize services
            FirstNameService = new Lazy<IFirstNameService>(() =>
                _firstNameService ??= new FirstNameService(_randomizer));
        }

        /// <summary>
        /// Service for generating Italian first names
        /// </summary>
        public Lazy<IFirstNameService> FirstNameService { get; }
    }
}