Describe 'Get-ForensicGuidPartitionTable' {    
    It 'should work with explicit parameters' {
        { Get-ForensicGuidPartitionTable -Path \\.\PHYSICALDRIVE2 } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicGuidPartitionTable \\.\PHYSICALDRIVE2 } | Should Not Throw
    }
    It 'should return a byte array from the -AsBytes parameter' {
    
    }
    It 'should throw if the drive is not Guid Partition Table format' {
        { Get-ForensicGuidPartitionTable -Path \\.\PHYSICALDRIVE0 } | Should Throw
    }
}