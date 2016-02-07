Describe 'Copy-ForensicFile' {    
    Context 'ByPath ParameterSet' { 
        It 'should work with explicit parameters' {
            { Copy-ForensicFile -Path C:\Windows\System32\config\SOFTWARE -Destination C:\Windows\Temp\SOFTWAREcopy } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Copy-ForensicFile C:\Windows\System32\config\SAM C:\Windows\Temp\SAMcopy } | Should Not Throw
        }
        It 'should fail if file does not exist' {
        
        }
        It 'should fail if destination file already exists' {
        
        }
    }
    Context 'ByIndex ParameterSet' { 
        It 'should work with explicit parameters' {
            { Copy-ForensicFile -VolumeName \\.\C: -Index 7 -Destination C:\Windows\Temp\BOOT } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Copy-ForensicFile -Index 4 -Destination C:\Windows\Temp\ATTRDEF } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Copy-ForensicFile 0 C:\Windows\Temp\MFT } | Should Not Throw
        }
        It 'should fail if index is not valid' {
        
        }
        It 'should fail if destination file already exists' {
        
        }
    }
}