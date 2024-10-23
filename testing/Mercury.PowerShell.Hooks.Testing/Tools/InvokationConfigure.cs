// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation.Runspaces;
using Mercury.PowerShell.Hooks.Testing.Tools.Abstractions;

namespace Mercury.PowerShell.Hooks.Testing.Tools;

/// <summary>
///   Class to configure the invokation of a cmdlet.
/// </summary>
internal sealed class InvokationConfigure : IInvokationConfigure {
  private readonly Dictionary<string, object> _parameters = new();
  private readonly List<SessionStateVariableEntry> _variableEntries = new();

  /// <inheritdoc />
  public IInvokationConfigure WithVariableEntry(SessionStateVariableEntry variableEntry) {
    _variableEntries.Add(variableEntry);

    return this;
  }

  /// <inheritdoc />
  public IInvokationConfigure WithVariableEntry(string name, object value) {
    _variableEntries.Add(new SessionStateVariableEntry(name, value, string.Empty));

    return this;
  }

  /// <inheritdoc />
  public IInvokationConfigure WithParameter(string key, object value) {
    _parameters.Add(key, value);

    return this;
  }

  private InvokationArgs Build()
    => new(_variableEntries, _parameters);

  /// <summary>
  ///   Configures the invokation of a cmdlet using the provided configuration.
  /// </summary>
  /// <param name="configure">The configuration to be used.</param>
  /// <returns>The <see cref="InvokationArgs" /> configured instance.</returns>
  public static InvokationArgs ToInvoke(Action<IInvokationConfigure> configure) {
    var invokationConfigure = new InvokationConfigure();
    configure(invokationConfigure);

    return invokationConfigure.Build();
  }
}
