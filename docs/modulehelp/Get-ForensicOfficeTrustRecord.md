# Get-ForensicOfficeTrustRecord

## SYNOPSIS
Gets files that have been explicity trusted by users of Microsoft Offfice applications.

## SYNTAX

### ByVolume
```
Get-ForensicOfficeTrustRecord [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicOfficeTrustRecord -HivePath <String>
```

## DESCRIPTION
The Get-ForensicOfficeFileMru cmdlet parses NTUSER.DAT registry hives to determine what files have been explicitly trusted by users of Microsoft Office applications.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicOfficeTrustRecord
```

This example shows Get-ForensicOfficeTrustRecord parsing all user's NTUSER.DAT hives.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicOfficeTrustRecord -HivePath C:\Users\tester\NTUSER.DAT
```

This command uses the HivePath parameter of Get-ForensicOfficeTrustRecord to specify an exported NTUSER.DAT hive to parse.

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

### PowerForensics.Artifacts.MicrosoftOffice.TrustRecords

## NOTES

## RELATED LINKS

