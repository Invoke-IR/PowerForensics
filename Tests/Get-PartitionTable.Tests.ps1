Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-PartitionTable' {    
    Context 'Get-PartitionTable for \\.\PHYSICALDRIVE0' { 
        It 'should work with -Path' {
            { Get-PartitionTable -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
        }
    }
}