Describe 'Get-ForensicSid' {    
    It 'should work with explicit parameters' {
        { Get-ForensicSid -HivePath C:\Windows\System32\config\SAM } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicSid \\.\C: } | Should Not Throw
    }
    It 'should work with nonmandatory parameters' {
        { Get-ForensicSid } | Should Not Throw
    }
    It 'should fail if the hive is not a SAM hive' {
        Write-Host 'Need to make the cmdlet check the header of the hive'
        { Get-ForensicSid -HivePath C:\Windows\System32\config\SOFTWARE } | Should Throw
    }
}