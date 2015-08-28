Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-UsnJrnlInformation' {    
    Context 'Get-UsnJrnlInformation for the C drive' { 
        It 'should work with -VolumeName' {
            { Get-UsnJrnlInformation -VolumeName C } | Should Not Throw
        }
        It 'should work without -VolumeName' {
            { Get-UsnJrnlInformation } | Should Not Throw
        }
    }
}