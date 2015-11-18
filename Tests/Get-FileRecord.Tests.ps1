Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-FileRecord' {    
    Context 'should work without -VolumeName' { 
        It 'should not error' {
            { $mft = Get-ForensicFileRecord } | Should Not Throw
            [GC]::Collect()
        }
    }
    Context 'get the FileRecord for C:\$Volume' {
        It 'should work with -Path' {
            (Get-ForensicFileRecord -Path 'C:\$Volume').FullName | Should Be 'C:\$Volume'
        }
        It 'should work without -Path' {
            (Get-ForensicFileRecord -Path 'C:\$Volume').FullName | Should Be 'C:\$Volume'
        }
    }
    Context 'get the FileRecord for Index 5' {
        It 'should work with -Index' {
            (Get-ForensicFileRecord -Index 5).FullName | Should Be 'C:'
        }
        It 'should work without -Index' {
            (Get-ForensicFileRecord 5).FullName | Should Be 'C:'
        }
    }
}