// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;

namespace Mercury.PowerShell.Hooks.Core.ComplexTypes;

/// <summary>
///   Represents a hook store item.
/// </summary>
public readonly struct HookStoreItem : IEquatable<HookStoreItem>, IEqualityComparer<HookStoreItem> {
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
    => Identifier == other.Identifier && Action.Equals(other.Action);

  /// <inheritdoc />
  public override bool Equals(object? obj)
    => obj is HookStoreItem other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
    => HashCode.Combine(Identifier, Action);

  /// <inheritdoc />
  public bool Equals(HookStoreItem x, HookStoreItem y)
    => x.Identifier == y.Identifier && x.Action.Equals(y.Action);

  /// <inheritdoc />
  public int GetHashCode(HookStoreItem obj)
    => HashCode.Combine(obj.Identifier, obj.Action);

  public static bool operator ==(HookStoreItem left, HookStoreItem right)
    => left.Equals(right);

  public static bool operator !=(HookStoreItem left, HookStoreItem right)
    => !left.Equals(right);
}
