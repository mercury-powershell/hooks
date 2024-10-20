// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.ArgumentCompleters.Attributes;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to get a hook or a hook store.
/// </summary>
[OutputType(typeof(HookStore), typeof(HookStoreItem))]
[Cmdlet(VerbsCommon.Get, "Hook")]
public sealed class GetHookCmdlet : PSCmdlet {
  /// <summary>
  ///   The type of the hook.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
  public required HookType Type { get; init; }

  /// <summary>
  ///   The identifier of the hook.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [HookIdentifierCompleter]
  public string? Identifier { get; init; }

  /// <inheritdoc />
  protected override void ProcessRecord() {
    var variableKey = Type.GetVariableKey();
    var hookVariable = SessionState.PSVariable.Get(variableKey);

    if (hookVariable?.Value is not HookStore hookStore) {
      return;
    }

    if (!string.IsNullOrWhiteSpace(Identifier)) {
      if (!hookStore.Items.Select(item => item.Identifier).Contains(Identifier, StringComparer.OrdinalIgnoreCase)) {
        return;
      }

      var item = hookStore.Items.FirstOrDefault(item => item.Identifier.Equals(Identifier, StringComparison.OrdinalIgnoreCase));

      WriteObject(item);
      return;
    }

    WriteObject(hookStore);
  }
}
