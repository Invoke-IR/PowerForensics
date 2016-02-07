Describe 'Get-ForensicVolumeInformation' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            (Get-ForensicVolumeInformation -VolumeName \\.\C:).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should work with positional parameters' {
            (Get-ForensicVolumeInformation \\.\C:).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should work without nonmandatory parameters' {
            (Get-ForensicVolumeInformation).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicVolumeInformation -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            (Get-ForensicVolumeInformation -Path 'C:\$Volume').Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should fail when the file is not $Volume' {
            { Get-ForensicVolumeInformation -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicVolumeInformation -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}