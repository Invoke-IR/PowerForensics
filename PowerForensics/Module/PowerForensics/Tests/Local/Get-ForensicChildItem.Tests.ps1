Describe 'Get-ForensicChildItem' {    
    It 'should work with explicit parameters' {
        { Get-ForensicChildItem -Path C:\ } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicChildItem C:\ } | Should Not Throw
    }
    It 'should work without nonmandatory parameters' {
        Set-Location \
        { Get-ForensicChildItem } | Should Not Throw
    }
    It 'should fail gracefully if the file does not exist' {
        { Get-ForensicChildItem -Path C:\windoxs } | Should Not Throw
    }
    It 'should work with listing system files' {
        (Get-ForensicChildItem -Path 'C:\$Volume').FullName | Should Be '$Volume'
    }
}