// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using Mercury.PowerShell.Hooks.Utilities;

namespace Mercury.PowerShell.Hooks.Cmdlets.Abstractions;

/// <summary>
///   Base class for proxy cmdlets.
/// </summary>
/// <param name="targetCommand">The target command to be proxied.</param>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public abstract class PSProxyCmdlet(string targetCommand) : PSCmdlet, IDynamicParameters, IDisposable {
  protected SteppablePipeline? SteppablePipeline { get; set; }

  /// <inheritdoc />
  public void Dispose() {
    GC.SuppressFinalize(this);
    SteppablePipeline?.Dispose();
  }

  /// <inheritdoc />
  public object GetDynamicParameters() {
    var commandInfo = InvokeCommand.GetCommand(targetCommand, CommandTypes.Cmdlet, MyInvocation.BoundParameters.Values.ToArray());
    var dynamicParams = commandInfo.Parameters.Where(pair => pair.Value.IsDynamic).ToList();

    if (dynamicParams.Count == 0) {
      return new object();
    }

    var runtimeDefinedParameterDictionary = new RuntimeDefinedParameterDictionary();

    foreach (var (key, parameterMetadata) in dynamicParams) {
      if (MyInvocation.MyCommand.Parameters.ContainsKey(key)) {
        continue;
      }

      var dynamicParam = new RuntimeDefinedParameter(parameterMetadata.Name, parameterMetadata.ParameterType, parameterMetadata.Attributes);
      runtimeDefinedParameterDictionary.Add(parameterMetadata.Name, dynamicParam);
    }

    return OnGetDynamicParameters(runtimeDefinedParameterDictionary);
  }

  /// <summary>
  ///   Method to be called when the cmdlet is initialized.
  /// </summary>
  /// <param name="runtimeDefinedParameterDictionary">The runtime defined parameters dictionary.</param>
  /// <returns>The runtime defined parameters dictionary.</returns>
  protected virtual object OnGetDynamicParameters(RuntimeDefinedParameterDictionary runtimeDefinedParameterDictionary)
    => runtimeDefinedParameterDictionary;

  /// <summary>
  ///   Action to be executed together with <see cref="BeginProcessing" /> method.
  /// </summary>
  protected virtual void OnBeginProcessing() { }

  /// <summary>
  ///   Action to be executed together with <see cref="ProcessRecord" /> method.
  /// </summary>
  protected virtual void OnProcessRecord() { }

  /// <summary>
  ///   Action to be executed together with <see cref="EndProcessing" /> method.
  /// </summary>
  protected virtual void OnEndProcessing() { }

  /// <inheritdoc />
  protected sealed override void BeginProcessing() {
    if (MyInvocation.BoundParameters.TryGetValue("OutBuffer", out var _)) {
      MyInvocation.BoundParameters["OutBuffer"] = 1;
    }

    var commandInfo = InvokeCommand.GetCommand(targetCommand, CommandTypes.Cmdlet);
    var scriptBlock = ScriptBlock.Create("& $CommandInfo @BoundParameters @UnboundArguments");
    SessionState.PSVariable.Set("CommandInfo", commandInfo);
    SessionState.PSVariable.Set("UnboundArguments", MyInvocation.UnboundArguments);
    SessionState.PSVariable.Set("BoundParameters", MyInvocation.BoundParameters);

    SteppablePipeline = scriptBlock.GetSteppablePipeline(MyInvocation.CommandOrigin);

    Parallel.Invoke(() => SteppablePipeline.Begin(this), OnBeginProcessing);
  }

  /// <inheritdoc />
  protected override void ProcessRecord()
    => Parallel.Invoke(() => SteppablePipeline?.Process(PSCmdletUtilities.GetParameters(this)), OnProcessRecord);

  /// <inheritdoc />
  protected sealed override void EndProcessing()
    => Parallel.Invoke(() => SteppablePipeline?.End(), OnEndProcessing);

  /// <inheritdoc />
  protected sealed override void StopProcessing()
    => Dispose();
}
