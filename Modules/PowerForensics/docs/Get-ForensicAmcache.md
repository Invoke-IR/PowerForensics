# Get-ForensicAmcache

## SYNOPSIS
Gets previously run commands from the Amcache.hve registry hive.

## SYNTAX

### ByVolume
```
Get-ForensicAmcache [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicAmcache -HivePath <String>
```

## DESCRIPTION
The Get-Amcache cmdlet parses the Amcache.hve registry hive to derive applications that were recently used. If you don&apos;t specify a hive path (-HivePath), the cmdlet parses the C:\Windows\AppCompat\Programs\Amcache.hve.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-Amcache
```

This example shows Get-Amcache being run against the default Amcache.hve (C:\Windows\AppCompat\Programs\Amcache.hve)

### Example 2
```
[ADMIN]: PS C:\> Get-Amcache -HivePath C:\Windows\AppCompat\Programs\Amcache.hve
```

This is an example of Get-Amcache taking a Amcache.hve as an argument.

## PARAMETERS

### -HivePath
Registry hive to parse.

```yaml
Type: String
Parameter Sets: ByPath
Aliases: Path

Required: True
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

### None


## OUTPUTS

### PowerForensics.Artifacts.Amcache

## NOTES

## RELATED LINKS

