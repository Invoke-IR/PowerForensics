Describe 'Get-ForensicRegistryKey' {    
    Context 'ByKey ParameterSet' {
        It 'should work with from Hive Root' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SOFTWARE } | Should Not Throw
        }
        It 'should work with the Key parameter specified' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SYSTEM -Key ControlSet001 } | Should Not Throw
        }
        It 'should fail when the file is not a registry hive' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the specified Key does not exist' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SAM -Key keydoesnotexist } | Should Throw
        }

    }
    Context 'Recursive ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SAM -Recurse } | Should Not Throw
        }
        It 'should fail when the file is not a registry hive' {
            { Get-ForensicRegistryKey -HivePath C:\Windows\notepad.exe -Recurse } | Should Throw
        }
    }
}