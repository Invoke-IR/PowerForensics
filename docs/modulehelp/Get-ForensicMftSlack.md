# Get-ForensicMftSlack

## SYNOPSIS
Gets the Master File Table (MFT) slack space for the specified volume.

## SYNTAX

### ByIndex
```
Get-ForensicMftSlack [-VolumeName <String>] [[-Index] <Int32>]
```

### ByPath
```
Get-ForensicMftSlack [-Path] <String>
```

### ByMftPath
```
Get-ForensicMftSlack -MftPath <String>
```

## DESCRIPTION
The Get-ForensicMftSlack cmdlet returns a byte array representing the slack space found in Master File Table (MFT) records.

Each MFT File Record is 1024 bytes long. When a file record does not allocate all 1024 bytes, the remaining bytes are considered "slack". To compute slack space, compare the AllocatedSize and RealSize properties of a FileRecord object.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicMftSlack -VolumeName C:
```

This command uses Get-ForensicMftSlack to get slack space from the $MFT file on the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicMftSlack  -VolumeName C: -Index 24212
```

This command uses Get-ForensicMftSlack to get the slack space from the MFT record at index 24212 on the C:\ logical volume.

### Example 3
```
[ADMIN]: PS C:\> Get-ForensicMftSlack -Path C:\Windows\system32\cmd.exe
```

This command uses Get-ForensicMftSlack to get the slack space on the Cmd.exe MFT record.

### Example 4
```
[ADMIN]: PS C:\> Get-ForensicMftSlack -MftPath C:\evidence\MFT
```

This command uses Get-ForensicMftSlack to get the MFT slack space from an exported Master File Table.

## PARAMETERS

### -Index
The index of the MFT entry to return MFT slack space for.

```yaml
Type: Int32
Parameter Sets: ByIndex
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MftPath
Path to an exported Master File Table.

```yaml
Type: String
Parameter Sets: ByMftPath
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
The path to the file to return MFT slack space for.

```yaml
Type: String
Parameter Sets: ByPath
Aliases: FullName

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -VolumeName
Specifies the name of the volume or logical partition.

Enter the volume name in one of the following formats: \\.\C:, C:, or C.

```yaml
Type: String
Parameter Sets: ByIndex
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### System.String


## OUTPUTS

### System.Byte[]

## NOTES

## RELATED LINKS

