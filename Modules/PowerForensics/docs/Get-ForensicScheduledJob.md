# Get-ForensicScheduledJob

## SYNOPSIS
Gets the scheduled jobs from the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicScheduledJob [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicScheduledJob -Path <String>
```

## DESCRIPTION
The Get-ForensicScheduledJob cmdlet parses the binary structure in the specified ScheduledJob file. If a file is not specified, Get-ForensicScheduledJob parses all .job files in the C:\Windows\Tasks directory.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the &apos;Run as administrator&apos; option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicScheduledJob -Volume C:
```

This example parses the scheduled jobs in the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicScheduledJob -Path C:\Windows\Tasks\GoogleUpdateTaskMachineUA.job


ProductVersion       : Windows8_1
FileVersion          : 1
Uuid                 : e841ef0f-7b64-45da-a8fb-1c3e05196ce1
ErrorRetryCount      : 0
ErrorRetryInterval   : 0
IdleDeadline         : 60
IdleWait             : 10
MaximumRuntime       : 4294967294
ExitCode             : 0
Status               : SCHED_S_TASK_READY
Flags                : RUN_ONLY_IF_DOCKED, KILL_IF_GOING_ON_BATTERIES, DISABLED
RunTime              : 11/17/2015 8:11:00 PM
RunningInstanceCount : 0
ApplicationName      : C:\Program Files\Google\Update\GoogleUpdate.exe
Parameters           : ?/ua /installsource scheduler
WorkingDirectory     :
Author               : ?WIN-OL5AKAF1OUJ\Uproot
Comment              : GKeeps your Google software up to date. If this task is disabled or stopped, your Google
                       software will not be kept up to date, meaning security vulnerabilities that may arise cannot be
                       fixed and features may not work. This task uninstalls itself when there is no Google software
                       using it.
StartTime            : 10/21/2015 8:11:00 AM
```

This command parses the scheduled jobs in the C:\Windows\Tasks\GoogleUpdateTaskMachineUA.job file.

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

### System.String

## NOTES

## RELATED LINKS

