Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-BootSector' {    
    
    Context 'Get-BootSector for \\.\PHYSICALDRIVE0' { 

        It 'should not error' {
            { Get-ForensicBootSector -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
        }
    }
}