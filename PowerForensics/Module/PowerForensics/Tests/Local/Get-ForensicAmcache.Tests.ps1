Describe 'Get-ForensicAmcache' {    
    Context 'ByVolume ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAmcache -VolumeName \\.\C: } | Should Not Throw
        }
        It 'should work with positional parameters' {
            { Get-ForensicAmcache \\.\C: } | Should Not Throw
        }
        It 'should work without nonmandatory parameters' {
            { Get-ForensicAmcache } | Should Not Throw
        }
        It 'should fail when the volume does not exist' {
            { Get-ForensicAmcache -VolumeName \\.\L: } | Should Throw
        }
    }
    Context 'ByPath ParameterSet' {
        It 'should work with explicit parameters' {
            { Get-ForensicAmcache -HivePath C:\Windows\appcompat\Programs\Amcache.hve } | Should Not Throw
        }
        It 'should fail when the file is not an Amcache.hve' {
            { Get-ForensicAmcache -HivePath C:\Windows\System32\config\SAM } | Should Throw
        }
        It 'should fail when the file does not exist' {
            { Get-ForensicAmcache -HivePath C:\crazydirectory\thisshouldnotexist.hve } | Should Throw
        }
    }
}