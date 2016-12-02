# Get-ForensicUsnJrnl

## SYNOPSIS
Gets the UsnJrnl entries from the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicUsnJrnl [[-VolumeName] <String>] [-Usn <Int64>]
```

### ByPath
```
Get-ForensicUsnJrnl -Path <String> [-Usn <Int64>]
```

## DESCRIPTION
The Get-ForensicUsnJrnl cmdlet parses the $UsnJrnl file&apos;s $J data stream to return UsnJrnl entries. If you do not specify a Usn (Update Sequence Number), it returns all entries in the $UsnJrnl.

The $UsnJrnl file maintains a record of all file system operations that have occurred. Because the file is circular, entries are overwritten.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> $usn = Get-ForensicUsnJrnl
```

This command gets the file system operations on the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> $r = Get-ForensicFileRecord C:\temp\helloworld.txt
[ADMIN]: PS C:\> $r.Attribute[0].UpdateSequenceNumber
713538320

[ADMIN]:PS C:\> Get-ForensicUsnJrnl -Usn $r.Attribute[0].UpdateSequenceNumber

VolumePath               : \\.\C:
Version                  : 2.0
RecordNumber             : 132245
FileSequenceNumber       : 52
ParentFileRecordNumber   : 191621
ParentFileSequenceNumber : 59
Usn                      : 713538320
TimeStamp                : 11/17/2015 10:02:56 PM
Reason                   : DATA_EXTEND, FILE_CREATE, CLOSE
SourceInfo               : 0
SecurityId               : 0
FileAttributes           : ARCHIVE
FileName                 : helloworld.txt
```

This example uses Get-ForensicFileRecord and Get-ForensicUsnJrnl to get the UsnJrnl entries in the helloworld.txt file.

The first command gets the file record entries in the helloworld.txt files. The second command gets the USN of the first attribute in the Ntfs.FileRecord object that Get-ForensicFileRecord returns.
The third command uses Get-ForensicUsnJrnl to get the UsnJrnl record for the USN.

A file's most recent entry number can be found in its MFT FileRecord's $STANDARD_INFORMATION attribute.

### Example 3
```
[ADMIN]: PS C:\> $usn = Get-ForensicUsnJrnl -Path C:\evidence\UsnJrnl
```

This command get the UsnJrnl record of an exported UsnJrnl file.

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

### -Usn
The Update Sequence Number of the record to return.

```yaml
Type: Int64
Parameter Sets: (All)
Aliases: 

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

### PowerForensics.Ntfs.UsnJrnl

## NOTES

## RELATED LINKS

