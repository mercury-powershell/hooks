// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using Mercury.PowerShell.Hooks.Testing.Tools.Abstractions;

namespace Mercury.PowerShell.Hooks.Testing.Tools;

/// <summary>
///   Helper class to invoke cmdlets.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class PSCmdletHelper {
  private const string HELP_FILE_NAME = "Microsoft.Windows.Installer.PowerShell.dll-Help.xml";

  private static Pipeline createPipeline<TCmdlet>(Action<IInvokationPrepare> action) where TCmdlet : PSCmdlet {
    InvokationPrepare.Configure(action, out var variableEntries, out var parameters);

    var cmdletAttribute = typeof(TCmdlet).GetCustomAttribute<CmdletAttribute>(true)
                          ?? throw new ArgumentException($"The cmdlet '{typeof(TCmdlet).Name}' does not have the 'CmdletAttribute'.");
    var cmdletName = $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";
    var command = new Command(cmdletName);

    var initialSessionState = InitialSessionState.CreateDefault2();

    initialSessionState.Variables.Add(variableEntries);
    initialSessionState.Commands.Add(new SessionStateCmdletEntry(cmdletName, typeof(TCmdlet), HELP_FILE_NAME));

    var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
    runspace.ApartmentState = ApartmentState.STA;
    runspace.Open();

    var pipeline = runspace.CreatePipeline();

    foreach (var parameter in parameters) {
      command.Parameters.Add(parameter);
    }

    pipeline.Commands.Add(command);

    return pipeline;
  }

  /// <summary>
  ///   Invokes a cmdlet and returns the output.
  /// </summary>
  /// <param name="action">The action to prepare the invokation.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The output of the cmdlet.</returns>
  /// <exception cref="ArgumentException">The cmdlet does not have the <see cref="CmdletAttribute" />.</exception>
  public static IEnumerable<PSObject> InvokeCollection<TCmdlet>(Action<IInvokationPrepare> action) where TCmdlet : PSCmdlet {
    using var pipeline = createPipeline<TCmdlet>(action);

    return pipeline.Invoke();
  }

  /// <summary>
  ///   Invokes a cmdlet and returns the output.
  /// </summary>
  /// <param name="action">The action to prepare the invokation.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The output of the cmdlet.</returns>
  /// <exception cref="ArgumentException">The cmdlet does not have the <see cref="CmdletAttribute" />.</exception>
  public static PSObject? Invoke<TCmdlet>(Action<IInvokationPrepare> action) where TCmdlet : PSCmdlet
    => InvokeCollection<TCmdlet>(action).FirstOrDefault();

  /// <summary>
  ///   Invokes a cmdlet and returns a collection of error records.
  /// </summary>
  /// <param name="action">The action to prepare the invokation.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The error records thrown by the cmdlet; otherwise, an empty collection.</returns>
  /// <exception cref="ArgumentException">The cmdlet does not have the <see cref="CmdletAttribute" />.</exception>
  public static IEnumerable<ErrorRecord> TryInvokeCollection<TCmdlet>(Action<IInvokationPrepare> action) where TCmdlet : PSCmdlet {
    using var pipeline = createPipeline<TCmdlet>(action);

    pipeline.Invoke();

    if (!pipeline.HadErrors) {
      return [];
    }

    return pipeline.Error
      .ReadToEnd()
      .Cast<PSObject>()
      .Select(psobject => psobject.BaseObject)
      .Cast<ErrorRecord>();
  }

  /// <summary>
  ///   Invokes a cmdlet and returns an error record.
  /// </summary>
  /// <param name="action">The action to prepare the invokation.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The error record thrown by the cmdlet; otherwise, <see langword="null" />.</returns>
  /// <exception cref="ArgumentException">The cmdlet does not have the <see cref="CmdletAttribute" />.</exception>
  public static ErrorRecord? TryInvoke<TCmdlet>(Action<IInvokationPrepare> action) where TCmdlet : PSCmdlet
    => TryInvokeCollection<TCmdlet>(action).FirstOrDefault();
}
