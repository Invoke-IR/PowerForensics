# Get-ForensicTimezone

## SYNOPSIS
Gets the system's timezone.

## SYNTAX

### ByVolume
```
Get-ForensicTimezone [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicTimezone -HivePath <String>
```

## DESCRIPTION
The Get-ForensicTimezone cmdlet parses the SYSTEM hive or a hive that you specify to derive the system's current timezone.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicTimezone

RegistryTimezone              dotNetStandardTimezone        dotNetDaylightTimezone                 IsDaylightSavingTime
----------------              ----------------------        ----------------------                 --------------------
Eastern Standard Time         Eastern Standard Time         Eastern Daylight Time                                 False
```

This command gets the time zones from the system hive.

### Example 2
```
[ADMIN]: PS C:\> Get-Timezone -HivePath C:\evidence\SYSTEM

RegistryTimezone              dotNetStandardTimezone        dotNetDaylightTimezone                 IsDaylightSavingTime
----------------              ----------------------        ----------------------                 --------------------
Eastern Standard Time         Eastern Standard Time         Eastern Daylight Time                                 False
```

This command gets the time zones from an exported SYSTEM hive.

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

### PowerForensics.Artifacts.Timezone

## NOTES

## RELATED LINKS

