Describe 'Get-ForensicUserAssist' {    
    It 'should work with explicit parameters' {
        { Get-ForensicUserAssist -HivePath C:\Users\tester\NTUSER.DAT } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicUserAssist \\.\C: } | Should Not Throw
    }
    It 'should fail if the hive is not a NTUSER.DAT hive' {
        Write-Host 'Need to make the cmdlet check the header of the hive'
        { Get-ForensicUserAssist -HivePath C:\Windows\System32\config\SAM } | Should Throw
    }
}