// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Hooks.Core.Enums;

namespace Mercury.PowerShell.Hooks.Core.ComplexTypes;

/// <summary>
///   Represents a hook store.
/// </summary>
public readonly struct HookStore : IEquatable<HookStore>, IEqualityComparer<HookStore> {
  /// <summary>
  ///   The type of the hook store.
  /// </summary>
  public required HookType Type { get; init; }

  /// <summary>
  ///   The items in the hook store.
  /// </summary>
  public required HashSet<HookStoreItem> Items { get; init; }

  [SetsRequiredMembers]
  private HookStore(HookType type, HashSet<HookStoreItem> items) {
    Type = type;
    Items = items;
  }

  /// <summary>
  ///   Create a new hook store.
  /// </summary>
  /// <param name="type">The type of the hook store.</param>
  /// <param name="items">The items in the hook store.</param>
  /// <returns>The new hook store.</returns>
  public static HookStore NewStore(HookType type, HashSet<HookStoreItem> items)
    => new(type, items);

  /// <summary>
  ///   Create a new hook store with no items.
  /// </summary>
  /// <param name="type">The type of the hook store.</param>
  /// <returns>The new hook store.</returns>
  public static HookStore NewStore(HookType type)
    => new(type, []);

  /// <inheritdoc />
  public bool Equals(HookStore other)
    => Type == other.Type && Items.Equals(other.Items);

  /// <inheritdoc />
  public override bool Equals(object? obj)
    => obj is HookStore other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
    => HashCode.Combine((int)Type, Items);

  /// <inheritdoc />
  public bool Equals(HookStore x, HookStore y)
    => x.Type == y.Type && x.Items.Equals(y.Items);

  /// <inheritdoc />
  public int GetHashCode(HookStore obj)
    => HashCode.Combine((int)obj.Type, obj.Items);

  public static bool operator ==(HookStore left, HookStore right)
    => left.Equals(right);

  public static bool operator !=(HookStore left, HookStore right)
    => !left.Equals(right);
}
