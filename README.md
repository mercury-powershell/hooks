# Mercury.PowerShell.Hooks

This module is inspired by the [z-shell][1] hooks, trying to bring the same functionality to PowerShell.

## About

This module provides a way to define hooks in PowerShell. The hook is a script block or a function that is executed when a specific event occurs. The module provides a way to define hooks and to trigger them.

Currently, the module supports the following hooks:

- [ ] PrePrompt
- [ ] ChangeWorkingDirectory
- [ ] AddToHistory
- [ ] Periodic
- [ ] PreExecution

## Installation

To install the module, you can use the following command:

```powershell
Install-Module -Name Mercury.PowerShell.Hooks
```

## Usage

See the documentation for more information:

- [English][3]

## License

This project is licensed under the MIT License - see the [LICENSE][4] file for details.

[1]: https://www.zsh.org/
[2]: https://learn.microsoft.com/en-us/dotnet/api/system.management.automation.host.pshost?view=powershellsdk-7.4.0
[3]: documentation/en-US/Mercury.PowerShell.Hooks.md
[4]: LICENSE.md
