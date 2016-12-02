# Get-ForensicRunMru

## SYNOPSIS
Gets the commands that were issued by the user to the run dialog.

## SYNTAX

### ByVolume
```
Get-ForensicRunMru [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicRunMru -HivePath <String>
```

## DESCRIPTION
The Get-ForensicRunMru cmdlet parses a user&apos;s NTUSER.DAT file to derive the commands that have been issued to the run dialog.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicRunMru -VolumeName \\.\C:
```

This command gets the URLs typed into Internet Explorer from all user's NTUSER.DAT hives on the C: logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicRunMru -HivePath C:\Users\Public\NTUSER.DAT
```

This command gets the URLs typed into Internet Explorer from the C:\Users\Public\NTUSER.DAT hive.

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

### System.String

## NOTES

## RELATED LINKS

