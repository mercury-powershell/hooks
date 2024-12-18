// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Cmdlets.Abstractions;
using Mercury.PowerShell.Hooks.Core;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Core.Extensions;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Proxy cmdlet to the <c>Out-Default</c> cmdlet.
/// </summary>
[Cmdlet(VerbsData.Out, "Default", HelpUri = "https://go.microsoft.com/fwlink/?LinkID=2096486", RemotingCapability = RemotingCapability.None)]
public sealed class OutDefaultCmdlet() : PSProxyCmdlet("Microsoft.PowerShell.Core\\Out-Default") {
  /// <summary>
  ///   Determines whether the output should be sent to PowerShell's transcription services.
  /// </summary>
  [Parameter]
  public SwitchParameter Transcript { get; init; } = true;

  /// <summary>
  ///   Accepts input to the cmdlet.
  /// </summary>
  [Parameter(ValueFromPipeline = true)]
  public PSObject InputObject { get; init; } = default!;

  /// <inheritdoc />
  protected override object PipelineDefinedParameters()
    => GetParameter(this, cmdlet => cmdlet.InputObject);

  /// <inheritdoc />
  protected override void OnEndProcessing() {
    var hookTypeKey = HookType.PrePrompt.GetVariableKey();

    if (!StateManager.TryGetValue(hookTypeKey, out HookStore hookStore)) {
      return;
    }

    HookStore.InvokeAll(hookStore.Items);
  }
}
