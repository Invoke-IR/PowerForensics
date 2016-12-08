<#The MIT License (MIT)

Copyright (c) 2015 Jaime Olivares

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
#>

param (
    [string]$ProjectPath
)

$docsdir = "$($ProjectPath)\docs"
$modulehelpdir = "$($ProjectPath)\Modules\PowerForensics\docs"
$xmldir = "$($ProjectPath)\src\PowerForensicsCore\bin\Release\netstandard1.6"

Remove-Item -Path "$($docsdir)\modulehelp\*" -Force
Remove-Item -Path "$($docsdir)\publicapi\*" -Force

# Create an individual xml file foreach type
[xml]$xmldoc = Get-Content "$($xmldir)\PowerForensics.xml"

$sb = New-Object System.Text.StringBuilder
$first = $true

foreach($m in $xmldoc.doc.members.member)
{
    if($m.name.StartsWith('T:'))
    {
        if($first)
        {
            $first = $false
        }
        else
        {
            $null = $sb.Append('</members></doc>')
            $sb.ToString() | Out-File -FilePath "$($xmldir)\$($typename).xml"     
        }

        $typename = $m.name.TrimStart('T:')
        $null = $sb.Clear()
        $null = $sb.Append('<?xml version="1.0"?><doc><assembly><name>PowerForensics</name></assembly><members>')
    }
    
    $null = $sb.Append($m.OuterXml)
}

# Convert xml documents to Markdown files
$xslt = New-Object -TypeName "System.Xml.Xsl.XslCompiledTransform"

# xslt.Load(stylesheet);
$xslt.Load("$($ProjectPath)\xmldoc2md\xmldoc2md.xsl")

foreach($item in (Get-ChildItem "$($xmldir)\*.xml" -Exclude "PowerForensics.xml" -File))
{
    $output = "$($docsdir)\publicapi\$($item.name.Replace('xml','md'))"
    
    # xslt.Transform(sourceFile, null, sw);
    $xslt.Transform($item.FullName, $output)
}

# Build mkdocs.yml file based on documentation we have generated
$sb = New-Object System.Text.StringBuilder

$begin = @"
site_name: PowerForensics
repo_url: https://github.com/Invoke-IR/PowerForensics
site_favicon: favicon.ico
pages:
- Home: 'index.md'
- Get Involved: 'getinvolved.md'
- PowerShell Module:
    - Installation: 'moduleinstall.md'
    - Module Load Process: 'moduleload.md'
    - Cmdlets:`n
"@

$null = $sb.Append($begin)

# Copy Cmdlet Help from PS Module to docs directory
Copy-Item "$($modulehelpdir)\*" "$($docsdir)\modulehelp"

# iterate through cmdlets
foreach($item in (Get-ChildItem "$($docsdir)\modulehelp"))
{
    $filename = $($item.Name.Replace('.md',''))
    $cmdlet = "        - $($filename): 'modulehelp/$($item.Name)'"
    $null = $sb.Append("$($cmdlet)`n")
}

$mid = @"
- Development:
    - Public API:`n
"@

$null = $sb.Append($mid)

# iterate through types
foreach($item in (Get-ChildItem "$($docsdir)\publicapi"))
{
    $name = $($item.Name.Replace('.md',''))
    $type = "        - $($name): 'publicapi/$($item.Name)'"
    $null = $sb.Append("$($type)`n")
}

$end = @"
- About:
    - License: 'LICENSE.md'
"@

$null = $sb.Append($end)

$sb.ToString() | Out-File -FilePath "$($ProjectPath)\mkdocs.yml"

Remove-Item -Path "$($xmldir)\*.xml" -Exclude PowerForensics.xml -Force