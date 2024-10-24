// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Core;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;
using Mercury.PowerShell.Hooks.Exceptions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to register a hook.
/// </summary>
[OutputType(typeof(HookStoreItem))]
[Cmdlet(VerbsLifecycle.Register, "Hook")]
public sealed class RegisterHookCmdlet : PSCmdlet {
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
  protected override void BeginProcessing()
    => StateManager.InitialState();

  /// <inheritdoc />
  protected override void ProcessRecord() {
    var hookTypeKey = Type.GetVariableKey();
    var hookStore = StateManager.Get<HookStore>(hookTypeKey);

    var hookItem = HookStoreItem.NewItem(Identifier, Action);

    if (!hookStore.Items.Add(hookItem)) {
      WriteError(IdentifierAlreadyExistsException.AsRecord(Type, Identifier));
      return;
    }

    if (PassThru.IsPresent) {
      WriteObject(hookItem);
    }

    StateManager.AddOrUpdate(hookTypeKey, hookStore);
  }
}
