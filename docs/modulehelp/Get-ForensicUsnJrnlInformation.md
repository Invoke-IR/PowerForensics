# Get-ForensicUsnJrnlInformation

## SYNOPSIS
Gets metadata about the specified volume's $UsnJrnl.

## SYNTAX

### ByVolume
```
Get-ForensicUsnJrnlInformation [[-VolumeName] <String>] [-AsBytes]
```

### ByPath
```
Get-ForensicUsnJrnlInformation -Path <String> [-AsBytes]
```

## DESCRIPTION
The Get-ForensicUsnJrnlInformation cmdlet parses the $UsnJrnl file&apos;s $MAX data stream and returns metadata about the UsnJrnl configuration.

By default, this cmdlet parses the $UsnJrnl file on the C:\ drive. To specify a drive, use the
VolumeName parameter. To specify an exported $UsnJrnl file, use the Path parameter.

You can also use the AsBytes parameter to get the metadata in byte format.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicUsnJrnlInformation

   MaxSize    AllocationDelta                 UsnId
   -------    ---------------                 -----
  33554432            8388608    130547872109887937
```

This command gets metadata about the $UsnJrnl on the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicUsnJrnlInformation -Path C:\evidence\UsnJrnl

   MaxSize    AllocationDelta                 UsnId
   -------    ---------------                 -----
  33554432            8388608    130547872109887937
```

This command gets metadata about the $UsnJrnl on an exported UsnJrnl file.

### Example 3
```
[ADMIN]: PS C:\> Get-UsnJrnlInformation -AsBytes | Format-ForensicHex

Offset     _00_01_02_03_04_05_06_07_08_09_0A_0B_0C_0D_0E_0F  Ascii
------     ------------------------------------------------  -----
0x00000000  00 00 00 02 00 00 00 00 00 00 80 00 00 00 00 00  ................
0x00000010  C1 01 4B 17 99 CC CF 01 00 00 00 00 00 00 00 00  ..K.............
```

This command gets the gets metadata about the $Max data stream as a byte array.

## PARAMETERS

### -AsBytes
Returns the $UsnJrnl $Max data stream as byte array instead of as a UsnJrnlDetail object.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: 

Required: False
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

### PowerForensics.Ntfs.UsnJrnlDetail

## NOTES

## RELATED LINKS

