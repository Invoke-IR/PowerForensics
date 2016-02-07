Describe 'Get-ForensicShellLink' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicShellLink -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicShellLink \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicShellLink } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicShellLink -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            # Need to have a path to a ShellLink file here        
        }
        It 'should fail when the file is not a ShellLink' {
            { Get-ForensicShellLink -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicShellLink -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}