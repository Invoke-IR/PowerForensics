Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

if(Test-Path -Path C:\Windows\Tasks -PathType Container)
{
    $files = Get-ChildItem -Path C:\Windows\Tasks

    Write-Host $files.Length

    Describe 'Get-ScheduledJobRaw' {
        Context 'Parse all job files in the C:\windows\tasks directory' { 
            It 'should work with -VolumeName' {
                { $jobs = Get-ScheduledJobRaw -VolumeName C } | Should Not Throw
                [GC]::Collect()
            }
            It 'should work without -VolumeName' {
                { $jobs = Get-ScheduledJobRaw } | Should Not Throw
                [GC]::Collect()
            }
        }
        Context 'Parse a single job file based on Path' {
            It 'should work with -Path' {
                { $jobs = Get-ScheduledJobRaw -Path $files[0].FullName } | Should Not Throw
            }
            It 'should fail without -Path' {
                { $jobs = Get-ScheduledJobRaw $files[0].FullName } | Should Throw
            }
        }
    }
}