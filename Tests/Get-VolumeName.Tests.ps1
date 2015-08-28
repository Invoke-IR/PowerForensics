Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

label C: testdrive

Describe 'Get-VolumeName' {    
    Context 'Get-VolumeName of C drive' { 
        It 'should work with -VolumeName' {
            (Get-VolumeName -VolumeName C).VolumeNameString | Should Be "testdrive"
        }
        It 'should work without -VolumeName' {
            (Get-VolumeName).VolumeNameString | Should Be "testdrive"
        }
    }
}