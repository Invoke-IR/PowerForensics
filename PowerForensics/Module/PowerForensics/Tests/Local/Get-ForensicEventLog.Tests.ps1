Describe 'Get-ForensicEventLog' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicEventLog -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicEventLog \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicEventLog } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicEventLog -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicEventLog -Path C:\Windows\System32\winevt\Logs\Application.evtx } | Should Not Throw
        }
        It 'should fail when the file is not an EventLog' {
            { Get-ForensicEventLog -Path C:\Windows\system32\cmd.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicEventLog -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}