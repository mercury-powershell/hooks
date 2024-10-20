// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Cmdlets.Abstractions;
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
  /// <remarks>
  ///   The <c>Out-Default</c> cmdlet proxy only works normally when the <c>InputObject</c> parameter is forwarded to the <c>Out-Default</c> cmdlet.
  ///   This is because the <c>Out-Default</c> cmdlet is a special cmdlet that is not intended to be used directly.
  /// </remarks>
  protected override void ProcessRecord()
    => Parallel.Invoke(() => SteppablePipeline?.Process(GetParameter(this, cmdlet => cmdlet.InputObject)), OnProcessRecord);

  /// <inheritdoc />
  protected override void OnEndProcessing() {
    var variableKey = HookType.PrePrompt.GetVariableKey();
    var hookVariable = SessionState.PSVariable.Get(variableKey);

    if (hookVariable?.Value is not HookStore hookStore) {
      return;
    }

    Parallel.ForEach(hookStore.Items, new ParallelOptions {
      MaxDegreeOfParallelism = 4
    }, item => item.Action.Invoke());
  }
}
