Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-ChildItemRaw' {    
    It 'should work without -Path' {
        { Get-ForensicChildItemRaw C:\ } | Should Not Throw
    }
    It 'should work with -Path' {
        { Get-ForensicChildItemRaw -Path C:\ } | Should Not Throw
    }
    It 'should fail gracefully if the file does not exist' {
        { Get-ForensicChildItemRaw -Path C:\windoxs } | Should Not Throw
    }
    It 'should work with listing system files' {
        (Get-ForensicChildItemRaw -Path 'C:\$Volume').FullName | Should Be '$Volume'
    }
}