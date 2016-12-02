# Get-ForensicEventLog

## SYNOPSIS
Gets the events in an event log or in all event logs.

## SYNTAX

### ByVolume
```
Get-ForensicEventLog [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicEventLog -Path <String>
```

## DESCRIPTION
The Get-ForensicEventLog cmdlet parses the specified event Log file and returns an array of EventRecord objects. If you don't specify an event log, Get-ForensicEventLog parses all event logs in the C:\Windows\system32\winevt\Logs directory.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicEventLog
```

This command runs Get-ForensicEventLog to parse all event logs in the C:\windows\system32\winevt\logs\ directory.

### Example 2
```
[ADMIN]: PS C:\> Get-EventLog -Path C:\evidence\Application.evtx
```

This command uses Get-EventLog to parse an exported Application event log

## PARAMETERS

### -Path
Specifies the path of the file to be parsed.

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
Specifies the name of the volume or logical partition (Ex. \\.\C:, \\.\HARDDISKVOLUME1, or C).

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

### PowerForensics.EventLog.EventRecord

## NOTES

## RELATED LINKS

