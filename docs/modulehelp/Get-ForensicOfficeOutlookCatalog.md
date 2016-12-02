# Get-ForensicOfficeOutlookCatalog

## SYNOPSIS
Gets the location of Microsoft Outlook catalog (pst/ost) files.

## SYNTAX

### ByVolume
```
Get-ForensicOfficeOutlookCatalog [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicOfficeOutlookCatalog -HivePath <String>
```

## DESCRIPTION
The Get-ForensicOfficeOutlookCatalog cmdlet parses NTUSER.DAT registry hives to determine the location of Microsoft Outlook catalog files.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicOfficeOutlookCatalog
```

This example shows Get-ForensicOfficeOutlookCatalog parsing all user's NTUSER.DAT hives.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicOfficeOutlookCatalog -HivePath C:\Users\tester\NTUSER.DAT
```

This command uses the HivePath parameter of Get-ForensicOfficeOutlookCatalog to specify an exported NTUSER.DAT hive to parse.

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

### PowerForensics.Artifacts.MicrosoftOffice.OutlookCatalog

## NOTES

## RELATED LINKS

