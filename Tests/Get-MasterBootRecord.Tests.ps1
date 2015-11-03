Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-MasterBootRecord' { 
    Context 'Path is provided' { 
        It 'should not error' {
            { Get-MasterBootRecord -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
        }
    }
}