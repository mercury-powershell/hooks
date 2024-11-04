// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Cmdlets.Abstractions;
using Mercury.PowerShell.Hooks.Core;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Proxy cmdlet to the <c>Push-Location</c> cmdlet.
/// </summary>
[Cmdlet(VerbsCommon.Push, "Location", DefaultParameterSetName = PATH_PARAMETER_SET, HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2097105")]
public sealed class PushLocationCmdlet() : PSProxyCmdlet("Microsoft.PowerShell.Management\\Push-Location") {
  private const string PATH_PARAMETER_SET = "Path";
  private const string LITERAL_PATH_PARAMETER_SET = "LiteralPath";

  /// <summary>
  ///   Changes your location to the location specified by this path after it adds (pushes) the current location onto the top of the stack. Enter a path to
  ///   any location whose provider supports this cmdlet. Wildcards are permitted. The parameter name is optional.
  /// </summary>
  [Parameter(Position = 0, ParameterSetName = PATH_PARAMETER_SET, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
  [Alias("PSPath", "LP")]
  [ValidateNotNullOrEmpty]
  public string Path { get; init; } = string.Empty;

  /// <summary>
  ///   Specifies the path to the new location. Unlike the Path parameter, the value of the LiteralPath parameter is used exactly as it is typed. No
  ///   characters are interpreted as wildcards. If the path includes escape characters, enclose it in single quotation marks. Single quotation marks tell
  ///   PowerShell not to interpret any characters as escape sequences
  /// </summary>
  [Parameter(ParameterSetName = LITERAL_PATH_PARAMETER_SET, ValueFromPipelineByPropertyName = true)]
  [ValidateNotNullOrEmpty]
  public string LiteralPath { get; init; } = default!;

  /// <summary>
  ///   Passes an object representing the location to the pipeline. By default, this cmdlet does not generate any output.
  /// </summary>
  [Parameter]
  public SwitchParameter PassThru { get; init; }

  /// <summary>
  ///   Specifies the location stack to which the current location is added. Enter a location stack name. If the stack does not exist, Push-Location
  ///   creates it.
  /// </summary>
  /// <remarks>
  ///   Without this parameter, Push-Location adds the location to the current location stack. By default, the current location stack is the unnamed
  ///   default location stack that PowerShell creates. To make a location stack the current location stack, use the StackName parameter of the
  ///   Set-Location cmdlet. For more information about location stacks, see the Notes section.
  /// </remarks>
  /// <seealso href="https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.management/push-location?view=powershell-7.4#notes">Notes</seealso>
  [Parameter(ValueFromPipelineByPropertyName = true)]
  public string StackName { get; init; } = default!;

  /// <inheritdoc />
  protected override void OnEndProcessing() {
    var hookTypeKey = HookType.ChangeWorkingDirectory.GetVariableKey();

    if (!StateManager.TryGetValue(hookTypeKey, out HookStore hookStore)) {
      return;
    }

    HookStore.InvokeAll(hookStore.Items);
  }
}
