# Get-ForensicPartitionTable

## SYNOPSIS
Gets a list of partition objects on the specified disk.

## SYNTAX

```
Get-ForensicPartitionTable [-Path] <String>
```

## DESCRIPTION
The Get-ForensicPartitionTable cmdlet gets one or more Partition objects depending on the specified DrivePath.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicPartitionTable -DrivePath \\.\PHYSICALDRIVE0


Bootable     SystemID     StartSector     EndSector
--------     --------     -----------     ---------
True         NTFS         2048            125827072
```

This command gets all MBR partitions on the \\.\PHYSICALDRIVE0 disk.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicPartitionTable -Path \\.\PHYSICALDRIVE1

PartitionTypeGUID   : e3c9e316-0b5c-4db8-817d-f92df00215ae
UniquePartitionGUID : ff1a8a47-08f8-43ab-b410-53697f0b2323
StartingLBA         : 34
EndingLBA           : 65569
Attributes          : 0
PartitionName       : Microsoft reserved partition

PartitionTypeGUID   : ebd0a0a2-b9e5-4433-87c0-68b6b72699c7
UniquePartitionGUID : 6d76ae42-b6c1-4fbe-8d42-20cd366026b4
StartingLBA         : 67584
EndingLBA           : 2164735
Attributes          : 0
PartitionName       : Basic data partition

PartitionTypeGUID   : ebd0a0a2-b9e5-4433-87c0-68b6b72699c7
UniquePartitionGUID : d6795c3a-8a4d-4fb4-91a0-488812cce027
StartingLBA         : 2164736
EndingLBA           : 4261887
Attributes          : 0
PartitionName       : Basic data partition
```

This command gets all GPT partitions on the \\.\PHYSICALDRIVE1 disk.

## PARAMETERS

### -Path
Specified the physical drive to investigate. (Ex. \\.\PHYSICALDRIVE0)

```yaml
Type: String
Parameter Sets: (All)
Aliases: DrivePath

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### None


## OUTPUTS

### PowerForensics.GuidPartitionTableEntry[]

## NOTES

## RELATED LINKS

