// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Cmdlets.Abstractions;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Proxy cmdlet to the <c>Pop-Location</c> cmdlet.
/// </summary>
[Cmdlet(VerbsCommon.Pop, "Location", HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096907")]
public sealed class PopLocationCmdlet() : PSProxyCmdlet("Microsoft.PowerShell.Management\\Pop-Location") {
  /// <summary>
  ///   Passes an object that represents the location to the pipeline. By default, this cmdlet does not generate any output.
  /// </summary>
  [Parameter]
  public SwitchParameter PassThru { get; init; }

  /// <summary>
  ///   Specifies the location stack from which the location is popped. Enter a location stack name.
  /// </summary>
  /// <remarks>
  ///   Without this parameter, Pop-Location pops a location from the current location stack. By default, the current location stack is the unnamed default
  ///   location stack that PowerShell creates. To make a location stack the current location stack, use the StackName parameter of the Set-Location
  ///   cmdlet. For more information about location stacks, see the Notes section.
  /// </remarks>
  /// <seealso href="https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.management/pop-location?view=powershell-7.4#notes">Notes</seealso>
  [Parameter(ValueFromPipelineByPropertyName = true)]
  public string? StackName { get; init; }

  /// <inheritdoc />
  protected override void OnEndProcessing() {
    var variableKey = HookType.ChangeWorkingDirectory.GetVariableKey();
    var hooksVariable = SessionState.PSVariable.Get(variableKey);

    if (hooksVariable is null) {
      return;
    }

    try {
      if (hooksVariable.Value is not HookStore hookStore) {
        return;
      }

      Parallel.ForEach(hookStore.Items, new ParallelOptions {
        MaxDegreeOfParallelism = 4
      }, item => item.Action.Invoke());
    }
    catch (Exception ex) {
      WriteError(new ErrorRecord(ex, "InvokeHooksFailed", ErrorCategory.InvalidOperation, null));
    }
  }
}
