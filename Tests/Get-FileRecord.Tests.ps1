Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-FileRecord' {    
    Context 'should work without -VolumeName' { 
        It 'should not error' {
            { $mft = Get-FileRecord } | Should Not Throw
            [GC]::Collect()
        }
    }
    Context 'get the FileRecord for C:\$Volume' {
        It 'should work with -Path' {
            (Get-FileRecord -Path 'C:\$Volume').FullName | Should Be 'C:\Windows\notepad.exe'
        }
        It 'should work without -Path' {
            (Get-FileRecord -Path 'C:\$Volume').FullName | Should Be 'C:\Windows\notepad.exe'
        }
    }
    Context 'get the FileRecord for Index 5' {
        It 'should work with -Index' {
            (Get-FileRecord -Index 5).FullName | Should Be 'C:'
        }
        It 'should work without -Index' {
            (Get-FileRecord 5).FullName | Should Be 'C:'
        }
    }
}