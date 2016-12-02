# Get-ForensicRegistryValue

## SYNOPSIS
Gets the values of the specified registry key.

## SYNTAX

```
Get-ForensicRegistryValue [-HivePath] <String> [[-Key] <String>] [[-Value] <String>]
```

## DESCRIPTION
The Get-ForensicRegistryValue cmdlet parses a registry hive and returns the values of a specified key.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicRegistryValue -HivePath C:\Windows\system32\config\SOFTWARE -Key Microsoft\Windows\CurrentVersion\Run
```

This command gets the values of the Run key.

### Example 2
```
[ADMIN]: PS C:\> Get-RegistryValue -HivePath C:\Windows\system32\config\SYSTEM -Key ControlSet001\Serivces\Enum -Value NextParentID.72bb93.8

HivePath   : C:\Windows\system32\config\SYSTEM
Key        : Enum
DataLength : 4
DataType   : REG_DWORD
Name       : NextParentID.72bb93.8
Allocated  : True
```

This command gets the NextParentID.72bb93.8 value of the HKLM:\SYSTEM\ControlSet001\Services\Enum key.

## PARAMETERS

### -HivePath
The registry hive to parse.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Path

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Key
The key to list values from.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Value
The specific value to return.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### PowerForensics.Registry.ValueKey

## NOTES

## RELATED LINKS

