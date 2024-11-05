// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.ComponentModel.DataAnnotations;

namespace Mercury.PowerShell.Hooks.Core;

/// <summary>
///   Methods to validate data annotations.
/// </summary>
internal static class DataAnnotationsValidator {
  /// <summary>
  ///   Validates the instance and throws an exception if the validation fails.
  /// </summary>
  /// <param name="instance">The instance to validate.</param>
  public static void ValidateAndThrow(object instance) {
    var context = new ValidationContext(instance);
    var validationResult = new List<ValidationResult>();

    if (!Validator.TryValidateObject(instance, context, validationResult, true)) {
      throw new AggregateException(validationResult.Select(result => new ValidationException(result.ErrorMessage)));
    }
  }
}
