---
external help file: Mercury.PowerShell.Hooks.dll-Help.xml
Module Name: Mercury.PowerShell.Hooks
online version:
schema: 2.0.0
---

# Set-HookOption

## SYNOPSIS

Set the module current options.

## SYNTAX

```
Set-HookOption [[-MaxDegreeOfParallelism] <Int32>] [[-PeriodicTimeSpan] <TimeSpan>]
 [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION

Set the module current options. The options are used to control the behavior of the module.

## EXAMPLES

### Example 1

```powershell
PS C:\> Set-HookOption -MaxDegreeOfParallelism 4 -PeriodicTimeSpan 00:00:30
```

This example sets the module options to use a maximum degree of parallelism of 4 and a periodic time span of 30 seconds.

## PARAMETERS

### -MaxDegreeOfParallelism

The maximum degree of parallelism to use for the module.

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -PeriodicTimeSpan

The time span to use for periodic actions.

```yaml
Type: TimeSpan
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -ProgressAction

The action preference to use for the cmdlet.

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

### System.Nullable`1[[System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]

### System.Nullable`1[[System.TimeSpan, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]

## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS
