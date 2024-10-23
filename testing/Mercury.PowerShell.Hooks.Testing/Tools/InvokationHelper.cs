// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using Mercury.PowerShell.Hooks.Testing.Tools.Abstractions;

namespace Mercury.PowerShell.Hooks.Testing.Tools;

/// <summary>
///   Helper class to invoke cmdlets.
/// </summary>
public static class InvokationHelper {
  internal const string HELP_FILE_NAME = "Microsoft.Windows.Installer.PowerShell.dll-Help.xml";

  /// <summary>
  ///   Invokes a cmdlet and returns the output.
  /// </summary>
  /// <param name="configure">The configuration to be used.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The output of the cmdlet.</returns>
  /// <exception cref="ArgumentException">The cmdlet does not have the <see cref="CmdletAttribute" />.</exception>
  public static IEnumerable<PSObject> InvokeCollection<TCmdlet>(Action<IInvokationConfigure> configure) where TCmdlet : PSCmdlet {
    var (variableEntries, parameters, helpFileName) = InvokationConfigure.ToInvoke(configure);

    var cmdletAttribute = typeof(TCmdlet).GetCustomAttribute<CmdletAttribute>(true)
                          ?? throw new ArgumentException($"The cmdlet '{typeof(TCmdlet).Name}' does not have the 'CmdletAttribute'.");
    var cmdletName = $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";

    var initialSessionState = InitialSessionState.CreateDefault2();

    initialSessionState.Variables.Add(variableEntries);
    initialSessionState.Commands.Add(new SessionStateCmdletEntry(cmdletName, typeof(TCmdlet), helpFileName));

    using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
    runspace.ApartmentState = ApartmentState.STA;
    runspace.Open();

    using var pipeline = runspace.CreatePipeline();

    var command = new Command(cmdletName);

    foreach (var (key, value) in parameters) {
      command.Parameters.Add(new CommandParameter(key, value));
    }

    pipeline.Commands.Add(command);

    return pipeline.Invoke();
  }

  /// <summary>
  ///   Invokes a cmdlet and returns the output.
  /// </summary>
  /// <param name="configure">The configuration to be used.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The output of the cmdlet.</returns>
  public static PSObject? Invoke<TCmdlet>(Action<IInvokationConfigure> configure) where TCmdlet : PSCmdlet
    => InvokeCollection<TCmdlet>(configure).FirstOrDefault();

  /// <summary>
  ///   Invokes a cmdlet and returns the exception.
  /// </summary>
  /// <param name="configure">The configuration to be used.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The exception thrown by the cmdlet; otherwise, <see langword="null" />.</returns>
  public static Exception? Exception<TCmdlet>(Action<IInvokationConfigure> configure) where TCmdlet : PSCmdlet {
    try {
      InvokeCollection<TCmdlet>(configure);

      return null;
    }
    catch (Exception ex) {
      return ex;
    }
  }

  /// <summary>
  ///   Invokes a cmdlet and returns the error record.
  /// </summary>
  /// <param name="configure">The configuration to be used.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet to be invoked.</typeparam>
  /// <returns>The error record thrown by the cmdlet; otherwise, <see langword="null" />.</returns>
  /// <exception cref="ArgumentException">The cmdlet does not have the <see cref="CmdletAttribute" />.</exception>
  public static ErrorRecord? ErrorRecord<TCmdlet>(Action<IInvokationConfigure> configure) where TCmdlet : PSCmdlet {
    var (variableEntries, parameters, helpFileName) = InvokationConfigure.ToInvoke(configure);

    var cmdletAttribute = typeof(TCmdlet).GetCustomAttribute<CmdletAttribute>(true)
                          ?? throw new ArgumentException($"The cmdlet '{typeof(TCmdlet).Name}' does not have the 'CmdletAttribute'.");
    var cmdletName = $"{cmdletAttribute.VerbName}-{cmdletAttribute.NounName}";

    var initialSessionState = InitialSessionState.CreateDefault2();

    initialSessionState.Variables.Add(variableEntries);
    initialSessionState.Commands.Add(new SessionStateCmdletEntry(cmdletName, typeof(TCmdlet), helpFileName));

    using var runspace = RunspaceFactory.CreateRunspace(initialSessionState);
    runspace.ApartmentState = ApartmentState.STA;
    runspace.Open();

    using var pipeline = runspace.CreatePipeline();

    var command = new Command(cmdletName);

    foreach (var (key, value) in parameters) {
      command.Parameters.Add(new CommandParameter(key, value));
    }

    pipeline.Commands.Add(command);
    pipeline.Invoke();

    if (!pipeline.HadErrors) {
      return null;
    }

    var errorRecord = pipeline.Error
      .ReadToEnd()
      .Cast<PSObject>()
      .Select(psobject => psobject.BaseObject)
      .Cast<ErrorRecord>()
      .First();

    return errorRecord;
  }
}
