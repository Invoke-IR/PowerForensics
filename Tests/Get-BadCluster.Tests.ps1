Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-BadCluster' {    
    Context 'Test the C drive for Bad Clusters' { 
        It 'should work with -VolumeName' {
            { Get-BadCluster -VolumeName C } | Should Not Throw
        }
        It 'should work without -VolumeName' {
            { Get-BadCluster } | Should Not Throw
        }
    }
}