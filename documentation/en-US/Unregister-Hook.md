---
external help file: Mercury.PowerShell.Hooks.dll-Help.xml
Module Name: Mercury.PowerShell.Hooks
online version: https://go.microsoft.com/fwlink/?LinkID=2097049
schema: 2.0.0
---

# Unregister-Hook

## SYNOPSIS

Unregisters a hook for a specific event.

## SYNTAX

```
Unregister-Hook [-Type] <HookType> [-Identifier] <String> [-PassThru] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION

Unregisters a hook for a specific event. The hook will no longer be executed when the event is triggered.

## EXAMPLES

### Example 1

```powershell
PS C:\> Unregister-Hook -Type PrePrompt -Identifier "87b2eda3-eb76-42bd-a704-71ea84bf3a15"
```

This example unregisters the hook with the identifier `87b2eda3-eb76-42bd-a704-71ea84bf3a15` from the hook store.

### Example 2

```powershell
PS C:\> Unregister-Hook -Type ChangeWorkingDirectory -Identifier "SayCurrentDirectory"
```

This example unregisters the hook with the identifier `SayCurrentDirectory` from the hook store.

## PARAMETERS

### -Identifier

The unique identifier of the hook.

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
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Type

The type of the hook to be unregistered.

```yaml
Type: HookType
Parameter Sets: (All)
Aliases:
Accepted values: ChangeWorkingDirectory, PrePrompt

Required: True
Position: 0
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

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Mercury.PowerShell.Hooks.Core.Enums.HookType

### System.String

### System.Management.Automation.SwitchParameter

## OUTPUTS

### Mercury.PowerShell.Hooks.Core.ComplexTypes.HookStoreItem

## NOTES

## RELATED LINKS
