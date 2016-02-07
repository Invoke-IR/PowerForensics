Describe 'Get-ForensicPrefetch' {      
    if(Test-Path C:\Windows\Prefetch){
        Context 'ByVolume ParameterSet' { 
            It 'should work with explicit parameters' {
                { Get-ForensicPrefetch -VolumeName \\.\C: } | Should Not Throw
            }
            It 'should work with positional parameters' {
                { Get-ForensicPrefetch \\.\C: } | Should Not Throw
            }
            It 'should work without nonmandatory parameters' {
                { Get-ForensicPrefetch } | Should Not Throw
            }
            It 'should fail when the volume does not exist' {
                { Get-ForensicPrefetch -VolumeName \\.\L: } | Should Throw
            }
            It 'should work with the Fast parameter' {
                { Get-ForensicPrefetch -Fast } | Should Not Throw
            }
        }
        Context 'ByPath ParameterSet' {
            It 'should work with explicit parameters' {
                Write-Host 'Need to write something to detect the name of a prefetch file'
            }
            It 'should fail when the file does not exist' {
                { Get-ForensicPrefetch -Path C:\this\file\does\not\exist.txt } | Should Throw
            }
        }
    }
    else{
        Write-Host 'Prefetch is not enabled'
    }
}