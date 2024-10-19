@{
  GUID                 = "bec0b0f2-f120-42a1-91d1-10bc1aef6f58"

  RootModule           = "Mercury.PowerShell.Hooks.dll"
  ModuleVersion        = "0.1.0.0"

  Author               = "Bruno Sales"
  Copyright            = "(c) Bruno Sales. All rights reserved."

  PowerShellVersion    = "7.0"
  CompatiblePSEditions = @("Core")

  FunctionsToExport    = @()
  CmdletsToExport      = @()
  VariablesToExport    = @()
  AliasesToExport      = @()

  PrivateData          = @{
    PSData = @{
      Tags         = @("Mercury", "PowerShell", "Hooks")
      ProjectUri   = "https://github.com/mercury-powershell/hooks"
      LicenseUri   = "https://github.com/mercury-powershell/hooks/blob/main/LICENSE"
      ReleaseNotes = ""
    }
  }
}
