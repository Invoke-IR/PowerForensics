# Get-ForensicAlternateDataStream

## SYNOPSIS
Gets the NTFS Alternate Data Streams on the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicAlternateDataStream [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicAlternateDataStream -Path <String>
```

## DESCRIPTION
The Get-ForensicAlternateDataStream cmdlet parses the Master File Table and returns AlternateDataStream objects for files that contain more than one $DATA attribute.

NTFS stores file contents in $DATA attributes. The file system allows a single file to maintain multiple $DATA
attributes. When a file has more than one $DATA attribute the additional attributes are referred to as "Alternate Data
Streams".

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicAlternateDataStream
```

This example shows Get-ForensicAlternateDataStream getting all ADS on the C:\ logical volume.

## PARAMETERS

### -Path
The path of a file that should be checked for alternate data streams.

```yaml
Type: String
Parameter Sets: ByPath
Aliases: FullName

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

### PowerForensics.Artifacts.AlternateDataStream

## NOTES

## RELATED LINKS

