Describe 'Get-ForensicFileSlack' {    
    Context 'ByIndex ParameterSet' {
        It 'should work with explicit parameters' {
            #{ Get-ForensicFileSlack -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with the Index Parameter' {
            { Get-ForensicFileSlack -VolumeName \\.\C: -Index 0 } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicFileSlack 0 } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            #{ Get-ForensicFileSlack } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicFileSlack -VolumeName \\.\L: -Index 0 } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicFileSlack -Path C:\Windows\notepad.exe } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicFileSlack C:\Windows\notepad.exe } | Should Not Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicFileSlack -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
}