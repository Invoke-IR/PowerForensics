# Took forever on AppVeyor
Describe 'Get-ForensicAlternateDataStream' {    
    <#Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAlternateDataStream -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicAlternateDataStream \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicAlternateDataStream } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicAlternateDataStream -VolumeName \\.\L: } | Should Throw
        }
    }#>
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAlternateDataStream -Path 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        }
        It 'should return two ADS named $J and $Max' {
            $ads = Get-ForensicAlternateDataStream -Path 'C:\$Extend\$UsnJrnl'
            $ads.Length | Should Be 2
            $ads[0].StreamName | Should Be '$J'
            $ads[1].StreamName | Should Be '$Max'
        }
        It 'should return nothing if file has no ADS' {
            $foo = Get-ForensicAlternateDataStream -Path C:\Windows\notepad.exe
            $foo | Should Be $null
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicAlternateDataStream -Path C:\This\file\should\not\exist.txt } | Should Throw
        }
    }
}