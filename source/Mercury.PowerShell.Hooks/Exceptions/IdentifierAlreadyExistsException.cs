// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using Mercury.PowerShell.Hooks.Core.Enums;

namespace Mercury.PowerShell.Hooks.Exceptions;

/// <summary>
///   Exception thrown when a hook identifier already exists in the hook store.
/// </summary>
/// <param name="storeType">The type of the hook store.</param>
/// <param name="identifier">The identifier of the hook.</param>
public sealed class IdentifierAlreadyExistsException(HookType storeType, string identifier)
  : Exception($"The hook with identifier '{identifier}' already exists in the '{storeType}' hook store.") {
  private const string ERROR_ID = "IdentifierAlreadyExists";

  /// <summary>
  ///   Creates an <see cref="ErrorRecord" /> from the exception.
  /// </summary>
  /// <param name="storeType">The type of the hook store.</param>
  /// <param name="identifier">The identifier of the hook.</param>
  /// <returns>The <see cref="ErrorRecord" /> created from the exception.</returns>
  public static ErrorRecord AsRecord(HookType storeType, string identifier) {
    var exception = new IdentifierAlreadyExistsException(storeType, identifier);

    return new ErrorRecord(exception, ERROR_ID, ErrorCategory.ResourceExists, identifier);
  }
}
