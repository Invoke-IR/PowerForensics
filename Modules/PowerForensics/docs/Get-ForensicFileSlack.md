# Get-ForensicFileSlack

## SYNOPSIS
Gets the specified volume's slack space.

## SYNTAX

### ByIndex
```
Get-ForensicFileSlack [-VolumeName <String>] [[-Index] <Int32>]
```

### ByPath
```
Get-ForensicFileSlack [-Path] <String>
```

## DESCRIPTION
The Get-ForensicFileSlack cmdlet gets the specified volume&apos;s slack space as a byte array.

&quot;Slack space&quot; is the difference between the true size of a file&apos;s contents and the allocated size of a file on disk.

When NTFS stores data in a file, the data must be allocated in cluster-sized chunks (commonly 4096 bytes), which creates slack space.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicFileSlack -VolumeName \\.\C: -Index 0
```

This command uses Get-ForensicFileSlack to get the slack space from the file that is MFT record index 0 on the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicFileSlack -Path C:\windows\notepad.exe
```

This command uses Get-ForensicFileSlack to return the slack space for Notepad.exe.

## PARAMETERS

### -Index
The index number of the file to return slack space for.

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

### -Path
The path of the file to return slack space for.

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

