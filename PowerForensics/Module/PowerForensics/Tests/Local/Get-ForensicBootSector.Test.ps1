# Not Done
Describe 'Get-ForensicBootSector' {    
    It 'should work with explicit parameters' {
        { Get-ForensicBootSector -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicBootSector \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with the -AsBytes parameter' {
        (Get-ForensicBootSector \\.\PHYSICALDRIVE0 -AsBytes).GetType().Name | Should Be 'Byte[]'
    }
    It 'should parse a Master Boot Record formatted drive' {
    
    }
    It 'should parse a Guid Partition Table formatted drive' {
    
    }
}