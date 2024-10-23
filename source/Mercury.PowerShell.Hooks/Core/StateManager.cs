// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Core;

/// <summary>
///   Manages the state of the current thread.
/// </summary>
internal static class StateManager {
  private static readonly ConcurrentDictionary<string, object> _threadState;

  static StateManager()
    => _threadState = [];

  /// <summary>
  ///   Initializes the state manager with empty stores for the available hooks.
  /// </summary>
  public static void InitialState() {
    var availableHooks = new[] {
      HookType.ChangeWorkingDirectory,
      HookType.PrePrompt
    };

    foreach (var availableHook in availableHooks) {
      var availableHookKey = availableHook.GetVariableKey();

      if (!TryGetValue<HookStore>(availableHookKey, out var _)) {
        AddOrUpdate(availableHookKey, HookStore.NewStore(availableHook));
      }
    }
  }

  /// <summary>
  ///   Adds a key/value pair to the <see cref="StateManager" /> if the key does not already exist, or updates a key/value pair in the
  ///   <see cref="StateManager" /> if the key already exists.
  /// </summary>
  /// <param name="key">The key to be added or whose value should be updated.</param>
  /// <param name="value">The value to be added or updated.</param>
  /// <typeparam name="TIn">The type of the value to store.</typeparam>
  public static void AddOrUpdate<TIn>(string key, TIn value) where TIn : notnull
    => _threadState.AddOrUpdate(key, value, (_, _) => value);

  /// <summary>
  ///   Attempts to get the value associated with the specified key from the <see cref="StateManager" />.
  /// </summary>
  /// <param name="key">The key of the value to get.</param>
  /// <param name="value">
  ///   When this method returns, contains the object from the <see cref="StateManager" /> that has the specified key, or the default value of the type if
  ///   the operation failed.
  /// </param>
  /// <typeparam name="TOut">The type of the value to get.</typeparam>
  /// <returns><c>true</c> if the value was found; otherwise, <c>false</c>.</returns>
  public static bool TryGetValue<TOut>(string key, [NotNullWhen(true)] out TOut? value) where TOut : notnull {
    if (_threadState.TryGetValue(key, out var obj) &&
        obj is TOut outValue) {
      value = outValue;
      return true;
    }

    value = default;
    return false;
  }

  /// <summary>
  ///   Gets the value associated with the specified key from the <see cref="StateManager" />.
  /// </summary>
  /// <param name="key">The key of the value to get.</param>
  /// <typeparam name="TOut">The type of the value to get.</typeparam>
  /// <returns>The value associated with the specified key.</returns>
  /// <exception cref="KeyNotFoundException">If the <paramref name="key" /> was not found in the state manager.</exception>
  public static TOut Get<TOut>(string key) where TOut : notnull
    => TryGetValue<TOut>(key, out var outValue)
      ? outValue
      : throw new KeyNotFoundException($"The key '{key}' was not found in the state manager.");

  /// <summary>
  ///   Attempts to remove and return the value that has the specified key from the <see cref="StateManager" />.
  /// </summary>
  /// <param name="key">The key of the element to remove.</param>
  /// <returns><c>true</c> if the element was removed; otherwise, <c>false</c>.</returns>
  /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <c>null</c>.</exception>
  public static bool Remove(string key) {
    ArgumentNullException.ThrowIfNull(key);

    return _threadState.TryRemove(key, out var _);
  }
}
