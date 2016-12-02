# Get-ForensicSid

## SYNOPSIS
Gets the system's Security Identifier (SID).

## SYNTAX

### ByVolume
```
Get-ForensicSid [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicSid -HivePath <String>
```

## DESCRIPTION
The Get-ForensicSid cmdlet parses the SAM hive to derive the system's Security Identifier.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicSid | Format-List

BinaryLength     : 24
AccountDomainSid : S-1-5-21-390730339-1025693957-1587674390
Value            : S-1-5-21-390730339-1025693957-1587674390
```

This command parses the C:\Windows\system32\config\SAM hive and returns the results in a list.

### Example 2
```
[ADMIN]: PS C:\> Get-ForensicSid -HivePath C:\Windows\System32\config\SAM

BinaryLength     : 24
AccountDomainSid : S-1-5-21-390730339-1025693957-1587674390
Value            : S-1-5-21-390730339-1025693957-1587674390
```

This command uses the HivePath parameter of Get-ForensicSid to specify an exported SAM hive to parse.

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

### System.Security.Principal.SecurityIdentifier

## NOTES

## RELATED LINKS

