// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

namespace Mercury.PowerShell.Hooks.ArgumentCompleters.Attributes;

/// <summary>
///   Attribute to complete hook identifiers.
/// </summary>
public sealed class HookIdentifierCompleterAttribute : ArgumentCompleterAttribute, IArgumentCompleterFactory {
  /// <inheritdoc />
  public IArgumentCompleter Create()
    => new HookIdentifierCompleter();
}
