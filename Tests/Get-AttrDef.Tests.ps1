Import-Module -Force $PSScriptRoot\..\PowerForensics.psd1

Describe 'Get-AttrDef' {    
    Context 'Get the attribute definition for the C drive' { 
        It 'should work with -VolumeName' {
            { Get-ForensicAttrDef -VolumeName C } | Should Not Throw
        }
        It 'should work with -VolumeName' {
            { Get-ForensicAttrDef } | Should Not Throw
        }
    }
}