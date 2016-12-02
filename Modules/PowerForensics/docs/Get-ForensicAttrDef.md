# Get-ForensicAttrDef

## SYNOPSIS
Gets information about all the Master File Table (MFT) file attributes usable in a volume.

## SYNTAX

### ByVolume
```
Get-ForensicAttrDef [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicAttrDef -Path <String>
```

## DESCRIPTION
The Get-AttrDef cmdlet parses the $AttrDef file on the specified volume and returns information about all MFT file
attributes usable in the volume.

By default, the cmdlet parses the $AttrDef file on the C:\ drive. To change the target drive, use the VolumeName parameter or use the Path parameter to specify an exported $AttrDef file.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-AttrDef -VolumeName \\.\C:

Name                      Type   MinSize MaxSize
----                      ----   ------- -------
$STANDARD_INFORMATION     16     48      72
$ATTRIBUTE_LIST           32     0       18446744073709551615
$FILE_NAME                48     68      578
$OBJECT_ID                64     0       256
$SECURITY_DESCRIPTOR      80     0       18446744073709551615
$VOLUME_NAME              96     2       256
$VOLUME_INFORMATION       112    12      12
$DATA                     128    0       18446744073709551615
$INDEX_ROOT               144    0       18446744073709551615
$INDEX_ALLOCATION         160    0       18446744073709551615
$BITMAP                   176    0       18446744073709551615
$REPARSE_POINT            192    0       16384
$EA_INFORMATION           208    8       8
$EA                       224    0       65536
$LOGGED_UTILITY_STREAM    256    0       65536
```

This example shows returning the MFT Attribute definitions for the C Volume.

### Example 2
```
[ADMIN]: PS C:\> Get-AttrDef -Path 'C:\$AttrDef'

Name                      Type   MinSize MaxSize
----                      ----   ------- -------
$STANDARD_INFORMATION     16     48      72
$ATTRIBUTE_LIST           32     0       18446744073709551615
$FILE_NAME                48     68      578
$OBJECT_ID                64     0       256
$SECURITY_DESCRIPTOR      80     0       18446744073709551615
$VOLUME_NAME              96     2       256
$VOLUME_INFORMATION       112    12      12
$DATA                     128    0       18446744073709551615
$INDEX_ROOT               144    0       18446744073709551615
$INDEX_ALLOCATION         160    0       18446744073709551615
$BITMAP                   176    0       18446744073709551615
$REPARSE_POINT            192    0       16384
$EA_INFORMATION           208    8       8
$EA                       224    0       65536
$LOGGED_UTILITY_STREAM    256    0       65536
```

This example shows Get-AttrDef being run against an exported file.

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

### PowerForensics.Ntfs.AttrDef

## NOTES

## RELATED LINKS

