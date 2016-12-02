# Get-ForensicUserAssist

## SYNOPSIS
Gets the UserAssist entries from the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicUserAssist [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicUserAssist -HivePath <String>
```

## DESCRIPTION
The Get-ForensicUserAssist cmdlet parses the NTUSER.DAT registry hive to derive applications that were recently used by a particular user.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicUserAssist -VolumeName \\.\C:
```

This command gets applications that the Public user used from all user's NTUSER.DAT hives on the C: logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicUserAssist -HivePath C:\Users\Public\NTUSER.DAT
```

This command gets applications that the Public user used from the C:\Users\Public\NTUSER.DAT hive.

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

### PowerForensics.Artifacts.UserAssist

## NOTES

## RELATED LINKS

