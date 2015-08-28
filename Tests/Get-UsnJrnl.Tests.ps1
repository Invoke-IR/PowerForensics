Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-UsnJrnl' {    
    Context 'get all UsnJrnl entries' { 
        It 'should work with -VolumeName' {
            { $usn = Get-UsnJrnl -VolumeName C } | Should Not Throw
            [GC]::Collect()
        }
        It 'should work without -VolumeName' {
            { $usn = Get-UsnJrnl } | Should Not Throw
            [GC]::Collect()
        }
    }
}