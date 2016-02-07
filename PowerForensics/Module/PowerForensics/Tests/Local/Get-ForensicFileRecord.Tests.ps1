Describe 'Get-ForensicFileRecord' {    
    Context 'ByIndex ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicFileRecord -VolumeName \\.\C: -Index 5 } | Should Not Throw
        }
        It 'should work with positional "Index" parameter' {
            { Get-ForensicFileRecord -VolumeName \\.\C: 5 }
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicFileRecord -Index 5} | Should Not Throw
        }
        It 'should work without nonmandatory and positional parameters' {
            { Get-ForensicFileRecord 5 } | Should Not Throw
        }
        It 'should return 1 File Record' {
            $r = Get-ForensicFileRecord 5
            $r.Length | Should Be 1
        }
        It 'should return all File Records' {
            { $mft = Get-ForensicFileRecord } | Should Not Throw
            [GC]::Collect()
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            (Get-ForensicFileRecord -Path 'C:\$Volume').FullName | Should Be 'C:\$Volume'
        }
        It 'should work with positional parameters' {
            (Get-ForensicFileRecord 'C:\$Volume').FullName | Should Be 'C:\$Volume'
        }
        It 'should work fail if the file does not exist' {
            { Get-ForensicFileRecord -Path C:\this\file\does\not\exist.txt } | Should Throw
        }
    }
    Context 'ByMftPath ParameterSet' {
        It 'should work with explicit parameters' {
            #{ Get-ForensicFileRecord -MftPath 'C:\$MFT' } | Should Not Throw
        }
        It 'should fail if not a $MFT file' {
            #{ Get-ForensicFileRecord -MftPath C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail if the file does not exist' {
            #{ Get-ForensicFileRecord -MftPath C:\this\file\does\not\exist.txt } | Should Throw 
        }
    }
}