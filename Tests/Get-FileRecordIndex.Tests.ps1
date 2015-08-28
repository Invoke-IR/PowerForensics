Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-FileRecordIndex' {    
    
    Context 'Get record index for C:\' { 
        It 'should be 5' {
            (Get-FileRecordIndex C:\) | Should be 5
        }
    }
    Context 'Get record index for $AttrDef' { 
        It 'should be 4' {
            (Get-FileRecordIndex 'C:\$AttrDef') | Should be 4
        }
    }
    Context 'Get record index for $UsnJrnl' { 
        It 'should work' {
            { Get-FileRecordIndex 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        }
    }
    Context 'Get record index for notepad.exe' { 
        It 'should work' {
            { Get-FileRecordIndex 'C:\windows\notepad.exe' } | Should Not Throw
        }
    }
}