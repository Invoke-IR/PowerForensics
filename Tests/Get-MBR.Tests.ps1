Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-MBR' { 
    Context 'Path is provided' { 
        It 'should not error' {
            { Get-MBR -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
        }
    }
}