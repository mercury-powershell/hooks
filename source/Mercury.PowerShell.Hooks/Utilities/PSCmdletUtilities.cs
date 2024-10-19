// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Reflection;

namespace Mercury.PowerShell.Hooks.Utilities;

/// <summary>
///   Utility methods for <see cref="PSCmdlet" />.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
[ExcludeFromCodeCoverage]
internal static class PSCmdletUtilities {
  /// <summary>
  ///   Get the parameters of a cmdlet.
  /// </summary>
  /// <param name="cmdlet">The cmdlet.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet.</typeparam>
  /// <returns>A <see cref="PSObject" /> with the parameters of the cmdlet.</returns>
  public static PSObject GetParameters<TCmdlet>(TCmdlet cmdlet) where TCmdlet : PSCmdlet {
    var psobject = new PSObject();
    var properties = typeof(TCmdlet)
      .GetProperties()
      .Where(property => property.GetCustomAttribute<ParameterAttribute>() is not null)
      .Select(property => new PSNoteProperty(property.Name, property.GetValue(cmdlet)))
      .ToArray();

    foreach (var property in properties) {
      psobject.Properties.Add(property);
    }

    return psobject;
  }

  /// <summary>
  ///   Get a parameter from a cmdlet.
  /// </summary>
  /// <param name="cmdlet">The cmdlet.</param>
  /// <param name="exportFunc">The export function.</param>
  /// <typeparam name="TCmdlet">The type of the cmdlet.</typeparam>
  /// <typeparam name="TOut">The type of the parameter.</typeparam>
  /// <returns>The parameter.</returns>
  public static TOut GetParameter<TCmdlet, TOut>(TCmdlet cmdlet, Func<TCmdlet, TOut> exportFunc) where TCmdlet : PSCmdlet
    => exportFunc(cmdlet);
}
