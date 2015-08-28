Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Copy-FileRaw' {    
    Context 'Copy C:\windows\system32\config\SAM with Copy-Item' { 
        It 'should error' {
            { Copy-Item C:\Windows\System32\config\SAM C:\Windows\Temp\SAMcopy } | Should Throw
        }
    }
    Context 'Copy C:\windows\system32\config\SAM with Copy-FileRaw' { 
        It 'should work' {
            { Copy-FileRaw C:\Windows\System32\config\SAM C:\Windows\Temp\SAMcopy } | Should Not Throw
        }
    }
}