Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-ScheduledJobRaw' {      
    Write-Host 'Need to work on this...'
    #Context 'Parse job files in the C:\windows\tasks directory' { 
    #    It 'should work with -VolumeName' {
    #        { $jobs = Get-ScheduledJobRaw -VolumeName C } | Should Not Throw
    #        [GC]::Collect()
    #    }
    #    It 'should work without -VolumeName' {
    #        { $jobs = Get-ScheduledJobRaw } | Should Not Throw
    #        [GC]::Collect()
    #    }
    #}
    $tasks = Get-ChildItemRaw C:\Windows\Tasks
    foreach($t in $tasks)
    {
        Write-Host $t.FullName
    }
}