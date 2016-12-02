# Get-ForensicBootSector

## SYNOPSIS
Gets the Boot Sector (Master Boot Record or Guid Partition Table) for the specified physical drive.

## SYNTAX

```
Get-ForensicBootSector [-Path] <String> [-AsBytes]
```

## DESCRIPTION
The Get-ForensicBootSector cmdlet parses Logical Block Address 0 of the specified physical drive, determines whether the
disk is formatted using a Master Boot Record or a Guid Partition Table, and returns a MasterBootRecord or GuidPartitionTable object. 

You can also use the AsBytes switch parameter to return the raw bytes of the Boot Sector.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

Use this cmdlet instead of Get-MasterBootRecord or Get-GuidPartitionTable when the disk's partitioning scheme is unknown.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicBootSector -Path \\.\PHYSICALDRIVE0

MBRSignature    DiskSignature    BootCode                  PartitionTable
------------    -------------    --------                  --------------
Windows 6.1+    82D4BA7D         {51, 192, 142, 208...}    {NTFS}
```

This example shows Get-ForensicBootSector being used to return the MasterBootRecord object from \\.\PHYSICALDRIVE0.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicBootSector -Path \\.\PHYSICALDRIVE1

Revision                 : 0.1
HeaderSize               : 92
MyLBA                    : 1
AlternateLBA             : 20971519
FirstUsableLBA           : 34
LastUsableLBA            : 20971486
DiskGUID                 : f913e110-0835-4cf1-96c7-380b5db4a42d
PartitionEntryLBA        : 2
NumberOfPartitionEntries : 128
SizeOfPartitionEntry     : 128
PartitionTable           : {Microsoft reserved partition, Basic data partition, Basic data partition}
```

This example shows Get-ForensicBootSector being used to return the GPT object from \\.\PHYSICALDRIVE1.

### Example 3
```
[ADMIN]: PS C:\> Get-ForensicBootSector -Path \\.\PHYSICALDRIVE2 -AsBytes | Format-Hex

Offset     _00_01_02_03_04_05_06_07_08_09_0A_0B_0C_0D_0E_0F  Ascii           
------     ------------------------------------------------  -----           
0x00000000  33 C0 8E D0 BC 00 7C 8E C0 8E D8 BE 00 7C BF 00  3.....|......|..
0x00000010  06 B9 00 02 FC F3 A4 50 68 1C 06 CB FB B9 04 00  .......Ph.......
0x00000020  BD BE 07 80 7E 00 00 7C 0B 0F 85 0E 01 83 C5 10  ....~..|........
0x00000030  E2 F1 CD 18 88 56 00 55 C6 46 11 05 C6 46 10 00  .....V.U.F...F..
0x00000040  B4 41 BB AA 55 CD 13 5D 72 0F 81 FB 55 AA 75 09  .A..U..]r...U.u.
0x00000050  F7 C1 01 00 74 03 FE 46 10 66 60 80 7E 10 00 74  ....t..F.f`.~..t
0x00000060  26 66 68 00 00 00 00 66 FF 76 08 68 00 00 68 00  &amp;fh....f.v.h..h.
0x00000070  7C 68 01 00 68 10 00 B4 42 8A 56 00 8B F4 CD 13  |h..h...B.V.....
0x00000080  9F 83 C4 10 9E EB 14 B8 01 02 BB 00 7C 8A 56 00  ............|.V.
0x00000090  8A 76 01 8A 4E 02 8A 6E 03 CD 13 66 61 73 1C FE  .v..N..n...fas..
0x000000A0  4E 11 75 0C 80 7E 00 80 0F 84 8A 00 B2 80 EB 84  N.u..~..........
0x000000B0  55 32 E4 8A 56 00 CD 13 5D EB 9E 81 3E FE 7D 55  U2..V...]...&gt;.}U
0x000000C0  AA 75 6E FF 76 00 E8 8D 00 75 17 FA B0 D1 E6 64  .un.v....u.....d
0x000000D0  E8 83 00 B0 DF E6 60 E8 7C 00 B0 FF E6 64 E8 75  ......`.|....d.u
0x000000E0  00 FB B8 00 BB CD 1A 66 23 C0 75 3B 66 81 FB 54  .......f#.u;f..T
0x000000F0  43 50 41 75 32 81 F9 02 01 72 2C 66 68 07 BB 00  CPAu2....r,fh...
0x00000100  00 66 68 00 02 00 00 66 68 08 00 00 00 66 53 66  .fh....fh....fSf
0x00000110  53 66 55 66 68 00 00 00 00 66 68 00 7C 00 00 66  SfUfh....fh.|..f
0x00000120  61 68 00 00 07 CD 1A 5A 32 F6 EA 00 7C 00 00 CD  ah.....Z2...|...
0x00000130  18 A0 B7 07 EB 08 A0 B6 07 EB 03 A0 B5 07 32 E4  ..............2.
0x00000140  05 00 07 8B F0 AC 3C 00 74 09 BB 07 00 B4 0E CD  ......&lt;.t.......
0x00000150  10 EB F2 F4 EB FD 2B C9 E4 64 EB 00 24 02 E0 F8  ......+..d..$...
0x00000160  24 02 C3 49 6E 76 61 6C 69 64 20 70 61 72 74 69  $..Invalid parti
0x00000170  74 69 6F 6E 20 74 61 62 6C 65 00 45 72 72 6F 72  tion table.Error
0x00000180  20 6C 6F 61 64 69 6E 67 20 6F 70 65 72 61 74 69   loading operati
0x00000190  6E 67 20 73 79 73 74 65 6D 00 4D 69 73 73 69 6E  ng system.Missin
0x000001A0  67 20 6F 70 65 72 61 74 69 6E 67 20 73 79 73 74  g operating syst
0x000001B0  65 6D 00 00 00 63 7B 9A B3 64 EE 2F 00 00 00 20  em...c{..d./... 
0x000001C0  21 00 07 65 24 41 00 08 00 00 00 00 10 00 00 65  !..e$A.........e
0x000001D0  25 41 0B AA 28 82 00 08 10 00 00 00 10 00 00 AA  %A..(...........
0x000001E0  29 82 0B EF 2C C3 00 08 20 00 00 00 10 00 00 EF  )...,... .......
0x000001F0  2D C3 0F FE FF 90 00 08 30 00 00 F0 AF 00 55 AA  -.......0.....U.
```

This example shows how the AsBytes parameter returns the Boot Sector as a byte array.

## PARAMETERS

### -AsBytes
Specifies that the Guid Partition Table is returned as raw bytes instead of as a custom object.

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
Specifies the physical drive to investigate. (Ex. \\.\PHYSICALDRIVE0)

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

### PowerForensics.MasterBootRecord

### PowerForensics.GuidPartitionTable

### System.Byte[]

## NOTES

## RELATED LINKS

