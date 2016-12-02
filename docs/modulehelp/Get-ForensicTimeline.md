# Get-ForensicTimeline

## SYNOPSIS
Creates a forensic timeline.

## SYNTAX

```
Get-ForensicTimeline [[-VolumeName] <String>]
```

## DESCRIPTION
The Get-ForensicTimeline cmdlet creates a forensic timeline for the selected volume or logical drive. It runs several PowerForensics cmdlets and returns all results as ForensicTimeline objects, instead of objects of different types. The result is a forensic timeline, that is, is a chronology of diagnostic events.

The cmdlets that Invoke-ForensicTimeline runs include:
-- Get-ForensicScheduledJob
-- Get-ForensicShellLink
-- Get-ForensicUsnJrnl
-- Get-ForensicEventLog
-- Get-ForensicRegistryKey

The cmdlet returns data that includes MFT file record, registry keys, Amcache, event logs, and much more.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicTimeline -VolumeName C
```

This command creates a forensic timeline for the C: volume on the local system.

### Example 2
```
[ADMIN]: PS C:\> $t = Get-ForensicTimeline -VolumeName D:

PS C:\&gt; $t[0]

Date         : 1/1/1999 12:00:00 AM
ActivityType : MACB
Source       : SCHEDULEDJOB
SourceType   :
User         : Server01\User01
FileName     : C:\Program Files (x86)\Dropbox\Update\DropboxUpdate.exe
Description  : [PROGRAM EXECUTION] C:\Program Files (x86)\Dropbox\Update\DropboxUpdate.exe executed
               at 1/1/1999 12:00:00 AM via Scheduled Job
```

This example shows the properties of the ForensicTimeline object. Invoke-ForensicTimeline returns the results of the disparate cmdlets in the same object type.

The first command command creates a forensic timeline for the D: volume on the local system and saves the results in the $t variable.

The second command displays the properties of the first object in $t, which was produced by the Get-ForensicScheduledJob cmdlet.

### Example 3
```
[ADMIN]: PS C:\> Get-ForensicTimeline -VolumeName \\.\C: | Group-Object -Property Source | Format-Table Count, Name

  Count Name
  ----- ----
      4 SCHEDULEDJOB
   1916 ShellLink
1276123 MFT
 293715 USNJRNL
   9319 EVENTLOG
 423900 REGISTRY
```

This command runs Invoke-ForensicTimeline on the C: drive. Then, it groups the objects by the value of their Source property so you can see the cmdlets that were run to produce the data, and it formats the results into a table of Count and Name, so the values of these properties are not truncated.

The output of this command varies based on the system and drive contents.

### Example 4
```
[ADMIN]: PS C:\> Get-ForensicTimeline | Sort-Object -Property Date
```

The command returns the output of Invoke-ForensicTimeline in chronological order to produce a true timeline of the events.

## PARAMETERS

### -VolumeName
Specifies the volume or logical partition that Invoke-ForensicTimeline analyzes. 

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

### PowerForensics.Formats.ForensicTimeline

## NOTES

## RELATED LINKS

