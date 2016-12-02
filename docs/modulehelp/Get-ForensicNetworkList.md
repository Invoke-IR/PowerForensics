# Get-ForensicNetworkList

## SYNOPSIS
Gets a list of networks that the system has previously been connected to.

## SYNTAX

### ByVolume
```
Get-ForensicNetworkList [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicNetworkList -HivePath <String>
```

## DESCRIPTION
The Get-ForensicNetworkList cmdlet parses the SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList key to derive a list of previously connected networks.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicNetworkList
```

This command uses Get-ForensicNetworkList to parse the SOFTWARE hive on the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicNetworkList -HivePath C:\evidence\SOFTWARE
```

This command uses Get-ForensicNetworkList on an exported SOFTWARE hive.

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

### PowerForensics.Artifacts.NetworkList

## NOTES

## RELATED LINKS

