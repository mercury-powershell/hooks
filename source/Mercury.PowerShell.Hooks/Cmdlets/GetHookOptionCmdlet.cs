// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Core;
using Mercury.PowerShell.Hooks.Core.ComplexTypes.Options;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to get the hook options.
/// </summary>
[OutputType(typeof(HookOptions))]
[Cmdlet(VerbsCommon.Get, "HookOption")]
public sealed class GetHookOptionCmdlet : PSCmdlet {
  /// <inheritdoc />
  protected override void ProcessRecord() {
    if (!StateManager.TryGetValue<HookOptions>(HookOptions.KEY, out var options)) {
      options = HookOptions.InitialValue;
    }

    WriteObject(options);
  }
}
