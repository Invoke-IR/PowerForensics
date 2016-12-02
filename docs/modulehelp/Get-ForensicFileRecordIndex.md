# Get-ForensicFileRecordIndex

## SYNOPSIS
Gets the MFT Record Index for the specified file.

## SYNTAX

```
Get-ForensicFileRecordIndex [-Path] <String>
```

## DESCRIPTION
The Get-ForensicFileRecordIndex cmdlet returns the Master File Table Record Index Number for the specified file.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicFileRecordIndex C:\Windows\Notepad.exe
```

This command uses Get-ForensicFileRecordIndex to get the Master File Table Record Index for C:\Windows\Notepad.exe.

## PARAMETERS

### -Path
The path of the file to return the Master File Table record index for.

```yaml
Type: String
Parameter Sets: (All)
Aliases: FullName

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

## INPUTS

### System.String


## OUTPUTS

### System.UInt64

## NOTES

## RELATED LINKS

