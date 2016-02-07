Describe 'Get-ForensicTimeline' {    
    It 'should work with explicit parameters' {
        { $timeline = Get-ForensicTimeline -VolumeName \\.\C: } | Should Not Throw
        [GC]::Collect()
    }
    It 'should work with positional parameters' {
        { $timeline = Get-ForensicTimeline \\.\C: } | Should Not Throw
        [GC]::Collect()
    }
    It 'should work without nonmandatory parameters' {
        { $timeline = Get-ForensicTimeline } | Should Not Throw
        [GC]::Collect()
    }
    It 'should fail when the file does not exist' {
        { $timeline = Get-ForensicTimeline -VolumeName \\.\L: } | Should Throw
    }
}