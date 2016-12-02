# Get-ForensicContent

## SYNOPSIS
{{Fill in the Synopsis}}

## SYNTAX

### ByPath
```
Get-ForensicContent [-Path] <String> [-Encoding <FileSystemCmdletProviderEncoding>] [-TotalCount <Int64>]
 [-Tail <Int64>]
```

### ByIndex
```
Get-ForensicContent [-VolumeName <String>] -Index <Int32> [-Encoding <FileSystemCmdletProviderEncoding>]
 [-TotalCount <Int64>] [-Tail <Int64>]
```

## DESCRIPTION
{{Fill in the Description}}

## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -Encoding
{{Fill Encoding Description}}

```yaml
Type: FileSystemCmdletProviderEncoding
Parameter Sets: (All)
Aliases: 
Accepted values: Unknown, String, Unicode, Byte, BigEndianUnicode, UTF8, UTF7, UTF32, Ascii, Default, Oem, BigEndianUTF32

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Index
{{Fill Index Description}}

```yaml
Type: Int32
Parameter Sets: ByIndex
Aliases: 

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Path
{{Fill Path Description}}

```yaml
Type: String
Parameter Sets: ByPath
Aliases: FullName

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Tail
{{Fill Tail Description}}

```yaml
Type: Int64
Parameter Sets: (All)
Aliases: Last

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -TotalCount
{{Fill TotalCount Description}}

```yaml
Type: Int64
Parameter Sets: (All)
Aliases: First, Head

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -VolumeName
{{Fill VolumeName Description}}

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

### None


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

