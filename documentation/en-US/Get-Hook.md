---
external help file: Mercury.PowerShell.Hooks.dll-Help.xml
Module Name: Mercury.PowerShell.Hooks
online version:
schema: 2.0.0
---

# Get-Hook

## SYNOPSIS

Gets a hook from the hook store or the entire hook store.

## SYNTAX

```
Get-Hook [-Type] <HookType> [[-Identifier] <String>] [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION

Gets a hook from the hook store or the entire hook store. If an identifier is provided, the hook with that identifier will be returned. If no identifier is provided, a view of the entire hook store will be returned.

## EXAMPLES

### Example 1

```powershell
PS C:\> Get-Hook -Type ChangeWorkingDirectory
```

This example get a view of the hook store with type `ChangeWorkingDirectory`.

### Example 2

```powershell
PS C:\> Get-Hook -Type ChangeWorkingDirectory -Identifier "MyHook"
```

This example gets the hook with the identifier `MyHook` of type `ChangeWorkingDirectory` from the hook store.

## PARAMETERS

### -Identifier

The unique identifier of the hook.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -Type

The type of hook to get.

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

## OUTPUTS

### Mercury.PowerShell.Hooks.Core.ComplexTypes.HookStore

### Mercury.PowerShell.Hooks.Core.ComplexTypes.HookStoreItem

## NOTES

## RELATED LINKS
