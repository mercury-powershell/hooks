// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.Hooks.Core.Enums;

/// <summary>
///   Available hook types.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum HookType {
  /// <summary>
  ///   Hook to be executed when the <c>$PWD</c> is changed.
  /// </summary>
  ChangeWorkingDirectory,

  /// <summary>
  ///   Hook to be executed before the prompt is displayed.
  /// </summary>
  PrePrompt
}
