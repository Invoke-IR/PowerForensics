Describe 'Get-ForensicRegistryValue' {    
    It 'should return one value if the Value parameter is used' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM -Key SAM -Value C } | Should Not Throw
    }
    It 'should return all values if the Value parameter is not used' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM -Key SAM } | Should Not Throw
    }
    It 'should fail if there are no values' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM } | Should Throw
    }
    It 'should fail if the value does not exist' {
        { Get-ForensicRegistryValue -HivePath C:\Windows\System32\config\SAM -Key SAM -Value Jared } | Should Throw
    }
}