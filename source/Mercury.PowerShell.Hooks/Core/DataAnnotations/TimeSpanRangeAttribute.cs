// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;

namespace Mercury.PowerShell.Hooks.Core.DataAnnotations;

/// <summary>
///   Represents a validation attribute that specifies that a data field value must be a <see cref="TimeSpan" /> within a specified range.
/// </summary>
/// <param name="minimum">The minimum value of the <see cref="TimeSpan" />.</param>
/// <param name="maximum">The maximum value of the <see cref="TimeSpan" />.</param>
internal sealed class TimeSpanRangeAttribute(long minimum, long maximum) : ValidationAttribute {
  /// <inheritdoc />
  public override bool IsValid(object? value) {
    if (value is not TimeSpan timeSpan) {
      return false;
    }

    var minimumTimeSpan = new TimeSpan(minimum);
    var maximumTimeSpan = new TimeSpan(maximum);

    return timeSpan >= minimumTimeSpan && timeSpan <= maximumTimeSpan;
  }

  /// <inheritdoc />
  public override string FormatErrorMessage(string name)
    => $"{name} must be between {minimum} and {maximum}.";
}
