using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ItFaker.Models;

/// <summary>
/// Represents a person's gender
/// </summary>
public enum Gender
{
    /// <summary>
    /// Male gender
    /// </summary>
    Male,

    /// <summary>
    /// Female gender
    /// </summary>
    Female
}

/// <summary>
/// Represents an item with an associated probability weight
/// </summary>
/// <typeparam name="T">The type of the item</typeparam>
public class WeightedItem<T>
{
    /// <summary>
    /// The item itself
    /// </summary>
    [JsonPropertyName("value")]
    public required T Item { get; set; }

    /// <summary>
    /// The weight (probability) of the item
    /// </summary>
    [JsonPropertyName("weight")]
    public double Weight { get; set; }
}

/// <summary>
/// Represents a collection of weighted items
/// </summary>
public class WeightedData
{
    /// <summary>
    /// The collection of weighted items
    /// </summary>
    [JsonPropertyName("items")]
    public required List<WeightedItem<string>> Items { get; set; }
}