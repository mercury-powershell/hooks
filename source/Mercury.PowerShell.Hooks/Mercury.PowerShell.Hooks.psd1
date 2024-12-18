@{
  GUID                 = "bec0b0f2-f120-42a1-91d1-10bc1aef6f58"

  RootModule           = "Mercury.PowerShell.Hooks.dll"
  ModuleVersion        = "0.2.0.1"

  Author               = "Bruno Sales"
  Description          = "This module is inspired by the z-shell hooks, trying to bring the same functionality of hooks to PowerShell 7+."
  Copyright            = "(c) Bruno Sales. All rights reserved."

  PowerShellVersion    = "7.0"
  CompatiblePSEditions = @("Core")

  FunctionsToExport    = @()
  CmdletsToExport      = @(
    "Get-Hook",
    "Register-Hook",
    "Unregister-Hook",
    "Get-HookOption",
    "Set-HookOption",
    "Out-Default",
    "Pop-Location",
    "Push-Location",
    "Set-Location"
  )
  VariablesToExport    = @()
  AliasesToExport      = @()

  FormatsToProcess     = @(
    "Mercury.PowerShell.Hooks.Format.ps1xml"
  )

  PrivateData          = @{
    PSData = @{
      Tags         = @("Mercury", "PowerShell", "Hooks")
      ProjectUri   = "https://github.com/mercury-sh/hooks"
      LicenseUri   = "https://github.com/mercury-sh/hooks/blob/main/LICENSE"
      ReleaseNotes = ""
    }
  }
}
