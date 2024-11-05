// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Core;
using Mercury.PowerShell.Hooks.Core.ComplexTypes.Options;

namespace Mercury.PowerShell.Hooks.Cmdlets;

/// <summary>
///   Cmdlet to set the hooks options.
/// </summary>
[Cmdlet(VerbsCommon.Set, "HookOption")]
public sealed class SetHookOptionCmdlet : PSCmdlet {
  /// <summary>
  ///   The maximum degree of parallelism of the hooks.
  /// </summary>
  [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = false)]
  public int? MaxDegreeOfParallelism { get; set; }

  /// <summary>
  ///   The periodic time span of the periodic hooks.
  /// </summary>
  [Parameter(Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = false)]
  public TimeSpan? PeriodicTimeSpan { get; set; }

  /// <inheritdoc />
  protected override void ProcessRecord() {
    if (!StateManager.TryGetValue<HookOptions>(HookOptions.KEY, out var options)) {
      options = HookOptions.InitialValue;
    }

    var configuredOptions = Configure(options);

    DataAnnotationsValidator.ValidateAndThrow(configuredOptions);
    StateManager.AddOrUpdate(HookOptions.KEY, configuredOptions);
  }

  private HookOptions Configure(HookOptions options) {
    options.MaxDegreeOfParallelism = MaxDegreeOfParallelism ?? options.MaxDegreeOfParallelism;
    options.PeriodicTimeSpan = PeriodicTimeSpan ?? options.PeriodicTimeSpan;

    return options;
  }
}
