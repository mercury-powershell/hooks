// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation.Runspaces;

namespace Mercury.PowerShell.Hooks.Testing.Tools.Abstractions;

/// <summary>
///   Interface to configure the invokation of a cmdlet.
/// </summary>
public interface IInvokationConfigure {
  /// <summary>
  ///   Add a variable entry to the configuration.
  /// </summary>
  /// <param name="variableEntry">The variable entry to be added.</param>
  /// <returns>The current instance of the configuration.</returns>
  IInvokationConfigure WithVariableEntry(SessionStateVariableEntry variableEntry);

  /// <summary>
  ///   Add a variable entry to the configuration.
  /// </summary>
  /// <param name="name">The name of the variable.</param>
  /// <param name="value">The value of the variable.</param>
  /// <returns>The current instance of the configuration.</returns>
  IInvokationConfigure WithVariableEntry(string name, object value);

  /// <summary>
  ///   Add a parameter to the configuration.
  /// </summary>
  /// <param name="key">The key of the parameter.</param>
  /// <param name="value">The value of the parameter.</param>
  /// <returns>The current instance of the configuration.</returns>
  IInvokationConfigure WithParameter(string key, object value);
}
