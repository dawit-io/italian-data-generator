using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;

namespace ItFaker.Common;

/// <summary>
/// Implementation of IRandomizer using Bogus Randomizer
/// </summary>
public class BogusRandomizer : IRandomizer
{
    private readonly Randomizer _randomizer;

    /// <summary>
    /// Initializes a new instance of the BogusRandomizer class
    /// </summary>
    /// <param name="randomizer">Bogus Randomizer instance</param>
    public BogusRandomizer(Randomizer randomizer)
    {
        _randomizer = randomizer ?? throw new ArgumentNullException(nameof(randomizer));
    }

    /// <summary>
    /// Gets a random integer between min and max values (inclusive)
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <returns>A random integer</returns>
    public int Number(int min, int max)
    {
        return _randomizer.Number(min, max);
    }

    /// <summary>
    /// Gets a random element from an array
    /// </summary>
    /// <typeparam name="T">Type of elements in the array</typeparam>
    /// <param name="array">The array to select from</param>
    /// <returns>A random element from the array</returns>
    public T ArrayElement<T>(T[] array)
    {
        return _randomizer.ArrayElement(array);
    }
}
