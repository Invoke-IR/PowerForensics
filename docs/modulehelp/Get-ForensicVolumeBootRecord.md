# Get-ForensicVolumeBootRecord

## SYNOPSIS
Gets the Volume Boot Record from the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicVolumeBootRecord [[-VolumeName] <String>] [-AsBytes]
```

### ByPath
```
Get-ForensicVolumeBootRecord -Path <String> [-AsBytes]
```

## DESCRIPTION
The Get-ForensicVolumeBootRecord cmdlet reads the first 512 bytes (first sector) of the Logical Volume, also known as the Volume Boot Record, and parses its data structure to return a VolumeBootRecord object.

By default, this cmdlet parses the $Boot file on the C:\ drive. To specify the target drive, use the VolumeName parameter. To specify an exported $Boot file, use the Path parameter.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicVolumeBootRecord-VolumeName C


Signature             : NTFS    
BytesPerSector        : 512
SectorsPerCluster     : 8
ReservedSectors       : 0
MediaDescriptor       : 248
SectorsPerTrack       : 63
NumberOfHeads         : 255
HiddenSectors         : 2048
TotalSectors          : 125825023
LCN_MFT               : 786432
LCN_MFTMirr           : 2
ClustersPerFileRecord : 246
ClustersPerIndexBlock : 1
VolumeSN              : E3133CD4233CD4CA
Code                  : {0, 0, 0, 0...}
```

This command gets the VolumeBootRecord object for the C drive.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicVolumeBootRecord -VolumeName C: -AsBytes | Format-ForensicHex

Offset     _00_01_02_03_04_05_06_07_08_09_0A_0B_0C_0D_0E_0F  Ascii
------     ------------------------------------------------  -----
0x00000000  EB 52 90 4E 54 46 53 20 20 20 20 00 02 08 00 00  .R.NTFS    .....
0x00000010  00 00 00 00 00 F8 00 00 3F 00 FF 00 00 08 00 00  ........?.......
0x00000020  00 00 00 00 80 00 80 00 FF EF 7F 07 00 00 00 00  ................
0x00000030  00 00 0C 00 00 00 00 00 02 00 00 00 00 00 00 00  ................
0x00000040  F6 00 00 00 01 00 00 00 E3 13 3C D4 23 3C D4 CA  ..........&lt;.#&lt;..
0x00000050  00 00 00 00 FA 33 C0 8E D0 BC 00 7C FB 68 C0 07  .....3.....|.h..
0x00000060  1F 1E 68 66 00 CB 88 16 0E 00 66 81 3E 03 00 4E  ..hf......f.&gt;..N
0x00000070  54 46 53 75 15 B4 41 BB AA 55 CD 13 72 0C 81 FB  TFSu..A..U..r...
0x00000080  55 AA 75 06 F7 C1 01 00 75 03 E9 DD 00 1E 83 EC  U.u.....u.......
0x00000090  18 68 1A 00 B4 48 8A 16 0E 00 8B F4 16 1F CD 13  .h...H..........
0x000000A0  9F 83 C4 18 9E 58 1F 72 E1 3B 06 0B 00 75 DB A3  .....X.r.;...u..
0x000000B0  0F 00 C1 2E 0F 00 04 1E 5A 33 DB B9 00 20 2B C8  ........Z3... +.
0x000000C0  66 FF 06 11 00 03 16 0F 00 8E C2 FF 06 16 00 E8  f...............
0x000000D0  4B 00 2B C8 77 EF B8 00 BB CD 1A 66 23 C0 75 2D  K.+.w......f#.u-
0x000000E0  66 81 FB 54 43 50 41 75 24 81 F9 02 01 72 1E 16  f..TCPAu$....r..
0x000000F0  68 07 BB 16 68 52 11 16 68 09 00 66 53 66 53 66  h...hR..h..fSfSf
0x00000100  55 16 16 16 68 B8 01 66 61 0E 07 CD 1A 33 C0 BF  U...h..fa....3..
0x00000110  0A 13 B9 F6 0C FC F3 AA E9 FE 01 90 90 66 60 1E  .............f`.
0x00000120  06 66 A1 11 00 66 03 06 1C 00 1E 66 68 00 00 00  .f...f.....fh...
0x00000130  00 66 50 06 53 68 01 00 68 10 00 B4 42 8A 16 0E  .fP.Sh..h...B...
0x00000140  00 16 1F 8B F4 CD 13 66 59 5B 5A 66 59 66 59 1F  .......fY[ZfYfY.
0x00000150  0F 82 16 00 66 FF 06 11 00 03 16 0F 00 8E C2 FF  ....f...........
0x00000160  0E 16 00 75 BC 07 1F 66 61 C3 A1 F6 01 E8 09 00  ...u...fa.......
0x00000170  A1 FA 01 E8 03 00 F4 EB FD 8B F0 AC 3C 00 74 09  ............&lt;.t.
0x00000180  B4 0E BB 07 00 CD 10 EB F2 C3 0D 0A 41 20 64 69  ............A di
0x00000190  73 6B 20 72 65 61 64 20 65 72 72 6F 72 20 6F 63  sk read error oc
0x000001A0  63 75 72 72 65 64 00 0D 0A 42 4F 4F 54 4D 47 52  curred...BOOTMGR
0x000001B0  20 69 73 20 63 6F 6D 70 72 65 73 73 65 64 00 0D   is compressed..
0x000001C0  0A 50 72 65 73 73 20 43 74 72 6C 2B 41 6C 74 2B  .Press Ctrl+Alt+
0x000001D0  44 65 6C 20 74 6F 20 72 65 73 74 61 72 74 0D 0A  Del to restart..
0x000001E0  00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00  ................
0x000001F0  00 00 00 00 00 00 8A 01 A7 01 BF 01 00 00 55 AA  ..............U.
```

This commands get the bytes that represent the Volume Boot Record as a byte array.

## PARAMETERS

### -AsBytes
Returns Volume Boot Record as byte array instead of as VolumeBootRecord object.

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

### PowerForensics.Ntfs.VolumeBootRecord

## NOTES

## RELATED LINKS

