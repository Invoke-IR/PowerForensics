Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

if(Test-Path C:\Windows\Prefetch){
    Describe 'Get-Prefetch' {      
        Context 'Get-Prefetch from the C:\windows\prefetch directory' { 
            It 'should work without any parameters' {
                { $pf = Get-ForensicPrefetch } | Should Not Throw
                [GC]::Collect()
            }
            It 'should work with -Fast' {
                { $pf = Get-ForensicPrefetch -Fast } | Should Not Throw
                [GC]::Collect()
            }
        }
    }
}