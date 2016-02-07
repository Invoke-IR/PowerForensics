Describe 'Get-ForensicVolumeBootRecord' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicVolumeBootRecord -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicVolumeBootRecord \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicVolumeBootRecord } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicVolumeBootRecord -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicVolumeBootRecord -Path 'C:\$Boot' } | Should Not Throw
        }
        It 'should fail when the file is not $Boot' {
            { Get-ForensicVolumeBootRecord -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicVolumeBootRecord -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}