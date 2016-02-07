Describe 'Get-ForensicFileRecordIndex' {    
    It 'should work with explicit parameters' {
        { Get-ForensicFileRecordIndex -Path C:\Windows\notepad.exe } | Should Not Throw
    }
    It 'should work with positional parameters' {
        { Get-ForensicFileRecordIndex C:\Windows\system32\cmd.exe } | Should Not Throw
    }
    It 'should work with directories' {
        (Get-ForensicFileRecordIndex -Path C:\) | Should be 5
    }
    It 'should work with system files' {
        { Get-ForensicFileRecordIndex -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
    }
    It 'should fail when the file does not exist' {
        { Get-ForensicFileRecordIndex -Path C:\this\file\does\not\exist.txt } | Should Throw
    }
}