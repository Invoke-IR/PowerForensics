Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-MBR' {    
    
    Context 'Get-MBR for \\.\PHYSICALDRIVE0' { 

        It 'should not error' {
            { Get-MBR -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
        }
    }
}