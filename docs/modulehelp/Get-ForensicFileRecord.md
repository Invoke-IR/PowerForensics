# Get-ForensicFileRecord

## SYNOPSIS
Gets the file records from the Master File Table of the specified volume.

## SYNTAX

### ByIndex
```
Get-ForensicFileRecord [-VolumeName <String>] [[-Index] <Int32>] [-AsBytes]
```

### ByPath
```
Get-ForensicFileRecord [-Path] <String> [-AsBytes]
```

### ByMftPath
```
Get-ForensicFileRecord -MftPath <String>
```

## DESCRIPTION
The Get-ForensicFileRecord cmdlet parses the $MFT file and returns an array of FileRecord entries.

By default, this cmdlet parses the $MFT file on the C:\ drive. To change the target drive, use the VolumeName parameter or use the Path parameter to specify an exported $MFT file.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> $mft = Get-ForensicFileRecord
```

This command uses Get-ForensicFileRecord to return all records from the Master File Table on the default C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicFileRecord -VolumeName C: -Index 0


FullName             : C:\$MFT
Name                 : $MFT
SequenceNumber       : 1
RecordNumber         : 0
ParentSequenceNumber : 5
ParentRecordNumber   : 5
Directory            : False
Deleted              : False
ModifiedTime         : 8/13/2015 9:35:13 PM
AccessedTime         : 8/13/2015 9:35:13 PM
ChangedTime          : 8/13/2015 9:35:13 PM
BornTime             : 8/13/2015 9:35:13 PM
FNModifiedTime       : 8/13/2015 9:35:13 PM
FNAccessedTime       : 8/13/2015 9:35:13 PM
FNChangedTime        : 8/13/2015 9:35:13 PM
FNBornTime           : 8/13/2015 9:35:13 PM
```

This command uses Get-ForensicFileRecord to get the Master File Table record at index 0 on the C:\ logical volume.

### Example 3
```
[ADMIN]: PS C:\> Get-ForensicFileRecord -Path C:\Windows\system32\cmd.exe


FullName             : C:\Windows\System32\cmd.exe
Name                 : cmd.exe
SequenceNumber       : 1
RecordNumber         : 38224
ParentSequenceNumber : 1
ParentRecordNumber   : 4061
Directory            : False
Deleted              : False
ModifiedTime         : 7/10/2015 10:59:58 AM
AccessedTime         : 7/10/2015 10:59:58 AM
ChangedTime          : 10/21/2015 2:07:46 PM
BornTime             : 7/10/2015 10:59:58 AM
FNModifiedTime       : 8/13/2015 9:35:46 PM
FNAccessedTime       : 8/13/2015 9:35:46 PM
FNChangedTime        : 8/13/2015 9:35:46 PM
FNBornTime           : 8/13/2015 9:35:46 PM<
```

This command uses Get-ForensicFileRecord to get the Master File Table record for C:\Windows\system32\cmd.exe.

### Example 4
```
[ADMIN]: PS C:\> $mft = Get-ForensicFileRecord -MftPath C:\evidence\MFT
```

This command uses Get-ForensicFileRecord to return all Master File Table records from the exported MFT at C:\evidence\MFT.

