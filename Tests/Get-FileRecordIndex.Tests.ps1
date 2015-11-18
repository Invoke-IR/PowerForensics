Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-FileRecordIndex' {    
    
    Context 'Get record index for C:\' { 
        It 'should be 5' {
            (Get-ForensicFileRecordIndex C:\) | Should be 5
        }
    }
    Context 'Get record index for $AttrDef' { 
        It 'should be 4' {
            (Get-ForensicFileRecordIndex 'C:\$AttrDef') | Should be 4
        }
    }
    Context 'Get record index for $UsnJrnl' { 
        It 'should work' {
            { Get-ForensicFileRecordIndex 'C:\$Extend\$UsnJrnl' } | Should Not Throw
        }
    }
}