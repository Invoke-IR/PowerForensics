# Get-ForensicRegistryKey

## SYNOPSIS
Gets the keys of the specified registry hive.

## SYNTAX

### ByKey
```
Get-ForensicRegistryKey -HivePath <String> [-Key <String>]
```

### Recursive
```
Get-ForensicRegistryKey -HivePath <String> [-Recurse]
```

## DESCRIPTION
The Get-ForensicRegistryKey cmdlet parses a registry hive and returns the subkeys of the specified key.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicRegistryKey -HivePath C:\Windows\system32\config\SOFTWARE -Key Tenable


HivePath                : C:\Windows\system32\config\SOFTWARE
WriteTime               : 8/14/2015 4:18:52 PM
NumberOfSubKeys         : 0
NumberOfVolatileSubKeys : 0
NumberOfValues          : 1
FullName                : Tenable\Nessus
Name                    : Nessus
Allocated               : True
```

This command gets the subkeys of the HKLM:\SOFTWARE\Tenable key.

### Example 1
```
[ADMIN]: PS C:\> $nk = Get-RegistryKey -HivePath C:\Windows\system32\config\SAM -Recurse
```

This gets all keys in the SAM hive.

## PARAMETERS

### -HivePath
The registry hive to parse.

```yaml
Type: String
Parameter Sets: (All)
Aliases: Path

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Key
The key to begin listing subkeys from.

```yaml
Type: String
Parameter Sets: ByKey
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Recurse
Recursively list all keys in the specified hive.

```yaml
Type: SwitchParameter
Parameter Sets: Recursive
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### PowerForensics.Registry.NamedKey

## NOTES

## RELATED LINKS