### Example 5
```
[ADMIN]: PS C:\> Get-ForensicFileRecord -Path C:\Windows\notepad.exe -AsBytes | Format-Hex

Offset     _00_01_02_03_04_05_06_07_08_09_0A_0B_0C_0D_0E_0F  Ascii
------     ------------------------------------------------  -----
0x00000000  46 49 4C 45 30 00 03 00 73 2B 77 13 00 00 00 00  FILE0...s+w.....
0x00000010  01 00 02 00 38 00 01 00 00 03 00 00 00 04 00 00  ....8...........
0x00000020  00 00 00 00 00 00 00 00 0E 00 00 00 F0 5F 01 00  ............._..
0x00000030  0B 00 72 00 00 00 00 00 10 00 00 00 60 00 00 00  ..r.........`...
0x00000040  00 00 00 00 00 00 00 00 48 00 00 00 18 00 00 00  ........H.......
0x00000050  28 FE C1 84 F0 D5 D0 01 20 47 DB 80 8A CD D0 01  (....... G......
0x00000060  DB 9C 17 87 F7 D5 D0 01 28 FE C1 84 F0 D5 D0 01  ........(.......
0x00000070  20 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00   ...............
0x00000080  00 00 00 00 15 02 00 00 00 00 00 00 00 00 00 00  ................
0x00000090  B0 05 DB 02 00 00 00 00 30 00 00 00 70 00 00 00  ........0...p...
0x000000A0  00 00 00 00 00 00 0B 00 58 00 00 00 18 00 01 00  ........X.......
0x000000B0  B9 05 00 00 00 00 01 00 28 FE C1 84 F0 D5 D0 01  ........(.......
0x000000C0  20 47 DB 80 8A CD D0 01 3C 40 96 B8 F0 D5 D0 01   G......&lt;@......
0x000000D0  28 FE C1 84 F0 D5 D0 01 00 50 03 00 00 00 00 00  (........P......
0x000000E0  00 48 03 00 00 00 00 00 20 00 00 00 00 00 00 00  .H...... .......
0x000000F0  0B 00 6E 00 6F 00 74 00 65 00 70 00 61 00 64 00  ..n.o.t.e.p.a.d.
0x00000100  2E 00 65 00 78 00 65 00 30 00 00 00 70 00 00 00  ..e.x.e.0...p...
0x00000110  00 00 00 00 00 00 09 00 58 00 00 00 18 00 01 00  ........X.......
0x00000120  FC 50 01 00 00 00 02 00 28 FE C1 84 F0 D5 D0 01  .P......(.......
0x00000130  20 47 DB 80 8A CD D0 01 63 33 61 B6 F0 D5 D0 01   G......c3a.....
0x00000140  28 FE C1 84 F0 D5 D0 01 00 50 03 00 00 00 00 00  (........P......
0x00000150  00 48 03 00 00 00 00 00 20 00 00 00 00 00 00 00  .H...... .......
0x00000160  0B 03 6E 00 6F 00 74 00 65 00 70 00 61 00 64 00  ..n.o.t.e.p.a.d.
0x00000170  2E 00 65 00 78 00 65 00 80 00 00 00 48 00 00 00  ..e.x.e.....H...
0x00000180  01 00 00 00 00 00 04 00 00 00 00 00 00 00 00 00  ................
0x00000190  34 00 00 00 00 00 00 00 40 00 00 00 00 00 00 00  4.......@.......
0x000001A0  00 50 03 00 00 00 00 00 00 48 03 00 00 00 00 00  .P.......H......
0x000001B0  00 48 03 00 00 00 00 00 31 35 18 9A 27 00 00 00  .H......15..&apos;...
0x000001C0  D0 00 00 00 20 00 00 00 00 00 00 00 00 00 0C 00  .... ...........
0x000001D0  08 00 00 00 18 00 00 00 8D 00 00 00 94 00 00 00  ................
0x000001E0  E0 00 00 00 B0 00 00 00 00 00 00 00 00 00 0D 00  ................
0x000001F0  94 00 00 00 18 00 00 00 94 00 00 00 00 16 72 00  ..............r.
0x00000200  24 4B 45 52 4E 45 4C 2E 50 55 52 47 45 2E 45 53  $KERNEL.PURGE.ES
0x00000210  42 43 41 43 48 45 00 72 00 00 00 03 00 02 0C 42  BCACHE.r.......B
0x00000220  73 2C DB 07 D6 D0 01 00 C7 9B 0B C7 89 D0 01 02  s,..............
0x00000230  00 00 00 54 00 27 01 0C 80 00 00 20 1C FE AD 81  ...T.&apos;..... ....
0x00000240  46 39 9A 4D FE 67 59 E9 30 3C 30 C5 21 CF F3 83  F9.M.gY.0&lt;0.!...
0x00000250  0E 71 77 E8 7E 64 02 1D C3 DA 49 31 1B 00 04 80  .qw.~d....I1....
0x00000260  00 00 14 B4 F8 DF 0F CF 38 8F 82 08 41 92 26 4C  ........8...A.&amp;L
0x00000270  8B 81 D0 25 5F 31 77 12 03 80 F6 10 83 6B 95 CF  ...%_1w......k..
0x00000280  01 80 B6 D8 39 88 FC D0 01 00 00 00 00 00 00 00  ....9...........
0x00000290  00 01 00 00 68 00 00 00 00 09 18 00 00 00 0A 00  ....h...........
0x000002A0  38 00 00 00 30 00 00 00 24 00 54 00 58 00 46 00  8...0...$.T.X.F.
0x000002B0  5F 00 44 00 41 00 54 00 41 00 00 00 00 00 00 00  _.D.A.T.A.......
0x000002C0  05 00 00 00 00 00 05 00 01 00 00 00 01 00 00 00  ................
0x000002D0  C1 10 00 00 00 00 00 00 03 72 0A 00 02 00 00 00  .........r......
0x000002E0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000002F0  02 00 00 00 4F 1A 00 00 FF FF FF FF 82 79 47 11  ....O........yG.
0x00000300  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000310  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000320  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000330  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000340  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000350  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000360  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000370  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000380  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x00000390  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000003A0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000003B0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000003C0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000003D0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000003E0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000003F0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
```

This command uses Get-ForensicFileRecord to get the Master File Table record for C:\Windows\notepad.exe as a byte array.

## PARAMETERS

### -AsBytes
Returns Master File Table Entry as byte array instead of as FileRecord object.

```yaml
Type: SwitchParameter
Parameter Sets: ByIndex, ByPath
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Index
The index number of the desired Master File Table entry.

```yaml
Type: Int32
Parameter Sets: ByIndex
Aliases: 

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MftPath
Path to an exported Master File Table.

```yaml
Type: String
Parameter Sets: ByMftPath
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
The path of the desired Master File Table entry.

```yaml
Type: String
Parameter Sets: ByPath
Aliases: FullName

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -VolumeName
Specifies the name of the volume or logical partition.

Enter the volume name in one of the following formats: \\.\C:, C:, or C.

```yaml
Type: String
Parameter Sets: ByIndex
Aliases: 

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

## INPUTS

### System.String


## OUTPUTS

### PowerForensics.Ntfs.FileRecord

### System.Byte

## NOTES

## RELATED LINKS

