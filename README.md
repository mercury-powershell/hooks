# Mercury.PowerShell.Hooks

This module is inspired by the [z-shell][1] hooks, trying to bring the same functionality to PowerShell.

## About

This module provides a way to define hooks in PowerShell. The hook is a script block or a function that is executed when a specific event occurs. The module provides a way to define hooks and to trigger them.

Currently, the module supports the following hooks:

- [x] PrePrompt
  - This hook is executed before the prompt is displayed. Made using a proxy cmdlet for `Out-Default` 'cause the `prompt` function is triggered when using `tab-completion`.
- [x] ChangeWorkingDirectory
  - This hook is executed when the working directory is changed. Made using a proxy cmdlet for `Set-Location`, `Push-Location`, and `Pop-Location`.
- [ ] AddToHistory
  - Still not implemented. Looking for a way to hook into the history commands.
- [x] Periodic
  - This hook is executed periodically. All instances of `PowerShell` that uses the module share the same timer and only one of them will trigger the hook.
  - Predefined interval: `10s`.
- [ ] ~~PreExecution~~
  - For now this one seems impossible to implement. Maybe if I implement a custom [PSHost][2], but there is no guarantee that it will work.

**PS**: This module is still in development, so some features may not work as expected. If you find any issues, please report them.

**PS²**: The `StateManager` is not shared between instances of `PowerShell`, so if a value is set in one instance, it will not be available in another instance. I'm planning to share a SQLite database between instances to store the state.

## Installation

To install the module, you can use the following command:

```powershell
Install-Module -Name Mercury.PowerShell.Hooks -AllowClobber
```

Then, you can import the module using the following command:

```powershell
Import-Module -Name Mercury.PowerShell.Hooks
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
