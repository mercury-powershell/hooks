// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Runspaces;
using Mercury.PowerShell.Hooks.Core.ComplexTypes.Options;
using Mercury.PowerShell.Hooks.Core.Enums;
using Pwsh = System.Management.Automation.PowerShell;

namespace Mercury.PowerShell.Hooks.Core.ComplexTypes;

/// <summary>
///   Represents a hook store.
/// </summary>
public readonly struct HookStore : IEquatable<HookStore> {
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
    => HashCode.Combine(Type, Items);

  /// <summary>
  ///   Check if two hook stores are equal.
  /// </summary>
  /// <param name="left">The left hook store.</param>
  /// <param name="right">The right hook store.</param>
  /// <returns>True if the hook stores are equal, false otherwise.</returns>
  public static bool operator ==(HookStore left, HookStore right)
    => left.Equals(right);

  /// <summary>
  ///   Check if two hook stores are different.
  /// </summary>
  /// <param name="left">The left hook store.</param>
  /// <param name="right">The right hook store.</param>
  /// <returns>True if the hook stores are different, false otherwise.</returns>
  public static bool operator !=(HookStore left, HookStore right)
    => !left.Equals(right);

  /// <summary>
  ///   Parallel invoke all the actions in the hook store.
  /// </summary>
  /// <param name="items">The hook store items.</param>
  /// <returns>A task representing the operation.</returns>
  public static Task InvokeAll(HashSet<HookStoreItem> items) => Task.Run(() => {
    if (!StateManager.TryGetValue<HookOptions>(HookOptions.KEY, out var options)) {
      options = HookOptions.InitialValue;
    }

    using var runspacePool = RunspaceFactory.CreateRunspacePool(1, options.MaxDegreeOfParallelism);
    runspacePool.ThreadOptions = PSThreadOptions.UseNewThread;
    runspacePool.ApartmentState = ApartmentState.MTA;
    runspacePool.Open();

    var tasks = items.Select(item => Task.Run(() => {
        using var ps = Pwsh.Create();
        ps.RunspacePool = runspacePool;

        try {
          item.Action.Invoke();
        }
        catch {
          // Log the exception
          // Do nothing for now
        }
      }))
      .ToArray();

    Task.WaitAll(tasks);
  });
}
