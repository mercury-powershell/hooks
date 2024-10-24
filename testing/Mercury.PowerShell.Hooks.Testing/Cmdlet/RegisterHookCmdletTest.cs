// Copyright (c) Bruno Sales <me@baliestri.dev>. Licensed under the MIT License.
// See the LICENSE file in the repository root for full license text.

using System.Management.Automation;
using Mercury.PowerShell.Hooks.Cmdlets;
using Mercury.PowerShell.Hooks.Core.ComplexTypes;
using Mercury.PowerShell.Hooks.Core.Enums;
using Mercury.PowerShell.Hooks.Exceptions;

namespace Mercury.PowerShell.Hooks.Testing.Cmdlet;

public sealed class RegisterHookCmdletTest {
  [Test]
  public void OutputType_When_PassThruIsNotPresent_ShouldBe_Null() {
    // Act
    var output = InvokationHelper.Invoke<RegisterHookCmdlet>(configure => configure
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", Guid.NewGuid().ToString())
      .WithParameter("Action", ScriptBlock.Create("""{ Write-Host "Current Directory: $PWD" }""")));

    // Assert
    Assert.That(output, Is.Null);
  }

  [Test]
  public void OutputType_When_PassThruIsPresent_ShouldBe_HookStoreItem() {
    // Act
    var output = InvokationHelper.Invoke<RegisterHookCmdlet>(configure => configure
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", Guid.NewGuid().ToString())
      .WithParameter("Action", ScriptBlock.Create("""{ Write-Host "Current Directory: $PWD" }"""))
      .WithParameter("PassThru", true));

    // Assert
    Assert.That(output, Is.Not.Null);
    Assert.That(output.BaseObject, Is.InstanceOf<HookStoreItem>());
  }

  [Test]
  public void OutputType_When_IdentifierAlreadyExists_ShouldBe_ErrorRecord() {
    // Arrange
    var identifier = Guid.NewGuid().ToString();
    _ = InvokationHelper.Invoke<RegisterHookCmdlet>(configure => configure
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", identifier)
      .WithParameter("Action", ScriptBlock.Create("""{ Write-Host "Current Directory: $PWD" }""")));

    // Act
    var errorRecord = InvokationHelper.ErrorRecord<RegisterHookCmdlet>(configure => configure
      .WithParameter("Type", HookType.ChangeWorkingDirectory)
      .WithParameter("Identifier", identifier)
      .WithParameter("Action", ScriptBlock.Create("""{ Write-Host "Current Directory: $PWD" }""")));

    // Assert
    Assert.That(errorRecord, Is.Not.Null);
    Assert.Multiple(() => {
      Assert.That(errorRecord.Exception, Is.InstanceOf<IdentifierAlreadyExistsException>());
      Assert.That(errorRecord.Exception.Message,
        Is.EqualTo($"The hook with identifier '{identifier}' already exists in the 'ChangeWorkingDirectory' hook store."));
    });
  }
}
