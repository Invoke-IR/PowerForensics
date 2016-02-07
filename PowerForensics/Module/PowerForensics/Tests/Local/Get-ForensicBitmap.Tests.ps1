Describe 'Get-ForensicBitmap' {    
    Context 'ByVolume ParameterSet' { 
        It 'should work with explicit parameters' {
            (Get-ForensicBitmap -VolumeName \\.\C: -Cluster 0).InUse | Should Be $true
        }
        It 'should work with positional parameters' {
            (Get-ForensicBitmap \\.\C: -Cluster 0).InUse | Should Be $true
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicBitmap -Cluster 0 } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicBitmap -VolumeName \\.\L: -Cluster 0 } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicBitmap -Path 'C:\$Bitmap' -Cluster 0 } | Should Not Throw
        }
        # I dont think there is a way to ensure that the file is truly a Bitmap file
        It 'should fail if the file does not exist' {
            { Get-ForensicBitmap -Path C:\this\file\should\not\exist.txt -Cluster 0 } | Should Throw
        }
    }
}