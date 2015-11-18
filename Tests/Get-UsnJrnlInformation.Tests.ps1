Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-UsnJrnlInformation' {    
    Context 'Get-UsnJrnlInformation for the C drive' { 
        It 'should work with -VolumeName' {
            { Get-ForensicUsnJrnlInformation -VolumeName C } | Should Not Throw
        }
        It 'should work without -VolumeName' {
            { Get-ForensicUsnJrnlInformation } | Should Not Throw
        }
    }
}