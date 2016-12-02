# Get-ForensicGuidPartitionTable

## SYNOPSIS
Gets the Guid Partition Table for the specified physical drive.

## SYNTAX

```
Get-ForensicGuidPartitionTable [-Path] <String> [-AsBytes]
```

## DESCRIPTION
The Get-ForensicGuidPartitionTable cmdlet gets the Guid Partition Table for the specified physical drive.

By default, Get-ForensicGuidPartitionTable returns a GuidPartitionTable object. You can also use the AsBytes switch parameter to return the raw bytes of the Guid Partition Table.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicGuidPartitionTable -Path \\.\PHYSICALDRIVE1

Revision                 : 0.1
HeaderSize               : 92
MyLBA                    : 1
AlternateLBA             : 20971519
FirstUsableLBA           : 34
LastUsableLBA            : 20971486
DiskGuid                 : f913e110-0835-4cf1-96c7-380b5db4a42d
PartitionEntryLBA        : 2
NumberOfPartitionEntries : 128
SizeOfPartitionEntry     : 128
PartitionTable           : {Microsoft reserved partition, Basic data partition, Basic data partition}
```

This is an example of Get-GuidPartitionTable being run against \\.\PHYSICALDRIVE1

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicGuidPartitionTable -Path \\.\PHYSICALDRIVE1 -AsBytes | Format-Hex

Offset     _00_01_02_03_04_05_06_07_08_09_0A_0B_0C_0D_0E_0F  Ascii
------     ------------------------------------------------  -----
0x00000000  45 46 49 20 50 41 52 54 00 00 01 00 5C 00 00 00  EFI PART....\...
0x00000010  F3 73 9F 97 00 00 00 00 01 00 00 00 00 00 00 00  .s..............
0x00000020  FF FF 3F 01 00 00 00 00 22 00 00 00 00 00 00 00  ..?.....&quot;.......
0x00000030  DE FF 3F 01 00 00 00 00 10 E1 13 F9 35 08 F1 4C  ..?.........5..L
0x00000040  96 C7 38 0B 5D B4 A4 2D 02 00 00 00 00 00 00 00  ..8.]..-........
0x00000050  80 00 00 00 80 00 00 00 3B 04 A4 F8 00 00 00 00  ........;.......
0x00000060  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000070  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000080  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000090  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000000A0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000000B0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000000C0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000000D0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000000E0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000000F0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000100  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000110  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000120  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000130  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000140  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000150  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000160  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000170  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000180  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000190  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001A0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001B0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001C0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001D0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001E0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001F0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
```

This command uses Get-ForensicGuidPartitionTable and its AsBytes parameter to return the GPT as a byte array.

## PARAMETERS

### -AsBytes
Returns Guid Partition Table as byte array instead of as GuidPartitionTable object.

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

### PowerForensics.GuidPartitionTable

### System.Byte[]

## NOTES

## RELATED LINKS

