# Get-ForensicVolumeInformation

## SYNOPSIS
Gets information about the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicVolumeInformation [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicVolumeInformation -Path <String>
```

## DESCRIPTION
The Get-ForensicVolumeInformation cmdlet parses the $Volume file&apos;s $VOLUME_INFORMATION attribute to return the metadata about the specified volume.

By default, the cmdlet parses the $Volume file on the C:\ drive. To specify an alternate target drive, use the -VolumeName parameter. To specify an exported $Volume file, use the -Path parameter.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicVolumeInformation

Name    : VOLUME_INFORMATION
Version : 3.1
Flags   : 0
```

This command gets the metadata about the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicVolumeInformation -Path 'C:\evidence\$Volume'
```

This command gets metadata about an exported volume file, C:\evidence\$Volume.

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

### PowerForensics.Ntfs.VolumeInformation

## NOTES

## RELATED LINKS

