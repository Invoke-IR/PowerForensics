# Get-ForensicBitmap

## SYNOPSIS
Determines whether the specified cluster is allocated.

## SYNTAX

### ByVolume
```
Get-ForensicBitmap [[-VolumeName] <String>] -Cluster <Int64>
```

### ByPath
```
Get-ForensicBitmap -Path <String> -Cluster <Int64>
```

## DESCRIPTION
The Get-Bitmap cmdlet parses the $Bitmap file to determine whether or not the specified cluster is allocated.

By default, the cmdlet parses the $Bitmap file on the C:\ drive. To change the target drive, use the
VolumeName parameter or use the Path parameter to specify an exported $Bitmap file.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-Bitmap -Cluster 1000

Cluster InUse
------- -----
   1000  True
```

This example shows Get-Bitmap being used to check Cluster 1000's allocation status.

### Example 2
```
[ADMIN]: PS C:\> Get-Bitmap -Cluster 1000 -Path 'C:\$Bitmap'

Cluster InUse
------- -----
   1000  True
```

This example shows Get-Bitmap checking cluster 1000 of the exported C:\$Bitmap file.

## PARAMETERS

### -Cluster
The cluster number to check for allocation.

```yaml
Type: Int64
Parameter Sets: (All)
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

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

### PowerForensics.Ntfs.Bitmap

## NOTES

## RELATED LINKS

