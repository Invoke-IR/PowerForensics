Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-ChildItemRaw' {    
    It 'should work without -Path' {
        { Get-ChildItemRaw C:\ } | Should Not Throw
    }
    It 'should work with -Path' {
        { Get-ChildItemRaw -Path C:\ } | Should Not Throw
    }
    It 'should fail gracefully if the file does not exist' {
        { Get-ChildItemRaw -Path C:\windoxs } | Should Not Throw
    }
    It 'should work with listing system files' {
        (Get-ChildItemRaw -Path 'C:\$Volume').FullName | Should Be 'C:\$Volume'
    }
}