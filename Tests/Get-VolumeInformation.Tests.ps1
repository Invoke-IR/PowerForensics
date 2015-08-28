Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-VolumeInformation' {    
    Context 'Get-VolumeInformation of C drive' { 
        It 'should work with -VolumeName' {
            (Get-VolumeInformation -VolumeName C).Name | Should Be 'VOLUME_INFORMATION'
        }
        It 'should work without -VolumeName' {
            (Get-VolumeInformation).Name | Should Be 'VOLUME_INFORMATION'
        }
    }
}