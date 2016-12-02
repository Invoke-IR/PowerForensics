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
    [string]$xml,
    [string]$xsl,
    [string]$docsdir,
    [string]$helpdir
)

# Convert VS XML Docs to Markdown
# Create an individual xml file foreach type
[xml]$xmldoc = Get-Content $xml

# Convert xml documents to Markdown files
$xslt = New-Object -TypeName "System.Xml.Xsl.XslCompiledTransform"

# xslt.Load(stylesheet);
$xslt.Load($xsl)

# xslt.Transform(sourceFile, null, sw);
$xslt.Transform($xml, "$($docsdir)\publicapi.md")

# Combine PowerShell Module Help Markdown files
Remove-Item -Path "$($docsdir)\cmdlethelp.md" -Force
foreach($file in (Get-ChildItem $helpdir))
{
    Get-Content -Encoding Ascii $file.FullName | Select-Object -Skip 5 | Out-File -Encoding Ascii -FilePath "$($docsdir)\cmdlethelp.md" -Append
}