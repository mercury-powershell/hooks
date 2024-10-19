// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.ArgumentCompleters.Attributes;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;
using Mercury.PowerShell.Hooks.Exceptions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to unregister a proxy hook.
/// </summary>
[OutputType(typeof(HookStoreItem))]
[Cmdlet(VerbsLifecycle.Unregister, "Hook")]
public sealed class UnregisterHookCmdlet : PSCmdlet {
  /// <summary>
  ///   The type of the hook.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required HookType Type { get; init; }

  /// <summary>
  ///   The unique identifier of the hook.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  [HookIdentifierCompleter]
  public required string Identifier { get; init; }

  /// <summary>
  ///   Passes an object representing the hook to the pipeline. By default, this cmdlet does not generate any output.
  /// </summary>
  [Parameter(Position = 2, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  public SwitchParameter PassThru { get; init; }

  /// <inheritdoc />
  protected override void ProcessRecord() {
    var variableKey = Type.GetVariableKey();
    var hookVariable = SessionState.PSVariable.Get(variableKey);

    if (hookVariable?.Value is not HookStore hookStore) {
      return;
    }

    var item = hookStore.Items.FirstOrDefault(item => item.Identifier == Identifier);

    if (!hookStore.Items.Remove(item) &&
        PassThru.IsPresent) {
      WriteError(IdentifierNotFoundException.AsRecord(Type, Identifier));

      return;
    }

    if (PassThru.IsPresent) {
      WriteObject(item);
    }
  }
}
