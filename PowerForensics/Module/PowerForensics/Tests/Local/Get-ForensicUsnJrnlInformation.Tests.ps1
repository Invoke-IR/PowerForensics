Describe 'Get-ForensicUsnJrnlInformation' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            { Get-ForensicUsnJrnlInformation -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicUsnJrnlInformation \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicUsnJrnlInformation } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicUsnJrnlInformation -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicUsnJrnlInformation -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        } 
        It 'should fail when the file is not $UsnJrnl' {
            { Get-ForensicUsnJrnlInformation -Path C:\Windows\notepad.exe } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicUsnJrnlInformation -Path C:\this\file\does\not\exist.txt } | Should Throw
        }  
    }
}