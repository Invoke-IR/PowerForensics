# Get-ForensicUnallocatedSpace

## SYNOPSIS
Gets the unallocated space on the specified partition/volume.

## SYNTAX

```
Get-ForensicUnallocatedSpace [[-VolumeName] <String>] [-Path <UInt64>]
```

## DESCRIPTION
The Get-ForensicUnallocatedSpace cmdlet parses the $Bitmap file to find clusters that are marked as unallocated (not in use by the file system). Then, the cmdlet returns the unallocated clusters as a byte array.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicUnallocatedSpace -VolumeName \\.\Z:
```

This command gets a byte array of unallocated clusters in the \\.\Z: volume.

## PARAMETERS

### -Path
Path to $Bitmap file.

```yaml
Type: UInt64
Parameter Sets: (All)
Aliases: FullName

Required: False
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
Parameter Sets: (All)
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

### System.Byte[]

## NOTES

## RELATED LINKS

