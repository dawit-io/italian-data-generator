using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItFaker.Common;

/// <summary>
/// Interface for random number and element generation
/// </summary>
public interface IRandomizer
{
    /// <summary>
    /// Gets a random integer between min and max values (inclusive)
    /// </summary>
    /// <param name="min">Minimum value</param>
    /// <param name="max">Maximum value</param>
    /// <returns>A random integer</returns>
    int Number(int min, int max);

    /// <summary>
    /// Gets a random element from an array
    /// </summary>
    /// <typeparam name="T">Type of elements in the array</typeparam>
    /// <param name="array">The array to select from</param>
    /// <returns>A random element from the array</returns>
    T ArrayElement<T>(T[] array);
}
