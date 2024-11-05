// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Core;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.ComplexTypes.Options;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks;

/// <summary>
///   The module assembly initializer.
/// </summary>
public sealed class ModuleAssemblyInitializer : IModuleAssemblyInitializer {
  /// <inheritdoc />
  public void OnImport() {
    PreRegisterHookOptions();
    PreRegisterHookStores();
  }

  private static void PreRegisterHookStores() {
    var availableHooks = new[] {
      HookType.ChangeWorkingDirectory,
      HookType.PrePrompt
    };

    foreach (var availableHook in availableHooks) {
      var availableHookKey = availableHook.GetVariableKey();

      if (!StateManager.TryGetValue<HookStore>(availableHookKey, out var _)) {
        StateManager.AddOrUpdate(availableHookKey, HookStore.NewStore(availableHook));
      }
    }
  }

  private static void PreRegisterHookOptions() {
    if (!StateManager.TryGetValue<HookOptions>(HookOptions.KEY, out var _)) {
      StateManager.AddOrUpdate(HookOptions.KEY, HookOptions.InitialValue);
    }
  }
}
