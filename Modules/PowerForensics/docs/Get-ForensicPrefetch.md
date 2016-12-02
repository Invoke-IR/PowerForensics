# Get-ForensicPrefetch

## SYNOPSIS
Gets the Prefetch objects from the specified volume or file.

## SYNTAX

### ByVolume
```
Get-ForensicPrefetch [[-VolumeName] <String>] [-Fast]
```

### ByPath
```
Get-ForensicPrefetch -Path <String> [-Fast]
```

## DESCRIPTION
The Get-ForensicPrefetch cmdlet parses the binary structure in the specified Prefetch file. If a file is not specified, Get-Prefetch parses all .pf files in the C:\Windows\Prefetch directory.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicPrefetch
```

This command gets an array of all Prefetch files in the C:\Windows\Prefetch directory.

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicPrefetch -Path C:\Windows\Prefetch\CMD.EXE-89305D47.pf


Version            : WINDOWS_8
Name               : CMD.EXE
Path               : \DEVICE\HARDDISKVOLUME1\WINDOWS\SYSTEM32\CMD.EXE
PathHash           : 89305D47
DependencyCount    : 25
PrefetchAccessTime : {4/3/2015 4:29:25 AM, 4/3/2015 4:29:18 AM, 3/31/2015 12:33:17 PM, 3/31/2015 
                     12:22:42 PM...}
DeviceCount        : 1
RunCount           : 40
```

This command parses the Prefetch file specified by the Path parameter.

## PARAMETERS

### -Fast
Use the Windows API to list files within the C:\Windows\Prefetch directory. WARNING: Not forensically sound.

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

### PowerForensics.Artifacts.Prefetch

## NOTES

## RELATED LINKS

