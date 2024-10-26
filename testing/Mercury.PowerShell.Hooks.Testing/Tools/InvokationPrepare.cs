// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation.Runspaces;
using Mercury.PowerShell.Hooks.Testing.Tools.Abstractions;

namespace Mercury.PowerShell.Hooks.Testing.Tools;

/// <summary>
///   Class to prepare the invokation of a cmdlet.
/// </summary>
internal sealed class InvokationPrepare : IInvokationPrepare {
  private readonly List<CommandParameter> _parameters = [];
  private readonly List<SessionStateVariableEntry> _variableEntries = [];

  /// <inheritdoc />
  public IInvokationPrepare WithVariableEntry(SessionStateVariableEntry variableEntry) {
    _variableEntries.Add(variableEntry);

    return this;
  }

  /// <inheritdoc />
  public IInvokationPrepare WithVariableEntry(string name, object value)
    => WithVariableEntry(new SessionStateVariableEntry(name, value, string.Empty));

  /// <inheritdoc />
  public IInvokationPrepare WithParameter(CommandParameter commandParameter) {
    _parameters.Add(commandParameter);

    return this;
  }

  /// <inheritdoc />
  public IInvokationPrepare WithParameter(string key, object value)
    => WithParameter(new CommandParameter(key, value));

  private void Build(out IEnumerable<SessionStateVariableEntry> variableEntries, out IEnumerable<CommandParameter> parameters)
    => (variableEntries, parameters) = (_variableEntries, _parameters);

  /// <summary>
  ///   Configures the invokation of a cmdlet using the provided configuration.
  /// </summary>
  /// <param name="configure">The action to prepare the invokation.</param>
  /// <param name="variableEntries">The variable entries to be used.</param>
  /// <param name="parameters">The parameters to be used.</param>
  public static void Configure(Action<IInvokationPrepare> configure, out IEnumerable<SessionStateVariableEntry> variableEntries,
  out IEnumerable<CommandParameter> parameters) {
    var invokationConfigure = new InvokationPrepare();
    configure(invokationConfigure);

    invokationConfigure.Build(out variableEntries, out parameters);
  }
}
