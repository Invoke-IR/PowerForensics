Describe 'Get-ForensicMasterBootRecord' { 
    It 'should work with explicit parameters' {
        { Get-ForensicMasterBootRecord -Path \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicMasterBootRecord \\.\PHYSICALDRIVE0 } | Should Not Throw
    }
    It 'should return a byte array from the -AsBytes parameter' {
        $mbr = Get-ForensicMasterBootRecord -Path \\.\PHYSICALDRIVE0 -AsBytes
        $mbr.Length | Should Be 512
        $mbr.GetType().Name | Should Be 'Byte[]'
    }
    It 'should return the protective MBR if the drive is Guid Partition Table format' {
    
    }
}