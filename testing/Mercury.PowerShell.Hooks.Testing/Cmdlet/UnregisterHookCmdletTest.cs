// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.Cmdlets;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Exceptions;

namespace Mercury.PowerShell.Hooks.Testing.Cmdlet;

public sealed class UnregisterHookCmdletTest {
  [Test]
  public void OutputType_When_IdentifierIsUnknown_ShouldBe_ErrorRecord() {
    // Act
    var identifier = Guid.NewGuid().ToString();
    var errorRecord = PSCmdletHelper.TryInvoke<UnregisterHookCmdlet>(prepare => prepare
      .WithParameter("Type", HookType.PrePrompt)
      .WithParameter("Identifier", identifier)
      .WithParameter("PassThru", true));

    // Assert
    Assert.That(errorRecord, Is.Not.Null);
    Assert.Multiple(() => {
      Assert.That(errorRecord.Exception, Is.InstanceOf<IdentifierNotFoundException>());
      Assert.That(errorRecord.Exception.Message,
        Is.EqualTo($"The hook with identifier '{identifier}' was not found in the 'PrePrompt' hook store."));
    });
  }

  [Test]
  public void OutputType_When_IdentifierIsKnown_ShouldBe_HookStoreItem() {
    // Arrange
    var identifier = Guid.NewGuid().ToString();
    _ = PSCmdletHelper.Invoke<RegisterHookCmdlet>(prepare => prepare
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", identifier)
      .WithParameter("Action", ScriptBlock.Create("""{ Write-Host "Current Directory: $PWD" }""")));

    // Act
    var output = PSCmdletHelper.Invoke<UnregisterHookCmdlet>(prepare => prepare
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", identifier)
      .WithParameter("PassThru", true));

    // Assert
    Assert.That(output, Is.Not.Null);
    Assert.That(output.BaseObject, Is.InstanceOf<HookStoreItem>());
  }

  [Test]
  public void OutputType_When_PassThruIsNotPresent_ShouldBe_Null() {
    // Arrange
    var identifier = Guid.NewGuid().ToString();
    _ = PSCmdletHelper.Invoke<RegisterHookCmdlet>(prepare => prepare
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", identifier)
      .WithParameter("Action", ScriptBlock.Create("""{ Write-Host "Current Directory: $PWD" }""")));

    // Act
    var output = PSCmdletHelper.Invoke<UnregisterHookCmdlet>(prepare => prepare
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", identifier));

    // Assert
    Assert.That(output, Is.Null);
  }
}
