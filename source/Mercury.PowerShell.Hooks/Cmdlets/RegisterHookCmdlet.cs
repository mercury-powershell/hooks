// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to register a hook.
/// </summary>
[OutputType(typeof(HookStoreItem))]
[Cmdlet(VerbsLifecycle.Register, "Hook")]
public sealed class RegisterHookCmdlet : PSCmdlet {
  internal static readonly Dictionary<string, PSVariable> _hookVariables = new();

  /// <summary>
  ///   The type of the hook.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required HookType Type { get; init; }

  /// <summary>
  ///   The unique identifier of the hook.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required string Identifier { get; init; }

  /// <summary>
  ///   The action to be executed when the hook is triggered.
  /// </summary>
  [Parameter(Position = 2, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required ScriptBlock Action { get; init; }

  /// <summary>
  ///   Passes an object representing the hook to the pipeline. By default, this cmdlet does not generate any output.
  /// </summary>
  [Parameter(Position = 3, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public SwitchParameter PassThru { get; init; }

  /// <inheritdoc />
  protected override void BeginProcessing() {
    var availableHooks = new[] {
      HookType.ChangeWorkingDirectory,
      HookType.PrePrompt
    };

    foreach (var availableHook in availableHooks) {
      var availableHookKey = availableHook.GetVariableKey();
      var hooksVariable = SessionState.PSVariable.Get(availableHookKey) ??
                          new PSVariable(availableHookKey, HookStore.NewStore(availableHook), ScopedItemOptions.Private);

      if (_hookVariables.TryGetValue(availableHookKey, out var existingVariable)) {
        if (existingVariable.Value is not HookStore existingVariableStore ||
            hooksVariable.Value is not HookStore hooksVariableStore) {
          throw new InvalidOperationException("The hook store is not valid.");
        }

        if (existingVariableStore != hooksVariableStore) {
          _hookVariables.Remove(availableHookKey);
        }
      }

      _hookVariables.TryAdd(availableHookKey, hooksVariable);
    }
  }

  /// <inheritdoc />
  protected override void ProcessRecord() {
    var entryName = Type.GetVariableKey();

    if (!_hookVariables.TryGetValue(entryName, out var entry)) {
      throw new InvalidOperationException("The hook store is not valid.");
    }

    if (entry.Value is not HookStore hookStore) {
      throw new InvalidOperationException("The hook store is not valid.");
    }

    var hookItem = HookStoreItem.NewItem(Identifier, Action);

    if (!hookStore.Items.Add(hookItem)) {
      WriteObject($"The hook with identifier '{Identifier}' already exists in the '{Type}' hook store.");
    }

    if (PassThru.IsPresent) {
      WriteObject(hookItem);
    }
  }

  /// <inheritdoc />
  protected override void EndProcessing() {
    foreach (var variable in _hookVariables.Values) {
      SessionState.PSVariable.Set(variable);
    }
  }
}
