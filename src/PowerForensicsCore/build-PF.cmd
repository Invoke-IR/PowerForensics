del ..\..\Modules\PowerForensics\coreclr\PowerForensics.dll
move .\bin\Release\netstandard1.6\PowerForensics.dll ..\..\Modules\PowerForensics\lib\coreclr
copy ..\..\Modules\PowerForensics\lib\coreclr\PowerForensics.dll ..\..\lib\coreclr
copy ..\..\Modules\PowerForensics\lib\PSv2\PowerForensics.dll ..\..\lib\PSv2
set CURRENTDIR=%cd%
powershell.exe -ExecutionPolicy Unrestricted -File "%CURRENTDIR%\..\..\xmldoc2md\xmldoc2md.ps1" -xml "%CURRENTDIR%\bin\Release\netstandard1.6\PowerForensics.xml" -xsl "%CURRENTDIR%\..\..\xmldoc2md\xmldoc2md.xsl" -docsdir "%CURRENTDIR%\..\..\docs" -helpdir "%CURRENTDIR%\..\..\Modules\PowerForensics\docs"