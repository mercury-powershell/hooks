// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;
using Mercury.PowerShell.Hooks.Core.DataAnnotations;

namespace Mercury.PowerShell.Hooks.Core.ComplexTypes.Options;

/// <summary>
///   Represents the options for the module hooks.
/// </summary>
public struct HookOptions : IEquatable<HookOptions> {
  private const long MINIMUM_TIME_SPAN = 10000000;
  private const long MAXIMUM_TIME_SPAN = 9223372036854775807;

  /// <summary>
  ///   The key of the hook options.
  /// </summary>
  public const string KEY = "Mercury_HookOptions";

  /// <summary>
  ///   The initial value of the hook options.
  /// </summary>
  public static readonly HookOptions InitialValue = new() {
    MaxDegreeOfParallelism = 4,
    PeriodicTimeSpan = TimeSpan.FromSeconds(10)
  };

  /// <summary>
  ///   The maximum degree of parallelism of the hooks.
  /// </summary>
  [Required(ErrorMessage = "The maximum degree of parallelism is required.")]
  [GreaterOrEqualTo(1, ErrorMessage = "The maximum degree of parallelism must be greater than zero.")]
  public required int MaxDegreeOfParallelism { get; set; }

  /// <summary>
  ///   The periodic time span of the periodic hooks.
  /// </summary>
  [Required(ErrorMessage = "The periodic time span is required.")]
  [TimeSpanRange(MINIMUM_TIME_SPAN, MAXIMUM_TIME_SPAN, ErrorMessage = "The periodic time span must be greater than zero.")]
  public required TimeSpan PeriodicTimeSpan { get; set; }

  /// <inheritdoc />
  public bool Equals(HookOptions other)
    => MaxDegreeOfParallelism == other.MaxDegreeOfParallelism &&
       PeriodicTimeSpan.Equals(other.PeriodicTimeSpan);

  /// <inheritdoc />
  public override bool Equals(object? obj)
    => obj is HookOptions other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
    => HashCode.Combine(MaxDegreeOfParallelism, PeriodicTimeSpan);

  public static bool operator ==(HookOptions left, HookOptions right) => left.Equals(right);

  public static bool operator !=(HookOptions left, HookOptions right) => !(left == right);
}
