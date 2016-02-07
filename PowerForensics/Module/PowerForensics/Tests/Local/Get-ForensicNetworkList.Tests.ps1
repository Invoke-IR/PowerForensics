Describe 'Get-ForensicNetworkList' {    
    It 'should work with explicit parameters' {
        { Get-ForensicNetworkList -HivePath C:\Windows\System32\config\SOFTWARE } | Should Not Throw 
    }
    It 'should work with positional parameters' {
        { Get-ForensicNetworkList \\.\C: } | Should Not Throw
    }
    It 'should work with nonmandatory parameters' {
        { Get-ForensicNetworkList } | Should Not Throw
    }
    It 'should fail if the hive is not a SOFTWARE hive' {
        { Get-ForensicNetworkList -HivePath C:\Windows\notepad.exe } | Should Throw
    }
}