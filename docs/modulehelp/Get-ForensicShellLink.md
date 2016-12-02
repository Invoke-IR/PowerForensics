# Get-ForensicShellLink

## SYNOPSIS
Gets infromation about Shell Link (.LNK) files on the specified volume.

## SYNTAX

### ByVolume
```
Get-ForensicShellLink [[-VolumeName] <String>]
```

### ByPath
```
Get-ForensicShellLink -Path <String>
```

## DESCRIPTION
The Get-ForensicShellLink cmdlet parses the binary structure in the specified ShellLink (.lnk) file. If you do not specify a file, Get-ShellLink parses all .lnk files in the specified volume.

Except as noted, the cmdlets in the PowerForensics module require the permissions of a member of the Administrators group on the computer. To run them, start Windows PowerShell with the 'Run as administrator' option.

## EXAMPLES

### Example 1
```
[ADMIN]: PS C:\> Get-ForensicShellLink
```

This command parses all .lnk files on the C:\ logical volume.

### Example 2
```
[ADMIN]: PS C:\> Get-ShellLink -Path C:\test\PowerForensics.dll-Help.xml.lnk


Path                      : PowerForensics.dll-Help.xml.lnk
CreationTime              : 11/6/2015 8:01:39 PM
AccessTime                : 11/16/2015 2:45:45 AM
WriteTime                 : 11/17/2015 10:18:59 PM
FileSize                  : 202700
LocalBasePath             : C:\test\PowerForensics.dll-Help.xml
CommandLineArguments      :
CommonNetworkRelativeLink :
```

This command, which runs Get-ForensicShellLink with a single file path, gets only the corresponding ShellLink object.

## PARAMETERS

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

### PowerForensics.Artifacts.ShellLink

## NOTES

## RELATED LINKS

