@{
  GUID                 = "bec0b0f2-f120-42a1-91d1-10bc1aef6f58"

  RootModule           = "Mercury.PowerShell.Hooks.dll"
  ModuleVersion        = "0.1.0.0"

  Author               = "Bruno Sales"
  Copyright            = "(c) Bruno Sales. All rights reserved."

  PowerShellVersion    = "7.0"
  CompatiblePSEditions = @("Core")

  FunctionsToExport    = @()
  CmdletsToExport      = @(
    "Get-Hook",
    "Register-Hook",
    "Unregister-Hook",
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
      ProjectUri   = "https://github.com/mercury-powershell/hooks"
      LicenseUri   = "https://github.com/mercury-powershell/hooks/blob/main/LICENSE"
      ReleaseNotes = ""
    }
  }
}
