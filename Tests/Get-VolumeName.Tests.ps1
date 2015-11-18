Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

label C: testdrive

Describe 'Get-VolumeName' {    
    Context 'Get-VolumeName of C drive' { 
        It 'should work with -VolumeName' {
            (Get-ForensicVolumeName -VolumeName C).VolumeNameString | Should Be "testdrive"
        }
        It 'should work without -VolumeName' {
            (Get-ForensicVolumeName).VolumeNameString | Should Be "testdrive"
        }
    }
}