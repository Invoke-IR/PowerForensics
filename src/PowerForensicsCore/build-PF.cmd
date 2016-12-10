del ..\..\Modules\PowerForensics\coreclr\PowerForensics.dll
move .\bin\Release\netstandard1.6\PowerForensics.dll ..\..\Modules\PowerForensics\lib\coreclr
copy ..\..\Modules\PowerForensics\lib\coreclr\PowerForensics.dll ..\..\lib\coreclr
copy ..\..\Modules\PowerForensics\lib\PSv2\PowerForensics.dll ..\..\lib\PSv2
set CURRENTDIR=%cd%
powershell.exe -ExecutionPolicy Unrestricted -File "%CURRENTDIR%\..\..\build\build.ps1" -ProjectPath "%CURRENTDIR%\..\.."