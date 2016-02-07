Describe 'Get-ForensicVolumeName' {    
    label.exe C: testdrive
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            (Get-ForensicVolumeName -VolumeName \\.\C:).VolumeNameString | Should Be 'testdrive'
        }
        It 'should work with positional parameters' {
            (Get-ForensicVolumeName \\.\C:).VolumeNameString | Should Be 'testdrive'
        }
        It 'should work without nonmandatory parameters' {
            (Get-ForensicVolumeName).VolumeNameString | Should Be 'testdrive'
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicVolumeName -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters'{
            (Get-ForensicVolumeName -Path 'C:\$Volume').VolumeNameString | Should Be 'testdrive'
        }
        It 'should fail when the file is not $Volume' {
            { Get-ForensicVolumeName -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicVolumeName -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}