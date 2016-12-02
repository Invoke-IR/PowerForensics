# Invoke-ForensicDD

## SYNOPSIS
Gets a byte-for-byte copy of a file, disk, or partition.

## SYNTAX

```
Invoke-ForensicDD [-InFile] <String> [[-OutFile] <String>] [[-Offset] <Int64>] [[-BlockSize] <UInt32>]
 [-Count] <UInt32>
```

## DESCRIPTION
The Invoke-DD cmdlet generates and returns an exact copy of a file, disk, or partition. 

Use the Offset (starting point), BlockSize (bytes/operation), and Count (# blocks) parameters to determine the segment of the InFile that is copied.

This cmdlet designed to work just like the popular dd Unix utility. For information about the dd utility, see "dd (Unix)" (https://en.wikipedia.org/wiki/Dd_%28Unix%29) in Wikipedia.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Invoke-ForensicDD Invoke-DD -InFile \\.\PHYSICALDRIVE0 -Offset 0 -Count 1
```

This command copies the first sector of the Master Boot Record of the
\\.\PHYSICALDRIVE0 disk to the console.

The command uses the default values for OutFile (stdout; the Windows PowerShell console) and BlockSize (512; 1 sector).

### Example 2
```
[ADMIN]: PS C:\> Invoke-ForensicDD Invoke-DD -InFile \\.\HARDDISKVOLUME1 -OutFile C:\Users\Public\Desktop\MBR -Offset 512 -BlockSize 1024 -Count 3
```

This command copies three 1024-size blocks of the specified volume to a file in the C:\Users\Public\Desktop\MBR directory. It begins copying at the second sector (after the 512-byte offset).

It uses the Offset parameter to specify the starting point of the copy operation, the BlockSize parameter to specify the bytes per copy operation, and the Count parameter to specify the number of copy operations. 

It also uses the OutFile parameter to specify a location for the output. The default is writing to the Windows PowerShell console (stdout).

## PARAMETERS

### -BlockSize
Specifies the number of bytes to read/write in each operation. The default value is 512 (1 disc sector).

When reading from a device, such as \\.\PHYSICALDRIVE0 or \\.\C:, the value of BlockSize must be divisible by 512.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases: 

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Count
Specifies the number of blocks that Invoke-ForensicDD reads from the file, disk, or partition. This parameter is required.

```yaml
Type: UInt32
Parameter Sets: (All)
Aliases: 

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -InFile
Specifies the file, disk or partition to be copied, for example \\.\PHYSICALDRIVE0, \\.\HARDDISKVOLUME1, or \\.\C:. This parameter is required.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Offset
Specifies the starting point in the file for the copy operation as a byte offset. This parameter is required.

```yaml
Type: Int64
Parameter Sets: (All)
Aliases: 

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -OutFile
Writes the output to the specified file or directory. 

This parameter is optional. But default, Invoke-ForensicDD writes the output to standard ouptut ("stdout"), which is the Windows PowerShell console, but you can use this parameter or redirect the output.

```yaml
Type: String
Parameter Sets: (All)
Aliases: 

Required: False
Position: 1
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

