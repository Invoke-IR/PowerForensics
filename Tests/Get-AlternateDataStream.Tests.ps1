Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-AlternateDataStream' {    
    Context 'Get all ADS from every file' {
        It 'should work with -VolumeName parameter' {
            { Get-ForensicAlternateDataStream -VolumeName C } | Should Not Throw
        }
        It 'should fail with positional -VolumeName parameter' {
            { Get-ForensicAlternateDataStream C } | Should Throw
        }
        It 'should work without -VolumeName parameter' {
            { Get-ForensicAlternateDataStream } | Should Not Throw
        }
    }
    Context 'Get ADS from C:\$Extend\$UsnJrnl' {
        It 'should work with -Path parameter' {
            { Get-ForensicAlternateDataStream -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        }
        It 'should fail without -Path parameter' {
            { Get-ForensicAlternateDataStream 'C:\$Extend\$UsnJrnl' } | Should Throw
        }
        It 'should return two ADS named $J and $Max' {
            $ads = Get-ForensicAlternateDataStream -Path 'C:\$Extend\$UsnJrnl'
            $ads.Length | Should Be 2
            $ads[0].StreamName | Should Be '$J'
            $ads[1].StreamName | Should Be '$Max'
        }
    }
}