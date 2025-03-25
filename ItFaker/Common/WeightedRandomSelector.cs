using System;
using System.Collections.Generic;
using System.Linq;
using ItFaker.Models;

namespace ItFaker.Common;

/// <summary>
/// Utility class for selecting items based on weighted probabilities
/// </summary>
/// <typeparam name="T">The type of items to select</typeparam>
public class WeightedRandomSelector<T>
{
    private readonly List<WeightedItem<T>> _items;
    private readonly double _totalWeight;
    private readonly Random _random = new Random();

    /// <summary>
    /// Initializes a new instance of the WeightedRandomSelector class
    /// </summary>
    /// <param name="items">Collection of items with associated weights</param>
    /// <exception cref="ArgumentException">Thrown when items is null or empty</exception>
    public WeightedRandomSelector(List<WeightedItem<T>> items)
    {
        if (items == null || !items.Any())
        {
            throw new ArgumentException("Items collection cannot be null or empty", nameof(items));
        }

        _items = items;
        _totalWeight = items.Sum(item => item.Weight);
    }

    /// <summary>
    /// Selects an item randomly based on the weighted probabilities
    /// </summary>
    /// <returns>The selected item</returns>
    public T Select()
    {
        // Generate a random value between 0 and the total weight
        double randomValue = _random.NextDouble() * _totalWeight;
        double currentWeight = 0;

        // Find the item that corresponds to the random value
        foreach (var weightedItem in _items)
        {
            currentWeight += weightedItem.Weight;
            if (randomValue < currentWeight)
            {
                return weightedItem.Item;
            }
        }

        // Fallback in case of any rounding errors - return the last item
        return _items.Last().Item;
    }
}