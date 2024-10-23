// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation.Runspaces;

namespace Mercury.PowerShell.Hooks.Testing.Tools;

/// <summary>
///   Arguments to be passed to the invokation helper.
/// </summary>
/// <param name="VariableEntries">The variable entries to be set in the session state.</param>
/// <param name="Parameters">The parameters to be passed to the script.</param>
/// <param name="HelpFileName">The help file name to be used in the invokation helper.</param>
public sealed record InvokationArgs(
  IEnumerable<SessionStateVariableEntry> VariableEntries,
  Dictionary<string, object> Parameters,
  string HelpFileName = InvokationHelper.HELP_FILE_NAME
);
