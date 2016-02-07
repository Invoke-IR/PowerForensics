Describe 'Get-ForensicUsnJrnl' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { $usn = Get-ForensicUsnJrnl -VolumeName \\.\C: } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with positional parameters' {
            { $usn = Get-ForensicUsnJrnl \\.\C: } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work without nonmandatory parameters' {
            { $usn = Get-ForensicUsnJrnl } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with the Usn parameter' {
            
        }
        It 'should return just one entry with the Usn parameter' {
            
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicUsnJrnl -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { $usn = Get-ForensicUsnJrnl -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work with the Usn parameter' {
        
        }
        It 'should return just one entry with the Usn parameter' {
        
        }
        It 'should fail when the file is not $UsnJrnl' {
            { Get-ForensicUsnJrnl -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicUsnJrnl -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}