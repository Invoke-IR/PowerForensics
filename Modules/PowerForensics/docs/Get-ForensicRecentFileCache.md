# Get-ForensicRecentFileCache

## SYNOPSIS
Gets previously run commands from the RecentFileCache.bcf file.

## SYNTAX

### ByVolume
```
Get-ForensicRecentFileCache [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicRecentFileCache -Path <String>
```

## DESCRIPTION
The Get-ForensicRecentFileCache cmdlet parses the RecentFileCache.bcf file to derive applications that were recently used. If you don&apos;t specify a file path (-Path), the cmdlet parses the C:\Windows\AppCompat\Programs\RecentFileCache.bcf.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicRecentFileCache
```

This example shows Get-ForensicRecentFileCache being run against the default RecentFileCache.bcf (C:\Windows\AppCompat\Programs\RecentFileCache.bcf)

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicRecentFileCache -Path C:\Windows\AppCompat\Programs\RecentFileCache.bcf
```

This is an example of Get-ForensicRecentFileCache taking a RecentFileCache.bcf file path as an argument.

## PARAMETERS

### -Path
Path to RecentFileCache.bcf file to process.

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

