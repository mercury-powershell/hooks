---
external help file: Mercury.PowerShell.Hooks.dll-Help.xml
Module Name: Mercury.PowerShell.Hooks
online version: https://go.microsoft.com/fwlink/?LinkID=2097105
schema: 2.0.0
---

# Register-Hook

## SYNOPSIS

Registers a hook for a specific event.

## SYNTAX

```
Register-Hook [-Type] <HookType> [-Identifier] <String> [-Action] <ScriptBlock> [-PassThru]
 [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION

Registers a hook for a specific event. The hook will be executed when the event is triggered.

## EXAMPLES

### Example 1

```powershell
PS C:\> Register-Hook -Type PrePrompt -Identifier "87b2eda3-eb76-42bd-a704-71ea84bf3a15" -Action { Write-Host "Hello, World!" }
```

This example registers a hook that writes "Hello, World!" to the console when the PrePrompt hook type is triggered. The identifier can be any string
that uniquely identifies the hook.

### Example 2

```powershell
PS C:\> Register-Hook -Type ChangeWorkingDirectory -Identifier "SayCurrentDirectory" -Action { Write-Host "The current directory is: $PWD" }
```

This example registers a hook that writes the current directory to the console when the ChangeWorkingDirectory hook type is triggered.

### Example 3

```powershell
PS C:\> Register-Hook -Type ChangeWorkingDirectory -Identifier "SayCurrentDirectory" -Action $Function:SomeFunctionYouCreated
```

This example registers a hook that calls a function named `SomeFunctionYouCreated` when the ChangeWorkingDirectory hook type is triggered.

## PARAMETERS

### -Action

The action to be executed when the hook is triggered.

```yaml
Type: ScriptBlock
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Identifier

The unique identifier for the hook.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -PassThru

Passes an object representing the hook to the pipeline. By default, this cmdlet does not generate any output.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -ProgressAction

The preference for how to handle progress information.

```yaml
Type: ActionPreference
Parameter Sets: (All)
Aliases: proga

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Type

The type of the hook.

```yaml
Type: HookType
Parameter Sets: (All)
Aliases:
Accepted values: ChangeWorkingDirectory, Periodic, PrePrompt

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Mercury.PowerShell.Hooks.Core.Enums.HookType

### System.String

### System.Management.Automation.ScriptBlock

### System.Management.Automation.SwitchParameter

## OUTPUTS

### Mercury.PowerShell.Hooks.Core.ComplexTypes.HookStoreItem

## NOTES

## RELATED LINKS
