Describe 'ConvertTo-ForensicTimeline' {
    It 'should work with explicit parameters' {
        { ConvertTo-ForensicTimeline -InputObject (Get-ForensicFileRecord -Index 0) } | Should Not Throw
    }
    It 'should work through the pipeline' {
        { Get-ForensicFileRecord -Index 0 | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    <#It 'should work with Prefetch objects' {
        { Get-ForensicPrefetch -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }#>
    It 'should work with ScheduledJob objects' {
        { Get-ForensicScheduledJob -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with ShellLink objects' {
        { Get-ForensicShellLink -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with UserAssist objects' {
        { Get-ForensicUserAssist -HivePath C:\Users\tester\NTUSER.DAT | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with EventRecord objects' {
        { Get-ForensicEventLog -Path C:\Windows\System32\winevt\Logs\Application.evtx | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with FileRecord objects' {
        { Get-ForensicFileRecord -Index 0 | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with UsnJrnl objects' {
        { Get-ForensicUsnJrnl -VolumeName \\.\C: | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should work with NamedKey objects' {
        { Get-ForensicRegistryKey -HivePath C:\Windows\System32\config\SAM | ConvertTo-ForensicTimeline } | Should Not Throw
    }
    It 'should fail for unsupported types' {
        { Get-Process -Name lsass | ConvertTo-ForensicTimeline } | Should Throw
    }

}