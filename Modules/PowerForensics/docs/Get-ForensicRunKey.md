# Get-ForensicRunKey

## SYNOPSIS
Gets applications that will autostart due to their inclusion in a "Run" key.

## SYNTAX

### ByVolume
```
Get-ForensicRunKey [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicRunKey -HivePath <String>
```

## DESCRIPTION
The Get-ForensicRunKey cmdlet parses the SOFTWARE and NTUSER.DAT hives to produce a list of applications that have been added to a "Run" key.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicRunKey
```

{{ Add example description here }}

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicRunKey -HivePath C:\Windows\System32\config\SOFTWARE
```

{{ Add example description here }}

## PARAMETERS

### -HivePath
Registry hive to parse.

```yaml
Type: String
Parameter Sets: ByPath
Aliases: Path

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -VolumeName
Specifies the name of the volume or logical partition.

Enter the volume name in one of the following formats: \\.\C:, C:, or C.

```yaml
Type: String
Parameter Sets: ByVolume
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### PowerForensics.Artifacts.Persistence.RunKey

## NOTES

## RELATED LINKS

