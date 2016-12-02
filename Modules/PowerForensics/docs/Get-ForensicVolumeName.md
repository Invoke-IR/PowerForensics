# Get-ForensicVolumeName

## SYNOPSIS
Gets the name of the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicVolumeName [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicVolumeName -Path <String>
```

## DESCRIPTION
The Get-ForensicVolumeName cmdlet parses the $Volume file's $VOLUME_NAME attribute to return the name of the specified volume.

By default, the cmdlet parses the $Volume file on the C:\ drive. To specify an alternate target drive, use the VolumeName parameter. To specify an exported $Volume file, use the Path parameter.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-VolumeName -VolumeName \\.\C:

VolumeNameString
----------------
testdrive
```

This command gets the name of the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-VolumeName -Path 'C:\evidence\$Volume'

VolumeNameString
----------------
testdrive
```

This command gets the name of the volume in C:\evidence\$Volume file, and exported $Volume file.

## PARAMETERS

### -Path
Path to file to be parsed.

```yaml
Type: String
Parameter Sets: ByPath
Aliases: FullName

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByPropertyName)
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

### System.String


## OUTPUTS

### PowerForensics.Ntfs.VolumeName

## NOTES

## RELATED LINKS

