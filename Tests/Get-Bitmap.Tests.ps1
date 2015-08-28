Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-Bitmap' {    
    Context 'Test the C drive for Bad Clusters' { 
        #It 'should work with -VolumeName' {
        #    (Get-Bitmap -VolumeName C -Cluster 0) | Should Be 0
        #}
        It 'should work without -VolumeName' {
            (Get-Bitmap -Cluster 0).Cluster | Should Be 0
        }
    }
}