Describe 'Get-ForensicScheduledJob' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicScheduledJob -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicScheduledJob \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicScheduledJob } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicScheduledJob -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            
        }
        It 'should fail when the file is not a ScheduledJob' {
            { Get-ForensicScheduledJob -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicScheduledJob -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}