Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-VolumeBootRecord' {    
    
    Context 'Get-VolumeBootRecord for the C drive' { 

        It 'should work with -VolumeName' {
            { Get-VolumeBootRecord -VolumeName C } | Should Not Throw
        }
        It 'should work without -VolumeName' {
            { Get-VolumeBootRecord } | Should Not Throw
        }
    }
}