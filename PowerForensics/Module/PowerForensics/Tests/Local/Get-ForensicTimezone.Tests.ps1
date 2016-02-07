Describe 'Get-ForensicTimezone' {    
    It 'should work with explicit parameters' {
        { Get-ForensicTimezone -HivePath C:\Windows\System32\config\SYSTEM } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicTimezone \\.\C: } | Should Not Throw
    }
    It 'should work with nonmandatory parameters' {
        { Get-ForensicTimezone } | Should Not Throw
    }
    It 'should fail if the hive is not a SYSTEM hive' {
        Write-Host 'Need to make the cmdlet check the header of the hive'
        { Get-ForensicTimezone -HivePath C:\Windows\System32\config\SAM } | Should Throw
    }
}