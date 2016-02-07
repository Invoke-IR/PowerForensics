# Not Done
Describe 'Get-ForensicContent' {  
    Context 'ByIndex ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicContent -VolumeName \\.\C: -Index 0 } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicContent -Index 0 } | Should Not Throw
        }
        It 'should work with the Encoding parameter' {
            
        }
        It 'should work with the TotalCount parameter' {
            (Get-ForensicContent -VolumeName \\.\C: -Index 0 -TotalCount 10).Length | Should Be 10
        }
        It 'should work with the Tail parameter' {
            (Get-ForensicContent -VolumeName \\.\C: -Index 0 -Tail 10).Length | Should Be 10
        }
        It 'should fail if the file does not exist' {
            { Get-ForensicContent -VolumeName \\.\C: -Index 1000000 } | Should Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicContent -VolumeName \\.\L: -Index 0 } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicContent -Path C:\Windows\System32\config\SAM } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicContent C:\Windows\System32\config\SAM } | Should Not Throw
        }
        It 'should work with the Encoding parameter' {
            
        }
        It 'should work with the TotalCount parameter' {
            (Get-ForensicContent -Path C:\Windows\System32\config\SAM -TotalCount 10).Length | Should Be 10
        }
        It 'should work with the Tail parameter' {
            (Get-ForensicContent -Path C:\Windows\System32\config\SAM -Tail 10).Length | Should Be 10
        }
        It 'should fail if the file does not exist' {
            { Get-ForensicContent -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}