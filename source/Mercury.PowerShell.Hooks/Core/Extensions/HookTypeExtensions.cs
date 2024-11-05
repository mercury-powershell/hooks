// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Hooks.Core.Enums;

namespace Mercury.PowerShell.Hooks.Core.Extensions;

/// <summary>
///   Extension methods for the <see cref="HookType" /> enum.
/// </summary>
[ExcludeFromCodeCoverage]
internal static class HookTypeExtensions {
  private const string PREFIX = "Mercury_HookStore_";

  /// <summary>
  ///   Get the variable key for the hook type.
  /// </summary>
  /// <param name="hookType">The hook type.</param>
  /// <returns>The variable key.</returns>
  /// <exception cref="ArgumentOutOfRangeException">If the hook type is not supported.</exception>
  public static string GetVariableKey(this HookType hookType)
    => hookType switch {
      HookType.ChangeWorkingDirectory => $"{PREFIX}{nameof(HookType.ChangeWorkingDirectory)}",
      HookType.Periodic => $"{PREFIX}{nameof(HookType.Periodic)}",
      HookType.PrePrompt => $"{PREFIX}{nameof(HookType.PrePrompt)}",
      var _ => throw new ArgumentOutOfRangeException(nameof(hookType), hookType, null)
    };
}
