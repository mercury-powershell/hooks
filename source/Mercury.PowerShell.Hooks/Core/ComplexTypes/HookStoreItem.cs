// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.Hooks.Core.ComplexTypes;

/// <summary>
///   Represents a hook store item.
/// </summary>
public readonly struct HookStoreItem : IEquatable<HookStoreItem> {
  /// <summary>
  ///   The unique identifier of the hook.
  /// </summary>
  public required string Identifier { get; init; }

  /// <summary>
  ///   The action to be executed when the hook is triggered.
  /// </summary>
  public required ScriptBlock Action { get; init; }

  [SetsRequiredMembers]
  private HookStoreItem(string identifier, ScriptBlock action) {
    Identifier = identifier;
    Action = action;
  }

  /// <summary>
  ///   Create a new hook store item.
  /// </summary>
  /// <param name="identifier">The unique identifier of the hook.</param>
  /// <param name="action">The action to be executed when the hook is triggered.</param>
  /// <returns>The new hook store item.</returns>
  public static HookStoreItem NewItem(string identifier, ScriptBlock action)
    => new(identifier, action);

  /// <inheritdoc />
  public bool Equals(HookStoreItem other)
    => Identifier.Equals(other.Identifier, StringComparison.OrdinalIgnoreCase);

  /// <inheritdoc />
  public override bool Equals(object? obj)
    => obj is HookStoreItem other &&
       Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
    => Identifier.GetHashCode();

  /// <summary>
  ///   Check if two hook store items are equal.
  /// </summary>
  /// <param name="left">The left hook store item.</param>
  /// <param name="right">The right hook store item.</param>
  /// <returns>True if the hook store items are equal, false otherwise.</returns>
  public static bool operator ==(HookStoreItem left, HookStoreItem right)
    => left.Equals(right);

  /// <summary>
  ///   Check if two hook store items are different.
  /// </summary>
  /// <param name="left">The left hook store item.</param>
  /// <param name="right">The right hook store item.</param>
  /// <returns>True if the hook store items are different, false otherwise.</returns>
  public static bool operator !=(HookStoreItem left, HookStoreItem right)
    => !left.Equals(right);
}
