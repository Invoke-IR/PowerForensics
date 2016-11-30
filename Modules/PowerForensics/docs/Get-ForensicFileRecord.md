---
external help file: PowerForensics-help.xml
online version: 
schema: 2.0.0
---

# Get-ForensicFileRecord

## SYNOPSIS
{{Fill in the Synopsis}}

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
{{Fill in the Description}}

## EXAMPLES

### Example 1
```
PS C:\> {{ Add example code here }}
```

{{ Add example description here }}

## PARAMETERS

### -AsBytes
{{Fill AsBytes Description}}

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
{{Fill Index Description}}

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
{{Fill MftPath Description}}

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
{{Fill Path Description}}

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

### System.String


## OUTPUTS

### System.Object

## NOTES

## RELATED LINKS

