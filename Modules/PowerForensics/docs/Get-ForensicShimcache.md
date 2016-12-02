# Get-ForensicShimcache

## SYNOPSIS
Gets previously run commands from the Shimcache forensic artifact.

## SYNTAX

### ByVolume
```
Get-ForensicShimcache [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicShimcache -HivePath <String>
```

## DESCRIPTION
The Get-ForensicShimcache cmdlet parses the AppCompatCache (AppCompatibility on XP) registry value to derive applications that were recently used. If you don&apos;t specify a hive path (-HivePath), the cmdlet parses the local System hive.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicShimcache
```

This example shows Get-ForensicShimcache being run against the default System Hive (C:\Windows\system32\config\SYSTEM)

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicShimcache -HivePath C:\evidence\system
```

This is an example of Get-ForensicShimcache taking an exported SYSTEM hive as an argument.

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

### PowerForensics.Artifacts.Shimcache

## NOTES

## RELATED LINKS

